namespace ProductsApi.Models
{
	using System.Collections.Generic;

	public class DplanList<T>
	{
		public DplanList(IEnumerable<T> list)
		{
			this.Data = list;
		}

		public IEnumerable<T> Data;
	}
}