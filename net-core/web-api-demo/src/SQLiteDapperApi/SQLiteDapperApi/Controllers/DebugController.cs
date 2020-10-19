namespace SQLiteDapperApi.Controllers
{
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using SQLiteDapperApi.Dtos;

	[Route("api/[controller]")]
	[ApiController]
	public class DebugController : ControllerBase
	{
		IHostingEnvironment hostingEnvironment;

		public DebugController(IHostingEnvironment hostingEnvironment)
		{
			this.hostingEnvironment = hostingEnvironment;
		}

		// GET: api/debug
		[HttpGet(Name = "GetDebugInfo")]
		public ObjectResult GetDebug(
			[FromHeader(Name = "Accept")] string accept,
			[FromHeader(Name = "Accept-Language")] string language,
			[FromHeader(Name = "Accept-Encoding")] string encoding,
			[FromHeader(Name = "Host")] string host,
			[FromHeader(Name = "User-Agent")] string userAgent)
		{
			var dto = new DebugInfoDto(HttpContext.Connection.RemoteIpAddress, hostingEnvironment);
			dto.Language = language;
			dto.Accept = accept;
			dto.Encoding = encoding;
			dto.Host = host;
			dto.UserAgent = userAgent;

			return Ok(dto);
		}
	}
}
