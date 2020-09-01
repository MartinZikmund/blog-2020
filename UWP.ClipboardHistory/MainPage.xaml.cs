using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UWP.ClipboardHistory
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            Clipboard.HistoryChanged += Clipboard_HistoryChanged;
            Clipboard.HistoryEnabledChanged += Clipboard_HistoryEnabledChanged;            
        }

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Artificial delay, as in non-debug Clipboard.GetHistoryItemsAsync returns 
            // until the app is fully loaded.
            await Task.Delay(100); 
            await ReloadHistoryAsync();
        }

        public ObservableCollection<ClipboardHistoryItem> ClipboardHistory { get; } = new ObservableCollection<ClipboardHistoryItem>();

        private async void Clipboard_HistoryChanged(object sender, ClipboardHistoryChangedEventArgs e)
        {
            await ReloadHistoryAsync();
        }

        private async Task ReloadHistoryAsync()
        {
            var historyItems = await Clipboard.GetHistoryItemsAsync();
            if (historyItems.Status == ClipboardHistoryItemsResultStatus.Success)
            {
                ClipboardHistory.Clear();
                foreach (var item in historyItems.Items)
                {
                    ClipboardHistory.Add(item);
                }
            }
        }

        private void Clipboard_HistoryEnabledChanged(object sender, object e)
        {
            var isEnabled = Clipboard.IsHistoryEnabled();
            ClipboardHistoryDisabledMessage.Visibility = isEnabled ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
