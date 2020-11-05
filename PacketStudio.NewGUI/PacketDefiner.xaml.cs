using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PacketDotNet;
using PacketDotNet.Sctp.Chunks;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.NewGUI.PacketTemplatesControls;
using UserControl = System.Windows.Controls.UserControl;

namespace PacketStudio.NewGUI
{
    /// <summary>
    /// Interaction logic for PacketDefiner.xaml
    /// </summary>
    public partial class PacketDefiner : UserControl
    {
        public event EventHandler PacketChanged;

        private HexDeserializer _deserializer = new HexDeserializer();

        private IPacketTemplateControl PacketTemplateControl => packetTemplatePanel.Children.Count != 0 ? (packetTemplatePanel.Children[0] as IPacketTemplateControl) : null;

        public bool IsValid =>
            _deserializer.TryDeserialize(hexTextBox.Text, out var data) &&
            PacketTemplateControl.IsValidWithPayload(data);

        /// <summary>
        /// Returns the errors causing this defined to show 'IsValid' as false (otherwise null)
        /// </summary>
        public string ValidationError
        {
            get
            {
                // TODO
                return null;
            }
        }

        public TempPacketSaveData PacketBytes
        {
            get
            {
                IPacketTemplateControl templateControl = PacketTemplateControl;
                if (templateControl == null)
                    return null;

                if (!_deserializer.TryDeserialize(this.hexTextBox.Text, out byte[] bytes)) return null;

                var (success, res, _) = templateControl.GeneratePacket(bytes);
                return success ? res : null;

            }
        }

        public bool IsHexStream => _deserializer.IsHexStream(this.hexTextBox.Text);

        public PacketDefiner()
        {
            InitializeComponent();
        }

        private void hexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PacketChanged?.Invoke(this, new EventArgs());
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

        private Dictionary<ListBoxItem, Func<UserControl>> _templatesBoxItemsToControlsCreators = new Dictionary<ListBoxItem, Func<UserControl>>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            templatesListBox.Items.Clear();

            var assm = this.GetType().Assembly;
            var packetTemplateTypes = assm.GetTypes().Where(type => typeof(IPacketTemplateControl).IsAssignableFrom(type) && !type.IsInterface);
            foreach (var packetTemplateType in packetTemplateTypes)
            {
                DisplayNameAttribute attr = packetTemplateType.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                var name = attr.DisplayName;


                ListBoxItem lbi = new ListBoxItem()
                {
                    Content = name
                };

                _templatesBoxItemsToControlsCreators[lbi] = () => (UserControl) Activator.CreateInstance(packetTemplateType);

                templatesListBox.Items.Add(lbi);
            }

            templatesListBox.SelectedIndex = 0;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (templatesListBox.SelectedItem is ListBoxItem lbi)
            {
                if (_templatesBoxItemsToControlsCreators.TryGetValue(lbi, out var creator))
                {
                    if (packetTemplatePanel.Children.Count != 0)
                    {
                        var lastTemplateControl = packetTemplatePanel.Children[0] as IPacketTemplateControl;
                        lastTemplateControl.Changed -= PacketTemplateControlChanged;
                    }
                    packetTemplatePanel.Children.Clear();

                    var newControl = creator();
                    if(newControl is IPacketTemplateControl newPacketTemplateControl)
                        newPacketTemplateControl.Changed += PacketTemplateControlChanged;
                    packetTemplatePanel.Children.Add(newControl);
                }
            }
        }

        private void PacketTemplateControlChanged(object sender, EventArgs e) => this.PacketChanged?.Invoke(this,e);

    }

    public class UdpPacketFactory
    {
        public TempPacketSaveData GetPacket(byte[] payload, int streamId)
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
            return new TempPacketSaveData(etherPacket.Bytes, LinkLayerType.Ethernet);
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
