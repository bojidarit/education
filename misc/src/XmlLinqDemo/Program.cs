namespace XmlLinqDemo
{
	using System;
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
				.AppendAttribute("elementFormDefault", "qualified")
				.AppendAttribute("targetNamespace", nsDemo.NamespaceName);

			var import = CreatePrefixedElement(nsXsd, "import")
				.AppendAttribute("namespace", nsGml.NamespaceName)
				.AppendAttribute("schemaLocation", "http://schemas.opengis.net/wfs/2.0/wfs.xsd");
			root.Add(import);

			var feature = CreatePrefixedElement(nsXsd, "element")
				.AppendAttribute("name", featureName)
				.AppendAttribute("type", $"{demoPrefix}:{featureType}")
				.AppendAttribute("substitutionGroup", $"{gmlPrefix}:_Feature");
			root.Add(feature);

			var type = CreatePrefixedElement(nsXsd, "complexType")
				.AppendAttribute("name", featureType);
			root.Add(type);

			var sequence = type.AppendAndGetChild(CreatePrefixedElement(nsXsd, "complexContent"))
				.AppendAndGetChild(CreatePrefixedElement(nsXsd, "extension").AppendAttribute("base", $"{gmlPrefix}:AbstractFeatureType"))
				.AppendAndGetChild(CreatePrefixedElement(nsXsd, "sequence"));

			Enumerable
				.Range(1, 10)
				.ToList()
				.ForEach(i => sequence.Add(
					CreatePrefixedElement(nsXsd, "element")
					.AppendAttribute("maxOccurs", 1)
					.AppendAttribute("minOccurs", 0)
					.AppendAttribute("title", $"Title {i}")
					.AppendAttribute("name", $"Item {i}")
					.AppendAttribute("nillable", i.GetType().IsNullable().ToString().ToLower())
					.AppendAttribute("type", i.GetType().GetGmlType())));

			PrintTitle("Pure XElement to string");
			Console.WriteLine(root);

			//Console.WriteLine(nl + nl + nl);
			//var doc = new XDocument(root);
			//SerializeXDoc(doc);
		}


		#region Helpers

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
