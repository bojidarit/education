namespace WPFSimpleHttpClient
{
	using Dtos;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Common
	{
		public const string JsonContentType = "application/json";
		public const string HttpVerbPost = "POST";

		#region HTTP API Server specific

		public static readonly string ApiKeyParamName = "apikey";
		public static readonly string ApiKeyParamValue = "12345";
		public static readonly string ApiPath = "client/";
		public static readonly string JsonFormatParamValue = "json";

		#endregion //HTTP API Server specific

		#region Methods

		public static Uri MakeRequestUri(Uri baseAddress)
		{
			string path = Flurl.Url.Combine(baseAddress.ToString(), ApiPath);
			Uri uri = null;
			Uri.TryCreate(path, UriKind.Absolute, out uri);
			return uri;
		}

		public static string MakeRequestAddress(Uri baseAddress, string apiPath) =>
			Flurl.Url.Combine(baseAddress.ToString(), apiPath);

		public static string MakeRequestAddress(string baseAddress, string apiPath) =>
			Flurl.Url.Combine(baseAddress, apiPath);

		public static string PrepareJsonBody<T>(T data)
		{
			string body = Newtonsoft.Json.JsonConvert.SerializeObject(
				data,
				Newtonsoft.Json.Formatting.None,
				new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

			return body;
		}

		public static dynamic MakeExpandoBody(string library, string method, object[] values)
		{
			List<PropMold> props = new List<PropMold>
				{
					PropMold.Make("ApiKey", Common.ApiKeyParamValue),
					PropMold.Make("Format", Common.JsonFormatParamValue),
					PropMold.Make("Method", $"{library.ToLower()}.{method.ToLower()}"),
				};

			// Add method parameters
			if (values != null && values.Length > 0)
			{
				int id = 1;
				props.AddRange(values.Select(p => PropMold.Make($"p{id++}", p)));
			}

			dynamic parameters = Helpers.MakeExpandoWithDefaults(props);

			return parameters;
		}

		#endregion //Methods
	}
}
