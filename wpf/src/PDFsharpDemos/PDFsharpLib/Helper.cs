namespace PDFsharpLib
{
	using PdfSharp.Pdf;
	using System;
	using System.Diagnostics;
	using System.IO;

	public static class Helper
	{
		public static readonly string Line = new string('-', 70);

		public static bool SaveAndOpenPdfDocument(string filename, PdfDocument document)
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(folder, filename);

			Console.WriteLine($"Will save '{path}'.{Environment.NewLine}Do you want to start a viewer (Y/n)?");
			var keyInfo = Console.ReadKey();
			var key = keyInfo.KeyChar.ToString().ToUpper();

			try
			{
				Console.Clear();
				document.Save(path);
			}
			catch (Exception ex)
			{
				WriteError(ex, "Error when saving the document");
				return false;
			}

			if (key == "N")
			{
				Console.WriteLine($"Document saved successfully.{Environment.NewLine}{path}{Environment.NewLine}");
				return true;
			}

			try
			{
				// ...and start a viewer.
				Process.Start(path);
			}
			catch (Exception ex)
			{
				WriteError(ex, "Error when opening the document");
				return false;
			}

			return true;
		}

		public static void WriteError(Exception ex, string title)
		{
			Console.WriteLine(Line);
			Console.WriteLine($"-- {(string.IsNullOrEmpty(title) ? "Error" : title)}");
			Console.WriteLine(Line);

			Console.WriteLine($"Exception: {ex.GetType().FullName}");
			Console.WriteLine($"Message: {ex.Message}{Environment.NewLine}");
		}
	}
}
