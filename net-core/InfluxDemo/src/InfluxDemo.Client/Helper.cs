namespace InfluxDemo.Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Helper
	{
		public static List<string> SplitByLine(string lines)
		{
			if (string.IsNullOrEmpty(lines))
			{
				return null;
			}

			var result = lines.Split(
				Environment.NewLine.ToCharArray(), 
				StringSplitOptions.RemoveEmptyEntries);

			return result.ToList();
		}
	}
}
