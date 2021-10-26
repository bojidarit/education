namespace SerializerConsoleApp
{
	using System.Xml.Linq;

	public interface IXElementEnabled
	{
		XElement ToXElement(string name);
	}
}
