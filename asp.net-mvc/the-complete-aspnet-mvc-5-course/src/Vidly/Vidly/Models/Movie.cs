namespace Vidly.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Movie
	{
		#region Constructors

		public Movie()
		{
			this.DateAdded = DateTime.Today;
		}

		public Movie(int id, string name)
			: this()
		{
			this.Id = id;
			this.Name = name;
		}

		#endregion //Constructors

		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		public Genre Genre { get; set; }

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

		public override string ToString() =>
			$"{this.GetType().Name} {{ Name = '{this.Name}', " +
			$"ReleaseDate = {this.ReleaseDate.ToShortDateString()}, " +
			$"GenreId = {this.GenreId}, " +
			$"NumberInStock = {this.NumberInStock} }}";
	}
}