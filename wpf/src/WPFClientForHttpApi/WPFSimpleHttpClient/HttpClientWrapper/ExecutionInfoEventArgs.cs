namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;

	public class ExecutionInfoEventArgs : EventArgs
	{
		public ExecutionInfoEventArgs(Uri uri, HttpVerb verb)
		{
			this.RequestUri = uri;
			this.RequestVerb = verb;
		}

		public ExecutionInfoEventArgs(Uri uri, HttpVerb verb, object body)
			: this(uri, verb)
		{
			this.Body = body;
		}

		#region Properties

		public Uri RequestUri { get; private set; }

		public HttpVerb RequestVerb { get; private set; }

		public object Body { get; private set; }

		#endregion //Properties
	}
}
