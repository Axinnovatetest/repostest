using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using PdfSharp.Pdf.Content.Objects;
using Psz.Core.BaseData.Reporting.Events;
using Psz.Core.BaseData.Reporting.Models;
using RazorLight;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Reporting
{
	public class IText
	{
		public static async Task<byte[]> GetItextPDF(Reporting.Models.ITextHeaderFooterProps props)
		{
			string htmlFooter = string.Empty;
			ITextHeaderEventHandler headerHandler = null;
			ITextFooterEventHandler footerEventHandler = null;
			//body
			string bodyTemplatePath = @$"Reporting\PdfTemplates\{props.BodyTemplate}.cshtml";
			string bodyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), bodyTemplatePath);
			var bodyEngine = new RazorLightEngineBuilder()
			.UseFileSystemProject(System.IO.Path.GetDirectoryName(bodyPath))
			.UseMemoryCachingProvider()
			.Build();
			var htmlBody = await bodyEngine.CompileRenderAsync(System.IO.Path.GetFileName(bodyTemplatePath), props.BodyData);
			if(props.HasFooter)
			{
				var footerTemplatePath = $@"Reporting\PdfTemplates\{props.FooterTemplate}.cshtml";
				string footerPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), footerTemplatePath);
				var footerEngine = new RazorLightEngineBuilder()
			   .UseFileSystemProject(System.IO.Path.GetDirectoryName(footerPath))
			   .UseMemoryCachingProvider()
			   .Build();
				htmlFooter = await footerEngine.CompileRenderAsync(System.IO.Path.GetFileName(footerTemplatePath), props.FooterData);
			}
			using(var memoryStream = new MemoryStream())
			{
				using(iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream))
				{
					PdfDocument pdfDocument = new PdfDocument(writer);
					if(props.Rotate)
						pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());
					if(props.HasHeader)
					{
						headerHandler = new ITextHeaderEventHandler(props.HeaderText, props.Logo, props.HeaderLogoWithCounter, props.HeaderLogoWithText, props.DocumentTitle, props.HeaderFirstPageOnly);
						pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
					}
					if(props.HasFooter)
					{
						if(!string.IsNullOrEmpty(htmlFooter))
						{
							footerEventHandler = new ITextFooterEventHandler(htmlFooter, props.FooterWithCounter, props.FooterLeftText, props.FooterCenterText);
							pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, footerEventHandler);
						}
					}
					ConverterProperties converterProperties = new ConverterProperties();
					FontProvider fontProvider = new DefaultFontProvider();
					HtmlConverter.ConvertToDocument(htmlBody, pdfDocument, converterProperties);
					if(props.HeaderLogoWithCounter)
						headerHandler.WriteTotal(pdfDocument);
					if(props.FooterWithCounter)
						footerEventHandler.WriteTotal(pdfDocument);
					pdfDocument.Close();
					return memoryStream.ToArray();
				}
			}
		}
	}
}