namespace WPFSimpleHttpClient.Extensions
{
	using System.Threading.Tasks;
	using WebClientWrapper;

	public static class WebApiClientExtension
	{
		public static async Task<string> PostRawDataAsync(this WebApiClient client,
			string library, string method, object[] values)
		{
			dynamic parameters = Common.MakeExpandoBody(library, method, values);

			return await client.PostAsync(Common.ApiPath, parameters);
		}
	}
}
