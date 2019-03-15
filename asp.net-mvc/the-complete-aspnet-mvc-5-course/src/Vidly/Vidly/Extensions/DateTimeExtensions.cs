namespace Vidly.Extensions
{
	using System;

	public static class DateTimeExtensions
	{
		public static DateTime? NullIfMinValue(this DateTime date) =>
			date == DateTime.MinValue ? default(DateTime?) : date;
	}
}