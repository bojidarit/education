namespace Influx2Demo.Logic
{
	using CsvHelper;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Globalization;
	using System.IO;
	using System.Linq;

	public static class DataParser
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

		public static DataTable MakeDataTableFromCsv(string csv)
		{
			if (string.IsNullOrEmpty(csv))
			{
				return null;
			}

			using (var reader = new StringReader(csv))
			{
				using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					using (var dataReader = new CsvDataReader(csvReader))
					{
						var dataTable = new DataTable();
						dataTable.Load(dataReader);
						return dataTable;
					}
				}
			}
		}

		public static string TrimTopCsv(string csv, int lines)
		{
			var list = SplitByLine(csv);
			var trimmed = list.Skip(lines);
			return string.Join(Environment.NewLine, trimmed);
		}
	}
}
