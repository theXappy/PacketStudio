using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using PacketDotNet;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
	public partial class IpPacketDefControl : UserControl, IPacketDefiner
	{
		public IpPacketDefControl()
		{
			InitializeComponent();
		}

		public event EventHandler PacketChanged;
		public bool IsValid => int.TryParse(streamIdTextBox.Text, out _);

		public string Error => IsValid == false
			? $"Failed to parse UDP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'"
			: String.Empty;

		public byte[] Generate(byte[] rawBytes)
		{
			int StreamId;
			if (!int.TryParse(streamIdTextBox.Text, out StreamId))
			{
				throw new Exception($"Failed to parse IP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'");
			}
			int nextProto;
			try
			{
				if (nextProtoTextBox.Text.StartsWith("0x")) // Hex
				{
					nextProto = Convert.ToInt32(nextProtoTextBox.Text.Substring(2), 16);
				}
				else // Dec
				{
					nextProto = (int.Parse(nextProtoTextBox.Text));
				}
			}
			catch (Exception)
			{
				throw new Exception($"Failed to parse Next Protocol. Must be an integer or hex (with 0x), was: '{nextProtoTextBox.Text}'");
			}

			PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
			PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetPacketType.IpV4);

			bool flip = StreamId < 0;
			StreamId = Math.Abs(StreamId);
			Random r = new Random(StreamId);

			IPAddress sourceIp = new IPAddress(r.Next());
			IPAddress destIp = new IPAddress(r.Next());
			if (flip)
			{
				IPAddress tempAddress = sourceIp;
				sourceIp = destIp;
				destIp = tempAddress;
			}
			IPProtocolType type = (IPProtocolType) nextProto;
			IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) { NextHeader = type };
			ipPacket.PayloadData = rawBytes;
			etherPacket.PayloadPacket = ipPacket;
			return etherPacket.Bytes;
		}

		public PacketSaveData GetSaveData(string packetHex)
		{
			return new PacketSaveDataV2(packetHex,HexStreamType.IpPayload, streamIdTextBox.Text,nextProtoTextBox.Text);
		}

		public void LoadSaveData(PacketSaveData data)
		{
			streamIdTextBox.Text = data.StreamID;
			var asV2 = data as PacketSaveDataV2;
			if (asV2 != null)
			{
				nextProtoTextBox.Text = asV2.Text;
			}
		}

		private void streamIdTextBox_TextChanged(object sender, EventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}
	}
}
