using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using PacketDotNet.Sctp.Chunks;
using PacketStudio.DataAccess;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("SCTP Packet")]
    [Order(3)]
    public partial class SctpTemplateControl : UserControl, IPacketTemplateControl
    {
        private static SctpPacketFactory _factory = new SctpPacketFactory();
        private static Dictionary<string, SctpPayloadProtocol> _singletonMap;

        private static Dictionary<string, SctpPayloadProtocol> _map
        {
            get
            {
                if (_singletonMap == null)
                {
                    _singletonMap = new Dictionary<string, SctpPayloadProtocol>();
                    for (int i = 0; i < 100; i++)
                    {
                        SctpPayloadProtocol type = (SctpPayloadProtocol)i;
                        string name;
                        name = Enum.IsDefined(typeof(SctpPayloadProtocol), i) ? type.ToString() : "Unallocated";
                        _singletonMap[name] = type;
                    }
                }
                return _singletonMap;
            }
        }


        public SctpTemplateControl()
        {
            InitializeComponent();

            foreach (KeyValuePair<string, SctpPayloadProtocol> nameAndType in _map)
            {
                SctpPayloadProtocol type = nameAndType.Value;
                string name = nameAndType.Key;
                string label = $"{(int) type} {name}";
                ppidBox.Items.Add(label);
            }

            // Setting default to Ethernet
            ppidBox.SelectedIndex = (int)SctpPayloadProtocol.Reserved;

            streamTextbox.Text = "1";
        }

        public event EventHandler Changed;

        public bool IsValidWithPayload(byte[] raw)
        {
            // Only restriction: payload can't be too long
            if (string.IsNullOrEmpty(ppidBox?.SelectionBoxItem as string))
                return false;

            string name = ppidBox.SelectionBoxItem as string;
            name = name.Substring(name.IndexOf(" ", StringComparison.Ordinal)).Trim();
            if (!_map.TryGetValue(name, out var type))
                return false;
            
            return raw.Length <= ushort.MaxValue;
        }

        public (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex)
        {
            string name = ppidBox.SelectedItem as string;
            Debug.WriteLine($@"@@@ [sctp] gen with ppid: '{name}'");
            name = name.Substring(name.IndexOf(" ", StringComparison.Ordinal)).Trim();
            if (!_map.TryGetValue(name, out var ppid))
                return (false, null, $"Bad PPID value {name} at {this.GetType()}");


            if (int.TryParse(this.streamTextbox.Text, out int streamId))
            {
                TempPacketSaveData packet = _factory.GetPacket(rawHex, streamId, ppid);
                return (true, packet, null);
            }

            return (false, null, $"Unknown error in {this.GetType()}");
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);

        private void ppidBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Debug.WriteLine($@"@@@ [sctp] SelectionChanged '{e.AddedItems[0]}', Selectd ppid (from box item): {ppidBox.SelectionBoxItem}, Selectd ppid (from item): {ppidBox.SelectedItem}");
            Changed?.Invoke(this, e);
        }
    }
}
