namespace XmlLinqDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    class Program
    {
        #region Fields

        static string line = new string('-', 60);
        static string nl = Environment.NewLine;

        static string xsdPrefix = "xsd";
        static string gmlPrefix = "gml";
        static string wfsPrefix = "wfs";
        static string demoPrefix = "demo";

        static XNamespace nsXsd = "http://www.w3.org/2001/XMLSchema";
        static XNamespace nsGml = "http://www.opengis.net/gml/3.2";
        static XNamespace nsWfs = "http://www.opengis.net/wfs/2.0";
        static XNamespace nsDemo = "WfsDemo";

        static string featureName = "DemoFeature";
        static string featureType = "DemoFeatureType";

        #endregion


        static void Main(string[] args)
        {
            var root = CreatePrefixedElement(nsXsd, "schema")
                .AddPrefixedAttribute(nsDemo, demoPrefix)
                .AddPrefixedAttribute(nsXsd, xsdPrefix)
                .AddPrefixedAttribute(nsGml, gmlPrefix)
                .AddPrefixedAttribute(nsWfs, wfsPrefix)
                .AddAttribute("elementFormDefault", "qualified")
                .AddAttribute("targetNamespace", nsDemo.NamespaceName);

            var import = CreatePrefixedElement(nsXsd, "import")
                .AddAttribute("namespace", nsGml.NamespaceName)
                .AddAttribute("schemaLocation", "http://schemas.opengis.net/wfs/2.0/wfs.xsd");
            root.Add(import);

            var feature = CreatePrefixedElement(nsXsd, "element")
                .AddAttribute("name", featureName)
                .AddAttribute("type", $"{demoPrefix}:{featureType}")
                .AddAttribute("substitutionGroup", $"{gmlPrefix}:_Feature");
            root.Add(feature);

            var type = CreatePrefixedElement(nsXsd, "complexType")
                .AddAttribute("name", featureType);
            root.Add(type);

            var sequence = type.AppendAndGetChild(CreatePrefixedElement(nsXsd, "complexContent"))
                .AppendAndGetChild(CreatePrefixedElement(nsXsd, "extension").AddAttribute("base", $"{gmlPrefix}:AbstractFeatureType"))
                .AppendAndGetChild(CreatePrefixedElement(nsXsd, "sequence"));

            Enumerable
                .Range(1, 10)
                .ToList()
                .ForEach(i => sequence.Add(
                    CreateSimpleElement(
                        i,
                        new KeyValuePair<string, string>("title", $"Title {i}"),
                        new KeyValuePair<string, string>("name", $"Item {i}"))));

            var values = new List<KeyValuePair<object, object>>
            {
                new KeyValuePair<object, object>(0, "Don't know"),
                new KeyValuePair<object, object>(1, "Yes"),
                new KeyValuePair<object, object>(2, "No"),
            };

            sequence.Add(
                CreateEnumerableElement("Some Enum", "AnswerEnum", values));

            PrintTitle("Pure XElement to string");
            Console.WriteLine(root);

            //Console.WriteLine(nl + nl + nl);
            //var doc = new XDocument(root);
            //SerializeXDoc(doc);
        }


        #region Helpers

        public static XElement CreateSimpleElement(
            object value,
            KeyValuePair<string, string> info1,
            KeyValuePair<string, string> info2) =>
            CreatePrefixedElement(nsXsd, "element")
                .AddAttribute("maxOccurs", 1)
                .AddAttribute("minOccurs", 0)
                .AddAttribute(info1.Key, info1.Value)
                .AddAttribute(info2.Key, info2.Value)
                .AddAttribute("nillable", value.GetType().IsNullable().ToString().ToLower())
                .AddAttribute("type", value.GetType().GetGmlType());

        public static XElement CreateEnumerableElement(
            string title,
            string name,
            List<KeyValuePair<object, object>> values)
        {
            var hasValues = values != null && values.Any();
            var value = hasValues ? values.First().Key : string.Empty;

            var element = CreateSimpleElement(
                        value,
                        new KeyValuePair<string, string>("title", title),
                        new KeyValuePair<string, string>("name", name));

            if (!hasValues)
            {
                return element;
            }

            var sequence = element
                .AppendAndGetChild(CreatePrefixedElement(nsXsd, "complexType"))
                .AppendAndGetChild(CreatePrefixedElement(nsXsd, "sequence"));

            values.ForEach(v => sequence.Add(
                CreateSimpleElement(
                    v.Key,
                    new KeyValuePair<string, string>("key", v.Key.ToString()),
                    new KeyValuePair<string, string>("value", v.Value.ToString()))
            ));

            return element;
        }

        public static XElement CreatePrefixedElement(XNamespace ns, string name) =>
            new XElement(ns + name);

        static void SerializeXDoc(XDocument doc)
        {
            var sb = new StringBuilder();
            var xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = false;
            xws.Indent = true;

            using (var xw = XmlWriter.Create(sb, xws))
            {
                doc.Save(xw);
            }

            PrintTitle("Serialized XDocument");
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
        }

        static void PrintTitle(string title) => Console.WriteLine($"{line}{nl}{title}{nl}{line}");

        #endregion
    }
}
