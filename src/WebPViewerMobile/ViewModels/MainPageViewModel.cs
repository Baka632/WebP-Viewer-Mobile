using WebPViewerMobile.Helpers;
using WebPViewerMobile.Views;
using Windows.UI.Xaml.Controls;

namespace WebPViewerMobile.ViewModels
{
    public class MainPageViewModel : NotificationObject
    {
        public TitleBarHelper TitleBarHelper { get; }

        public MainPageViewModel(Frame frame)
        {
            TitleBarHelper = new TitleBarHelper(frame);
            NavigationHelper.CurrentFrame = frame;
            NavigationHelper.Navigate(typeof(MainFrame), null);
        }
    }
}
