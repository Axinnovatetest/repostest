using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HeaderFG
{
	public class GetFGBestandHeaderDraftHandler:IHandle<UserModel, ResponseModel<byte[]>>
	{

		public UserModel _user { get; }
		public GetFGBestandHeaderDraftHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<byte[]> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				return ResponseModel<byte[]>.SuccessResponse(GenerateExcelFile());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(_user == null)
				return ResponseModel<byte[]>.AccessDeniedResponse();
			return ResponseModel<byte[]>.SuccessResponse();
		}
		public static byte[] GenerateExcelFile()
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"GetFGBestnadDraft-{DateTime.Now:yyyyMMddTHHmmss}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var headers = new[]
			{
				"Artikelnummer",
				"Kunde",
				"Bezeichnung 1",
				"Bezeichnung 2",
				"Freigabestatus",
				"CS Kontakt",
				"Lagerort",
				"Bestand",
				"VK Gesamt",
				"Kosten gesamt (mit CU)",
				"Kosten gesamt (ohne CU)",
				"VKE",
				"UBG",
				"STD EDI",
				"Datum"
			};

			using(var package = new ExcelPackage(file))
			{
				var worksheet = package.Workbook.Worksheets.Add("CRP FG_Bestnad");

				// Ajouter les en-têtes
				for(int i = 0; i < headers.Length; i++)
				{
					var cell = worksheet.Cells[1, i + 1];
					cell.Value = headers[i];
					cell.Style.Font.Bold = true;
					cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

					// Ajouter des bordures
					var border = cell.Style.Border;
					border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				}

				// Ajustement automatique des colonnes
				worksheet.Cells[1, 1, 1, headers.Length].AutoFitColumns();

				// Propriétés du document
				package.Workbook.Properties.Title = "FG Bestand Draft";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";

				package.Save();
			}

			var fileBytes = File.ReadAllBytes(filePath);
			File.Delete(filePath); // Nettoyage du fichier temporaire
			return fileBytes;
		}
	}
}
