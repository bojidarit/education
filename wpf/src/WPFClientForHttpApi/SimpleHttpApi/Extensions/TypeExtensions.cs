namespace SimpleHttpApi.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public static class TypeExtensions
	{
		/// <summary>
		/// Finds the static method (no matter the case)
		/// </summary>
		private static MethodInfo GetStaticMethod(this Type type, string methodName, bool ignoreCase = true) =>
			type.GetMethods().Where(m => m.IsStatic)
				.FirstOrDefault(m => string.Compare(m.Name, methodName, ignoreCase) == 0);

		public static object ExecuteStaticMethod(this Type type, string methodName, object[] parameters, bool ignoreCase = true)
		{
			object result = null;

			MethodInfo method = type.GetStaticMethod(methodName, ignoreCase);

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

		public static dynamic MakeExpandoFromMethod(this Type type, string methodName, bool ignoreCase = true)
		{
			IDictionary<string, Object> obj =
				new System.Dynamic.ExpandoObject() as IDictionary<string, Object>;

			MethodInfo method = type.GetStaticMethod(methodName, ignoreCase);
			IEnumerable<ParameterInfo> props = method.GetParameters().OrderBy(p => p.Position);

			foreach (var prop in props)
			{
				obj.Add(prop.Name, prop.HasDefaultValue ? prop.DefaultValue : prop.ParameterType.GetDefaultValue());
			}

			return obj;
		}

		private static object GetDefaultValue(this Type type)
		{
			object result = null;

			if (type.IsValueType)
			{
				result = Activator.CreateInstance(type);
			}
			else
			{
				if (type == typeof(string))
				{
					result = string.Empty;
				}
			}

			return result;
		}
	}
}