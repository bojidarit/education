namespace SimpleHttpApi.Models
{
	using System.Runtime.Serialization;

	[DataContract(Name = "DataValue")]
	public class DataResultModel<T>
	{
		public DataResultModel(T result)
		{
			this.Result = result;
		}

		[DataMember]
		public T Result { get; private set; }
	}
}