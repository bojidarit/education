namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;

	public class HttpData
	{
		public HttpData(HttpResponseMessage response, string content)
		{
			this.RequestUri = response.RequestMessage.RequestUri;
			this.StatusCode = response.StatusCode;
			this.ContentType = response.Content.Headers.ContentType;
			this.Content = content;
			this.IsSuccessStatusCode = response.IsSuccessStatusCode;
		}

		public Uri RequestUri { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }
		public MediaTypeHeaderValue ContentType { get; private set; }
		public string Content { get; private set; }
		public bool IsSuccessStatusCode { get; private set; }
	}
}
