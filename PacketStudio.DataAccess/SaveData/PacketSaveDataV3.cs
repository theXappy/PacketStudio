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
        private readonly string _linkLayer;
        private readonly HexStreamType _type;
        private readonly string _streamId;
        private string _ppid;
        private string _extension;

        public override string Text => _text;
        public override HexStreamType Type => _type;
        public override string LinkLayerType => _linkLayer;
        public override string StreamID => _streamId;
        public override string PayloadProtoId => _ppid;
        public string Extension => _extension;

        public PacketSaveDataV3(string text, HexStreamType type, string linkLayer, string streamId, string ppid, string extension)
        {
            _text = text;
            _linkLayer = linkLayer;
            _type = type;
            _streamId = streamId;
            _ppid = ppid;
            _extension = extension;
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

            return new PacketSaveDataV3(text,type, linkLayerType,streamID, ppid, ext);
        }
    }
}