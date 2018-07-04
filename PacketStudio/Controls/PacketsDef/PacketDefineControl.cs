using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using PacketDotNet;
using PacketStudio.Core;
using PacketStudio.DataAccess;

namespace PacketStudio.Controls.PacketsDef
{

    public partial class PacketDefineControl : UserControl
    {
        private HexDeserializer _deserializer;

        public event EventHandler<EventArgs> ContentChanged;

        public override string Text => hexBox.Text;

        public HexStreamType PacketType { get; private set; }
        public bool IsHexStream => _deserializer.IsHexStream(Text);

        private Dictionary<HexStreamType, int> _streamTypeToFirstOffset { get; set; } =
            new Dictionary<HexStreamType, int>
            {
                { HexStreamType.RawEthernet , 0 },
                { HexStreamType.UdpPayload , 42 },
                { HexStreamType.SctpPayload , 62 },
                { HexStreamType.IpPayload , 34 },
            };

        private Dictionary<HexStreamType, Func<Control>> _streamTypeToPacketDefineControlFactory { get; set; } =
            new Dictionary<HexStreamType, Func<Control>>
            {
                { HexStreamType.RawEthernet , ()=>{return new RawPacketDefControl();} },
                { HexStreamType.UdpPayload , ()=>{return new UdpPacketDefControl();} },
                { HexStreamType.SctpPayload, ()=>{return new SctpPacketDefControl();} },
                { HexStreamType.IpPayload , ()=>{return new IpPacketDefControl();} },
            };



        public PacketDefineControl() : this(null)
        {
        }

        public PacketDefineControl(PacketSaveData data)
        {
            _deserializer = new HexDeserializer();
            InitializeComponent();

            hexBox.Text = data?.Text ?? "";
            HexStreamType type = data?.Type ?? HexStreamType.RawEthernet;
            switch (type)
            {
                case HexStreamType.UdpPayload:
                    packetTypeListBox.SelectedIndex = 1;
                    break;
                case HexStreamType.SctpPayload:
                    packetTypeListBox.SelectedIndex = 2;
                    break;
                case HexStreamType.RawEthernet:
                default:
                    packetTypeListBox.SelectedIndex = 0;
                    break;
            }
            PacketType = type;

            IPacketDefiner definer = GetCurrentDefiner();

            if (data != null)
            {
                definer.LoadSaveData(data);
            }
        }

        public byte[] GetPacket()
        {
            byte[] bytes;
            string rawHex = hexBox.Text;
            try
            {
                bytes = _deserializer.Deserialize(rawHex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed deserializing Input:\r\n{ex.Message}");
            }


            IPacketDefiner definer = GetCurrentDefiner();

            return definer.Generate(bytes);
        }

        public PacketSaveData GetSaveData()
        {
            IPacketDefiner definer = GetCurrentDefiner();

            return definer.GetSaveData(hexBox.Text);
        }

        public void SetSelection(int firstNibbleIndex, int bytesLength)
        {
            // Can only show if this is a hex stream
            if (!IsHexStream)
                return;

            bool isSecondNibble = firstNibbleIndex % 2 == 1;
            int firstByteIndex = firstNibbleIndex / 2;
            if (IsHexStream)
            {
                int headersLengthToRemove = _streamTypeToFirstOffset[PacketType];
                firstByteIndex -= headersLengthToRemove;

                if (firstByteIndex < 0 || bytesLength <= 0)
                {
                    hexBox.SelectionStart = 0;
                    hexBox.SelectionLength = 0;
                    return;
                }
                hexBox.SelectionStart = firstByteIndex * 2 + (isSecondNibble ? 1 : 0);
                hexBox.SelectionLength = bytesLength * 2;
            }
            hexBox.Select();
        }


        // Hack to allow CTRL+A to select all
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                hexBox.SelectAll();
                // These prevent from the control to make the annoying 'error' sound
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void InvokeContentChanged() => ContentChanged?.Invoke(this, new EventArgs());

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            InvokeContentChanged();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PacketType = (HexStreamType)packetTypeListBox.SelectedIndex;
            IPacketDefiner lastDefiner = GetCurrentDefiner();
            lastDefiner.PacketChanged -= PacketDefiner_PacketChanged;
            packetDefPanel.Controls.Clear();
            Control packetDefControl;
            Func<Control> controlCreatorFunc;
            if(!_streamTypeToPacketDefineControlFactory.TryGetValue(PacketType,out controlCreatorFunc))
            {
                // Couldn't find a creation function
                throw new ArgumentException($"Can't find creation method for packets of type '{PacketType}'.\r\n" +
                    $"This could be the result of sloppy addition of new encapsulation\r\n" +
                    $"type without updating the {nameof(PacketDefineControl)} class.");
            }
            // Calling C'tor of the desired packet def control
            packetDefControl = controlCreatorFunc();
            
            packetDefPanel.Controls.Add(packetDefControl);
            packetDefControl.Dock = DockStyle.Fill;
            ((IPacketDefiner)packetDefControl).PacketChanged += PacketDefiner_PacketChanged;

            InvokeContentChanged();
        }

        private void PacketDefiner_PacketChanged(object sender, EventArgs eventArgs)
        {
            InvokeContentChanged();
        }

        private IPacketDefiner GetCurrentDefiner()
        {
            IPacketDefiner definer = packetDefPanel.Controls?[0] as IPacketDefiner;
            if (definer == null)
            {
                throw new Exception("Can't find packet definer control???");
            }

            return definer;
        }
    }
}
