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
			IEnumerable<MovieDto> movies = 
				_context.Movies.ToList().Select(Mapper.Map<Movie, MovieDto>);

			return Ok(movies);
		}
	}
}
