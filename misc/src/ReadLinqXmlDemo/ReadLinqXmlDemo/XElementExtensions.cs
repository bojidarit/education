namespace ReadLinqXmlDemo
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public static class XElementExtensions
    {
        public static XElement NamespacedElement(this XElement element, XNamespace ns, string name) =>
            element.Element(ns + name);

        public static IEnumerable<XElement> NamespacedElements(this XElement element, XNamespace ns, string name) =>
            element.Elements(ns + name);

        public static XAttribute NamespacedAttribute(this XElement element, XNamespace ns, string name) =>
            element.Attribute(ns + name);
    }
}
