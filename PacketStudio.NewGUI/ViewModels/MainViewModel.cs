using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.DataAccess.SaveData;
using PacketStudio.DataAccess.SmartCapture;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private const string TAB_HEADER_PREFIX = "Packet";
        private int nextTabNumber = 1;

        private ObservableCollection<TabItemViewModel> tabItems;
        private TabItemViewModel currentTabItem = new TabItemViewModel() { Content = "HaHa" };
        private ISmartCaptureFile _smartCapture;
        private string[] _packetsDescs = new string[0];
        private int _selectedPacketIndex = 0;

        public ObservableCollection<TabItemViewModel> TabItems
        {
            get { return tabItems; }
            set
            {
                tabItems = value;
                this.RaisePropertyChanged(nameof(TabItems));
            }
        }

        public TabItemViewModel CurrentTabItem
        {
            get { return currentTabItem; }
            set
            {
                currentTabItem = value;
                this.RaisePropertyChanged(nameof(CurrentTabItem));
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
                var (link, data) = _smartCapture.GetPacket(_selectedPacketIndex);

                // TODO: Remvoe this 'tab number' header thingy
                int tabNumber = nextTabNumber++;
                CurrentTabItem = new TabItemViewModel()
                {
                    Header = $"{TAB_HEADER_PREFIX} {tabNumber}",
                };                
                CurrentTabItem.Load(new PacketSaveDataNG(HexStreamType.Raw, data.ToHex())
                {
                    Details = new Dictionary<string, string>()
                    {
                        {PacketSaveDataNGProtoFields.ENCAPS_TYPE, ((int)link).ToString()},
                    }
                });
            }
        }

        public void AddNewPacket(PacketSaveDataNG psdng = null)
        {
            if (psdng == null) {
                psdng = new PacketSaveDataNG(HexStreamType.Raw, String.Empty);
            }

            int tabNumber = nextTabNumber++;
            TabItemViewModel new_model1 = new TabItemViewModel()
            {
                Header = $"{TAB_HEADER_PREFIX} {tabNumber}",
                Content = $"",
            };
            new_model1.Load(psdng);
            tabItems.Add(new_model1);
        }


        public void ResetItemsCollection()
        {
            tabItems.Clear();
            // Add first packet
            nextTabNumber = 1;
            AddNewPacket(null);

        }
        public MainViewModel()
        {
            tabItems = new ObservableCollection<TabItemViewModel>();
            ResetItemsCollection();

            // Register self with parent MainWindow
            MainWindow.TabControlMainViewModel = this;
        }

        //public void LoadFile(IPacketsProviderNG provider)
        //{
        //    this.CurrentTabItem = new TabItemViewModel { Content = "Kaaaaa" };
        //    tabItems.Clear();
        //    nextTabNumber = 1;
        //    try {
        //        for (int i = 0; i < 10_000; i++) {
        //            AddNewPacket();
        //        }

        //        //foreach (PacketSaveDataNG packet in provider) {
        //        //    AddNewPacket(packet);
        //        //}

        //    }
        //    finally {
        //        // Packet provider might return 0 packets if the PCAP is empty/broken
        //        // In that case we are adding a single empty tab (packet)
        //        if (!tabItems.Any()) {
        //            AddNewPacket();
        //        }
        //    }
        //}

        // TODO: Expose this outside? Get it as an argument?
        private TSharkInterop _tshark = new TSharkInterop(SharksFinder.GetDirectories().First().TsharkPath);

        public void LoadFile(ISmartCaptureFile smartCapture)
        {
            _smartCapture = smartCapture;
            var tsharkTask = _tshark.GetTextOutputAsync(_smartCapture.GetPcapngFilePath(), CancellationToken.None);
            tsharkTask.ContinueWith((descTask) =>
            {
                PacketsDescriptions = descTask.Result;
                SelectedPacketIndex = 0;
            });
        }
    }
}