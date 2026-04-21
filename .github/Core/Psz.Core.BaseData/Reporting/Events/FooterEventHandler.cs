using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;

namespace Psz.Core.BaseData.Reporting.Events
{
	public class FooterEventHandler: IEventHandler, IDisposable
	{
		private readonly PdfFormXObject totalPagesPlaceholder;
		private readonly float footerY = 20;
		private readonly float placeholderWidth = 50;
		private readonly float placeholderHeight = 12;
		private readonly string LeftText = "";
		public readonly string CenterText = "";
		public FooterEventHandler()
		{

		}
		public FooterEventHandler(PdfDocument pdf, string leftText, string centerText)
		{
			// create a form XObject to be used as a placeholder for total pages
			totalPagesPlaceholder = new PdfFormXObject(new Rectangle(0, 0, placeholderWidth, placeholderHeight));
			LeftText = leftText;
			CenterText = centerText;
		}

		public void HandleEvent(Event e)
		{
			var docEvent = (PdfDocumentEvent)e;
			var pdfDoc = docEvent.GetDocument();
			var page = docEvent.GetPage();
			int pageNumber = pdfDoc.GetPageNumber(page);
			Rectangle pageSize = page.GetPageSize();

			// new content stream after existing content
			var pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
			var canvas = new Canvas(pdfCanvas, pageSize);

			// Three columns: left date, center text, right page text (Page X of [placeholder])
			Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 })).UseAllAvailableWidth();

			// Left
			table.AddCell(new Cell()
				.Add(new Paragraph(LeftText))
				.SetFontSize(9)
				.SetBorder(Border.NO_BORDER)
				.SetTextAlignment(TextAlignment.LEFT)
				.SetVerticalAlignment(VerticalAlignment.MIDDLE)
			);

			// Middle
			table.AddCell(new Cell()
				.Add(new Paragraph(CenterText).SetFontSize(9))
				.SetBorder(Border.NO_BORDER)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetVerticalAlignment(VerticalAlignment.MIDDLE)
			);

			// Right: "Page X of " + placeholder image
			Paragraph rightPara = new Paragraph()
				.Add($"SEITE {pageNumber} VON ")
				.SetFontSize(9)
				.SetTextAlignment(TextAlignment.RIGHT);

			// create layout Image from the XObject placeholder
			Image placeholderImage = new Image(totalPagesPlaceholder);
			placeholderImage.SetAutoScale(false);
			placeholderImage.SetHeight(9); // match font size roughly
			placeholderImage.SetWidth(placeholderWidth);
			// Make sure the image aligns well with baseline
			placeholderImage.SetRelativePosition(0, 2, 20, 0);

			// Add text and placeholder image to a cell aligned right
			Cell rightCell = new Cell()
				.Add(rightPara.Add(placeholderImage))
				.SetFontSize(9)
				.SetBorder(Border.NO_BORDER)
				.SetTextAlignment(TextAlignment.RIGHT)
				.SetVerticalAlignment(VerticalAlignment.MIDDLE);

			table.AddCell(rightCell);

			// position the table across the page width with a little left/right margin
			table.SetFixedPosition(
				pageSize.GetLeft() + 40,
				footerY,
				pageSize.GetWidth() - 80
			);

			canvas.Add(table);
			canvas.Close();
		}

		// Call this after you've finished adding pages but BEFORE pdfDoc.Close()
		public void WriteTotal(PdfDocument pdfDoc)
		{
			int total = pdfDoc.GetNumberOfPages();

			// Create a PdfCanvas that writes into the placeholder XObject
			var canvasForPlaceholder = new PdfCanvas(totalPagesPlaceholder, pdfDoc);
			var rect = new Rectangle(0, 0, placeholderWidth, placeholderHeight);
			var layoutCanvas = new Canvas(canvasForPlaceholder, rect);

			// Center the number inside the placeholder
			layoutCanvas.Add(new Paragraph(total.ToString())
				.SetFontSize(9)
				.SetTextAlignment(TextAlignment.CENTER)
				.SetMargin(0)
			);

			layoutCanvas.Close();
		}

		public void Dispose()
		{
			// nothing to dispose explicitly here, but left for completeness if you extend
		}
	}
}
