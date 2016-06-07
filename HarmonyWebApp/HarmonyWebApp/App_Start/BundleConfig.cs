using System.Web;
using System.Web.Optimization;

namespace HarmonyWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/main.js",
                      "~/Scripts/sidebar.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapLogin").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/login.css",
                      "~/Content/media.css"));

            bundles.Add(new StyleBundle("~/Content/cssLoggedIn").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/sidebar.css",
                     "~/Content/home.css",
                     "~/Content/font-awesome.min.css",
                     "~/Content/media.css"));
        }
    }
}
