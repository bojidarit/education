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
		private static string _formatParamName = "format";

		// GET: client/methods/{library}
		[Route("client/methods/{library}")]
		public IHttpActionResult GetMethods(string library)
		{
			try
			{
				Type type = GetLibraryPath(library);
				return Ok(type.GetStaticMethods().Where(m => !m.Contains("_")));
			}
			catch (Exception ex)
			{
				return BadRequest(SmartFormatException(ex));
			}
		}

		// GET: client/{target}
		// Example: http://localhost:50118/client/oblp_users.getuser?apikey=00000&p1=1
		[Route("client/{target}")]
		[HttpGet]
		public IHttpActionResult GetResult(string target)
		{
			Dictionary<string, string> parameters = null;
			TargetModel targetData = null;
			Type dataLogicType = null;

			try
			{
				targetData = GetMethodAndLibrary(target);
				dataLogicType = GetLibraryPath(targetData.Library);

				// Get parameters from query string
				parameters = QueryStringToDictionaty(this.ActionContext.Request.GetQueryNameValuePairs(), true);

				CheckApiKey(parameters);

				// Filter ApiKey parameter
				var paramValues = parameters.Where(p => p.Key != _apiKeyParamName && p.Key != _formatParamName)
					.Select(p => p.Value).ToArray();

				// Get result
				object result = null;
				result = dataLogicType.ExecuteStaticMethod(targetData.Method, paramValues);

				if (result == null)
				{
					return NotFound();
				}
				else
				{
					// PATCH: XML serialized cannot manage reference types inside of an object
					if (result.GetType() == typeof(DataListModel<User>))
					{
						return Ok((DataListModel<User>)result);
					}
					return Ok((DataListModel<DataResultModel<object>>)result);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(SmartFormatException(ex));
			}
		}

		// POST: client/{target}
		[Route("client/{target}")]
		[HttpPost]
		public IHttpActionResult PostResult(string target, ParametersModel parameters)
		{
			TargetModel targetData = null;

			try
			{
				targetData = GetMethodAndLibrary(target);
				Type dataLogicType = GetLibraryPath(targetData.Library);
				CheckApiKey(parameters.ApiKey);

				// Get result
				object result = null;
				result = dataLogicType.ExecuteStaticMethod(targetData.Method, parameters.Params);

				if (result != null)
				{
					return Ok(result);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(SmartFormatException(ex));
			}
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
			if (!parameters.ContainsKey(_apiKeyParamName))
			{
				throw new ArgumentException("Wrong or missing API Key");
			}

			return CheckApiKey(parameters[_apiKeyParamName]);
		}

		private bool CheckApiKey(string value)
		{
			if (string.Compare(value, _apiKeyParamValue) != 0)
			{
				throw new ArgumentException("Wrong API Key");
			}

			return true;
		}

		private string FormatException(Exception exception) =>
			$"{exception.GetType().Name}: '{exception.Message}'";

		private string SmartFormatException(Exception exception)
		{
			if (exception is TargetInvocationException)
			{
				TargetInvocationException reflectionException = exception as TargetInvocationException;
				if (reflectionException.InnerException != null)
				{
					exception = reflectionException.InnerException;
				}
			}

			return FormatException(exception);
		}

		private TargetModel GetMethodAndLibrary(string target)
		{
			var targetArray = target.Split('.');

			if (targetArray.Length < 2)
			{
				throw new ArgumentException("Wrong target. Must contains two items separated with dot symbol. " +
					$"The current one is '{target}'");
			}

			return new TargetModel(targetArray[0], targetArray[1]);
		}

		/// <summary>
		/// Gets the C# type of the library using reflection
		/// </summary>
		/// <param name="library">Library's internal name</param>
		/// <returns>Class type. Throws exception if not found</returns>
		private Type GetLibraryPath(string library)
		{
			Assembly currentAssembly = Assembly.GetExecutingAssembly();

			var typesInDataLogic = currentAssembly.GetTypes()
				.Where(t => t.IsClass && string.Compare(t.Namespace, "SimpleHttpApi.DataLogic", true) == 0);

			Type type = null;
			foreach (var item in typesInDataLogic)
			{
				if (item.MatchLibraryName(library))
				{
					type = item;
					break;
				}
			}

			if (type == null)
			{
				throw new ArgumentException($"Library '{library}' not found.");
			}

			return type;
		}

		#endregion //Helpers
	}
}
