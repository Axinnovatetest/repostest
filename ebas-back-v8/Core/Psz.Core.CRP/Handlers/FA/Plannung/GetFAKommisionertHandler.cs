using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAKommisionertHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private DateTime _data { get; set; }
		public GetFAKommisionertHandler(DateTime data)
		{
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var kommisionertEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAKommisionert(_data);

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(kommisionertEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			//if (this._user == null/*
			//    || this._user.Access.____*/)
			//{
			//    return ResponseModel<byte[]>.AccessDeniedResponse();
			//}
			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAKommisionertEntity> produktionPlannung)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Kommisionert-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Kommisionert");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 10;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Geplanter Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Halle";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Teilweise kommisioniert";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "FA Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Erledigt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Artikelkurztext";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bemerkung";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						foreach(var p in produktionPlannung)
						{
							//
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Geplanter_Termin.HasValue ? p.Geplanter_Termin.Value.ToString("dd/MM/yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Halle;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Teilweise_kommisioniert.HasValue && p.Teilweise_kommisioniert.Value ? "YES" : "NO";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.FA_Menge;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Erledigt;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Artikelkurztext;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Bemerkung;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(produktionPlannung != null && produktionPlannung.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					worksheet.Column(10).Width = 100;
					// Set some document properties
					package.Workbook.Properties.Title = "Kommisionert";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

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