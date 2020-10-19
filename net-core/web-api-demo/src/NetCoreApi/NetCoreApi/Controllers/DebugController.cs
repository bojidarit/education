namespace NetCoreApi.Controllers
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class DebugController : ControllerBase
	{
		// .net core 3.x specific
		private readonly IWebHostEnvironment environment;

		public DebugController(IWebHostEnvironment env)
		{
			environment = env;
		}

		// GET: api/debug
		[HttpGet(Name = "GetDebugInfo")]
		public ObjectResult GetDebug([FromHeader(Name = "Accept")] string accept,
			[FromHeader(Name = "Accept-Language")] string language,
			[FromHeader(Name = "Accept-Encoding")] string encoding,
			[FromHeader(Name = "Host")] string host,
			[FromHeader(Name = "User-Agent")] string userAgent)
		{
			var result = new Dtos.DebugInfoDto(HttpContext.Connection.RemoteIpAddress, environment);
			result.Language = language;
			result.Encoding = encoding;
			result.Accept = accept;
			result.Host = host;
			result.UserAgent = userAgent;
			result.PoweredBy = Helper.GetRuntimeInformation();

			return Ok(result);
		}
	}
}
