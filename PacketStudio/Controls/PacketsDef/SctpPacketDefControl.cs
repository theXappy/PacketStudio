using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using PacketDotNet;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{
	public partial class SctpPacketDefControl : UserControl, IPacketDefiner
	{
		public SctpPacketDefControl()
		{
			InitializeComponent();
		}

		public event EventHandler PacketChanged;
	    public HexStreamType StreamType => HexStreamType.SctpPayload;
	    public int HeadersLength => 62;
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

			PacketDotNet.Sctp.Chunks.SctpPayloadProtocol proto;
			try
			{
				if (ppidTextBox.Text.StartsWith("0x")) // Hex
				{
					proto = (PacketDotNet.Sctp.Chunks.SctpPayloadProtocol)Convert.ToInt64(ppidTextBox.Text.Substring(2), 16);
				}
				else // Dec
				{
					proto = (PacketDotNet.Sctp.Chunks.SctpPayloadProtocol)(int.Parse(ppidTextBox.Text));
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to parse UDP stream ID. Must be an integer OR hexadecimal number starting with '0x', was: '{ppidTextBox.Text}'. Inner error: {ex.Message}");
			}

			int sctpStreamId;
			if (!int.TryParse(streamIdTextBox.Text, out sctpStreamId))
			{
				throw new Exception($"Failed to parse UDP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'");
			}

			PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
			PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetPacketType.IPv4);

			bool flip = sctpStreamId < 0;
			sctpStreamId = Math.Abs(sctpStreamId);
			Random r = new Random(sctpStreamId);

			IPAddress sourceIp = new IPAddress(r.Next());
			IPAddress destIp = new IPAddress(r.Next());
			if (flip)
			{
				IPAddress tempAddress = sourceIp;
				sourceIp = destIp;
				destIp = tempAddress;
			}
			IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) { NextHeader = IPProtocolType.SCTP };
			SctpPacket sctpPacket = new SctpPacket(1, 1, 0, 0);
			var dataChunk = new PacketDotNet.Sctp.Chunks.SctpDataChunk(new PacketDotNet.Utils.ByteArraySegment(new byte[]
			{
				0x00,0x03,0x00,0x14,0x79,0x46,0x08,0xb7,0x00,0x00,0x00,0x17,0x00,0x00,0x00,0x19,0x00,0x00,0x00,0x00
			}), sctpPacket)
			{
				PayloadData = rawBytes,
				PayloadProtocol = proto
			};
			dataChunk.Length = (ushort)(16 + rawBytes.Length);
			byte[] ipPayload = sctpPacket.Bytes.Concat(dataChunk.Bytes).ToArray();
			ipPacket.PayloadData = ipPayload;
			etherPacket.PayloadPacket = ipPacket;
			return etherPacket.Bytes;
		}

		public PacketSaveData GetSaveData(string packetHex)
		{
			return new PacketSaveDataV2(packetHex, HexStreamType.SctpPayload, streamIdTextBox.Text,ppidTextBox.Text);
		}

		public void LoadSaveData(PacketSaveData data)
		{
			streamIdTextBox.Text = data.StreamID;
			var asV2 = data as PacketSaveDataV2;
			if (asV2 != null)
			{
				ppidTextBox.Text = asV2.PPID;
			}
		}

		private void streamIdTextBox_TextChanged(object sender, EventArgs e)
		{
			PacketChanged?.Invoke(this,new EventArgs());
		}

		private void ppidTextBox_TextChanged(object sender, EventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}
	}
}
