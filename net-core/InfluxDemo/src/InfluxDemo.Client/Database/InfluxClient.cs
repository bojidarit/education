namespace InfluxDemo.Client.Database
{
	using InfluxDB.Client;
	using InfluxDB.Client.Api.Domain;
	using System.Configuration;
	using System.Threading.Tasks;

	public static class InfluxClient
	{
		public static async Task<HealthCheck> GetHealth()
		{
			var conf = InfluxConf.Create();
			using (var client = InfluxDBClientFactory.Create(conf.Url, conf.Token))
			{
				var result = await client.HealthAsync();
				return result;
			}
		}

		private class InfluxConf
		{
			public string Url { get; set; }

			public char[] Token { get; set; }

			public static InfluxConf Create()
			{
				var conf = new InfluxConf();
				conf.Url = ConfigurationManager.AppSettings["url"];
				conf.Token = ConfigurationManager.AppSettings["token"].ToCharArray();

				return conf;
			}
		}
	}
}
