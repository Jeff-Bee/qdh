using System.Web;
using System.Web.Optimization;

namespace Laplace.LiteCOS.Wechat
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
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/Script/bootstrap").Include(
                     "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Style/bootstrap").Include(
                      "~/Content/bootstrap.css"
                      ));




            bundles.Add(new ScriptBundle("~/bundles/easyuiScript").Include(
                  "~/Scripts/EasyUI/jquery.easyui.min.js",
                  "~/Scripts/EasyUI/jquery.easyui.mobile.js",
                  "~/Scripts/EasyUI/easyui-lang-zh_CN.js"                
                ));

            bundles.Add(new StyleBundle("~/Content/easyuiCss").Include(              
                "~/Content/EasyUI/themes/mobile.css",
                "~/Content/EasyUI/themes/icon.css"
                ));


            bundles.Add(new StyleBundle("~/Style/weui").Include(
              "~/Styles/weui/weui.min.css"
              ));

            bundles.Add(new ScriptBundle("~/Script/weui").Include(
                "~/Scripts/weui/weui.min.js"
                ));
        }
    }
}
