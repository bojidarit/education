namespace ProductsApi.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Category
	{
		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }
	}
}