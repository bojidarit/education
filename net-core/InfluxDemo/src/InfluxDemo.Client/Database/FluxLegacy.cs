namespace InfluxDemo.Client.Database
{
	using InfluxDB.Client.Flux;
	using System.Threading.Tasks;

	// Using Client.Legacy
	// C# client that allows you to perform Flux queries against InfluxDB 1.7+.
	// The reference C# library for the InfluxDB 1.7+ /api/v2/query REST API using the Flux language.
	public static class FluxLegacy
	{
		#region Public Methods

		public static async Task<string> PingAsync()
		{
			var client = CreateFluxClient();

			if(client == null)
			{
				return null;
			}

			var ping = await client.PingAsync();
			var version = await client.VersionAsync();

			var result = $"{{\"ping\": {ping.ToString().ToLower()}, \"version\": \"{version}\"}}";
			return result;
		}

		#endregion


		#region Helpers

		private static FluxClient CreateFluxClient()
		{
			var options = new FluxConnectionOptions(ConfData.OssUrl);

			var fluxClient = FluxClientFactory.Create(options);

			return fluxClient;
		}

		#endregion
	}
}
