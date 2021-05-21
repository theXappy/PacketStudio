using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.Windows
{
    /// <summary>
    /// Interaction logic for EncodeTextWindow.xaml
    /// </summary>
    public partial class RelocatePacketWindow : RibbonWindow
    {
        public RelocatePacketWindow()
        {
            InitializeComponent();
        }

        private void InsertButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void RibbonWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            int x = 3;
        }
    }
}
