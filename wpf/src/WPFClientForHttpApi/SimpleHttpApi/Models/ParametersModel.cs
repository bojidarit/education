namespace SimpleHttpApi.Models
{
	using System.ComponentModel.DataAnnotations;

	public class ParametersModel
	{
		public static string XmlFormat = "XML";
		public static string JsonFormat = "JSON";

		[Required]
		public string Method { get; set; }

		[Required]
		public string ApiKey { get; set; }

		public string[] Params { get; set; }

		public string Format { get; set; }
	}
}