using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.Win32;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.DataAccess.Saver;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        // TODO: Load last/Load from available list in start up instead
        WiresharkDirectory _wiresharkDirectory ;
        private WiresharkInterop _wsInterOp;
        private TSharkInterop _tsharkInterOp;
        public WiresharkDirectory WiresharkDir
        {
            get => _wiresharkDirectory;
            set
            {
                _wiresharkDirectory = value;
                _wsInterOp = null;
                _tsharkInterOp = null;
                if (value != null) {
                    _wsInterOp = new WiresharkInterop(value.WiresharkPath);
                    _tsharkInterOp = new TSharkInterop(value.TsharkPath);
                }
            }
        }

        private readonly SessionState _sessionState = new SessionState();


        public static ViewModel TabControlViewModel;
        private TabItemViewModel CurrentTabItemModel => tabControl.SelectedItem as TabItemViewModel;

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
            InitializeComponent();

            WiresharkDir = new WiresharkDirectory(@"C:\Program Files\Wireshark\");

            // TODO: Replace with getting the value from config and using "LivePreviewDelay = *configValue*"
            ApplyNewPreviewDelayValue();

            hexEditor.Stream = new MemoryStream(new byte[0]);
        }

        private void AddNewPacketTab(object sender, EventArgs e)
        {
            TabControlViewModel?.AddNewPacket();
            _sessionState.HasUnsavedChanges = true;
        }

        private void UpdatePacketState(PacketDefiner invoker)
        {
            PacketDefiner definer = invoker;

            CurrentTabItemModel.SessionPacket = definer.SessionPacket;

            if (definer.IsValid)
            {
                // Update HEX view
                byte[] bytes = definer.ExportPacket.Data;


                // Some WPF hacking is going on here:
                // I want to update hexEditor's source (called "Stream") to the packet's bytes
                // setting this property gives hexEditor focus
                // I couldn't get the focus back to the sender (Main textbox, IP stream textbox, etc...)
                // no matter what I tried.
                // To solve that I hide & unhide the hexeditor
                // this, in turn, causes FLICKERING of the editors sometimes
                // to stop that from happening I disable the Dispatcher's processing and then reactivate (end of 'using')
                //
                // Next, I had an issue where doing so on the current invoking thread (it's probably the UI thread)
                // causes the cursor of the textbox act strange (updates only sometimes...).
                // To overcome this I 'enqueue' the update of the hexEditor after the UI thread finishes handling 
                // the current event. This is done by the 'BeginInvoke' call.
                Dispatcher.BeginInvoke(DispatcherPriority.Background,(Action)(() =>
                {
                    using (var d = Dispatcher.DisableProcessing())
                    {
                        hexEditor.Visibility = Visibility.Hidden;
                        try
                        {
                            hexEditor.Stream = new MemoryStream(bytes);
                        }
                        catch(InvalidOperationException)
                        {
                            // Don't care
                        }

                        hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        hexEditor.ForegroundSecondColor = hexEditor.Foreground;
                        hexEditor.Visibility = Visibility.Visible;
                    }
                }));

                // Update Tab's ViewModel
                CurrentTabItemModel.IsValid = true;
                CurrentTabItemModel.ExportPacket = definer.ExportPacket;

                // Call Live Update
                if (previewEnabledCheckbox.IsChecked == true)
                {
                    Task.Delay(_livePreviewDelay).ContinueWith(task => ShowLivePreview());
                }
            }
            else
            {
                CurrentTabItemModel.IsValid = false;
                CurrentTabItemModel.ExportPacket = null;

                // Code below requierd BeginInvoke otherwise
                // the caret in the PacketDefiner textbox might not update
                // (if the textbox triggered this event)
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action) (() =>
                {
                    using (var d = Dispatcher.DisableProcessing())
                    {
                        hexEditor.Visibility = Visibility.Hidden;
                        hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(135, 135, 135));
                        hexEditor.ForegroundSecondColor = hexEditor.Foreground;
                        hexEditor.Visibility = Visibility.Visible;
                    }
                }));
            }
        }

        private void PacketDefinerPacketChanged(object sender, EventArgs e)
        {
            if (_loading) return;

            UpdatePacketState(sender as PacketDefiner);
            _sessionState.HasUnsavedChanges = true;
        }


        private void SetPacketTreeInProgress()
        {
            Dispatcher.Invoke(() =>
            {
                packetTreeView.previewTree.Items.Clear();
                packetTreeView.previewTree.Items.Add("Loading...");
                var loadingColor = DockManager.Background;
                packetTreeView.previewTree.Background = loadingColor;
            });
        }

        private void UnsetPacketTreeInProgress()
        {
            Dispatcher.Invoke(() => packetTreeView.previewTree.Background = new SolidColorBrush(Colors.White));
        }

        private void ShowLivePreview()
        {
            SetPacketTreeInProgress();

            TempPacketSaveData packetBytes = null;
            Dispatcher.BeginInvoke((Action) (() => { packetBytes = CurrentTabItemModel.ExportPacket; })).Task
                .Wait();

            if (packetBytes != null && packetBytes.Data.Length != 0)
            {
                var tsharkTask = _tsharkInterOp.GetPdmlAsync(packetBytes);
                XElement packetPdml = tsharkTask.Result;
                packetTreeView.PopulatePacketTree(packetPdml);
            }

            UnsetPacketTreeInProgress();
        }


        // This method copies the current showing packet to the clipboard
        // The format of the data will be '0xAA, 0xBB'
        private void CopyCSharpShort(object sender, RoutedEventArgs e)
        {
            bool packetValid = AssertCurrentPacketValid();

            if (packetValid)
            {
                byte[] packet = CurrentTabItemModel.ExportPacket.Data;
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
                byte[] packet = CurrentTabItemModel.ExportPacket.Data;
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
            TabItemViewModel currItem = CurrentTabItemModel;
            if (currItem == null)
            {
                MessageBox.Show("Couldn't find current packet definer control.");
                return false;
            }

            if (!currItem.IsValid)
            {
                string error = currItem.ValidationError;
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

        private void PreviewDelayTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue == false) // - Keyboard focus is lost
            {
                ApplyNewPreviewDelayValue();
            }
        }

        private void PreviewDelayTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
            string valueString = previewDelayTextBox.Text;
            if (!int.TryParse(valueString, out var parsedValue))
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


        private void NormalizeHex(object sender, RoutedEventArgs e)
        {
            // TODO:
            //CurrentTabItemModel.NormalizeHex();
        }

        private void PacketTreeView_OnSelectedItemChanged(object sender,
            PacketTreeView.PacketTreeSelectionChangedArgs e)
        {
            if (e.BytesHiglightning == BytesHighlightning.Empty)
            {
                hexEditor.SelectionStop = -1;
                hexEditor.SelectionStart = -1;
            }
            else
            {
                hexEditor.SelectionStart = e.BytesHiglightning.Offset;
                hexEditor.SelectionStop = e.BytesHiglightning.Offset + e.BytesHiglightning.Length - 1;
            }
        }

        private void ExportToWireshark(object sender, RoutedEventArgs e)
        {
            var items = TabControlViewModel.TabItems;
            var invalidItems = items.Where(tabViewModel => !tabViewModel.IsValid);
            if (invalidItems.Any())
            {
                string invalidItemsNames =
                    string.Join(", ", invalidItems.Select(tabViewModel => $"\"{tabViewModel.Header}\""));
                string error = "Can not export to Wireshark because some tabs contain invalid packet definitions.\n\n" +
                               $"Invalid tabs: {invalidItemsNames}";
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<TempPacketSaveData> exportedPackets = items.Select(tabViewModel => tabViewModel.ExportPacket).ToList();
            TempPacketsSaver saver = new TempPacketsSaver();

            // TODO: Make this configurable
            var wsPath = SharksFinder.GetDirectories().First().WiresharkPath;
            WiresharkInterop wsInterop = new WiresharkInterop(wsPath);
            var wsSessionTask = wsInterop.ExportToWsAsync(exportedPackets);

            // When Wireshark exits we want to trigger a preview update because use might have changed preferences
            wsSessionTask.CliTask.Task.ContinueWith(_ =>
            {
                Debug.WriteLine($"@@@ Cont'ing interop task, Changed found: {wsSessionTask.PreferencesChanged}");
                if (wsSessionTask.PreferencesChanged)
                {
                    ShowLivePreview();
                }
            });
        }

        private bool _loading;

        private void OpenMenuItemClicked(object sender, MouseButtonEventArgs e)
        {
            if(_sessionState.HasUnsavedChanges)
            {
                // Prompt user about unsaved changes in current session
                var userSelection = MessageBox.Show("Unsaved Changes Alert",
                    "You have unsaved changes in the current session.\r\n" +
                    "\r\n" +
                    "Save changes?", MessageBoxButton.YesNoCancel);

                if (userSelection == MessageBoxResult.Cancel)
                {
                    // User wants to stay in current session
                    return;
                }

                if (userSelection == MessageBoxResult.Yes)
                {
                    // Save or Save As according to currently associated file
                    UserDecision finalSaveDecision = DoSave();

                    if (finalSaveDecision == UserDecision.Cancel)
                    {
                        // User cancelled while saving - interpret as "stay in current session"
                        return;
                    }
                }
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "All Capture Files|*.p2s;*.pcap;*.pcapng|" +
                         "PacketStudio 2 Session file (*.p2s)|*.p2s|" +
                         "Wireshark Capture files (*.pcap,*.pcapng)|*.pcap;*.pcapng"
            };
            var res = ofd.ShowDialog(this);
            if (res != true) return;

            string filePath = ofd.FileName;
            PacketsProvidersFactory ppf = new PacketsProvidersFactory();
            var provider = ppf.Create(ofd.FileName);
            using (Dispatcher.DisableProcessing())
            {
                _loading = true;
                TabControlViewModel.LoadFile(provider);
                _loading = false;
            }

            _sessionState.Reset();
            if (filePath.EndsWith(".p2s"))
            {
                // Only 'associating' session if opened a p2s file. pcap/pcapng aren't treated like that.
                _sessionState.AssociatedFilePath = ofd.FileName;
            }
        }

        // *************************************************
        //
        //                Saving to file
        //
        // *************************************************


        private UserDecision DoSave()
        {
            if (_sessionState.HasAssociatedFile)
            {
                if (_sessionState.HasUnsavedChanges)
                {
                    SaveSession(_sessionState.AssociatedFilePath);
                }
                // Else - Current state already saved to file, do nothing
                return UserDecision.Save;
            }

            // Not associated with a file - treat as "Save As..." (prompt for path)
            return DoSaveAs();
        }
        private UserDecision DoSaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "PacketStudio 2 Session file|*.p2s",
                DefaultExt = ".p2s"
            };
            var res = sfd.ShowDialog(this);
            if (!res.HasValue || res.Value == false)
                return UserDecision.Cancel;

            SaveSession(sfd.FileName);
            return UserDecision.Save;
        }

        private void SaveMenuItemClicked(object sender, MouseButtonEventArgs e) => DoSave();
        private void SaveAsMenuItemClicked(object sender, MouseButtonEventArgs e) => DoSaveAs();

        private void SaveSession(string path)
        {
            var sessionPackets = TabControlViewModel.TabItems.Select(model => model.SessionPacket);
            var p2SSaver = new P2sSaver();
            p2SSaver.Save(path, sessionPackets);

            _sessionState.AssociatedFilePath = path;
            _sessionState.HasUnsavedChanges = false;
        }


        private void ChangeWsDir(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Wireshark.exe|Wireshark.exe"
            };
            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(WiresharkDir.WiresharkPath);
            }
            catch
            {
                // IDK...
            }
            bool? res = ofd.ShowDialog();
            if (res != true) return;
            
            if (!ofd.CheckFileExists) return;

            string dirPath = Path.GetDirectoryName(ofd.FileName);
            if (SharksFinder.TryGetByPath(dirPath, out WiresharkDirectory wsDir))
            {
                string selectedPath = ofd.FileName;
                // Update settings

                // Raise list updated "event"
                WiresharkDir = wsDir;
            }
        }

        private void TabControl_OnOnCloseButtonClick(object sender, CloseTabEventArgs e)
        {
            // When closing last tab immediately open a new empty tab.
            int numTabs = tabControl.ItemsSource.OfType<object>().Count();
            Debug.WriteLine("Number of tabs: "+numTabs);
            if (numTabs == 1)
            {
                e.Cancel = true;
                TabControlViewModel.ResetItemsCollection();
            }
            _sessionState.HasUnsavedChanges = true;
        }

        private void TabControl_OnOnCloseAllTabs(object sender, CloseTabEventArgs e)
        {
            // Closing all tabs but making a new empty one.
            e.Cancel = true;
            TabControlViewModel.ResetItemsCollection();
            _sessionState.HasUnsavedChanges = true;
        }

        private void MenuButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if(!e.IsDown)
                return;
            if(e.Key != Key.Down && e.Key != Key.Up && 
               e.Key != Key.Enter && e.Key == Key.Return)
                return;

            MenuButton invokingButton = sender as MenuButton;
            Debug.WriteLine(e.Key);
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                switch (invokingButton.Name)
                {
                    case "newMenuButton":
                        NewSessionMenuItemClicked(sender,null);
                        break;
                    case "openMenuButton":
                        OpenMenuItemClicked(sender,null);
                        break;
                    case "saveMenuButton":
                        SaveMenuItemClicked(sender,null);
                        break;
                    case "saveAsMenuButton":
                        SaveAsMenuItemClicked(sender,null);
                        break;
                }

                e.Handled = true;
                return;
            }

            MenuButton next = null;
            if (e.Key == Key.Down)
            {
                next = _applicationMenu.Items[0] as MenuButton;
                for (int i = 0; i < _applicationMenu.Items.Count -1; i++)
                {
                    var candidate = _applicationMenu.Items[i] as MenuButton;
                    if (candidate == invokingButton)
                    {
                        next = _applicationMenu.Items[i + 1] as MenuButton;
                        break;
                    }
                }
            }
            else // Key ip UP
            {
                next = _applicationMenu.Items[_applicationMenu.Items.Count-1] as MenuButton;
                for (int i = 1; i < _applicationMenu.Items.Count; i++)
                {
                    var candidate = _applicationMenu.Items[i] as MenuButton;
                    if (candidate == invokingButton)
                    {
                        next = _applicationMenu.Items[i - 1] as MenuButton;
                        break;
                    }
                }
                
            }

            _applicationMenu.SelectedItem = next;
            next?.Focus();
            e.Handled = true;
        }

        private void NewSessionMenuItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (_sessionState.HasUnsavedChanges)
            {
                // Prompt user about unsaved changes in current session
                var userSelection = MessageBox.Show("Unsaved Changes Alert",
                    "You have unsaved changes in the current session.\r\n" +
                    "\r\n" +
                    "Save changes?", MessageBoxButton.YesNoCancel);

                if (userSelection == MessageBoxResult.Cancel)
                {
                    // User wants to stay in current session
                    return;
                }

                if (userSelection == MessageBoxResult.Yes)
                {
                    // Save or Save As according to currently associated file
                    UserDecision finalSaveDecision = DoSave();

                    if (finalSaveDecision == UserDecision.Cancel)
                    {
                        // User cancelled while saving - interpret as "stay in current session"
                        return;
                    }
                }
            }

            // Starting a new sessions!
            _sessionState.Reset();
            TabControlViewModel.ResetItemsCollection();
        }
    }

    internal enum UserDecision
    {
        Save,
        Cancel
    }
}
