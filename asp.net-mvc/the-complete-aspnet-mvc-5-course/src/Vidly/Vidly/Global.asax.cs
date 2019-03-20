namespace Vidly
{
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_BeginRequest()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				new System.Globalization.CultureInfo("en-us");
			System.Threading.Thread.CurrentThread.CurrentUICulture =
				new System.Globalization.CultureInfo("en-us");
		}
	}
}
