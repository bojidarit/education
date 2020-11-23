namespace Influx2Demo.Logic.DataStructures
{
	public class BucketDto
	{
		#region Properties

		public string Name { get; set; }

		public string Id { get; set; }

		public long RetentionPeriod { get; set; }

		public long RetentionPeriodDays => RetentionPeriod / 1000000000 / 24 / 60 / 60;

		public string NameWithRetention
		{
			get
			{
				var periodText = (RetentionPeriod == 0)
					? string.Empty
					: $" (keeps {RetentionPeriodDays} days)";
				return $"{Name}{periodText}";
			}
		}

		#endregion


		#region Methods

		public bool IsSystem() => Name.StartsWith("_");

		public override string ToString() => $"{{Id={Id}, Name='{Name}', RetentionPeriod={RetentionPeriod}}}";

		#endregion
	}
}
