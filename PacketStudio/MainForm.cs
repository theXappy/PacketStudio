using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Be.Windows.Forms;
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
using Action = System.Action;
// ReSharper disable LocalizableElement
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace PacketStudio
{
    public partial class MainForm : RibbonForm
	{
        private TempPacketsSaver _tps = new TempPacketsSaver();
        private WiresharkInterop _wireshark;
		private TSharkInterop _tshark;

		CancellationTokenSource _tokenSource;

		private BrushInfoColorArrayList badGradient = new BrushInfoColorArrayList(new[]
		{
			Color.FromArgb(0xEA,0x54,0x55),
			Color.FromArgb(0xFE,0xB6,0x92)
		});
		private BrushInfoColorArrayList goodGradient = new BrushInfoColorArrayList(new[]
		{
			Color.FromArgb(0x28,0xC7,0x6F),
			Color.FromArgb(0x81,0xFB,0xB8)
		});
		private static Color ERROR_PINK => Color.FromArgb(255, 92, 92); // Wireshark's error background color

	    private readonly System.Threading.Timer _timer;
		private bool _askAboutUnsaved;
		private bool _unsavedChangesExist = false;
		private int _nextPacketTabNumber = 1;
		private bool _isConstructing;
		
		private TabPage _tabRequestingRename = null;

		public MainForm()
		{
			_timer = new System.Threading.Timer(UpdateLivePreview);
			InitializeComponent();
			// Removing initial, unalligned, "packet 1" tab.
			tabControl.TabPages.Remove(tabControl.SelectedTab);
			// Adding new , alligned, "packet 1" tab
			tabControl_SelectedIndexChanged(null, null);

			_askAboutUnsaved = Settings.Default.lastAskToSaveSetting;

			bool previewActive = Settings.Default.livePreviewActive;
			previewtoolStripButton.Checked = previewActive;
			_delayMs = Settings.Default.livePreviewDelayMs;
			livePrevToolStripTextBox.Text = _delayMs.ToString();

			packetTreeView.DrawNode += DrawTreeNodeLikeWireshark;

			// TODO: This might throw...
			string wsPath = Path.GetDirectoryName(Settings.Default.lastWiresharkPath);
			WiresharkDirectory dir;
			// Try to find the wireshark by the path saved in the settings
			if (SharksFinder.TryGetByPath(wsPath, out dir))
			{
				_wireshark = new WiresharkInterop(dir.WiresharkPath);
				_tshark = new TSharkInterop(dir.TsharkPath);
			}
			else
			{
				// Fallback - get the default WS path found in the system
				dir = SharksFinder.GetDirectories().FirstOrDefault();
				if (dir != null)
				{
					_wireshark = new WiresharkInterop(dir.WiresharkPath);
					_tshark = new TSharkInterop(dir.TsharkPath);
				}
				else
				{
					MessageBox.Show(
						"Wireshark path not saved in the settings and couldn't be found in any of the default paths.\r\n\r\n" +
						"Please select the location in the next dialog",Text+" - Init Problem",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					_isConstructing = true;
					locateWireshark_Click(null, null);
					_isConstructing = false;
				}
			}

			_tokenSource = new CancellationTokenSource();
			_unsavedChangesExist = false;
		}

		private void DrawTreeNodeLikeWireshark(object sender, DrawTreeNodeEventArgs e)
		{
			if (e.Node == null) return;

			bool isProto = e.Node.BackColor == Color.FromArgb(240, 240, 240);
			bool isMalformed = e.Node.BackColor == ERROR_PINK;

			// This expands the blue selection background to the end of the control (like in ws)
			var newWidth = packetTreeView.Width - 4 - e.Bounds.X;
			var newBounds = new Rectangle(e.Bounds.X, e.Bounds.Y, newWidth, e.Bounds.Height);
			Font font = e.Node.NodeFont ?? e.Node.TreeView.Font;
			if (e.Node == e.Node.TreeView.SelectedNode)
			{
				var smaller = newBounds;
				var color = isMalformed ? Color.FromArgb(204, 102, 125) : Color.FromArgb(192, 220, 243);
				// This color is the blue color used for selection in wireshark
				e.Graphics.FillRectangle(new SolidBrush(color), smaller);
				TextRenderer.DrawText(e.Graphics, e.Node.Text, font, smaller, Color.Black, color, TextFormatFlags.GlyphOverhangPadding);
			}
			else
			{
				var smaller = newBounds;
				var color = isProto ? Color.FromArgb(240, 240, 240) : Color.White;
				if (isMalformed)
				{
					color = ERROR_PINK;
				}
				e.Graphics.FillRectangle(new SolidBrush(color), smaller);

				TextRenderer.DrawText(e.Graphics, e.Node.Text, font, smaller, Color.Black, TextFormatFlags.GlyphOverhangPadding);
			}
		}

		private void GeneratePcapButton_Click(object sender, EventArgs e)
		{
			string wsPath = Settings.Default.lastWiresharkPath.Trim('"'); // removing any leading or trailing quotes

			if (!File.Exists(wsPath))
			{
				MessageBox.Show($"No file found at the given wireshark path.\r\n{wsPath}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

			List<byte[]> packets;
			try
			{
				packets = GetAllDefinedPackets();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message,"PacketStudio",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}

			_wireshark.ExportToWsAsync(packets).ContinueWith((task) => Invoke((Action)QueueLivePreviewUpdate));
		}

		public List<byte[]> GetAllDefinedPackets()
		{
			List<byte[]> packets = new List<byte[]>();
			foreach (TabPage tabPage in tabControl.TabPages)
			{
				if (tabPage.IsPlusTab()) // plus tab is a special case
					continue;

				PacketDefineControl pdc = null;
				_tabToPdc.TryGetValue(tabPage, out pdc);
				if (pdc == null)
				{
					throw new Exception($"Could not find Packet Define Control in tab \"{tabPage.Text}\"");
				}
				try
				{
					byte[] nextPacket = pdc.GetPacket();
					packets.Add(nextPacket);
				}
				catch (Exception ex)
				{
					throw new Exception($"Error when deserializing packet.\r\nTab: \"{tabPage.Text}\"\r\nError: {ex.Message}", ex);
				}

			}
			return packets;
		}



		private void Form1_Load(object sender, EventArgs e)
		{
			// Docking manager will KEEP FUCKING RESETTING THIS TO TRUE no matter what I set in the designer mode or underlaying code
			// So every time the load forms, EAT A FUCKING FALSE YOU PEICE OF SHIT
			dockingManager1.AnimateAutoHiddenWindow = false;
			// Also, I can't choose 'no caption buttons' (will add ALL of them if I try) so I clear it here
			// I Changed my mind, commented out.
			//dockingManager1.CaptionButtons.Clear();
		}

		private TabPage AddNewTab(PacketSaveData saveData)
		{
			TabPage newPage = new TabPage("Packet " + (_nextPacketTabNumber));
			newPage.ContextMenu = new ContextMenu(new MenuItem[1]
			{
				new MenuItem("Rename",tabPage_onRenameRequested)
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

			if (!_noDraw)
			{
				// Update packet list to include new tab
				UpdatePacketListBox();
			}

			return newPage;
		}
		
		private TabPage _lastSelectedPage = null;
		private Dictionary<TabPage, PacketDefineControl> _tabToPdc = new Dictionary<TabPage, PacketDefineControl>();
		
		private void tabPage_onRenameRequested(object sender, EventArgs eventArgs)
		{
			Control tab = _tabRequestingRename as Control;
			if (tab == null)
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
				UpdatePacketListBox();
			}
		}

		private void UpdatePacketListBox()
		{
			packetTabsList.Items.Clear();
			int count = tabControl.TabPages.Count;
			int padding = (int)Math.Log10(count);
			int digitsRequired = 1 + padding;
			int index = 1;
			foreach (TabPage tabPage in tabControl.TabPages)
			{
				if (tabPage.IsPlusTab()) // plus tab is a special case
					continue;

				TabPacketListItem tpli = new TabPacketListItem(index.ToString().PadLeft(digitsRequired,'0'),tabPage);
				index++;

				packetTabsList.Items.Add(tpli);
			}
		}

		private void Pdc_ContentChanged(object sender, EventArgs e)
		{
			_unsavedChangesExist = true;
			Control maybePdc = tabControl.SelectedTab.Controls.Cast<Control>().FirstOrDefault(control => control is PacketDefineControl);
			UpdateHexView(maybePdc);
			QueueLivePreviewUpdate();
		}

		private void UpdateHexView(object sender)
		{
			PacketDefineControl pdc = sender as PacketDefineControl;
			if (pdc != null)
			{
				hexViewBox.ForeColor = Color.DarkGray;
				byte[] packet;
				try
				{
					packet = pdc.GetPacket();
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


        bool _livePreviewChecked = true;
        bool _livePreviewInContext = true;
        private void QueueLivePreviewUpdate()
		{
			_tokenSource?.Cancel();
			_tokenSource = new CancellationTokenSource();
			livePreviewTextBox.Text = string.Empty;

			// Assert live preview is on
			if (_livePreviewChecked)
			{
				_timer.Change(_delayMs, Timeout.Infinite);
			}

		}

		private void UpdateLivePreview(object state)
		{
			if (!Created) return;

			Invoke((Action)(() =>
			{
				livePrevStatusPanel.BackgroundColor = new BrushInfo(Color.Gray);
				var tabPage = tabControl.SelectedTab;
				PacketDefineControl pdc = tabPage.Controls[0] as PacketDefineControl;
				if (pdc == null)
				{
					livePreviewTextBox.Text = "Could not find packet define control...";
					return;
				}
				if (String.IsNullOrWhiteSpace(pdc.Text))
				{
					livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Vertical, badGradient);
					livePreviewTextBox.Text = "Packet contains no bytes.";
					return;
				}

				byte[] packet;
				try
				{
					packet = pdc.GetPacket();
				}
				catch (Exception ex)
				{
					livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Vertical, badGradient);
					livePreviewTextBox.Text = "Invalid packet. " + ex.Message;
					return;
				}



				string wsPath = Settings.Default.lastWiresharkPath.Trim('"'); // removing any leading or trailing quotes
				string tsharkPath = Path.GetDirectoryName(wsPath) + @"\tshark.exe";

				if (!File.Exists(tsharkPath))
				{
					livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Horizontal, badGradient);
					livePreviewTextBox.Text = $"Can't find TShark at:\r\n{tsharkPath}";
					return;
				}
				
				livePreviewTextBox.Text = $"Working...";

				IEnumerable<byte[]> packets;
				int packetIndex;
				if (_livePreviewInContext)
				{
					try
					{
						packets = GetAllDefinedPackets();
					}
					catch (Exception ex)
					{
						livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Vertical, badGradient);
						livePreviewTextBox.Text = ex.Message;
						return;
					}
					packetIndex = tabControl.SelectedIndex;
				}
				else
				{
					packets = new[] { packet };
					packetIndex = 0;
				}
				var _token = _tokenSource.Token;
				var tsharkTask = _tshark.GetPdmlAndJsonAsync(packets, packetIndex, _token);
				
				tsharkTask.ContinueWith((task) =>
				{
					packets = null; // Hoping GC will get the que
					ContinueTsharkTask(task, _token);
				}, _tokenSource.Token);
			}));
		}

		private void ContinueTsharkTask(Task<TSharkCombinedResults> tsharkTask, CancellationToken token)
		{
			if (token.IsCancellationRequested)
				return;
			livePreviewTextBox.Invoke((Action) (() =>
			{

				bool suspicious_flag = false;
				int last_index = 0;

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
					livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Vertical, badGradient);
					livePreviewTextBox.Text = tsharkTask.Exception?.Flatten().Message ?? "Unknown errors";
					return;
				}

				if (token.IsCancellationRequested)
				{
					// We got cancelled while checking the task results...
					return;
				}

				livePrevStatusPanel.BackgroundColor = new BrushInfo(GradientStyle.Vertical, goodGradient);

				// Get the parsed TShark's respose, base of proto tree
				TSharkCombinedResults results = tsharkTask.Result;
				XElement packetElement = results.Pdml;
				JObject packetJson = results.JsonRaw;
				IEnumerable<JProperty> jProps = packetJson.DescendantsAndSelf().Where(jtoken => jtoken is JProperty).Cast<JProperty>();
				List<JProperty> jPropsListed = null;

				TreeView tree = packetTreeView;

				// Get all CURRENTLY expanded nodes in the tree (from the OLD packet)
				IEnumerable<TreeNode> topNodes = tree.Nodes.OfType<TreeNode>();
				IEnumerable<TreeNode> nextGroup = topNodes;
				List<TreeNode> allNodes = new List<TreeNode>();
				allNodes.AddRange(nextGroup);
				bool moreFound = true;
				while (moreFound)
				{
					moreFound = false;
					var currentGroupCopy = nextGroup;
					nextGroup = new List<TreeNode>();
					foreach (var node in currentGroupCopy)
					{
						List<TreeNode> subNodes = node.Nodes.OfType<TreeNode>().ToList();
						nextGroup = nextGroup.Concat(subNodes);
						allNodes = allNodes.Concat(subNodes).ToList();
						moreFound |= subNodes.Any();
					}
				}
				// Some fields start with a bitmaps instead of the actual name, we try and skip the bitmask
				// e.g. ..01 1010 Packet Type: 0x1A
				// This LINQ also cuts the field name (removing bitmaps and value)
				// e.g. "Src Port: 2442" ----> "Src Port"
				char[] suspiciousChars = new char[] {'.', '0', '1'};
				var expandedNodes = (from node in allNodes
					where node.IsExpanded
					where node.Text.Any() // No nodes with empty names pl0x
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
					var next = stack.Dequeue();
					XContainer asContainer = next.Item2;
					foreach (XElement sub in asContainer.Elements())
					{
						XElement nextSub = sub;
						TreeNode nextNode = null;
						// Checl if this node hidden in wireshark's GUI
						var hideAttr = sub.Attribute(XName.Get("hide"));
						if (hideAttr != null && hideAttr.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
							continue;

						// Get 'Show name' and 'name'
						// 'Show name' should be the text displayed in wireshark's tree. We want to show that as well (e.g. "User Datagram Protocol, Src Port: 4, Dst Port: 5")
						// Fallback - Use 'name' which is the wireshark filter (e.g. 'udp.srcport')
						string showName = sub.Attribute(XName.Get("showname"))?.Value ??
						                  sub.Attribute(XName.Get("name"))?.Value ?? "UNKNOWN FIELD!";
						string name = sub.Attribute(XName.Get("name"))?.Value ?? "UNKNOWN FIELD!";

						if (showName == "data" && sub.Name == "field" && next.Item1 == null)
						{
							// Get the bytes count from the inner node called 'data' as well
							string bytesCount = ((XElement) sub.FirstNode).Attribute(XName.Get("size"))?.Value ?? "0";
							showName = $"Data ({bytesCount} bytes)"; // replace 'fale-field'wrapper'
							nextSub = sub as XElement;
							sub.Name = "proto"; // Overriding if set to 'field' so it will be added to the root
						}

						// SPECIAL CASE: No showname/filter - happens in some expert nodes/asn.1 nodes
						if (String.IsNullOrWhiteSpace(showName) || showName == "fake-field-wrapper")
						{
							// We add the childrens to the stack and indicating the parent is THIS NODE's parent (essentially skipping this node)
							stack.Enqueue(new Tuple<TreeNode, XElement>(next.Item1, sub));
							continue;
						}

						// Create the new tree node
						nextNode = new TreeNode(showName);
						nextNode.Name = name;

						// Hide position + size in HEX within the 'tool tip text'
						// Format is: *startIndex*,*length*,*suspicious_flag*
						// (both in bytes)
						string index = sub.Attribute(XName.Get("pos"))?.Value ?? "0";
						string length = sub.Attribute(XName.Get("size"))?.Value ?? "0";
						string susFlagStr = string.Empty;
						// Check if this node changes our suspicious state
						int curr_index = int.Parse(index);
						if (curr_index < last_index && sub.Name == "proto")
						{
							if (name != "_ws.malformed") // Special case - Malformed 'protocol' seems to casually ruin everything good.
							{
								// Change for this node and ANY OTHER FOLLOWING NOED
								suspicious_flag = true;
							}
						}
						last_index = curr_index;

						if (suspicious_flag)
						{
							if (jPropsListed == null)
							{
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
								// Digging further into the object if we have several arrays as a value (happends when the same field appears multiple times in the same packet)
								bool hasInnerArray = propValue.Children().All(jtoken => jtoken is JArray);
								bool anyLeft = true;
								if (hasInnerArray)
								{
									var temp = propValue.ElementAt(0) as JArray;
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
									susFlagStr = "1,"+hex;
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
							// Special special case: Malformed Packet is found in a 'proto' tag as well
							if (showName.Contains("Malformed Packet"))
							{
								// But malform labels should be pink!
								nextNode.BackColor = ERROR_PINK;
							}
							// Add as a 'lowest level' node to the tree
							tree.Nodes.Add(nextNode);
						}
						else // Any other field
						{
							// Append to the last node
							next.Item1.Nodes.Add(nextNode);
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

				livePreviewTextBox.Text = "Done.";
				GC.Collect();
			}));
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lastSelectedPage != null && !tabControl.SelectedTab.Equals(_lastSelectedPage))
			{
				_lastSelectedPage.Controls.Clear();
			}
			_lastSelectedPage = tabControl.SelectedTab;

			if (tabControl.SelectedTab.IsPlusTab())
			{
				// The selected tab is the special 'Plus Tab' - we should create a new packet
				TabPage plusTab = tabControl.SelectedTab;
				tabControl.TabPages.Remove(plusTab);
				TabPage newPage = AddNewTab(null);
				tabControl.TabPages.Add(plusTab);
				tabControl.SelectedTab = newPage;
				tabControl_SelectedIndexChanged(null, null);
			}
			else
			{
				// The selected tav is a reular one, add PDC
				TabPage tab = tabControl.SelectedTab;
				
				if (tab == null)
					return;
				PacketDefineControl pdc;
				if (!_tabToPdc.TryGetValue(tab, out pdc))
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
			Pdc_ContentChanged(null, null);
		}

		private void tabControl_MouseClick(object sender, MouseEventArgs e)
		{
			var tabs = tabControl.TabPages;
			
			// Getting tab control which was clicked bsaed on the mouse's location
			var pointedTab = tabs.Cast<TabPage>()
				.Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location))
				.FirstOrDefault();

			if(pointedTab == null)
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
				if (tabControl.SelectedTab == pointedTab && pointedTab?.TabIndex > 1)
				{
					tabControl.SelectedIndex = pointedTab.TabIndex - 1;
				}
				tabs.Remove(pointedTab);

				// Update packet list to remove this tab
				UpdatePacketListBox();
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

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult res = PromptUserToSave();

			if (res == DialogResult.OK)
			{
				_unsavedChangesExist = false;
			}
		}

		private void SaveAsB2p(string path)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("P0NTI4K");
			foreach (TabPage tab in tabControl.TabPages)
			{
				foreach (var control in tab.Controls)
				{
					PacketDefineControl casted = control as PacketDefineControl;
					if (casted != null)
					{
						PacketSaveDataV2 psd = casted.GetSaveData() as PacketSaveDataV2;
						sb.AppendLine(psd.ToString());
					}
				}
			}

			var outputFile = File.OpenWrite(path);
			StreamWriter sw = new StreamWriter(outputFile);
			sw.Write(sb.ToString());
			sw.Dispose();
		}

        private void SaveAsPcap(string path)
		{
			List<byte[]> packets = GetAllDefinedPackets();
            string tempFilePath = _tps.WritePackets(packets);
			File.Move(tempFilePath,path);
		}

		private DialogResult PromptUserToSave()
		{
			Dictionary<int, Action<string>> _filterIndexToSaveFunc = new Dictionary<int, Action<string>>()
			{
				{1,SaveAsB2p},
				{2,SaveAsPcap},
			};

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "ByteArrayToPcap file|*.b2p|pcap file|*.pcap";
			var res = sfd.ShowDialog();

			if (res != DialogResult.Yes && res != DialogResult.OK)
			{
				// If the user managed to respong with anythign other then 'Yes' or 'Ok' this is practicly a cancel
				return DialogResult.Cancel;
			}


			bool anyPacketsFound = false;
			foreach (TabPage tab in tabControl.TabPages)
			{
				foreach (var control in tab.Controls)
				{
					PacketDefineControl casted = control as PacketDefineControl;
					if (casted != null)
					{
						anyPacketsFound = true;
					}
				}
			}

			if (!anyPacketsFound)
			{
				MessageBox.Show("No packets to save", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.OK;
			}

			Action<string> saveFunc = _filterIndexToSaveFunc[sfd.FilterIndex];
			saveFunc(sfd.FileName);

			return DialogResult.OK;
		}

		private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "All Capture Files|*.b2p;*.pcap;*.pcapng|" +
						 "ByteArrayToPcap file (*.b2p)|*.b2p|" +
						 "Wireshark Capture files (*.pcap,*.pcapng)|*.pcap;*.pcapng";
			var res = ofd.ShowDialog();

			if (res != DialogResult.Yes && res != DialogResult.OK)
			{
				return;
			}

			LoadFile(ofd.FileName);
			_unsavedChangesExist = false;
		}



		public void LoadFile(string path)
		{
			if (!File.Exists(path))
			{
				MessageBox.Show($"No such file '{path}'", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}

			IPacketsProvider provider = null;
			string ext = Path.GetExtension(path);
			switch (ext)
			{
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
					MessageBox.Show($"Unsupported extension '{ext}'", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
			}

			if (provider != null)
			{
				LoadPackets(provider);
				Pdc_ContentChanged(null, null);
			}
		}

		private bool _noDraw = false;

		private void LoadPackets(IPacketsProvider provider)
		{
			_noDraw = true;


			tabControl.SelectedIndexChanged -= tabControl_SelectedIndexChanged;

			TabPage localPlusTab = null;

			List<TabPage> toRemove = new List<TabPage>();
			foreach (TabPage tabPage in tabControl.TabPages)
			{
				if (tabPage.IsPlusTab())
				{
					localPlusTab = tabPage;

				}
				toRemove.Add(tabPage);
			}

			foreach (TabPage tabPage in toRemove)
			{
				tabControl.TabPages.Remove(tabPage);
			}

			_nextPacketTabNumber = 1;

			try
			{
				foreach (PacketSaveData packet in provider)
				{
					AddNewTab(packet);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error reading packets from file.\r\nError Message: {ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			finally
			{
				provider.Dispose();
			}


			if (localPlusTab != null)
			{
				tabControl.TabPages.Add(localPlusTab);
			}

			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			tabControl_SelectedIndexChanged(null,null);

			_noDraw = false;
			UpdatePacketListBox();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && _askAboutUnsaved)
			{
				// Check if we even have packets to save (or if the GUI is empty)
				bool anyPacketsFound = false;
				foreach (TabPage tab in tabControl.TabPages)
				{
					foreach (var control in tab.Controls)
					{
						PacketDefineControl casted = control as PacketDefineControl;
						if (casted != null)
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
					var res = MessageBox.Show(this, $"Want to save your changes?", Text, MessageBoxButtons.YesNoCancel);
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

		private void askToSaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_askAboutUnsaved = !_askAboutUnsaved;
			Settings.Default.lastAskToSaveSetting = _askAboutUnsaved;
			Settings.Default.Save();
			//askToSaveToolStripMenuItem.Checked = _askAboutUnsaved;
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_unsavedChangesExist)
			{
				var res = MessageBox.Show(this, $"Want to save your changes?", Text, MessageBoxButtons.YesNoCancel);
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

			TabPage localPlusTab = null;

			List<TabPage> toRemove = new List<TabPage>();
			foreach (TabPage tabPage in tabControl.TabPages)
			{
				if (tabPage.IsPlusTab())
				{
					localPlusTab = tabPage;

				}
				toRemove.Add(tabPage);
			}

			tabControl.SelectedIndexChanged -= tabControl_SelectedIndexChanged;

			foreach (TabPage tabPage in toRemove)
			{
				tabControl.TabPages.Remove(tabPage);
			}

			_nextPacketTabNumber = 1;

			tabControl.TabPages.Add(localPlusTab);

			// Adding new , alligned, "packet 1" tab
			tabControl_SelectedIndexChanged(null, null);

			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
		}

		private void livePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newValue = !_livePreviewChecked;
            _livePreviewChecked = newValue;
            previewtoolStripButton.Checked = newValue;
            Settings.Default.livePreviewActive = newValue;
            Settings.Default.Save();
            //Also update the newly shown tree with the current packet's data

            QueueLivePreviewUpdate();
		}
		int _delayMs;

		private void livePreviewDelayBox_TextChanged(object sender, EventArgs e)
		{
			int newVal;
			if (int.TryParse(livePrevToolStripTextBox.Text, out newVal))
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

		private void packetTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string rawInstructions = packetTreeView.SelectedNode.ToolTipText;
			string[] split = rawInstructions.Split(',');
			int index; // in BYTES!
			int length; // in BYTES!
			int susFlagValue;
			bool suspicious = false;
			string hex = null;
			if (split.Length < 2 || !int.TryParse(split[0], out index) || !int.TryParse(split[1], out length)|| !int.TryParse(split[2], out susFlagValue))
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
			var tab = tabControl.SelectedTab;
			var pdc = tab.Controls.OfType<PacketDefineControl>().First();
			pdc.SetSelection(0,0); // Clear any old Selection

			if (!pdc.IsHexStream)
			{
				livePreviewTextBox.Text = "Bytes highlighting is only supported for 'clean' hex streams (no offsets, ASCII represenation, 0x's, spaces, etc)";
				return;
			}

			if (!suspicious || hex == null)
			{
				pdc.SetSelection(index*2, length);
				packetTreeView.Select();
			}
			else
			{
				var inputHex = String.Join("", pdc.GetPacket().Select(x => x.ToString("x2")));
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
					string candidate2 = inputHex.Substring(nibbleIndex+1, nibblesCountExpected);
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
						hexViewBox.Select(realIndex/2, length + 1);
					}
				}
			}
		}

		private void previewInBatPContextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var newVal = !_livePreviewInContext;
            _livePreviewInContext = newVal;
			previewContextToolStripButton.Checked = newVal;
			Settings.Default.livePreviewInContext = newVal;
			Settings.Default.Save();
			QueueLivePreviewUpdate();
		}

		private void packetTreeView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var node = packetTreeView.GetNodeAt(e.Location);
				if (node == null)
					return;

				packetTreeView.SelectedNode = node;
				
				ContextMenuStrip cms = new ContextMenuStrip();
				ExpandingToolStripButton tsb = new ExpandingToolStripButton("Exapnd Subtrees");
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
				var pref = tsb.GetPreferredSize(packetTreeView.Size);
				pref.Width = tsb.Width;
				cms.MinimumSize = pref;
				cms.Size = pref;
				cms.ShowCheckMargin = false;
				cms.ShowImageMargin = false;
				cms.Show(packetTreeView, e.Location);
			}
		}
		private void copyForCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var pdc = tabControl.SelectedTab.Controls.Cast<Control>().Single(c => c is PacketDefineControl) as PacketDefineControl;
			byte[] data;
			try
			{
				data = pdc.GetPacket();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error getting the current packet's data.\r\n{ex.Message}", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
			}
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("new byte[]");
			sb.AppendLine("{");
			IEnumerable<string> with0xs = data.Select(b => $"0x{b:X2}");
			sb.AppendLine(string.Join(",", with0xs));
			sb.AppendLine("};");
			Clipboard.SetText(sb.ToString());
		}

		private void hexViewBox_Copied(object sender, EventArgs e)
		{
			var hexBox = sender as HexBox;
			hexBox?.CopyHex();
		}

		private void locateWireshark_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Wireshark.exe|Wireshark.exe";
			try
			{
				ofd.InitialDirectory = Path.GetDirectoryName(Settings.Default.lastWiresharkPath);
			}
			catch
			{
				// IDK...
			}
			var res = ofd.ShowDialog();
			if (res == DialogResult.OK)
			{
				if (ofd.CheckFileExists)
				{
					string dirPath = Path.GetDirectoryName(ofd.FileName);
					WiresharkDirectory wsDir;
					if (SharksFinder.TryGetByPath(dirPath, out wsDir))
					{
						Settings.Default.lastWiresharkPath = ofd.FileName;
						Settings.Default.Save();
						_wireshark = new WiresharkInterop(wsDir.WiresharkPath);
						_tshark = new TSharkInterop(wsDir.TsharkPath);
					}
				}
				else if (_isConstructing)
				{
					// The user is fucking with us, giving us paths which does not exist
					// since we are in the ctor, we don't have a WS path to work with so we f*cking quit
					Environment.Exit(-1);
				}
			}
			else if (_isConstructing)
			{
				// We were called form the ctor, meaning no WS path is currently saved.
				// since the user cancelled the dialog, we don't have a WS path to work with so we f*cking quit
				Environment.Exit(-1);
			}
		}

		private TabPage _plusTab = null;
		private Rectangle _rectangle = Rectangle.Empty;
		private void tabControl_DragEnter(object sender, DragEventArgs e)
		{
			if (_plusTab != null)
			{
				return;
			}

			_plusTab = tabControl.TabPages.OfType<TabPage>().SingleOrDefault(page => page.IsPlusTab());
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


		private void tabControl_DragDrop(object sender, DragEventArgs e)
		{
			if (_plusTab != null)
			{
				tabControl.TabPages.Add(_plusTab);
				_plusTab = null;
			}
			// Update packet list to represent new order after tab dragging is finished
			UpdatePacketListBox();
		}

	    private void packetTabsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPacketListItem tpli = packetTabsList.SelectedItem as TabPacketListItem;
			if (tpli != null)
			{
				tabControl.SelectTab(tpli.Page);
			}
		}

		private bool _interceptingKeyDown = false;

		private void packetTreeView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Right)
			{
				var selectedNode = packetTreeView.SelectedNode;
				if (selectedNode != null)
				{
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
			}
		}

		private void packetTreeView_KeyDown(object sender, KeyEventArgs e)
		{
			if (_interceptingKeyDown)
			{
				e.Handled = true;
			}
			_interceptingKeyDown = false;
		}

		private void packetTabsList_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TabPacketListItem pointedItem = packetTabsList.Items.Cast<TabPacketListItem>()
					.Where((lvi, i) => packetTabsList.GetItemRectangle(i).Contains(e.Location))
					.FirstOrDefault();
				if (pointedItem != null)
				{
					packetTabsList.SelectedItem = pointedItem;
					_tabRequestingRename = pointedItem?.Page;
					pointedItem?.Page.ContextMenu.Show(packetTabsList, e.Location);
				}
			}
		}

		private void packetDefineControl1_Load(object sender, EventArgs e)
		{

		}
	}
}