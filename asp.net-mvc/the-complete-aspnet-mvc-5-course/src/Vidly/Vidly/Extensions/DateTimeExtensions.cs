namespace Vidly.Extensions
{
	using System;

	public static class DateTimeExtensions
	{
		public static string ShortDateFormat =>
			System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;

		public static string ShortDateFormattingString =>
			$"{{0:{ShortDateFormat}}}";

		public static DateTime? NullIfMinValue(this DateTime date) =>
			date == DateTime.MinValue ? default(DateTime?) : date;

		public static string FormatDateShort(this DateTime date) =>
			date.ToString(ShortDateFormat);

		public static string FormatDateShort(this DateTime? date) =>
			date.HasValue ? date.Value.FormatDateShort() : string.Empty;
	}
}