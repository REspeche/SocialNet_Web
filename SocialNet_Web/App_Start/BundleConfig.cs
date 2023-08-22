using System.Web.Optimization;

namespace SocialNet_Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* Layout */

            bundles.Add(new StyleBundle("~/css/layout").Include(
                        "~/assets/css/modules/autocomplete/angucomplete-alt.css",
                        "~/assets/css/modules/dialog-polyfill/dialog-polyfill.css",
                        "~/assets/css/modules/toastr/toastr.css",
                        "~/assets/css/modules/datetimepicker/ADM-dateTimePicker.css",
                        "~/assets/css/views/shared/_layout.css"));

            bundles.Add(new ScriptBundle("~/js/layout").Include(
                        "~/assets/js/modules/modernizr/modernizr-*",
                        "~/assets/js/modules/jquery/jquery-*",
                        "~/assets/js/modules/angular/angular.js",
                        "~/assets/js/modules/angular/angular-cookies.js",
                        "~/assets/js/modules/angular/angular-sanitize.js",
                        "~/assets/js/modules/autocomplete/angucomplete-alt.js",
                        "~/assets/js/modules/dialog-polyfill/dialog-polyfill.js",
                        "~/assets/js/modules/toastr/toastr.js",
                        "~/assets/js/modules/masonry/masonry.pkgd.min.js",
                        "~/assets/js/modules/masonry/angular-masonry.js",
                        "~/assets/js/modules/file-upload/ng-file-upload.js",
                        "~/assets/js/modules/datetimepicker/ADM-dateTimePicker.js",
                        "~/assets/js/commonFunctions.js",
                        "~/assets/js/app.js",
                        "~/assets/js/views/shared/_layout.js"));

            /* Views Index */
            /*  --Wall */

            bundles.Add(new StyleBundle("~/css/wall/index").Include(
                        "~/assets/css/modules/magnific-popup/magnific-popup.css",
                        "~/assets/css/views/wall/index.css"));

            bundles.Add(new ScriptBundle("~/js/wall/index").Include(
                        "~/assets/js/modules/magnific-popup/magnific-popup.js",
                        "~/assets/js/views/wall/index.js"));

            /*  --Photo */

            bundles.Add(new StyleBundle("~/css/photo/index").Include(
                        "~/assets/css/modules/magnific-popup/magnific-popup.css",
                        "~/assets/css/views/wall/index.css"));

            bundles.Add(new ScriptBundle("~/js/photo/index").Include(
                        "~/assets/js/modules/magnific-popup/magnific-popup.js",
                        "~/assets/js/views/wall/index.js"));

            /*  --Video */

            bundles.Add(new StyleBundle("~/css/video/index").Include(
                        "~/assets/css/modules/magnific-popup/magnific-popup.css",
                        "~/assets/css/views/wall/index.css"));

            bundles.Add(new ScriptBundle("~/js/video/index").Include(
                        "~/assets/js/modules/magnific-popup/magnific-popup.js",
                        "~/assets/js/views/wall/index.js"));

            /*  --Friend */

            bundles.Add(new StyleBundle("~/css/friend/index").Include(
                        "~/assets/css/views/friend/general.css",
                        "~/assets/css/views/friend/index.css"));

            bundles.Add(new ScriptBundle("~/js/friend/index").Include(
                        "~/assets/js/views/friend/index.js"));

            /*  --Profile */

            bundles.Add(new StyleBundle("~/css/profile/index").Include(
                        "~/assets/css/views/profile/index.css"));

            bundles.Add(new ScriptBundle("~/js/profile/index").Include(
                        "~/assets/js/views/profile/index.js"));

            /* Views Account */

            bundles.Add(new ScriptBundle("~/js/shared/_login").Include(
                        "~/assets/js/views/shared/_login.js"));

            bundles.Add(new ScriptBundle("~/js/account/changepassword").Include(
                        "~/assets/js/views/account/changepassword.js"));

            /*  --Group */

            bundles.Add(new StyleBundle("~/css/group/index").Include(
                        "~/assets/css/views/friend/general.css",
                        "~/assets/css/views/group/index.css"));

            bundles.Add(new ScriptBundle("~/js/group/index").Include(
                        "~/assets/js/views/group/index.js"));

            /*  --Company */

            bundles.Add(new StyleBundle("~/css/company/index").Include(
                        "~/assets/css/views/friend/general.css",
                        "~/assets/css/views/company/index.css"));

            bundles.Add(new ScriptBundle("~/js/company/index").Include(
                        "~/assets/js/views/company/index.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
