using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PacketStudio.NewGUI.WpfJokes;
using Syncfusion.Windows.Tools.Controls;

namespace PacketStudio.NewGUI
{
    /// <summary>
    /// Interaction logic for TabControlWrapper.xaml
    /// </summary>
    public partial class TabControlWrapper : TabControlExt
    {
        public TabControlWrapper()
        {
            SetResourceReference(StyleProperty, typeof(TabControlExt));

            InitializeComponent();
        }

        private Button outerButton = null;
        private Button newTabButton = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (newTabButton == null)
            {
                var step1 = WpfScavengerHunt.FindChild<TabPanelAdv>(this, null);
                var step2 = WpfScavengerHunt.FindChild<TabScrollViewer>(step1, null);
                var step3 = WpfScavengerHunt.FindChild<TabLayoutPanel>(step2, null);
                outerButton = WpfScavengerHunt.GetLogicalChildCollection<Button>(step3).SingleOrDefault();
                newTabButton = WpfScavengerHunt.FindChild<Button>(outerButton, null);
            }

            // Still did not fid the button...
            if (newTabButton == null) return;

            newTabButton.Background = new SolidColorBrush(Colors.Aquamarine);
            outerButton.Margin = new Thickness(5 + 1*(this.Items.Count),1,1,1);
        }
    }
}
