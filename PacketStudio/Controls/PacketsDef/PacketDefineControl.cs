using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Be.Windows.Forms;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.Controls.PacketsDef
{
    public partial class PacketDefineControl : UserControl
    {
        private readonly HexDeserializer _deserializer;
        private HexTypeWrapper _packetType;

        public event EventHandler<EventArgs> ContentChanged;

        public override string Text
        {
            get => scintillaHexBox.Text;
            set => scintillaHexBox.Text = value;
        }

        public int CaretPosition
        {
            set => scintillaHexBox.CurrentPosition = value;
        }

        public (int start, int len) GetSelection => (scintillaHexBox.SelectionStart, (scintillaHexBox.SelectionEnd - scintillaHexBox.SelectionStart) );

        public HexTypeWrapper PacketType
        {
            get { return _packetType; }
            private set
            {
                _packetType = value;
                packetTypeListBox.SelectedItem = value;
            }
        }

        public bool IsHexStream => _deserializer.IsHexStream(Text);

        // Ctor
        public PacketDefineControl() : this(null)
        {
        }

        public PacketDefineControl(PacketSaveData data)
        {
            _deserializer = new HexDeserializer();
            InitializeComponent();

            IEnumerable<HexStreamType> supportedTypes = PacketsDefinersDictionaries.SupportedTypes.ToArray();
            foreach (HexStreamType hexStreamType in Enum.GetValues(typeof(HexStreamType)))
            {
                // Check if Packet Define Control exists for this type
                bool isSupported = supportedTypes.Contains(hexStreamType);
                if (isSupported)
                {
                    // Supported  - Add to list box
                    HexTypeWrapper wrapper = new HexTypeWrapper(hexStreamType);
                    packetTypeListBox.Items.Add(wrapper);
                }
            }

            scintillaHexBox.Text = data?.Text ?? "";
            HexStreamType type = data?.Type ?? HexStreamType.Raw;
            HexTypeWrapper wrapped = new HexTypeWrapper(type);
            packetTypeListBox.SelectedItem = wrapped;
            PacketType = wrapped;

            IPacketDefiner definer = GetCurrentDefiner();

            if (data != null)
            {
                definer.LoadSaveData(data);
            }
        }

        // Get data for temp saver
        public TempPacketSaveData GetPacket()
        {
            byte[] bytes;
            string rawHex = scintillaHexBox.Text;
            try
            {
                bytes = _deserializer.Deserialize(rawHex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed deserializing Input:\r\n{ex.Message}");
            }


            IPacketDefiner definer = GetCurrentDefiner();

            byte[] data =  definer.Generate(bytes);
            LinkLayerType llt = definer.LinkLayer;
            TempPacketSaveData output = new TempPacketSaveData(data, llt);
            return output;
        }

        // Get data for real saver
        public PacketSaveData GetSaveData()
        {
            IPacketDefiner definer = GetCurrentDefiner();

            return definer.GetSaveData(scintillaHexBox.Text);
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
                int headersLengthToRemove = PacketsDefinersDictionaries.StreamTypeToFirstOffset[PacketType.Type];
                firstByteIndex -= headersLengthToRemove;

                if (firstByteIndex < 0 || bytesLength <= 0)
                {
                    scintillaHexBox.SelectionStart = 0;
                    scintillaHexBox.SelectionEnd = 0;
                    return;
                }
                scintillaHexBox.SelectionStart = firstByteIndex * 2 + (isSecondNibble ? 1 : 0);
                scintillaHexBox.SelectionEnd = scintillaHexBox.SelectionStart + (bytesLength * 2);
            }
            scintillaHexBox.Select();
        }

        public void NormalizeHex()
        {
            byte[] bytes;
            string rawHex = scintillaHexBox.Text;
            try
            {
                bytes = _deserializer.Deserialize(rawHex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed deserializing Input:\r\n{ex.Message}");
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append($"{b:x2}");
            }
            string finalHex = sb.ToString();
            this.scintillaHexBox.Text = finalHex;
        }

        public void FlattenProtoStack()
        {
            byte[] bytes;
            try
            {
                bytes = GetPacket().Data;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed deserializing Input:\r\n{ex.Message}");
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append($"{b:x2}");
            }
            string finalHex = sb.ToString();
            this.PacketType = HexStreamType.Raw;
            this.scintillaHexBox.Text = finalHex;
        }


        private void InvokeContentChanged() => ContentChanged?.Invoke(this, new EventArgs());

        private void ScintillaHexBox_TextChanged(object sender, EventArgs e)
        {
            InvokeContentChanged();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PacketType = (HexTypeWrapper)packetTypeListBox.SelectedItem;
            IPacketDefiner lastDefiner = GetCurrentDefiner();
            lastDefiner.PacketChanged -= PacketDefiner_PacketChanged;
            packetDefPanel.Controls.Clear();
            if (!PacketsDefinersDictionaries.StreamTypeToPacketDefineControlFactory.TryGetValue(PacketType.Type, out var controlCreatorFunc))
            {
                // Couldn't find a creation function
                throw new ArgumentException($"Can't find creation method for packets of type '{PacketType}'.\r\n" +
                    $"This could be the result of sloppy addition of new encapsulation\r\n" +
                    $"type without updating the {nameof(PacketDefineControl)} class.");
            }
            // Calling C'tor of the desired packet def control
            Control packetDefControl = controlCreatorFunc();

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
            if (!(packetDefPanel.Controls?[0] is IPacketDefiner definer))
            {
                throw new Exception("Can't find packet definer control???");
            }

            return definer;
        }

        private void PacketDefineControl_Load(object sender, EventArgs e)
        {
            // Adjusting 'line margin' on the scintilla text box so it shows
            scintillaHexBox.Margins[0].Width = 16;

            // Changing scintilla font
            scintillaHexBox.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            scintillaHexBox.Styles[ScintillaNET.Style.Default].Size = 12;

        }

    }
}
