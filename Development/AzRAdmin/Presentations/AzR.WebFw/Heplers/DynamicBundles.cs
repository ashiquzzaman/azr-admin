using System.Web;
using System.Web.Optimization;

namespace AzR.WebFw.Heplers
{
    public static class DynamicBundles
    {
        public static IHtmlString RenderSkin(string skin)
        {
            BundleTable.Bundles.Add(new StyleBundle("~/Content/admin-theme-" + skin).Include(
                "~/Content/app/azr-" + skin + ".css",
                "~/Content/app/style.css"));

            return Styles.Render("~/Content/admin-theme-" + skin);
        }
    }
}