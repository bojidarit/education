namespace Vidly.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using Vidly.Models;
	using Vidly.ViewModels;

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
			RandomMovieViewModel viewModel	 = new RandomMovieViewModel()
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

		// GET: Movie/Edit/{id}
		public ActionResult Edit(int id)
		{
			return Content($"Edit action with id = {id} from '{this.GetType().Name}'");
		}

		// GET: Movie/Released/{year}/{month}
		[Route("movie/released/{year:regex(\\d{4})}/{month:range(1,12)}")]
		public ActionResult ByReleaseDate(int year, int month)
		{
			return Content($"ByReleaseDate action with parameter year = {year} and " +
				$"month = {month} from {this.GetType().Name}");
		}
	}
}