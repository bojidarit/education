namespace WPFSimpleHttpClient.Extensions
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using WPFSimpleHttpClient.HttpClientWrapper;

	public static class HttpApiClientExtension
	{
		private static string _apiPath = "client/";

		public static async Task<IEnumerable<string>> GetMethodsAsync(this HttpApiClient client, string library)
		{
			string path = Flurl.Url.Combine(client.GetRequestUriString(_apiPath), "methods", library);

			Uri uri = null;
			if (Uri.TryCreate(path, UriKind.Absolute, out uri))
			{
				string data = await client.GetAsync(uri);

				if (!string.IsNullOrWhiteSpace(data))
				{
					return JsonConvert.DeserializeObject<IEnumerable<string>>(data);
				}
			}

			return null;
		}

		public static async Task<JToken[]> GetDataListAsync(this HttpApiClient client,
			string library, string method, object[] values)
		{
			JToken[] result = null;
			Uri uri = MakeSpecialRequestUri(client, library, method, values);

			string data = await client.GetAsync(uri);

			if (!string.IsNullOrWhiteSpace(data))
			{
				// Parse the JSON string
				JObject jObject = null;
				try
				{
					jObject = JObject.Parse(data);
				}
				catch (Exception ex)
				{
					client.OnErrorOccured(new HttpErrorEventArgs(ex, string.Empty, "Invalid response format. JSON expected."));
				}

				// Get "data" object as enumerable
				try
				{
					result = GetDataArray(jObject);
				}
				catch (Exception ex)
				{
					client.OnErrorOccured(new HttpErrorEventArgs(ex, string.Empty, "Invalid 'data' object."));
				}
			}

			return result;
		}

		public static async Task<DataTable> GetDataTableAsync(this HttpApiClient client,
			string library, string method, object[] values)
		{
			JToken[] data = null;

			try
			{
				data = await client.GetDataListAsync(library, method, values);
			}
			catch (Exception ex)
			{
				client.OnErrorOccured(new HttpErrorEventArgs(ex, string.Empty, "Invalid 'data' object."));
			}

			return JCollectionToDataTable(data);
		}

		/// <summary>
		/// Generates a pure JSON string from collection of JTokens and De-serialize it to a DataTable
		/// </summary>
		/// <param name="collection">Collection of JTokens</param>
		/// <returns>DataTable with all the item(s) in the collection</returns>
		private static DataTable JCollectionToDataTable(IEnumerable<JToken> collection)
		{
			DataTable result = null;

			if (collection != null)
			{
				var list = collection.Select(j => j.ToString(Formatting.None));

				string json = "[" + string.Join($",", list) + "]";

				result = JsonConvert.DeserializeObject<DataTable>(json);
			}

			return result;
		}

		/// <summary>
		/// Gets only "data" object as array from the parsed JSON object
		/// </summary>
		/// <param name="jObject"></param>
		/// <returns></returns>
		private static JToken[] GetDataArray(JObject jObject)
		{
			JToken[] result = null;

			if (jObject != null)
			{
				JToken jToken = null;

				if (jObject.TryGetValue("data", out jToken))
				{
					result = jToken.ToArray();
				}
			}

			return result;
		}

		/// <summary>
		/// Gets only "data" object as enumerable collection from the parsed JSON object
		/// </summary>
		/// <param name="jObject"></param>
		/// <returns></returns>
		private static IEnumerable<JToken> GetDataList(JObject jObject)
		{
			IEnumerable<JToken> result = null;

			if (jObject != null)
			{
				JToken jToken = null;

				if (jObject.TryGetValue("data", out jToken))
				{
					JArray array = (JArray)jToken;
					result = array as IEnumerable<JToken>;
				}
			}

			return result;
		}

		private static Uri MakeSpecialRequestUri(HttpApiClient client, string library, string method, params object[] values)
		{
			string path = $"{client.GetRequestUriString(_apiPath)}{library.ToLower()}.{method.ToLower()}?apikey=00000";

			if (values != null && values.Any())
			{
				int id = 1;
				var parameters = values.Select(i => $"p{id++}={i.ToString()}");
				path += $"&{string.Join("&", parameters)}";
			}

			Uri uri = null;
			Uri.TryCreate(path, UriKind.Absolute, out uri);

			return uri;
		}
	}
}
