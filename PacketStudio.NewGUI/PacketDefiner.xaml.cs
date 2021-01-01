using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PacketStudio.Core;
using PacketStudio.DataAccess;
using PacketStudio.DataAccess.SaveData;
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

        public void OnPacketChanged(object sender, EventArgs args)
        {
            PacketChanged?.Invoke(sender,args);
        }

        private readonly HexDeserializer _deserializer = new HexDeserializer();

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

        #region Dependency Props Stuff

        public static readonly DependencyProperty SessionPacketProperty = DependencyProperty.Register(nameof(SessionPacket), typeof(PacketSaveDataNG), typeof(PacketDefiner), new FrameworkPropertyMetadata(SessionPacketPropertyChangedCallback));
        public static readonly DependencyProperty ExportPacketProperty = DependencyProperty.Register(nameof(ExportPacket), typeof(TempPacketSaveData), typeof(PacketDefiner), new FrameworkPropertyMetadata(PacketPropertyChangedCallback));
        private static void SessionPacketPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PacketDefiner).SessionPacketPropertyChangedCallback(e);
        }
        private void SessionPacketPropertyChangedCallback(DependencyPropertyChangedEventArgs e)
        {
            var newPacket = e.NewValue as PacketSaveDataNG;
            if (newPacket != null)
            {
                // Getting the right listbox item of the "packet types list"
                var listItem = _streamTypeToListItems[newPacket.Type];

                // Switching to the new packet type to bring up the new packet template control
                templatesListBox.SelectedItem = listItem;
                hexTextBox.Text = newPacket.PacketData;

                this.PacketTemplateControl.LoadSaveDetails(newPacket.Details);
            }

        }
        private static void PacketPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debugger.Break();
            // ?
        }

        #endregion

        public PacketSaveDataNG SessionPacket
        {
            get
            {
                IPacketTemplateControl templateControl = PacketTemplateControl;
                if (templateControl == null)
                    return null;


                var details = templateControl.GenerateSaveDetails();

                string typeName = ((ListBoxItem) templatesListBox.SelectedItem).Content.ToString();
                HexStreamType type = _namesToHexType[typeName];

                string text = hexTextBox.Text;

                PacketSaveDataNG psd = new PacketSaveDataNG(type, text) {Details = details};
                return psd;
            }
            set
            {
                Console.WriteLine($"@@@ SessionPacket setter called with value : {value}");
                SetValue(SessionPacketProperty, value);
                this.PacketTemplateControl.LoadSaveDetails(SessionPacket.Details);
            }
        }

        public TempPacketSaveData ExportPacket
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
            set
            {
                SetValue(ExportPacketProperty, value);
            }
        }
        
        public bool IsHexStream => _deserializer.IsHexStream(this.hexTextBox.Text);

        public PacketDefiner()
        {
            InitializeComponent();
            // Remove placeholder text
            hexTextBox.Text = string.Empty;
            // Remove place holder item from packet types list box
            templatesListBox.Items.Clear();

            var assm = this.GetType().Assembly;
            var packetTemplateTypes = assm.GetTypes().Where(type => typeof(IPacketTemplateControl).IsAssignableFrom(type) && !type.IsInterface);
            var sortedTemplateTypes = packetTemplateTypes.ToList();
            sortedTemplateTypes.Sort((type1, type2) =>
            {
                var ord1 = type1.GetCustomAttributes(typeof(OrderAttribute)).Single() as OrderAttribute;
                var ord2 = type2.GetCustomAttributes(typeof(OrderAttribute)).Single() as OrderAttribute;
                return ord1.Order.CompareTo(ord2.Order);
            });

            foreach (var packetTemplateType in sortedTemplateTypes)
            {
                DisplayNameAttribute attr = packetTemplateType.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                var name = attr.DisplayName;

                HexStreamTypeAttribute typeAttr = packetTemplateType.GetCustomAttributes(typeof(HexStreamTypeAttribute), false).FirstOrDefault() as HexStreamTypeAttribute;
                HexStreamType sType = typeAttr.StreamType;

                ListBoxItem lbi = new ListBoxItem()
                {
                    Content = name
                };

                _templatesBoxItemsToControlsCreators[lbi] = () => (UserControl) Activator.CreateInstance(packetTemplateType);

                templatesListBox.Items.Add(lbi);


                _streamTypeToListItems[sType] = lbi;
                _namesToHexType[name] = sType;
            }

            templatesListBox.SelectedIndex = 0;
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnPacketChanged(this, new EventArgs());
        }

        private void HexTextBox_LostFocus(object sender, RoutedEventArgs e)
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

        private  readonly Dictionary<HexStreamType,ListBoxItem> _streamTypeToListItems = new Dictionary<HexStreamType, ListBoxItem>();
        private readonly Dictionary<ListBoxItem, Func<UserControl>> _templatesBoxItemsToControlsCreators = new Dictionary<ListBoxItem, Func<UserControl>>();
        private readonly Dictionary<string, HexStreamType> _namesToHexType = new Dictionary<string, HexStreamType>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // We might have loaded a saved packet into the control before it 
            // finished creating so previous tries to notify of the new packet were swallowd (since no one listened to the event)
            // Invoking the event again here makes sure the packet is notices
            OnPacketChanged(this, new EventArgs());
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

            this.OnPacketChanged(this,e);
        }

        private void PacketTemplateControlChanged(object sender, EventArgs e) => this.OnPacketChanged(this,e);

    }
}
