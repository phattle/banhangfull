using System.Web;
using System.Web.Optimization;

namespace OnChotto
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/BootstrapValidator/css").Include(
               "~/Scripts/BootstrapValidator/css/bootstrapValidator.min.css"
            ));
            bundles.Add(new ScriptBundle("~/BootstrapValidator/jquery").Include(
              "~/Scripts/BootstrapValidator/js/bootstrapValidator.min.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                        "~/Content/select2.min.css"));

            bundles.Add(new StyleBundle("~/Assets/Frontend/css/css").Include(
                    "~/Assets/Frontend/css/bootstrap.min.css",
                    "~/Assets/Frontend/css/font-awesome.min.css",
                    "~/Assets/Frontend/css/owl.carousel.css",
                    "~/Assets/Frontend/css/owl.transitions.css",
                    "~/Assets/Frontend/css/lightbox.css",
                    "~/Assets/Frontend/css/animate.min.css",
                    "~/Assets/Frontend/css/bootstrap-select.min.css",
                    "~/Assets/Frontend/css/config.css",
                    "~/Assets/Frontend/css/main.css", 
                    "~/Assets/Frontend/css/green.css",
                    "~/Assets/Frontend/css/site.css",
                    "~/Assets/Frontend/css/bootstrap-select.css",
                    "~/Assets/Frontend/css/bootstrap-select.min.css"

            ));

            bundles.Add(new ScriptBundle("~/Assets/Frontend/jquery").Include(
                       "~/Assets/Frontend/js/jquery-1.11.1.min.js",
                       "~/Assets/Frontend/js/bootstrap.min.js",
                       "~/Assets/Frontend/js/bootstrap-hover-dropdown.min.js",
                       "~/Assets/Frontend/js/owl.carousel.min.js",
                       "~/Assets/Frontend/js/echo.min.js",
                       "~/Assets/Frontend/js/jquery.easing-1.3.min.js",
                       "~/Assets/Frontend/js/bootstrap-slider.min.js",
                       "~/Assets/Frontend/js/jquery.rateit.min.js",
                       "~/Assets/Frontend/js/lightbox.min.js",
                       "~/Assets/Frontend/js/bootstrap-select.min.js",
                       "~/Scripts/jquery.popupoverlay.js",
                       "~/Assets/Frontend/js/wow.min.js",
                       "~/Assets/Frontend/js/scripts.js",
                       "~/Assets/Frontend/js/bootstrap-select.js",
                       "~/Assets/Frontend/js/script-bootstrap-select.js",
                        "~/Scripts/jquery.number.min.js",
                        "~/Scripts/FlyingCart.js",
                       "~/Scripts/select2.min.js",
                       "~/Scripts/Dropdown.js",
                       "~/Scripts/Paging.js",
                       "~/Scripts/kkcountdown.min.js",
                       "~/Scripts/app.js"
            ));






        }
    }
}
