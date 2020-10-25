namespace JwtDemo
{
	using System;

	static class Helper
	{
		/// <summary>
		/// Converts DateTime To UNIX TimeStamp
		/// </summary>
		/// <param name="dateTime">Local date and time</param>
		/// <returns>UNIX TimeStamp in Seconds</returns>
		public static long DateTimeToTimeStampSeconds(DateTime dateTime)
		{
			var offset = new DateTimeOffset(dateTime.ToLocalTime());
			return offset.ToUnixTimeSeconds();
		}

		/// <summary>
		/// Converts DateTime To UNIX TimeStamp
		/// </summary>
		/// <param name="dateTime">Local date and time</param>
		/// <returns>UNIX TimeStamp in Milliseconds</returns>
		public static long DateTimeToTimeStampMilliseconds(DateTime dateTime)
		{
			var offset = new DateTimeOffset(dateTime.ToLocalTime());
			return offset.ToUnixTimeMilliseconds();
		}
	}
}
