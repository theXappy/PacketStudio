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
        private int caretPos;
        private PacketSaveDataNG _sessionPacket;
        private TempPacketSaveData _exportPacket;

        public bool IsValid { get; set; }
        public string ValidationError { get; set; }

        public TempPacketSaveData ExportPacket
        {
            get => _exportPacket;
            set
            {
                _exportPacket = value;
                this.RaisePropertyChanged(nameof(ExportPacket));
            }
        }

        public PacketSaveDataNG SessionPacket
        {
            get => _sessionPacket;
            set
            {
                _sessionPacket = value;
                this.RaisePropertyChanged(nameof(SessionPacket));
                if (value == null) return;
                // Check for specific header name
                if (value.Metadata.TryGetValue(PackSaveDataNGMetaFields.HEADER_FIELD, out string newHeader))
                {
                    Header = newHeader;
                }
                if (value.Metadata.TryGetValue(PackSaveDataNGMetaFields.CARET_POS_FIELD, out string newCaretPosString))
                {
                    if (int.TryParse(newCaretPosString, out int newCaretPosInt))
                    {
                        CaretPosition = newCaretPosInt;
                    }
                }
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
        public int CaretPosition
        {
            get => caretPos;
            set
            {
                caretPos = value;
                this.RaisePropertyChanged(nameof(CaretPosition));
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

        public void NormalizeHex()
        {
            var expPacket = ExportPacket;
            SessionPacket = new PacketSaveDataNG(HexStreamType.Raw, expPacket.Data.ToHex());
        }
    }
}