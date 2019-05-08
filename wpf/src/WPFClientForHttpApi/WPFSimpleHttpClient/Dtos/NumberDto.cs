namespace WPFSimpleHttpClient.Dtos
{
	public class NumberDto : ISingleValueResult<decimal>, ISingleValueResult
	{
		public decimal Result { get; set; }

		public override string ToString() =>
			this.Result.ToString("F9");

		public string ToString(string format) =>
			this.Result.ToString(format);
	}
}
