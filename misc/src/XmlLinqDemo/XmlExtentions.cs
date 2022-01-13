namespace XmlLinqDemo
{
	using System.Xml.Linq;

	public static class XmlExtentions
	{
		public static XElement AppendAttribute(this XElement element, string name, object value)
		{
			element.Add(new XAttribute(name, value));
			return element;
		}

		public static XElement AppendAndGetChild(this XElement element, XElement child)
		{
			element.Add(child);
			return child;
		}

		public static XElement AddPrefixedAttribute(this XElement element, XNamespace ns, string prefix)
		{
			element.Add(new XAttribute(XNamespace.Xmlns + prefix, ns.NamespaceName));
			return element;
		}
	}
}
