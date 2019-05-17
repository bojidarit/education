namespace SimpleHttpApi.Models
{
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[DataContract(Name = "DataList")]
	public class DataListModel<T> : BaseDataModel
	{
		public DataListModel() { }

		public DataListModel(string library, string method, IEnumerable<T> list)
			: base(library, method)
		{
			this.Data = list;
		}

		[DataMember]
		public IEnumerable<T> Data { get; private set; }
	}
}