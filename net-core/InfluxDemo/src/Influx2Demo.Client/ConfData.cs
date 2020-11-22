namespace Influx2Demo.Client
{
	using System.Configuration;
	using System.Reflection;

	public static class ConfData
	{
		#region Properties

		public static string OssUrl =>
			GetAppConfigValue(MethodBase.GetCurrentMethod().Name);

		public static char[] OssFullAccessToken =>
			GetAppConfigValue(MethodBase.GetCurrentMethod().Name).ToCharArray();

		public static string CloudUrl =>
			GetAppConfigValue(MethodBase.GetCurrentMethod().Name);

		public static char[] CloudFullAccessToken =>
			GetAppConfigValue(MethodBase.GetCurrentMethod().Name).ToCharArray();

		#endregion


		#region Helpers

		public static string GetAppConfigValue(string getName)
		{
			var name = getName.Replace("get_", string.Empty);
			var result = ConfigurationManager.AppSettings[name];

			return result;
		}

		#endregion
	}
}
