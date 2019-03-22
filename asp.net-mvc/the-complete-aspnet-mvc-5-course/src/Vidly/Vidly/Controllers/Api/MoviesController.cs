namespace Vidly.Controllers.Api
{
	using System.Linq;
	using System.Web.Http;
	using Models;
	using Dtos;
	using AutoMapper;
	using System.Collections.Generic;

	public class MoviesController : ApiController
    {
		private ApplicationDbContext _context;

		public MoviesController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: /api/movies
		public IHttpActionResult GetMovies()
		{
			IEnumerable<MovieDto> mappedMovies = _context.Movies.ToList().Select(Mapper.Map<Movie, MovieDto>);

			var movies = mappedMovies.Select(m => m.GenerateLinks<MovieDto>(base.Request.RequestUri, m.Id.ToString()));

			return Ok(movies);
		}
	}
}
