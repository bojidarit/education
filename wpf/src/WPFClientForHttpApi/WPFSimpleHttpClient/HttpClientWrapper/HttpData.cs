namespace WPFSimpleHttpClient.HttpClientWrapper
{
	using System.Net.Http;

	public class HttpData<T> : HttpBaseData
	{
		public HttpData(HttpResponseMessage response, T content)
			: base(response)
		{
			this.Content = content;
		}

		public HttpData(HttpData<string> data, T content)
			: base(data)
		{
			this.Content = content;
		}

		public T Content { get; private set; }
	}
}
