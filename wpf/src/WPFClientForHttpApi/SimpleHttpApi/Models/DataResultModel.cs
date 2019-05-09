namespace SimpleHttpApi.Models
{
	using System.Reflection;
	using System.Runtime.Serialization;

	[DataContract(Name = "DataValue")]
	public class DataResultModel<T> : BaseDataModel
	{
		public DataResultModel(string library, string method, T result)
			: base(library, method)
		{
			this.Result = result;
		}

		[DataMember]
		public T Result { get; private set; }
	}
}