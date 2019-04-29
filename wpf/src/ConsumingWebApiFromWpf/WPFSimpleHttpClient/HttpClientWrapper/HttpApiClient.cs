namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using Newtonsoft.Json;
	using System;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;

	/// <summary>
	/// MS Docs: Call a Web API From a .NET Client (C#)
	/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
	/// </summary>
	public class HttpApiClient : IDisposable
	{
		public delegate void CustomErrorEventHandler(object sender, HttpErrorEventArgs e);
		public CustomErrorEventHandler ErrorEventHandler;
		private static HttpClient _client = new HttpClient();

		public HttpApiClient(Uri baseAddress, CustomErrorEventHandler eventHandler)
		{
			ErrorEventHandler += eventHandler;

			try
			{
				_client.BaseAddress = baseAddress;
				_client.DefaultRequestHeaders.Accept.Clear();
				_client.DefaultRequestHeaders.Accept.Add(
					new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, baseAddress.ToString()));
			}
		}

		public void Dispose()
		{
			_client?.Dispose();
		}

		#region Methods

		#region Public interface

		public virtual void OnErrorOccured(HttpErrorEventArgs eventArgs) =>
			ErrorEventHandler?.Invoke(this, eventArgs);

		public string GetRequestUriString(string apiPath) =>
			Flurl.Url.Combine(_client.BaseAddress.ToString(), apiPath);

		public async Task<string> GetAsync(Uri requestUri)
		{
			string result = string.Empty;
			HttpResponseMessage response = null;

			try
			{
				response = await _client.GetAsync(requestUri);

				result = await response.Content.ReadAsStringAsync();

				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, requestUri.ToString(), result));
			}

			return result;
		}

		#endregion //Public interface

		#endregion //Methods
	}
}
