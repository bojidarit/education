using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using System.Threading.Tasks;

namespace Influx2Demo.Logic
{
	// InfluxDB Client API
	// The reference C# client that allows query, write and InfluxDB 2.0 management.
	public class InfluxApi
	{
		#region Constructors

		public InfluxApi()
		{
		}

		public InfluxApi(string url, char[] token)
		{
			Url = url;
			Token = token;
		}

		#endregion


		#region Properties

		public string Url { get; set; }

		public char[] Token { get; set; }

		#endregion


		#region Public Methods

		// Get the health of an instance anytime during execution
		public async Task<HealthCheck> GetHealthAsync()
		{
			using (var client = CreateInfluxClient())
			{
				// Log HTTP Request and Response
				//client.SetLogLevel(LogLevel.Headers);

				return await client.HealthAsync();
			}
		}

		// The readiness of the InfluxDB 2.0.
		public async Task<Ready> GetReadyAsync()
		{
			using (var client = CreateInfluxClient())
			{
				return await client.ReadyAsync();
			}
		}

		#endregion


		#region Helpers

		private InfluxDBClient CreateInfluxClient()
		{
			// Create a instance of the InfluxDB 2.0 client.
			var client = InfluxDBClientFactory.Create(Url, Token);
			return client;
		}

		#endregion
	}
}
