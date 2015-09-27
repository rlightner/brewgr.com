using System.Web.Optimization;

namespace Brewgr.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/js")
				.Include(
					"~/js/lib/jquery.tmpl.min.js",
                    "~/js/lib/jquery.colorbox.js",

                    "~/js/lib/tooltip.js",
                    "~/js/lib/jquery.tipsy.js",
                    "~/js/lib/jquery.validate.js",
                    "~/js/lib/jquery.validate.unobtrusive.js",

                    "~/js/lib/jquery.raty.js",
                    "~/js/lib/jquery.filter_input.js",
                    "~/js/lib/jquery.timeago.js",
                    "~/js/lib/t.js",
                    "~/js/lib/jquery.chosen.js",
                    "~/js/lib/jquery.autosize.js",

                    "~/js/plugins.js",
					"~/js/script.js",
					"~/js/utility.js",
					"~/js/layout.js",
					"~/js/builder.js",
					"~/js/session.js",
					"~/js/style-chart.js"));

			bundles.Add(new StyleBundle("~/bundles/css")
				.Include(
					//"~/css/smoothness/jquery-ui-1.10.3.custom.css",
					//"~/css/base.css",
					//"~/css/skeleton.css",
                    //"~/css/style.css",
                    //"~/css/shortcodes.css",
                    //"~/css/custom.css",
					//"~/css/builder.css",

					"~/css/site.css",
                    "~/css/responsive.css",
                    "~/css/lib/colorbox.css"));
		} 
	}
}