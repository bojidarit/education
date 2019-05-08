namespace WPFSimpleHttpClient.Dtos
{
	public class DateTimeDto : ISingleValueResult<System.DateTime?>, ISingleValueResult
	{
		public System.DateTime? Result { get; set; }

		public override string ToString() =>
			this.Result.ToString();
	}
}
