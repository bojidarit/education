namespace HelloMigraDoc
{
	using MigraDoc.DocumentObjectModel;
	using MigraDoc.DocumentObjectModel.Fields;
	using MigraDoc.DocumentObjectModel.Shapes;
	using MigraDoc.DocumentObjectModel.Tables;
	using MigraDoc.Rendering;
	using PdfSharp.Pdf;
	using PDFsharpLib;
	using System;
	using System.Reflection;

	class HelloMigraProgram
	{
		#region Fields

		private static Document document;

		private static string[] Contacts = new[]
		{
			"empira Software GmbH",
			"Kirchstraße 19",
			"53840 Troisdorf",
			"Germany",
			"+49-2241-97367-0",
		};

		private static string[] LoremIpsums = new[]
		{
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam a purus sed lectus fermentum aliquet nec sed nibh. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Mauris vitae nisi neque. Morbi commodo mauris nec nisi finibus, sit amet pulvinar sem sodales. Sed vitae metus lacus. Nulla tellus nibh, tristique quis leo non, vulputate ullamcorper arcu. Mauris ornare, augue maximus dictum facilisis, nisi lacus euismod nibh, vitae fringilla quam libero nec magna. Vestibulum porttitor diam sit amet augue tempus malesuada. Interdum et malesuada fames ac ante ipsum primis in faucibus. Fusce dictum scelerisque dolor sit amet pretium.",
			"Aenean egestas eros ut sem semper blandit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Etiam auctor ligula a erat dapibus pulvinar. Nulla in nisi in elit sodales interdum id sed diam. Sed rutrum tincidunt risus, vitae ullamcorper velit semper eget. Maecenas porttitor ex urna, vel euismod leo congue et. Sed volutpat non mi in tempus. Mauris sollicitudin rutrum augue imperdiet sollicitudin. Cras euismod, mauris in hendrerit bibendum, arcu felis vestibulum enim, sit amet rutrum dolor metus in augue. Etiam non pretium libero. Nulla blandit arcu vel commodo molestie.",
			"Aliquam vitae augue fringilla, rutrum libero at, semper metus. Suspendisse et dolor nisi. Nam tristique ex a ex congue aliquam. Donec et efficitur velit. Integer maximus ipsum velit, in elementum dui finibus vitae. Sed vestibulum gravida urna at consequat. Quisque sagittis pulvinar sapien in vulputate. Donec a ex tempus, pretium dui ut, pretium mauris. Praesent nibh felis, vehicula non leo non, vulputate efficitur nulla. Maecenas elementum tortor eros, eu sollicitudin eros venenatis a. Duis egestas sed dui sit amet rutrum. Nunc sed venenatis velit, nec euismod leo.",
			"Pellentesque id nisi at arcu luctus ullamcorper sed ac eros. Vestibulum imperdiet risus semper blandit hendrerit. Nullam sed lacinia nisi, eget lacinia augue. Nunc nec nisl ac elit accumsan malesuada. Fusce ex justo, efficitur id dapibus eu, volutpat eu justo. Integer velit turpis, malesuada mollis maximus eu, dapibus at leo. Aliquam a magna massa. Donec et vestibulum justo, ac semper turpis. Nunc et ultricies sem. Quisque faucibus ut lectus id condimentum. Quisque non urna sem. In auctor finibus molestie. Proin at nulla feugiat, maximus urna ac, placerat ligula. Nunc rutrum eget enim sit amet aliquet. Proin non tellus consectetur tellus feugiat semper quis ac est. Fusce facilisis nisl sit amet iaculis aliquam.",
			"Donec ac massa velit. Vivamus dignissim velit ante, tempus commodo tellus euismod a. Nunc pulvinar nibh ut risus tempus tristique in sed tortor. Vestibulum vulputate purus quis imperdiet iaculis. Sed quis nulla tempor lorem finibus convallis. In eget interdum sem.",
		};

		private static string HeaderFooterFontName = "Consolas";
		private static Unit HeaderFooterFontSize = 10;
		private static readonly string Dots = new string ('.', 33);

		private static Row firstRow = null;

		#endregion


		#region Methods

		static void Main(string[] args)
		{
			CreateDocument();

			var pdfDocument = RenderMigraDocument();

			const string filename = "HelloMigraDoc_tempfile.pdf";
			Helper.SaveAndOpenPdfDocument(filename, pdfDocument);
		}

		/// <summary>
		/// Creates an absolutely minimalistic document.
		/// </summary>
		static void CreateDocument()
		{
			// Create a new MigraDoc document.
			document = new Document();
			document.Styles[StyleNames.Normal].Font.Name = "Lucida Sans Unicode";

			// Add a section to the document.
			var section = document.AddSection();
			var setup = section.PageSetup;

			// Set page setting - they are used to calculate footer table width

			// Alternative for setting page setup
			//setup = document.DefaultPageSetup.Clone();
			//setup.PageFormat = PageFormat.A4;

			setup.PageWidth = "21cm";
			setup.PageHeight = "29.7cm";
			setup.TopMargin = "3.3cm";
			setup.BottomMargin = "1.5cm";
			setup.LeftMargin = setup.RightMargin = "2cm";

			setup.FooterDistance = setup.HeaderDistance = "1cm";

			CreateContent(section);

			CreateHeader(section, imageHeight: "2cm");

			var sectionWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

			CreateFooter(section, sectionWidth);

			CreateSignature(section, sectionWidth);
		}

		private static void CreateContent(Section section)
		{
			// Add a paragraph to the section.
			var paragraph = section.AddParagraph();

			// Set font color.
			//paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
			paragraph.Format.Font.Color = Colors.DarkBlue;
			paragraph.Format.Alignment = ParagraphAlignment.Center;

			paragraph.AddLineBreak();

			// Add some text to the paragraph.
			var text = paragraph.AddFormattedText("Hello MigraDoc", TextFormat.Bold);
			text.Size = 25;

			paragraph.AddLineBreak();
			paragraph.AddLineBreak();

			for (var i = 0; i < Math.Min(LoremIpsums.Length, 5); i++)
			{
				// Some dummy text - multi-line
				paragraph = section.AddParagraph();
				text = paragraph.AddFormattedText(LoremIpsums[i]);
				text.Size = 16;
			}
		}

		static void CreateHeader(Section section, Unit imageHeight)
		{
			var header = section.Headers.Primary;

			var imageName = GetImageFromResource("Images.LogoLandscape.png");
			if (!string.IsNullOrEmpty(imageName))
			{
				var image = header.AddImage(imageName);
				image.Height = imageHeight;
				image.LockAspectRatio = true;
				image.RelativeVertical = RelativeVertical.Line;
				image.RelativeHorizontal = RelativeHorizontal.Margin;
				image.Top = ShapePosition.Top;
				image.Left = ShapePosition.Left;
				image.WrapFormat.Style = WrapStyle.Through;
			}

			imageName = GetImageFromResource("Images.CityLogo.png");
			if (!string.IsNullOrEmpty(imageName))
			{
				var image = header.AddImage(imageName);
				image.Height = imageHeight;
				image.LockAspectRatio = true;
				image.RelativeVertical = RelativeVertical.Line;
				image.RelativeHorizontal = RelativeHorizontal.Margin;
				image.Top = ShapePosition.Top;
				image.Left = ShapePosition.Center;
				image.WrapFormat.Style = WrapStyle.Through;
			}

			var paragraph = header.AddParagraph();
			paragraph.Format.Font.Name = HeaderFooterFontName;
			paragraph.Format.Font.Size = HeaderFooterFontSize;
			paragraph.Format.Alignment = ParagraphAlignment.Right;

			var contacts = string.Join(Environment.NewLine, Contacts);
			paragraph.AddText(contacts);
		}

		static void CreateFooter(Section section, float sectionWidth)
		{
			var columnWidth = sectionWidth / 4.0;

			// Create and style the primary footer.
			var footer = section.Footers.Primary;
			footer.Format.Font.Name = HeaderFooterFontName;
			footer.Format.Font.Size = HeaderFooterFontSize;

			// Add content to footer.

			var table = footer.AddTable();
			table.Borders.Top.Visible = true;
			table.Borders.Top.Width = "0.05cm";

			var column = table.AddColumn();
			column.Format.Alignment = ParagraphAlignment.Left;
			column.Width = columnWidth;

			column = table.AddColumn();
			column.Format.Alignment = ParagraphAlignment.Center;
			column.Width = columnWidth * 2.0;

			column = table.AddColumn();
			column.Format.Alignment = ParagraphAlignment.Right;
			column.Width = columnWidth;

			var row = table.AddRow();
			row.VerticalAlignment = VerticalAlignment.Bottom;

			var paragraph = row.Cells[0].AddParagraph();
			paragraph.Add(new DateField() { Format = "yyyy-MM-dd" });

			row.Cells[1].AddParagraph("Powered by empira Software GmbH");

			paragraph = row.Cells[2].AddParagraph();
			paragraph.Add(new PageField());
			paragraph.AddText(" of ");
			paragraph.Add(new NumPagesField());
		}

		private static void CreateSignature(Section section, float sectionWidth)
		{
			var columnWidth = sectionWidth / 4.0;

			var paragraph = section.AddParagraph();

			// Set table structure
			var table = section.AddTable();
			table.Format.KeepTogether = true;
			table.Borders.Visible = false;

			for (var i = 0; i < 4; i++)
			{
				var column = table.AddColumn();
				column.Width = columnWidth;
			}

			// Empty row for signatures table separation
			firstRow = table.AddRow();

			// Add table content
			AddSignatureData(table, "Created from", "Approved from", "administrator", "manager");
			AddSignatureData(table, "First check", "Second check", "supervisor 1", "supervisor 2");
			AddSignatureData(table, "Third check", "Fourth check", "supervisor 3", "supervisor 4");

			firstRow.KeepWith = table.Rows.Count - 1;
		}

		static void AddSignatureData(
			Table table,
			string from1,
			string from2,
			string position1,
			string position2)
		{
			var row = table.AddRow();

			row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
			row.Cells[0].AddParagraph().AddText(from1);

			row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
			row.Cells[1].AddParagraph().AddText(Dots);

			row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
			row.Cells[2].AddParagraph().AddText(from2);

			row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
			row.Cells[3].AddParagraph().AddText(Dots);

			row = table.AddRow();

			row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
			row.Cells[1].AddParagraph().AddText($"/{position1}/");

			row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
			row.Cells[3].AddParagraph().AddText($"/{position2}/");

			// Empty row for signature group separation
			table.AddRow();
		}

		static PdfDocument RenderMigraDocument(bool unicode = true)
		{
			// Create a renderer for the MigraDoc document.
			var pdfRenderer = new PdfDocumentRenderer(unicode);

			// Associate the MigraDoc document with a renderer.
			pdfRenderer.Document = document;

			// Layout and render document to PDF.
			pdfRenderer.RenderDocument();

			return pdfRenderer.PdfDocument;
		}

		static string GetImageFromResources(Assembly assembly, string name)
		{
			using (var stream = assembly.GetManifestResourceStream(name))
			{
				if (stream == null)
				{
					return null;
				}

				var lenght = (int)stream.Length;
				var buffer = new byte[lenght];
				stream.Read(buffer, 0, lenght);

				if (buffer == null)
				{
					return null;
				}

				return Convert.ToBase64String(buffer);
			}
		}

		/// <summary>
		/// Get image from embedded resources.
		/// </summary>
		/// <param name="imageFithFolderName">Must contain the path to the image delimited with dots instead of slash.</param>
		static string GetImageFromResource(string imageFithFolderName)
		{
			try
			{
				var assembly = Assembly.GetExecutingAssembly();
				var assemblyName = assembly.GetName().Name;
				var data = GetImageFromResources(assembly, $"{assemblyName}.{imageFithFolderName}");
				return !string.IsNullOrEmpty(data) ? $"base64:{data}" : null;
			}
			catch (Exception ex)
			{
				Helper.WriteError(ex, $"Getting Image From Resource ({imageFithFolderName}).");
				return null;
			}
		}

		#endregion
	}
}
