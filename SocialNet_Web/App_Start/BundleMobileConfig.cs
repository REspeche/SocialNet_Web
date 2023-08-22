using System.Web.Optimization;

namespace SocialNet_Web
{
    public class BundleMobileConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* Layout */

            bundles.Add(new ScriptBundle("~/js/layout.mobile").Include(
                        "~/assets/js/modules/modernizr/modernizr-*",
                        "~/assets/js/modules/jquery/jquery-*",
                        "~/assets/js/modules/angular/angular.js",
                        "~/assets/js/modules/angular/angular-cookies.js",
                        "~/assets/js/modules/angular/angular-sanitize.js",
                        "~/assets/js/modules/autocomplete/angucomplete-alt.js",
                        "~/assets/js/modules/dialog-polyfill/dialog-polyfill.js",
                        "~/assets/js/modules/toastr/toastr.js",
                        "~/assets/js/modules/file-upload/ng-file-upload.js",
                        "~/assets/js/modules/datetimepicker/ADM-dateTimePicker.js",
                        "~/assets/js/commonFunctions.js",
                        "~/assets/js/app.mobile.js",
                        "~/assets/js/views/shared/_layout.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
