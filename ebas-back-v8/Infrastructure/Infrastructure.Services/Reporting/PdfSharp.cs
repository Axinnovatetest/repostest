using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Services.Reporting
{
	public static class PdfSharp
	{
		public static byte[] CombinePDFs(List<byte[]> srcPDFs)
		{
			using(var ms = new MemoryStream())
			{
				using(var resultPDF = new PdfDocument(ms))
				{
					foreach(var pdf in srcPDFs)
					{
						using(var src = new MemoryStream(pdf))
						{
							using(var srcPDF = PdfReader.Open(src, PdfDocumentOpenMode.Import))
							{
								for(var i = 0; i < srcPDF.PageCount; i++)
								{
									resultPDF.AddPage(srcPDF.Pages[i]);
								}
							}
						}
					}
					resultPDF.Save(ms);
					return ms.ToArray();
				}
			}
		}
	}
}
