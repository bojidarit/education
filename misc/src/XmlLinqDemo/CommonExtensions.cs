namespace XmlLinqDemo
{
	using System;

	public static class CommonExtensions
	{
		public static bool IsNullable(this Type type) =>
			Nullable.GetUnderlyingType(type) != null;

		public static bool IsIntegerType(this Type type) =>
			type == typeof(byte)
			|| type == typeof(sbyte)
			|| type == typeof(short)
			|| type == typeof(ushort)
			|| type == typeof(uint)
			|| type == typeof(int)
			|| type == typeof(ulong)
			|| type == typeof(long);

		public static bool IsFloatingType(this Type type) =>
			type == typeof(float)
			|| type == typeof(double)
			|| type == typeof(decimal);

		public static string GetGmlType(this Type type)
		{
			if (type == typeof(string))
			{
				return "string";
			}
			else if (type == typeof(DateTime))
			{
				return "dateTime";
			}
			else if (type.IsIntegerType())
			{
				return "integer";
			}
			else if (type.IsFloatingType())
			{
				return "real";
			}
			else if (type == typeof(bool))
			{
				return "boolean";
			}

			return string.Empty;
		}
	}
}
