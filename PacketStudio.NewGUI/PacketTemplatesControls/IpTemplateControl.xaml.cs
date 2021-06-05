using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using PacketDotNet;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using PacketStudio.NewGUI.ProtocolFactories;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("IP Packet")]
    [Order(1)]
    [HexStreamType(HexStreamType.IpPayload)]
    public partial class IpTemplateControl : UserControl, IPacketTemplateControl
    {
        public int GetHeadersLength() => EthernetFields.HeaderLength + IPv4Fields.HeaderLength;

        private static readonly IpPacketFactory _factory = new IpPacketFactory();

        public IpTemplateControl()
        {
            InitializeComponent();
            streamTextbox.Text = "1";
            protocolTextbox.Text = "1";
        }

        public event EventHandler Changed;

        public bool IsValidWithPayload(byte[] raw)
        {
            // Assert we have a valid stream ID
            if (String.IsNullOrWhiteSpace(this.streamTextbox.Text)) { 
                return false;
            }
            if (!int.TryParse(this.streamTextbox.Text, out _)) {
                return false;
            }
            
            // Assert we have a valid protocol number
            if (String.IsNullOrWhiteSpace(this.protocolTextbox.Text)) { 
                return false;
            }
            if (!int.TryParse(this.protocolTextbox.Text, out int protoVal)) {
                return false;
            }
            if (protoVal < 0 || protoVal > 255) {
                return false;
            }

            // Only restriction left: payload can't be too long
            return raw.Length <= ushort.MaxValue;
        }

        public (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex)
        {
            if (int.TryParse(this.streamTextbox.Text, out int streamId))
            {
                if (int.TryParse(this.protocolTextbox.Text, out int protoInt))
                {
                    TempPacketSaveData packet = _factory.GetPacket(rawHex, streamId, (byte)protoInt);
                    return (true, packet, null);
                }
            }

            return (false, null, $"Unknown error in {this.GetType()}");
        }

        public Dictionary<string, string> GenerateSaveDetails()
        {
            var dict = new Dictionary<string, string>();
            dict[PacketSaveDataNGProtoFields.STREAM_ID] = streamTextbox.Text;
            return dict;
        }

        public void LoadSaveDetails(Dictionary<string, string> data)
        {
            string streamId;
            if (data.TryGetValue(PacketSaveDataNGProtoFields.STREAM_ID, out streamId))
                streamTextbox.Text = streamId;
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);

        private void ProtocolTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);
    }
}
