using System.Web;
using System.Web.Optimization;

namespace CryptoSystemDissertation
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

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/require.js",
                      "~/Scripts/pbkdf2.js",
                      "~/Scripts/System/System.debug.js",
                      "~/Scripts/System/System.IO.debug.js",
                      "~/Scripts/System/System.Text.debug.js",
                      "~/Scripts/System/System.Convert.debug.js",
                      "~/Scripts/System/System.BitConverter.debug.js",
                      "~/Scripts/System/System.BigInt.debug.js",
                      "~/Scripts/System/System.Security.Cryptography.SHA1.debug.js",
                      "~/Scripts/System/System.Security.Cryptography.debug.js",
                      "~/Scripts/System/System.Security.Cryptography.RSA.debug.js",
                      "~/Scripts/xml2json.min.js",
                      "~/Scripts/System/AppModules.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css"));
        }
    }
}
