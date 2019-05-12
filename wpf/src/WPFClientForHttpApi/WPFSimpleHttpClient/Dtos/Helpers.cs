namespace WPFSimpleHttpClient.Dtos
{
	using System;
	using System.Collections.Generic;
	using System.Dynamic;

	public static class Helpers
	{
		public static dynamic MakeExpandoWithDefaults(IEnumerable<PropMold> props)
		{
			IDictionary<string, object> obj = new ExpandoObject() as IDictionary<string, object>;

			foreach (PropMold prop in props)
			{
				obj.Add(prop.Name, prop.DefaultValue);
			}

			return obj;
		}

		public static dynamic MakeExpandoWithDefaults(IEnumerable<Tuple<string, Type>> props)
		{
			IDictionary<string, object> obj = new ExpandoObject() as IDictionary<string, object>;

			foreach (var prop in props)
			{
				obj.Add(prop.Item1, PropMold.GetDefault(prop.Item2));
			}

			return obj;
		}
	}
}
