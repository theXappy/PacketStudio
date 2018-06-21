using System;
using System.Linq;

namespace PacketStudio.DataAccess
{
	[Obsolete]
	[Serializable]
	public class PacketSaveDataV1 : PacketSaveData
	{
		private readonly string _text;
		private bool _udpPayload;
		private readonly string _streamId;

		public override string Text => _text;
		public override HexStreamType Type => UdpPayload ? HexStreamType.UdpPayload : HexStreamType.RawEthernet;
		public bool UdpPayload => _udpPayload;
		public override string StreamID => _streamId;

		public PacketSaveDataV1(string text, bool udpPayload, string streamId)
		{
			_text = text;
			_udpPayload = udpPayload;
			_streamId = streamId;
		}

		public override string ToString()
		{
			return Convert.ToBase64String(Text.Select(c => (byte)c).ToArray()) + "~" +
				(UdpPayload ? "1" : "0") + "~" +
				Convert.ToBase64String(StreamID.Select(c => (byte)c).ToArray());
		}

		public static PacketSaveDataV1 Parse(string str)
		{
			string[] splitted = str.Split('~');
			string text = new string(Convert.FromBase64String(splitted[0]).Select(b => (char)b).ToArray());
			bool udpPayload = splitted[1] == "1";
			string streamID = new string(Convert.FromBase64String(splitted[2]).Select(b => (char)b).ToArray());

			return new PacketSaveDataV1(text,udpPayload,streamID);
		}
	}
}
