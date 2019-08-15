namespace WPFSimpleHttpClient.Extensions
{
	using System.Threading.Tasks;
	using WebClientWrapper;

	public static class WebApiClientExtension
	{
		public static async Task<string> PostRawDataAsync(this WebApiClient client,
			string library, string method, object[] values)
		{
			string address = Common.MakeRequestAddress(client.BaseUri, Common.ApiPath);

			dynamic parameters = Common.MakeExpandoBody(library, method, values);

			return await client.PostAsync(address, parameters);
		}
	}
}
