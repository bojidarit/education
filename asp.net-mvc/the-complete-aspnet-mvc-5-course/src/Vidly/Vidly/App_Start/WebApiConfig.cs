namespace Vidly
{
	using System.Web.Http;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var settings = config.Formatters.JsonFormatter.SerializerSettings;

			settings.ContractResolver = 
				new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

			settings.Formatting = 
				Newtonsoft.Json.Formatting.Indented;

			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
