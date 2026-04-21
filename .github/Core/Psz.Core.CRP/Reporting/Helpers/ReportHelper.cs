using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;
using ZXing.Common;

namespace Psz.Core.CRP.Reporting.Helpers
{
	public class ReportHelper
	{
		public static string GenerateBarcodeBase64(string value)
		{
			var writer = new BarcodeWriterPixelData
			{
				Format = BarcodeFormat.CODE_128,
				Options = new EncodingOptions
				{
					Height = 80,
					Width = 300,
					Margin = 1
				}
			};

			var pixelData = writer.Write(value);

			using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
			var bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format32bppRgb);

			Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
			bitmap.UnlockBits(bitmapData);

			using var ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			return Convert.ToBase64String(ms.ToArray());
		}
		public static byte[] MergePdfs(byte[] pdf1, byte[] pdf2)
		{
			using var outputStream = new MemoryStream();

			using var writer = new PdfWriter(outputStream);
			using var destPdf = new PdfDocument(writer);
			var merger = new PdfMerger(destPdf);

			using var reader1 = new PdfReader(new MemoryStream(pdf1));
			using var srcPdf1 = new PdfDocument(reader1);
			merger.Merge(srcPdf1, 1, srcPdf1.GetNumberOfPages());

			using var reader2 = new PdfReader(new MemoryStream(pdf2));
			using var srcPdf2 = new PdfDocument(reader2);
			merger.Merge(srcPdf2, 1, srcPdf2.GetNumberOfPages());

			destPdf.Close();
			return outputStream.ToArray();
		}
	}
}
