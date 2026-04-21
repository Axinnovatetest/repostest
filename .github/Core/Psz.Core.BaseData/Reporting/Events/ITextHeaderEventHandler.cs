using iText.IO.Image;
using iText.Kernel.Colors;
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
	public class ITextHeaderEventHandler: IEventHandler
	{
		const int PLACEHOLDER_SIZE = 8;
		protected float yHeader = 720;
		protected PdfFormXObject placeholder;
		protected PdfFormXObject placeholderForFirstPage;
		protected float descent = 3;
		protected float side = 20;
		protected float x = 200;
		protected float y = 800;
		protected float space = 55f;
		private readonly string _headerText;
		private readonly string _logoBase64;
		private readonly float _headerHeight;
		private readonly string _documentTitle;
		private readonly bool _logoWithText;
		private readonly bool _logoWithCounter;
		private readonly bool _firstPageOnly;

		public ITextHeaderEventHandler(string headerText, string logoBase64, bool logoWithCounter, bool logoWithText, string documentTitle, bool firstPageOnly, float headerHeight = 60)
		{
			_headerText = headerText;
			_logoBase64 = logoBase64;
			_headerHeight = headerHeight;
			_documentTitle = documentTitle;
			_logoWithCounter = logoWithCounter;
			_logoWithText = logoWithText;
			_firstPageOnly = firstPageOnly;
			this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
			this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
		}
		public void HandleEvent(Event e)
		{
			PdfDocumentEvent docEvent = (PdfDocumentEvent)e;
			PdfDocument pdf = docEvent.GetDocument();
			PdfPage page = docEvent.GetPage();
			Rectangle pageSize = page.GetPageSize();
			int pageNumber = pdf.GetPageNumber(page);

			PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), docEvent.GetDocument());
			Canvas canvas = new Canvas(pdfCanvas, pageSize);
			Text t = null;
			float margin = 20;
			if(_firstPageOnly)
			{
				if(pageNumber > 1)
					return;
			}
			if(_logoWithText)
			{
				if(!_headerText.IsNullOrEmptyOrWitheSpaces())
				{
					// --- Left Text ---
					Paragraph leftText = new Paragraph(_headerText)
						.SetFontSize(12)
						.SetBold()
						.SetTextAlignment(TextAlignment.LEFT);

					canvas.ShowTextAligned(
						leftText,
						pageSize.GetLeft() + margin,
						pageSize.GetTop() - (_headerHeight / 2),
						TextAlignment.LEFT
					);
				}
				// --- Right Logo ---
				if(!string.IsNullOrWhiteSpace(_logoBase64))
				{
					byte[] imageBytes = Convert.FromBase64String(_logoBase64.Replace("data:image/png;base64,", ""));
					ImageData imgData = ImageDataFactory.Create(imageBytes);
					Image logo = new Image(imgData);

					// Resize logo to fit header height
					logo.SetAutoScale(false);
					logo.ScaleToFit(100, 100);

					float logoWidth = logo.GetImageScaledWidth();

					canvas.ShowTextAligned(
						new Paragraph().Add(logo),
						pageSize.GetRight() - margin - logoWidth / 2,
						pageSize.GetTop() - (_headerHeight / 2) + (-10),
						TextAlignment.CENTER
					);
				}
			}
			if(_logoWithCounter)
			{
				if(pageNumber == 1)
				{

					canvas = new iText.Layout.Canvas(pdfCanvas, new Rectangle(0, 0, pageSize.GetWidth(), 40));
					t = new Text("Seite " + pageNumber.ToString() + " von");
					Paragraph p = new Paragraph().Add(t);
					p.SetFontColor(ColorConstants.BLACK);
					p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
					canvas.ShowTextAligned(p, x + 330, yHeader, TextAlignment.RIGHT);
					pdfCanvas.AddXObjectAt(placeholderForFirstPage, x + 335, yHeader - descent);
					pdfCanvas.Release();
				}
				else
				{
					float imageWidth = 60;
					float imageHeight = 60;
					float imageX = pageSize.GetWidth() - imageWidth - x;
					x = 50;
					canvas = new iText.Layout.Canvas(pdfCanvas, new Rectangle(0, 0, pageSize.GetWidth(), 100));
					t = new Text("Seite/Page " + pageNumber.ToString() + " von/of ");
					Paragraph p = new Paragraph().Add(t);
					p.SetFontColor(ColorConstants.LIGHT_GRAY);
					p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
					p.SetWidth(pageSize.GetWidth() - 80);
					p.SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f));
					if(!string.IsNullOrWhiteSpace(_logoBase64))
					{
						var imageBytes = System.Convert.FromBase64String(_logoBase64.Replace("data:image/png;base64,", ""));
						Image img = new Image(ImageDataFactory.Create(imageBytes));
						img.SetAutoScale(false);
						img.ScaleToFit(imageWidth, imageHeight);
						img.SetRelativePosition(380, 1, 10, 100);
						p.Add(img);
					}

					canvas.ShowTextAligned(p, x, y, TextAlignment.LEFT);
					pdfCanvas.AddXObjectAt(placeholder, x + space, y - descent);
					pdfCanvas.Release();
				}
			}

		}
		public void WriteTotal(PdfDocument pdf)
		{
			Canvas canvas = new Canvas(placeholder, pdf);
			// Set font, font size, font color, and other text properties
			canvas.SetFontSize(PLACEHOLDER_SIZE)
				.SetFontColor(ColorConstants.LIGHT_GRAY)
				.SetTextAlignment(TextAlignment.CENTER).SetItalic();
			//x=20
			canvas.ShowTextAligned(pdf.GetNumberOfPages() + _documentTitle.ToString(), 20, descent, TextAlignment.LEFT);
			canvas.Close();

			Canvas canvasC2 = new Canvas(placeholderForFirstPage, pdf);
			canvasC2.SetFontSize(PLACEHOLDER_SIZE).SetFontColor(ColorConstants.BLACK).SetItalic();
			canvasC2.ShowTextAligned(pdf.GetNumberOfPages().ToString(), 0, descent, TextAlignment.LEFT);
			canvasC2.Close();
		}
	}
}
