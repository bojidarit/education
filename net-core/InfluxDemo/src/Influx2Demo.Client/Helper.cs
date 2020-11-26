namespace Influx2Demo.Client
{
	using Influx2Demo.Logic;
	using Influx2Demo.Logic.DataStructures.Enumerations;
	using System.Data;

	public static class Helper
	{

		#region Influx Related Methods

		public static InfluxApi CreateInfluxApi(InfluxDbType dbType)
		{
			var api = (dbType == InfluxDbType.OSS)
				? InfluxApi.Create(ConfData.OssUrl, ConfData.OssFullAccessToken, ConfData.OssOrganizationId, dbType)
				: InfluxApi.Create(ConfData.CloudUrl, ConfData.CloudFullAccessToken, ConfData.CloudOrganizationId, dbType);

			return api;
		}

		public static void UpgradeInfluxCsvTable(DataTable dataTable)
		{
			dataTable.Columns.RemoveAt(0);
			dataTable.Columns.RemoveAt(0);

			var indexColumn = dataTable.Columns.Add("#", typeof(long));
			indexColumn.SetOrdinal(0);
			long index = 1L;
			foreach (var row in dataTable.AsEnumerable())
			{
				row[indexColumn.Ordinal] = index++;
			}
		}

		#endregion
	}
}
