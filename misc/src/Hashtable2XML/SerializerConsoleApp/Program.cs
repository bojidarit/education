namespace SerializerConsoleApp
{
	using Newtonsoft.Json;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;

	class Program
	{
		static string line = new string('-', 60);

		static void Main(string[] args)
		{
			var table = new Hashtable();
			//table.Add(1, "Title");
			//table.Add(2, "X");
			//table.Add(3, "Y");

			table.Add(1, new Dictionary<string, object> { { "Title", "Надпис" } });
			table.Add(2, new Dictionary<string, object> { { "X", "Х" } });
			table.Add(3, new Dictionary<string, object> { { "Y", "У" } });

			PrintSubTitle("Pure HashTable");
			PrintHashtable(table);
			Console.WriteLine();

			PrintSubTitle("Ordered HashTable by integer key");
			PrintHashtableOrdered(table);
			Console.WriteLine();

			//SerializeItContract(table, new Type[] { typeof(Dictionary<string, object>) });

			SerializeItXDocument(table);
		}

		static void SerializeItXDocument(Hashtable table)
		{
			PrintTitle($"Pure {table.GetType().FullName}");
			Console.WriteLine(ObjectToString(table, Newtonsoft.Json.Formatting.Indented));

			PrintTitle("List of KeyValuePairs (Can be sorted by key using LINQ!!!)");
			var list = new List<KeyValuePair<object, object>>();
			foreach (DictionaryEntry item in table)
			{
				list.Add(new KeyValuePair<object, object>(item.Key, item.Value));
			}
			list = list.OrderBy(i => i.Key).ToList();
			Console.WriteLine(ObjectToString(list, Newtonsoft.Json.Formatting.Indented));

			Func<object, object> func = (o) =>
				(o.GetType().IsValueType || o is string)
				? new XAttribute("value", o) as object
				: new XElement("value", o);

			var xElements = list
				.Select(pair => new XElement("attribute",
									new XAttribute("name", pair.Key),
									func(pair.Value)))
				.ToList();
			PrintTitle($"List of {xElements.First().GetType().FullName}");
			xElements.ForEach(item => Console.WriteLine(item));

			SerializeListOfXElements(xElements);
		}

		static void SerializeListOfXElements(IList xElements)
		{
			PrintTitle($"Serialized HashTable with XDocumet");
			PrintSubTitle("Using XDocument.ToString()");
			var doc = new XDocument(new XElement("HashTable", xElements));

			// Returns the indented XML for this node.
			Console.WriteLine(doc.ToString());
			PrintSubTitle("Using XmlWriter");

			var sb = new StringBuilder();
			var xws = new XmlWriterSettings();
			xws.OmitXmlDeclaration = false;
			xws.Indent = true;

			using (var xw = XmlWriter.Create(sb, xws))
			{
				doc.Save(xw);
			}

			Console.WriteLine(sb.ToString());
			Console.WriteLine();
		}

		static void PrintSubTitle(string title) => Console.WriteLine($"{Environment.NewLine} ***{title}: ");

		static void PrintTitle(string title) => Console.WriteLine($"{line}{Environment.NewLine}{title}{Environment.NewLine}{line}");

		static void SerializeItContract(Hashtable table, IEnumerable<Type> knownTypes)
		{
			var sb = new StringBuilder();
			var serializer = new DataContractSerializer(typeof(Hashtable), knownTypes);
			using (var writer = new StringWriter(sb))
			using (var xmlWriter = XmlWriter.Create(writer))
			{
				serializer.WriteObject(xmlWriter, table);
			}

			Console.WriteLine(FormatXml(sb.ToString()));
			Console.WriteLine();

			using (var reader = new StringReader(sb.ToString()))
			using (var xmlReader = XmlReader.Create(reader))
			{
				table = (Hashtable)serializer.ReadObject(xmlReader);
				PrintHashtable(table);
			}
		}

		static void PrintHashtable(Hashtable table)
		{
			foreach (DictionaryEntry item in table)
			{
				Console.WriteLine($"DictionaryEntry['{item.Key}'] = '{ObjectToString(item.Value)}'");
			}
		}

		static void PrintHashtableOrdered(Hashtable table)
		{
			var keyTypes = table.Keys
				.Cast<object>()
				.Select(o => o.GetType())
				.Distinct();

			if (keyTypes.Count() == 1 && keyTypes.First() == typeof(int))
			{
				foreach (var key in table.Keys.Cast<int>().OrderBy(i => i))
				{
					Console.WriteLine($"DictionaryEntry[{key}] = '{ObjectToString(table[key])}'");
				}
			}
		}

		static string FormatXml(string xml)
		{
			try
			{
				var doc = XDocument.Parse(xml);
				return doc.ToString();
			}
			catch (Exception)
			{
				// Handle and throw if fatal exception here; don't just ignore them
				return xml;
			}
		}

		static string ObjectToString(object obj, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.None) =>
			obj.GetType().IsValueType ? obj.ToString() : JsonConvert.SerializeObject(obj, formatting);
	}
}
