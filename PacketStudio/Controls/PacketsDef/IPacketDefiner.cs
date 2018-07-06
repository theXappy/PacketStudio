using System;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
	public interface IPacketDefiner
	{
		event EventHandler PacketChanged;

	    HexStreamType StreamType { get; }
        int HeadersLength { get; }
        bool IsValid { get; }
		string Error { get; }
		byte[] Generate(byte[] rawBytes);
		PacketSaveData GetSaveData(string packetHex);
		void LoadSaveData(PacketSaveData data);
	}
}