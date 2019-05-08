namespace WPFSimpleHttpClient.Dtos
{
	public class StringDto : ISingleValueResult<string>, ISingleValueResult
	{
		public string Result { get; set; }

		public override string ToString() => this.Result;
	}
}
