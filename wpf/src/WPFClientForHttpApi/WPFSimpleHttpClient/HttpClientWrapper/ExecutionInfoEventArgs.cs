namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;

	public class ExecutionInfoEventArgs : EventArgs
	{
		public ExecutionInfoEventArgs(Uri uri)
		{
			this.RequestUri = uri;
		}

		#region Properties

		public Uri RequestUri { get; private set; }

		#endregion //Properties
	}
}
