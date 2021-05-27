using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using log4net;
using PacketStudio.Core;
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

    public class SessionPacketViewModel : ViewModelBase
    {
        private ILog _looger = LogManager.GetLogger(typeof(SessionPacketViewModel));

        private HexStreamType packetType;
        private string content;
        private int header;
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

        private PacketSaveDataNG _initalState = null;

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
                    Header = int.Parse(newHeader);
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

        public int Header
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
                if (SessionPacket != null)
                {
                    SessionPacket.PacketData = content;
                }

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

        // Note that IsModified is 'true' if _initalState is 'null' because that means we don't have
        // a packet we dereive from and any state of the current packet is "New" (and hence "Modified" compared to 'null')
        public bool IsModified
        {
            get
            {
                _looger.Debug($"IsModifed Invoked. _initialState null = {_initalState == null}, SessionPacket: {this.SessionPacket}, Are Equals: {_initalState?.Equals(SessionPacket)}");

                return !_initalState?.Equals(this.SessionPacket) ?? true;
            }
        }


        public void LoadInitialState(PacketSaveDataNG psd)
        {
            _initalState = psd.Clone() as PacketSaveDataNG;

            SessionPacket = psd;
            Content = psd.PacketData;
            PacketType = psd.Type;
        }

        public void FlattenStack()
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

        internal void NormalizeHex()
        {
            var _deserializer = new HexDeserializer();
            byte[] normalizedBytes = _deserializer.Deserialize(Content); // Might throw
            Content = normalizedBytes.ToHex();
        }


        public override string ToString()
        {
            return $"Header: {header}, HexType: {this.PacketType}, Content: {content}";
        }
    }
}