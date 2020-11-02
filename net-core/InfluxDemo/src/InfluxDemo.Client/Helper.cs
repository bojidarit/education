namespace InfluxDemo.Client
{
	using Microsoft.VisualBasic.FileIO;
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
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

		//CSV to DataTable
		public static DataTable GetDataTabletFromCsvString(string csvString)
		{
			DataTable csvData = new DataTable();
			using (TextReader textReader = new StringReader(csvString))
			{
				using (TextFieldParser csvReader = new TextFieldParser(textReader))
				{
					csvReader.SetDelimiters(new string[] { "," });
					csvReader.HasFieldsEnclosedInQuotes = false;
					string[] colFields = csvReader.ReadFields();

					var index = 1;
					foreach (string column in colFields)
					{
						var columnName = csvData.Columns.Contains(column) ? $"{column}{index}" : column;
						DataColumn datecolumn = new DataColumn(columnName);
						datecolumn.AllowDBNull = true;
						csvData.Columns.Add(datecolumn);
						index++;
					}

					while (!csvReader.EndOfData)
					{
						string[] fieldData = csvReader.ReadFields();

						//Making empty value as null
						for (int i = 0; i < fieldData.Length; i++)
						{
							if (fieldData[i] == "")
							{
								fieldData[i] = null;
							}
						}
						csvData.Rows.Add(fieldData);
					}
				}
			}
			return csvData;
		}
	}
}
