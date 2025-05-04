using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManifestX
{
    public sealed partial class ManifestSettings : UserControl
    {
        ConfigurationFile configurationFile = new ConfigurationFile();

        public ManifestSettings()
        {
            this.InitializeComponent();
            this.Loaded += ManifestSettings_Loaded;

        }
        public string LauncherConnect
        {
            get => (string)GetValue(LauncherConnectProperty);
            set => SetValue(LauncherConnectProperty, value);
        }
        public static readonly DependencyProperty LauncherConnectProperty =
            DependencyProperty.Register(nameof(LauncherConnect), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));
        public string ModsConnect
        {
            get => (string)GetValue(ModsConnectProperty);
            set => SetValue(ModsConnectProperty, value);
        }
        public static readonly DependencyProperty ModsConnectProperty =
            DependencyProperty.Register(nameof(ModsConnect), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));
        public string ManifestConnect
        {
            get => (string)GetValue(ManifestConnectProperty);
            set => SetValue(ManifestConnectProperty, value);
        }
        public static readonly DependencyProperty ManifestConnectProperty =
            DependencyProperty.Register(nameof(ManifestConnectProperty), typeof(string), typeof(Card), new PropertyMetadata(string.Empty));

        private async void ManifestSettings_Loaded(object sender, RoutedEventArgs e)
        {
            if (configurationFile.Configuration.Count > 0)
            {
                LauncherConnect = configurationFile.Configuration["VMX.Launcher"];
                ModsConnect = configurationFile.Configuration["VMX.ModsDirectory"];
                ManifestConnect = configurationFile.Configuration["VMX.ManifestDepotDirectory"];
                configFileText.Text = configurationFile.XToString();
            }
            else
            {
                LauncherConnect = "Launcher nije izabran";
                ModsConnect = "Mods Direktorijum nije izabran";
                ManifestConnect = "Manifest Depozitorijum Direktrorijum nije izabran";
            }
        }

        private async Task Pick(int pick)
        {
            IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            if (pick == 0)
            {
                var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
                filePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                filePicker.FileTypeFilter.Add(".exe");

                WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
                StorageFile file = await filePicker.PickSingleFileAsync();
                if (file != null)
                {
                    LauncherConnect = file.Path;

                    configurationFile.Configuration.AddOrUpdate("VMX.Launcher", LauncherConnect);
                }
            }
            else if (pick == 1)
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();

                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

                StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

                if (storageFolder != null)
                {
                    ModsConnect = storageFolder.Path;

                    configurationFile.Configuration.AddOrUpdate("VMX.ModsDirectory", ModsConnect);

                }

            }
            else if (pick == 2)
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();

                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

                StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

                if (storageFolder != null)
                {
                    ManifestConnect = storageFolder.Path;

                    configurationFile.Configuration.AddOrUpdate("VMX.ManifestDepotDirectory", ManifestConnect);

                }

            }
            configurationFile.Save();
            configFileText.Text = configurationFile.XToString();
            

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Pick(0);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Pick(1);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Pick(2);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ConfigurationFile.Clear();
            Alert.Send("Resetovali ste konfiguraciju", "Restartujte Marshal Manager");
        }
    }
}
