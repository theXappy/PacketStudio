using System.Diagnostics;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using Syncfusion.Windows.Shared;

namespace PacketStudio.NewGUI
{
    public class TabItemViewModel : NotificationObject
    {
        private string content;
        private string header;
        private PacketSaveData _corePacket;
        private TempPacketSaveData _packet;

        public bool IsValid { get; set; }
        public string ValidationError { get; set; }

        public TempPacketSaveData Packet
        {
            get => _packet;
            set
            {
                _packet = value;
                this.RaisePropertyChanged(nameof(Packet));
            }
        }

        public PacketSaveData CorePacket
        {
            get => _corePacket;
            set
            {
                _corePacket = value;
                this.RaisePropertyChanged(nameof(CorePacket));
            }
        }

        public string Header
        {
            get => header;
            set
            {
                header = value;
                this.RaisePropertyChanged(nameof(Header));
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                this.RaisePropertyChanged(nameof(Content));
            }
        }
    }
}