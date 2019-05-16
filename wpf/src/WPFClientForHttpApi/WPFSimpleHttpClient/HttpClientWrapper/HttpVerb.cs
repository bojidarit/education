namespace WPFSimpleHttpClient.HttpClientWrapper
{
	public enum HttpVerb
	{
		None = 0,
		Get = 1,
		Post = 2,
		Put = 4,
		Delete = 8,
		Head = 16,
		Patch = 32,
		Options = 64
	}
}
