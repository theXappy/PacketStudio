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
        private PacketSaveData basePacket;
        private TempPacketSaveData _packet;

        public bool IsValid { get; set; }
        public string ValidationError { get; set; }

        public TempPacketSaveData Packet
        {
            get
            {

                Debug.WriteLine($"@@@ [TIVM] Packet prop RETRIEVED {_packet}");
                return _packet;
            }
            set
            {
                Debug.WriteLine($"@@@ [TIVM] Packet prop updated to {value} (was: {_packet})");
                _packet = value;
            }
        }

        public PacketSaveData BasePacket
        {
            get { return basePacket; }
            set
            {
                basePacket = value;
                this.RaisePropertyChanged(nameof(BasePacket));
            }
        }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                this.RaisePropertyChanged(nameof(Header));
            }
        }

        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                this.RaisePropertyChanged(nameof(Content));
            }
        }
    }
}