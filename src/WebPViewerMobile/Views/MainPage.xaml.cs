using WebPViewerMobile.Helpers;
using WebPViewerMobile.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace WebPViewerMobile.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel { get; }

        private bool IsTitleBarTextBlockForwardBegun = false;
        private bool IsFirstRun = true;

        public MainPage()
        {
            this.InitializeComponent();

            AcrylicHelper.TrySetAcrylicBrush(this);
            ViewModel = new MainPageViewModel(ContentFrame);
            TitleBarHelper.BackButtonVisibilityChangedEvent += OnBackButtonVisibilityChanged;
            TitleBarHelper.TitleBarVisibilityChangedEvent += OnTitleBarVisibilityChanged;
        }

        private void OnTitleBarVisibilityChanged(CoreApplicationViewTitleBar bar)
        {
            if (bar.IsVisible)
            {
                StartTitleBarAnimation(Visibility.Visible);
            }
            else
            {
                StartTitleBarAnimation(Visibility.Collapsed);
            }
        }

        private void OnBackButtonVisibilityChanged(BackButtonVisibilityChangedEventArgs args)
        {
            StartTitleTextBlockAnimation(args.BackButtonVisibility);
        }

        private void StartTitleTextBlockAnimation(AppViewBackButtonVisibility buttonVisibility)
        {
            switch (buttonVisibility)
            {
                case AppViewBackButtonVisibility.Disabled:
                case AppViewBackButtonVisibility.Visible:
                    if (IsTitleBarTextBlockForwardBegun)
                    {
                        goto default;
                    }
                    TitleBarTextBlockForward.Begin();
                    IsTitleBarTextBlockForwardBegun = true;
                    break;
                case AppViewBackButtonVisibility.Collapsed:
                    TitleBarTextBlockBack.Begin();
                    IsTitleBarTextBlockForwardBegun = false;
                    break;
                default:
                    break;
            }
        }

        private void StartTitleBarAnimation(Visibility visibility)
        {
            if (IsFirstRun)
            {
                IsFirstRun = false;
                TitleBar.Visibility = visibility;
                return;
            }

            switch (visibility)
            {
                case Visibility.Visible:
                    TitleBarShow.Begin();
                    break;
                case Visibility.Collapsed:
                default:
                    break;
            }
            TitleBar.Visibility = visibility;
        }
    }
}
