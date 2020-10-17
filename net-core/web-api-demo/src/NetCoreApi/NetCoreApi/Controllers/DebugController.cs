namespace NetCoreApi.Controllers
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[controller]")]
	[ApiController]
	public class DebugController : ControllerBase
	{
		// GET: api/debug
		[HttpGet(Name = "GetDebugInfo")]
		public ObjectResult GetDebug([FromHeader(Name = "Accept")] string accept,
			[FromHeader(Name = "Accept-Language")] string language,
			[FromHeader(Name = "User-Agent")] string userAgent)
		{
			var result = new Dtos.DebugInfoDto(HttpContext.Connection.RemoteIpAddress);
			result.Language = language;
			result.Accept = accept;
			result.UserAgent = userAgent;
			result.PoweredBy = Helper.GetRuntimeInformation();

			return Ok(result);
		}
	}
}
