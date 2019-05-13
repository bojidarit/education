namespace SimpleHttpApi.Models
{
	using System.ComponentModel.DataAnnotations;

	public class ParametersModel
	{
		[Required]
		public string ApiKey { get; set; }

		public string[] Params { get; set; }
	}
}