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

		// GET: /api/movies/{id}
		public IHttpActionResult GetMovie(int id)
		{
			Movie movie = _context.Movies.SingleOrDefault(m => m.Id == id);

			if(movie == null)
			{
				return NotFound();
			}

			var movieDto = Mapper.Map<Movie, MovieDto>(movie);

			return Ok(movieDto.GenerateLinks<MovieDto>(base.Request.RequestUri));
		}

		// POST: /api/movies
		[HttpPost]
		public IHttpActionResult CreateMovie(MovieDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var movie = Mapper.Map<MovieDto, Movie>(dto);

			_context.Movies.Add(movie);
			_context.SaveChanges();

			dto.Id = movie.Id;
			dto.GenerateLinks<MovieDto>(base.Request.RequestUri, dto.Id.ToString());

			return Created(dto.Href, dto);
		}
	}
}
