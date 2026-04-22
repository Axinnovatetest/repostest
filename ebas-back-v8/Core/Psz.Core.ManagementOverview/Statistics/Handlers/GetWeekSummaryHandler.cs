using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Drawing;


namespace Psz.Core.ManagementOverview.Statistics.Handlers
{
	public class GetWeekSummaryHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetWeekSummaryHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				// - 
				return ResponseModel<byte[]>.SuccessResponse(getData());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<byte[]>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal byte[] getData()
		{

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using(var package = new ExcelPackage())
			{
				#region Worksheet Budget
				AddWorksheetBudget(package);
				AddWorksheetRawMaterial(package);
				#endregion Worksheet Budget



				// Set some document properties
				package.Workbook.Properties.Title = "Stats KW";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";

				// save our new workbook and we are done!
				package.Save();
				return package.GetAsByteArray();
			}
		}

		private static void AddWorksheetBudget(ExcelPackage package)
		{
			var worksheet = package.Workbook.Worksheets.Add("Budget");
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 9;

			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Site";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "PO Nummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anfrager";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Abteilung";
			worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lieferant";
			worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Betrag TND";
			worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Betrag EUR";
			worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Monat";
			worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "KW";

			var data = Infrastructure.Data.Access.Joins.MGO.Statistics.GetBudgetSummary();
			var rowNumber = headerRowNumber + 1;
			if(data != null && data.Count > 0)
			{
				foreach(var p in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.Site;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.PO_Nummer;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Anfrager;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Abteilung;
					worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Lieferant;
					worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Betrag_TND;
					worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Betrag_EUR;
					worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Monat;
					worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.KW;

					rowNumber += 1;
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
		}
		private static void AddWorksheetRawMaterial(ExcelPackage package)
		{
			var worksheet = package.Workbook.Worksheets.Add("LagerGZ");
			var headerRowNumber = 1;
			var startColumnNumber = 1;
			var numberOfColumns = 2;

			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Lagerort";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestandswert (Summe EUR)";

			var data = Infrastructure.Data.Access.Joins.MGO.Statistics.GetRawMaterialStockValue_GZ();
			var rowNumber = headerRowNumber + 1;
			if(data != null && data.Count > 0)
			{
				foreach(var p in data)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.Lagerort;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Bestandswert__Summe_EUR_;

					rowNumber += 1;
				}
			}
			if(data != null && data.Count > 0)
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
		}
	}
}
