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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using VXPASerializer;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Devices.Haptics;
using Windows.UI.Notifications;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ManifestX
{
    public sealed partial class ManifestCreator : UserControl
    {
        StorageFolder _storageFolder;
        string folderPath = null;
        string guid = Guid.NewGuid().ToString();
        string publicName = "NULL";
        string manifestDir = string.Empty;
        IntPtr hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

        public ManifestCreator(string ManifestConnector)
        {
            this.InitializeComponent();
            manifestDir = ManifestConnector;
            IdTextBox.Text = guid;
        }




        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Pick();
            if (_storageFolder != null)
            {
                folderPath = _storageFolder.Path;
                FolderPathText.Text = folderPath;
                publicName = NameTextBox.Text;
            }
            
        }

        private async Task Pick()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Window window = (Application.Current as App)?.m_window!;
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            _storageFolder = folder;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (folderPath == null) 
            {
                Alert.Send("Morate izabrati",string.Empty);
                return;
            }
            InterXML InterXML = new InterXML();

            InterXML.CreateInterXml(NameTextBox.Text, guid, folderPath, $"/{NameTextBox.Text}_{guid}.vxpa");
            Alert.Send("Manifest uspešno napravljen", string.Empty);


        }
    }
}
