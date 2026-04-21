using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Reporting.Events
{
	public class HeaderEventHandler: IEventHandler
	{
		private readonly string _headerText;
		private readonly string _logoBase64;
		private readonly float _headerHeight;
		public HeaderEventHandler(string headerText, string logoBase64, float headerHeight = 60)
		{
			_headerText = headerText;
			_logoBase64 = logoBase64;
			_headerHeight = headerHeight;
		}
		public void HandleEvent(Event e)
		{
			PdfDocumentEvent docEvent = (PdfDocumentEvent)e;
			PdfPage page = docEvent.GetPage();
			Rectangle pageSize = page.GetPageSize();

			PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), docEvent.GetDocument());
			Canvas canvas = new Canvas(pdfCanvas, pageSize);

			float margin = 20;

			// --- Left Text ---
			Paragraph leftText = new Paragraph(_headerText)
				.SetFontSize(18)
				.SetBold()
				.SetTextAlignment(TextAlignment.LEFT);

			canvas.ShowTextAligned(
				leftText,
				pageSize.GetLeft() + margin,
				pageSize.GetTop() - (_headerHeight / 2),
				TextAlignment.LEFT
			);

			// --- Right Logo ---
			if(!string.IsNullOrWhiteSpace(_logoBase64))
			{
				byte[] imageBytes = Convert.FromBase64String(_logoBase64);
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

			canvas.Close();
		}
	}
}
