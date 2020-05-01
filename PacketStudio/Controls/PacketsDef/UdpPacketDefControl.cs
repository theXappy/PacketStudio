using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using PacketDotNet;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.Controls.PacketsDef
{
	public partial class UdpPacketDefControl : UserControl, IPacketDefiner
	{
		public UdpPacketDefControl()
		{
			InitializeComponent();
		}

		public event EventHandler PacketChanged;
	    public HexStreamType StreamType => HexStreamType.UdpPayload;
	    public LinkLayerType LinkLayer => LinkLayerType.Ethernet;
	    public int HeadersLength => 42;
	    public bool IsValid => int.TryParse(streamIdTextBox.Text, out _);

		public string Error => IsValid == false
			? $"Failed to parse UDP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'"
			: String.Empty;

		public byte[] Generate(byte[] rawBytes)
		{
			int udpStreamId;
			if (!int.TryParse(streamIdTextBox.Text, out udpStreamId))
			{
				throw new Exception($"Failed to parse UDP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'");
			}

			PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
			PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetType.IPv4);

			bool flip = udpStreamId < 0;
			udpStreamId = Math.Abs(udpStreamId);
			Random r = new Random(udpStreamId);

			IPAddress sourceIp = new IPAddress(r.Next());
			IPAddress destIp = new IPAddress(r.Next());
			if (flip)
			{
				IPAddress tempAddress = sourceIp;
				sourceIp = destIp;
				destIp = tempAddress;
			}
			IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) { Protocol = ProtocolType.Udp };
			UdpPacket udpPacket = new UdpPacket(1, 1) { PayloadData = rawBytes };
			ipPacket.PayloadPacket = udpPacket;
			etherPacket.PayloadPacket = ipPacket;
			return etherPacket.Bytes;
		}

		public PacketSaveData GetSaveData(string packetHex)
		{
		    return PacketSaveDataFactory.GetPacketSaveData(packetHex, HexStreamType.UdpPayload, LinkLayerType.Ethernet,
		        streamIdTextBox.Text, "");
		}

		public void LoadSaveData(PacketSaveData data)
		{
			streamIdTextBox.Text = data.StreamID;
		}

		private void streamIdTextBox_TextChanged(object sender, EventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}
	}
}
