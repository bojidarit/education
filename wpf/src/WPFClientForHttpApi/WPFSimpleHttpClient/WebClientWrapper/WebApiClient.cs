namespace WPFSimpleHttpClient.WebClientWrapper
{
	using System;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	public class WebApiClient : WebClient
	{
		private int _timeoutSeconds = 100;	// Default for this class

		public WebApiClient(string baseAddress = null, string contentType = Common.JsonContentType, Encoding encoding = null, int timeoutSeconds = 100) : base()
		{
			this.BaseAddress = baseAddress;
			_timeoutSeconds = timeoutSeconds;
			this.Headers[HttpRequestHeader.ContentType] = 
				string.IsNullOrWhiteSpace(contentType) ? Common.JsonContentType : contentType;
			this.Encoding = encoding ?? Encoding.UTF8;
		}

		#region Properties

		public string LastRequestAddress { get; private set; }

		public string LastRequestBody { get; private set; }

		public Exception LastException { get; private set; }

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
				result = await UploadStringTaskAsync(requestAddress, body);
			}
			catch (Exception ex)
			{
				this.LastException = ex;

				string line = new string('-', 80);

				System.Diagnostics.Debug.WriteLine($"{line}{Environment.NewLine}WebApiClient.PostAsync<T>(string requestAddress, T value)" +
					$"{Environment.NewLine}Request Address: {requestAddress}" +
					$"{Environment.NewLine}Body: {body}" +
					$"{Environment.NewLine}{ex.GetType().Name}: {ex.Message}{Environment.NewLine}{line}");
			}

			return result;
		}

		#endregion //Methods
	}
}
