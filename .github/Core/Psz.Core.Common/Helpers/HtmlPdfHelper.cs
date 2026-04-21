using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.Common.Helpers
{
	public record PdfTag
	{
		public string tag { get; set; }
		public string content { get; set; }
		public PdfTag(string tag, string content)
		{
			this.tag = tag;
			this.content = content;
		}
	}
	public class HtmlPdfHelper
	{
		const int PLACEHOLDER_SIZE = 8;
		private class FooterEventHandler: IEventHandler
		{
			public string html;
			public FooterEventHandler(string html)
			{
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
		private class HeaderEventHandler: IEventHandler
		{
			protected PdfFormXObject placeholder;
			protected PdfFormXObject placeholderForFirstPage;
			string complement = string.Empty;
			public string html;
			protected float side = 20;
			protected float x = 200;
			protected float firstPagePageNumberPosition = 30;
			protected float yHeader = 705;
			protected float y = 800;
			protected float space = 55f;
			protected float descent = 3;
			public HeaderEventHandler(string html, string complement)
			{
				this.complement = complement;
				this.html = html;
				this.placeholder = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
				this.placeholderForFirstPage = new PdfFormXObject(new Rectangle(0, 0, side * 10, side));
			}
			public void WriteTotal(PdfDocument pdf)
			{
				Canvas canvas = new Canvas(placeholder, pdf);
				// Set font, font size, font color, and other text properties
				canvas.SetFontSize(PLACEHOLDER_SIZE + 2)
					.SetFontColor(ColorConstants.DARK_GRAY)
					.SetTextAlignment(TextAlignment.CENTER).SetItalic();
				canvas.ShowTextAligned(pdf.GetNumberOfPages() + complement.ToString(), 0, descent, TextAlignment.LEFT);
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
					c = new iText.Layout.Canvas(canvas, new Rectangle(0, 0, pageSize.GetWidth(), 100));
					t = new Text("Seite/Page " + pageNumber.ToString() + " von/of ");
					Paragraph p = new Paragraph().Add(t);
					p.SetFontColor(ColorConstants.DARK_GRAY);
					p.SetFontSize(PLACEHOLDER_SIZE + 2).SetItalic();
					p.SetMarginLeft(pageSize.GetWidth() * 0.1f);
					p.SetWidth(pageSize.GetWidth() * 0.6f);
					p.SetTextAlignment(TextAlignment.CENTER);
					c.ShowTextAligned(p, x, y, TextAlignment.CENTER);
					canvas.AddXObjectAt(placeholder, x + space, y - descent);
					canvas.Release();
				}
				// Create placeholder object to write number of pages
				canvas.Release();

				c.Flush();
				c.Close();
			}
		}


		public static byte[] Convert(string htmlToConvert, string htmlHeader = null, string htmlFooter = null, string docName = "")
		{
			using(MemoryStream steam = new MemoryStream())
			{
				using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(steam))
				{
					PdfDocument pdfDocument = new PdfDocument(writer);
					HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName);
					pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
					if(!string.IsNullOrEmpty(htmlFooter))
					{
						pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
					}
					ConverterProperties converterProperties = new ConverterProperties();
					FontProvider fontProvider = new DefaultFontProvider();
					HtmlConverter.ConvertToDocument(htmlToConvert, pdfDocument, converterProperties);
					// Write the total number of pages to the placeholder
					headerHandler.WriteTotal(pdfDocument);
					pdfDocument.Close();
					return steam.ToArray();
				}
			}
		}

		public static byte[] Convert(List<string> htmlToConverts, List<string> htmlHeaders = null, List<string> htmlFooters = null, string docName = "")
		{
			using(MemoryStream steam = new MemoryStream())
			{
				using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(steam))
				{
					PdfDocument pdfDocument = new PdfDocument(writer);
					for(int i = 0; i < htmlToConverts.Count; i++)
					{
						string htmlToConvert = htmlToConverts[i];
						var htmlHeader = htmlHeaders[i];
						var htmlFooter = htmlFooters[i];

						// -
						HeaderEventHandler headerHandler = new HeaderEventHandler(htmlHeader, docName);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler(htmlFooter));
						}
						ConverterProperties converterProperties = new ConverterProperties();
						FontProvider fontProvider = new DefaultFontProvider();


						HtmlConverter.ConvertToDocument(htmlToConvert, pdfDocument, converterProperties);
						// Write the total number of pages to the placeholder
						headerHandler.WriteTotal(pdfDocument);
					}
					// - 
					pdfDocument.Close();
					return steam.ToArray();
				}
			}

		}
		public static string Template(string template, List<PdfTag> lsTags = null)
		{
			if(!string.IsNullOrEmpty(template))
			{
				string templatePath = @"Reporting\PdfTemplates\" + template + ".html";
				string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), templatePath);
				if(File.Exists(path))
				{
					template = File.ReadAllText(path);
					if(lsTags is not null)
					{
						foreach(PdfTag pdftag in lsTags)
						{
							template = template.Replace(pdftag.tag, pdftag.content);
						}
					}
				}

			}
			return template;
		}
	}
}