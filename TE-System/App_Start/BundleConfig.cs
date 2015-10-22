using System.Web;
using System.Web.Optimization;

namespace TE_System
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/metro-bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/selectize.default.css",
                      "~/Content/selectize.bootstrap3.css",
                      "~/Content/selectize.min.css",
                      "~/Content/bootstrap-table.min.css",
                      "~/Content/bootstrap-datepicker3.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/customscripts").IncludeDirectory(
                "~/Scripts/tes", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").IncludeDirectory(
                "~/Scripts/bootstrap", "*.js"));


            BundleTable.EnableOptimizations = true;
        }
    }
}
