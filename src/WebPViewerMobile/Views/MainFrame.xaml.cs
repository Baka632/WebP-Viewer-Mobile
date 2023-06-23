using System;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using WebPViewerMobile.Helpers;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace WebPViewerMobile.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainFrame : Page
    {
        public MainFrame()
        {
            this.InitializeComponent();
            AcrylicHelper.TrySetAcrylicBrush(this);
        }

        private async void OpenFile(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".webp");
            IReadOnlyList<StorageFile> files = await picker.PickMultipleFilesAsync();
            if (files != null)
            {
                Frame.Navigate(typeof(ImageViewPage), files);
            }
        }
    }
}
