using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManifestX
{
    public sealed partial class ManifestList : UserControl
    {
        public ManifestList()
        {
            this.InitializeComponent();
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpandableContent.Visibility == Visibility.Collapsed)
            {
                ExpandableContent.Visibility = Visibility.Visible;
                ExpandButton.Content = "˄"; // Up arrow
            }
            else
            {
                ExpandableContent.Visibility = Visibility.Collapsed;
                ExpandButton.Content = "˅"; // Down arrow
            }
        }
    }
}
