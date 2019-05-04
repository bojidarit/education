namespace SimpleHttpApi.Extensions
{
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
	}
}