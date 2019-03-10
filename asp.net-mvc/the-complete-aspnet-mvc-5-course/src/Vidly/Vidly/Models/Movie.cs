namespace Vidly.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Movie
	{
		#region Ctros

		public Movie()
		{

		}

		public Movie(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		#endregion //Ctros

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public Genre Genre { get; set; }

		[Required]
		public DateTime ReleaseDate { get; set; }

		[Required]
		public DateTime DateAdded { get; set; }

		[Required]
		public int NumberInStock { get; set; }

		public int GenreId { get; set; }
	}
}