namespace Vidly.ViewModels
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Linq;
	using System.Web.Mvc;

	public class ManageMovieViewModel
	{
		public ManageMovieViewModel()
			: this(null, null)
		{

		}

		public ManageMovieViewModel(IEnumerable<Genre> genres, Movie movie)
		{
			this.Movie = movie ?? Movie.CreateMovie();
			this.ReleaseDateNullable = movie?.ReleaseDate;
			this.NumberInStockNullable = movie?.NumberInStock;

			this.Title = $"{(this.Movie.Id > 0 ? "Edit" : "New")} Movie";

			if (genres != null)
			{
				this.GenresList = genres.Select(i => new SelectListItem()
				{
					Text = i.Name,
					Value = i.Id.ToString(),
					Selected = false
				});
			}
		}

		public IEnumerable<SelectListItem> GenresList { get; private set; }

		/// <summary>
		/// The source model
		/// </summary>
		public Movie Movie { get; private set; }

		/// <summary>
		/// View title - depends on action
		/// </summary>
		public string Title { get; private set; }

		[Required]
		[Display(Name = "Release Date")]
		public DateTime? ReleaseDateNullable { get; set; }

		[Required]
		[Display(Name = "Number In Stock")]
		[Range(1, 20)]
		public int? NumberInStockNullable { get; set; }

		public Movie GetPopulatedMovie()
		{
			if (this.ReleaseDateNullable.HasValue)
			{
				this.Movie.ReleaseDate = this.ReleaseDateNullable.Value;
			}

			if (this.NumberInStockNullable.HasValue)
			{
				this.Movie.NumberInStock = this.NumberInStockNullable.Value;
			}

			return this.Movie;
		}
	}
}