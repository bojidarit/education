namespace InfluxDemo.Client.Database
{
	using InfluxDB.Client;
	using InfluxDB.Client.Api.Domain;
	using System.Threading.Tasks;

	public static class Influx
	{
		public static async Task<HealthCheck> GetHealth(bool useCloud)
		{
			using (var client = CreateInfluxClient(useCloud))
			{
				return await client.HealthAsync();
			}
		}

		public static async Task<Ready> GetReady(bool useCloud)
		{
			using (var client = CreateInfluxClient(useCloud))
			{
				return await client.ReadyAsync();
			}
		}

		private static InfluxDBClient CreateInfluxClient(bool useCloud = false)
		{
			if (useCloud)
			{
				var cloudClient = InfluxDBClientFactory.Create(ConfData.CloudUrl, ConfData.CloudToken);
				return cloudClient;
			}

			var ossClient = InfluxDBClientFactory.Create(ConfData.OssUrl, ConfData.OssToken);
			return ossClient;
		}
	}
}
