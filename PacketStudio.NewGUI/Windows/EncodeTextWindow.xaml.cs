using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.Windows
{
    /// <summary>
    /// Interaction logic for EncodeTextWindow.xaml
    /// </summary>
    public partial class EncodeTextWindow : RibbonWindow
    {
        public EncodeTextWindow()
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
    }
}
