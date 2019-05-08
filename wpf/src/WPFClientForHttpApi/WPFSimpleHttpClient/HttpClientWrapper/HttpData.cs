namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;

	public class HttpData<T>
	{
		public HttpData(HttpResponseMessage response, T content)
		{
			this.RequestUri = response.RequestMessage.RequestUri;
			this.StatusCode = response.StatusCode;
			this.ContentType = response.Content.Headers.ContentType;
			this.Content = content;
			this.IsSuccessStatusCode = response.IsSuccessStatusCode;
		}

		public HttpData(HttpData<string> data, T content)
		{
			this.RequestUri = data.RequestUri;
			this.StatusCode = data.StatusCode;
			this.ContentType = data.ContentType;
			this.Content = content;
			this.IsSuccessStatusCode = data.IsSuccessStatusCode;
		}

		public Uri RequestUri { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }
		public MediaTypeHeaderValue ContentType { get; private set; }
		public T Content { get; private set; }
		public bool IsSuccessStatusCode { get; private set; }

		public void SetSuccessFlag(bool flag) =>
			this.IsSuccessStatusCode = flag;
	}
}
