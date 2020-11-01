namespace InfluxDemo.Client.Database
{
	using System;

	public class FieldKeyItem
	{
		public string Measurement { get; set; }

		public string Key { get; set; }

		public string Type { get; set; }

		public string Field => $"{Key} ({Type})";

		public override string ToString() => $"{Measurement},({Key} as {Type})";

		public static FieldKeyItem Create (string line)
		{
			string[] array = line?.Split(",".ToCharArray());
			if (array== null || array.Length != 4)
			{
				throw new ArgumentException($"Parameter {nameof(line)} is not the proper CSV line.");
			}

			var item = new FieldKeyItem();
			item.Measurement = array[0];
			item.Key = array[2];
			item.Type = array[3];

			return item;
		}
	}
}
