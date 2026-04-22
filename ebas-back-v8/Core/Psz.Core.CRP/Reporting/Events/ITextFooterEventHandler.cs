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

namespace Psz.Core.CRP.Reporting.Events
{
	public class ITextFooterEventHandler: IEventHandler
	{
		private readonly string _htmlContent;
		private readonly bool _hasPageCounter;
		private readonly string _leftText;
		private readonly string _centerText;
		private readonly string _centerText2;
		private readonly PdfFormXObject totalPagesPlaceholder;
		private readonly float footerY = 20;
		private readonly float placeholderWidth = 50;
		private readonly float placeholderHeight = 12;
		public int height = 130;
		public iText.Kernel.Colors.DeviceRgb BgColor;
		public ITextFooterEventHandler(string htmlContent, bool hasPageCounter, string leftText, string centerText, string centerText2, iText.Kernel.Colors.DeviceRgb bgColor)
		{
			_htmlContent = htmlContent;
			_hasPageCounter = hasPageCounter;
			_leftText = leftText;
			_centerText = centerText;
			_centerText2 = centerText2;
			BgColor = bgColor;
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
			Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1.8f, 2.5f, 2, 1.5f })).UseAllAvailableWidth();

			// LEFT cell
			Cell leftCell;
			if(!_leftText.StringIsNullOrEmptyOrWhiteSpaces())
				leftCell = new Cell()
					.Add(new Paragraph(_leftText).SetFontSize(6))
					.SetTextAlignment(TextAlignment.LEFT);
			else
				leftCell = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetTextAlignment(TextAlignment.LEFT);

			leftCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
			table.AddCell(leftCell);

			// CENTER cell
			Cell centerCell;
			if(!_centerText.StringIsNullOrEmptyOrWhiteSpaces())
				centerCell = new Cell()
					.Add(new Paragraph(_centerText).SetFontSize(6))
					.SetTextAlignment(TextAlignment.CENTER);
			else
				centerCell = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetTextAlignment(TextAlignment.CENTER);

			centerCell.SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
			table.AddCell(centerCell);

			//center text 2
			Cell centerCell2;
			if(!_centerText2.StringIsNullOrEmptyOrWhiteSpaces())
				centerCell2 = new Cell()
					.Add(new Paragraph(_centerText2).SetFontSize(6))
					.SetTextAlignment(TextAlignment.CENTER);
			else
				centerCell2 = new Cell()
					.Add(new Paragraph(string.Empty))
					.SetTextAlignment(TextAlignment.CENTER);

			centerCell2.SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
			table.AddCell(centerCell2);

			// RIGHT cell (page counter) - may be empty if _hasPageCounter == false
			Cell rightCell;
			if(_hasPageCounter)
			{
				int pageNumber = pdf.GetPageNumber(page);

				Paragraph rightPara = new Paragraph()
					.Add($"SEITE {pageNumber} VON ")
					.SetFontSize(6)
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
					.SetFontSize(6)
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
				pageSize.GetWidth() - 50
			);
			if(BgColor != null)
				table.SetBackgroundColor(BgColor);
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
				.SetFontSize(6)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetMargin(0)
				.SetMarginTop(1.3f)
			);
			layoutCanvas.Close();
		}
		public void Dispose()
		{

		}
	}
}
