﻿using System;
using System.ComponentModel;
using System.Windows.Controls;
using PacketStudio.DataAccess;

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

        public string GenerateSaveDataJson()
        {
            throw new NotImplementedException();
        }

        public void LoadSaveDataJson(string json)
        {
            throw new NotImplementedException();
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);
    }
}