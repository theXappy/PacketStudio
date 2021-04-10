using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using PacketStudio.NewGUI.ProtocolFactories;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("TCP Packet")]
    [Order(1)]
    [HexStreamType(HexStreamType.TcpPayload)]
    public partial class TcpTemplateControl : UserControl, IPacketTemplateControl
    {
        private static readonly TcpPacketFactory _factory = new TcpPacketFactory();

        public TcpTemplateControl()
        {
            InitializeComponent();
            streamTextbox.Text = "1";
        }

        public event EventHandler Changed;

        public bool IsValidWithPayload(byte[] raw)
        {
            // Only restriction: payload can't be too long
            return raw.Length <= ushort.MaxValue;
        }

        public (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex)
        {
            if (int.TryParse(this.streamTextbox.Text, out int streamId))
            {
                TempPacketSaveData packet = _factory.GetPacket(rawHex, streamId);
                return (true, packet, null);
            }

            return (false, null, $"Unknown error in {this.GetType()}");
        }

        public Dictionary<string, string> GenerateSaveDetails()
        {
            var dict = new Dictionary<string,string>();
            dict[PacketSaveDataNGProtoFields.STREAM_ID] = streamTextbox.Text;
            return dict;
        }

        public void LoadSaveDetails(Dictionary<string, string> data)
        {
            string streamId;
            if(data.TryGetValue(PacketSaveDataNGProtoFields.STREAM_ID, out streamId))
                streamTextbox.Text = streamId;
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);
    }
}
