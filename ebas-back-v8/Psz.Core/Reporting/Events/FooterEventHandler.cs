using iText.Html2pdf;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;


namespace Psz.Core.Reporting.Events
{
	public class FooterEventHandler: IEventHandler
	{
		public string html;
		public int height = 130;
		public FooterEventHandler(string html)
		{
			this.html = html;
		}
		public FooterEventHandler(string html, int height)
		{
			this.height = height;
			this.html = html;
		}
		public void HandleEvent(Event currentEvent)
		{
			PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
			var page = docEvent.GetPage();
			PdfDocument pdf = docEvent.GetDocument();
			Rectangle pageSize = page.GetPageSize();

			// PdfCanvas to draw low-level, and a layout Canvas for layout elements
			PdfCanvas pdfCanvas = new PdfCanvas(page);
			Canvas c = new Canvas(pdfCanvas, new Rectangle(0, 0, pageSize.GetWidth(), height));

			// 1) Add HTML footer content
			foreach(var item in HtmlConverter.ConvertToElements(html))
			{
				// ConvertToElements returns IElement (IBlockElement for block-level)
				if(item is IBlockElement block)
					c.Add(block);
				else if(item is IElement elem) // fallback
					c.Add((IBlockElement)elem);
			}

			// 2) Add page number "x/y" at bottom-right
			int pageNum = pdf.GetPageNumber(page);              // current page index (1-based)
			int totalPages = pdf.GetNumberOfPages();            // total pages

			string pageText = $"{pageNum}/{totalPages}";

			PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
			Paragraph p = new Paragraph(pageText)
				.SetFont(font)
				.SetFontSize(9)
				.SetMargin(0)
				.SetMultipliedLeading(1);

			// X position: page width - right margin (40 pts)
			float x = pageSize.GetWidth() - 40f;
			// Y position: a bit above the bottom (15 pts)
			float y = 15f;

			// Draw aligned to the right
			c.ShowTextAligned(p, x, y, TextAlignment.RIGHT);

			// finalize
			c.Flush();
			c.Close();
		}
	}
}