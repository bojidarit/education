namespace InfluxDemo.Client.Database
{
	using InfluxDB.Client;
	using InfluxDB.Client.Api.Domain;
	using System.Threading.Tasks;

	// Using: Client
	// The reference C# client that allows query, write and InfluxDB 2.0 management.
	// Plus InfluxDB 1.8 forward compatibility client for API v.2
	public static class Influx
	{
		private const string autoRetentionPolicy = "autogen";

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
		public static async Task<Ready> GetReadyOssAsync()
		{
			using (var client = CreateInfluxClient(false))
			{
				return await client.ReadyAsync();
			}
		}

		#endregion


		#region Helpers

		// Mainly for Influx (Cloud) v2.0
		private static InfluxDBClient CreateInfluxClient(bool useCloud = false)
		{
			if (useCloud)
			{
				// Create a instance of the InfluxDB 2.0 client.
				var cloudClient = InfluxDBClientFactory.Create(ConfData.CloudUrl, ConfData.CloudToken);
				return cloudClient;
			}

			// For flux query and.or line protocol write - APIs v.2
			var ossClient = InfluxDBClientFactory.Create(ConfData.OssUrl, ConfData.OssToken);
			return ossClient;
		}

		// For InfluxDB 1.8 (OSS)
		// User for writing and reading (forward compatibility client for API v.2)
		private static InfluxDBClient CreateInfluxV1Client(string database)
		{
			// Create a instance of the InfluxDB 2.0 client to connect into InfluxDB 1.8.
			var client = InfluxDBClientFactory.CreateV1(
				ConfData.OssUrl,
				ConfData.OssUsername,
				ConfData.OssPassword,
				database,
				autoRetentionPolicy);

			return client;
		}

		#endregion
	}
}
