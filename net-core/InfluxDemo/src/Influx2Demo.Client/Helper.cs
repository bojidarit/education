namespace Influx2Demo.Client
{
	using Influx2Demo.Logic;
	using Influx2Demo.Logic.DataStructures.Enumerations;

	public static class Helper
	{

		#region Influx Related Methods

		public static InfluxApi CreateInfluxApi(InfluxDbType dbType)
		{
			var api = (dbType == InfluxDbType.OSS)
				? new InfluxApi(ConfData.OssUrl, ConfData.OssFullAccessToken, ConfData.OssOrganizationId)
				: new InfluxApi(ConfData.CloudUrl, ConfData.CloudFullAccessToken, ConfData.CloudOrganizationId);

			return api;
		}

		#endregion
	}
}
