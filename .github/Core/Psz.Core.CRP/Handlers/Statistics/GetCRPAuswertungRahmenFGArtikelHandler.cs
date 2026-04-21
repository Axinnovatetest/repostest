using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Drawing;
using System.Globalization;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<byte[]> GetCRPAuswertungRahmenFGArtikel(UserModel user)
		{
			if(user == null)
				return ResponseModel<byte[]>.AccessDeniedResponse();

			return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetCRPAuswertungRahmenFGArtikel()));

		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.CRP.CRPAuswertungRahmenFGArtikelEntity> _data)
		{
			try
			{

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"CRP-Auswertung Rahmen FG Artikel");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Type";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Originalmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Restmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Einzelpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Preis Restmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Enddatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Status";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(_data != null && _data.Count > 0)
					{
						foreach(var p in _data)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Rahmen;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Type;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Originalmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Restmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Einzelpreis + "€";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.PreisRestmenge + "€";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Enddatum.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Status;

							rowNumber += 1;
						}
					}
					// Doc content
					if(_data != null && _data.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Set some document properties
					package.Workbook.Properties.Title = "CRP-Auswertung Rahmen FG Artikel";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					for(int i = 1; i <= 7; i++)
					{
						worksheet.Column(i).Width = 25;
					}
					// save our new workbook and we are done!
					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}