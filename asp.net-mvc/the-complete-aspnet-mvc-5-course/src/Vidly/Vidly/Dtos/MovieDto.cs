namespace Vidly.Dtos
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class MovieDto : HateoasDtoBase
	{
		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[Display(Name = "Release Date")]
		public DateTime ReleaseDate { get; set; }

		[Editable(false)]
		[Required]
		[Display(Name = "Date Added")]
		public DateTime DateAdded { get; set; }

		[Required]
		[Display(Name = "Number In Stock")]
		public int NumberInStock { get; set; }

		[Required]
		[Display(Name = "Gender")]
		public int GenreId { get; set; }
	}
}