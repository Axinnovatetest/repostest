using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<byte[]> GetForecastsDraft(UserModel user, int type)
		{
			try
			{
				var validationResponse = ValidateGetForecastDraft(user);
				if(!validationResponse.Success)
					return validationResponse;

				var response = type == (int)Enums.CRPEnums.ForcastDraftType.Hoch
					? GetHoshDraft()
					: GetQuerDraft();

				return ResponseModel<byte[]>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> ValidateGetForecastDraft(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GetHoshDraft()
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Forecast_Hosh-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"CRP Auswertung");

				var headerRowNumber = 1;
				var startColumnNumber = 1;
				var numberOfColumns = 4;

				worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Artikelnummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Material";
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Datum";
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Menge";

				worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

				// Set some document properties
				package.Workbook.Properties.Title = "Forecast Draft Hosh";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";

				// save our new workbook and we are done!
				package.Save();

				return File.ReadAllBytes(filePath);
			}
		}
		public static byte[] GetQuerDraft()
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Forecast_Quer-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"CRP Auswertung");

				var headerRowNumber = 1;
				var startColumnNumber = 1;
				var numberOfColumns = 4;

				worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Nummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Material";

				worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

				// Set some document properties
				package.Workbook.Properties.Title = "Forecast Draft Quer";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";

				// save our new workbook and we are done!
				package.Save();

				return File.ReadAllBytes(filePath);
			}
		}
	}
}
