namespace SimpleHttpApi.Controllers
{
	using Models;
	using SimpleHttpApi.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Reflection;
	using System.Web.Http;

	public class ExecuteController : ApiController
	{
		private static string _apiKeyParamName = "apikey";
		private static string _apiKeyParamValue = "00000";

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
				return BadRequest(FormatException(ex));
			}

			// TODO: Execute method with parameters that came from the query string ... 
			parameters.Add("library", library);
			parameters.Add("method", method);

			return Ok(parameters);
		}

		// GET: client/methods/{library}
		[Route("client/methods/{library}")]
		public IHttpActionResult GetMethods(string library)
		{
			Assembly currentAssembly = Assembly.GetExecutingAssembly();
			try
			{
				Type type = currentAssembly.GetType($"SimpleHttpApi.DataLogic.{library}");
				return Ok(type.GetStaticMethods());
			}
			catch (Exception ex)
			{
				return BadRequest(FormatException(ex));
			}
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

			// There is only one library for now
			Type dataLogicType = typeof(DataLogic.Users);

			string library = targetArray[0];
			string method = targetArray[1];

			// Check for the only library
			if (string.Compare(library, dataLogicType.Name, true) != 0)
			{
				return BadRequest("Wrong library");
			}

			// Get parameters from query string
			try
			{
				parameters = QueryStringToDictionaty(this.ActionContext.Request.GetQueryNameValuePairs(), true);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			// Check parameters
			if (parameters != null)
			{
				if (!CheckApiKey(parameters))
				{
					return BadRequest("Wrong or missing API Key");
				}

				object result = null;

				try
				{
					// Filter ApiKey parameter
					var paramValues = parameters.Where(p => p.Key != _apiKeyParamName)
						.Select(p => p.Value).ToArray();

					result = ObjectExtensions.ExecuteStaticMethod(dataLogicType, method, paramValues);

					if (result == null)
					{
						return NotFound();
					}
					else
					{
						// PATCH: XML serialized cannot manage reference types inside of an object
						if( result.GetType() == typeof(DataListModel<User>))
						{
							return Ok((DataListModel<User>)result);
						}
						return Ok(result);
					}
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}

			return BadRequest("No parameters");
		}

		#region Helpers

		private T CastToType<T>(object data) =>
			(T)Convert.ChangeType(data, typeof(T));

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

			return parameters.ToDictionary(i => i.Key.ToLower(), i => i.Value.FirstOrDefault());
		}

		private bool CheckApiKey(Dictionary<string, string> parameters)
		{
			bool result = parameters.ContainsKey(_apiKeyParamName);

			{
				result = string.Compare(parameters[_apiKeyParamName], _apiKeyParamValue) == 0;
			}

			return result;
		}

		private string FormatException(Exception exception) =>
			$"{exception.GetType().Name}: '{exception.Message}'";

		#endregion //Helpers
	}
}
