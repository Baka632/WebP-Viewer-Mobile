using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WebPViewerMobile.Helpers
{
    public class XamlHelper
    {
        public static bool InverseBoolean(bool value) => !value;
        public static Visibility InverseVisibility(bool value) => !value ? Visibility.Visible : Visibility.Collapsed;
    }
}
