namespace MVCWebAppNginxIPReal
{
	using System.Text.Json;

	public static class Helpers
	{
		public static string ToJsonString<T>(T obj)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,
				IgnoreReadOnlyProperties = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			var json = JsonSerializer.Serialize(obj, options);

			return json;
		}
	}
}
