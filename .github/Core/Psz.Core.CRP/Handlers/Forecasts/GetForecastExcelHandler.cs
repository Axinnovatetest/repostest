using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<byte[]> GetForecastsExcel(UserModel user, CRPExcelRequestModel data)
		{
			try
			{
				var validationRespnse = ValidateGetForcastsExcel(user);
				if(!validationRespnse.Success)
					return validationRespnse;

				int? kundennummer = null;
				if(data.KundenNr is not null)
				{
					var adressen = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(data.KundenNr ?? -1);
					kundennummer = adressen.Kundennummer;
				}
				var entities = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetCRPDataForExcel(kundennummer, data.TypeId, data.OnlyLastVersion);
				return ResponseModel<byte[]>.SuccessResponse(GetCRPExcel(entities));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidateGetForcastsExcel(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetCRPExcel(List<Infrastructure.Data.Entities.Joins.CRP.ForecastsExcelEntity> data)
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"CRP_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"CRP Auswertung");

				var headerRowNumber = 1;
				var startColumnNumber = 1;
				var numberOfColumns = 11;

				worksheet.Row(1).Style.Font.Bold = true;
				worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

				worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dockument-Art.";
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bedarfstermin";
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichung 1=Kundenartikelnummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Menge";
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Jahr";
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "KW";
				worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VKE";
				worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gesamtpreis";
				worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Datum";

				var rowNumber = headerRowNumber + 1;
				if(data != null && data.Count > 0)
				{
					foreach(var p in data)
					{
						worksheet.Cells[rowNumber, startColumnNumber].Value = p.Kunden;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Type;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = (p.Bedarfstermin == null) ? "" : p.Bedarfstermin.Value.ToString("dd/MM/yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Material;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Menge;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Jahr;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.KW;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.VKE;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.GesamtPreis;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = (p.Datum == null) ? "" : p.Datum.Value.ToString("dd/MM/yyyy");
						rowNumber += 1;
					}
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
				package.Workbook.Properties.Title = "CRP Auswertung";
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
		}
	}
}