namespace NetCoreApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System;
	using System.IO;

	[Route("api/[controller]")]
	[ApiController]
	public class DataController : ControllerBase
	{
		private const string outputFolderName = "api";
		private const string outputFileName = "data.txt";
		private readonly string lineBegin = "/" + new string('-', 98) + "\\";
		private readonly string lineEnd = "\\" + new string('-', 98) + "/";

		/// <summary>
		/// Get: api/data?data=value
		/// Getting the parameter from query string
		/// </summary>
		[HttpGet]
		public IActionResult GetData([FromQuery(Name = "data")] string data)
		{
			SaveData(data);

			return Ok();
		}

		/// <summary>
		/// Get: api/data/{data}
		/// Getting the parameter using MVC way
		/// </summary>
		[HttpGet("{data}")]
		public IActionResult Get(string data)
		{
			SaveData(data);

			return Ok();
		}

		private void SaveData(string data)
		{
			var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(myDocuments, outputFolderName);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			System.IO.File.AppendAllText(
				Path.Combine(path, outputFileName),
				$"{lineBegin}{Environment.NewLine}{data}{Environment.NewLine}{lineEnd}{Environment.NewLine}");
		}
	}
}
