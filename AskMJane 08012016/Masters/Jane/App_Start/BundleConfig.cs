using System.Web.Optimization;

namespace Jane.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Style Bundles
            bundles.Add(new StyleBundle("~/bundles/cssbundle").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/site.css",
                      "~/Content/isteven-multiselect/isteven-multiselect.css"));

            bundles.Add(new StyleBundle("~/bundles/smartadmincss").Include(
                    "~/Content/animate-admin.css",
                    "~/Content/style-admin.css",
                    "~/Content/DataTables/css/dataTables.bootstrap.min.css"));
            // End Style Bundles 

            // Scripts Bundles
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/smartpanel").Include(
                   "~/Scripts/jquery.metisMenu.js",
                   "~/Scripts/jquery.slimscroll.min.js",
                   "~/Scripts/pace.min.js",
                   "~/Scripts/inspinia.js",
                   "~/Content/DataTables/js/jquery.dataTables.js",
                   "~/Content/DataTables/js/dataTables.bootstrap.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/js-cookies.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-cookies.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/ngGeolocation.js",
                "~/Scripts/isteven-multiselect/isteven-multiselect.js",
                "~/Scripts/angular-google-maps.js",
                 "~/Scripts/angular-ui/angular-ui.min.js",
                "~/Scripts/ngPlacesAutocomplete.js",
                "~/Scripts/ng-file-upload.js",
                "~/Scripts/ng-file-upload-shim.min.js",
                "~/Scripts/jquery.dotdotdot.min.js",
                "~/Scripts/custom-hover.js",
                "~/Scripts/app/app.js",
                "~/Scripts/tools/spinnerDirective.js",
                "~/Scripts/app/AdminCtrl.js",
                "~/Scripts/app/MainCtrl.js",
                "~/Scripts/app/AdminOrderCtrl.js",
                "~/Scripts/app/DispensaryDetailsCtrl.js",
                "~/Scripts/app/DispensaryCtrl.js",
                "~/Scripts/app/InventoryCtrl.js",
                "~/Scripts/app/DispensaryDashboardCtrl.js",
                "~/Scripts/app/MasterProductCtrl.js",
                "~/Scripts/app/LoginCtrl.js",
                "~/Scripts/app/StoreCtrl.js",
                "~/Scripts/app/PatientApplicationCtrl.js",
                "~/Scripts/app/PatientApprovalCtrl.js",
                "~/Scripts/app/RolesManagerCtrl.js",
                "~/Scripts/app/CheckoutCtrl.js",
                "~/Scripts/app/CartCtrl.js",
                "~/Scripts/app/ProductCtrl.js",
                "~/Scripts/app/MyAccountCtrl.js",
                "~/Scripts/app/PendingDispensaryCtrl.js",
                "~/Scripts/jquery.sidr.min.js"
                ));



            //End Script Bundles
        }
    }
}
