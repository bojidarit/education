namespace SimpleHttpApi.Models
{
	public class TargetModel
	{
		public TargetModel(string library, string method)
		{
			this.Library = library;
			this.Method = method;
		}

		public string Library { get; private set; }
		public string Method { get; private set; }

	}
}