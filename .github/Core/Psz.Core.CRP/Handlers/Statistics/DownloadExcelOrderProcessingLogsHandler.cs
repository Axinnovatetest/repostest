using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService
	{
		public ResponseModel<byte[]> DownloadExcelOrderProcessingLogs(Identity.Models.UserModel user, OPSearchLogsModel data)
		{
			try
			{
				var validationResponse = this.ValidateDownloadExcelOrderProcessingLogs(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var LagerBetandFGEntity = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetOrderProcessingLogs(
					null,
					null, data.SearchValueVorfallNr, data.SearchValuePosition, data.SearchValueartikelnummer, data.SearchValueUsername
					, data.ListSearchType);
				var response = SaveToExcelFile(LagerBetandFGEntity);


				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidateDownloadExcelOrderProcessingLogs(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] SaveToExcelFile(
		List<Infrastructure.Data.Entities.Joins.CRP.OrderProcessingLogs> orderProcessingLogs)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Order Processing Logs-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Order Processing Logs");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Vorfall Nr.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dock Nr.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Pos";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "User";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Typ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Log";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Date Time";


					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(orderProcessingLogs != null && orderProcessingLogs.Count > 0)
					{
						foreach(var p in orderProcessingLogs)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.VorfallNr;
							//worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = "0"; // Set the format to display as integer without commas

							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.DokNr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Pos;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = "0";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.User;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.TYP;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Log;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (p.DateTime == null) ? "" : p.DateTime.Value.ToString("dd/MM/yyyy HH:mm:ss");
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							rowNumber += 1;
						}
					}
					// Doc content
					if(orderProcessingLogs != null && orderProcessingLogs.Count > 0)
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
					package.Workbook.Properties.Title = "Order Processing Logs";
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