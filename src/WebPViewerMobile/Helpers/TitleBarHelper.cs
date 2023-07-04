using System;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebPViewerMobile.Helpers
{
    public class TitleBarHelper
    {
        CoreApplicationViewTitleBar CoreTitleBar { get; } = CoreApplication.GetCurrentView().TitleBar;
        public ApplicationViewTitleBar PresentationTitleBar { get; } = ApplicationView.GetForCurrentView().TitleBar;
        public SystemNavigationManager NavigationManager { get; } = SystemNavigationManager.GetForCurrentView();
        public Frame CurrentFrame { get; }
        public static event Action<BackButtonVisibilityChangedEventArgs> BackButtonVisibilityChangedEvent;
        public static event Action<CoreApplicationViewTitleBar> TitleBarVisibilityChangedEvent;

        public TitleBarHelper(Frame frame)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                CoreTitleBar.ExtendViewIntoTitleBar = true;
                CoreTitleBar.IsVisibleChanged += OnTitleBarVisibilityChanged;

                #region TitleBarColor
                PresentationTitleBar.ButtonBackgroundColor = Colors.Transparent;
                Color ForegroundColor;
                switch (Application.Current.RequestedTheme)
                {
                    case ApplicationTheme.Light:
                        ForegroundColor = Colors.Black;
                        break;
                    case ApplicationTheme.Dark:
                        ForegroundColor = Colors.White;
                        break;
                    default:
                        ForegroundColor = Colors.White;
                        break;
                }
                PresentationTitleBar.ButtonForegroundColor = ForegroundColor;
                #endregion

                CurrentFrame = frame;
                CurrentFrame.Navigated += CurrentFrame_Navigated;
            }
        }

        private void OnTitleBarVisibilityChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBarVisibilityChangedEvent?.Invoke(sender);
        }

        private void CurrentFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            NavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibilityToBoolean(CurrentFrame.CanGoBack);
            BackButtonVisibilityChangedEvent?.Invoke(new BackButtonVisibilityChangedEventArgs(NavigationManager.AppViewBackButtonVisibility));

            AppViewBackButtonVisibility AppViewBackButtonVisibilityToBoolean(bool canGoBack)
            {
                if (canGoBack)
                {
                    return AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    return AppViewBackButtonVisibility.Collapsed;
                }
            }
        }
    }

    public class BackButtonVisibilityChangedEventArgs : EventArgs
    {
        public AppViewBackButtonVisibility BackButtonVisibility { get; }

        public BackButtonVisibilityChangedEventArgs(AppViewBackButtonVisibility visibility)
        {
            BackButtonVisibility = visibility;
        }
    }
}
