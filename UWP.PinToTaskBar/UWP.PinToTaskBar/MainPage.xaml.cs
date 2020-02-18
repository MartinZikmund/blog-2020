using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Popups;
using Windows.UI.Shell;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP.PinToTaskBar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void PinToTaskBar_Click(object sender, RoutedEventArgs e)
        {
            if (!ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
            {
                await ShowMessageAsync("API requires Windows 10 version 16299 or newer.");
                return;
            }

            var taskbarManager = TaskbarManager.GetDefault();
            
            if (!taskbarManager.IsSupported)
            {
                await ShowMessageAsync("Taskbar is not supported on this version of Windows 10.");
                return;
            }

            var isPinned = await taskbarManager.IsCurrentAppPinnedAsync();

            if (isPinned)
            {
                await ShowMessageAsync("Application is already pinned.");
                return;
            }

            if (!taskbarManager.IsPinningAllowed)
            {
                await ShowMessageAsync("Pinning to task bar is not allowed on this PC.");
                return;
            }


            var pinned = await taskbarManager.RequestPinCurrentAppAsync();
            if (pinned)
            {
                await ShowMessageAsync("App is successfully pinned!");
            }
            else
            {
                await ShowMessageAsync("App was not pinned!");
            }
        }

        private async Task ShowMessageAsync(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
