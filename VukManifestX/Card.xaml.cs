using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Media.Devices;
using VukXML;
using Marshalate;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VukManifestX
{
    public sealed partial class Card : UserControl
    {
        public Card()
        {
            this.InitializeComponent();
        }
        public VukJavaMod VukJavaMod = new VukJavaMod();
        public string modsFolder = string.Empty;
        public string mnfstxPath = string.Empty;
        public BitmapImage PreviewImage
        {
            get => (BitmapImage)GetValue(PreviewImageProperty);
            set => SetValue(PreviewImageProperty, value);
        }
        public static readonly DependencyProperty PreviewImageProperty =
            DependencyProperty.Register(nameof(PreviewImage), typeof(BitmapImage), typeof(Card), new PropertyMetadata(null));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }
        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
            
            
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(Card), new PropertyMetadata(false));

        public VukJavaMod GetModActive()
        {
            return this.VukJavaMod;
            
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            VukJavaMod._enabled = IsChecked;
            
            Marshal marshal = new Marshal();
            if (IsChecked == true) 
            {
                marshal.As(modsFolder, VukJavaMod, mnfstxPath);
            }
            else
            {
                marshal.TakeOut(modsFolder, VukJavaMod._filename);
            }
            
        }
    }
}
