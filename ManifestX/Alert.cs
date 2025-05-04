using CommunityToolkit.WinUI.Notifications;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Diagnostics;

namespace ManifestX
{
    public class Alert
    {
        public static void Send(string message, string code)
        {
            new ToastContentBuilder()
                .AddText(message)
                .AddAttributionText(code)
                .Show();
        }
    }
}
