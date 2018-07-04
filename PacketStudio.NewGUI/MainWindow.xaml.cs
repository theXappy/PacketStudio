using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using PacketStudio.Core;
using Syncfusion.Windows.Tools.Controls;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace ByteArrayToPcap.NewGUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RibbonWindow
	{
		private PacketDefiner CurrentShowingDefiner => (((tabControl.SelectedItem as TabItemExt)?.Content as DockPanel)?.Children?[0] as PacketDefiner) ?? null;


		private static int _packetsCounter = 0;
		private int _livePreviewDelay;
		private int LivePreviewDelay
		{
			get => _livePreviewDelay;
			set
			{
				_livePreviewDelay = value;
				if (previewDelayButton != null)
				{
					previewDelayButton.Label = "Preview Delay: " + LivePreviewDelay;
				}
			}
		}

		private Brush _protoBrush = new SolidColorBrush(Color.FromRgb(240, 240, 240));
		private Brush _fieldBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

		public MainWindow()
		{
			InitializeComponent();

			tabControl.Items.Clear(); // Remove "Packet 1"
			AddNewPacketTab(null, null); // Re-add "Packet 1"

			// TODO: Replace with getting the value from config and using "LivePreviewDelay = *configValue*"
			ApplyNewPreviewDelayValue();
		}

		private void AddNewPacketTab(object sender, EventArgs e)
		{
			_packetsCounter++;
			DockPanel panel = new DockPanel()
			{
				LastChildFill = true
			};
			PacketDefiner pd = new PacketDefiner()
			{
				Margin = new Thickness(5),
				Width = Double.NaN, // This means 'Auto'
				Height = Double.NaN // This means 'Auto'
			};
			panel.Children.Add(pd);
			pd.PacketChanged += PacketDefinerPacketChanged;
			TabItemExt newTab = new TabItemExt()
			{
				Header = $"Packet {_packetsCounter}",
				Margin = new Thickness(1),
				Content = panel
			};
			newTab.MouseDown += PacketTab_MouseDown;

			tabControl.Items.Add(newTab);

			if (tabControl.Items.Count > 1)
			{
				tabControl.CloseButtonType = CloseButtonType.Individual;
			}
		}

		private void PacketDefinerPacketChanged(object sender, EventArgs e)
		{
			var definer = ((PacketDefiner)sender);
			if (definer.IsValid)
			{
				// Update HEX view
				byte[] bytes = definer.PacketBytes;
				var converter = new ByteArrayToIndexedHexStringConverter();
				string str = converter.Convert(bytes, typeof(string), "16" /*bytes per line */, CultureInfo.CurrentCulture) as string;
				hexBox.Text = str;
				hexBox.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
			}
			else
			{
				hexBox.Foreground = new SolidColorBrush(Color.FromRgb(135, 135, 135));
			}

			// Call Live Update
			if (previewEnabledCheckbox.IsChecked == true && definer.IsValid)
			{
				Task.Delay(_livePreviewDelay).ContinueWith(task => ShowLivePreview());
			}
		}

		TSharkInterop _tSharkInterop = new TSharkInterop(@"C:\Program Files\Wireshark\tshark.exe");

		private void ShowLivePreview()
		{
			byte[] packetBytes = null;
			AutoResetEvent are = new AutoResetEvent(false);
			this.Dispatcher.Invoke(() =>
			{
				packetBytes = CurrentShowingDefiner.PacketBytes;
				are.Set();
			});
			are.WaitOne();
			if(packetBytes.Length == 0)
				return;
			XElement a = _tSharkInterop.GetPdmlAsync(packetBytes).Result;
			PopulateLivePreview(a);
		}

		private void PopulateLivePreview(XElement rootElement)
		{
			this.Dispatcher.Invoke((Action)(() =>
			{
				// Clean up
				previewTree.Items.Clear();

				//
				Dictionary<XElement, TreeViewItem> elementNodes = new Dictionary<XElement, TreeViewItem>();
				var initialNodes = rootElement.Nodes().Where(node => node is XElement).Cast<XElement>();
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
					string bytesIndexStr = nextElem.GetAttribute("pos")?.Value ?? "0";
					int bytesIndex = 0;
					int.TryParse(bytesIndexStr, out bytesIndex);
					string bytesLengthStr = nextElem.GetAttribute("size")?.Value ?? "0";
					int bytesLength = 0;
					int.TryParse(bytesLengthStr, out bytesLength);
					nextNode.ToolTip = new BytesHiglightning(bytesIndex,bytesLength); 
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

			// Making sure definer is in hex stream mode (all hex, no sapces/tabs/other junk)
			if (!CurrentShowingDefiner.IsHexStream)
				return;


			// TODO:
			//hexBox.Select(bytesOffsetForHexBox, numOfBytesForHexBox);
			bool selected = CurrentShowingDefiner.TrySelect(bh.Offset, bh.Length);
			this.Dispatcher.BeginInvoke((Action)(()=>{
				//hexBox.Focus();
				CurrentShowingDefiner.hexTextBox.Focus();
				tvi.Focus();
			}),new object[0]);
			
		}

		private void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			e.Handled = true;
		}

		private void PacketTab_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Allowing users to close tabs using the middle mouse button

			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				if (tabControl.Items.Count == 1) // User is trying to remove the last tab
					return; // Don't let them

				// Else, More packet tabs exist. we can remove this one
				tabControl.Items.Remove(sender);
				tabControl_TabClosed(tabControl, new CloseTabEventArgs(sender as TabItemExt));
			}
		}


		private void tabControl_TabClosed(object sender, CloseTabEventArgs e)
		{
			if (tabControl.Items.Count == 1)
			{
				// We are closing the 1 before last tab
				// So we hide the 'close' button on the last tab
				// ( So the user can't stay with 0 packet tabs)
				tabControl.CloseButtonType = CloseButtonType.Hide;
			}

		}

		// This method copies the current showing packet to the clipboard
		// The format of the data will be '0xAA, 0xBB'
		private void CopyCSharpShort(object sender, RoutedEventArgs e)
		{
			bool packetValid = AssertCurrentPacketValid();

			if (packetValid)
			{
				byte[] packet = CurrentShowingDefiner.PacketBytes;
				IEnumerable<string> bytesAsStrings = packet.Select(b => "0x" + b.ToString("X2"));
				string combinedString = string.Join(", ", bytesAsStrings);
				Clipboard.SetText(combinedString);
			}

			copyForCSharpButton.IsDropDownOpen = false;
		}

		// This method copies the current showing packet to the clipboard
		// The format of the data will be 'new byte[]{0xAA, 0xBB};'
		private void CopyCSharpLong(object sender, RoutedEventArgs e)
		{
			bool packetValid = AssertCurrentPacketValid();

			if (packetValid)
			{
				byte[] packet = CurrentShowingDefiner.PacketBytes;
				IEnumerable<string> bytesAsStrings = packet.Select(b => "0x" + b.ToString("X2"));
				string combinedString = "new byte[]{" + string.Join(", ", bytesAsStrings) + "};";
				Clipboard.SetText(combinedString);
			}
			copyForCSharpButton.IsDropDownOpen = false;
		}

		/// <summary>
		/// Asserts the current showing packet is valid. Otherwise shows an error to the user
		/// </summary>
		/// <returns>True if the packet is valid, false otherwise</returns>
		private bool AssertCurrentPacketValid()
		{
			PacketDefiner currenDefiner = CurrentShowingDefiner;
			if (currenDefiner == null)
			{
				MessageBox.Show("Couldn't find current packet definer control.");
				return false;
			}

			if (!currenDefiner.IsValid)
			{
				string error = currenDefiner.ValidationError;
				MessageBox.Show("Error in the packet definer: \r\n" + error);
				return false;
			}
			return true;
		}

		private void HidePreviewDelayTextBoxShow()
		{
			previewDelayTextBox.Visibility = Visibility.Collapsed;
			previewDelayButton.Visibility = Visibility.Visible;
		}

		private void ShowPreviewDelayTextBox(object sender, RoutedEventArgs e)
		{
			previewDelayTextBox.Visibility = Visibility.Visible;
			previewDelayButton.Visibility = Visibility.Collapsed;
			previewDelayTextBox.Select(previewDelayTextBox.Text.Length, 0);
			previewDelayTextBox.Focus();
			previewDelayTextBox.ScrollToEnd();
		}

		private void previewDelayTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue == false) // - Keyboard focus is lost
			{
				ApplyNewPreviewDelayValue();
			}
		}

		private void previewDelayTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				ApplyNewPreviewDelayValue();
				e.Handled = true;
			}
			else if (e.Key == Key.Escape)
			{
				// Restoring last value
				previewDelayTextBox.Text = LivePreviewDelay.ToString();
				// Hiding textbox, showing button
				HidePreviewDelayTextBoxShow();
				e.Handled = true;
			}
		}

		private void ApplyNewPreviewDelayValue()
		{
			int parsedValue;
			string valueString = previewDelayTextBox.Text;
			if (!int.TryParse(valueString, out parsedValue))
			{
				MessageBox.Show("Invalid preview delay value. Couldn't parse to an integer.");
			}
			else
			{
				// Note: Setting the value below also updates the label of the 'preview delay button'
				LivePreviewDelay = parsedValue;
			}
			HidePreviewDelayTextBoxShow();

		}

		private void hexBox_LostFocus(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
		}

		private void RibbonButton_Click(object sender, RoutedEventArgs e)
		{
			CurrentShowingDefiner.NormalizeHex();
		}
	}
}
