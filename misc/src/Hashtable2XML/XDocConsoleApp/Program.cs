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
		static string nameName = "name";
		static string keyName = "key";
		static string valueName = "value";
		static string typeName = "type";
		static string itemName = "item";

		static void Main(string[] args)
		{
			var root = new XElement("Project");
			var doc = new XDocument(root);

			AppendSimpleList(root, "OrderedList");

			AppendListOfDicts(root, "ListOfDicts");

			SerializeXDoc(doc);
		}

		static void AppendListOfDicts(XElement root, string name)
		{
			var list = new List<Dictionary<string, object>>();
			list.Add(new Dictionary<string, object> { { "Column 1", "12345" }, { "Column 2", "54321" }, { "Column 3", "00000" } });
			list.Add(new Dictionary<string, object> { { "Column 1", "12345" }, { "Column 2", "54321" } });
			list.Add(new Dictionary<string, object> { { "Column 3", "00000" } });

			//var list = new List<Dictionary<string, object>>();
			//list.Add(new Dictionary<string, object> {
			//	{ "Item 1", Enumerable.Range(1, 3).ToArray() },
			//	{ "Item 2", new double[] { 1.23, 4.56 } } });

			AppendChild(root, ObjectToNode(list.Select(i => ToUniversalDict(i)).ToList(), "itemsSource", name));
		}

		static Dictionary<object, object> ToUniversalDict(IDictionary iDict)
		{
			var dict = new Dictionary<object, object>();

			if (iDict != null && iDict.Count > 0)
			{
				var er = iDict.GetEnumerator();
				while (er.MoveNext())
				{
					dict.Add(er.Key, er.Value);
				}
			}

			return dict;
		}

		static void AppendSimpleList(XElement root, string name)
		{
			//var list = new string[] { "Title", "X", "Y" };
			//var list = Enumerable.Range(1, 10).ToList();

			//var morning = DateTime.Today.AddHours(7);
			//var list = Enumerable.Range(1, 10).Select(n => morning.AddHours(n)).ToList();

			//var rnd = new Random();
			//var list = Enumerable.Range(1, 10).Select(n => Math.Round(rnd.NextDouble() * 100.0, 3)).ToList();

			var list = new List<object> {
				Enumerable.Range(1, 3).ToArray(),
				new string[] { "Title", "X", "Y" },
				new double[] { 1.23, 2.34, 3.45 } };

			AppendChild(root, ObjectToNode(list, "argumentList", name));
		}

		static object ObjectToNode(object o, string nodeName, string attrName = null)
		{
			if (string.IsNullOrEmpty(nodeName))
			{
				throw new ArgumentException($"{nameof(nodeName)} is mandatory.");
			}

			var node = CreateNode(nodeName, attrName, o);

			var handled = true;
			var type = o.GetType();

			if (o is string || (type.IsValueType && type.IsPrimitive))
			{
				node.Add(new XAttribute(valueName, o));
			}
			else if (o is IEnumerable)
			{
				((IEnumerable)o)
					.Cast<object>()
					.Select(i => ObjectToNode(i, itemName))
					.ToList()
					.ForEach(i => AppendChild(node, i));
			}
			else if (o is KeyValuePair<object, object>)
			{
				var pair = (KeyValuePair<object, object>)o;
				node.Add(ObjectToNode(pair.Key, keyName));
				node.Add(ObjectToNode(pair.Value, valueName));
			}
			else
			{
				handled = false;
			}

			if (!handled)
			{
				node.Add(o);
			}

			return node;
		}

		static object GetTypeAttribute(object typeObject) =>
			new XAttribute(typeName, typeObject.GetType().Name);

		static XElement CreateSimpleNode(string nodeName, string attrName)
		{
			var node = new XElement(nodeName);

			if (!string.IsNullOrEmpty(attrName))
			{
				node.Add(new XAttribute(nameName, attrName));
			}

			return node;
		}

		static XElement CreateNode(string nodeName, string attrName, object typeObject)
		{
			var node = CreateSimpleNode(nodeName, attrName);

			if (typeObject != null)
			{
				node.Add(GetTypeAttribute(typeObject));
			}

			return node;
		}

		static void AppendChild(XElement root, object child)
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
