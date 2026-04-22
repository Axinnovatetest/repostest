using iText.Html2pdf;
using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Psz.Core.MaterialManagement.Reporting.Events
{
	public class FooterEventHandler_2: IEventHandler
	{
		public string html;
		public int height = 130;
		public FooterEventHandler_2(string html)
		{
			this.html = html;
		}
		public FooterEventHandler_2(string html, int height)
		{
			this.height = height;
			this.html = html;
		}
		public void HandleEvent(Event currentEvent)
		{
			PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
			iText.Kernel.Geom.Rectangle pageSize = docEvent.GetPage().GetPageSize();
			var canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(docEvent.GetPage());
			Canvas c = new iText.Layout.Canvas(canvas, new iText.Kernel.Geom.Rectangle(0, 0, pageSize.GetWidth(), 130));
			PdfDocument pdf = docEvent.GetDocument();
			int numberOfPages = pdf.GetNumberOfPages();

			foreach(var item in HtmlConverter.ConvertToElements(html))
			{
				c.Add((IBlockElement)item);
			}
			c.Flush();
			c.Close();

		}
	}
}