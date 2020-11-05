using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using Syncfusion.SfSkinManager;
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
        public static ViewModel TabControlViewModel;

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


        public MainWindow()
        {
            SfSkinManager.SetTheme(this, new Theme("MaterialDarkBlue"));
            SfSkinManager.ApplyStylesOnApplication = true;
            

            InitializeComponent();

            if (false)
            {
                tabControl.Items.Clear(); // Remove "Packet 1"
                AddNewPacketTab(null, null); // Re-add "Packet 1"
            }

            // TODO: Replace with getting the value from config and using "LivePreviewDelay = *configValue*"
            ApplyNewPreviewDelayValue();

            hexEditor.Stream = new MemoryStream(new byte[] { 0xaa, 0xbb, 0xcc });


        }

        private void AddNewPacketTab(object sender, EventArgs e)
        {
            TabControlWrapper mnw = new TabControlWrapper();

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
            panel.Children.Add(mnw);
            pd.PacketChanged += PacketDefinerPacketChanged;
            TabItemExt newTab = new TabItemExt()
            {
                Header = $"Packet {_packetsCounter}",
                Margin = new Thickness(1),
                Padding = new Thickness(0, 0, 5, 0),
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
            PacketDefiner definer = ((PacketDefiner)sender);
            if (definer.IsValid)
            {
                // Update HEX view
                byte[] bytes = definer.PacketBytes.Data;


                // Some WPF hacking is going on here:
                // I want to update hexEditor's "Stream" to the packet's bytes
                // setting this property gives hexEditor focus
                // I couldn't get the focus back to the sender (Main textbox, stream textbox, etc...)
                // no matter what I tried.
                // To solve that I hide & unhide the hexeditor
                // this causes FLICKERING of the editors sometimes
                // to stop that from happening I disable the Dispatcher's processing and then reactivate (end of 'using')
                using (var d = Dispatcher.DisableProcessing())
                {
                    hexEditor.Visibility = Visibility.Hidden;
                    hexEditor.Stream = new MemoryStream(bytes);
                    hexEditor.Visibility = Visibility.Visible;
                }
                hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                hexEditor.ForegroundSecondColor = hexEditor.Foreground;

                // Call Live Update
                if (previewEnabledCheckbox.IsChecked == true)
                {
                    Task.Delay(_livePreviewDelay).ContinueWith(task => ShowLivePreview());
                }
            }
            else
            {
                // Code below requierd BeginInvoke otherwise
                // the caret in the PacketDefiner textbox might not update
                // (if the textbox triggered this event)
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
                 {
                     hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(135, 135, 135));
                     hexEditor.ForegroundSecondColor = hexEditor.Foreground;
                 }));
                return;
            }
        }

        TSharkInterop _tSharkInterop = new TSharkInterop(@"C:\Program Files\Wireshark\tshark.exe");

        private void ShowLivePreview()
        {
            TempPacketSaveData packetBytes = null;
            AutoResetEvent are = new AutoResetEvent(false);
            this.Dispatcher.Invoke(() =>
            {
                packetBytes = CurrentShowingDefiner.PacketBytes;
                are.Set();
            });
            are.WaitOne();
            if (packetBytes == null || packetBytes.Data.Length == 0)
                return;
            XElement a = _tSharkInterop.GetPdmlAsync(packetBytes).Result;
            packetTreeView.PopulateLivePreview(a);
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
                byte[] packet = CurrentShowingDefiner.PacketBytes.Data;
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
                byte[] packet = CurrentShowingDefiner.PacketBytes.Data;
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
