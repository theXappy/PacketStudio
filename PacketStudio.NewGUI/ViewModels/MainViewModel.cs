using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.DataAccess.SaveData;
using PacketStudio.DataAccess.SmartCapture;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private const string TAB_HEADER_PREFIX = "Packet";
        private int nextTabNumber = 1;

        private ObservableCollection<SessionPacketViewModel> _modifiedPackets;
        private SessionPacketViewModel _currentSessionPacket;
        public ISmartCaptureFile BackingSmartCapture { get; set; }
        private string[] _packetsDescs = new string[0];
        private int _selectedPacketIndex = 0;

        public ObservableCollection<SessionPacketViewModel> ModifiedPackets
        {
            get { return _modifiedPackets; }
            set
            {
                _modifiedPackets = value;
                this.RaisePropertyChanged(nameof(ModifiedPackets));
            }
        }

        public SessionPacketViewModel CurrentSessionPacket
        {
            get { return _currentSessionPacket; }
            set
            {
                // Handle last packet
                if (_currentSessionPacket != null)
                {
                    if (_currentSessionPacket.IsModified)
                    {
                        Debug.WriteLine(" ### Packet was modified!!!");
                        // ??
                    }
                    else
                    {
                        Debug.WriteLine(" ### Packet was NOT modified");
                        ModifiedPackets.Remove(_currentSessionPacket);
                    }
                }

                // Handle new packet
                _currentSessionPacket = value;
                ModifiedPackets.Add(_currentSessionPacket);
                this.RaisePropertyChanged(nameof(CurrentSessionPacket));
            }
        }

        public string[] PacketsDescriptions
        {
            get { return _packetsDescs; }
            set
            {
                _packetsDescs = value;
                this.RaisePropertyChanged(nameof(PacketsDescriptions));
            }
        }

        public int SelectedPacketIndex
        {
            get { return _selectedPacketIndex; }
            set
            {
                _selectedPacketIndex = value;
                this.RaisePropertyChanged(nameof(SelectedPacketIndex));

                if(_selectedPacketIndex == -1)
                    return;

                // TODO: if _selected too big what happens? throws?
                // TODO: Use link layer
                var (link, data) = BackingSmartCapture.GetPacket(_selectedPacketIndex);

                // TODO: Remvoe this 'tab number' header thingy
                var sessionPacketObj = _modifiedPackets.FirstOrDefault((SessionPacketViewModel viewModel) => viewModel.Header == _selectedPacketIndex);
                if (sessionPacketObj != null) {
                    Debug.WriteLine($" @@@ A session packet object was already created and stored for packet #{_selectedPacketIndex}");
                    CurrentSessionPacket = sessionPacketObj;
                }
                else {
                    Debug.WriteLine($" @@@ NO session packet object foudn packet #{_selectedPacketIndex}, Creating new one!");
                    CurrentSessionPacket = new SessionPacketViewModel()
                    {
                        Header = _selectedPacketIndex,
                    };
                    CurrentSessionPacket.LoadInitialState(new PacketSaveDataNG(HexStreamType.Raw, data.ToHex())
                    {
                        Details = new Dictionary<string, string>()
                    {
                        {PacketSaveDataNGProtoFields.ENCAPS_TYPE, ((int)link).ToString()},
                    }
                    });
                }
            }
        }

        public void AddNewPacket(PacketSaveDataNG psdng = null)
        {
            if (psdng == null) {
                psdng = new PacketSaveDataNG(HexStreamType.Raw, String.Empty);
            }

            int tabNumber = nextTabNumber++;
            SessionPacketViewModel new_model1 = new SessionPacketViewModel()
            {
                Header = tabNumber,
                Content = $"",
            };
            new_model1.LoadInitialState(psdng);
            _modifiedPackets.Add(new_model1);
        }


        public void ResetItemsCollection()
        {
            _modifiedPackets.Clear();
            // Add first packet
            nextTabNumber = 1;
            AddNewPacket(null);

        }
        public MainViewModel()
        {
            _modifiedPackets = new ObservableCollection<SessionPacketViewModel>();
            CurrentSessionPacket = new SessionPacketViewModel();

            // Register self with parent MainWindow
            MainWindow.SessionViewModel = this;
        }

        private TSharkInterop _tshark = new TSharkInterop(SharksFinder.GetDirectories().First().TsharkPath);

        public void LoadFile(ISmartCaptureFile smartCapture)
        {
            BackingSmartCapture = smartCapture;
            var tsharkTask = _tshark.GetTextOutputAsync(BackingSmartCapture.GetPcapngFilePath(), CancellationToken.None);
            tsharkTask.ContinueWith((descTask) =>
            {
                _modifiedPackets.Clear();
                PacketsDescriptions = descTask.Result;
                SelectedPacketIndex = 0;
            });
        }

        public void MovePacket(int newIndex)
        {
            var currPacket = this.CurrentSessionPacket;
            _modifiedPackets.Remove(currPacket);

            BackingSmartCapture.MovePacket(this.SelectedPacketIndex, newIndex);
            
            var tsharkTask = _tshark.GetTextOutputAsync(BackingSmartCapture.GetPcapngFilePath(), CancellationToken.None);
            tsharkTask.ContinueWith((descTask) =>
            {
                PacketsDescriptions = descTask.Result;
                SelectedPacketIndex = newIndex;
            });
        }
    }
}