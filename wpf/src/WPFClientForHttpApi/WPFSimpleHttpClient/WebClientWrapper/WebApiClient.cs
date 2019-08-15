namespace WPFSimpleHttpClient.WebClientWrapper
{
	using System;
	using System.Net;
	using System.Threading.Tasks;

	public class WebApiClient : IDisposable
	{
		public Uri BaseUri { get; private set; } = null;

		private string _contentType = Common.JsonContentType;
		private WebClient _webClient = null;

		public WebApiClient(Uri baseUri)
		{
			this.BaseUri = baseUri;
			_webClient = new WebClient();
			_webClient.Headers[HttpRequestHeader.ContentType] = _contentType;
		}

		public void Dispose()
		{
			_webClient?.Dispose();
		}

		public async Task<string> PostAsync<T>(string requestAddress, T value)
		{
			string result = null;

			try
			{
				string body = Common.PrepareJsonBody(value);
				result = await _webClient.UploadStringTaskAsync(requestAddress, body);
			}
			catch (Exception ex)
			{
				// TODO: ...
			}

			return result;
		}
	}
}
