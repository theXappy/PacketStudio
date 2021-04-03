using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.Windows
{
    /// <summary>
    /// Interaction logic for InsertAsciiWindow.xaml
    /// </summary>
    public partial class InsertAsciiWindow : RibbonWindow
    {
        public InsertAsciiWindow()
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
