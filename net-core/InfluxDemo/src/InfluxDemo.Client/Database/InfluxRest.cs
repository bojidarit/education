namespace InfluxDemo.Client.Database
{
	using RestSharp;
	using System;
	using System.Reflection;
	using System.Threading.Tasks;

	// RestSharp library used
	// Source: https://restsharp.dev/api/
	public static class InfluxRest
	{
		public static async Task<string> QueryRawAsync(string query, string db = null, Epoch epoch = Epoch.None)
		{
			var client = CreateRestClient();
			var response = await ExecuteQueryAsync(client, query, db, epoch);
			if (!response.IsSuccessful)
			{
				var content = string.IsNullOrEmpty(response.Content)
					? string.Empty
					: $"{Environment.NewLine}*** Content: {response.Content}";
				throw new Exception(
					$"==> Response status: {response.StatusCode}{content}" +
						$"{Environment.NewLine}*** Error: {response.ErrorMessage}",
					response.ErrorException);
			}

			return response.Content;
		}

		private static RestClient CreateRestClient(string accept = "application/csv")
		{
			var client = new RestClient(ConfData.OssUrl);
			client.AddDefaultHeader("Accept", accept);
			var auth = System.Text.Encoding.UTF8.GetBytes(ConfData.OssUsername + ":" + new string(ConfData.OssPassword));
			client.AddDefaultHeader("Authorization", "Basic " + Convert.ToBase64String(auth));

			var assemblyName = Assembly.GetCallingAssembly().GetName();
			client.UserAgent = $"{assemblyName}/OSS";

			return client;
		}

		private static async Task<IRestResponse> ExecuteQueryAsync(
			RestClient client,
			string query,
			string db = null,
			Epoch epoch = Epoch.None)
		{
			var restRequest = new RestRequest("query", Method.GET);
			restRequest.AddParameter("q", query, ParameterType.QueryString);

			if (!string.IsNullOrEmpty(db))
			{
				restRequest.AddParameter("db", db, ParameterType.QueryString);
			}

			if (epoch != Epoch.None)
			{
				restRequest.AddParameter("epoch", epoch.ToString().ToLower(), ParameterType.QueryString);
			}

			var response = await Task.Run(() => client.Execute(restRequest));

			return response;
		}
	}
}
