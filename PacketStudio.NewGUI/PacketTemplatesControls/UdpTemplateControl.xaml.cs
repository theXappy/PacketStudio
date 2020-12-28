using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("UDP Packet")]
    [Order(2)]
    [HexStreamType(HexStreamType.UdpPayload)]
    public partial class UdpTemplateControl : UserControl, IPacketTemplateControl
    {
        private static readonly UdpPacketFactory _factory = new UdpPacketFactory();

        public UdpTemplateControl()
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
            throw new NotImplementedException();
        }

        public void LoadSaveDetails(Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);
    }
}
