namespace InfluxDemo.Client.Database
{
	using RestSharp;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Threading.Tasks;
	using System.Windows;

	// RestSharp library used
	// Source: https://restsharp.dev/api/
	public static class InfluxRest
	{
		private const string SchemaMeasurements = "MEASUREMENTS";
		private const string SchemaFieldKeys = "FIELD KEYS";
		private const string SchemaTagKeys = "TAG KEYS";

		#region Schema Methods

		public static async Task<List<string>> GetDatabases()
		{
			var csv = await GetRawAsync("SHOW DATABASES");
			if (string.IsNullOrEmpty(csv))
			{
				return null;
			}

			return Helper.CsvGetLastColumn(csv);
		}

		public static async Task<List<string>> GetMeasurements(string database)
		{
			var csv = await GetRawAsync($"SHOW {SchemaMeasurements}", database);
			if (string.IsNullOrEmpty(csv))
			{
				return null;
			}

			return Helper.CsvGetLastColumn(csv);
		}

		public static async Task<List<TagKeyItem>> GetTagKeys(string db, string measure)
		{
			var csv = await GetRawAsync($"SHOW {SchemaTagKeys} FROM \"{measure}\"", db);
			if (string.IsNullOrEmpty(csv))
			{
				return null;
			}

			var helper = new { tagKey = string.Empty };
			var list = Helper.MapToAnonymousCsv(csv, helper);

			var result = list.Select(i => new TagKeyItem
			{
				Measurement = measure,
				Key = i.tagKey
			}).ToList();

			return result;
		}

		public static async Task<List<FieldKeyItem>> GetFieldKeys(string db, string measure)
		{
			var csv = await GetRawAsync($"SHOW {SchemaFieldKeys} FROM \"{measure}\"", db);
			if (string.IsNullOrEmpty(csv))
			{
				return null;
			}

			var helper = new
			{
				name = string.Empty,
				fieldKey = string.Empty,
				fieldType = string.Empty,
			};
			var list = Helper.MapToAnonymousCsv(csv, helper);
			var fieldKeys = list.Select(i => new FieldKeyItem(i.name, i.fieldKey, i.fieldType)).ToList();

			return fieldKeys;
		}

		public static async Task<string> GetRawAsync(string query, string db = null)
		{
			try
			{
				var content = await QueryRawAsync(query, db);
				return content;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, query);
			}

			return null;
		}

		#endregion


		#region Helpers

		private static async Task<string> QueryRawAsync(string query, string db = null, Epoch epoch = Epoch.S)
		{
			var client = CreateRestClient();
			var response = await ExecuteQueryAsync(client, query, db, epoch);
			var content = response.Content;
			var contentLength = content?.Length ?? -1;

			if (response.IsSuccessful)
			{
				Debug.WriteLine($"<== Response status: {response.StatusCode}; " +
					$"Content type: '{response.ContentType}'; Content Length: {contentLength}");
			}
			else
			{
				var contentStr = (contentLength <= 0)
					? string.Empty
					: $"{Environment.NewLine}*** Content: {response.Content}";
				var error = $"<== Response status: {response.StatusCode}{contentStr}" +
						$"{Environment.NewLine}*** Error: {response.ErrorMessage}";

				Debug.WriteLine(error);

				throw new Exception(error, response.ErrorException);
			}

			return content;
		}

		private static RestClient CreateRestClient(string accept = "application/csv")
		{
			var client = new RestClient(ConfData.OssUrl);
			client.AddDefaultHeader("Accept", accept);
			var auth = System.Text.Encoding.UTF8.GetBytes(ConfData.OssUsername + ":" + new string(ConfData.OssPassword));
			client.AddDefaultHeader("Authorization", "Basic " + Convert.ToBase64String(auth));

			var assemblyName = Assembly.GetCallingAssembly().GetName();
			client.UserAgent = $"{assemblyName}/OSS";

			var parameters = client.DefaultParameters.Select(p => $"[{p}]");
			Debug.WriteLine($"==> Rest Client Default Parameters: {string.Join(", ", parameters)}");

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

			Debug.WriteLine($"==> Sending request to: {client.BaseUrl}");
			var parameters = restRequest.Parameters.Select(p => $"[{p}]");
			Debug.WriteLine($"==> Rest Request Parameters: {string.Join(", ", parameters)}");

			var response = await Task.Run(() => client.Execute(restRequest));

			return response;
		}

		#endregion
	}
}
