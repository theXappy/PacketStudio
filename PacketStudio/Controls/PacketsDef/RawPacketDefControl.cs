using System;
using System.Windows.Forms;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
	public partial class RawPacketDefControl : UserControl, IPacketDefiner
	{
	    public RawPacketDefControl()
		{
			InitializeComponent();
		}

        // Unused in this class
	    event EventHandler IPacketDefiner.PacketChanged
	    {
	        add {}
	        remove {}
	    }

	    public bool IsValid => true;
		public string Error => String.Empty;

		public byte[] Generate(byte[] rawBytes)
		{
			return rawBytes;
		}

		public PacketSaveData GetSaveData(string packetHex)
		{
			return new PacketSaveDataV2(packetHex,HexStreamType.RawEthernet,"1","1");
		}

		public void LoadSaveData(PacketSaveData data)
		{
		}
	}
}
