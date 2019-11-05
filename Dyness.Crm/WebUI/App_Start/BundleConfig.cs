using System.Web.Optimization;

namespace WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/assets/vendors/base/vendors.bundle.css").Include("~/assets/vendors/base/vendors.bundle.css"));

            bundles.Add(new ScriptBundle("~/assets/vendors/base/vendors.bundle.js").Include("~/assets/vendors/base/vendors.bundle.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}