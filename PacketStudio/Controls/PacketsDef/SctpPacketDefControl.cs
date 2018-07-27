using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using PacketDotNet;
using PacketDotNet.Sctp.Chunks;
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
	    public bool IsValid => int.TryParse(streamIdTextBox.Text, out _) && TryParsePPID(ppidTextBox.Text,out _);

		public string Error => IsValid == false
			? $"Failed to parse SCTP stream ID or PPID."
			: String.Empty;

		public byte[] Generate(byte[] rawBytes)
        {
            int udpStreamId;
            if (!int.TryParse(streamIdTextBox.Text, out udpStreamId))
            {
                throw new Exception($"Failed to parse SCTP stream ID. Must be an integer, was: '{streamIdTextBox.Text}'");
            }

            SctpPayloadProtocol proto = ParsePPID(ppidTextBox.Text);

            int sctpStreamId;
            if (!int.TryParse(streamIdTextBox.Text, out sctpStreamId))
            {
                throw new Exception($"Failed to parse SCTP PPID. Must be an integer, was: '{streamIdTextBox.Text}'");
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
            SctpDataChunk dataChunk = new PacketDotNet.Sctp.Chunks.SctpDataChunk(new PacketDotNet.Utils.ByteArraySegment(new byte[]
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

        private bool TryParsePPID(string txt,out SctpPayloadProtocol output)
        {
            output = 0;
            try
            {
                output = ParsePPID(txt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <param name="txt">Either a raw decimal value or hex value prepended with '0x' </param>
        private SctpPayloadProtocol ParsePPID(string txt)
        {
            PacketDotNet.Sctp.Chunks.SctpPayloadProtocol proto;
            try
            {
                if (txt.StartsWith("0x")) // Hex
                {
                    proto = (PacketDotNet.Sctp.Chunks.SctpPayloadProtocol)Convert.ToInt64(txt.Substring(2), 16);
                }
                else // Dec
                {
                    proto = (PacketDotNet.Sctp.Chunks.SctpPayloadProtocol)(int.Parse(txt));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse SCTP PPID. Must be an integer OR hexadecimal number starting with '0x', was: '{ppidTextBox.Text}'. Inner error: {ex.Message}");
            }

            return proto;
        }

        public PacketSaveData GetSaveData(string packetHex)
		{
			return new PacketSaveDataV2(packetHex, HexStreamType.SctpPayload, streamIdTextBox.Text,ppidTextBox.Text);
		}

		public void LoadSaveData(PacketSaveData data)
		{
			streamIdTextBox.Text = data.StreamID;
			PacketSaveDataV2 asV2 = data as PacketSaveDataV2;
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

            // Trying to resolve the PPID so the use know what protocol wireshark is going to try
		    try
		    {
		        SctpPayloadProtocol spp = ParsePPID(ppidTextBox.Text);
		        if (!Enum.IsDefined(typeof(SctpPayloadProtocol), spp))
		        {
		            resProtoValueLabel.Text = "Unassigned";
		        }
		        else
		        {
		            resProtoValueLabel.Text = spp.ToString();
		        }
		    }
		    catch
		    {
		        resProtoValueLabel.Text = "ERROR";
            }
		}
	}
}
