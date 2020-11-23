namespace Influx2Demo.Logic
{
	public static class StringExt
	{
		public static string SingleQuote(this string text) => $"'{text}'";

		public static string Quote(this string text) => $"\"{text}\"";
	}
}
