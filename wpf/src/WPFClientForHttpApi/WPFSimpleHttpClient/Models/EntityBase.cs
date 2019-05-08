namespace WPFSimpleHttpClient.Models
{
	using Catel.Data;
	using System.Runtime.Serialization;

	public class EntityBase : ModelBase
	{
		protected EntityBase() { }

		protected EntityBase(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

		internal void AcceptChanges()
		{
			IsDirty = false;
		}
	}
}
