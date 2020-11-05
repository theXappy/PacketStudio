using System;
using System.ComponentModel;
using System.Windows.Controls;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("UDP Packet")]
    public partial class UdpTemplateControl : UserControl, IPacketTemplateControl
    {
        private static UdpPacketFactory _factory = new UdpPacketFactory();

        public UdpTemplateControl()
        {
            InitializeComponent();
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

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);
    }
}
