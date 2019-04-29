namespace SimpleHttpApi.Models
{
	using System.Collections.Generic;

	public class DataListModel<T>
	{
		public DataListModel(string library, string method, IEnumerable<T> list)
		{
			this.Data = list;
			this.Library = library;
			this.Method = method;
		}

		public string Library { get; private set; }

		public string Method { get; private set; }

		public IEnumerable<T> Data { get; private set; }
	}
}