namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Net.Http.Headers;

	public class ExecutionInfoEventArgs : EventArgs
	{
		public ExecutionInfoEventArgs(Uri uri, HttpVerb verb)
		{
			this.RequestUri = uri;
			this.RequestVerb = verb;
		}

		public ExecutionInfoEventArgs(Uri uri, HttpVerb verb, object body, HttpRequestHeaders requestHeaders, HttpContentHeaders contentHeaders)
			: this(uri, verb)
		{
			this.Body = body;
			this.ContentHeaders = contentHeaders;
			this.RequestHeaders = requestHeaders;
		}

		#region Properties

		public Uri RequestUri { get; private set; }

		public HttpVerb RequestVerb { get; private set; }

		public object Body { get; private set; }

		public HttpContentHeaders ContentHeaders { get; private set; }

		public HttpRequestHeaders RequestHeaders { get; private set; }

		#endregion //Properties
	}
}
