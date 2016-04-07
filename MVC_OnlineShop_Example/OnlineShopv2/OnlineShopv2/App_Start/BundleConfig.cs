using System.Web;
using System.Web.Optimization;

namespace OnlineShopv2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jscore").Include(
                        "~/assets/client/js/jquery-1.12.0.min.js",
                        "~/assets/client/js/jquery-ui.js",
                        "~/assets/client/js/bootstrap.min.js",
                        "~/assets/client/js/move-top.js",
                        "~/assets/client/js/easing.js",
                        "~/assets/client/js/startstop-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/controller").Include(
                "~/assets/client/js/controller/baseController.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/core").Include(
                      "~/assets/client/css/bootstrap.css",
                      "~/assets/client/css/bootstrap-social.css",
                      "~/assets/client/css/font-awesome.min.css",
                      "~/assets/client/css/bootstrap-theme.css",
                      "~/assets/client/css/jquery-ui.css",
                      "~/assets/client/css/style.css",
                      "~/assets/client/css/slider.css"
                      ));
            
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
