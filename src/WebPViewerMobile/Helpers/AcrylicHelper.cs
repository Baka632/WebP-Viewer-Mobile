using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WebPViewerMobile.Helpers
{
    /// <summary>
    /// 帮助设置亚克力背景的类
    /// </summary>
    public static class AcrylicHelper
    {
        /// <summary>
        /// 尝试将指定的控件的背景以默认配置设置为亚克力背景
        /// </summary>
        /// <param name="control">指定的控件</param>
        /// <returns>指示过程是否成功的值</returns>
        public static bool TrySetAcrylicBrush(Control control)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
            {
                if (Application.Current.Resources["SystemControlChromeMediumLowAcrylicWindowMediumBrush"] is AcrylicBrush brush)
                {
                    control.Background = brush;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
