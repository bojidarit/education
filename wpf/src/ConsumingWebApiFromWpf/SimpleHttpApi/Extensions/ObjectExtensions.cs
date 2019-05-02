namespace SimpleHttpApi.Extensions
{
	using System;
	using System.Linq;
	using System.Reflection;

	public static class ObjectExtensions
	{
		public static object GetPropertyValue(this object obj, string property, bool ignoreCase = true)
		{
			object result = null;

			if (obj != null && !string.IsNullOrWhiteSpace(property))
			{
				// Find the property no matter the case
				PropertyInfo propertyInfo = obj.GetType().GetProperties()
					.FirstOrDefault(p => string.Compare(p.Name, property, ignoreCase) == 0);

				// Get the value
				if (propertyInfo != null)
				{
					result = propertyInfo.GetValue(obj);
				}
			}

			return result;
		}

		public static object ExecuteStaticMethod(Type type, string methodName, string[] parameters, bool ignoreCase = true)
		{
			object result = null;

			// Find the method no matter the case
			MethodInfo method = type.GetMethods()
				.FirstOrDefault(m => string.Compare(m.Name, methodName, ignoreCase) == 0);

			// Execute method and get result
			result = method?.Invoke(null, parameters.ToArray());

			return result;
		}
	}
}