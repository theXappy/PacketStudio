using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.Providers;
using PacketStudio.DataAccess.SaveData;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.ViewModels
{
    public class MainViewModel : NotificationObject
    {
        private const string TAB_HEADER_PREFIX = "Packet";
        private int nextTabNumber = 1;

        private NewButtonAlignment newButtonAlignment = NewButtonAlignment.Last;
        private Brush newButtonBackground = Brushes.DimGray;
        private Thickness newButtonMargin = new Thickness(50,1,50,1); 
        private bool isNewButtonClosedonNoChild = true;
        private bool isNewButtonEnabled = true;
        private ObservableCollection<TabItemViewModel> tabItems;

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

        

        public void AddNewPacket(PacketSaveDataNG psdng = null)
        {
            if (psdng == null)
            {
                psdng = new PacketSaveDataNG(HexStreamType.Raw,String.Empty);
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

        public void LoadFile(IPacketsProviderNG provider)
        {
            tabItems.Clear();
            nextTabNumber = 1;
            foreach (PacketSaveDataNG packet in provider)
            {
              AddNewPacket(packet);  
            }
        }
    }
}