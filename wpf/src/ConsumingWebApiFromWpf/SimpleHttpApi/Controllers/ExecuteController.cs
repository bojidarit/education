namespace SimpleHttpApi.Controllers
{
	using SimpleHttpApi.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Web.Http;

	public class ExecuteController : ApiController
	{
		private IEnumerable<User> _users = new List<User>()
		{
			new User(1, "User 1", new DateTime(2010, 1, 1), 1.23M, true),
			new User(2, "User 2", new DateTime(2013, 12, 31), 2.34M, false),
			new User(3, "User 3", new DateTime(2018, 1, 1), 4.56789M, true),
		};

		// GET: api/execute/{library}/{method}
		// Example: http://localhost:50118/api/execute/oblp_users/getuser?apikey=00000&p1=1
		[Route("api/execute/{library}/{method}")]
		public IHttpActionResult GetResult(string library, string method)
		{
			Dictionary<string, string> parameters = null;

			try
			{
				parameters = QueryStringToDictionaty(this.ActionContext.Request.GetQueryNameValuePairs(), true);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			// TODO: Execute method with parameters that came from the query string ... 
			parameters.Add("library", library);
			parameters.Add("method", method);

			return Ok(parameters);
		}

		// GET: client/{target}
		// Example: http://localhost:50118/client/oblp_users.getuser?apikey=00000&p1=1
		[Route("client/{target}")]
		public IHttpActionResult GetResult(string target)
		{
			Dictionary<string, string> parameters = null;
			var targetArray = target.Split('.');

			if (targetArray.Length < 2)
			{
				return BadRequest("Wrong target. Must contains two items separated with dot symbol. " +
					$"The current one is '{target}'");
			}

			string library = targetArray[0];
			string method = targetArray[1];

			try
			{
				parameters = QueryStringToDictionaty(this.ActionContext.Request.GetQueryNameValuePairs(), true);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			if (parameters != null)
			{
				var users = GetUsers(parameters.FirstOrDefault(i => i.Key.StartsWith("p")));

				if (users.Any())
				{
					return Ok(new DataListModel<User>(library, method, users));
				}
				else
				{
					return NotFound();
				}
			}

			return BadRequest("No parameters");
		}

		private IEnumerable<User> GetUsers(KeyValuePair<string, string> parameter)
		{
			List<User> result = new List<User>();
			int id = -1;

			Int32.TryParse(parameter.Value, out id);

			if (id > 0)
			{
				if (_users.Any(u => u.Id == id))
				{
					result.AddRange(_users.Where(u => u.Id == id));
				}
			}
			else
			{
				result.AddRange(_users);
			}

			return result;
		}

		private Dictionary<string, string> QueryStringToDictionaty(
			IEnumerable<KeyValuePair<string, string>> queryKeyValuePairs, bool checkForDuplicates)
		{
			Dictionary<string, List<string>> parameters = queryKeyValuePairs
				.GroupBy(i => i.Key).ToDictionary(p => p.Key, p => p.ToList().Select(i => i.Value).ToList());

			if (checkForDuplicates)
			{
				// Check if there are one or more parameter(s) with the same names
				var badParams = parameters.Where(i => i.Value.Count > 1);
				if (badParams.Any())
				{
					string badOnece = string.Join(",", badParams.Select(p => $"'{p.Key}'"));
					throw new System.ArgumentException($"ERROR - There are parameter(s) used more than once: [{badOnece}]");
				}
			}

			return parameters.ToDictionary(i => i.Key, i => i.Value.FirstOrDefault());
		}
	}
}
