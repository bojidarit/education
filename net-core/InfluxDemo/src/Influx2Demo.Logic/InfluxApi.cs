namespace Influx2Demo.Logic
{
	using Influx2Demo.Logic.DataStructures;
	using InfluxDB.Client;
	using InfluxDB.Client.Api.Domain;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;

	// InfluxDB Client API
	// The reference C# client that allows query, write and InfluxDB 2.0 management.
	public class InfluxApi
	{
		#region Constants

		private const string autoRetentionPolicy = "autogen";
		private const string DefaultNegativeDuration = "-5y";
		public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";


		// Schema flux commands
		private static readonly string SchemaBuckets =
			$"buckets() |> keep(columns: [{"name".Quote()}, {"id".Quote()}, {"retentionPeriod".Quote()}])";
		private static readonly string SchemaImport =
			$"import {"influxdata/influxdb/schema".Quote()}";
		private const string SchemaMeasurements =
			"schema.measurements(bucket: {0})";

		// xKeys functions searches within "last 30 days" ONLY!!!
		// "schema.measurementFieldKeys(bucket: {0}, measurement: {1})";
		// "schema.measurementTagKeys(bucket: {0}, measurement: {1})";

		private const string SchemaFieldKeys =
			"schema.tagKeys(bucket: {0}, predicate: (r) => r._measurement == {1}, start: {2})";
		private const string SchemaTagKeys =
			"schema.fieldKeys(bucket: {0}, predicate: (r) => r._measurement == {1}, start: {2})";

		#endregion


		#region Constructors

		public InfluxApi()
		{
		}

		public InfluxApi(string url, char[] token, string orgId)
		{
			if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(orgId))
			{
				throw new ArgumentException();
			}

			Url = url;
			Token = token;
			OrganizationId = orgId;
		}

		#endregion


		#region Properties

		public string Url { get; set; }

		public char[] Token { get; set; }

		public string OrganizationId { get; private set; }

		#endregion


		#region General Methods

		// Get the health of an instance anytime during execution
		public async Task<HealthCheck> GetHealthAsync()
		{
			using (var client = CreateInfluxClient())
			{
				// Log HTTP Request and Response
				//client.SetLogLevel(LogLevel.Headers);

				Debug.WriteLine($"==> Getting {typeof(HealthCheck).FullName}...");
				return await client.HealthAsync();
			}
		}

		// The readiness of the InfluxDB 2.0.
		public async Task<Ready> GetReadyAsync()
		{
			using (var client = CreateInfluxClient())
			{
				Debug.WriteLine($"==> Getting {typeof(Ready).FullName}...");
				return await client.ReadyAsync();
			}
		}

		public async Task<List<T>> FluxQueryAsync<T>(string flux)
		{
			using (var influxDBClient = InfluxDBClientFactory.Create(Url, Token))
			{
				Debug.WriteLine($"==> Executing flux query{Environment.NewLine}{flux}");
				var result = await influxDBClient.GetQueryApi().QueryAsync<T>(flux, OrganizationId);
				return result;
			}
		}

		public async Task<string> FluxQueryRawAsync(string flux)
		{
			using (var influxDBClient = InfluxDBClientFactory.Create(Url, Token))
			{
				Debug.WriteLine($"==> Executing flux query{Environment.NewLine}{flux}");
				var result = await influxDBClient.GetQueryApi().QueryRawAsync(flux, OrganizationId);
				return result;
			}
		}

		public async Task<DataTable> FluxQueryToDataTableAsync(string flux)
		{
			var csv = await FluxQueryRawAsync(flux);
			var dataTable = DataParser.MakeDataTableFromCsv(csv);
			return dataTable;
		}

		#endregion


		#region Schema Methods

		public async Task<List<BucketDto>> GetBuckets(bool showSystem = false)
		{
			var all = await FluxQueryAsync<BucketDto>(SchemaBuckets);
			return showSystem ? all : all.Where(b => !b.IsSystem()).ToList();
		}

		public async Task<List<string>> GetMeasurements(string bucket)
		{
			var flux = $"{SchemaImport}{Environment.NewLine}" +
				$"{string.Format(SchemaMeasurements, bucket.Quote())}";
			var all = await FluxQueryAsync<ValueDto>(flux);

			return all.Select(i => i.Value).ToList();
		}

		public async Task<List<string>> GetTagKeys(string bucket, string measurement)
		{
			var flux = $"{SchemaImport}{Environment.NewLine}" +
				$"{string.Format(SchemaTagKeys, bucket.Quote(), measurement.Quote(), DefaultNegativeDuration)}";
			var all = await FluxQueryAsync<ValueDto>(flux);

			return GetKeysRealValues(all);
		}

		public async Task<List<string>> GetFieldKeys(string bucket, string measurement)
		{
			var flux = $"{SchemaImport}{Environment.NewLine}" +
				$"{string.Format(SchemaFieldKeys, bucket.Quote(), measurement.Quote(), DefaultNegativeDuration)}";
			var all = await FluxQueryAsync<ValueDto>(flux);

			return GetKeysRealValues(all);
		}

		#endregion


		#region Flux Query Generation

		public string GetSampleDataFlux(string bucket, string measurement, int limit)
		{
			var list = new List<string>();
			list.Add($"{SchemaImport}{Environment.NewLine}");
			list.Add($"from(bucket: {bucket.Quote()})");
			list.Add($"  |> range(start: {DefaultNegativeDuration})");
			list.Add($"  |> filter(fn:(r) => r._measurement == {measurement.Quote()})");
			list.Add($"  |> schema.fieldsAsCols()");
			list.Add($"  |> drop(columns: [{"_start".Quote()}, {"_stop".Quote()}, {"_measurement".Quote()}])");
			list.Add($"  |> limit(n: {limit})");

			return string.Join("", list);
		}

		#endregion


		#region Helpers

		private InfluxDBClient CreateInfluxClient()
		{
			// Create a instance of the InfluxDB 2.0 client.
			var client = InfluxDBClientFactory.Create(Url, Token);
			return client;
		}

		private List<string> GetKeysRealValues(List<ValueDto> values) =>
			values.Where(i => !i.Value.StartsWith("_")).Select(i => i.Value).ToList();

		#endregion
	}
}
