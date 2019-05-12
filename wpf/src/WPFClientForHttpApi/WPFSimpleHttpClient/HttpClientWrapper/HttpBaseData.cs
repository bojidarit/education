namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;

	/// <summary>
	/// Class to hold only the data that was needed form HttpResponseMessage
	/// </summary>
	public class HttpBaseData
	{
		public HttpBaseData(HttpResponseMessage response)
		{
			this.RequestUri = response.RequestMessage.RequestUri;
			this.StatusCode = response.StatusCode;
			this.ContentType = response.Content.Headers.ContentType;
			this.IsSuccessStatusCode = response.IsSuccessStatusCode;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		public HttpBaseData(HttpBaseData data)
		{
			this.RequestUri = data.RequestUri;
			this.StatusCode = data.StatusCode;
			this.ContentType = data.ContentType;
			this.IsSuccessStatusCode = data.IsSuccessStatusCode;
		}

		public Uri RequestUri { get; private set; }
		public HttpStatusCode StatusCode { get; private set; }
		public MediaTypeHeaderValue ContentType { get; private set; }
		public bool IsSuccessStatusCode { get; private set; }

		public void SetSuccessFlag(bool flag) =>
			this.IsSuccessStatusCode = flag;
	}
}
