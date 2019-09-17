using Be.Windows.Forms;
using Humanizer;
using Humanizer.Bytes;
using Newtonsoft.Json.Linq;
using PacketStudio.Controls;
using PacketStudio.Controls.PacketsDef;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.Properties;
using PacketStudio.Utils;
using Syncfusion.Drawing;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PacketStudio.DataAccess.Json;
using PacketStudio.DataAccess.SaveData;
using Action = System.Action;
// ReSharper disable LocalizableElement
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace PacketStudio
{
    public partial class MainForm : RibbonForm
    {
#pragma warning disable CRRSP01 // A misspelled word has been found
        private const string PDML_SHOWNAME_ATTR = "showname";
#pragma warning restore CRRSP01 // A misspelled word has been found

        enum StatusType
        {
            Neutral = 0,
            Good = 1,
            Warning = 3,
            Bad = 2,
        }

        // InterOps
        private string _longStatus = "";
        private WiresharkInterop _wiresharkField;
        private TempPacketsSaver _packetSaver = new TempPacketsSaver();
        private TSharkInterop _tshark;

        // Tshark Heuristic Dissectos
        private Task<List<TSharkHeuristicProtocolEntry>> _heurDissectorsTask;
        private List<TSharkHeuristicProtocolEntry> heurDissectorsList => _heurDissectorsTask.Result;

        private WiresharkInterop _wireshark
        {
            get => _wiresharkField;
            set
            {
                WiresharkInteropUpdating(_wiresharkField, value);
                _wiresharkField = value;
            }
        }

        private CapInfosInterop _capinfos;

        // Unsaved changes members
        private bool _unsavedChangesExist;
        private bool _askAboutUnsaved;

        // Intercept key press in tree view (for shift-right expanding)
        private bool _interceptingKeyDown;

        // Whether we are in the constructor
        private readonly bool _isConstructing;

        // The previous selected tab
        private TabPageAdv _lastSelectedPage;
        // Reference to the '+' Tab
        private TabPageAdv _plusTab;

        // Live Preview members
        bool _livePreviewChecked = true;
        bool _livePreviewInContext = true;
        // Preview delay
        int _delayMs;
        // Preview timer
        private readonly System.Threading.Timer _livePrevTimer;
        // PacketList Preview timer
        private readonly System.Threading.Timer _packetListTimer;

        // Number for the next created packet tab
        private int _nextPacketTabNumber = 1;

        // Are we in the middle of loading a capture file
        private bool _isLoadingFile;

        // Are we in the middle of updating packets list
        private bool _isUpdatingList;

        // Original form name
        private string _rawFormName;

        private Rectangle _rectangle = Rectangle.Empty;

        private TabPageAdv _tabRequestingRename;

        // Mapping the tab pages and their Packet Define Controls
        private Dictionary<TabPageAdv, PacketDefineControl> _tabToPdc = new Dictionary<TabPageAdv, PacketDefineControl>();

        private CancellationTokenSource _livePrevTokenSource;
        private CancellationTokenSource _packetListTokenSource;

        // Colors for the status panel
        private BrushInfoColorArrayList _badGradient = new BrushInfoColorArrayList(new[]
        {
            Color.FromArgb(0xEA,0x54,0x55),
            Color.FromArgb(0xFE,0xB6,0x92)
        });
        private BrushInfoColorArrayList _goodGradient = new BrushInfoColorArrayList(new[]
        {
            Color.FromArgb(0x28,0xC7,0x6F),
            Color.FromArgb(0x81,0xFB,0xB8)
        });
        private BrushInfoColorArrayList _warnGradient = new BrushInfoColorArrayList(new[]
        {
            Color.FromArgb(241,194,107),
            Color.FromArgb(251,245,149)
        });

        // Colors for the status bar
        private Color _neutralStatusColor;
        private Color _badStatusColor = Color.FromArgb(0xEA, 0x54, 0x55);
        private Color _goodStatusColor = Color.FromArgb(0x28, 0xC7, 0x6F);
        private Color _warnStatusColor = Color.FromArgb(241, 194, 107);

        private static readonly Color ERROR_PINK = Color.FromArgb(255, 92, 92); // Wireshark's error background color

        public MainForm()
        {
            _livePrevTokenSource = new CancellationTokenSource();
            _packetListTokenSource = new CancellationTokenSource();

            _livePrevTimer = new System.Threading.Timer(UpdateLivePreview);
            _packetListTimer = new System.Threading.Timer(state => UpdatePacketListBox(_packetListTokenSource.Token));

            InitializeComponent();
            wsVerPanel.BackColor = Color.Transparent;
            statusTextPanel.BackColor = Color.Transparent;

            // Save default window title ( so we can append file names to it)
            _rawFormName = Text;
            _neutralStatusColor = statusBar.MetroColor;

            // Removing initial, unaligned, "packet 1" tab.
            tabControl.TabPages.Remove(tabControl.SelectedTab);
            // Adding new , aligned, "packet 1" tab
            TabControl_SelectedIndexChanged(null, null);

            // Load settings
            _askAboutUnsaved = Settings.Default.lastAskToSaveSetting;
            previewContextToolStripButton.Checked = Settings.Default.livePreviewInContext;
            packetListPreviewToolStripButton.Checked = Settings.Default.livePreviewPacketList;
            previewtoolStripButton.Checked = Settings.Default.livePreviewActive;
            _delayMs = Settings.Default.livePreviewDelayMs;
            livePrevToolStripTextBox.Text = _delayMs.ToString();


            _lastUserEnabledHeuristics =
                HackyBsae64StringListSerializer.Deserialize(Settings.Default.EnabledHeuristicDissectors);
            _lastUserDisabledHeuristics =
                HackyBsae64StringListSerializer.Deserialize(Settings.Default.DisabledHeuristicDissectors);
            _needToBeEnabledHeuristics =
                HackyBsae64StringListSerializer.Deserialize(Settings.Default.NeedToBeEnabledDissectors);
            _needToBeDisabledHeuristics =
                HackyBsae64StringListSerializer.Deserialize(Settings.Default.NeedToBeDisabledDissectors);


            packetTreeView.DrawNode += DrawTreeNodeLikeWireshark;

            // TODO: This might throw...
            string wsPath = Path.GetDirectoryName(Settings.Default.lastWiresharkPath);
            WiresharkDirectory dir;
            // Try to find the wireshark by the path saved in the settings
            if (SharksFinder.TryGetByPath(wsPath, out dir))
            {
                _wireshark = new WiresharkInterop(dir.WiresharkPath);
                _tshark = new TSharkInterop(dir.TsharkPath);
                _capinfos = new CapInfosInterop(dir.CapinfosPath);
            }
            else
            {
                // Fallback - get the default WS path found in the system
                dir = SharksFinder.GetDirectories().FirstOrDefault();
                if (dir != null)
                {
                    _wireshark = new WiresharkInterop(dir.WiresharkPath);
                    _tshark = new TSharkInterop(dir.TsharkPath);
                    _capinfos = new CapInfosInterop(dir.CapinfosPath);
                }
                else
                {
                    ShowErrorMessageBox("Wireshark path not saved in the settings and couldn't be found in any of the default paths.\r\n\r\n" +
                        "Please select the location in the next dialog", MessageBoxIcon.Exclamation);
                    _isConstructing = true;
                    LocateWireshark_Click(null, null);
                    _isConstructing = false;
                }
            }

            // Now we should have TShark, start getting heuristic dissector in the background
            _heurDissectorsTask = _tshark.GetHeurDissectors();

            _unsavedChangesExist = false;


            // Allow dropping in the background of the form
            // Also recursively register drag-drop events for all controls under the main form, such as the main tab control
            WireDragDrop(new Control[] { this });
            // Allow dropping in the dock manager's panels (i.e. hex view panel, tree view panel)
            WireDragDrop(this.dockingManager.ControlsArray);

        }

        private TabPageAdv AddNewTab(PacketSaveData saveData)
        {
            TabPageAdv newPage = new TabPageAdv("Packet " + (_nextPacketTabNumber));
            newPage.TabForeColor = Color.White;
            newPage.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Rename",TabPage_onRenameRequested)
            });

            _nextPacketTabNumber++;
            PacketDefineControl pdc = new PacketDefineControl(saveData);
            pdc.ContentChanged += Pdc_ContentChanged;
            pdc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            pdc.Dock = DockStyle.Fill;

            // Create mapping between tab and pdc
            _tabToPdc.Add(newPage, pdc);

            // Add tab to tab control
            tabControl.TabPages.Add(newPage);

            if (!_isLoadingFile)
            {
                // If we aren't in the middle of loading many packets (hence this packet was created with the + tab)
                // Update packet list to include new tab
                QueuePacketListUpdate();
            }

            return newPage;
        }

        private void ContinueTsharkTask(Task<TSharkCombinedResults> tsharkTask, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return;
            this.Invoke((Action)(() =>
           {
               bool suspiciousFlag = false;
               int lastIndex = 0;

               if (tsharkTask.IsCanceled) // task was canceled
                   return;


               if (tsharkTask.IsFaulted)
               {
                   if (token.IsCancellationRequested)
                   {
                       // We dont care because the task was cancelled.
                       return;
                   }

                   // Show the exit code + errors
                   UpdateStatus(tsharkTask.Exception?.Flatten().Message ?? "Unknown errors", StatusType.Bad);

                   return;
               }

               if (tsharkTask.Result.TotalFailure)
               {
                   if (token.IsCancellationRequested)
                   {
                       // We dont care because the task was cancelled.
                       return;
                   }

                   // Show the exit code + errors
                   UpdateStatus("PDML retrival exception:" + tsharkTask.Result.PdmlException + "\r\n\r\n" +
                                "JSON retrival exception:" + tsharkTask.Result.JsonException, StatusType.Bad);

                   return;
               }

               if (token.IsCancellationRequested)
               {
                   // We got cancelled while checking the task results...
                   return;
               }

               UpdateStatus("OK", StatusType.Good);

               // Get the parsed TShark's response, base of proto tree
               TSharkCombinedResults results = tsharkTask.Result;
               XElement packetElement = results.Pdml;
               JObject packetJson = results.JsonRaw;
               IEnumerable<JProperty> jProps = packetJson?.DescendantsAndSelf()
                                                          .Where(jtoken => jtoken is JProperty)
                                                          .Cast<JProperty>()
                                               ?? Enumerable.Empty<JProperty>();
               List<JProperty> jPropsListed = null;

               TreeView tree = packetTreeView;

               // Get all CURRENTLY expanded nodes in the tree (from the OLD packet)
               IEnumerable<TreeNode> topNodes = tree.Nodes.OfType<TreeNode>();
               IEnumerable<TreeNode> nextGroup = topNodes;
               List<TreeNode> allNodes = new List<TreeNode>();

               // Resharper complains about possible enumerations of 'nextGroup'. This time, it's wrong.
               // disabling the warning with the 'comment scope' below
               // ReSharper disable PossibleMultipleEnumeration
               allNodes.AddRange(nextGroup);
               bool moreFound = true;
               while (moreFound)
               {
                   moreFound = false;
                   IEnumerable<TreeNode> currentGroupCopy = nextGroup;
                   nextGroup = new List<TreeNode>();
                   foreach (TreeNode node in currentGroupCopy)
                   {
                       List<TreeNode> subNodes = node.Nodes.OfType<TreeNode>().ToList();
                       nextGroup = nextGroup.Concat(subNodes);
                       allNodes = allNodes.Concat(subNodes).ToList();
                       moreFound |= subNodes.Any();
                   }
               }
               // ReSharper restore PossibleMultipleEnumeration

               // Some fields start with a bitmaps instead of the actual name, we try and skip the bitmask
               // e.g. ..01 1010 Packet Type: 0x1A
               // This LINQ also cuts the field name (removing bitmaps and value)
               // e.g. "Src Port: 2442" ----> "Src Port"
               char[] suspiciousChars = { '.', '0', '1' };
               List<string> expandedNodes = (from node in allNodes
                                             where node.IsExpanded
                                             where node.Text.Any() // No nodes with empty names please
                                             let nodeName = node.Text
                                             let suspiciousNameStart = suspiciousChars.Contains(nodeName[0])
                                             let nodeNameStart = suspiciousNameStart ? nodeName.IndexOf(" ") : 0
                                             let firstColonIndex = nodeName.IndexOf(':')
                                             let nodeNameEnd = firstColonIndex != -1 ? firstColonIndex : nodeName.Length
                                             select nodeName.Substring(nodeNameStart, nodeNameEnd - nodeNameStart)).ToList();

               // Removing OLD packet from the tree view
               tree.Nodes.Clear();


               // Parsing TShark response and adding nodes accordingly
               Queue<Tuple<TreeNode, XElement>> stack = new Queue<Tuple<TreeNode, XElement>>();
               stack.Enqueue(new Tuple<TreeNode, XElement>(null, packetElement));
               while (stack.Count != 0)
               {
                   Tuple<TreeNode, XElement> next = stack.Dequeue();
                   XContainer asContainer = next.Item2;
                   foreach (XElement sub in asContainer.Elements())
                   {
                       XElement nextSub = sub;
                       // Check if this node hidden in wireshark's GUI
                       XAttribute hideAttr = sub.Attribute(XName.Get("hide"));
                       if (hideAttr != null && hideAttr.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                           continue;

                       // Get 'Show name' and 'name'
                       // 'Show name' should be the text displayed in wireshark's tree. We want to show that as well (e.g. "User Datagram Protocol, Src Port: 4, Dst Port: 5")
                       // Fallback - Use 'name' which is the wireshark filter (e.g. 'udp.srcport')
                       string showName = sub.Attribute(XName.Get(PDML_SHOWNAME_ATTR))?.Value ??
                                        sub.Attribute(XName.Get("name"))?.Value ?? "UNKNOWN FIELD!";
                       string name = sub.Attribute(XName.Get("name"))?.Value ?? "UNKNOWN FIELD!";

                       if (showName == "data" && sub.Name == "field" && next.Item1 == null)
                       {
                           // Get the bytes count from the inner node called 'data' as well
                           string bytesCount = ((XElement)sub.FirstNode).Attribute(XName.Get("size"))?.Value ?? "0";
                           showName = $"Data ({bytesCount} bytes)"; // replace 'fake-field'wrapper'
                           nextSub = sub;
                           sub.Name = "proto"; // Overriding if set to 'field' so it will be added to the root
                       }

                       // SPECIAL CASE: No showname/filter - happens in some expert nodes/asn.1 nodes
                       if (String.IsNullOrWhiteSpace(showName) || showName == "fake-field-wrapper")
                       {
                           // We add the children to the stack and indicating the parent is THIS NODE's parent (essentially skipping this node)
                           stack.Enqueue(new Tuple<TreeNode, XElement>(next.Item1, sub));
                           continue;
                       }

                       // Create the new tree node
                       TreeNode nextNode = new TreeNode(showName) { Name = name };

                       // Hide position + size in HEX within the 'tool tip text'
                       // Format is: *startIndex*,*length*,*suspicious_flag*
                       // (both in bytes)
                       string index = sub.Attribute(XName.Get("pos"))?.Value ?? "0";
                       string length = sub.Attribute(XName.Get("size"))?.Value ?? "0";
                       string susFlagStr;
                       // Check if this node changes our suspicious state
                       int currIndex = int.Parse(index);
                       if (currIndex < lastIndex && sub.Name == "proto")
                       {
                           if (name != "_ws.malformed") // Special case - Malformed 'protocol' seems to casually ruin everything good.
                           {
                               // Change for this node and ANY OTHER FOLLOWING NODE
                               suspiciousFlag = true;
                           }
                       }
                       lastIndex = currIndex;

                       if (suspiciousFlag)
                       {
                           if (jPropsListed == null)
                           {
                               // ReSharper disable once PossibleMultipleEnumeration
                               jPropsListed = jProps.ToList();
                           }
                           string expectedJsonName = name + "_raw"; // 'name' is the wireshark filter.
                           JProperty ourProp = jPropsListed.FirstOrDefault(prop => prop.Name == expectedJsonName);
                           if (ourProp == null)
                           {
                               // Well fuck me.
                               susFlagStr = "0,00";
                           }
                           else
                           {
                               JArray propValue = ourProp?.Value as JArray;
                               // Digging further into the object if we have several arrays as a value
                               // (happens when the same field appears multiple times in the same packet)
                               bool hasInnerArray = propValue.Children().All(jtoken => jtoken is JArray);
                               bool anyLeft = true;
                               if (hasInnerArray)
                               {
                                   JArray temp = propValue.ElementAt(0) as JArray;
                                   propValue.RemoveAt(0);
                                   anyLeft = propValue.HasValues;
                                   propValue = temp;
                               }
                               if (!anyLeft)
                               {
                                   jPropsListed.Remove(ourProp);
                               }

                               string hex = propValue?.ElementAt(0)?.ToString();
                               if (hex == null)
                               {
                                   susFlagStr = "0,00";
                               }
                               else
                               {
                                   susFlagStr = "1," + hex;
                               }
                           }
                       }
                       else
                       {
                           susFlagStr = "0,00";
                       }
                       nextNode.ToolTipText = index + "," + length + "," + susFlagStr;

                       // Add to the stack of nodes to examine (to check for more children)
                       stack.Enqueue(new Tuple<TreeNode, XElement>(nextNode, nextSub));


                       // Determine 'parent node' to attach this node to.

                       // SPECIAL CASE: protocol nodes should be attached to the root of the tree (such as Ethernet, IP, UDP)
                       // Their XML tag is named 'proto
                       if (sub.Name == "proto")
                       {
                           if (showName.Contains("General information"))
                           {
                               // We don't want this node
                               continue;
                           }

                           // Protocol labels should be gray:
                           nextNode.BackColor = Color.FromArgb(240, 240, 240);
                           // Special case: Malformed Packet is found in a 'proto' tag as well
                           if (showName.Contains("Malformed Packet"))
                           {
                               // But malformed labels should be pink!
                               nextNode.BackColor = ERROR_PINK;
                           }
                           // Add as a 'lowest level' node to the tree
                           tree.Nodes.Add(nextNode);
                       }
                       else // Any other field
                       {
                           if (next.Item1 == null)
                           {
                               // This case is dissector who add NON 'proto' items directly to the root tree
                               // (e.g. USB URB)
                               tree.Nodes.Add(nextNode);

                           }
                           else
                           {
                               next.Item1.Nodes.Add(nextNode);
                           }
                       }
                   }
               }


               // Re-expand previously expanded nodes (based on stored node's names)
               Stack<TreeNode> nodesToExpand = new Stack<TreeNode>();
               foreach (TreeNode baseNode in tree.Nodes)
               {
                   nodesToExpand.Push(baseNode);
               }
               Stack<TreeNode> nextNodesToExpand;
               do
               {
                   nextNodesToExpand = new Stack<TreeNode>();
                   while (nodesToExpand.Count != 0)
                   {
                       TreeNode next = nodesToExpand.Pop();
                       bool wasExpanded = expandedNodes.Any(oldExpandedNodeName => next.Text.StartsWith(oldExpandedNodeName));
                       if (wasExpanded)
                           next.Expand();
                       foreach (TreeNode sub in next.Nodes)
                       {
                           nextNodesToExpand.Push(sub);
                       }
                   }
                   nodesToExpand = nextNodesToExpand;
               } while (nextNodesToExpand.Any());

               UpdateStatus("OK", StatusType.Good);
               GC.Collect();

           }));
        }


        private void UpdateStatus(string status, StatusType statusType)
        {
            _longStatus = status;

            // Truncating status string because if the Text field is too long a Memory access exception raises
            // next draw (most likely in this function when changing the MetroColor below).
            statusTextPanel.Text = status.Replace("\r\n", " ").Truncate(100);

            void ChangeStatusItemsForeColor(Color foreColor)
            {
                foreach (StatusBarAdvPanel statusBarAdvPanel in statusBar.Panels)
                {
                    statusBarAdvPanel.ForeColor = foreColor;
                }
            }

            ChangeStatusItemsForeColor(Color.White);
            switch (statusType)
            {
                case StatusType.Good:
                    statusBar.MetroColor = _goodStatusColor;
                    break;
                case StatusType.Warning:
                    statusBar.MetroColor = _warnStatusColor;
                    ChangeStatusItemsForeColor(Color.Black);
                    break;
                case StatusType.Bad:
                    statusBar.MetroColor = _badStatusColor;
                    break;
                case StatusType.Neutral:
                default:
                    statusBar.MetroColor = _neutralStatusColor;
                    break;
            }
        }

        private void CopyForCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PacketDefineControl pdc = GetCurrentPacketDefineControl();
            byte[] data;
            try
            {
                data = pdc.GetPacket().Data;
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox($"Error getting the current packet's data.\r\n{ex.Message}", MessageBoxIcon.Error);
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("new byte[]");
            sb.AppendLine("{");
            IEnumerable<string> withHexPrefixes = data.Select(b => $"0x{b:X2}");
            sb.AppendLine(string.Join(",", withHexPrefixes));
            sb.AppendLine("};");
            Clipboard.SetText(sb.ToString());
        }

        private void Control_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Not a file dragging (maybe tabs?)
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Skip(1).Any())
            {
                // Multiple files dropped, can't open
                ShowErrorMessageBox("Please only drop a single file.", MessageBoxIcon.Exclamation);
                return;
            }

            // Ask about unsaved changes
            if (_unsavedChangesExist)
            {
                DialogResult res = MessageBox.Show(this, "Want to save your changes?", Text, MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        DialogResult res2 = PromptUserToSave();
                        if (res2 == DialogResult.Cancel)
                        {
                            // User canceled saving, probably meant to cancel clearing as well
                            return;
                        }

                        break;
                }
            }

            string file = files.First();
            LoadFile(file);
        }

        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                // User is dragging tabs outside of the tab control, we restore the plus tab since dropping here
                // won't trigger the 'drop' function for tabs
                if (!(sender is TabControl || sender is TabPage))
                {
                    if (_plusTab != null)
                    {
                        tabControl.TabPages.Add(_plusTab);
                        _plusTab = null;
                    }
                }
            }
        }

        private void DrawTreeNodeLikeWireshark(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            bool isProto = e.Node.BackColor == Color.FromArgb(240, 240, 240);
            bool isMalformed = e.Node.BackColor == ERROR_PINK;

            // This expands the blue selection background to the end of the control (like in ws)
            int newWidth = packetTreeView.Width - 4 - e.Bounds.X;
            Rectangle newBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, newWidth, e.Bounds.Height);
            Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
            if (e.Node == e.Node.TreeView.SelectedNode)
            {
                Rectangle smaller = newBounds;
                Color color = isMalformed ? Color.FromArgb(204, 102, 125) : Color.FromArgb(192, 220, 243);
                // This color is the blue color used for selection in wireshark
                e.Graphics.FillRectangle(new SolidBrush(color), smaller);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, smaller, Color.Black, color, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                Rectangle smaller = newBounds;
                Color color = isProto ? Color.FromArgb(240, 240, 240) : Color.White;
                if (isMalformed)
                {
                    color = ERROR_PINK;
                }
                e.Graphics.FillRectangle(new SolidBrush(color), smaller);

                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, smaller, Color.Black, TextFormatFlags.GlyphOverhangPadding);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FlattenProtocolStackToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PacketDefineControl pdc = GetCurrentPacketDefineControl();
            try
            {
                // Check if packet is valid
                pdc.GetPacket();
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox($"Error getting the current packet's data.\r\n{ex.Message}", MessageBoxIcon.Error);
                return;
            }
            pdc.FlattenProtoStack();

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // Docking manager will KEEP RESETTING to TRUE no matter what I set in the designer mode or underlying code
            // So every time the load forms, Setting to FALSE *here* seems to solve this.
            dockingManager.AnimateAutoHiddenWindow = false;

            // Also, I can't choose 'no caption buttons' (will add ALL of them if I try) so I clear it here
            // I Changed my mind, commented out.
            //dockingManager1.CaptionButtons.Clear();

            // Give focus to PDC (and it's hex box)
            PacketDefineControl pdc = GetCurrentPacketDefineControl();
            pdc.Select();
        }

        private void GeneratePcapButton_Click(object sender, EventArgs e)
        {
            string wsPath = Settings.Default.lastWiresharkPath.Trim('"'); // removing any leading or trailing quotes

            if (!File.Exists(wsPath))
            {
                ShowErrorMessageBox($"No file found at the given wireshark path.\r\n{wsPath}", MessageBoxIcon.Error);
                return;
            }

            List<TempPacketSaveData> packets;
            try
            {
                packets = GetAllDefinedPackets();
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox("Exception when trying to get all defined packets.\r\nMessage:\r\n" + ex.Message, MessageBoxIcon.Error);
                return;
            }

            _wireshark.ExportToWsAsync(packets).ContinueWith((task) => Invoke((Action)QueueLivePreviewUpdate));
        }

        private PacketDefineControl GetCurrentPacketDefineControl()
        {
            return tabControl.SelectedTab.Controls.Cast<Control>().Single(c => c is PacketDefineControl) as PacketDefineControl;
        }

        private void HexViewBox_Copied(object sender, EventArgs e)
        {
            HexBox hexBox = sender as HexBox;
            hexBox?.CopyHex();
        }

        private void LivePreviewDelayBox_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(livePrevToolStripTextBox.Text, out int newVal))
            {
                _delayMs = newVal;
                livePrevToolStripTextBox.BackColor = Color.White;
                Settings.Default.livePreviewDelayMs = newVal;
                Settings.Default.Save();
            }
            else
            {
                livePrevToolStripTextBox.BackColor = Color.Salmon;
            }
        }

        private void LivePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool newValue = !_livePreviewChecked;
            _livePreviewChecked = newValue;
            previewtoolStripButton.Checked = newValue;
            Settings.Default.livePreviewActive = newValue;
            Settings.Default.Save();
            //Also update the newly shown tree with the current packet's data

            QueueLivePreviewUpdate();
        }

        private void LoadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "All Capture Files|*.pss;*.b2p;*.pcap;*.pcapng|" +
                         "PacketStudio Session file (*.pss)|*.pss|" +
                         "ByteArrayToPcap file (*.b2p)|*.b2p|" +
                         "Wireshark Capture files (*.pcap,*.pcapng)|*.pcap;*.pcapng"
            };
            DialogResult res = ofd.ShowDialog();

            if (res != DialogResult.Yes && res != DialogResult.OK)
            {
                return;
            }

            LoadFile(ofd.FileName);
            _unsavedChangesExist = false;
        }

        private bool LoadPackets(IPacketsProvider provider)
        {
            _isLoadingFile = true;


            tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;

            TabPageAdv localPlusTab = null;

            List<TabPageAdv> toRemove = new List<TabPageAdv>();
            foreach (TabPageAdv tabPage in tabControl.TabPages)
            {
                if (tabPage.IsPlusTab())
                {
                    localPlusTab = tabPage;

                }
                toRemove.Add(tabPage);
            }

            foreach (TabPageAdv tabPage in toRemove)
            {
                tabControl.TabPages.Remove(tabPage);
            }

            _nextPacketTabNumber = 1;

            bool anyPacketsFound = false;
            try
            {
                foreach (PacketSaveData packet in provider)
                {
                    anyPacketsFound = true;
                    AddNewTab(packet);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox($"Error reading packets from file.\r\nError Message: {ex.Message}", MessageBoxIcon.Error);
            }
            finally
            {
                provider.Dispose();
            }
            if (!anyPacketsFound)
            {
                ShowErrorMessageBox("Error reading packets from file.\r\n" +
                                    "No packets found.\t\n" +
                                    "(This could also be a result of failure to open the file)", MessageBoxIcon.Error);
            }


            if (localPlusTab != null)
            {
                tabControl.TabPages.Add(localPlusTab);
                if (!anyPacketsFound)
                {
                    // No packets are found so select the plus tab, this way a new "packet 1" will be created
                    tabControl.SelectedTab = localPlusTab;
                }
            }

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            TabControl_SelectedIndexChanged(null, null);

            _isLoadingFile = false;
            QueuePacketListUpdate();

            return anyPacketsFound;
        }

        private void LocateWireshark_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Wireshark.exe|Wireshark.exe"
            };
            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Settings.Default.lastWiresharkPath);
            }
            catch
            {
                // IDK...
            }
            DialogResult res = ofd.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (ofd.CheckFileExists)
                {
                    string dirPath = Path.GetDirectoryName(ofd.FileName);
                    if (SharksFinder.TryGetByPath(dirPath, out WiresharkDirectory wsDir))
                    {
                        Settings.Default.lastWiresharkPath = ofd.FileName;
                        Settings.Default.Save();
                        _wireshark = new WiresharkInterop(wsDir.WiresharkPath);
                        _tshark = new TSharkInterop(wsDir.TsharkPath);
                        _capinfos = new CapInfosInterop(wsDir.CapinfosPath);
                    }
                }
                else if (_isConstructing)
                {
                    // The user is fucking with us, giving us paths which does not exist
                    // since we are in the ctor, we don't have a WS path to work with so we quit
                    Environment.Exit(-1);
                }
            }
            else if (_isConstructing)
            {
                // We were called form the ctor, meaning no WS path is currently saved.
                // since the user cancelled the dialog, we don't have a WS path to work with so we quit
                Environment.Exit(-1);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _askAboutUnsaved)
            {
                // Check if we even have packets to save (or if the GUI is empty)
                bool anyPacketsFound = false;
                foreach (TabPageAdv tab in tabControl.TabPages)
                {
                    foreach (object control in tab.Controls)
                    {
                        if (control is PacketDefineControl casted)
                        {
                            bool containsPackeText = !String.IsNullOrWhiteSpace(casted.Text);
                            anyPacketsFound |= containsPackeText;
                        }
                    }
                }
                if (!anyPacketsFound)
                {
                    _unsavedChangesExist = false;
                }

                if (_unsavedChangesExist)
                {
                    DialogResult res = MessageBox.Show(this, "Want to save your changes?", Text, MessageBoxButtons.YesNoCancel);
                    switch (res)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                        case DialogResult.Yes:
                            DialogResult res2 = PromptUserToSave();

                            if (res2 == DialogResult.Cancel)
                            {
                                // User canceled saving, probably meant to cancel closing as well
                                e.Cancel = true;
                            }

                            break;
                    }
                }
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_unsavedChangesExist)
            {
                DialogResult res = MessageBox.Show(this, "Want to save your changes?", Text, MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        DialogResult res2 = PromptUserToSave();
                        if (res2 == DialogResult.Cancel)
                        {
                            // User canceled saving, probably meant to cancel clearing as well
                            return;
                        }

                        break;
                }
            }

            TabPageAdv localPlusTab = null;

            List<TabPageAdv> toRemove = new List<TabPageAdv>();
            foreach (TabPageAdv tabPage in tabControl.TabPages)
            {
                if (tabPage.IsPlusTab())
                {
                    localPlusTab = tabPage;

                }
                toRemove.Add(tabPage);
            }

            tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;

            foreach (TabPageAdv tabPage in toRemove)
            {
                tabControl.TabPages.Remove(tabPage);
            }

            _nextPacketTabNumber = 1;

            if (localPlusTab == null)
            {
                throw new Exception("Couldn't find the Plus Tab in the tabs under the tab control.");
            }
            tabControl.TabPages.Add(localPlusTab);

            // Adding new , aligned, "packet 1" tab
            TabControl_SelectedIndexChanged(null, null);

            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Remove any old filename from the form's title
            Text = _rawFormName;
        }

        private void NormalizeHexToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PacketDefineControl pdc = GetCurrentPacketDefineControl();
            try
            {
                // Check if packet is valid
                pdc.GetPacket();
            }
            catch (Exception ex)
            {
                ShowErrorMessageBox($"Error getting the current packet's data.\r\n{ex.Message}", MessageBoxIcon.Error);
                return;
            }
            pdc.NormalizeHex();

        }



        private void PacketTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string rawInstructions = packetTreeView.SelectedNode.ToolTipText;
            string[] split = rawInstructions.Split(',');

            // ReSharper disable InlineOutVariableDeclaration
            int index; // in BYTES!
            int length; // in BYTES!
            int susFlagValue;
            // ReSharper restore InlineOutVariableDeclaration

            bool suspicious = false;
            string hex = null;
            // Parse index,length and flag from tooltip text string
            if (split.Length < 2 ||
                !int.TryParse(split[0], out index) ||
                !int.TryParse(split[1], out length) ||
                !int.TryParse(split[2], out susFlagValue))
            {
                return;
            }

            if (susFlagValue == 1)
            {
                suspicious = true;
                hex = split[3];
            }

            // update hex view
            hexViewBox.Select(index, length);

            // Update Packet Define Control (Only if it contains a 'hex stream'!!)
            TabPageAdv tab = tabControl.SelectedTab;
            PacketDefineControl pdc = tab.Controls.OfType<PacketDefineControl>().First();
            pdc.SetSelection(0, 0); // Clear any old Selection

            if (!pdc.IsHexStream)
            {
                string status = "Bytes highlighting is only supported for 'clean' hex streams\r\n" +
                                          "(no offsets, ASCII representation, 0x's, spaces, etc)\r\n" +
                                          $"You can use \"{refactorDropDownButton.Text}\"→\"{normalizeHexToolStripMenuItem.Text}\" to get a clean hex stream.";
                UpdateStatus(status, StatusType.Warning);
                return;
            }

            if (!suspicious || hex == null)
            {
                pdc.SetSelection(index * 2, length);
                packetTreeView.Select();
            }
            else
            {
                string inputHex = String.Join(string.Empty, pdc.GetPacket().Data.Select(x => x.ToString("x2")));
                int nibbleIndex = index * 2;
                int nibblesCountExpected = hex.Length;
                string candidate = inputHex.Substring(nibbleIndex, nibblesCountExpected);

                bool isInRightPlace = false;

                if (candidate.StartsWith(hex))
                {
                    // All good.
                    isInRightPlace = true;
                }
                else
                {
                    string candidate2 = inputHex.Substring(nibbleIndex + 1, nibblesCountExpected);
                    if (candidate2.StartsWith(hex))
                    {
                        // All good.
                        isInRightPlace = true;
                    }
                }

                if (isInRightPlace)
                {
                    // Act as usual
                    pdc.SetSelection(index * 2, length);
                    packetTreeView.Select();
                }
                else
                {
                    if (length < 5 || length < (inputHex.Length / 2) / 50)
                    {
                        // Don't even try for short hex strings
                        hexViewBox.Select(0, 0);
                        return;
                    }


                    // Try to find real offset:
                    int realIndex = inputHex.IndexOf(hex);
                    if (realIndex == -1)
                    {
                        // Couldn't find in input, nothing much to do here...
                    }
                    else
                    {
                        pdc.SetSelection(realIndex, length);
                        packetTreeView.Select();

                        // fix hex view
                        hexViewBox.Select(realIndex / 2, length + 1);
                    }
                }
            }
        }


        private void PacketTreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            TreeNode node = packetTreeView.GetNodeAt(e.Location);
            if (node == null)
                return;

            packetTreeView.SelectedNode = node;

            ContextMenuStrip cms = new ContextMenuStrip();
            ExpandingToolStripButton tsb = new ExpandingToolStripButton("Expand Subtrees");
            tsb.Width = 110;
            tsb.TextAlign = ContentAlignment.MiddleLeft;
            cms.Items.Add(tsb);
            cms.AutoClose = true;

            ToolStripItemClickedEventHandler expandHandler = (obj, arg) =>
            {
                if (arg.ClickedItem == tsb)
                {
                    node.ExpandAll();
                }
            };
            ToolStripDropDownClosedEventHandler cleanUpHandler = null;
            cleanUpHandler = (o, args) =>
            {
                cms.Items.Remove(tsb);
                tsb.Dispose();
                cms.Closed -= cleanUpHandler; // Is this even required? am I crazy?
            };
            cms.Closed += cleanUpHandler;
            cms.ItemClicked += expandHandler;
            cms.AutoSize = true;
            Size pref = tsb.GetPreferredSize(packetTreeView.Size);
            pref.Width = tsb.Width;
            cms.MinimumSize = pref;
            cms.Size = pref;
            cms.ShowCheckMargin = false;
            cms.ShowImageMargin = false;
            cms.Show(packetTreeView, e.Location);
        }

        private void PacketTreeView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode != Keys.Right)
            {
                return;
            }

            TreeNode selectedNode = packetTreeView.SelectedNode;
            if (selectedNode == null) return;

            // Check if this is a 'Shit right' click (to expand all sub trees) or just normal right click
            if (e.Shift)
            {
                selectedNode.ExpandAll();
                // Keeping this node in view by de-selecting and re-selecting it
                packetTreeView.SelectedNode = null;
                packetTreeView.SelectedNode = selectedNode;
                // Signals to packetTreeView_KeyDown that this key press should be intercepted
                _interceptingKeyDown = true;
            }
        }

        private void PacketTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (_interceptingKeyDown)
            {
                e.Handled = true;
            }
            _interceptingKeyDown = false;
        }

        private void Pdc_ContentChanged(object sender, EventArgs e)
        {
            _unsavedChangesExist = true;
            Control maybePdc = tabControl.SelectedTab.Controls.Cast<Control>().FirstOrDefault(control => control is PacketDefineControl);
            UpdateHexView(maybePdc);
            QueueLivePreviewUpdate();
            QueuePacketListUpdate();
        }

        private void QueuePacketListUpdate()
        {

            try
            {
                _packetListTokenSource?.Cancel();
            }
            catch (Exception)
            {
                // For some reason this calls Process.Kill which might throw access is denied?
            }

            _packetListTokenSource = new CancellationTokenSource();

            if (packetListPreviewToolStripButton.Checked)
            {
                _packetListTimer.Change(_delayMs, Timeout.Infinite);
            }
        }

        private void previewInBatPContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool newVal = !_livePreviewInContext;
            _livePreviewInContext = newVal;
            previewContextToolStripButton.Checked = newVal;
            Settings.Default.livePreviewInContext = newVal;
            Settings.Default.Save();
            QueueLivePreviewUpdate();
        }

        private DialogResult PromptUserToSave()
        {
            Dictionary<int, Action<string>> filterIndexToSaveFunc = new Dictionary<int, Action<string>>()
            {
                {1,SaveAsPss},
                {2,SaveAsB2P},
                {3,SaveAsPcap},
            };

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PacketStudio Session file|*.pss|" +
                         "ByteArrayToPcap file|*.b2p|" +
                         "Wireshark/tcpdump/... -pcap|*.pcap"
            };
            DialogResult res = sfd.ShowDialog();

            if (res != DialogResult.Yes && res != DialogResult.OK)
            {
                // If the user managed to respond with anything other then 'Yes' or 'Ok' this is practically a cancel
                return DialogResult.Cancel;
            }


            bool anyPacketsFound = false;
            foreach (TabPageAdv tab in tabControl.TabPages)
            {
                foreach (object control in tab.Controls)
                {
                    if (control is PacketDefineControl)
                    {
                        anyPacketsFound = true;
                    }
                }
            }

            if (!anyPacketsFound)
            {
                ShowErrorMessageBox("No packets to save", MessageBoxIcon.Error);
                return DialogResult.OK;
            }

            Action<string> saveFunc = filterIndexToSaveFunc[sfd.FilterIndex];
            saveFunc(sfd.FileName);

            return DialogResult.OK;
        }
        private void QueueLivePreviewUpdate()
        {
            try
            {
                _livePrevTokenSource?.Cancel();
            }
            catch (Exception)
            {
                // For some reason this calls Process.Kill which might throw access is denied?
            }

            _livePrevTokenSource = new CancellationTokenSource();
            UpdateStatus(String.Empty, StatusType.Neutral);

            // Assert live preview is on
            if (_livePreviewChecked)
            {
                _livePrevTimer.Change(_delayMs, Timeout.Infinite);
            }

        }

        private void SaveAsPss(string path)
        {
            List<PacketSaveDataV3> allSaveData = new List<PacketSaveDataV3>(tabControl.TabPages.Count);
            foreach (TabPageAdv tabPage in tabControl.TabPages)
            {
                if (tabPage.IsPlusTab()) // plus tab is a special case
                    continue;

                _tabToPdc.TryGetValue(tabPage, out PacketDefineControl pdc);
                if (pdc == null)
                {
                    throw new Exception($"Could not find Packet Define Control in tab \"{tabPage.Text}\"");
                }
                try
                {
                    PacketSaveData psd = pdc.GetSaveData();
                    if (!(psd is PacketSaveDataV3))
                    {
                        throw new Exception("Expecting Packet Define Cotnrols to return PacketSaveDataV3. Can't serialize any other versions to .pss file");
                    }
                    allSaveData.Add(psd as PacketSaveDataV3);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error when getting save data for packet.\r\nTab: \"{tabPage.Text}\"\r\nError: {ex.Message}", ex);
                }
            }

            PssFileSaver saver = new PssFileSaver();
            saver.Save(path, allSaveData);
        }


        private void SaveAsB2P(string path)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (TabPageAdv tabPage in tabControl.TabPages)
            {
                if (tabPage.IsPlusTab()) // plus tab is a special case
                    continue;

                _tabToPdc.TryGetValue(tabPage, out PacketDefineControl pdc);
                if (pdc == null)
                {
                    throw new Exception($"Could not find Packet Define Control in tab \"{tabPage.Text}\"");
                }
                try
                {
                    PacketSaveData psd = pdc.GetSaveData();
                    if (first)
                    {
                        // For the first packet, append the PSD magic word (so it's in the .b2p's first line
                        sb.AppendLine(psd.MagicWord);
                        first = false;
                    }
                    sb.AppendLine(psd.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error when getting save data for packet.\r\nTab: \"{tabPage.Text}\"\r\nError: {ex.Message}", ex);
                }
            }

            FileStream outputFile = File.OpenWrite(path);
            StreamWriter sw = new StreamWriter(outputFile);
            sw.Write(sb.ToString());
            sw.Dispose();
        }

        private void SaveAsPcap(string path)
        {
            List<TempPacketSaveData> packets = GetAllDefinedPackets();
            string tempFilePath = _packetSaver.WritePackets(packets);
            File.Move(tempFilePath, path);
        }

        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = PromptUserToSave();

            if (res == DialogResult.OK)
            {
                _unsavedChangesExist = false;
            }
        }

        private void ShowErrorMessageBox(string errorMessage, MessageBoxIcon icon)
        {
            MessageBox.Show(errorMessage, _rawFormName, MessageBoxButtons.OK, icon);
        }


        private void TabControl_DragDrop(object sender, DragEventArgs e)
        {
            if (_plusTab != null)
            {
                tabControl.TabPages.Add(_plusTab);
                _plusTab = null;
            }
            // Update packet list to represent new order after tab dragging is finished
            QueuePacketListUpdate();
        }
        private void TabControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // This is a file drag-drop into the GUI, not trying to hide the plus tab.
                return;
            }


            // If we reached here, this might be a packet tab drag-drop
            if (_plusTab != null)
            {
                return;
            }

            _plusTab = tabControl.TabPages.OfType<TabPageAdv>().SingleOrDefault(page => page.IsPlusTab());
            if (_plusTab != null)
            {
                _rectangle = tabControl.GetTabRect(tabControl.TabCount - 1);
                Control control = tabControl;
                while (control != null)
                {
                    _rectangle.X += control.Bounds.X;
                    _rectangle.Y += control.Bounds.Y;
                    control = control.Parent;
                }
                tabControl.TabPages.Remove(_plusTab);
            }
        }

        private void TabControl_MouseClick(object sender, MouseEventArgs e)
        {
            TabPageAdvCollection tabs = tabControl.TabPages;

            // Getting tab control which was clicked based on the mouse's location
            TabPageAdv pointedTab = tabs.Cast<TabPageAdv>()
                .Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location))
                .FirstOrDefault();

            if (pointedTab == null)
                return;

            if (e.Button == MouseButtons.Middle)
            {
                // Use clicked a tab with it's middle mouse button, probably trying to remove the tab
                // (Common UI respond like in Chrome)

                if (pointedTab.IsPlusTab()) // Not removing the plus tab
                    return;

                if (tabs.Count == 2)
                {
                    // Only have this packet left and the "plusTab", not removing
                    return;
                }

                // If we are removing the currently selected tab, reselect the previous tab
                if (tabControl.SelectedTab == pointedTab && pointedTab.TabIndex > 1)
                {
                    tabControl.SelectedIndex = pointedTab.TabIndex - 1;
                }
                tabs.Remove(pointedTab);

                // Update packet list to remove this tab
                QueuePacketListUpdate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Use clicked a tab with it's right mouse button, probably looking for a context menu (to rename)

                if (pointedTab.IsPlusTab()) // Not renaming the plus tab
                    return;

                _tabRequestingRename = pointedTab;
                pointedTab.ContextMenu.Show(tabControl, e.Location);
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPageAdv currentSelectTab = tabControl.SelectedTab;
            bool isCurrentTabEqualLastTab = currentSelectTab == _lastSelectedPage;


            if (_lastSelectedPage != null && !isCurrentTabEqualLastTab)
            {
                // Last tab isn't the current tab
                // remove it's controls to free handles
                _lastSelectedPage.Controls.Clear();
            }

            // Update last selected tab
            _lastSelectedPage = tabControl.SelectedTab;

            if (tabControl.SelectedTab.IsPlusTab())
            {
                // The selected tab is the special 'Plus Tab' - we should create a new packet
                TabPageAdv localPlusTab = tabControl.SelectedTab;
                tabControl.TabPages.Remove(localPlusTab);
                TabPageAdv newPage = AddNewTab(null);
                tabControl.TabPages.Add(localPlusTab);
                tabControl.SelectedTab = newPage;
                TabControl_SelectedIndexChanged(null, null);
            }
            else
            {
                // The selected tab is a regular one, add PDC
                TabPageAdv tab = tabControl.SelectedTab;

                if (tab == null)
                    return;
                if (!_tabToPdc.TryGetValue(tab, out PacketDefineControl pdc))
                {
                    // Catastrophic error
                    return;
                }
                tab.Controls.Add(pdc);

                // Give focus to it's PDC
                tab.Controls[0].Focus();
            }

            // Remove old packet from tree view & Hex view
            packetTreeView.Nodes.Clear();
            hexViewBox.ByteProvider = new DynamicByteProvider(new byte[0]);

            // Update hex & Tree
            QueueLivePreviewUpdate();
        }

        private void TabPage_onRenameRequested(object sender, EventArgs eventArgs)
        {
            if (!(_tabRequestingRename is Control tab))
            {
                // Something bad happened
                return;
            }
            TabRenameDialog trd = new TabRenameDialog(tab.Text);
            DialogResult result = trd.ShowDialog();
            if (result == DialogResult.OK)
            {
                tab.Text = trd.NewName;
                // Update packet list to show new tab's name
                QueuePacketListUpdate();
            }
        }

        private void UpdateHexView(object sender)
        {
            if (sender is PacketDefineControl pdc)
            {
                hexViewBox.ForeColor = Color.DarkGray;
                byte[] packet;
                try
                {
                    packet = pdc.GetPacket().Data;
                }
                catch (Exception)
                {
                    packet = null;
                }
                if (packet != null && packet.Any())
                {
                    DynamicByteProvider dbp = new DynamicByteProvider(packet);
                    hexViewBox.ByteProvider = dbp;
                    hexViewBox.ForeColor = Color.Black;
                }
            }
        }

        private void UpdateLivePreview(object state)
        {
            Debug.Write("LIVE UPDATE TIMER TIRGGERED");
            if (!Created) return;

            Invoke((Action)(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                UpdateStatus(string.Empty, StatusType.Neutral);
                TabPageAdv tabPage = tabControl.SelectedTab;
                if (!(tabPage.Controls[0] is PacketDefineControl pdc))
                {
                    UpdateStatus("Could not find packet define control...", StatusType.Bad);
                    return;
                }
                if (String.IsNullOrWhiteSpace(pdc.Text))
                {
                    UpdateStatus("Packet contains no bytes.", StatusType.Bad);
                    return;
                }

                TempPacketSaveData packet;
                try
                {
                    packet = pdc.GetPacket();
                }
                catch (Exception ex)
                {
                    UpdateStatus("Invalid packet. " + ex.Message, StatusType.Bad);
                    return;
                }



                UpdateStatus("Working...", StatusType.Neutral);

                IEnumerable<TempPacketSaveData> packets;
                int packetIndex;
                if (_livePreviewInContext)
                {
                    try
                    {
                        packets = GetAllDefinedPackets();
                    }
                    catch (Exception ex)
                    {
                        UpdateStatus(ex.Message, StatusType.Bad);
                        return;
                    }
                    packetIndex = tabControl.SelectedIndex;
                }
                else
                {
                    packets = new[] { packet };
                    packetIndex = 0;
                }
                CancellationToken token = _livePrevTokenSource.Token;
                Task<TSharkCombinedResults> tsharkTask = _tshark.GetPdmlAndJsonAsync(packets, packetIndex, token, _needToBeEnabledHeuristics, _needToBeDisabledHeuristics);

                tsharkTask.ContinueWith((task) =>
                {
                    packets = null; // Hoping GC will get the que
                    ContinueTsharkTask(task, token);
                }, _livePrevTokenSource.Token);
            }));
        }

        private void UpdatePacketListBox(CancellationToken token)
        {
            List<TempPacketSaveData> packets;
            string[] packetsTextLines = null;
            int maxLine = 0;

            if (token.IsCancellationRequested)
                return;

            if (packetListPreviewToolStripButton.Checked)
            {
                try
                {
                    packets = GetAllDefinedPackets();
                    packetsTextLines = _tshark.GetTextOutputAsync(packets, 0, CancellationToken.None, _needToBeEnabledHeuristics, _needToBeDisabledHeuristics).Result;
                    maxLine = packetsTextLines.Max(s => s.Length);
                }
                catch (Exception)
                {
                    packetsTextLines = null;
                }
            }

            if (token.IsCancellationRequested)
                return;

            if (_isUpdatingList)
            {
                // Other thread already in updating...
                return;
            }

            if (packetsTextLines != null)
            {
                ParseTSharkTextOutput ptto = ParseTSharkTextOutput.Parse(packetsTextLines);
                List<ParseTSharkTextOutput.LinkedParsedPacket> zipped = ptto.Packets.Zip(tabControl.TabPages.Cast<TabPageAdv>(),
                    ParseTSharkTextOutput.LinkedParsedPacket.FromUnlinked).ToList();

                _isUpdatingList = true;

                // Actual list update
                _packetsListDataGrid.Invoke((Action)(() =>
                {
                    _packetsListDataGrid.DataSource = zipped;
                    _packetsListDataGrid.ClearSelection();

                    _isUpdatingList = false;
                    _packetsListDataGrid.StretchLastColumn();
                }));
            }
        }


        private void WireDragDrop(IEnumerable ctls)
        {
            foreach (Control ctl in ctls)
            {
                ctl.AllowDrop = true;
                ctl.DragEnter += Control_DragEnter;
                ctl.DragDrop += Control_DragDrop;
                ctl.DragLeave += CtlOnDragLeave;
                WireDragDrop(ctl.Controls);
            }
        }

        private void CtlOnDragLeave(object sender, EventArgs eventArgs)
        {
            if (!(sender is TabControl || sender is TabPage))
            {
                Console.WriteLine("DRAG LEFT " + sender.ToString());
            }
        }

        public List<TempPacketSaveData> GetAllDefinedPackets()
        {
            List<TempPacketSaveData> packets = new List<TempPacketSaveData>();
            foreach (TabPageAdv tabPage in tabControl.TabPages)
            {
                if (tabPage.IsPlusTab()) // plus tab is a special case
                    continue;

                _tabToPdc.TryGetValue(tabPage, out PacketDefineControl pdc);
                if (pdc == null)
                {
                    throw new Exception($"Could not find Packet Define Control in tab \"{tabPage.Text}\"");
                }
                try
                {
                    TempPacketSaveData nextPacket = pdc.GetPacket();
                    packets.Add(nextPacket);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error when deserializing packet.\r\nTab: \"{tabPage.Text}\"\r\nError: {ex.Message}", ex);
                }

            }
            return packets;
        }



        public void LoadFile(string path)
        {
            FileInfo finfo = new FileInfo(path);
            if (!finfo.Exists)
            {
                ShowErrorMessageBox($"No such file '{path}'", MessageBoxIcon.Error);
                return;
            }


            string extension = finfo.Extension.ToLower();
            bool isCapinfoSupported = extension == ".pcap" || extension == ".pcapng";
            // Use Humanizer to parse the size (in bytes) to the awesome 'ByteSize' type
            ByteSize bsize = finfo.Length.Bytes();
            // Get the file size as a string e.g. "20 MB", "5 KB" etc...
            string humanizedByteSize = $"{bsize.LargestWholeNumberValue:F2}{bsize.LargestWholeNumberSymbol}";
            if (isCapinfoSupported && bsize.Kilobytes > 500) // More than 500 KB
            {
                // Found a large pcap/pcapng, check if it has a large amount of packets
                try
                {
                    int packetsCount = this._capinfos.GetPacketsCount(finfo.FullName);
                    int MAX_RECOMMENDED_PACKETS_COUNT = 500;
                    if (packetsCount >= MAX_RECOMMENDED_PACKETS_COUNT)
                    {
                        // As suspected, this capture contains many packets, warn the user
                        string errorMessage = $"The file you are trying to load is quite large ({humanizedByteSize})\r\n" +
                                              $"The file contains {packetsCount:##,###} packets, it's recommended you work file files with less than {MAX_RECOMMENDED_PACKETS_COUNT} packets\r\n\r\n" +
                                              "Are you sure you want to load this file?";

                        DialogResult res = MessageBox.Show(errorMessage, _rawFormName, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        if (res == DialogResult.No)
                        {
                            // Abort loading of large file
                            return;
                        }
                    }
                }
                catch (Exception e)
                {
                    string errorMessage = $"The file you are trying to load is quite large ({humanizedByteSize})\r\n" +
                                          $"Also an error prevented {_rawFormName} from getting the packets count in this capture.\r\n" +
                                           "The error message is:\r\n" +
                                          $"{e}\r\n\r\n" +
                                           "Are you sure you want to load this file?";

                    DialogResult res = MessageBox.Show(errorMessage, _rawFormName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No)
                    {
                        // Abort loading of large file
                        return;
                    }
                }
            }

            IPacketsProvider provider = null;
            string ext = Path.GetExtension(path);
            switch (ext)
            {
                case "pss":
                case ".pss":
                    provider = new PssFileProvider(path);
                    break;
                case "b2p":
                case ".b2p":
                    provider = new B2pProvider(path);
                    break;
                case "pcap":
                case ".pcap":
                    provider = new PcapProvider(path);
                    break;
                case "pcapng":
                case ".pcapng":
                    provider = new PcapNGProvider(path);
                    break;
                default:
                    ShowErrorMessageBox($"Unsupported extension '{ext}'", MessageBoxIcon.Error);
                    break;
            }

            if (provider != null)
            {
                bool loaded = LoadPackets(provider);
                if (loaded)
                {
                    string fileName = Path.GetFileName(path);
                    Text = $"{_rawFormName} - {fileName}";
                    Pdc_ContentChanged(null, null);
                }
                else
                {
                    Text = _rawFormName;
                    Pdc_ContentChanged(null, null);
                    _unsavedChangesExist = false;
                }
            }
        }

        private void statusTextPanel_Click(object sender, EventArgs e)
        {
            ShowErrorMessageBox(_longStatus, MessageBoxIcon.Information);
        }

        private void WiresharkInteropUpdating(WiresharkInterop oldWsInterop, WiresharkInterop newWsInterop)
        {
            string dir = Path.GetDirectoryName(newWsInterop.WiresharkPath);
            wsVerPanel.Text = $"Wireshark {newWsInterop.Version}, Running from {dir}";
        }

        private void insertAsciiToolStripButton_Click(object sender, EventArgs e)
        {
            AsciiDialog asciiDialog = new AsciiDialog();
            DialogResult diagResult = asciiDialog.ShowDialog();
            if (diagResult == DialogResult.OK)
            {
                string text = asciiDialog.TextToInsert;
                byte[] textBytes = Encoding.ASCII.GetBytes(text);
                string textHex = textBytes.ToHex();
                PacketDefineControl pdc = GetCurrentPacketDefineControl();

                // Get current selection in PDC
                int selectionStart;
                int selectionLen;
                (selectionStart, selectionLen) = pdc.GetSelection;

                // There's a chance that nothing is selected - check selection length
                bool hasSelection = selectionLen != 0;
                if (!hasSelection)
                {
                    string currentPdcText = pdc.Text;
                    int currentPdcCaret = selectionStart;
                    string newText = currentPdcText.Substring(0, currentPdcCaret) + textHex +
                                     currentPdcText.Substring(currentPdcCaret);
                    pdc.Text = newText;
                    pdc.CaretPosition = currentPdcCaret + textHex.Length;
                }
                else
                {
                    string currentPdcText = pdc.Text;
                    // Removing the selected text
                    currentPdcText = currentPdcText.Substring(0, selectionStart) +
                                     currentPdcText.Substring(selectionStart + selectionLen);
                    // Inserting converted ASCII instead of removed text
                    string newText = currentPdcText.Substring(0, selectionStart) + textHex +
                                     currentPdcText.Substring(selectionStart);
                    pdc.Text = newText;
                    pdc.CaretPosition = selectionStart + textHex.Length;
                }

            }
        }

        private void packetListPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripButton tsb)
            {
                tsb.Checked = !tsb.Checked;
                Settings.Default.livePreviewPacketList = tsb.Checked;
                Settings.Default.Save();
                QueueLivePreviewUpdate();
            }
        }

        private List<string> _lastUserDisabledHeuristics = null;
        private List<string> _lastUserEnabledHeuristics = null;

        private List<string> _needToBeDisabledHeuristics = new List<string>();
        private List<string> _needToBeEnabledHeuristics = new List<string>();

        private async void heurDissectorsToolStripButton_Click(object sender, EventArgs e)
        {
            // Async-ly get dissectors list
            if (!_heurDissectorsTask.IsCompleted)
            {
                LoadingSplash splash = new LoadingSplash();
                splash.Show(this);
                await _heurDissectorsTask;
                splash.Close();
                splash.Dispose();
            }

            List<string> enabled = heurDissectorsList.Where(entry => entry.Enabled).Select(entry => entry.ShortName).ToList();
            List<string> disabled = heurDissectorsList.Where(entry => !entry.Enabled).Select(entry => entry.ShortName).ToList();

            bool isDefaultLists = false;
            if (_lastUserDisabledHeuristics == null)
            {
                isDefaultLists = true;
                _lastUserDisabledHeuristics = disabled;
            }
            if (_lastUserEnabledHeuristics == null)
            {
                isDefaultLists = true;
                _lastUserEnabledHeuristics = enabled;
            }

            DialogResult res;
            bool retried = false;
            HeurDissectorsDialog hdd = new HeurDissectorsDialog(_lastUserDisabledHeuristics, _lastUserEnabledHeuristics, isDefaultLists);
            do
            {
                res = hdd.ShowDialog(this);
                if (res == DialogResult.Retry)
                {
                    // User requested reload in the dialog.
                    _heurDissectorsTask = _tshark.GetHeurDissectors();
                    // Async-ly get dissectors list
                    _heurDissectorsTask.Wait();

                    enabled = heurDissectorsList.Where(entry => entry.Enabled).Select(entry => entry.ShortName).ToList();
                    disabled = heurDissectorsList.Where(entry => !entry.Enabled).Select(entry => entry.ShortName).ToList();

                    // Keeping old dialog's location
                    Point oldLocation = hdd.Location;
                    // Setting new dialog now, in the next loop iteration it will be shown
                    hdd.Dispose();
                    hdd = new HeurDissectorsDialog(disabled, enabled, true);
                    // Forcing new dialog to show where the old dialog was
                    hdd.StartPosition = FormStartPosition.Manual;
                    hdd.Location = oldLocation;

                    // Reset all saved lists
                    _lastUserDisabledHeuristics = null;
                    _lastUserEnabledHeuristics = null;
                    _needToBeEnabledHeuristics = null;
                    _needToBeDisabledHeuristics = null;

                    retried = true;
                }
            } while (res == DialogResult.Retry);

            if (res == DialogResult.OK)
            {
                _lastUserEnabledHeuristics = hdd.Enabled;
                HashSet<string> diagEnabledSet = new HashSet<string>(_lastUserEnabledHeuristics);
                diagEnabledSet.ExceptWith(enabled);
                _needToBeEnabledHeuristics = diagEnabledSet.ToList();

                _lastUserDisabledHeuristics = hdd.Available;
                HashSet<string> diagDisabledSet = new HashSet<string>(_lastUserDisabledHeuristics);
                diagDisabledSet.ExceptWith(disabled);
                _needToBeDisabledHeuristics = diagDisabledSet.ToList();
            }

            hdd.Dispose();
            if (res == DialogResult.OK || retried)
            {
                // Save to settings
                Settings.Default.EnabledHeuristicDissectors = string.Empty;
                if (_lastUserEnabledHeuristics != null)
                {
                    Settings.Default.EnabledHeuristicDissectors = HackyBsae64StringListSerializer.Serialize(_lastUserEnabledHeuristics);
                }
                Settings.Default.DisabledHeuristicDissectors = string.Empty;
                if (_lastUserDisabledHeuristics != null)
                {
                    Settings.Default.DisabledHeuristicDissectors = HackyBsae64StringListSerializer.Serialize(_lastUserDisabledHeuristics);
                }
                Settings.Default.NeedToBeEnabledDissectors = string.Empty;
                if (_needToBeEnabledHeuristics != null)
                {
                    Settings.Default.NeedToBeEnabledDissectors = HackyBsae64StringListSerializer.Serialize(_needToBeEnabledHeuristics);
                }
                Settings.Default.NeedToBeDisabledDissectors = string.Empty;
                if (_needToBeDisabledHeuristics != null)
                {
                    Settings.Default.NeedToBeDisabledDissectors = HackyBsae64StringListSerializer.Serialize(_needToBeDisabledHeuristics);
                }
                Settings.Default.Save();

                QueueLivePreviewUpdate();
            }
        }

        private void _packetsListDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (_packetsListDataGrid.SelectedRows.Count == 0)
                return;

            if (_isUpdatingList)
                return;

            DataGridViewRow selectedRow = _packetsListDataGrid.SelectedRows[0];
            var packet = selectedRow.DataBoundItem as ParseTSharkTextOutput.LinkedParsedPacket;
            tabControl.SelectedTab = packet.GetLinkedPage();

        }

        TouchStyleColorTable lightColorTable = new TouchStyleColorTable()
        {
            BackStageButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123))))),
            BackStageButtonForeColor = System.Drawing.Color.White,
            BackStageButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            BackStageCaptionColor = System.Drawing.Color.White,
            BackStageCloseButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80))))),
            BackStageCloseButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198))))),
            BackStageCloseButtonHoverForeColor = System.Drawing.Color.White,
            BackStageCloseButtonPressedForeColor = System.Drawing.Color.White,
            BackStageMaximizeButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198))))),
            BackStageMaximizeButtonHoverForeColor = System.Drawing.Color.White,
            BackStageMaximizeButtonPressedForeColor = System.Drawing.Color.White,
            BackStageMinimizeButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198))))),
            BackStageMinimizeButtonHoverForeColor = System.Drawing.Color.White,
            BackStageMinimizeButtonPressedForeColor = System.Drawing.Color.White,
            BackStageNavigationButtonBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(57)))), ((int)(((byte)(123))))),
            BackStageNavigationButtonForeColor = System.Drawing.Color.White,
            BackStageRestoreButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(114)))), ((int)(((byte)(198))))),
            BackStageRestoreButtonHoverForeColor = System.Drawing.Color.White,
            BackStageRestoreButtonPressedForeColor = System.Drawing.Color.White,
            BackStageSysytemButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            BackStageTabColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            BackStageTabForeColor = System.Drawing.Color.White,
            BackStageTabHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            BottomToolStripBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ButtonCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211))))),
            ButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242))))),
            ButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211))))),
            CheckBoxForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139))))),
            CheckedToolstripTabItemForeColor = System.Drawing.Color.Black,
            CloseButtonBackground = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80))))),
            CloseButtonForeColor = System.Drawing.Color.White,
            CloseButtonHoverForeColor = System.Drawing.Color.White,
            CloseButtonPressed = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(61)))), ((int)(((byte)(61))))),
            CloseButtonPressedForeColor = System.Drawing.Color.White,
            DropDownBodyColor = System.Drawing.Color.White,
            DropDownMenuItemBackground = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            DropDownSelectedTextForeColor = System.Drawing.Color.White,
            DropDownTextForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            DropDownTitleBackground = System.Drawing.Color.White,
            HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            HoverTabBackColor = System.Drawing.Color.White,
            HoverTabForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ImageMargin = System.Drawing.Color.White,
            InActiveToolStripTabItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            MaximizeButtonForeColor = System.Drawing.Color.White,
            MaximizeButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            MaximizeButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            MenuButtonArrowColor = System.Drawing.Color.White,
            MenuButtonHoverArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            MinimizeButtonForeColor = System.Drawing.Color.White,
            MinimizeButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            MinimizeButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            OverFlowArrowColor = System.Drawing.Color.White,
            QATButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(242))))),
            QATDownArrowColor = System.Drawing.Color.White,
            RestoreButtonForeColor = System.Drawing.Color.White,
            RestoreButtonHoverForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            RestoreButtonPressedForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            RibbonPanelBackColor = System.Drawing.Color.White,
            SplitButtonPressed = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            SplitButtonSelected = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(143)))), ((int)(((byte)(201))))),
            SystemButtonBackground = System.Drawing.Color.White,
            SystemButtonPressed = System.Drawing.Color.White,
            TabGroupColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(203)))), ((int)(((byte)(29))))),
            TabScrollArrowColor = System.Drawing.Color.White,
            TitleColor = System.Drawing.Color.White,
            ToolstripActiveTabItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolStripArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolStripBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolstripButtonPressedBorder = System.Drawing.Color.Black,
            ToolStripDropDownBackColor = System.Drawing.Color.White,
            ToolStripDropDownButtonHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247))))),
            ToolStripDropDownButtonSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240))))),
            ToolstripSelectedTabItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolStripSpliterColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolstripTabItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(73)))), ((int)(((byte)(255))))),
            ToolstripTabItemCheckedGradientBegin = System.Drawing.Color.Empty,
            ToolstripTabItemForeColor = System.Drawing.Color.White,
            ToolstripTabItemSelectedGradientBegin = System.Drawing.Color.Empty
        };
    }
}