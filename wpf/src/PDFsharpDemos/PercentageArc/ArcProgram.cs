namespace PercentageArc
{
	using PdfSharp.Drawing;
	using PdfSharp.Fonts;
	using PdfSharp.Pdf;
	using PDFsharpLib;

	class ArcProgram
	{
		#region Fields

		static PdfDocument Document;
		static XGraphicsState state;

		static XColor BackColor;
		static XColor BackColor2;
		static XColor ShadowColor;
		static double BorderWidth;
		static XPen BorderPen;

		static double BoxWidth = 590;
		static double BoxHeight = 360;

		#endregion


		#region Static Constructor

		static ArcProgram()
		{
			BackColor = XColors.WhiteSmoke;
			BackColor2 = XColors.LightGray;

			//BackColor = XColor.FromArgb(212, 224, 240);
			//BackColor2 = XColor.FromArgb(253, 254, 254);

			ShadowColor = XColors.Gainsboro;
			BorderWidth = 4.5;
			BorderPen = new XPen(XColors.DimGray, BorderWidth);
		}

		#endregion


		#region Main method

		static void Main(string[] args)
		{
			CreateDocument();
		}

		#endregion


		#region Helpers

		static void CreateDocument()
		{
			// Use WinAnsi (i.e. not Unicode) for all fonts if no XPdfFontOptions are specified.
			GlobalFontSettings.DefaultFontEncoding = PdfFontEncoding.WinAnsi;

			Document = new PdfDocument();
			Document.Info.Title = "PDFsharp Percentage Arc";
			Document.Info.Author = "Bojidar Wan";
			Document.Info.Subject = "Sample 0 to 100 percent arc";
			Document.Info.Keywords = "PDFsharp, XGraphics";

			var page = Document.AddPage();
			var gfx = XGraphics.FromPdfPage(page);

			BeginBox(gfx);

			// TODO: Draw content...
			var pen = new XPen(XColors.Black, width: 3.7);
			var diameter = BoxWidth - 60;
			var radius = diameter / 2.0;
			gfx.DrawArc(pen, x: 5, y: 10, width: diameter, height: diameter, startAngle: 180, sweepAngle: 180);
			gfx.DrawLine(pen, x1: 4, y1: 8 + radius, x2: diameter + 6, y2: 8 + radius);

			EndBox(gfx);

			Helper.SaveAndOpenPdfDocument("PercentageArc.pdf", Document);
		}


		/// <summary>
		/// Draws a sample box.
		/// </summary>
		static void BeginBox(XGraphics gfx, string title = null)
		{
			const int dEllipse = 15;
			var rect = new XRect(x: 0, y: 20, width: BoxWidth, height: BoxHeight);
			rect.Inflate(-10, -10);

			var rect2 = rect;
			rect2.Offset(BorderWidth, BorderWidth);
			gfx.DrawRoundedRectangle(new XSolidBrush(ShadowColor), rect2, new XSize(dEllipse + 8, dEllipse + 8));
			var brush = new XLinearGradientBrush(rect, BackColor, BackColor2, XLinearGradientMode.Vertical);
			gfx.DrawRoundedRectangle(BorderPen, brush, rect, new XSize(dEllipse, dEllipse));
			rect.Inflate(-5, -5);

			if (!string.IsNullOrEmpty(title))
			{
				var font = new XFont("Verdana", 12, XFontStyle.Regular);
				gfx.DrawString(title, font, XBrushes.Navy, rect, XStringFormats.TopCenter);
			}

			rect.Inflate(-10, -5);
			rect.Y += 20;
			rect.Height -= 20;

			state = gfx.Save();
			gfx.TranslateTransform(rect.X, rect.Y);
		}

		static void EndBox(XGraphics gfx)
		{
			gfx.Restore(state);
		}

		#endregion
	}
}
