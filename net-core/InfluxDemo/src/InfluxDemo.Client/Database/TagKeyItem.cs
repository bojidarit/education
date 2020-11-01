namespace InfluxDemo.Client.Database
{
	using System;

	public class TagKeyItem
	{
		public string Measurement { get; set; }

		public string Key { get; set; }

		public override string ToString() => $"{Measurement},{Key}";

		public static TagKeyItem Create(string line)
		{
			string[] array = line?.Split(",".ToCharArray());
			if (array == null || array.Length != 3)
			{
				throw new ArgumentException($"Parameter {nameof(line)} is not the proper CSV line.");
			}

			var tag = new TagKeyItem();
			tag.Measurement = array[0];
			tag.Key = array[2];

			return tag;
		}
	}
}
