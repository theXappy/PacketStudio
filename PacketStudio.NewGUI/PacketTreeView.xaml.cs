using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ByteArrayToPcap.NewGUI
{
    /// <summary>
    /// Interaction logic for PacketTreeView.xaml
    /// </summary>
    public partial class PacketTreeView : UserControl
    {
		public class PacketTreeSelectionChangedArgs : EventArgs
        {
            public BytesHiglightning BytesHiglightning { get; private set; }

            public PacketTreeSelectionChangedArgs(BytesHiglightning bytesHiglightning)
            {
                BytesHiglightning = bytesHiglightning;
            }
        }

        public EventHandler<PacketTreeSelectionChangedArgs> SelectedItemChanged;

        public PacketTreeView()
        {
            InitializeComponent();
        }

		private Brush _protoBrush = new SolidColorBrush(Color.FromRgb(240, 240, 240));
		private Brush _fieldBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));


        private Dictionary<TreeViewItem, BytesHiglightning> _bytesHiglightnings = new Dictionary<TreeViewItem, BytesHiglightning>();



		public void PopulateLivePreview(XElement rootElement)
		{
			_bytesHiglightnings.Clear();
			this.Dispatcher.Invoke((Action)(() =>
			{
				// Clean up
				previewTree.Items.Clear();

				//
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

					TreeViewItem nextNode = new TreeViewItem()
					{
						Header = showname,
						Background = isProtocolNode ? _protoBrush : _fieldBrush,
						Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0))
					};
                    nextNode.ToolTip = null;
					string bytesIndexStr = nextElem.GetAttribute("pos")?.Value ?? "0";
					int bytesIndex = 0;
					int.TryParse(bytesIndexStr, out bytesIndex);
					string bytesLengthStr = nextElem.GetAttribute("size")?.Value ?? "0";
					int bytesLength = 0;
					int.TryParse(bytesLengthStr, out bytesLength);
					_bytesHiglightnings[nextNode] = new BytesHiglightning(bytesIndex,bytesLength); 

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
			if (tvi == null)
				return;

			BytesHiglightning bh = tvi.ToolTip as BytesHiglightning;
			if(bh == null)
				return;

			SelectedItemChanged?.Invoke(this, new PacketTreeSelectionChangedArgs(bh));

			this.Dispatcher.BeginInvoke((Action)(()=>{
				tvi.Focus();
			}),new object[0]);
		}

		private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			e.Handled = true;
		}

    }
}
