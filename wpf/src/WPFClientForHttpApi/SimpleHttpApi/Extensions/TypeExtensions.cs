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

			// Find the static method no matter the case
			MethodInfo method = type.GetMethods()
				.Where(m => m.IsStatic)
				.FirstOrDefault(m => string.Compare(m.Name, methodName, ignoreCase) == 0);

			// Execute method and get result
			result = method?.Invoke(null, parameters.ToArray());

			return result;
		}

		public static IEnumerable<string> GetStaticMethods(this Type type) =>
			type.GetMethods().Where(m => m.IsStatic).Select(m => m.Name);

		public static bool MatchLibraryName(this Type type, string libraryName, bool ignoreCase = true)
		{
			string value = string.Empty;

			PropertyInfo propertyInfo = type.GetProperties()
				.FirstOrDefault(p => string.Compare(p.Name, "LibraryName", ignoreCase) == 0);

			if (propertyInfo != null)
			{
				value = propertyInfo.GetValue(null).ToString();
			}

			return string.Compare(value, libraryName, ignoreCase) == 0;
		}
	}
}