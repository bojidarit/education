namespace MVCWebApp5.Controllers
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using MVCWebApp5.Dtos;
	using MVCWebApp5.Models;
	using System.Diagnostics;

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> logger;
		private readonly IWebHostEnvironment env;

		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
		{
			this.logger = logger;
			this.env = env;
		}

		public IActionResult Index()
		{
			var debug = new DebugInfo(HttpContext, env);
			ViewData["DebugInfo"] = debug.ToString();

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Route("debug")]
		public IActionResult Debug()
		{
			var debug = new DebugInfo(HttpContext, env);
			return Ok(debug);
		}
	}
}
