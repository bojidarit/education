namespace Graphics
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using PdfSharp.Fonts;
    using PdfSharp.Pdf;
	using PDFsharpLib;

	/// <summary>
	/// This sample shows some of the capabilities of the XGraphcis class.
	/// Source: https://github.com/empira/PDFsharp-samples
	/// </summary>
	class Program
    {
        static void Main()
        {
            // Use WinAnsi (i.e. not Unicode) for all fonts if no XPdfFontOptions are specified.
            GlobalFontSettings.DefaultFontEncoding = PdfFontEncoding.WinAnsi;

            Document = new PdfDocument();
            Document.Info.Title = "PDFsharp XGraphic Sample";
            Document.Info.Author = "Stefan Lange";
            Document.Info.Subject = "Created with code snippets that show the use of graphical functions";
            Document.Info.Keywords = "PDFsharp, XGraphics";


            // Create demonstration pages.
            new LinesAndCurves().DrawPage(Document.AddPage());
            new Shapes().DrawPage(Document.AddPage());
            new Paths().DrawPage(Document.AddPage());
            new Text().DrawPage(Document.AddPage());
            new Images().DrawPage(Document.AddPage());

            // Save the document...
            const string filename = "HelloGraphics_tempfile.pdf";
            Helper.SaveAndOpenPdfDocument(filename, Document);
        }

        internal static PdfDocument Document;
    }
}

