namespace WPFClientApp.WebApiClient
{
	using Newtonsoft.Json;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	/// <summary>
	/// MS Docs: Call a Web API From a .NET Client (C#)
	/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
	/// </summary>
	public class WebApiHttpClient : IDisposable
	{
		public delegate void CustomErrorEventHandler(object sender, HttpErrorEventArgs e);
		public CustomErrorEventHandler ErrorEventHandler;
		private static HttpClient _client = new HttpClient();

		public WebApiHttpClient(Uri baseAddress, CustomErrorEventHandler eventHandler)
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

		protected virtual void OnErrorOccured(HttpErrorEventArgs eventArgs) =>
			ErrorEventHandler?.Invoke(this, eventArgs);

		private string MakeRequestUri(string apiPath) =>
			Flurl.Url.Combine(_client.BaseAddress.ToString(), apiPath);

		private string MakeRequestUri(string apiPath, object parameter) =>
			Flurl.Url.Combine(_client.BaseAddress.ToString(), apiPath, parameter?.ToString() ?? string.Empty);

		#region Public interface

		public async Task<T> GetAsync<T>(Uri requestUri)
		{
			T result = default(T);

			try
			{
				HttpResponseMessage response = await _client.GetAsync(requestUri);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					result = JsonConvert.DeserializeObject<T>(data);
				}
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, requestUri.ToString()));
			}

			return result;
		}

		public async Task<T> GetAsync<T>(string apiPath, object parameter = null)
		{
			string path = MakeRequestUri(apiPath, parameter);

			Uri uri = null;

			if (Uri.TryCreate(path, UriKind.Absolute, out uri))
			{
				return await GetAsync<T>(uri);
			}

			return await Task.FromResult(default(T));
		}

		/// <summary>
		/// Makes a POST request to create new resource
		/// </summary>
		/// <returns>return URI of the created resource</returns>
		public async Task<Uri> CreateAsync<T>(string apiPath, T item)
		{
			HttpResponseMessage response = null;
			bool success = false;
			string path = MakeRequestUri(apiPath);

			try
			{
				response = await _client.PostAsJsonAsync(path, item);
				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();
				success = true;
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, path));
			}

			return success ? response?.Headers.Location : null;
		}

		public async Task<bool> UpdateAsync<T>(string apiPath, T item, object parameter)
		{
			bool result = false;
			string path = MakeRequestUri(apiPath, parameter);

			try
			{
				HttpResponseMessage response = await _client.PutAsJsonAsync(path, item);
				response.EnsureSuccessStatusCode();
				result = true;
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, path));
			}

			return result;
		}

		public async Task<bool> DeleteAsync(string apiPath, object parameter)
		{
			bool result = false;
			string path = MakeRequestUri(apiPath, parameter);

			try
			{
				HttpResponseMessage response = await _client.DeleteAsync(path);

				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();

				result = true;
			}
			catch (Exception ex)
			{
				OnErrorOccured(new HttpErrorEventArgs(ex, path));
			}

			return result;
		}

		#endregion //Public interface

		#endregion //Methods
	}
}
