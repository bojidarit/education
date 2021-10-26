namespace SerializerConsoleApp
{
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class IdObj : IXElementEnabled
	{
		public IdObj(string id, object obj)
		{
			Id = id;
			Obj = obj;
		}

		public string Id { get; set; }
		public object Obj { get; set; }

		public XElement ToXElement(string name) =>
			new XElement(name, new List<XElement> { new XElement(nameof(Id), Id), new XElement(nameof(Obj), Obj) });
	}
}
