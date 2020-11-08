namespace JwtLib
{
	using Newtonsoft.Json;

	public static class Helper
	{
		public static readonly string ClaimValueTypesUsername = "username";
		public static readonly string ClaimValueTypesExpiration = "exp";

		public static string BeautifyJson(string text)
		{
			var json = JsonConvert.DeserializeObject(text);
			var result = JsonConvert.SerializeObject(json, Formatting.Indented);
			return result;
		}
	}
}
