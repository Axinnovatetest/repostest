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

namespace Psz.Core.MaterialManagement.Reporting.Events
{
	public class HeaderEventHandler: IEventHandler
	{
		const int PLACEHOLDER_SIZE = 8;
		protected PdfFormXObject placeholder;
		protected PdfFormXObject placeholderForFirstPage;
		string complement = string.Empty;
		public Boolean isSpecialFirstPage = true;
		public string html;
		protected float side = 20;
		protected float x = 200;
		protected float firstPagePageNumberPosition = 30;
		protected float yHeader = 720;
		protected float y = 800;
		protected float space = 55f;
		protected float descent = 3;
		protected string image = "";
		protected float offsetYPageNumber = 0;
		protected bool setPages;
		protected bool LieferPlannung;
		public HeaderEventHandler(string html, string complement, string image = null, bool setPages = true, bool lieferPlannung = false)
		{
			this.complement = complement;
			this.html = html;
			this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
			this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
			this.image = image;
			if(string.IsNullOrWhiteSpace(this.image))
			{
				this.image = System.Convert.ToBase64String(Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst()?.Logo);
			}

			this.setPages = setPages;
			this.LieferPlannung = lieferPlannung;
		}
		public HeaderEventHandler(string html, string complement, Boolean isSpecialFirstPage, float offsetYPageNumber)
		{
			this.complement = complement;
			this.isSpecialFirstPage = isSpecialFirstPage;
			this.offsetYPageNumber = offsetYPageNumber;
			this.html = html;
			this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
			this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));

		}
		public void WriteTotal(PdfDocument pdf)
		{
			Canvas canvas = new Canvas(placeholder, pdf);
			// Set font, font size, font color, and other text properties
			canvas.SetFontSize(PLACEHOLDER_SIZE)
				.SetFontColor(ColorConstants.LIGHT_GRAY)
				.SetTextAlignment(TextAlignment.CENTER).SetItalic();
			//x=20
			canvas.ShowTextAligned(pdf.GetNumberOfPages() + complement.ToString(), 20, descent, TextAlignment.LEFT);
			canvas.Close();

			Canvas canvasC2 = new Canvas(placeholderForFirstPage, pdf);
			canvasC2.SetFontSize(PLACEHOLDER_SIZE).SetFontColor(ColorConstants.BLACK).SetItalic();
			canvasC2.ShowTextAligned(pdf.GetNumberOfPages().ToString(), 0, descent, TextAlignment.LEFT);
			canvasC2.Close();


		}
		public void HandleEvent(Event @currentEvent)
		{

			PdfDocumentEvent docEvent = (PdfDocumentEvent)@currentEvent;
			PdfDocument pdf = docEvent.GetDocument();
			iText.Kernel.Pdf.PdfPage page = docEvent.GetPage();
			int pageNumber = pdf.GetPageNumber(page);
			// do not add header in the first page
			//if(pageNumber>1)
			Canvas c = null;
			Text t = null;
			PdfCanvas canvas = null;
			iText.Kernel.Geom.Rectangle pageSize = docEvent.GetPage().GetPageSize();
			canvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(docEvent.GetPage());
			if(LieferPlannung)
			{
				if(pageNumber != 1)
					return;

				float imageWidth = 100;
				float imageHeight = 100;
				x = 50;
				c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 100));
				t = new Text("Übersicht Lieferplannung");
				t.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
				t.SetFontSize(14);
				Paragraph p = new Paragraph().Add(t);
				if(!string.IsNullOrWhiteSpace(image))
				{
					var imageBytes = System.Convert.FromBase64String(image.Replace("data:image/png;base64,", ""));
					Image img = new Image(ImageDataFactory.Create(imageBytes));
					img.SetAutoScale(false);
					img.ScaleToFit(imageWidth, imageHeight);
					img.SetRelativePosition(250, 20, 10, 100);
					p.Add(img);
				}
				p.SetFontColor(ColorConstants.BLACK);
				p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
				p.SetWidth(pageSize.GetWidth());
				c.ShowTextAligned(p, x, y, TextAlignment.LEFT);
				canvas.Release();
			}
			if(setPages)
			{
				if(pageNumber == 1)
				{

					c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 40));
					t = new Text("Seite " + pageNumber.ToString() + " von");
					Paragraph p = new Paragraph().Add(t);
					p.SetFontColor(ColorConstants.BLACK);
					p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
					c.ShowTextAligned(p, x + 330, yHeader, TextAlignment.RIGHT);
					canvas.AddXObjectAt(placeholderForFirstPage, x + 335, yHeader - descent);
					canvas.Release();
				}
				else
				{
					float imageWidth = 60;
					float imageHeight = 60;
					float imageX = pageSize.GetWidth() - imageWidth - x;
					x = 50;
					c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 100));
					t = new Text("Seite/Page " + pageNumber.ToString() + " von/of ");
					Paragraph p = new Paragraph().Add(t);
					p.SetFontColor(ColorConstants.LIGHT_GRAY);
					p.SetFontSize(PLACEHOLDER_SIZE).SetItalic();
					p.SetWidth(pageSize.GetWidth() - 80);
					p.SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f));
					if(!string.IsNullOrWhiteSpace(image))
					{
						var imageBytes = System.Convert.FromBase64String(image.Replace("data:image/png;base64,", ""));
						Image img = new Image(ImageDataFactory.Create(imageBytes));
						img.SetAutoScale(false);
						img.ScaleToFit(imageWidth, imageHeight);
						img.SetRelativePosition(380, 1, 10, 100);
						p.Add(img);
					}

					c.ShowTextAligned(p, x, y, TextAlignment.LEFT);
					canvas.AddXObjectAt(placeholder, x + space, y - descent);
					canvas.Release();
				}
			}
			// Create placeholder object to write number of pages
			canvas.Release();
			if(setPages)
			{
				c.Flush();
				c.Close();
			}
		}
	}
}