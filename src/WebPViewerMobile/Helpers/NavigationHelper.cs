using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace WebPViewerMobile.Helpers
{
    internal static class NavigationHelper
    {
        public static Frame CurrentFrame { get; set; }
        public static SystemNavigationManager NavigationManager { get; } = SystemNavigationManager.GetForCurrentView();

        static NavigationHelper()
        {
            NavigationManager.BackRequested += BackRequested;
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += OnAcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += OnPointerPressed;
        }

        private static void BackRequested(object sender, BackRequestedEventArgs e)
        {
            OnBackRequested(e);
        }

        public static void Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo transitionInfo = null)
        {
            CurrentFrame.Navigate(sourcePageType, parameter, transitionInfo);
        }

        /// <summary>
        /// 当按下快捷键时调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private static void OnAcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            //支持 Alt + 左/右方向键 导航
            if (args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && (args.VirtualKey == VirtualKey.Left || args.VirtualKey == VirtualKey.Right)
                && args.KeyStatus.IsMenuKeyDown == true
                && !args.Handled)
            {
                if (args.VirtualKey == VirtualKey.Left)
                {
                    args.Handled = TryGoBack();
                }
                else if (args.VirtualKey == VirtualKey.Right)
                {
                    args.Handled = TryGoForward();
                }
            }
        }

        /// <summary>
        /// 当鼠标按键时调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private static void OnPointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            //支持扩展鼠标按键导航
            if (args.CurrentPoint.Properties.IsXButton1Pressed)
            {
                args.Handled = !TryGoBack();
            }
            else if (args.CurrentPoint.Properties.IsXButton2Pressed)
            {
                args.Handled = !TryGoForward();
            }
        }

        /// <summary>
        /// 当系统请求进行向后导航时调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private static void OnBackRequested(BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = TryGoBack();
            }
        }

        /// <summary>
        /// 尝试进行向后导航
        /// </summary>
        /// <returns>指示导航是否成功的值</returns>
        public static bool TryGoBack()
        {
            if (CurrentFrame.CanGoBack)
            {
                CurrentFrame.GoBack();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 尝试进行向前导航
        /// </summary>
        /// <returns>指示导航是否成功的值</returns>
        private static bool TryGoForward()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (CurrentFrame.CanGoForward)
            {
                CurrentFrame.GoForward();
                return true;
            }
            return false;
        }
    }
}
