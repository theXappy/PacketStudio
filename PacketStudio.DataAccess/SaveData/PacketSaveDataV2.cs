using System;
using System.Linq;
using PcapngFile;

namespace PacketStudio.DataAccess.SaveData
{
    [Obsolete]
    [Serializable]
	public class PacketSaveDataV2 : PacketSaveData
	{
	    public const string MAGIC_WORD = "P0NTI4K";
	    public override string MagicWord => MAGIC_WORD;

        private readonly string _text;
		private readonly HexStreamType _type;
		private readonly string _streamId;
		private string _ppid;

	    public override string Text => _text;
		public override HexStreamType Type => _type;
	    public override string LinkLayerType => ((byte)LinkType.Ethernet).ToString();
        public override string StreamID => _streamId;
	    public override string PayloadProtoId => _ppid;
        public override string Extension => "";

        public PacketSaveDataV2(string text, HexStreamType type, string streamId, string ppid)
		{
			_text = text;
			_type = type;
			_streamId = streamId;
			_ppid = ppid;
		}

		public override string ToString()
		{
			return Convert.ToBase64String(Text.Select(c => (byte)c).ToArray()) + "~" +
			       Type.ToString() + "~" +
			       Convert.ToBase64String(StreamID.Select(c => (byte)c).ToArray()) + "~" +
			       Convert.ToBase64String(PayloadProtoId.Select(c => (byte)c).ToArray()) ;
		}

		public static PacketSaveDataV2 Parse(string str)
		{
			string[] splitted = str.Split('~');
			string text = new string(Convert.FromBase64String(splitted[0]).Select(b => (char)b).ToArray());

		    string hexStreamText = splitted[1];
            // HACK: The enum member used to be called Raw Ethernet before it turned into the more generic 'raw' option
		    if (hexStreamText == "RawEthernet")
		    {
		        hexStreamText = "Raw";
		    }
            HexStreamType type = (HexStreamType)(Enum.Parse(typeof(HexStreamType),hexStreamText));

			string streamID = new string(Convert.FromBase64String(splitted[2]).Select(b => (char)b).ToArray());
			string ppid = new string(Convert.FromBase64String(splitted[3]).Select(b => (char)b).ToArray());

			return new PacketSaveDataV2(text,type,streamID,ppid);
		}
	}
}