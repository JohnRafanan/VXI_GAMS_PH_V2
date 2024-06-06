using System.Web;
using System.Web.Optimization;

namespace VXI_GAMS_US
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"
            //,"~/public/momentjs/moment.js"
            //,"~/public/multi-select/js/jquery.multi-select.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/bicubicInterpolation").Include(
                "~/public/plugins/bicubicInterpolation/bicubicInterpolation.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/public/js/admin.js",
                "~/public/app/app.js",
                "~/public/app/services/popup.service.js",
                "~/public/app/services/toast.service.js",
                "~/public/app/factories/api.factory.js",
                "~/public/app/factories/signalr.factory.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/controller/login").Include(
                "~/public/app/controllers/login.controller.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/controller/home").Include(
                "~/public/app/controllers/home.controller.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Main/css").Include(
                "~/public/multi-select/css/multi-select.css",
                "~/public/font/roboto/roboto.css",
                "~/public/css/materialize.css",
                "~/public/css/style.css"));
        }
    }
}
