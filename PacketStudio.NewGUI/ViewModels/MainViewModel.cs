using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FastPcapng;
using FastPcapng.Internal;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using Haukcode.PcapngUtils.PcapNG.CommonTypes;
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


        private MemoryPcapng _backingPcapng = new MemoryPcapng();
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

        public int PacketsCount => PacketsDescriptions.Length;

        public int SelectedPacketIndex
        {
            get { return _selectedPacketIndex; }
            set
            {
                Debug.WriteLine($" XXX SelectedIndex changed to: {value}");
                if (value != -1)
                {
                    // We say no to '-1's from the gui!
                    _selectedPacketIndex = value;
                    return;
                }
                this.RaisePropertyChanged(nameof(SelectedPacketIndex));

                if (_selectedPacketIndex == -1)
                    return;

                // TODO: if _selected too big what happens? throws?
                // TODO: Use link layer
                var packetBlock = _backingPcapng.GetPacket(_selectedPacketIndex);

                var iface = _backingPcapng.Interfaces[packetBlock.InterfaceID];
                var ifaceLinkType = iface.LinkType;

                // TODO: Remvoe this 'tab number' header thingy
                var sessionPacketObj = _modifiedPackets.FirstOrDefault((SessionPacketViewModel viewModel) => viewModel.Header == _selectedPacketIndex);
                if (sessionPacketObj != null)
                {
                    Debug.WriteLine($" @@@ A session packet object was already created and stored for packet #{_selectedPacketIndex}");
                    CurrentSessionPacket = sessionPacketObj;
                }
                else
                {
                    Debug.WriteLine($" @@@ NO session packet object foudn packet #{_selectedPacketIndex}, Creating new one!");
                    CurrentSessionPacket = new SessionPacketViewModel()
                    {
                        Header = _selectedPacketIndex,
                    };
                    CurrentSessionPacket.LoadInitialState(new PacketSaveDataNG(HexStreamType.Raw, packetBlock.Data.ToHex())
                    {
                        Details = new Dictionary<string, string>()
                    {
                        {PacketSaveDataNGProtoFields.ENCAPS_TYPE, ((int)ifaceLinkType).ToString()},
                    }
                    });
                }
            }
        }

        public MemoryPcapng BackingPcapng => _backingPcapng;

        public void AddNewPacket(PacketSaveDataNG psdng = null)
        {
            if (psdng == null)
            {
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

        public void LoadFile(string path)
        {
            _backingPcapng = MemoryPcapng.ParsePcapng(path);

            _modifiedPackets.Clear();

            var updateDescsTask = UpdatePacketsDescriptions();
            updateDescsTask.ContinueWith(task =>
            {
                this.SelectedPacketIndex = 0;
            });
        }

        public Task UpdatePacketsDescriptions()
        {
            ApplyModifications();

            WiresharkPipeSender sender = new WiresharkPipeSender();
            string pipeName = "ps_2_ws_pipe" + (new Random()).Next();
             sender.SendPcapngAsync(pipeName, _backingPcapng);
            var tsharkTask = _tshark.GetTextOutputAsync(@"\\.\pipe\" + pipeName, CancellationToken.None);

            return tsharkTask.ContinueWith(task =>
            {
                string[] descLines = task.Result;
                while (descLines.All(line => string.IsNullOrWhiteSpace(line)))
                {
                    Debug.WriteLine(" @@@ Tshark messed up, retying...");
                    string pipeName = "ps_2_ws_pipe" + (new Random()).Next();
                    sender.SendPcapngAsync(pipeName, _backingPcapng);
                    descLines = _tshark.GetTextOutputAsync(@"\\.\pipe\" + pipeName, CancellationToken.None).Result;
                }

                this.PacketsDescriptions = descLines;
            });
        }

        public void MovePacket(int newIndex)
        {
            int currectIndex = CurrentSessionPacket.Header;
            Debug.WriteLine($" @@@ Trying to move packet #{currectIndex} to Index #{newIndex}");
            if (currectIndex == newIndex)
            {
                // Trying to move to same place
                return;
            }

            _backingPcapng.MovePacket(currectIndex, newIndex);
            CurrentSessionPacket.Header = newIndex;
            // Update other modified packets
            if (currectIndex > newIndex)
            {
                foreach (var pktVm in ModifiedPackets.Where(pkt => pkt.Header >= newIndex && pkt.Header < currectIndex))
                {
                    pktVm.Header++;
                }
            }
            else if (currectIndex < newIndex)
            {
                foreach (var pktVm in ModifiedPackets.Where(pkt => pkt.Header > newIndex && pkt.Header < currectIndex))
                {
                    pktVm.Header--;
                }
            }

            UpdatePacketsDescriptions().Wait();
        }

            public void ApplyModifications()
        {
            Debug.WriteLine(" @@@ ====== Applying Modifications ======");
            foreach (SessionPacketViewModel packetVm in ModifiedPackets.Where(packetVm => packetVm.IsModified).ToList())
            {
                int index = packetVm.Header;
                Debug.WriteLine($" @@@ Applying modification for packet #{index}");
                TempPacketSaveData packet = packetVm.ExportPacket;

                EnhancedPacketBlock oldEpb = _backingPcapng.GetPacket(index);
                oldEpb.Data = packet.Data;
                if (_backingPcapng.Interfaces[oldEpb.InterfaceID].LinkType != (LinkTypes)packet.LinkLayer)
                {
                    // Mismatch link type, find other interface
                    var matchingInterface =
                        _backingPcapng.Interfaces.FirstOrDefault(iface =>
                            iface.LinkType == (LinkTypes)packet.LinkLayer);
                    if (matchingInterface == null)
                    {
                        throw new NotImplementedException("No interface for the linklayer of the packet");
                    }
                    var matchingIfaceId = _backingPcapng.Interfaces.IndexOf(matchingInterface);
                    oldEpb.InterfaceID = matchingIfaceId;
                }

                _backingPcapng.UpdatePacket(index, oldEpb);

                ModifiedPackets.Remove(packetVm);
            }
        }
    }
}