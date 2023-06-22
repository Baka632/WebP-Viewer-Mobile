using System.Collections.Generic;
using WebPViewerMobile.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace WebPViewerMobile.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImageViewPage : Page
    {
        public ImageViewPageViewModel ViewModel { get; }

        public ImageViewPage()
        {
            ViewModel = new ImageViewPageViewModel();
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IReadOnlyList<IStorageItem> items)
            {
                await ViewModel.PrepareFiles(items);
            }
            else if (e.Parameter is IStorageItem item)
            {
                await ViewModel.PrepareFile(item);
            }
        }
    }
}
