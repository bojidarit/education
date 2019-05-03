namespace SimpleHttpApi.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public static class TypeExtensions
	{
		public static object ExecuteStaticMethod(this Type type, string methodName, string[] parameters, bool ignoreCase = true)
		{
			object result = null;

			// Find the method no matter the case
			MethodInfo method = type.GetMethods()
				.Where(m => m.IsStatic)
				.FirstOrDefault(m => string.Compare(m.Name, methodName, ignoreCase) == 0);

			// Execute method and get result
			result = method?.Invoke(null, parameters.ToArray());

			return result;
		}

		public static IEnumerable<string> GetStaticMethods(this Type type) =>
			type.GetMethods().Where(m => m.IsStatic).Select(m => m.Name);
	}
}