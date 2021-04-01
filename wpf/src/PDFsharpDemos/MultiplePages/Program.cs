namespace MultiplePages
{
	using PdfSharp.Drawing;
	using PdfSharp.Pdf;
	using System;
	using System.Diagnostics;
	using System.IO;

	/// <summary>
	/// This sample shows one way to create a PDF document with multiple pages.
	/// Note: Consider using MigraDoc instead of PDFsharp for large documents. 
	/// You can use many attributes to format text and you get the line-breaks and page-breaks for free.
	/// Source: http://www.pdfsharp.net/wiki/MultiplePages-sample.ashx
	/// </summary>
	class Program
	{
		/// <summary>
		/// Sample code that shows the LayoutHelper class at work. The sample uses short texts that will always fit into a single line. 
		/// Adding line-breaks to texts that do not fit into a single line is beyond the scope of this sample.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			var document = new PdfDocument();

			// Sample uses DIN A4, page height is 29.7 cm. We use margins of 2.5 cm.
			var helper = new LayoutHelper(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
			var left = XUnit.FromCentimeter(2.5);

			// Random generator with seed value, so created document will always be the same.
			var rand = new Random(42);

			const int headerFontSize = 20;
			const int normalFontSize = 10;

			var fontHeader = new XFont("Verdana", headerFontSize, XFontStyle.BoldItalic);
			var fontNormal = new XFont("Verdana", normalFontSize, XFontStyle.Regular);

			const int totalLines = 666;
			var washeader = false;
			for (int line = 0; line < totalLines; ++line)
			{
				var isHeader = line == 0 || !washeader && line < totalLines - 1 && rand.Next(15) == 0;
				washeader = isHeader;

				// We do not want a single header at the bottom of the page,
				// so if we have a header we require space for header and a normal text line.
				var top = helper.GetLinePosition(
					isHeader ? headerFontSize + 5 : normalFontSize + 2,
					isHeader ? headerFontSize + 5 + normalFontSize : normalFontSize);

				helper.Gfx.DrawString(isHeader ? "Sed massa libero, semper a nisi nec" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
					isHeader ? fontHeader : fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
			}

			// Save the document... 
			const string filename = "MultiplePages_tempfile.pdf";
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(folder, filename);

			Console.WriteLine($"Will save '{path}'.{Environment.NewLine}Do you want to start a viewer (Y/n)?");
			var keyInfo = Console.ReadKey();
			var key = keyInfo.KeyChar.ToString().ToUpper();

			document.Save(path);

			if (key == "N")
			{
				Console.Clear();
				return;
			}

			// ...and start a viewer.
			Process.Start(path);
		}
	}
}
