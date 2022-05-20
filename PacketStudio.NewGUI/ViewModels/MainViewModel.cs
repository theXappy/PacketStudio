using FastPcapng;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.PcapNG.BlockTypes;
using Haukcode.PcapngUtils.PcapNG.CommonTypes;
using Haukcode.PcapngUtils.PcapNG.OptionTypes;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using Syncfusion.Windows.Controls.Gantt;
using Task = System.Threading.Tasks.Task;
using PacketStudio.DataAccess.Providers;

namespace PacketStudio.NewGUI.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private const string TAB_HEADER_PREFIX = "Packet";
        private int nextTabNumber = 1;

        private ObservableCollection<SessionPacketViewModel> _modifiedPackets;
        private SessionPacketViewModel _currentSessionPacket;


        private MemoryPcapng _backingPcapng = new MemoryPcapng();
        private ObservableCollection<string> _packetsDescs = new(new string[0]);
        private int _selectedPacketIndex = 0;

        private object _modifiedPacketsLock = new();

        /// <summary>
        /// Be a good sport and don't change this collection. Treat it as a read-only collection.
        /// </summary>
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
                Debug.WriteLine($"CurrentSessionPacket Updating!!!!! Value: {value}");

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
                        lock (_modifiedPacketsLock)
                        {
                            ModifiedPackets.Remove(_currentSessionPacket);
                        }
                    }
                }

                // Handle new packet
                _currentSessionPacket = value;
                lock (_modifiedPacketsLock)
                {
                    if (!ModifiedPackets.Contains(_currentSessionPacket))
                    {
                        ModifiedPackets.Add(_currentSessionPacket);
                    }
                }

                this.RaisePropertyChanged(nameof(CurrentSessionPacket));
            }
        }

        public ObservableCollection<string> PacketsDescriptions
        {
            get { return _packetsDescs; }
            set
            {
                _packetsDescs = value;
                this.RaisePropertyChanged(nameof(PacketsDescriptions));
            }
        }

        public int PacketsCount => PacketsDescriptions.Count;

        public int SelectedPacketIndex
        {
            get { return _selectedPacketIndex; }
            set
            {
                Debug.WriteLine($" XXX SelectedIndex changed to: {value}");
                if (value == -1)
                {
                    // We say no to '-1's from the gui!
                    return;
                }
                _selectedPacketIndex = value;
                this.RaisePropertyChanged(nameof(SelectedPacketIndex));

                if (_selectedPacketIndex == -1)
                    return;

                // TODO: if _selected too big what happens? throws?
                // TODO: Use link layer
                var packetBlock = _backingPcapng.GetPacket(_selectedPacketIndex);

                var iface = _backingPcapng.Interfaces[packetBlock.InterfaceID];
                var ifaceLinkType = iface.LinkType;

                // TODO: Remvoe this 'tab number' header thingy
                var sessionPacketObj = ModifiedPackets.FirstOrDefault((SessionPacketViewModel viewModel) => viewModel.PacketIndex == _selectedPacketIndex);
                Debug.WriteLine($" XXX Trying to find previous packetVM with this index: {_selectedPacketIndex} did it work? {sessionPacketObj != null}");
                if (sessionPacketObj != null)
                {
                    Debug.WriteLine($" XXX It worked so changing CurrentSessionPacket to it");
                    CurrentSessionPacket = sessionPacketObj;
                }
                else
                {
                    Debug.WriteLine($" XXX It did not work so we are generating a new SessionPacketViewMode");
                    CurrentSessionPacket = new SessionPacketViewModel()
                    {
                        PacketIndex = _selectedPacketIndex,
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

        public bool IsPacketsDescriptionsUpdating
        {
            get => _isPacketsDescriptionsUpdating;
            set
            {
                _isPacketsDescriptionsUpdating = value;
                this.RaisePropertyChanged(nameof(IsPacketsDescriptionsUpdating));
            }
        }

        public bool IsPacketPreviewUpdating
        {
            get
            {
                return _isPacketPreviewUpdating;
            }
            set
            {
                _isPacketPreviewUpdating = value;
                this.RaisePropertyChanged(nameof(IsPacketPreviewUpdating));
            }
        }


        public void AddNewPacket(PacketSaveDataNG psdng = null)
        {
            if (psdng == null)
            {
                psdng = new PacketSaveDataNG(HexStreamType.Raw, String.Empty);
            }

            int tabNumber = nextTabNumber++;
            SessionPacketViewModel new_model1 = new SessionPacketViewModel()
            {
                PacketIndex = tabNumber,
                Content = $"",
            };
            new_model1.LoadInitialState(psdng);
            ModifiedPackets.Add(new_model1);

            // TODO: Are those default parameters good?
            _backingPcapng.AppendPacket(new EnhancedPacketBlock(0, new TimestampHelper(0, 0), 1, new byte[1] { 0x00 }, new EnhancedPacketOption()));
        }


        public void ResetItemsCollection()
        {
            ModifiedPackets.Clear();
            // Add first packet
            nextTabNumber = 1;
            AddNewPacket(null);

        }
        public MainViewModel()
        {
            ModifiedPackets = new ObservableCollection<SessionPacketViewModel>();
            var newPacket = new SessionPacketViewModel();
            newPacket.LoadInitialState(new PacketSaveDataNG(HexStreamType.Raw, ""));
            this.CurrentSessionPacket = newPacket;

            // Register self with parent MainWindow
            MainWindow.SessionViewModel = this;
        }

        public TSharkInterop TsharkInterop { get; set; }
        private bool _isPacketsDescriptionsUpdating;
        private bool _isPacketPreviewUpdating;

        public Task LoadFileAsync(string path, CancellationToken token)
        {
            return LoadFileAsync(MemoryPcapng.ParsePcapng(path), token);
        }
        public Task LoadFileAsync(CancellationToken token)
        {
            MemoryPcapng pcap = new MemoryPcapng();
            return LoadFileAsync(pcap, token);
        }
        public Task LoadFileAsync(MemoryPcapng memPcap, CancellationToken token)
        {
            _backingPcapng = memPcap;

            ModifiedPackets.Clear();

            var updateDescsTask = UpdatePacketsDescriptions(token);
            return updateDescsTask.ContinueWith(task =>
            {
                this.SelectedPacketIndex = 0;
            }, token);
        }


        public Task UpdatePacketsDescriptions(CancellationToken token)
        {
            IsPacketsDescriptionsUpdating = true;

            ApplyModifications();

            Dictionary<int, SessionPacketViewModel> theMisfits = ModifiedPackets.Where(pkt => !pkt.IsValid).ToDictionary(pkt => pkt.PacketIndex);

            WiresharkPipeSender sender = new WiresharkPipeSender();
            string pipeName = "ps_2_ws_pipe" + (new Random()).Next();
            sender.SendPcapngAsync(pipeName, _backingPcapng);

            var newCollection = new ObservableCollection<string>();

            int DEBUG_HOW_MANY_TIMES_RAN = 0;

            var i = 0;
            void HandleNewTSharkTextLine(string line)
            {
                DEBUG_HOW_MANY_TIMES_RAN++;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // On first valid packet update the collection
                    if (this.PacketsDescriptions != newCollection)
                    {
                        this.PacketsDescriptions = newCollection;
                    }

                    if (theMisfits.TryGetValue(i, out _))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            string prefix = (i + 1).ToString().PadLeft(5, ' ');
                            newCollection.Add(
                                $"{prefix}\t[ PacketStudio: ERROR GENERATING PACKET! ]"); // TODO: Get 'Validation Error' from the object and show to user?
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            newCollection.Add(line);
                        });
                    }
                    i++;
                }
            }


            var tsharkTask = TsharkInterop.GetTextOutputAsync(@"\\.\pipe\" + pipeName, token, HandleNewTSharkTextLine);

            // Pad with "ERROR GEN'ING" to number of packets
            tsharkTask.ContinueWith(t =>
            {
                while (i != this.PacketsCount)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string prefix = (i + 1).ToString().PadLeft(5, ' ');
                        newCollection.Add(
                            $"{prefix}\t[ PacketStudio: ERROR GENERATING PACKET! ]"); // TODO: Get 'Validation Error' from the object and show to user?
                    });
                    i++;
                }
            });


            tsharkTask.ContinueWith(_ => Debug.WriteLine($" @@@ Did TShark fail us? Our Func's Invoked {DEBUG_HOW_MANY_TIMES_RAN} times"));
            tsharkTask.ContinueWith(_ => IsPacketsDescriptionsUpdating = false);

            return tsharkTask;
        }

        public Task MovePacket(int newIndex)
        {
            int currectIndex = CurrentSessionPacket.PacketIndex;
            Debug.WriteLine($" @@@ Trying to move packet #{currectIndex} to Index #{newIndex}");
            if (currectIndex == newIndex)
            {
                // Trying to move to same place
                return Task.CompletedTask;
            }

            _backingPcapng.MovePacket(currectIndex, newIndex);
            CurrentSessionPacket.PacketIndex = newIndex;
            // Update other modified packets
            lock (_modifiedPacketsLock)
            {
                if (currectIndex > newIndex)
                {
                    foreach (var pktVm in ModifiedPackets.Where(pkt =>
                        pkt.PacketIndex >= newIndex && pkt.PacketIndex < currectIndex))
                    {
                        pktVm.PacketIndex++;
                    }
                }
                else if (currectIndex < newIndex)
                {
                    foreach (var pktVm in ModifiedPackets.Where(pkt =>
                        pkt.PacketIndex > newIndex && pkt.PacketIndex < currectIndex))
                    {
                        pktVm.PacketIndex--;
                    }
                }
            }

            return UpdatePacketsDescriptions(CancellationToken.None);
        }

        public void ApplyModifications()
        {
            lock (_modifiedPacketsLock)
            {
                List<InterfaceDescriptionBlock> newInterfacesList = new List<InterfaceDescriptionBlock>();

                int GetIndexOrAdd(InterfaceDescriptionBlock iface)
                {
                    // Let's find the (old or new) interface's index will be after re-ordering
                    int matchingIfaceId = newInterfacesList.IndexOf(iface);
                    if (matchingIfaceId == -1)
                    {
                        // Not yet in the new interfaces list. Let's add it now
                        newInterfacesList.Add(iface);
                        matchingIfaceId = newInterfacesList.IndexOf(iface);
                    }
                    return matchingIfaceId;
                }


                foreach (SessionPacketViewModel packetVm in ModifiedPackets.ToList())
                {
                    int index = packetVm.PacketIndex;
                    EnhancedPacketBlock oldEpb = _backingPcapng.GetPacket(index);

                    // Note we are both checking if the packet is valid and if we can 'Export it' using ".ExportPacket" because
                    // very quick changes to the packet in the GUI might make it both IsValid=True && ExportPacket==null
                    TempPacketSaveData packet = packetVm.ExportPacket;
                    if (!packetVm.IsValid || packet == null)
                    {
                        Debug.WriteLine($" @@@ = Cannot export. '!packetVm.IsValid || packet == null' failed. Is it the null part? {packet == null}");
                        // If the VM is not valid we can't generate a packet so let's just reset this packet in the backing pacpng
                        // so when wireshark processes it it doesn't freak out/ruin other packets (e.g. in TCP stream)
                        Array.Clear(oldEpb.Data, 0, oldEpb.Data.Length);

                        InterfaceDescriptionBlock matchingInterface = _backingPcapng.Interfaces[oldEpb.InterfaceID];
                        oldEpb.InterfaceID = GetIndexOrAdd(matchingInterface);
                    }
                    else
                    {
                        if (packetVm.IsModified)
                        {
                            Debug.WriteLine($" @@@ = It's valid! Applying modification for packet #{index}");
                            oldEpb.Data = packet.Data;
                            Debug.WriteLine($" @@@ = Its Data: {packet.Data.ToHex()}");
                            oldEpb.PacketLength = packet.Data.Length;
                            Debug.WriteLine($" @@@ = Its Data Length: {packet.Data.Length}");
                        }

                        // Figure out best interface ID for this packet
                        // Check if prev packet at this position had our link layer -- then use it's iface id


                        InterfaceDescriptionBlock matchingInterface = _backingPcapng.Interfaces[oldEpb.InterfaceID];
                        if (matchingInterface.LinkType != (LinkTypes)packet.LinkLayer)
                        {
                            // Mismatch link type, Just find any another interface with that type and go with it...
                            matchingInterface =
                                _backingPcapng.Interfaces.FirstOrDefault(iface =>
                                    iface.LinkType == (LinkTypes)packet.LinkLayer);
                            if (matchingInterface == null)
                            {
                                // No interface for that link type yet. Creating a new one here!
                                matchingInterface = new InterfaceDescriptionBlock((LinkTypes)packet.LinkLayer, 0, new InterfaceDescriptionOption());
                            }
                        }

                        oldEpb.InterfaceID = GetIndexOrAdd(matchingInterface);

                        // This packet is no longer 'modified' compared to the pcapng
                        // but NOT if it's current packet
                        if (packetVm != this.CurrentSessionPacket)
                        {
                            Debug.WriteLine($" @@@ Removing modiifed packet: {packetVm.PacketIndex}. Is it even in the collection? {ModifiedPackets.Contains(packetVm)}");
                            ModifiedPackets.Remove(packetVm);
                        }
                        else
                        {
                            // This makes the packet report "IsModifed" = false
                            packetVm.LoadInitialState(packetVm.SessionPacket);
                            Debug.WriteLine(" @@@ HaHaHaHaHaHaHaHaHa That was a nice bug :)");
                        }
                    }

                    _backingPcapng.UpdatePacket(index, oldEpb);

                } // foreach

                // Now let's remove those unused interface blocks
                _backingPcapng.Interfaces.Clear();
                _backingPcapng.Interfaces.AddRange(newInterfacesList);

            }
        }

        public void DeletePacket()
        {
            if (this.PacketsCount == 1)
            {
                // Don't allow removing last one?
                // Or remove and add new empty one?
                return;
            }

            lock (_modifiedPacketsLock)
            {
                ModifiedPackets.Remove(this.CurrentSessionPacket);
            }

            this.BackingPcapng.RemovePacket(SelectedPacketIndex);
            this.SelectedPacketIndex = Math.Max(0, SelectedPacketIndex - 1);
        }

        public void UpdateCurrentPacketState(bool isValid, TempPacketSaveData definerExportPacket)
        {
            bool isPreviouslyModified = CurrentSessionPacket.IsModified;
            CurrentSessionPacket.IsValid = isValid;
            CurrentSessionPacket.ExportPacket = definerExportPacket;

            // If packet "became" modified
            if (isPreviouslyModified && CurrentSessionPacket.IsModified)
            {
                if (!ModifiedPackets.Contains(CurrentSessionPacket)) // Prevent duplicates
                {
                    ModifiedPackets.Add(CurrentSessionPacket);
                }
            }

        }
    }
}