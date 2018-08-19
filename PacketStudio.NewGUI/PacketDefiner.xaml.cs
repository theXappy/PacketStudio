using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PacketDotNet;
using PacketDotNet.Sctp.Chunks;
using PacketStudio.Core;
using PacketStudio.DataAccess;

namespace ByteArrayToPcap.NewGUI
{
	/// <summary>
	/// Interaction logic for PacketDefiner.xaml
	/// </summary>
	public partial class PacketDefiner : UserControl
	{
		public event EventHandler PacketChanged;

		private HexDeserializer _deserializer = new HexDeserializer();

		private UdpPacketFactory _udpPacketFactory = new UdpPacketFactory();
		private SctpPacketFactory _sctpPacketFactory = new SctpPacketFactory();

		public bool IsValid
		{
			get
			{
				if (!_deserializer.TryDeserialize(this.hexTextBox.Text, out _))
				{
					return false;
				}
				switch (packetTypeCombo.SelectedIndex)
				{
					case 1: // UDP
						string udpStreamIdStr = streamIdTextbox.Text;
						return int.TryParse(udpStreamIdStr, out _);
					case 2: // SCPT
						string sctpStreamIdStr = streamIdTextbox.Text;
					    if (!int.TryParse(sctpStreamIdStr, out _))
					        return false;
						string sctpPpid = ppidTextbox.Text;
						if (sctpPpid.StartsWith("0x")) // PPID defined with HEX value
						{
						    try
						    {
						        sctpPpid = sctpPpid.Substring(2);
						        Convert.ToInt64(sctpPpid, 16);
                            }
						    catch
						    {
						        return false;
						    }
						}
						else // PPID defined with decimal value
						{
						    return int.TryParse(sctpPpid, out _);
						}
					    break;
				}
				return true;
			}
		}

		/// <summary>
		/// Returns the errors causing this defined to show 'IsValid' as false (otherwise null)
		/// </summary>
		public string ValidationError
		{
			get
			{
				try
				{
					_deserializer.Deserialize(this.hexTextBox.Text);
				}
				catch (Exception e)
				{
					return e.Message;
				}
				switch (packetTypeCombo.SelectedIndex)
				{
					case 1: // UDP
						string udpStreamIdStr = streamIdTextbox.Text;
						try
						{
							int.Parse(udpStreamIdStr);
						}
						catch (Exception e)
						{
							return e.Message;
						}
						break;
					case 2: // SCTP
						string sctpStreamIdStr = streamIdTextbox.Text;
						try
						{
							int.Parse(sctpStreamIdStr);
						}
						catch (Exception e)
						{
							return e.Message;
						}
						string sctpPpid = ppidTextbox.Text;
						try
						{
							if (sctpPpid.StartsWith("0x")) // PPID defined with HEX value
							{
								sctpPpid = sctpPpid.Substring(2);
								Convert.ToInt32(sctpPpid, 16);
							}
							else // PPID defined with decimal value
							{
								int.Parse(sctpPpid);
							}
						}
						catch (Exception e)
						{
							return e.Message;
						}
						break;
				}

				// No errors encountered, everything's fine!
				return null;
			}
		}

		public TempPacketSaveData PacketBytes
		{
			get

			{
                // TODO
				//if (!IsValid)
				//	return new byte[0];

				//byte[] rawBytes = _deserializer.Deserialize(this.hexTextBox.Text);
				//switch (packetTypeCombo.SelectedIndex)
				//{
				//	case 0: // Raw Ethernet
				//	default:
				//		return rawBytes;
				//    case 1: // UDP
				//		int parsedUdpId = int.Parse(streamIdTextbox.Text);
				//		return _udpPacketFactory.GetPacket(rawBytes, parsedUdpId);
				//	case 2: // SCPT
				//		string sctpStreamId = streamIdTextbox.Text;
				//		int parsedSctpId = sctpStreamId.StartsWith("0x")
				//			? Convert.ToInt32(sctpStreamId.Substring(2), 16) // Parse as HEX
				//			: int.Parse(sctpStreamId); // Parse as Decimal
				//		int parsedPpid = int.Parse(ppidTextbox.Text);
				//		return _sctpPacketFactory.GetPacket(rawBytes, parsedSctpId, parsedPpid);
				//}
			    return null;
			}
		}

		public bool IsHexStream => _deserializer.IsHexStream(this.hexTextBox.Text);



		public PacketDefiner()
		{
			InitializeComponent();
			ComboBox_SelectionChanged(null, null);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (streamIdLabel == null || streamIdTextbox == null || ppidLabel == null || ppidTextbox == null)
				return;

			if (packetTypeCombo.SelectedIndex > 0) // UDP or SCTP
			{
				// Show Stream ID
				streamIdLabel.Visibility = Visibility.Visible;
				streamIdTextbox.Visibility = Visibility.Visible;
				if (packetTypeCombo.SelectedIndex == 2) // SCTP: Show PPID
				{
					ppidLabel.Visibility = Visibility.Visible;
					ppidTextbox.Visibility = Visibility.Visible;
				}
				else // UDP: Hide PPID
				{
					ppidLabel.Visibility = Visibility.Hidden;
					ppidTextbox.Visibility = Visibility.Hidden;
				}
			}
			else // Raw Ethernet
			{
				// Hide all fields
				streamIdLabel.Visibility = Visibility.Hidden;
				streamIdTextbox.Visibility = Visibility.Hidden;
				ppidLabel.Visibility = Visibility.Hidden;
				ppidTextbox.Visibility = Visibility.Hidden;

			}


			PacketChanged?.Invoke(this, new EventArgs());
		}

		private void streamIdTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}

		private void ppidTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			PacketChanged?.Invoke(this, new EventArgs());
		}

		private void packetTypeCombo_MouseEnter(object sender, MouseEventArgs e)
		{
			// Allow scrolling throu the packet types when hovering the combo box
			// ( This is the existing behaviour if the control was CLICKED before scorlling, this allows the scrolling wihtout clicking)
			packetTypeCombo.Focus();
		}

		public bool TrySelect(int byteOffset, int bytesLength)
		{
			switch (packetTypeCombo.SelectedIndex)
			{
				case 0: // Raw Ethernet
				default:
					break;
				case 1: // UDP
					byteOffset -= 42;
					break;
				case 2: // SCTP
					byteOffset -= 62;
					break;
			}
			if (byteOffset < 0 || bytesLength <= 0 || (byteOffset*2 >= hexTextBox.Text.Length))
			{
				hexTextBox.SelectionStart = 0;
				hexTextBox.SelectionLength = 0;
				return false;
			}


			hexTextBox.SelectionStart = byteOffset * 2;
			hexTextBox.SelectionLength = bytesLength * 2;
			hexTextBox.Select(byteOffset * 2, bytesLength * 2);
			return true;
		}

		private void hexTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			e.Handled = true;
		}

		internal void NormalizeHex()
		{
			bool padded = false;
			byte[] bytes = null;
			try
			{
				bytes = _deserializer.Deserialize(hexTextBox.Text);
			}
			catch (FormatException)
			{
				bool throwOrg = false;
				try
				{
					bytes = _deserializer.Deserialize(hexTextBox.Text + "0");
					padded = true;
				}
				catch
				{
					throwOrg = true;
				}
				if (throwOrg)
				{
					throw;
				}
			}
			StringBuilder sb = new StringBuilder();
			foreach (byte b in bytes)
			{
				sb.Append(b.ToString("X2"));
			}
			string normalized = sb.ToString();
			if (padded)
			{
				normalized.Substring(0, normalized.Length - 1);
			}
			hexTextBox.Text = normalized;

		}
	}

	public class UdpPacketFactory
	{
		public byte[] GetPacket(byte[] payload, int streamId)
		{
			PhysicalAddress emptyAddress = PhysicalAddress.Parse("000000000000");
			PacketDotNet.EthernetPacket etherPacket = new EthernetPacket(emptyAddress, emptyAddress, EthernetPacketType.IPv4);

			bool flip = streamId < 0;
			streamId = Math.Abs(streamId);
			Random r = new Random(streamId);

			IPAddress sourceIp = new IPAddress(r.Next());
			IPAddress destIp = new IPAddress(r.Next());
			if (flip)
			{
				IPAddress tempAddress = sourceIp;
				sourceIp = destIp;
				destIp = tempAddress;
			}
			IPv4Packet ipPacket = new IPv4Packet(sourceIp, destIp) { NextHeader = IPProtocolType.UDP };
			UdpPacket udpPacket = new UdpPacket(1, 1) { PayloadData = payload };
			ipPacket.PayloadPacket = udpPacket;
			etherPacket.PayloadPacket = ipPacket;
			return etherPacket.Bytes;
		}
	}

	public class SctpPacketFactory
	{
		public byte[] GetPacket(byte[] payload, int sctpStreamId, int ppid)
		{
			PacketDotNet.Sctp.Chunks.SctpPayloadProtocol proto = (SctpPayloadProtocol)ppid;

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
				PayloadData = payload,
				PayloadProtocol = proto
			};
			dataChunk.Length = (ushort)(16 + payload.Length);
			byte[] ipPayload = sctpPacket.Bytes.Concat(dataChunk.Bytes).ToArray();
			ipPacket.PayloadData = ipPayload;
			etherPacket.PayloadPacket = ipPacket;
			return etherPacket.Bytes;
		}
	}
}
