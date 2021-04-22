using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.NewGUI.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TabItemViewModel : ViewModelBase
    {
        private HexStreamType packetType;
        private string content;
        private string header;
        private int caretPos;
        private int selStart;
        private int selLen;
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
                OnPropertyChanged(nameof(ExportPacket));
            }
        }

        public PacketSaveDataNG SessionPacket
        {
            get => _sessionPacket;
            set
            {
                _sessionPacket = value;
                OnPropertyChanged(nameof(SessionPacket));
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
                OnPropertyChanged(nameof(Header));
            }
        }
        public int CaretPosition
        {
            get => caretPos;
            set
            {
                caretPos = value;
                OnPropertyChanged(nameof(CaretPosition));
            }
        }
        public int SelectionLength
        {
            get => selLen;
            set
            {
                selLen = value;
                OnPropertyChanged(nameof(SelectionLength));
            }
        }
        public int SelectionStart
        {
            get => selStart;
            set
            {
                selStart = value;
                OnPropertyChanged(nameof(SelectionStart));
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public HexStreamType PacketType
        {
            get => packetType;
            set
            {
                packetType = value;
                OnPropertyChanged(nameof(PacketType));
            }
        }

        public void Load(PacketSaveDataNG psd)
        {
            SessionPacket = psd;
            Content = psd.PacketData;
            PacketType = psd.Type;
        }

        public void NormalizeHex()
        {
            var expPacket = ExportPacket;
            byte[] data = expPacket.Data;
            if (data != null)
            {
                SessionPacket = new PacketSaveDataNG(HexStreamType.Raw, expPacket.Data.ToHex());
                Content = data.ToHex();
                PacketType = HexStreamType.Raw;
            }
            else
            {
                throw new Exception("Packet's Hex is invalid");
            }
        }
    }
}