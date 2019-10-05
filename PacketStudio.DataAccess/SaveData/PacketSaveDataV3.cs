using System;
using System.Linq;

namespace PacketStudio.DataAccess.SaveData
{
    [Serializable]
    public class PacketSaveDataV3 : PacketSaveData
    {
        public const string MAGIC_WORD = "M4NTR4";
        public override string MagicWord => MAGIC_WORD;

        private readonly string _text;
        private readonly string _linkLayerType;
        private readonly HexStreamType _type;
        private readonly string _streamId;
        private string _payloadProtoId;
        private string _extension;

        public override string Text => _text;
        public override HexStreamType Type => _type;
        public override string LinkLayerType => _linkLayerType;
        public override string StreamID => _streamId;
        public override string PayloadProtoId => _payloadProtoId;
        public override string Extension => _extension;

        public PacketSaveDataV3(string text, HexStreamType type, string linkLayerType, string streamId, string payloadProtoId, string extension)
        {
            _text = text;
            _linkLayerType = linkLayerType;
            _type = type;
            _streamId = streamId;
            _payloadProtoId = payloadProtoId;
            _extension = extension;
        }

        public void SetExtension(string ext)
        {
            _extension = ext;
        }

        public override string ToString()
        {
            return Convert.ToBase64String(Text.Select(c => (byte)c).ToArray()) + "~" +
                   Type + "~" +
                   Convert.ToBase64String(LinkLayerType.Select(c => (byte)c).ToArray()) + "~" +
                   Convert.ToBase64String(StreamID.Select(c => (byte)c).ToArray()) + "~" +
                   Convert.ToBase64String(PayloadProtoId.Select(c => (byte)c).ToArray()) + "~" +
                   Convert.ToBase64String(Extension.Select(c => (byte)c).ToArray()) + "~";
        }

        public static PacketSaveDataV3 Parse(string str)
        {
            string[] splitted = str.Split('~');
            string text = new string(Convert.FromBase64String(splitted[0]).Select(b => (char)b).ToArray());

            string hexStreamText = splitted[1];
            // HACK: The enum member used to be called Raw Ethernet before it turned into the more generic 'raw' option
            if (hexStreamText == "RawEthernet")
            {
                hexStreamText = "Raw";
            }
            HexStreamType type = (HexStreamType)(Enum.Parse(typeof(HexStreamType), hexStreamText));

            string linkLayerType = new string(Convert.FromBase64String(splitted[2]).Select(b => (char)b).ToArray());
            string streamID = new string(Convert.FromBase64String(splitted[3]).Select(b => (char)b).ToArray());
            string ppid = new string(Convert.FromBase64String(splitted[4]).Select(b => (char)b).ToArray());
            string ext = new string(Convert.FromBase64String(splitted[5]).Select(b => (char)b).ToArray());

            return new PacketSaveDataV3(text, type, linkLayerType, streamID, ppid, ext);
        }

        protected bool Equals(PacketSaveDataV3 other)
        {
            return string.Equals(Text, other.Text) && string.Equals(LinkLayerType, other.LinkLayerType) &&
                   Type == other.Type && string.Equals(StreamID, other.StreamID) &&
                   string.Equals(PayloadProtoId, other.PayloadProtoId) && string.Equals(Extension, other.Extension);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PacketSaveDataV3)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LinkLayerType != null ? LinkLayerType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Type;
                hashCode = (hashCode * 397) ^ (StreamID != null ? StreamID.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PayloadProtoId != null ? PayloadProtoId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Extension != null ? Extension.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}