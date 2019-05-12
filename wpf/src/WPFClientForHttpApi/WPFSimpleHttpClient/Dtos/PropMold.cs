namespace WPFSimpleHttpClient.Dtos
{
	using System;

	public class PropMold
	{
		private PropMold() { }

		public string Name { get; private set; }
		public Type Type { get; private set; }
		public object DefaultValue { get; private set; }

		public static PropMold Make<T>(string name) =>
			Make(name, GetDefault<T>());

		public static PropMold Make<T>(string name, T defaultValue)
		{
			PropMold result = new PropMold()
			{
				Name = name,
				Type = typeof(T),
				DefaultValue = defaultValue
			};

			return result;
		}

		private static object GetDefault<T>()
		{
			object result = default(T);

			// Set specific default value according to the type
			if (typeof(T) == typeof(string))
			{
				result = string.Empty;
			}

			return result;
		}

		public static object GetDefault(Type type)
		{
			object result = null;

			if (type.IsValueType)
			{
				result = Activator.CreateInstance(type);
			}
			else if (type == typeof(string))
			{
				result = string.Empty;
			}

			return result;
		}
	}
}
