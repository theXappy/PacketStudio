﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using PacketDotNet;
using PacketDotNet.Sctp.Chunks;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
using PacketStudio.NewGUI.ProtocolFactories;

namespace PacketStudio.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("SCTP Packet")]
    [Order((int)HexStreamType.SctpPayload)]
    [HexStreamType(HexStreamType.SctpPayload)]
    public partial class SctpTemplateControl : UserControl, IPacketTemplateControl
    {
        public int GetHeadersLength() => EthernetFields.HeaderLength + IPv4Fields.HeaderLength + SctpFields.HeaderLength + SctpDataChunkFields.HeaderLength;

        private static readonly SctpPacketFactory _factory = new SctpPacketFactory();
        private static Dictionary<string, SctpPayloadProtocol> _singletonMap;

        private static Dictionary<string, SctpPayloadProtocol> PpidsMap
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

            foreach (KeyValuePair<string, SctpPayloadProtocol> nameAndType in PpidsMap)
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
            if (!PpidsMap.TryGetValue(name, out _))
                return false;
            
            return raw.Length <= ushort.MaxValue;
        }

        public (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex)
        {
            string name = ppidBox.SelectedItem as string;
            name = name.Substring(name.IndexOf(" ", StringComparison.Ordinal)).Trim();
            if (!PpidsMap.TryGetValue(name, out var ppid))
                return (false, null, $"Bad PPID value {name} at {this.GetType()}");


            if (int.TryParse(this.streamTextbox.Text, out int streamId))
            {
                TempPacketSaveData packet = _factory.GetPacket(rawHex, streamId, ppid);
                return (true, packet, null);
            }

            return (false, null, $"Unknown error in {this.GetType()}");
        }

        public Dictionary<string, string> GenerateSaveDetails()
        {
            var dict = new Dictionary<string,string>();
            dict[PacketSaveDataNGProtoFields.STREAM_ID] = streamTextbox.Text;
            dict[PacketSaveDataNGProtoFields.PPID_ID] = ppidBox.Text;
            return dict;
        }

        public void LoadSaveDetails(Dictionary<string, string> data)
        {
            if(data.TryGetValue(PacketSaveDataNGProtoFields.STREAM_ID, out string streamId))
                streamTextbox.Text = streamId;
            if(data.TryGetValue(PacketSaveDataNGProtoFields.PPID_ID, out string ppid))
                ppidBox.Text = ppid;
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);

        private void PpidBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Changed?.Invoke(this, e);

    }
}
