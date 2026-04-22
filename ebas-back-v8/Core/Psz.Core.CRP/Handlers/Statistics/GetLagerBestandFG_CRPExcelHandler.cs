using OfficeOpenXml;
using Psz.Core.Common.Models;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<byte[]> GetLagerBestandFG_CRPExcel(Identity.Models.UserModel user)
		{
			try
			{
				var validationResponse = this.ValidateGetLagerBestandFG_CRPExcel(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var LagerBetandFGEntity = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetLagerBestandFG_CRP();
				var response = SaveToExcelFile(LagerBetandFGEntity);


				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidateGetLagerBestandFG_CRPExcel(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.CRP.LagerBestandFG_CRPEntity> lagerBestandFG_CRPEntity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Lagerbestand FG-CRP-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Lagerbestand FG-CRP");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 16;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "VK Gesamt.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kosten gesamt (mit CU)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Kosten gesamt (ohne CU)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "VKE";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "UBG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Std EDI";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "BemerkungCRP";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(lagerBestandFG_CRPEntity != null && lagerBestandFG_CRPEntity.Count > 0)
					{
						foreach(var p in lagerBestandFG_CRPEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Bezeichnung_2;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.CS_Kontakt;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Lagerort;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Mindestbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.VK_Gesamt;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Kosten_gesamt;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Kosten_gesamt_ohne_cu;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.VKE;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.UBG ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.EdiDefault ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.BemerkungCRP;
							rowNumber += 1;
						}
					}
					// Doc content
					if(lagerBestandFG_CRPEntity != null && lagerBestandFG_CRPEntity.Count > 0)
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
					package.Workbook.Properties.Title = "Lagerbestand FG-CRP";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					for(int i = 1; i <= 7; i++)
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