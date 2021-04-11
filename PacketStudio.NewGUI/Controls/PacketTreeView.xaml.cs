using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace PacketStudio.NewGUI.Controls
{
    /// <summary>
    /// Interaction logic for PacketTreeView.xaml
    /// </summary>
    public partial class PacketTreeView : UserControl
    {
        public class PacketTreeSelectionChangedArgs : EventArgs
        {
            public BytesHighlightning BytesHiglightning { get; private set; }

            public PacketTreeSelectionChangedArgs(BytesHighlightning bytesHiglightning)
            {
                BytesHiglightning = bytesHiglightning;
            }
        }

        public event EventHandler<PacketTreeSelectionChangedArgs> SelectedItemChanged;

        public PacketTreeView()
        {
            InitializeComponent();
            // Remove placeholder item
            //previewTree.Items.Clear();
        }

        private readonly Brush _protoBrush = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        private readonly Brush _fieldBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));


        private readonly Dictionary<TreeViewItem, BytesHighlightning> _bytesHiglightnings = new Dictionary<TreeViewItem, BytesHighlightning>();



        public void PopulatePacketTree(XElement rootElement)
        {
            _bytesHiglightnings.Clear();
            this.Dispatcher.Invoke((Action)(() =>
            {
                // Clean up
                previewTree.Items.Clear();

                Dictionary<XElement, TreeViewItem> elementNodes = new Dictionary<XElement, TreeViewItem>();
                IEnumerable<XElement> initialNodes = rootElement.Nodes().Where(node => node is XElement).Cast<XElement>();
                Stack<XElement> elements = new Stack<XElement>(initialNodes);
                while (elements.Count > 0)
                {
                    XElement nextElem = elements.Pop();

                    // Check if this element hidden in wireshark, hide it as well
                    XAttribute hideAttr = nextElem.GetAttribute("hide");
                    if (hideAttr.ValueIs("yes"))
                        continue;

                    bool isGeneralInfo = nextElem.GetAttribute("showname").ValueIs("General information");
                    if (isGeneralInfo)
                        continue;

                    foreach (XNode subNode in nextElem.Nodes())
                    {
                        if (subNode is XElement)
                        {
                            elements.Push(subNode as XElement);
                        }
                    }

                    bool isFakeFieldWrapper = nextElem.GetAttribute("name").ValueIs("fake-field-wrapper");
                    if (isFakeFieldWrapper)
                        continue;

                    TreeViewItem parentNode = null;
                    string showname = nextElem.GetAttribute("showname")?.Value ?? nextElem.GetAttribute("name")?.Value ?? "UNKNOWN FIELD!";

                    bool isLowercaseData = showname.Equals("data", StringComparison.CurrentCulture); // Do NOT ignore case!
                    XElement lengthSubNode = nextElem
                        .ElementNodes().FirstOrDefault(element => element.GetAttribute("showname")?.Value.StartsWith("Length") == true);
                    bool containsLengthSubNode = lengthSubNode != null;
                    if (isLowercaseData && containsLengthSubNode)
                    {
                        string length = new string(lengthSubNode.GetAttribute("showname")?.Value?.SkipWhile(c => c != ' ')
                            .ToArray());
                        length = length.Trim();
                        showname = $"Data ({length} bytes)";
                    }


                    XElement parent = nextElem.Parent;
                    bool hasParent = (parent != null && elementNodes.TryGetValue(parent, out parentNode));

                    bool isProtocolNode = !hasParent;

                    TreeViewItem nextNode = new TreeViewItem
                    {
                        Header = showname,
                        Background = isProtocolNode ? _protoBrush : _fieldBrush,
                        Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                        ToolTip = null,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Left
                    };
                    string bytesIndexStr = nextElem.GetAttribute("pos")?.Value ?? "0";
                    int.TryParse(bytesIndexStr, out var bytesIndex);
                    string bytesLengthStr = nextElem.GetAttribute("size")?.Value ?? "0";
                    int.TryParse(bytesLengthStr, out var bytesLength);

                    // Save aside the bite highlighting instructions
                    BytesHighlightning bHighlightning = new BytesHighlightning(bytesIndex, bytesLength);
                    if (bytesLength == 0)
                    {
                        bHighlightning.Offset = 0;
                    }
                    _bytesHiglightnings[nextNode] = bHighlightning;

                    nextNode.Selected += TreeItem_Selected;

                    elementNodes[nextElem] = nextNode;
                    if (hasParent)
                    {
                        // Insert in parent's nodes list
                        parentNode.Items.Insert(0, nextNode);
                    }
                    else
                    {
                        // Insert at root level
                        previewTree.Items.Insert(0, nextNode);
                    }
                }
            }));

        }



        private void TreeItem_Selected(object sender, RoutedEventArgs routedEventArgs)
        {

            routedEventArgs.Handled = true;

            TreeViewItem tvi = sender as TreeViewItem;

            if (!_bytesHiglightnings.TryGetValue(tvi, out BytesHighlightning bHighlightning))
                return;

            SelectedItemChanged?.Invoke(this, new PacketTreeSelectionChangedArgs(bHighlightning));

            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                tvi.Focus();
            }), new object[0]);
        }

        private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

    }
}
