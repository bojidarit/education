namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	/// <summary>
	/// MS Docs: Call a Web API From a .NET Client (C#)
	/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
	/// </summary>
	public class HttpApiClient : IDisposable
	{
		public static string jsonEncoding = "application/json";

		public delegate void CustomErrorEventHandler(object sender, HttpErrorEventArgs e);
		public delegate void ExecutionInfoEventHandler(object sender, ExecutionInfoEventArgs e);
		
		public CustomErrorEventHandler ErrorEventHandler;
		public ExecutionInfoEventHandler ExecutionInfoHandler;

		private static HttpClient _client = new HttpClient();

		public HttpApiClient(Uri baseAddress, CustomErrorEventHandler errorEventHandler, ExecutionInfoEventHandler executionEventHandler = null)
		{
			ErrorEventHandler += errorEventHandler;
			if(executionEventHandler != null)
			{
				ExecutionInfoHandler += executionEventHandler;
			}

			try
			{
				_client.BaseAddress = baseAddress;
				_client.DefaultRequestHeaders.Accept.Clear();
				_client.DefaultRequestHeaders.Accept.Add(
					new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(jsonEncoding));
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

		public virtual void OnRequestExecute(ExecutionInfoEventArgs eventArgs) =>
			ExecutionInfoHandler?.Invoke(this, eventArgs);

		public string GetRequestUriString(string apiPath) =>
			Flurl.Url.Combine(_client.BaseAddress.ToString(), apiPath);

		public async Task<HttpData<string>> GetAsync(Uri requestUri)
		{
			HttpData<string> result = null;
			string data = string.Empty;
			HttpResponseMessage response = null;

			try
			{
				OnRequestExecute(new ExecutionInfoEventArgs(requestUri));
				response = await _client.GetAsync(requestUri);

				data = await response.Content.ReadAsStringAsync();
				result = new HttpData<string>(response, data);
				
				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, requestUri.ToString(), HttpVerb.Get, $"Content: '{result?.Content}'"));
			}

			return result;
		}

		public async Task<HttpData<string>> PostAsync<T>(Uri requestUri, T value)
		{
			HttpData<string> result = null;
			string data = string.Empty;
			HttpResponseMessage response = null;

			try
			{
				// Prepare JSON content
				string json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
				var content = new StringContent(json, System.Text.Encoding.UTF8, jsonEncoding);

				OnRequestExecute(new ExecutionInfoEventArgs(requestUri));
				response = await _client.PostAsync(requestUri, content);

				data = await response.Content.ReadAsStringAsync();
				result = new HttpData<string>(response, data);

				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, requestUri.ToString(), HttpVerb.Post, $"Content: '{result?.Content}'"));
			}

			return result;
		}

		#endregion //Public interface

		#endregion //Methods
	}
}
