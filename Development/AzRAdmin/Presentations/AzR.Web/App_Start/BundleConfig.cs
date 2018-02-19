using System.Web.Optimization;

namespace AzR.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive").Include(
                "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/singalr").Include(
                "~/Scripts/jquery.signalR-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/app-js").Include(
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/select2.js",
                "~/Scripts/respond.js",
                "~/Scripts/app/underscore.js",
                "~/Scripts/app/spin.js",
                "~/Scripts/app/drag-drop.js",
                "~/Scripts/app/azr-admin.js"));

            bundles.Add(new StyleBundle("~/Content/app-css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/font-awesome.css",
                "~/Content/app/ionicons.css",
                "~/Content/app/ionicons.css",
                "~/Content/app/flag-icon.css",
                "~/Content/PagedList.css",
                "~/Content/app/animate.css",
                "~/Content/css/select2.css",
                "~/Content/select2-bootstrap.css",
                "~/Content/app/azr-admin.css"));



#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
