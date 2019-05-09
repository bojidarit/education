namespace SimpleHttpApi.Models
{
	using System.Runtime.Serialization;

	[DataContract]
	public class BaseDataModel
	{
		public BaseDataModel(string library, string method)
		{
			this.Library = library;
			this.Method = method;
		}

		[DataMember]
		public string Library { get; private set; }

		[DataMember]
		public string Method { get; private set; }
	}
}