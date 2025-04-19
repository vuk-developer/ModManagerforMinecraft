using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Devices;
using VukXML;
using Marshalate;
using Windows.UI.ApplicationSettings;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using WinRT.Interop;
using System.Threading.Tasks;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VukManifestX
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        
        StackPanel stackPanel = new StackPanel();
        VukXML.VukXML vukXML = new VukXML.VukXML();
        string MainFile = string.Empty;
        string modsFolder = Environment.GetEnvironmentVariable("appdata") + "/.minecraft/mods";
        int i = 0;
        Marshalate.Marshal marshal = new Marshalate.Marshal();
        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
        public MainWindow()
        {
            
            this.InitializeComponent();
            this.SetTitleBar(AppTitleBar);
            this.ExtendsContentIntoTitleBar = true;
            PopulateGUI();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            marshal.AllClear(modsFolder);
            Directory.Delete(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/temp",true);
        }

        private async Task Load()
        {
            if (i==1)
            {
                return;
            }
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".xvuk");
            nint windowHandle = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            MainFile = file.Path;
            i = 1;

        }
        private async void PopulateGUI()
        {
            await Load();
            foreach (var data in vukXML.GetModsFromXML(MainFile).Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VukJavaMod = data,
                    modsFolder = modsFolder,
                    mnfstxPath = MainFile
                };


                CardListPanel.Children.Add(card);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Btn2.IsEnabled = false;
            Btn1.IsEnabled = true;
            MainPanel.Children.Clear();

            MainPanel.Children.Add(new ManifestCreator());

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Btn1.IsEnabled = false;
            Btn2.IsEnabled = true;

            stackPanel.Children.Clear();
            MainPanel.Children.Clear();
            stackPanel.Padding = new Thickness(20);
            stackPanel.Spacing = 10;
            foreach (var data in vukXML.GetModsFromXML(MainFile).Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VukJavaMod = data,
                    modsFolder = modsFolder,
                    mnfstxPath = MainFile
                };

                stackPanel.Children.Add(card);
            }
            MainPanel.Children.Add(stackPanel);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start(@"C:\XboxGames\Minecraft Launcher\Content\Minecraft.exe");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Btn1.IsEnabled = false;
            Btn2.IsEnabled = true;
            Load();
            MainPanel.Children.Clear();
            stackPanel.Children.Clear();
            stackPanel.Padding = new Thickness(20);
            stackPanel.Spacing = 10;
            foreach (var data in vukXML.GetModsFromXML(MainFile).Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VukJavaMod = data,
                    mnfstxPath = MainFile,
                    modsFolder = modsFolder
                };

                stackPanel.Children.Add(card);
            }
            MainPanel.Children.Add(stackPanel);
        }


    }
}
