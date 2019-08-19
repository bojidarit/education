namespace WPFSimpleHttpClient.WebClientWrapper
{
	using System;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	public class WebApiClient : WebClient
	{
		private int _timeoutSeconds = 100;  // Default for this class

		public WebApiClient(string baseAddress = null, string contentType = Common.JsonContentType, Encoding encoding = null, int timeoutSeconds = 100) : base()
		{
			this.BaseAddress = baseAddress;
			_timeoutSeconds = timeoutSeconds;
			this.Headers[HttpRequestHeader.ContentType] =
				string.IsNullOrWhiteSpace(contentType) ? Common.JsonContentType : contentType;
			this.Encoding = encoding ?? Encoding.UTF8;
		}

		#region Properties, 

		public string LastRequestAddress { get; private set; }

		public string LastRequestBody { get; private set; }

		public Exception LastException { get; private set; }

		public string LastExceptionDetails { get; private set; }

		#endregion //Properties

		#region Methods

		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);

			// Time, in milliseconds, before the request times out.
			webRequest.Timeout = _timeoutSeconds * 1000;

			return webRequest;
		}

		public async Task<string> PostAsync<T>(string requestAddress, T value)
		{
			string result = null;
			string body = string.Empty;

			try
			{
				body = Common.PrepareJsonBody(value);
				this.LastRequestAddress = string.IsNullOrEmpty(this.BaseAddress) ? requestAddress : Common.MakeRequestAddress(this.BaseAddress, requestAddress);
				this.LastRequestBody = body;
				result = await UploadStringTaskAsync(requestAddress, Common.HttpVerbPost, body);
			}
			catch (WebException webException)
			{
				HttpWebResponse httpWebResponse = webException.Response as HttpWebResponse;
				this.LastExceptionDetails = (httpWebResponse == null) ? webException.Status.ToString() :
					$"{webException.Status.ToString()}: ({(int)httpWebResponse.StatusCode}) '{httpWebResponse.StatusDescription}' " +
					$"from ({httpWebResponse.Method}) {httpWebResponse.ResponseUri}";
				HandleException(webException, body);
			}
			catch (Exception ex)
			{
				HandleException(ex, body);
			}

			return result;
		}

		private void HandleException(Exception ex, string body)
		{
			this.LastException = ex;

			string line = new string('-', 80);

			System.Diagnostics.Debug.WriteLine($"{line}{Environment.NewLine}WebApiClient.PostAsync<T>(string requestAddress, T value)" +
				$"{Environment.NewLine}Request Address: {this.LastRequestAddress}" +
				$"{Environment.NewLine}Body: {body}" +
				$"{Environment.NewLine}Exception Details: {this.LastExceptionDetails}" +
				$"{Environment.NewLine}{ex.GetType().Name}: {ex.Message}{Environment.NewLine}{line}");
		}

		#endregion //Methods
	}
}
