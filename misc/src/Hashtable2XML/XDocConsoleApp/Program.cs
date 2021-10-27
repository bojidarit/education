namespace XDocConsoleApp
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;

	class Program
	{
		static string line = new string('-', 60);
		static string nl = Environment.NewLine;

		static void Main(string[] args)
		{
			var root = new XElement("Project");
			var doc = new XDocument(root);

			AppendSimpleList(root, "OrderedList");

			AppendListOfDicts(root, "ListOfDicts");

			SerializeXDoc(doc);

			//var list = new List<string>();
			//var dict = new Dictionary<string, object>();
			//Console.WriteLine($"{nameof(list)} is IEnumerable: {list is IEnumerable}{nl}Type = {list.GetType().FullName}");
			//Console.WriteLine($"{nameof(dict)} is IDictionary: {dict is IDictionary}{nl}Type = {dict.GetType().FullName}");
		}

		static void AppendListOfDicts(XElement root, string name)
		{
			var list = new List<Dictionary<string, string>>();
			list.Add(new Dictionary<string, string> { { "Column 1", "12345" }, { "Column 2", "54321" }, { "Column 3", "00000" } });
			list.Add(new Dictionary<string, string> { { "Column 1", "12345" }, { "Column 2", "54321" } });
			list.Add(new Dictionary<string, string> { { "Column 3", "00000" } });

			var child = new XElement("itemsSource", new XAttribute("name", name));
			AppendChildren(child, list.Select(i => new XElement("item", new XAttribute("type", i.GetType().Name), i)));

			AppendChild(root, child);
		}

		static void AppendSimpleList(XElement root, string name)
		{
			//var list = new List<object> { "Title", "X", "Y" };
			//var list = Enumerable.Range(1, 10);

			//var morning = DateTime.Today.AddHours(7);
			//var list = Enumerable.Range(1, 10).Select(n => morning.AddHours(n));

			var rnd = new Random();
			var list = Enumerable.Range(1, 10).Select(n => Math.Round(rnd.NextDouble() * 100.0, 5));

			var child = new XElement("argumentList", new XAttribute("name", name));
			AppendChildren(child, list.Select(i => new XElement("item", new XAttribute("type", i.GetType().Name), i)));

			AppendChild(root, child);
		}

		static void AppendChildren(XElement root, IEnumerable<XElement> children)
		{
			if (children != null && children.Any())
			{
				root.Add(children);
			}
		}

		static void AppendChild(XElement root, XElement child)
		{
			if (child != null)
			{
				root.Add(child);
			}
		}

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

		static void PrintTitle(string title) => Console.WriteLine($"{line}{Environment.NewLine}{title}{Environment.NewLine}{line}");
	}
}
