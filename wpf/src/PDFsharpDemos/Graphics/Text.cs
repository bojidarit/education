using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Graphics
{
    /// <summary>
    /// Shows how to handle text.
    /// </summary>
    public class Text : Base
    {
        public void DrawPage(PdfPage page)
        {
            var gfx = XGraphics.FromPdfPage(page);

            DrawTitle(page, gfx, "Text");

            DrawText(gfx, 1);
            DrawTextAlignment(gfx, 2);
            MeasureText(gfx, 3);
        }

        /// <summary>
        /// Draws text in different styles.
        /// </summary>
        void DrawText(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "Text Styles");

            const string facename = "Times New Roman";

            //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);
            var options = new XPdfFontOptions(PdfFontEncoding.WinAnsi);

            var fontRegular = new XFont(facename, 20, XFontStyle.Regular, options);
            var fontBold = new XFont(facename, 20, XFontStyle.Bold, options);
            var fontItalic = new XFont(facename, 20, XFontStyle.Italic, options);
            var fontBoldItalic = new XFont(facename, 20, XFontStyle.BoldItalic, options);

            // The default alignment is baseline left (that differs from GDI+).
            gfx.DrawString("Times (regular)", fontRegular, XBrushes.DarkSlateGray, 0, 30);
            gfx.DrawString("Times (bold)", fontBold, XBrushes.DarkSlateGray, 0, 65);
            gfx.DrawString("Times (italic)", fontItalic, XBrushes.DarkSlateGray, 0, 100);
            gfx.DrawString("Times (bold italic)", fontBoldItalic, XBrushes.DarkSlateGray, 0, 135);

            EndBox(gfx);
        }

        /// <summary>
        /// Shows how to align text in the layout rectangle.
        /// </summary>
        void DrawTextAlignment(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "Text Alignment");
            var rect = new XRect(0, 0, 250, 140);

            var font = new XFont("Verdana", 10);
            var brush = XBrushes.Purple;
            var format = new XStringFormat();

            gfx.DrawRectangle(XPens.YellowGreen, rect);
            gfx.DrawLine(XPens.YellowGreen, rect.Width / 2, 0, rect.Width / 2, rect.Height);
            gfx.DrawLine(XPens.YellowGreen, 0, rect.Height / 2, rect.Width, rect.Height / 2);

            gfx.DrawString("TopLeft", font, brush, rect, format);

            format.Alignment = XStringAlignment.Center;
            gfx.DrawString("TopCenter", font, brush, rect, format);

            format.Alignment = XStringAlignment.Far;
            gfx.DrawString("TopRight", font, brush, rect, format);

            format.LineAlignment = XLineAlignment.Center;
            format.Alignment = XStringAlignment.Near;
            gfx.DrawString("CenterLeft", font, brush, rect, format);

            format.Alignment = XStringAlignment.Center;
            gfx.DrawString("Center", font, brush, rect, format);

            format.Alignment = XStringAlignment.Far;
            gfx.DrawString("CenterRight", font, brush, rect, format);

            format.LineAlignment = XLineAlignment.Far;
            format.Alignment = XStringAlignment.Near;
            gfx.DrawString("BottomLeft", font, brush, rect, format);

            format.Alignment = XStringAlignment.Center;
            gfx.DrawString("BottomCenter", font, brush, rect, format);

            format.Alignment = XStringAlignment.Far;
            gfx.DrawString("BottomRight", font, brush, rect, format);

            EndBox(gfx);
        }

        /// <summary>
        /// Shows how to get text metric information.
        /// </summary>
        void MeasureText(XGraphics gfx, int number)
        {
            BeginBox(gfx, number, "Measure Text");

            const XFontStyle style = XFontStyle.Regular;
            var font = new XFont("Times New Roman", 95, style);

            const string text = "Hello";
            const double x = 20, y = 100;
            var size = gfx.MeasureString(text, font);

            var lineSpace = font.GetHeight();
            var cellSpace = font.FontFamily.GetLineSpacing(style);
            var cellAscent = font.FontFamily.GetCellAscent(style);
            var cellDescent = font.FontFamily.GetCellDescent(style);
            var cellLeading = cellSpace - cellAscent - cellDescent;

            // Get effective ascent.
            var ascent = lineSpace * cellAscent / cellSpace;
            gfx.DrawRectangle(XBrushes.Bisque, x, y - ascent, size.Width, ascent);

            // Get effective descent.
            var descent = lineSpace * cellDescent / cellSpace;
            gfx.DrawRectangle(XBrushes.LightGreen, x, y, size.Width, descent);

            // Get effective leading.
            var leading = lineSpace * cellLeading / cellSpace;
            gfx.DrawRectangle(XBrushes.Yellow, x, y + descent, size.Width, leading);

            // Draw text half transparent.
            var color = XColors.DarkSlateBlue;
            color.A = 0.6;
            gfx.DrawString(text, font, new XSolidBrush(color), x, y);

            EndBox(gfx);
        }
    }
}
