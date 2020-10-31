namespace InfluxDemo.Client.Database
{
	using InfluxDB.Client;
	using InfluxDB.Client.Api.Domain;
	using System.Threading.Tasks;

	// Using: Client
	// The reference C# client that allows query, write and InfluxDB 2.0 management.
	public static class Influx
	{
		#region Public Methods

		// Get the health of an instance anytime during execution
		public static async Task<HealthCheck> GetHealthAsync(bool useCloud)
		{
			using (var client = CreateInfluxClient(useCloud))
			{
				// Log HTTP Request and Response
				//client.SetLogLevel(LogLevel.Headers);

				return await client.HealthAsync();
			}
		}

		// Get the readiness of a instance at startup
		public static async Task<Ready> GetReadyAsync(bool useCloud)
		{
			using (var client = CreateInfluxClient(useCloud))
			{
				return await client.ReadyAsync();
			}
		}

		#endregion


		#region Helpers

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

		#endregion
	}
}
