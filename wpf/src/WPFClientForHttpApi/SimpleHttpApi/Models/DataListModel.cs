namespace SimpleHttpApi.Models
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract(Name = "DataList")]
	public class DataListModel<T>
	{
		public DataListModel(string library, string method, IEnumerable<T> list)
		{
			this.Data = list;
			this.Library = library;
			this.Method = method;
		}

		[DataMember]
		public string Library { get; private set; }

		[DataMember]
		public string Method { get; private set; }

		[DataMember]
		public IEnumerable<T> Data { get; private set; }
	}
}