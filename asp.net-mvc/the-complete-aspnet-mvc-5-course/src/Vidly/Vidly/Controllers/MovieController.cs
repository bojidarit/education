namespace Vidly.Controllers
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Validation;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using Vidly.Models;
	using Vidly.ViewModels;

	public class MovieController : Controller
	{
		private ApplicationDbContext _context;

		public MovieController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: Movie/Index
		public ActionResult Index()
		{
			var movies = _context.Movies.Include(m => m.Genre);
			return View(movies);
		}

		// GET: Movie/New
		public ActionResult New()
		{
			var viewModel = new ManageMovieViewModel(_context.Genres, null);
			return View("ManageMovie", viewModel);
		}

		// GET: Movie/Edit/{id}
		public ActionResult Edit(int id)
		{
			Movie movie = _context.Movies.SingleOrDefault(m => m.Id == id);

			if (movie == null)
			{
				return HttpNotFound($"There is no " +
					$"{this.GetType().Name.Replace("Controller", "")} with Id = {id}");
			}

			ManageMovieViewModel viewModel = new ManageMovieViewModel(_context.Genres, movie);

			return View("ManageMovie", viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(ManageMovieViewModel movieVeiwModel)
		{
			Movie model = movieVeiwModel.GetPopulatedMovie();

			if (!ModelState.IsValid)
			{
				var viewModel = new ManageMovieViewModel(_context.Genres, model);

				return View("ManageMovie", viewModel);
			}

			if (model.Id > 0)
			{
				// Existing Record
				Movie movieFromDb = _context.Movies.SingleOrDefault(m => m.Id == model.Id);
				if (movieFromDb != null)
				{
					movieFromDb.Name = model.Name;
					movieFromDb.ReleaseDate = model.ReleaseDate;
					movieFromDb.GenreId = model.GenreId;
					movieFromDb.NumberInStock = model.NumberInStock;
				}
			}
			else
			{
				// New Record
				_context.Movies.Add(model);
			}

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbEntityValidationException e)
			{
				return Content(GetValidationExceptionInfo(e));
			}

			return RedirectToAction("Index", "Movie");
		}

		#region Helpers

		private string GetValidationExceptionInfo(DbEntityValidationException e)
		{
			StringBuilder sb = new StringBuilder();

			if (e != null)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					sb.Append($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
					foreach (var ve in eve.ValidationErrors)
					{
						sb.Append($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
					}
				}
			}

			return sb.ToString();
		}

		#endregion //Helpers

		#region Demo Actions

		// GET: Movie/Details/{id}
		public ActionResult Details(int id)
		{
			Movie movie = _context.Movies
				.Include(m => m.Genre)
				.SingleOrDefault(m => m.Id == id);

			if (movie != null)
			{
				return View(movie);
			}
			else
			{
				return HttpNotFound($"There is no movie with Id = {id}");
			}
		}

		// GET: Movie/PagedIndex
		public ActionResult PagedIndex(int? pageIndex, string sortBy)
		{
			if (!pageIndex.HasValue)
			{
				pageIndex = 1;
			}

			if (string.IsNullOrWhiteSpace(sortBy))
			{
				sortBy = "Name";
			}

			return Content($"pageIndex = {pageIndex}, sortBy = '{sortBy}'");
		}

		// GET: Movie/Random
		public ActionResult Random()
		{
			RandomMovieViewModel viewModel = new RandomMovieViewModel()
			{
				Movie = new Movie() { Id = 777, Name = "Random movie..." },
				Customers = new List<Customer>() { new Customer(1, "Steve"), new Customer(3, "John") }
			};

			return View(viewModel);

			//return Content("Hello MVC...");
			//return HttpNotFound();
			//return new EmptyResult();
			//return RedirectToAction("index", "home", new { page = 1, sortBy = "name" });
		}

		// GET: Movie/Released/{year}/{month}
		[Route("movie/released/{year:regex(\\d{4})}/{month:range(1,12)}")]
		public ActionResult ByReleaseDate(int year, int month)
		{
			return Content($"ByReleaseDate action with parameter year = {year} and " +
				$"month = {month} from {this.GetType().Name}");
		}

		#endregion //Demo Actions
	}
}