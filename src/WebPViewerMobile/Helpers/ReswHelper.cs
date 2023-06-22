using Windows.ApplicationModel.Resources;

namespace WebPViewerMobile.Helpers
{
    internal class ReswHelper
    {
        public static string GetReswString(string name)
        {
            ResourceLoader loader = new ResourceLoader();
            return loader.GetString(name);
        }
    }
}
