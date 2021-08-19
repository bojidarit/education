namespace MVCWebAppNginxIPReal.Controllers
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;
	using MVCWebAppNginxIPReal.Dtos;
	using MVCWebAppNginxIPReal.Models;
	using System.Diagnostics;
	using System.Linq;

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IWebHostEnvironment _env;

		public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
		{
			_logger = logger;
			_env = env;
		}

		public IActionResult Index()
		{
			var debug = new DebugInfo(HttpContext, _env);
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
			var debug = new DebugInfo(HttpContext, _env);
			return Ok(debug);
		}

		[Route("headers")]
		public IActionResult Headers()
		{
			var dict = Request.Headers.ToDictionary(h => h.Key, h => h.Value);
			return Ok(dict);
		}

		[Route("query")]
		public IActionResult Query()
		{
			// Example: localhost:48411/query?foo=bar&name=john&family=doe
			var dict = Request.Query.ToDictionary(q => q.Key, q => q.Value);
			return Ok(dict);
		}
	}
}
