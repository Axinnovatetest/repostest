using Infrastructure.Data.Access.Tables.CTS;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Psz.Core.CustomerService.Handlers.InsideSalesWerkstermin
{
	public class InsideSalesWerksterminUpdates: IInsideSalesWerksterminUpdates
	{
		public ResponseModel<List<InsideSalesWerkserminUpdatesModel>> getFAWithChangedWerkstermin(UserModel user, bool? insConfirmation)
		{
			try
			{
				#region Validations 
				if(user == null)
				{
					return ResponseModel<List<InsideSalesWerkserminUpdatesModel>>.AccessDeniedResponse();
				}

				#endregion

				var werksterminUpdatesFromDB = InsideSalesWerksterminUpdatesAccess.GetWerksterminHistoryByInsConfirmation(insConfirmation ?? false);

				if(werksterminUpdatesFromDB.Count > 0)
				{
					var werksterminUpdatesList = new List<InsideSalesWerkserminUpdatesModel>();
					foreach(var werkstermin in werksterminUpdatesFromDB)
					{
						var insideSalesWerkstermin = new InsideSalesWerkserminUpdatesModel();
						insideSalesWerkstermin.Id = werkstermin.Id;
						insideSalesWerkstermin.FertigungId = werkstermin.FertigungId;
						insideSalesWerkstermin.FertigungNumber = werkstermin.FertigungNumber;
						insideSalesWerkstermin.InsConfirmation = werkstermin.InsConfirmation;
						insideSalesWerkstermin.CustomerName = werkstermin.CustomerName;
						insideSalesWerkstermin.CustomerNumber = werkstermin.CustomerNumber;
						insideSalesWerkstermin.CustomerOrderNumber = werkstermin.CustomerOrderNumber;
						insideSalesWerkstermin.NewWorkDate = werkstermin.NewWorkDate;
						insideSalesWerkstermin.OldWorkDate = werkstermin.OldWorkDate;
						insideSalesWerkstermin.ReasonCapacityComments = werkstermin.ReasonCapacityComments;
						insideSalesWerkstermin.ReasonDefectiveComments = werkstermin.ReasonDefectiveComments;
						insideSalesWerkstermin.ReasonMaterialComments = werkstermin.ReasonMaterialComments;
						insideSalesWerkstermin.ReasonQualityComments = werkstermin.ReasonQualityComments;
						insideSalesWerkstermin.ReasonStatusP = werkstermin.ReasonStatusP;
						insideSalesWerkstermin.EditDate = werkstermin.EditDate;
						insideSalesWerkstermin.ArtikelNummer = werkstermin.ArticleNumber;
						insideSalesWerkstermin.ArticleId = werkstermin.ArticleId;

						werksterminUpdatesList.Add(insideSalesWerkstermin);
					}
					return ResponseModel<List<InsideSalesWerkserminUpdatesModel>>.SuccessResponse(werksterminUpdatesList);
				}

				return ResponseModel<List<InsideSalesWerkserminUpdatesModel>>.FailureResponse(null, new List<string> { "Werkstermin list is empty" });

			} catch(System.Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> UpdateInsConfirmation(InsConfirmationWerkterminModel model, UserModel user)
		{
			int result = 0;

			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(model != null)
			{
				result = InsideSalesWerksterminUpdatesAccess.UpdateInsByFertigungNummer(model.WerkterminId, model.InsConfirmation, user.Id, user.Username, DateTime.Now);
			}

			if(result > 0)
			{

				return ResponseModel<int>.SuccessResponse(result);
			}
			else
			{
				return ResponseModel<int>.FailureResponse("Error when updating INS confirmation");
			}
		}

		public ResponseModel<byte[]> getFAWithChangedWerkterminHistory_XLS(UserModel user, bool? insConfirmation)
		{
			try
			{
				var results = getFAWithChangedWerkstermin(user, insConfirmation);
				if(!results.Success)
				{
				}

				if(results.Body.Count <= 0)
				{
					return ResponseModel<byte[]>.FailureResponse("Empty data");
				}

				var _data = results.Body;
				// - 
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Werktermin Updates History");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 13;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Fertigung Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikel nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Customer Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Customer Order Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Ins Confirmation";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Material Reason";

					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Capacity Reason";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Defect Reason";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Quality Reason";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Status P Check";

					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Termin ALT";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Termin Neu";

					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Edit Date";


					var rowNumber = headerRowNumber;
					//Loop through
					foreach(var l in _data.OrderBy(a => a.CustomerName))
					{
						rowNumber++;
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.FertigungNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.ArtikelNummer;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.CustomerName;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.CustomerNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.InsConfirmation;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.ReasonMaterialComments;

						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.ReasonCapacityComments;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.ReasonDefectiveComments;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.ReasonQualityComments;

						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.ReasonStatusP;

						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.OldWorkDate.HasValue == true ? l.OldWorkDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.NewWorkDate.HasValue == true ? l.NewWorkDate.Value : "";

						worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.EditDate.HasValue == true ? l.NewWorkDate.Value : "";


						worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;


						worksheet.Row(rowNumber).Height = 18;
					}
					using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
						//worksheet.Column(i).Width = 15;
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Format headers
					worksheet.Row(1).Height = 15;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Set some document properties
					package.Workbook.Properties.Title = $"Werktermin-UpdatesHistory-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!


					return ResponseModel<byte[]>.SuccessResponse(package.GetAsByteArray());
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

	}
}
