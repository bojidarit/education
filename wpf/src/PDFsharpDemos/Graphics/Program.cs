namespace Graphics
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using PdfSharp.Fonts;
    using PdfSharp.Pdf;

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
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(folder, filename);

            Console.WriteLine($"Will save '{path}'.{Environment.NewLine}Do you want to start a viewer (Y/n)?");
            var keyInfo = Console.ReadKey();
            var key = keyInfo.KeyChar.ToString().ToUpper();

            Document.Save(path);

            if (key == "N")
            {
                Console.Clear();
                return;
            }

            // ...and start a viewer.
            Process.Start(path);
        }

        internal static PdfDocument Document;
    }
}

