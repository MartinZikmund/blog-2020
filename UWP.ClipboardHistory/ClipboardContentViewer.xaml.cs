using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace UWP.ClipboardHistory
{
    public sealed partial class ClipboardContentViewer : UserControl
    {
        public ClipboardContentViewer()
        {
            this.InitializeComponent();
        }

        public DataPackageView Data
        {
            get { return (DataPackageView)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(DataPackageView), typeof(ClipboardContentViewer), new PropertyMetadata(null, OnDataChanged));

        private static async void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (ClipboardContentViewer)d;
            viewer.ContentGrid.Children.Clear();
            if (e.NewValue is DataPackageView view)
            {
                if (view.Contains(StandardDataFormats.Bitmap))
                {
                    var bitmapReference = await view.GetBitmapAsync();
                    if (bitmapReference != null)
                    {
                        var bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(await bitmapReference.OpenReadAsync());
                        viewer.ContentGrid.Children.Add(new Image() { Width = 300, Height = 300, Stretch = Stretch.None, Source = bitmap });
                    }
                }
                else if (view.Contains(StandardDataFormats.Text))
                {
                    var text = await view.GetTextAsync();
                    if (text != null)
                    {
                        viewer.ContentGrid.Children.Add(new TextBlock() { Text = text });
                    }
                }
            }
        }
    }
}
