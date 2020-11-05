using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Metro.WPF;
using Syncfusion.Windows.Tools.Controls;

namespace ByteArrayToPcap.NewGUI
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
                var step1 = WpfHacks.FindChild<TabPanelAdv>(this, null);
                var step2 = WpfHacks.FindChild<TabScrollViewer>(step1, null);
                var step3 = WpfHacks.FindChild<TabLayoutPanel>(step2, null);
                outerButton = WpfHacks.GetLogicalChildCollection<Button>(step3).SingleOrDefault();
                newTabButton = WpfHacks.FindChild<Button>(outerButton, null);
            }

            // Still did not fid the button...
            if (newTabButton == null) return;

            newTabButton.Background = new SolidColorBrush(Colors.Aquamarine);
            outerButton.Margin = new Thickness(5 + 1*(this.Items.Count),1,1,1);
        }
    }

    public static class WpfHacks
    {


        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static List<T> GetLogicalChildCollection<T>(this DependencyObject parent) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            List<T> output = new List<T>();

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType != null)
                {
                    // child element found.
                    output.Add(childType);
                }
            }

            return output;
        }
    }
}
