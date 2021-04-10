using System.IO;
using System.Windows.Forms;
using PacketStudio.Core;
using PacketStudio.NewGUI.ViewModels;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI.Windows
{
    /// <summary>
    /// Interaction logic for WiresharkFinderWindow.xaml
    /// </summary>
    public partial class WiresharkFinderWindow : RibbonWindow
    {
        public WiresharkFinderWindow()
        {
            InitializeComponent();
        }

        private void ChooseCustomDirectoryButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Wireshark.exe|Wireshark.exe"
            };
            DialogResult res = ofd.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                if (ofd.CheckFileExists)
                {
                    string dirPath = Path.GetDirectoryName(ofd.FileName);
                    if (SharksFinder.TryGetByPath(dirPath, out WiresharkDirectory wd))
                    {
                        WiresharkFinderViewModel wfvm = this.DataContext as WiresharkFinderViewModel;
                        wfvm.AddCustomDirectory(wd);
                    }
                }
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // This button is only clickable if a directory was chosen (WPF assertions)
            // So everything's fine.
            this.DialogResult = true;
            this.Close();
        }
    }
}
