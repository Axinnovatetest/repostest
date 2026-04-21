using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;
using System.Drawing;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<byte[]> GetExtraOrdersAuswertungExcel(UserModel user, ExtraOrdersNeedsInOtherPlantsRequestModel data)
		{
			if(user == null)
				return ResponseModel<byte[]>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetExtraOrdersAuswertungExcel(GetFertigungLager(data.Lager), data.Lager);
				var response = SaveToExcelFile(entities, data);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.PRS.ExtraOrdersAuswertungEntity> entities, ExtraOrdersNeedsInOtherPlantsRequestModel data)
		{
			try
			{
				var unit = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetById(data.Lager)?.Lagerort;
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Auwertung Extra orders-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Auwertung Extra orders");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 9;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Style.Font.Bold = true;
					worksheet.Row(2).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(2).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber - 1, startColumnNumber].Value = $"Werk: {unit}";
					worksheet.Cells[headerRowNumber - 1, startColumnNumber].Style.Font.Bold = true;
					//
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ-Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Stückpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Summe von Bestellungen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Wert ohne Bedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Summe Bedarf andere Werke";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Lagernummer des Werks";


					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(entities != null && entities.Count > 0)
					{
						foreach(var p in entities)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Stock ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Einkaufspreis ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.SumOrders ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.ValueWithoutRequirement ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.OtherPlantsRequirement ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.OtherPlants ?? "";
							rowNumber += 1;
						}
					}
					// Doc content
					if(entities != null && entities.Count > 0)
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
					using(var range = worksheet.Cells[2, 1, 2, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Set some document properties
					package.Workbook.Properties.Title = "Auswertung Extra orders";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					for(int i = 1; i <= 3; i++)
					{
						worksheet.Column(i).Width = 25;
					}
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