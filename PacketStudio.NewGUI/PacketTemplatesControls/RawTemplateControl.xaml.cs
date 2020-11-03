using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PacketStudio.DataAccess;

namespace ByteArrayToPcap.NewGUI.PacketTemplatesControls
{
    /// <summary>
    /// Interaction logic for UdpTemplateControl.xaml
    /// </summary>
    [DisplayName("Raw Frame")]
    public partial class RawTemplateControl : UserControl, IPacketTemplateControl
    {
        private static Dictionary<string, LinkLayerType> _singletonMap = null;
        private static Dictionary<string, LinkLayerType> _map
        {
            get
            {
                if (_singletonMap == null)
                {
                    _singletonMap = new Dictionary<string, LinkLayerType>();
                    for (int i = 0; i < ushort.MaxValue; i++)
                    {
                        if (Enum.IsDefined(typeof(LinkLayerType), i))
                        {
                            LinkLayerType type = (LinkLayerType) i;
                            string name = type.ToString();
                            _singletonMap[name] = type;
                        }
                    }
                }
                return _singletonMap;
            }
        }

        public RawTemplateControl()
        {
            InitializeComponent();

            foreach (KeyValuePair<string, LinkLayerType> nameAndType in _map)
            {
                LinkLayerType type = nameAndType.Value;
                string name = nameAndType.Key;
                string label = $"{(int) type} {name}";
                linkLayersBox.Items.Add(label);
            }

            linkLayersBox.SelectedIndex = 0;
        }



        public event EventHandler Changed;

        public bool IsValid => true;

        public (bool success, TempPacketSaveData packet, string error) GeneratePacket(byte[] rawHex)
        {
            string name = linkLayersBox.SelectionBoxItem as string;
            name = name.Substring(name.IndexOf(" ", StringComparison.Ordinal)).Trim();
            if (_map.TryGetValue(name, out LinkLayerType type))
                return (true, new TempPacketSaveData(rawHex, type), null);
            return (false, null, "Couldn't result Link Layer type");
        }

        private void StreamTextbox_OnTextChanged(object sender, TextChangedEventArgs e) => Changed?.Invoke(this, e);

        private void LinkLayersBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) =>
            this.Changed?.Invoke(this, e);
    }
}
