using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;
using FastPcapng;
using log4net;
using log4net.Util;
using Microsoft.Win32;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.DataAccess.Saver;
using PacketStudio.NewGUI.Controls;
using PacketStudio.NewGUI.PacketTemplatesControls;
using PacketStudio.NewGUI.Properties;
using PacketStudio.NewGUI.ViewModels;
using PacketStudio.NewGUI.Windows;
using Syncfusion.Windows.Tools.Controls;

#pragma warning disable CA1416 // Validate platform compatibility

namespace PacketStudio.NewGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        const int MAX_RECENT_FILES_ENTRIES = 10;

        private readonly ILog _logger;
        
        private bool _fileLoading;

        WiresharkDirectory _wiresharkDirectory;
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
                if (value != null)
                {
                    _wsInterOp = new WiresharkInterop(value.WiresharkPath);
                    _tsharkInterOp = new TSharkInterop(value.TsharkPath);
                }
            }
        }

        private readonly SessionSaveState _sessionSaveState = new SessionSaveState();
        public static MainViewModel SessionViewModel { get; set; }
        private PacketDefiner CurrentPacketDefiner => theOneAndOnlyPd;

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

        private CancellationTokenSource _packetsListTokenSource = null;
        private object _packetListTokenSrouceLock = new object();

        public MainWindow()
        {
            // Init logger
            var configFile = new FileInfo("log4net.config");
            log4net.Config.XmlConfigurator.Configure(configFile);

            _logger = LogManager.GetLogger(typeof(MainWindow));
            _logger.Info("PacketStudio started! Inside MainWindow Ctor.");


            InitializeComponent();

            bool foundWsInSettings = SharksFinder.TryGetByPath(Settings.Default.WiresharkDir, out WiresharkDirectory dir);
            if (foundWsInSettings)
            {
                _logger.Info($"Wireshark directory found in settings. Dir: {dir.WiresharkPath}");

                // Found wireshark in settings.
                this.WiresharkDir = dir;
            }
            else
            {
                _logger.Info($"No Wireshark directory in settings. Prompting user to choose version.");

                // No wireshark in settings, prompt user to select a version
                WiresharkFinderWindow wfw = new WiresharkFinderWindow();
                bool? userChoseWiresharkVersion = wfw.ShowDialog();
                if (!userChoseWiresharkVersion.HasValue || !userChoseWiresharkVersion.Value)
                {
                    _logger.Info($"User canceled Wireshark version selection windows. Terminating!");
                    Environment.Exit(1);
                }

                var finderViewModel = wfw.DataContext as WiresharkFinderViewModel;
                if (finderViewModel == null)
                {
                    _logger.Error($"ViewModel of wireshark version window was null (or not {nameof(WiresharkFinderViewModel)}). Terminating!");
                    Environment.Exit(1);
                }

                WiresharkDir = finderViewModel.SelectedItem;
                _logger.Info($"User selected this Wireshark version: {WiresharkDir.WiresharkPath}");

                _logger.Info($"Saving user's choice to settings. Wireshark version: {WiresharkDir.WiresharkPath}");
                // Also save in settings for next runs
                Settings.Default.WiresharkDir = WiresharkDir.WiresharkPath;
                Settings.Default.Save();
                _logger.Info($"Saved Wireshark version to settings!");
            }

            // TODO: Replace with getting the value from config and using "LivePreviewDelay = *configValue*"
            _logger.Info($"Applying Preview delay value");
            ApplyNewPreviewDelayValue();
            _logger.Info($"Applied Preview delay value");

            hexEditor.Stream = new MemoryStream(Array.Empty<byte>());

            LoadRecentFilesFromSettings();
        }

        private void UpdatePacketState(PacketDefiner invoker, bool avoidPacketsListUpdate = false)
        {
            PacketDefiner definer = invoker;

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
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
                {
                    using var d = Dispatcher.DisableProcessing();
                    hexEditor.Visibility = Visibility.Hidden;
                    try
                    {
                        hexEditor.Stream = new MemoryStream(bytes);
                    }
                    catch (InvalidOperationException)
                    {
                        // Don't care
                    }

                    hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    hexEditor.ForegroundSecondColor = hexEditor.Foreground;
                    hexEditor.Visibility = Visibility.Visible;
                }));

                // Update Tab's MainViewModel
                SessionViewModel.UpdateCurrentPacketState(true, definer.ExportPacket);

                // Call Live Update (packet tree)
                if (previewEnabledCheckbox.IsChecked == true)
                {
                    Task.Delay(_livePreviewDelay).ContinueWith(task => UpdateLivePreviewTree());
                }
            }
            else
            {
                // Update Tab's MainViewModel
                SessionViewModel.UpdateCurrentPacketState(false, null);

                // Code below requierd BeginInvoke otherwise
                // the caret in the PacketDefiner textbox might not update
                // (if the textbox triggered this event)
                Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
                {
                    using var d = Dispatcher.DisableProcessing();
                    hexEditor.Visibility = Visibility.Hidden;
                    hexEditor.Foreground = new SolidColorBrush(Color.FromRgb(135, 135, 135));
                    hexEditor.ForegroundSecondColor = hexEditor.Foreground;
                    hexEditor.Visibility = Visibility.Visible;
                }));
            }

            // If this is a legit packet change (an not just selection of a different packet) also queue a packets list update
            _logger.DebugExt(() => $"Inside {nameof(UpdatePacketState)} : Is this a modified packet: {SessionViewModel.CurrentSessionPacket.IsModified}");
            if (!avoidPacketsListUpdate && SessionViewModel.CurrentSessionPacket.IsModified)
            {
                UpdatePacketsListAsync();
            }
        }

        private void PacketDefinerPacketChanged(object sender, EventArgs e)
        {
            if (_fileLoading) return;

            UpdatePacketState(sender as PacketDefiner);
            _sessionSaveState.HasUnsavedChanges = true;
        }

        /// <summary>
        /// Update Live Preview panels in the GUI. Invokes TShark and waits for it.
        /// </summary>
        private void UpdateLivePreviewTree()
        {
            _logger.DebugExt(() => $"{nameof(UpdateLivePreviewTree)} Invoked");

            //
            //  * Protocol Tree *
            //

            // Indicate 'update in progress' instead of previous packet tree
            SessionViewModel.IsPacketPreviewUpdating = true;

            TempPacketSaveData packetBytes = null;
            Dispatcher.BeginInvoke((Action)(() => { packetBytes = SessionViewModel.CurrentSessionPacket.ExportPacket; })).Task
                .Wait();

            if (packetBytes != null && packetBytes.Data.Length != 0)
            {
                var tsharkTask = _tsharkInterOp.GetPdmlAsync(packetBytes, CancellationToken.None);
                XElement packetPdml = tsharkTask.Result;
                packetTreeView.PopulatePacketTree(packetPdml);
            }

            // Remove 'update in progress' indicators
            SessionViewModel.IsPacketPreviewUpdating = false;

            _logger.DebugExt(() => $"{nameof(UpdateLivePreviewTree)} Finished");
        }

        private Task UpdatePacketsListAsync()
        {
            return Task.Run(() =>
            {
                _logger.DebugExt(() => $"{nameof(UpdatePacketsListAsync)} Invoked");
                if (!Monitor.TryEnter(_packetListTokenSrouceLock))
                {
                    // Another thread is currently locking the source below so it will get the state of packets WE wanted to 
                    // update to anyway (it didn't reach "Update Packets Descs" function)
                    _logger.DebugExt(() =>
                        $"{nameof(UpdatePacketsListAsync)} Aborted because other thread is already starting it");
                    return;
                }


                _logger.DebugExt(() => $"{nameof(UpdatePacketsListAsync)} Trying to cancel previous task");
                try
                {
                    //if (!Debugger.IsAttached)
                    {
                        // Only cancelling if running without debugger.
                        // This might cause multiple Win32 expections which, when a debugger is attached, make the GUI freeze.
                        _packetsListTokenSource?.Cancel();
                        _packetsListTokenSource = null;
                    }
                }
                catch
                {
                    // Ignored
                }

                _logger.DebugExt(() => $"{nameof(UpdatePacketsListAsync)} Creating new source + token");
                _packetsListTokenSource = new CancellationTokenSource();
                CancellationToken cToken = _packetsListTokenSource.Token;

                // Waiting the delay
                Task.Delay(_livePreviewDelay, cToken).Wait();

                Monitor.Exit(_packetListTokenSrouceLock);

                _logger.DebugExt(() => $"{nameof(UpdatePacketsListAsync)} Running 'Packets Descriptions' Update func");
                SessionViewModel.UpdatePacketsDescriptions(cToken).Wait();
                _logger.DebugExt(() => $"{nameof(UpdatePacketsListAsync)} Finished");
            });
        }

        #region Copy Paste Funcs
        private void DoPaste(object sender, RoutedEventArgs e) => CurrentPacketDefiner.Paste();
        private void DoCopy(object sender, RoutedEventArgs e) => CurrentPacketDefiner.Copy();
        private void DoCut(object sender, RoutedEventArgs e) => CurrentPacketDefiner.Cut();
        // This method copies the current showing packet to the clipboard
        // The format of the data will be '0xAA, 0xBB'
        private void CopyCSharpShort(object sender, RoutedEventArgs e)
        {
            bool packetValid = AssertCurrentPacketValid();

            if (packetValid)
            {
                byte[] packet = SessionViewModel.CurrentSessionPacket.ExportPacket.Data;
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
                byte[] packet = SessionViewModel.CurrentSessionPacket.ExportPacket.Data;
                IEnumerable<string> bytesAsStrings = packet.Select(b => "0x" + b.ToString("X2"));
                string combinedString = "new byte[]{" + string.Join(", ", bytesAsStrings) + "};";
                Clipboard.SetText(combinedString);
            }

            copyForCSharpButton.IsDropDownOpen = false;
        }
        #endregion
        
        /// <summary>
        /// Asserts the current showing packet is valid. Otherwise shows an error to the user
        /// </summary>
        /// <returns>True if the packet is valid, false otherwise</returns>
        private bool AssertCurrentPacketValid()
        {
            SessionPacketViewModel currItem = SessionViewModel.CurrentSessionPacket;
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
            if ((bool)e.NewValue == false) // - Keyboard focus is lost
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
            try
            {
                SessionViewModel.CurrentSessionPacket.NormalizeHex();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to normalize.\n" + exception.Message, "Error");
            }

        }

        private void FlattenStack(object sender, RoutedEventArgs e)
        {
            try
            {
                SessionViewModel.CurrentSessionPacket.FlattenStack();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to flatten stack.\n" + exception.Message, "Error");
            }
        }


        BytesHighlightning _lastBytesHighlightning = null;
        private void PacketTreeView_OnSelectedItemChanged(object sender,
            PacketTreeView.PacketTreeSelectionChangedArgs e)
        {
            if (e.BytesHiglightning == _lastBytesHighlightning)
            {
                return;
            }
            _lastBytesHighlightning = e.BytesHiglightning;

            if (e.BytesHiglightning == BytesHighlightning.Empty)
            {
                hexEditor.SelectionStop = -1;
                hexEditor.SelectionStart = -1;
                bool isDefinerNormalized = Regex.IsMatch(SessionViewModel.CurrentSessionPacket.Content, "^[0-9A-Fa-f]+$");
                if (isDefinerNormalized)
                {
                    // User selected an item from the generated headers, nothing to show in the packet definer control
                    // Remove any previous selection (so to not confuse the user that the previous selection is also the right one for the current selecte field)
                    SessionViewModel.CurrentSessionPacket.SelectionLength = 0;
                }
            }
            else
            {
                hexEditor.SelectionStart = e.BytesHiglightning.Offset;
                hexEditor.SelectionStop = e.BytesHiglightning.Offset + e.BytesHiglightning.Length - 1;

                bool isDefinerNormalized = Regex.IsMatch(SessionViewModel.CurrentSessionPacket.Content, "^[0-9A-Fa-f]+$");
                if (isDefinerNormalized)
                {
                    int headersLength = GetHeadersLengthFromType(SessionViewModel.CurrentSessionPacket.PacketType);
                    int newOffset = (e.BytesHiglightning.Offset - headersLength) * 2;
                    if (newOffset < 0)
                    {
                        // User selected an item from the generated headers, nothing to show in the packet definer control
                        // Remove any previous selection (so to not confuse the user that the previous selection is also the right one for the current selecte field)
                        SessionViewModel.CurrentSessionPacket.SelectionLength = 0;
                        return;
                    }
                    // User selected an item within the user's input in the definer - highlighting it for them.
                    SessionViewModel.CurrentSessionPacket.SelectionStart = newOffset;
                    SessionViewModel.CurrentSessionPacket.SelectionLength = e.BytesHiglightning.Length * 2;
                }
            }
        }

        private void ExportToWireshark(object sender, RoutedEventArgs e)
        {
            var items = SessionViewModel.ModifiedPackets;
            var invalidItems = items.Where(tabViewModel => !tabViewModel.IsValid);
            if (invalidItems.Any())
            {
                string invalidItemsNames =
                    string.Join(", ", invalidItems.Select(tabViewModel => $"\"{tabViewModel.PacketIndex}\""));
                string error = "Can not export to Wireshark because some tabs contain invalid packet definitions.\n\n" +
                               $"Invalid tabs: {invalidItemsNames}";
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // That .Where is important since the currenlty viewed packet is part of the ModifiedPackets collection
            // but if it's unchanged we should treat it as modified (this can save us duplicating the import file if no
            // other packets are modified)
            List<TempPacketSaveData> exportedPackets = items
                .Where(item => item.IsModified)
                .Select(tabViewModel => tabViewModel.ExportPacket).ToList();
            SessionViewModel.ApplyModifications();


            var wsSender = new WiresharkPipeSender();

            string pipeName = "ps_2_ws_pipe" + (new Random()).Next();
            var senderTask = wsSender.SendPcapngAsync(pipeName, SessionViewModel.BackingPcapng);
            var wsSessionTask = _wsInterOp.RunWireshark(@"\\.\pipe\" + pipeName);

            // The following task continues when the user exits Wireshark.
            // In this case, we might want to trigger a preview update IF the user changed preferences using WS's GUI 
            // (These will also take effect when we run TShark)
            wsSessionTask.CliTask.Task.ContinueWith(_ =>
            {
                if (wsSessionTask.PreferencesChanged)
                {
                    UpdateLivePreviewTree();
                    UpdatePacketsListAsync();
                }
            });
        }

        private void OpenMenuItemClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} - OpenMenuItemClicked");

            if (_sessionSaveState.HasUnsavedChanges)
            {
                // Prompt user about unsaved changes in current session
                var userSelection = MessageBox.Show(
                    "You have unsaved changes in the current session.\r\n" +
                    "\r\n" +
                    "Save changes?",
                    "Unsaved Changes Alert",
                    MessageBoxButton.YesNoCancel);

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
            LoadFile(filePath);
        }

        private void LoadFile(string filePath)
        {
            // TODO: Get rid of this factor or make it return ISmartCaptures ...
            PacketsProvidersFactory ppf = new PacketsProvidersFactory();
            var provider = ppf.Create(filePath);

            bool fileLoaded = false;
            using (Dispatcher.DisableProcessing())
            {
                _fileLoading = true;
                try
                {
                    if (!filePath.EndsWith("pcapng"))
                    {
                        throw new Exception("Trying to open a file without the .pcapng extension. It's not supported yet...");
                    }
                    // TODO: Maybe not a None token?
                    SessionViewModel.LoadFileAsync(filePath, CancellationToken.None);
                    fileLoaded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open file {filePath}\n" +
                        $"Error: {ex.Message}");
                }
                _fileLoading = false;
            }

            _sessionSaveState.Reset();

            if (!fileLoaded)
            {
                return;
            }

            if (filePath.EndsWith(".p2s"))
            {
                // Only 'associating' session if opened a p2s file. pcap/pcapng aren't treated like that.
                _sessionSaveState.AssociatedFilePath = filePath;
            }

            //
            // Add to 'Recent files list' (or not, if present)
            //

            AddToRecentFiles(filePath);
        }

        #region Recent Files List Maintanance
        private void AddToRecentFiles(string filePath)
        {
            string recentFilesEncoded = Settings.Default.RecentFiles;

            if (recentFilesEncoded.Contains(filePath))
            {
                // Already in list, remove old occurence before adding to head
                recentFilesEncoded = recentFilesEncoded.Replace($"{filePath},", string.Empty);
            }

            // Adding to head (to indicate most recent)
            recentFilesEncoded = $"{filePath}," + recentFilesEncoded;

            // Check if we have too many entires (by counting commas in the encoded string)
            if (recentFilesEncoded.Count(ch => ch == ',') > MAX_RECENT_FILES_ENTRIES)
            {
                // We have too many... let's cut a few (most likly just 1)
                // Take all characters and count commas. When number of commas == MAX_RECENT_FILES_ENTRIES then stop.
                int i = 0;
                recentFilesEncoded = new string(recentFilesEncoded.TakeWhile(c => c != ',' || (++i < MAX_RECENT_FILES_ENTRIES)).ToArray());
            }

            // Adding file to 'recent files' list in the app menu
            Settings.Default.RecentFiles = recentFilesEncoded;
            Settings.Default.Save();

            // Just reload the entire thing from the settings (easier than maintaining dup code here nad in LoadRecentFilesFromSettings
            LoadRecentFilesFromSettings();
        }
        void LoadRecentFilesFromSettings()
        {
            _applicationMenu.MenuItems.Clear();
            foreach (string filePath in Settings.Default.RecentFiles.Split(','))
            {
                string normalizedFilePath = filePath.Trim(',');
                if (String.IsNullOrWhiteSpace(normalizedFilePath))
                    continue;
                if (!File.Exists(normalizedFilePath))
                    continue;


                // Adding file to 'recent files' list in the app menu
                var newMenuButton = new SimpleMenuButton()
                {
                    Label = normalizedFilePath,
                };
                newMenuButton.Click += RecentFileMenuItemClicked;
                _applicationMenu.MenuItems.Add(newMenuButton);
            }
        }
        private void RecentFileMenuItemClicked(object sender, RoutedEventArgs e)
        {
            SimpleMenuButton smb = sender as SimpleMenuButton;
            LoadFile(smb.Label);
        }
        #endregion

        #region Save Functions
        private void SaveSession(string destP2sPath)
        {
            var sessionPackets = SessionViewModel.ModifiedPackets.Select(model => model.SessionPacket);
            var p2SSaver = new P2sSaver();
            p2SSaver.Save(destP2sPath, sessionPackets);

            _sessionSaveState.AssociatedFilePath = destP2sPath;
            _sessionSaveState.HasUnsavedChanges = false;
        }
        private UserDecision DoSave()
        {
            if (!_sessionSaveState.HasAssociatedFile) return DoSaveAs(); // No file? Ask user what to do
            if (!_sessionSaveState.HasUnsavedChanges) return UserDecision.Save; // There's a file but no unsaved chagnges. do nothing.
            // There's a file AND some unsaved changes. Save 'em.
            SaveSession(_sessionSaveState.AssociatedFilePath);
            return UserDecision.Save;

            // Not associated with a file - treat as "Save As..." (prompt for path)
        }
        private UserDecision DoSaveAs()
        {
            SaveFileDialog saveDestinationPrompt = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "PacketStudio 2 Session file|*.p2s",
                DefaultExt = ".p2s"
            };
            var res = saveDestinationPrompt.ShowDialog(this);
            if (res.Value == false) return UserDecision.Cancel; // User decided not to save after all
            // User wanted to save. Save to the file he selected
            SaveSession(saveDestinationPrompt.FileName);
            return UserDecision.Save;
        }
        private void SaveMenuItemClicked(object sender, RoutedEventArgs e) => DoSave();
        private void SaveAsMenuItemClicked(object sender, RoutedEventArgs e) => DoSaveAs();
        #endregion

        private void ExitMenuItemClicked(object sender, RoutedEventArgs e) => Close();

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
                // Update settings
                Settings.Default.WiresharkDir = wsDir.WiresharkPath;

                // Raise list updated "event"
                WiresharkDir = wsDir;
            }
        }


        private void MenuButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if (!e.IsDown)
                return;
            if (e.Key != Key.Down && e.Key != Key.Up &&
               e.Key != Key.Enter && e.Key == Key.Return)
                return;

            MenuButton invokingButton = sender as MenuButton;
            Debug.WriteLine(e.Key);
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                switch (invokingButton.Name)
                {
                    case "newMenuButton":
                        NewSessionMenuItemClicked(sender, null);
                        break;
                    case "openMenuButton":
                        OpenMenuItemClicked(sender, null);
                        break;
                    case "saveMenuButton":
                        SaveMenuItemClicked(sender, null);
                        break;
                    case "saveAsMenuButton":
                        SaveAsMenuItemClicked(sender, null);
                        break;
                }

                e.Handled = true;
                return;
            }

            MenuButton next = null;
            if (e.Key == Key.Down)
            {
                next = _applicationMenu.Items[0] as MenuButton;
                for (int i = 0; i < _applicationMenu.Items.Count - 1; i++)
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
                next = _applicationMenu.Items[^1] as MenuButton;
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

        private void NewSessionMenuItemClicked(object sender, RoutedEventArgs e)
        {
            if (_sessionSaveState.HasUnsavedChanges)
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
            _sessionSaveState.Reset();
            SessionViewModel.ResetItemsCollection();
        }


        private void EncodeText(object sender, RoutedEventArgs e)
        {
            EncodeTextWindow iaw = new EncodeTextWindow();
            bool? res = iaw.ShowDialog();
            if (res == true)
            {
                EncodeTextViewModel iavm = iaw.DataContext as EncodeTextViewModel;
                Encoding enc = iavm.AvailableEncodings[iavm.SelectedEncIndex];
                byte[] encoded = enc.GetBytes(iavm.Text);
                string hexString = encoded.ToHex();

                // Insert the encoded ASCII bytes after the current caret position
                int pos = SessionViewModel.CurrentSessionPacket.CaretPosition;

                string newPacketData = SessionViewModel.CurrentSessionPacket.Content.Insert(pos, hexString);
                SessionViewModel.CurrentSessionPacket.Content = newPacketData;
                SessionViewModel.CurrentSessionPacket.CaretPosition = pos + hexString.Length;

            }
        }

        private void PacketDefiner_OnCaretPositionChanged(object sender, EventArgs e)
        {
            PacketDefiner pd = sender as PacketDefiner;
            if (pd.DataContext is SessionPacketViewModel tivm)
                tivm.CaretPosition = pd.CaretPosition;
        }

        #region Move Packets
        private void MoveBackward(object sender, RoutedEventArgs e)
        {
            int newIndex = SessionViewModel.SelectedPacketIndex - 1;
            if (newIndex < 0)
            {
                Debug.WriteLine(" @@@ Trying to move to negative index... stopping");
                return;
            }

            SessionViewModel.MovePacket(newIndex).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() => packetsList.SelectedIndex = newIndex);
            });
        }
        private void MoveForward(object sender, RoutedEventArgs e)
        {
            int newIndex = SessionViewModel.SelectedPacketIndex + 1;
            if (newIndex >= SessionViewModel.PacketsCount)
            {
                Debug.WriteLine(" @@@ Trying to move to too high of an index... stopping");
                return;
            }

            SessionViewModel.MovePacket(newIndex).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() => packetsList.SelectedIndex = newIndex);
            });
        }

        private void DoMoveDialog(object sender, RoutedEventArgs e)
        {
            RelocatePacketWindow rpw = new RelocatePacketWindow();
            RelocatePacketViewModel rpvm = rpw.DataContext as RelocatePacketViewModel;
            rpvm.MaxPacketPosition = SessionViewModel.PacketsCount;
            // Set 'NewPosition' to current position so the windows starts by showing the current state of
            // the packets list
            rpvm.NewPosition = SessionViewModel.SelectedPacketIndex.ToString();

            // Prompts & block
            bool? res = rpw.ShowDialog();
            if (res == true)
            {
                int index = int.Parse(rpvm.NewPosition);
                SessionViewModel.MovePacket(index).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() => packetsList.SelectedIndex = index);
                });
            }
        }

        #endregion

        #region Drag Drop P/Invoke Ugliness
        const int WM_DROPFILES = 0x233;

        [DllImport("shell32.dll")]
        static extern void DragAcceptFiles(IntPtr hwnd, bool fAccept);

        [DllImport("shell32.dll")]
        static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder filename, uint cch);

        [DllImport("shell32.dll")]
        static extern void DragFinish(IntPtr hDrop);


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            DragAcceptFiles(hwnd, true);

            // The call to OnSourceInitialized above, for some reason, triggers DataContextChanged in the PacketDefiner
            // so it de-registers our handler and we need to re-register it here.
            // IIRC, this function is ran once in the entire program's life. So it's an ugly hack but that's what it is.
            ReReisterPacketChangedEvent();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DROPFILES)
            {
                handled = true;
                return HandleDropFiles(wParam);
            }

            return IntPtr.Zero;
        }

        private IntPtr HandleDropFiles(IntPtr hDrop)
        {
            const int MAX_PATH = 260;

            var count = DragQueryFile(hDrop, 0xFFFFFFFF, null, 0);

            List<string> droppedFiles = new List<string>();
            for (uint i = 0; i < count; i++)
            {
                int size = (int)DragQueryFile(hDrop, i, null, 0);

                var filename = new StringBuilder(size + 1);
                DragQueryFile(hDrop, i, filename, MAX_PATH);

                droppedFiles.Add(filename.ToString());
            }
            Task.Run(() => HandleDropFiles(droppedFiles));

            DragFinish(hDrop);

            return IntPtr.Zero;
        }

        private void HandleDropFiles(List<string> droppedFiles)
        {
            if (droppedFiles.Count < 1) // Nothing dropped?
                return;
            if (droppedFiles.Count > 1)
            {
                MessageBox.Show($"Dropping is supported for single files only.\n(You dropped {droppedFiles})");
                return;
            }

            var file = droppedFiles.Single();
            Dispatcher.Invoke(() => LoadFile(file));
        }
        #endregion

        private void packetsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int currIndex = packetsList.SelectedIndex;
            if (currIndex == -1)
            {
                if (SessionViewModel.SelectedPacketIndex != -1)
                {
                    packetsList.SelectedIndex = SessionViewModel.SelectedPacketIndex;
                }
            }
        }

        private void AddNewPacket(object sender, RoutedEventArgs e)
        {
            SessionViewModel.AddNewPacket();
            _sessionSaveState.HasUnsavedChanges = true;
            if (previewEnabledCheckbox.IsChecked == true)
            {
                UpdatePacketsListAsync();
            }
        }

        private void DeletePacket(object sender, RoutedEventArgs e)
        {
            SessionViewModel.DeletePacket();
        }

        #region PacketsList packet switching workaround

        private void PacketsList_OnSourceUpdated(object? sender, DataTransferEventArgs e) =>
            ReReisterPacketChangedEvent();

        private void ReReisterPacketChangedEvent()
        {
            //
            // This event handler runs everytime a new packet is selected and AFTER the packet was successfuly loaded
            // into the packet definer (so all the unwanted 'PacketChanged' trials to raise already happened)
            // we can now re-register to this event.
            // Notice: The de-registeration is happening in PacketDefiner's "data context changed" event handler
            //

            CurrentPacketDefiner.ResetPacketUpdateEvent(); // Prevent weird edge cases (hopefully)
            Debug.WriteLine(" @@@ Registering PacketChange handler LOL!!!!! ");
            CurrentPacketDefiner.PacketChanged += PacketDefinerPacketChanged;

            // Only time this function runs and we don't want to update the packet state is when we are still in InitializeComponents in the Ctor
            // this bool tells us if this is the case
            if (this.IsInitialized)
            {
                UpdatePacketState(CurrentPacketDefiner, avoidPacketsListUpdate: true);
            }
        }
        #endregion

    }
}
#pragma warning restore CA1416 // Validate platform compatibility
