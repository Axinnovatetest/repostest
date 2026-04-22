using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Drawing;
using System.Globalization;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<byte[]> GetHsitorieFAPlannungDraft(UserModel user)
		{
			if(user == null)
				return ResponseModel<byte[]>.AccessDeniedResponse();

			try
			{
				var response = GenerateEmptyDraft();
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal byte[] GenerateEmptyDraft()
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Historie FA Plannung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Historie FA Plannung");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 39;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					using(var range = worksheet.Cells[1, 1, 1, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Werk";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Planungsstatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Customer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Atribut";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Short";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Comment 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Comment 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "FA Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Shipped Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Open Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "PN PSZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Status TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Costs";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Shipped Qty Man";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Kommisioniert_teilweise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Kommisioniert_komplett";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Kabel_geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Kabel_geschnitten_Datum";

					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Termin Werk";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "KW";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "FA_Druckdatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "Wish Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 28].Value = "Bemerkung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 29].Value = "Gewerk_Teilweise_Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 30].Value = "Verpackungsart";
					worksheet.Cells[headerRowNumber, startColumnNumber + 31].Value = "Verpackungsmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 32].Value = "Losgroesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 33].Value = "Techniker";

					worksheet.Cells[headerRowNumber, startColumnNumber + 34].Value = "Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 35].Value = "Technik Kontakt TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 36].Value = "Status Intern";
					worksheet.Cells[headerRowNumber, startColumnNumber + 37].Value = "erstelldatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 38].Value = "Bemerkung_Kommissionierung_AL";

					var rowNumber = headerRowNumber + 1;

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Historie FA Plannung";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(9).Width = 50;
					worksheet.Column(10).Width = 50;
					worksheet.Column(29).Width = 50;
					// save our new workbook and we are done!
					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}