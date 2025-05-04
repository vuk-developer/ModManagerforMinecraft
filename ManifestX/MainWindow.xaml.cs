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
using Brigadier;
using Windows.UI.ApplicationSettings;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using WinRT.Interop;
using System.Threading.Tasks;
using System.Threading;
using VXPASerializer;
using VXPASerializer.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManifestX
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        
        StackPanel stackPanel = new StackPanel();
        
        ScrollViewer scrollViewer = new ScrollViewer();
        InterXML InterXML = new InterXML();
        Button button = new Button();
        
        Brigadier.Marshal marshal = new Brigadier.Marshal();
        string MainFile = string.Empty;
        string modsFolder = string.Empty;
        string LauncherConnector = string.Empty;
        string ManifestConnector = string.Empty;
        int i = 0;
        public MainWindow()
        {
            ConfigurationFile configurationFile = new ConfigurationFile();
            try
            {
                LauncherConnector = configurationFile.Configuration["VMX.Launcher"];
                modsFolder = configurationFile.Configuration["VMX.ModsDirectory"];
                ManifestConnector = configurationFile.Configuration["VMX.ManifestDepotDirectory"];
            }catch 
            {
                LauncherConnector = @"C:\XboxGames\Minecraft Launcher\Content\Minecraft.exe";
                modsFolder = Environment.GetEnvironmentVariable("appdata") + "/.minecraft/mods";
                ManifestConnector = Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/";

            }

            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            stackPanel.Padding = new Thickness(20);
            
            stackPanel.Spacing = 10;
            stackPanel.CanBeScrollAnchor = true;
            button.Content = "Omogući sve";
            button.Margin = new Thickness(7, 0, 0, 0);
            button.Click += Button_Click;
            this.InitializeComponent();
            MainPanel.Margin = new Thickness(30, 0, 0, 0);
            this.SetTitleBar(AppTitleBar);
            this.ExtendsContentIntoTitleBar = true;
            PopulateGUI();
            this.Closed += MainWindow_Closed;
            navView.Loaded += (s, e) =>
            {
                var settings = (NavigationViewItem)navView.SettingsItem;
                settings.Tag = "settings";
            };
        }


        private async void Commit_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".exe");
            nint windowHandle = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);
            try
            {
                StorageFile file = await fileOpenPicker.PickSingleFileAsync();
                if (file != null)
                {
                    MainFile = file.Path;
                    i = 1;
                }
                else
                {
                    return;
                }

            }
            catch
            {
                
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (button.Content == "Omogući sve") 
            {
                button.Content = "Onemogući sve";
            }
            else
            {
                button.Content = "Omogući sve";
            }
            foreach (Card mod in stackPanel.Children.Where((e)=>e is Card))
            {
                if (mod.IsChecked is false)
                {
                    mod.IsChecked = true;
                }
                else
                {
                    mod.IsChecked = false;
                }
            }
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            marshal.AllClear(modsFolder);
            if (Directory.Exists(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/temp"))
            {
                Directory.Delete(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/temp", true);
            }
        }

        private async Task Load()
        {
            if (i==1)
            {
                return;
            }
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".vxpa");
            nint windowHandle = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);
            try
            {
                StorageFile file = await fileOpenPicker.PickSingleFileAsync();
                if (file != null)
                {
                    MainFile = file.Path;
                    i = 1;
                }
                else
                {
                    return;
                }

            }
            catch 
            {
                
            }

        }
        private async void PopulateGUI()
        {
            navView.Header = "Kontrolni Panel";
            await Load();
            if (MainFile == string.Empty)
            {
                return;
            }
            stackPanel.Children.Add(button);
            VMXProfile vMXProfile = InterXML.GetModsFromXML(MainFile);
            foreach (var data in vMXProfile.Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VMXMod = data,
                    modsFolder = modsFolder,
                    mnfstxPath = MainFile
                };


                stackPanel.Children.Add(card);
            }
            HeaderID.Text = vMXProfile.Id;
            HeaderName.Text = vMXProfile.Name;
            scrollViewer.Content = stackPanel;
            MainPanel.Children.Add(scrollViewer);
        }

        private void InvokeLaunch()
        {
            Process.Start(@"C:\XboxGames\Minecraft Launcher\Content\Minecraft.exe");
        }
        private async void InvokeOpen()
        {
            navView.Header = "Kontrolni Panel";
            marshal.AllClear(modsFolder);

            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".vxpa");
            nint windowHandle = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);
            try
            {
                StorageFile file = await fileOpenPicker.PickSingleFileAsync();
                if (file != null)
                {
                    MainFile = file.Path;
                }
            }
            catch
            {

            }

            MainPanel.Children.Clear();
            stackPanel.Children.Clear();
            stackPanel.Children.Add(button);
            VMXProfile vMXProfile = InterXML.GetModsFromXML(MainFile);
            foreach (var data in vMXProfile.Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VMXMod = data,
                    mnfstxPath = MainFile,
                    modsFolder = modsFolder
                };

                stackPanel.Children.Add(card);
            }
            HeaderID.Text = vMXProfile.Id;
            HeaderName.Text = vMXProfile.Name;
            scrollViewer.Content = stackPanel;
            MainPanel.Children.Add(scrollViewer);

        }
        private void InvokeNewManifest()
        {
            navView.Header = "Napravi Novi Manifest";
            MainPanel.Children.Clear();

            MainPanel.Children.Add(new ManifestCreator(ManifestConnector));
            

        }
        private void InvokeControl()
        {
            navView.Header = "Kontrolni Panel";
            if (MainFile == string.Empty) 
            { 
                return;
            }
           
            MainPanel.Children.Clear();
            stackPanel.Children.Clear();
            stackPanel.Children.Add(button);
            VMXProfile vMXProfile = InterXML.GetModsFromXML(MainFile);

            foreach (var data in vMXProfile.Mods)
            {
                var card = new Card
                {
                    Title = data._name,
                    Subtitle = data._framework,
                    Description = data._description,
                    IsChecked = (bool)data._enabled,
                    PreviewImage = new BitmapImage(new Uri(Environment.GetEnvironmentVariable("appdata") + "/.manifestsv/Assets/img.png")),
                    VMXMod = data,
                    mnfstxPath = MainFile,
                    modsFolder = modsFolder
                };

                stackPanel.Children.Add(card);
            }
            HeaderID.Text = vMXProfile.Id;
            HeaderName.Text = vMXProfile.Name;
            scrollViewer.Content = stackPanel;
            MainPanel.Children.Add(scrollViewer);
        }
        private void InvokeDepot()
        {
            navView.Header = "Manifest Depozitorijum";
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new ManifestList());
        }
        private void InvokeSettings()
        {
            navView.Header = "Postavke";
            MainPanel.Children.Clear();
            MainPanel.Children.Add(new ManifestSettings());
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedItem = args.InvokedItemContainer as NavigationViewItem;
            var tag = invokedItem?.Tag?.ToString();

            switch (tag)
            {
                case "mpanel":
                    InvokeControl();
                    break;
                case "open":
                    InvokeOpen();
                    break;
                case "new":
                    InvokeNewManifest();
                    break;
                case "launch":
                    InvokeLaunch();
                    break;
                case "depot":
                    InvokeDepot();
                    break;
                case "settings":
                    InvokeSettings();
                    break;
            }
        }
    }
}
