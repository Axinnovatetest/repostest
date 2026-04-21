using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;

	public class GetPlanningOrdersHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Technic.PlanningOrderRequestModel _data { get; set; }
		public GetPlanningOrdersHandler(UserModel user, Models.Article.Statistics.Technic.PlanningOrderRequestModel data)
		{
			this._user = user;
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

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Technic.GetPlanningOrder(this._data.Lager, this._data.EmployeeName)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PlanningOrder>();
				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(statisticsEntities));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.Technic_PlanningOrder> entities)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"PlanningOrder-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PlanningOrder");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 24;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"PlanningOrder - {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Lagerort_id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Sonderfertigung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Techniker";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "AB_Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Plan";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Termin_besprochen";

					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Offen Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Zeit in min pro Stück";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Prüfstatus TN Ware";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Status_intern";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Bemerkung Technik";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Info CS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Quick Area";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Kommisioniert teilweise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Kommisioniert komplett";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Kabel geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Kabel geschnitten Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "FA Gestartet";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Urs Artikelnummer";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var w in entities)
					{
						worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Lagerort_id;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Erstmuster?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Sonderfertigung?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Techniker;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.AB_Termin?.ToString("dd/MM/yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Plan?.ToString("dd/MM/yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Termin_besprochen;

						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.PSZ_;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Menge;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Offen_Anzahl;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.FA;
						worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Zeit_in_min_pro_Stuck;
						worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Status;
						worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Prufstatus_TN_Ware;
						worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Status_intern;
						worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Bemerkung_Technik;
						worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Info_CS;
						worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Quick_Area?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.Kommisioniert_teilweise?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Kommisioniert_komplett?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.Kabel_geschnitten?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Kabel_geschnitten_Datum?.ToString("dd/MM/yyyy");
						worksheet.Cells[rowNumber, startColumnNumber + 22].Value = w?.FA_Gestartet?.ToString();
						worksheet.Cells[rowNumber, startColumnNumber + 23].Value = w?.Urs_Artikelnummer;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

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
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"PlanningOrder";
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
