namespace MVCWebApp5.Controllers
{
	using HelperLib;
	using HelperLib.Dtos;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Extensions;
	using Microsoft.AspNetCore.Mvc;
	using MVCWebApp5.Models;
	using System;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	public class HomeController : Controller
	{
		private readonly IWebHostEnvironment env;

		public HomeController(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public IActionResult Index()
		{
			var debug = GetDebugInfo(HttpContext, env);
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
			var debug = GetDebugInfo(HttpContext, env);
			return Ok(debug);
		}

		private DebugInfo GetDebugInfo(HttpContext httpContext, IWebHostEnvironment env)
		{
			var result = new DebugInfo();
			var connection = httpContext?.Connection;
			var request = httpContext?.Request;

			var hostInfo = Util.GetHostInfo();
			result.HostName = hostInfo.Item1;
			result.ActiveHostIp = Util.GetActiveHostIp();
			result.DhcpHostIp = Util.GetHostDhspIpAddress();
			result.HostIPs = hostInfo.Item2;

			if (connection != null)
			{
				result.RemoteIp = connection.RemoteIpAddress.ToString();
				result.RemotePort = connection.RemotePort;
				result.LocalIp = connection.LocalIpAddress.ToString();
				result.LocalPort = connection.LocalPort;
			}

			if (request != null)
			{
				result.DisplayUrl = request.GetDisplayUrl();
				result.BaseUrl = result.DisplayUrl.TrimEnd(request.Path.ToString().ToCharArray()) + "/";
			}

			if (env != null)
			{
				result.EnvironmentName = env.EnvironmentName;
				result.WebRootPath = env.WebRootPath;
				result.ContentRootPath = env.ContentRootPath;
			}

			result.TargetFramework = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
			result.EnvironmentVersion = Environment.Version.ToString();
			result.FrameworkDescription = RuntimeInformation.FrameworkDescription;
			result.OSDescription = RuntimeInformation.OSDescription;
			result.OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
			result.ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();

			return result;
		}
	}
}
