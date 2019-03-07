namespace Vidly.Controllers
{
	using System;
	using System.Web.Mvc;
	using Vidly.Models;

	public class MovieController : Controller
	{
		// GET: Movie/Index
		public ActionResult Index(int? pageIndex, string sortBy)
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
			Movie movie = new Movie() { Id = 777, Name = "Random movie..." };
			return View(movie);

			//return Content("Hello MVC...");
			//return HttpNotFound();
			//return new EmptyResult();
			//return RedirectToAction("index", "home", new { page = 1, sortBy = "name" });
		}

		// GET: Movie/Edit/{id}
		public ActionResult Edit(int id)
		{
			return Content($"Edit action with id = {id} from '{this.GetType().Name}'");
		}

		// GET: Movie/Released/{year}/{month}
		public ActionResult ByReleaseDate(int? year, int? month)
		{
			DateTime today = DateTime.Today;
			return Content($"ByReleaseDate action with parameter year = {year ?? today.Year}, " +
				$"month = {month ?? today.Month} from {this.GetType().Name}");
		}
	}
}