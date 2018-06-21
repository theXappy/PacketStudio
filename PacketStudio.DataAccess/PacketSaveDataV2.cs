using System;
using System.Linq;

namespace PacketStudio.DataAccess
{
	[Serializable]
	public class PacketSaveDataV2 : PacketSaveData
	{
		private readonly string _text;
		private readonly HexStreamType _type;
		private readonly string _streamId;
		private string _ppid;

		public override string Text => _text;
		public override HexStreamType Type => _type;
		public override string StreamID => _streamId;
		public string PPID => _ppid;

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
			       Convert.ToBase64String(PPID.Select(c => (byte)c).ToArray()) ;
		}

		public static PacketSaveDataV2 Parse(string str)
		{
			string[] splitted = str.Split('~');
			string text = new string(Convert.FromBase64String(splitted[0]).Select(b => (char)b).ToArray());
			HexStreamType type = (HexStreamType)(Enum.Parse(typeof(HexStreamType),splitted[1]));
			string streamID = new string(Convert.FromBase64String(splitted[2]).Select(b => (char)b).ToArray());
			string ppid = new string(Convert.FromBase64String(splitted[3]).Select(b => (char)b).ToArray());

			return new PacketSaveDataV2(text,type,streamID,ppid);
		}
	}
}