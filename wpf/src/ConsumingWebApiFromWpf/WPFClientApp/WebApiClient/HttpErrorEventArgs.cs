namespace WPFClientApp.WebApiClient
{
	using System;
	using System.Text;

	public class HttpErrorEventArgs : EventArgs
	{
		public HttpErrorEventArgs(Exception exception, string requestUri)
		{
			this.Exception = exception;
			this.RequestUri = requestUri;
		}

		#region Properties

		public string RequestUri { get; set; }

		public Exception Exception { get; set; }

		public string HierarchyExceptionMessages =>
			GetFullHttpException();

		public string ExceptionType =>
			this.Exception?.GetType().Name;

		#endregion //Properties

		#region Methods

		private void GetInnerExceptions(Exception exception, StringBuilder stringBuilder)
		{
			stringBuilder.Append($"{exception.Message} {Environment.NewLine}");
			if (exception.InnerException != null)
			{
				GetInnerExceptions(exception.InnerException, stringBuilder);
			}
		}

		private string GetFullHttpException()
		{
			StringBuilder stringBuilder = new StringBuilder($"Request Uri: {this.RequestUri} {Environment.NewLine}");
			GetInnerExceptions(this.Exception, stringBuilder);
			return stringBuilder.ToString();
		}

		#endregion //Methods
	}
}
