namespace NetCoreApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using System;

	[ApiController]
	public class DataController : ControllerBase
	{
		private readonly string lineBegin = "/" + new string('-', 98) + "\\";
		private readonly string lineEnd = "\\" + new string('-', 98) + "/";

		[HttpGet("api/[controller]")]
		public IActionResult GetData([FromQuery(Name = "data")]string data)
		{
			SaveData(data);

			return Ok();
		}

		[HttpGet("api/[controller]/{data}")]
		public IActionResult Get(string data)
		{
			SaveData(data);

			return Ok();
		}

		private void SaveData(string data)
		{
			System.IO.File.AppendAllText(
				@"c:\temp\api\data.txt",
				$"{lineBegin}{Environment.NewLine}{data}{Environment.NewLine}{lineEnd}{Environment.NewLine}");
		}
	}
}
