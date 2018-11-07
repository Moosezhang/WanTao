using System.Web;
using System.Web.Optimization;

namespace AccountTrain.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/nprogress").Include("~/Scripts/nprogress-0.2.0/nprogress.css"));
            bundles.Add(new ScriptBundle("~/bundles/nprogress").Include("~/Scripts/nprogress-0.2.0/nprogress.js"));

            bundles.Add(new StyleBundle("~/Content/elementui").Include("~/Content/element-ui.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/elementui").Include("~/Scripts/element-ui.min.js"));

            bundles.Add(new StyleBundle("~/Content/amazeui").Include("~/Content/amazeui.min.css", "~/Content/bootstrap.css"));
            bundles.Add(new ScriptBundle("~/bundles/amazeui").Include("~/Scripts/amazeui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                      "~/Scripts/vue.js"
                       , "~/Scripts/polyfill.min.js"
                      , "~/Scripts/axios.min.js"
                      , "~/Scripts/vee-validate.min.js"
                                             , "~/Scripts/zh_CN.js"
                                                                    , "~/Scripts/setup.js"));
            bundles.Add(new ScriptBundle("~/bundles/echart").Include("~/Scripts/echarts-3.8.4/echarts-3.8.4.min.js", "~/Scripts/echarts-3.8.4/theme/shine.js"));
        }
    }
}
