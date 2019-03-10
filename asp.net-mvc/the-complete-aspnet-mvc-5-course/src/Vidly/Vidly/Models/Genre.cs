namespace Vidly.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Genre
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
	}
}