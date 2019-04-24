namespace ProductsApi.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Web.Http;

	public class ExecuteController : ApiController
	{
		// GET: api/execute/
		// Example: http://localhost:50118/api/execute/oblp_users/getuser?apikey=00000&p1=1
		[Route("api/execute/{library}/{method}")]
		public IHttpActionResult GetResult(string library, string method)
		{
			// TODO: Do we need the type of the parameters?!?...

			Dictionary<string, List<string>> parameters = this.ActionContext.Request.GetQueryNameValuePairs()
				.GroupBy(i => i.Key).ToDictionary(p => p.Key, p => p.ToList().Select(i => i.Value).ToList());

			// When there are one or more parameter(s) with the same names => return bad request 
			var badParams = parameters.Where(i => i.Value.Count > 1);
			if (badParams.Any())
			{
				string badOnece = string.Join(",", badParams.Select(p => $"'{p.Key}'"));
				return BadRequest($"ERROR - There are parameter(s) used more than once: [{badOnece}]");
			}

			var dictionary = parameters.ToDictionary(i => i.Key, i => i.Value.FirstOrDefault());
			dictionary.Add("library", library);
			dictionary.Add("method", method);

			// TODO: Execute method with parameters that came from the query string ... 

			return Ok(dictionary);
		}
	}
}
