namespace WPFSimpleHttpClient.HttpClientWrapper
{
	//using System.Net.Http;

	public class HttpDataExpando : HttpBaseData
	{
		public HttpDataExpando(HttpBaseData data, dynamic[] content)
			: base(data)
		{
			this.Content = content;
		}

		public dynamic[] Content { get; private set; }
	}
}
