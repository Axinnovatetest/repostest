using iText.Html2pdf;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Psz.Core.BaseData.Reporting.Events
{
	public class ITextFooterEventHandler: IEventHandler
	{
		private readonly string _htmlContent;
		private readonly bool _hasPageCounter;
		private readonly string _leftText;
		private readonly string _centerText;
		private readonly PdfFormXObject totalPagesPlaceholder;
		private readonly float footerY = 20;
		private readonly float placeholderWidth = 50;
		private readonly float placeholderHeight = 12;
		public int height = 130;
		public ITextFooterEventHandler(string htmlContent, bool hasPageCounter, string leftText, string centerText)
		{
			_htmlContent = htmlContent;
			_hasPageCounter = hasPageCounter;
			_leftText = leftText;
			_centerText = centerText;
			totalPagesPlaceholder = new PdfFormXObject(new Rectangle(0, 0, placeholderWidth, placeholderHeight));
		}
		public ITextFooterEventHandler()
		{
			totalPagesPlaceholder = new PdfFormXObject(new Rectangle(0, 0, placeholderWidth, placeholderHeight));
		}
		public void HandleEvent(Event currentEvent)
		{
			PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
			var page = docEvent.GetPage();
			PdfDocument pdf = docEvent.GetDocument();
			Rectangle pageSize = page.GetPageSize();
			PdfCanvas pdfCanvas = new PdfCanvas(page);
			Canvas c = new Canvas(pdfCanvas, new Rectangle(0, 0, pageSize.GetWidth(), height));
			// Add HTML footer content

			foreach(var item in HtmlConverter.ConvertToElements(_htmlContent))
			{
				// ConvertToElements returns IElement (IBlockElement for block-level)
				if(item is IBlockElement block)
					c.Add(block);
				else if(item is IElement elem) // fallback
					c.Add((IBlockElement)elem);
			}

			// footer other texts
			// Three columns: left date, center text, right page text (Page X of [placeholder])
			Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 })).UseAllAvailableWidth();

			// LEFT cell
			Cell leftCell;
			if(!_leftText.IsNullOrEmptyOrWitheSpaces())
				leftCell = new Cell()
					.Add(new Paragraph(_leftText).SetFontSize(7))
					.SetTextAlignment(TextAlignment.LEFT);
			else
				leftCell = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetTextAlignment(TextAlignment.LEFT);

			leftCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
			table.AddCell(leftCell);

			// CENTER cell
			Cell centerCell;
			if(!_centerText.IsNullOrEmptyOrWitheSpaces())
				centerCell = new Cell()
					.Add(new Paragraph(_centerText).SetFontSize(7))
					.SetTextAlignment(TextAlignment.CENTER);
			else
				centerCell = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetTextAlignment(TextAlignment.CENTER);

			centerCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
			table.AddCell(centerCell);

			// RIGHT cell (page counter) - may be empty if _hasPageCounter == false
			Cell rightCell;
			if(_hasPageCounter)
			{
				int pageNumber = pdf.GetPageNumber(page);

				Paragraph rightPara = new Paragraph()
					.Add($"{pageNumber}/")
					.SetFontSize(7)
					.SetTextAlignment(TextAlignment.RIGHT);

				Image placeholderImage = new Image(totalPagesPlaceholder);
				placeholderImage.SetAutoScale(false);
				placeholderImage.SetHeight(11);
				// use a smaller width so it doesn't push the cell too wide
				placeholderImage.SetWidth(40);
				// nudge baseline so image aligns with text
				placeholderImage.SetRelativePosition(0, 4, 16, 0);

				rightCell = new Cell()
					.Add(rightPara.Add(placeholderImage))
					.SetFontSize(7)
					.SetBorder(Border.NO_BORDER)
					.SetTextAlignment(TextAlignment.RIGHT)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE);
			}
			else
			{
				rightCell = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetBorder(Border.NO_BORDER)
					.SetTextAlignment(TextAlignment.RIGHT)
					.SetVerticalAlignment(VerticalAlignment.MIDDLE);
			}

			table.AddCell(rightCell);
			// position the table across the page width with a little left/right margin
			table.SetFixedPosition(
				pageSize.GetLeft() + 40,
				footerY,
				pageSize.GetWidth() - 80
			);

			c.Add(table);
			c.Close();
		}
		public void WriteTotal(PdfDocument pdfDoc)
		{
			int total = pdfDoc.GetNumberOfPages();

			// Create a PdfCanvas that writes into the placeholder XObject
			var canvasForPlaceholder = new PdfCanvas(totalPagesPlaceholder, pdfDoc);
			var rect = new Rectangle(0, 0, placeholderWidth, placeholderHeight);
			var layoutCanvas = new Canvas(canvasForPlaceholder, rect);

			// Center the number inside the placeholder
			layoutCanvas.Add(new Paragraph(total.ToString())
				.SetFontSize(7)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetMargin(0)
			);
			layoutCanvas.Close();
		}
		public void Dispose()
		{

		}
	}
}
