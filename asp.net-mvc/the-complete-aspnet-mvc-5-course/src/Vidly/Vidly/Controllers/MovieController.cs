namespace Vidly.Controllers
{
	using System.Web.Mvc;
	using Vidly.Models;

	public class MovieController : Controller
	{
		// GET: Movies/Random
		public ActionResult Random()
		{
			Movie movie = new Movie() { Id = 777, Name = "Random movie..." };
			return View(movie);
		}
	}
}