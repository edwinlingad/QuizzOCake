using System.Web;
using System.Web.Optimization;

namespace L2L.WebApi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/lib/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/lib/jQuery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/lib/bootstrap.js",
                      "~/Scripts/lib/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap")
                        .Include("~/Content/lib/bootstrap.min.css",
                        new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css/font-awesome")
                        .Include("~/Content/lib/font-awesome-4.4.0/css/font-awesome.css",
                        new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/Content/css/qz-ext-lib").Include(
                        "~/Content/lib/animate/animate.min.css",
                        "~/Content/lib/hover/hover-min.css",
                        "~/Content/lib/bootstrap.min.css",
                        "~/Content/lib/toastr/toastr.min.css",
                        "~/Scripts/lib/jquery-ui-1.11.4.custom/jquery-ui.min.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/css/qz-core").Include(
                        "~/Content/stylesheets/utils/edhelpers.css",
                        "~/Content/stylesheets/utils/ed-css.css",
                        "~/Content/stylesheets/layout-promo.css",
                        "~/Content/stylesheets/forms.css",
                        "~/Content/stylesheets/layout.css",
                        "~/Content/stylesheets/common-site.css",
                        "~/Content/stylesheets/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/qz-main")
                        .IncludeDirectory("~/Content/stylesheets/common", "*.css", true)
                        .IncludeDirectory("~/Content/stylesheets/directives", "*.css", true)
                        .IncludeDirectory("~/Content/stylesheets/utils", "*.css", true)
                        .IncludeDirectory("~/Content/stylesheets/controllers", "*.css", true)
                        );

            bundles.Add(new ScriptBundle("~/bundles/qz-ext-lib").Include(
                        "~/Scripts/lib/bootstrap.js",
                        "~/Scripts/lib/respond.js",
                        "~/Scripts/lib/bootbox/bootbox.min.js",
                        "~/Scripts/lib/toastr/toastr.min.js",
                        "~/Scripts/lib/jquery-ui-1.11.4.custom/jquery-ui.min.js",
                        "~/Scripts/lib/jQuery/jquery.scrollTo.min.js",
                        "~/Scripts/lib/angular/angular.min.js",
                        "~/Scripts/lib/angular/angular-animate.min.js",
                        "~/Scripts/lib/angular/angular-touch.min.js",
                        "~/Scripts/lib/angular/angular-ui/angular-ui-router.js",
                        "~/Scripts/lib/angular/angular-ui/sortable.js",
                        "~/Scripts/lib/angular/angular-resource.min.js",
                        "~/Scripts/lib/angular/angular-route.min.js",
                        "~/Scripts/lib/angular/angular-ui/ui-bootstrap.min.js",
                        "~/Scripts/lib/angular/angular-ui/ui-bootstrap-tpls.min.js",
                        "~/Scripts/lib/angular/angular-sanitize.min.js",
                        "~/Scripts/lib/angular/angular-cookies.min.js",
                        "~/Scripts/lib/angular/zInfiniteScroll.js",
                        "~/Scripts/lib/angular-viewhead/angularjs-viewhead.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/qz-core").Include(
                        "~/Scripts/app/l2lApp.js",
                        "~/Scripts/app/l2lControllers.js",
                        "~/Scripts/app/constants.js")
                        .IncludeDirectory("~/Scripts/utils", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/qz-main")
                        .IncludeDirectory("~/Scripts/app/animation", "*.js", true)
                        .IncludeDirectory("~/Scripts/app/controllers", "*.js", true)
                        .IncludeDirectory("~/Scripts/app/directives", "*.js", true)
                        .IncludeDirectory("~/Scripts/app/filters", "*.js", true)
                        .IncludeDirectory("~/Scripts/app/services", "*.js", true)
                        .IncludeDirectory("~/Scripts/app/utilities", "*.js", true)
                        );
            bundles.Add(new ScriptBundle("~/bundles/qz-utils").IncludeDirectory(
                        "~/Scripts/utils",
                        "*.js",
                        true));

            /* Problems */

            //BundleTable.EnableOptimizations = true;

        }
    }
}
