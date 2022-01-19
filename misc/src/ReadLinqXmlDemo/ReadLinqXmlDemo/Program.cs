namespace ReadLinqXmlDemo
{
    using System;
    using System.Xml;
    using System.Xml.Linq;

    class Program
    {
        #region Fields

        static string line = new string('-', 60);
        static string nl = Environment.NewLine;

        static XNamespace nsWfs = "http://www.opengis.net/wfs/2.0";
        static XNamespace nsFes = "http://www.opengis.net/fes/2.0";
        static XNamespace nsXsi = "http://www.w3.org/2001/XMLSchema-instance";


        #endregion


        #region XML

        static string deleteXml = @"<root xmlns:wfs=""http://www.opengis.net/wfs/2.0"" xmlns:fes=""http://www.opengis.net/fes/2.0"">
   <wfs:Delete typeName=""InWaterA_1M"">
      <fes:Filter>
         <fes:ResourceId rid=""InWaterA_1M.1013""/>
      </fes:Filter>
   </wfs:Delete> </root>";

        static string updateXml = @"<root xmlns:wfs=""http://www.opengis.net/wfs/2.0"" xmlns:fes=""http://www.opengis.net/fes/2.0"">
   <wfs:Update typeName=""BuiltUpA_1M"">
      <wfs:Property>
         <wfs:ValueReference>title</wfs:ValueReference>
         <wfs:Value>Some Title</wfs:Value>
      </wfs:Property>
      <wfs:Property>
         <wfs:ValueReference>population</wfs:ValueReference>
         <wfs:Value>234000</wfs:Value>
      </wfs:Property>
      <wfs:Property>
         <wfs:ValueReference>area</wfs:ValueReference>
         <wfs:Value>234000</wfs:Value>
      </wfs:Property>
      <fes:Filter>
         <fes:ResourceId rid = ""BuiltUpA_1M.10131""/>
      </fes:Filter>
      <fes:Filter>
         <fes:ResourceId rid = ""BuiltUpA_1M.10177""/>
      </fes:Filter>
   </wfs:Update> </root>";

        #endregion

        static void Main(string[] args)
        {
            //var element = XElement.Parse(deleteXml).Element(nsWfs + "Delete");
            var element = XElement.Parse(updateXml).Element(nsWfs + "Update");

            PrintTitle("Source XElement");
            Console.WriteLine(element);

            PrintTitle("Filters");
            foreach (var filter in element.NamespacedElements(nsFes, "Filter"))
            {
                foreach (var res in filter.NamespacedElements(nsFes, "ResourceId"))
                {
                    var ridAttribute = res.Attribute(XName.Get("rid"));
                    if (ridAttribute == null || string.IsNullOrEmpty(ridAttribute.Value))
                    {
                        continue;
                    }

                    Console.WriteLine(ridAttribute.Value);
                }
            }

            PrintTitle("properties");
            foreach (var prop in element.NamespacedElements(nsWfs, "Property"))
            {
                var valRef = prop.NamespacedElement(nsWfs, "ValueReference");
                var value = prop.NamespacedElement(nsWfs, "Value");
                Console.WriteLine($"Attribute['{valRef?.Value}'] = '{value?.Value}'");
            }
        }


        #region Helpers

        static void PrintTitle(string title) => Console.WriteLine($"{line}{nl}{title}{nl}{line}");

        #endregion
    }
}
