using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI
{
    public class ViewModel : NotificationObject
    {
        private NewButtonAlignment newButtonAlignment = NewButtonAlignment.Last;
        private Brush newButtonBackground = Brushes.DimGray;
        private Thickness newButtonMargin = new Thickness(5,1,5,1); 
        private bool isNewButtonClosedonNoChild = true;
        private bool isNewButtonEnabled = true;
        private ObservableCollection<TabItemViewModel> tabItems;
        private readonly DelegateCommand<object> newButtonClickCommand;

        public ICommand NewButtonClickCommand
        {
            get
            {
                return newButtonClickCommand;
            }
        }

        public Thickness NewButtonMargin
        {
            get { return newButtonMargin; }
            set
            {
                newButtonMargin = value;
                this.RaisePropertyChanged(nameof(NewButtonMargin));
            }
        }
        public Brush NewButtonBackground
        {
            get { return newButtonBackground; }
            set
            {
                newButtonBackground = value;
                this.RaisePropertyChanged(nameof(NewButtonBackground));
            }
        }

        public NewButtonAlignment NewButtonAlignment
        {
            get { return newButtonAlignment; }
            set
            {
                newButtonAlignment = value;
                this.RaisePropertyChanged(nameof(NewButtonAlignment));
            }
        }
        public bool IsNewButtonEnabled
        {
            get { return isNewButtonEnabled; }
            set
            {
                isNewButtonEnabled = value;
                this.RaisePropertyChanged(nameof(IsNewButtonEnabled));
            }
        }
        public bool IsNewButtonClosedonNoChild
        {
            get { return isNewButtonClosedonNoChild; }
            set
            {
                isNewButtonClosedonNoChild = value;
                this.RaisePropertyChanged(nameof(IsNewButtonClosedonNoChild));
            }
        }

        public ObservableCollection<TabItemViewModel> TabItems
        {
            get { return tabItems; }
            set
            {
                tabItems = value;
                this.RaisePropertyChanged(nameof(TabItems));
            }
        }
        public void NewButtonClicked(object parameter)
        {
            TabControlExt tabControl = parameter as TabControlExt;
            int count = tabControl.Items.Count + 1;
            TabItemViewModel new_model1 = new TabItemViewModel()
            {
                Header = "tabItem" + count,
                Content = "This is the content of " + count + " tabitem."
            };
            tabItems.Add(new_model1);
        }


        public void PopulateCollection()
        {
            TabItemViewModel model1 = new TabItemViewModel()
            {
                Header = "tabItem1",
                Content = "This is the content of 1 tabitem.",
                CorePacket = new PacketSaveDataV3("aabb11223344556677889900",HexStreamType.UdpPayload,LinkLayerType.Ethernet.ToString(),"2","2", ""),
                Packet = new TempPacketSaveData(new byte[5],LinkLayerType.Ethernet )
            };
            TabItemViewModel model2 = new TabItemViewModel()
            {
                Header = "tabItem2",
                Content = "This is the content of 2 tabitem.",
                CorePacket = new PacketSaveDataV3("ccbb00998877665544332211",HexStreamType.Raw,LinkLayerType.Ethernet.ToString(),"2","2", ""),
                Packet = new TempPacketSaveData(new byte[3],LinkLayerType.A653Icm )
            };

            //Adding tab item details to the collection
            tabItems.Add(model1);
            tabItems.Add(model2);
        }
        public ViewModel()
        {
            tabItems = new ObservableCollection<TabItemViewModel>();
            PopulateCollection();
            newButtonClickCommand = new DelegateCommand<object>(NewButtonClicked);

            // Register self with parent MainWindow
            MainWindow.TabControlViewModel = this;
        }
    }
}