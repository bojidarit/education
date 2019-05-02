namespace SimpleHttpApi.Extensions
{
	using System.Reflection;

	public static class ObjectExtensions
	{
		public static object GetPropertyValue(this object obj, string property)
		{
			object result = null;

			if (obj != null && !string.IsNullOrWhiteSpace(property))
			{
				PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
				if (propertyInfo != null)
				{
					result = propertyInfo.GetValue(obj);
				}
			}

			return result;
		}
	}
}