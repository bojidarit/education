namespace InfluxDemo.Client
{
	using System.Configuration;

	public static class ConfData
	{
		public static string OssUrl =>
			ConfigurationManager.AppSettings[nameof(OssUrl)];

		public static string OssUsername =>
			ConfigurationManager.AppSettings[nameof(OssUsername)];

		public static char[] OssPassword =>
			ConfigurationManager.AppSettings[nameof(OssPassword)].ToCharArray();

		public static char[] OssToken =>
			ConfigurationManager.AppSettings[nameof(OssToken)]?.ToCharArray();

		public static string CloudUrl =>
			ConfigurationManager.AppSettings[nameof(CloudUrl)];

		public static char[] CloudToken =>
			ConfigurationManager.AppSettings[nameof(CloudToken)]?.ToCharArray();

		public static char[] CloudReadWriteToken =>
			ConfigurationManager.AppSettings[nameof(CloudReadWriteToken)]?.ToCharArray();

		public static string CloudDefBucketId =>
			ConfigurationManager.AppSettings[nameof(CloudDefBucketId)];

		public static string CloudOrganizationId =>
			ConfigurationManager.AppSettings[nameof(CloudOrganizationId)];
	}
}
