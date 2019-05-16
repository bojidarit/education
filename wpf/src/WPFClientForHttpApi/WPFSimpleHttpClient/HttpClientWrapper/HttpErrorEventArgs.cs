namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System;
	using System.Text;

	public class HttpErrorEventArgs : EventArgs
	{
		public HttpErrorEventArgs(Exception exception, string requestUri, HttpVerb verb = HttpVerb.None, string additionalInfo = null)
		{
			this.Exception = exception;
			this.RequestUri = requestUri;
			this.RequestVerb = verb;
			this.AdditionalInfo = additionalInfo ?? string.Empty;
		}

		#region Properties

		public string RequestUri { get; private set; }

		public HttpVerb RequestVerb { get; private set; }

		public Exception Exception { get; private set; }

		public string AdditionalInfo { get; private set; }

		public string HierarchyExceptionMessages =>
			GetFullHttpException();

		public string ExceptionType =>
			this.Exception?.GetType().Name;

		#endregion //Properties

		#region Methods

		private void GetInnerExceptions(Exception exception, StringBuilder stringBuilder)
		{
			stringBuilder.Append($"{Environment.NewLine} {exception.Message} {Environment.NewLine}");
			if (exception.InnerException != null)
			{
				stringBuilder.Append(Environment.NewLine);
				GetInnerExceptions(exception.InnerException, stringBuilder);
			}

			if (!string.IsNullOrWhiteSpace(this.AdditionalInfo))
			{
				stringBuilder.Append($"{Environment.NewLine}{this.AdditionalInfo}");
			}
		}

		private string GetFullHttpException()
		{
			StringBuilder stringBuilder = new StringBuilder($"Request Uri: {this.RequestUri} {Environment.NewLine}" +
				$"Request Type: {this.RequestVerb.ToString().ToUpper()} {Environment.NewLine}");
			GetInnerExceptions(this.Exception, stringBuilder);
			return stringBuilder.ToString();
		}

		#endregion //Methods
	}
}
