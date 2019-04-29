namespace ProductsApi.Models
{
	using System.Collections.Generic;

	public class DataListModel<T>
	{
		public DataListModel(IEnumerable<T> list)
		{
			this.Data = list;
		}

		public IEnumerable<T> Data;
	}
}