using System;
using System.Windows.Forms;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;

namespace PacketStudio.Controls.PacketsDef
{
	public partial class RawPacketDefControl : UserControl, IPacketDefiner
	{
	    public LinkLayerType _linkLayer = LinkLayerType.Ethernet;

	    public event EventHandler PacketChanged;

        public HexStreamType StreamType => HexStreamType.Raw;
	    public LinkLayerType LinkLayer => _linkLayer;
        public int HeadersLength => 0;
	    public bool IsValid => true;
		public string Error => String.Empty;

	    public RawPacketDefControl()
	    {
	        InitializeComponent();
	        linkLayerTextBox_TextChanged(this, null);
	    }

        public byte[] Generate(byte[] rawBytes)
		{
			return rawBytes;
		}

		public PacketSaveData GetSaveData(string packetHex)
		{
            return PacketSaveDataFactory.GetPacketSaveData(packetHex,HexStreamType.Raw,linkLayerTextBox.Text,"1","1","");
		}

		public void LoadSaveData(PacketSaveData data)
		{
		    linkLayerTextBox.Text = data.LinkLayerType;
		}

        private void linkLayerTextBox_TextChanged(object sender, EventArgs e)
        {
            PacketChanged?.Invoke(this, new EventArgs());

            // Trying to resolve the PPID so the use know what protocol wireshark is going to try
            try
            {
                _linkLayer = ParseLinkLayer(linkLayerTextBox.Text);
                if (!Enum.IsDefined(typeof(LinkLayerType), _linkLayer))
                {
                    resProtoValueLabel.Text = "Unassigned";
                }
                else
                {
                    resProtoValueLabel.Text = _linkLayer.ToString();
                }
            }
            catch
            {
                resProtoValueLabel.Text = "ERROR";
            }
        }

	    /// <param name="txt">Either a raw decimal value or hex value prepended with '0x' </param>
	    private LinkLayerType ParseLinkLayer(string txt)
	    {
	        LinkLayerType proto;
	        try
	        {
	            if (txt.StartsWith("0x")) // Hex
	            {
	                proto = (LinkLayerType)Convert.ToInt64(txt.Substring(2), 16);
	            }
	            else // Dec
	            {
	                proto = (LinkLayerType)(int.Parse(txt));
	            }
	        }
	        catch (Exception ex)
	        {
	            throw new Exception($"Failed to parse Link Layer ID. Must be an integer OR hexadecimal number starting with '0x', was: '{txt}'. Inner error: {ex.Message}");
	        }

	        return proto;
	    }
    }
}
