namespace Vidly.ViewModels
{
	using Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	public class ManageMovieViewModel
	{
		public ManageMovieViewModel(IEnumerable<Genre> genres, Movie movie, string title)
		{
			this.Movie = movie;
			this.Title = title;

			this.GenresList = genres.Select(i => new SelectListItem()
			{
				Text = i.Name,
				Value = i.Id.ToString(),
				Selected = false
			});
		}

		public IEnumerable<SelectListItem> GenresList { get; private set; }

		public Movie Movie { get; private set; }

		public string Title { get; private set; }
	}
}