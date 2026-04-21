using OfficeOpenXml;
using Psz.Core.Apps.WorkPlan.Models.WorkSchedule;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public class ExportArticleWPLTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel user { get; set; }
		private int? countryId { get; set; }
		private int? hallId { get; set; }
		private decimal? minDiff { get; set; }

		public ExportArticleWPLTimeDiffHandler(Identity.Models.UserModel user, int? _countryId, int? _hallId, decimal? _minDiff)
		{
			this.user = user;

			countryId = _countryId;
			hallId = _hallId;
			minDiff = _minDiff;
		}

		public ResponseModel<byte[]> Handle()
		{
			lock(new object())
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, countryId, hallId, minDiff);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<byte[]> Perform(Identity.Models.UserModel user, int? _countryId, int? _hallId, decimal? _minDiff)
		{
			var articleEntities = (Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticleWPLTimeDiff(countryId: _countryId, hallId: _hallId, minDiff: _minDiff)
				   ?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleEntity>())
				   .Select(x => new ArticleWPLTimeDiffModel(x))?.ToList();

			return ResponseModel<byte[]>.SuccessResponse(getExcel(user.SelectedLanguage, articleEntities));
		}
		public ResponseModel<byte[]> Validate()
		{
			throw new NotImplementedException();
		}
		static byte[] getExcel(string selectedLanguage, List<ArticleWPLTimeDiffModel> articleTimeEntities)
		{
			selectedLanguage = selectedLanguage ?? "DE";
			string filePath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Articles - AP - {DateTime.Now.ToString("yyyy/MM/dd")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 1;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Blue;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(3).Height = 20;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[2, 2, 2, numberOfColumns + 1].Merge = true;
					worksheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

					worksheet.Cells[2, 2].Value = $"Articles - AP Time list [{DateTime.Now.ToString("yyy-MM-dd HH:mm:ss")}]";
					worksheet.Cells[3, 2].Value = $"";
					worksheet.Cells[3, 2, 3, numberOfColumns + 1].Merge = true;
					worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = GetHeaderName(ExcelColumnNumber.Artikelnummer, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Status_Extern].Value = GetHeaderName(ExcelColumnNumber.Status_Extern, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Status_Intern].Value = GetHeaderName(ExcelColumnNumber.Status_Intern, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Country].Value = GetHeaderName(ExcelColumnNumber.Country, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Hall].Value = GetHeaderName(ExcelColumnNumber.Hall, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.P3000_Vorgabezeit_min].Value = GetHeaderName(ExcelColumnNumber.P3000_Vorgabezeit_min, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.P3000_losgrosse].Value = GetHeaderName(ExcelColumnNumber.P3000_losgrosse, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Real_Losgrosse_der_letzten_5_FA].Value = GetHeaderName(ExcelColumnNumber.Real_Losgrosse_der_letzten_5_FA, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Stuck_in_min].Value = GetHeaderName(ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Stuck_in_min, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min].Value = GetHeaderName(ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min].Value = GetHeaderName(ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz_Time_pro_Losgrosse_P3000_vs_Prod].Value = GetHeaderName(ExcelColumnNumber.Differenz_Time_pro_Losgrosse_P3000_vs_Prod, selectedLanguage); //
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod].Value = GetHeaderName(ExcelColumnNumber.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod, selectedLanguage); //

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var fa in articleTimeEntities)
					{
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = fa.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Status_Extern].Value = fa.Status_Extern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Status_Intern].Value = fa.Status_Intern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Country].Value = fa.Country;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Hall].Value = fa.Hall;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.P3000_Vorgabezeit_min].Value = fa.P3000_Vorgabezeit_min;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.P3000_losgrosse].Value = fa.P3000_losgrosse;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Real_Losgrosse_der_letzten_5_FA].Value = fa.Real_Losgrosse_der_letzten_5_FA;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Stuck_in_min].Value = fa.Total_Operation_Time_laut_AP_pro_Stuck_in_min;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min].Value = fa.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min].Value = fa.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz_Time_pro_Losgrosse_P3000_vs_Prod].Value = fa.Differenz_Time_pro_Losgrosse_P3000_vs_Prod;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod].Value = fa.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// Doc content
					if(articleTimeEntities != null && articleTimeEntities.Count > 0)
					{
						using(var range = worksheet.Cells[2, 2, rowNumber - 1, numberOfColumns + 1])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					// Pre + Header
					using(var range = worksheet.Cells[2, 2, headerRowNumber, numberOfColumns + 1])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}


					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Articles - Work Plan Time";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
				}
				// -
				return File.ReadAllBytes(filePath);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

		public enum ExcelColumnNumber
		{
			Artikelnummer = 1,
			Status_Extern = 2,
			Status_Intern = 3,
			Country = 4,
			Hall = 5,
			P3000_Vorgabezeit_min = 6,
			P3000_losgrosse = 7,
			Real_Losgrosse_der_letzten_5_FA = 8,
			Total_Operation_Time_laut_AP_pro_Stuck_in_min = 9,
			Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min = 10,
			Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min = 11,
			Differenz_Time_pro_Losgrosse_P3000_vs_Prod = 12,
			Differenz_Time_pro_Losgrosse_in_FA_vs_Prod = 13
		}
		public static string GetHeaderName(ExcelColumnNumber excelColumnNumber, string language)
		{
			language = language?.ToUpper();
			switch(excelColumnNumber)
			{
				case ExcelColumnNumber.Artikelnummer:
					if(language == "DE")
						return "Artikelnummer";
					if(language == "TN")
						return "Nr";
					if(language == "CZ")
						return "Nr";
					if(language == "AL")
						return "Nr";
					// English
					return "Number";
				case ExcelColumnNumber.Status_Extern:
					if(language == "DE")
						return "Status Extern";
					if(language == "TN")
						return "Status Extern";
					if(language == "CZ")
						return "Status Extern";
					if(language == "AL")
						return "Status Extern";
					// English
					return "Status Extern";
				case ExcelColumnNumber.Status_Intern:
					if(language == "DE")
						return "Status Intern";
					if(language == "TN")
						return "Status Intern";
					if(language == "CZ")
						return "Status Intern";
					if(language == "AL")
						return "Status Intern";
					// English
					return "Status Intern";
				case ExcelColumnNumber.Country:
					if(language == "DE")
						return "Land";
					if(language == "TN")
						return "Country ";
					if(language == "CZ")
						return "Country";
					if(language == "AL")
						return "Country";
					// English
					return "Country";
				case ExcelColumnNumber.Hall:
					if(language == "DE")
						return "Halle";
					if(language == "TN")
						return "Hall";
					if(language == "CZ")
						return "Hall";
					if(language == "AL")
						return "Hall";
					// English
					return "Hall";
				case ExcelColumnNumber.P3000_Vorgabezeit_min:
					if(language == "DE")
						return "P3000 Vorgabezeit (min)";
					if(language == "TN")
						return "P3000 Vorgabezeit (min)";
					if(language == "CZ")
						return "P3000 Vorgabezeit (min)";
					if(language == "AL")
						return "P3000 Vorgabezeit (min)";
					// English
					return "P3000 Vorgabezeit (min)";
				case ExcelColumnNumber.P3000_losgrosse:
					if(language == "DE")
						return "P3000 losgröße";
					if(language == "TN")
						return "P3000 losgröße";
					if(language == "CZ")
						return "P3000 losgröße";
					if(language == "AL")
						return "P3000 losgröße";
					// English
					return "P3000 losgröße";
				case ExcelColumnNumber.Real_Losgrosse_der_letzten_5_FA:
					if(language == "DE")
						return "Real Losgröße der letzten 5 FA";
					if(language == "TN")
						return "Real Losgröße der letzten 5 FA";
					if(language == "CZ")
						return "Real Losgröße der letzten 5 FA";
					if(language == "AL")
						return "Real Losgröße der letzten 5 FA";
					// English
					return "Real Losgröße der letzten 5 FA";
				case ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Stuck_in_min:
					if(language == "DE")
						return "Total Operation Time laut AP pro Stück (min)";
					if(language == "TN")
						return "Total Operation Time laut AP pro Stück (min)";
					if(language == "CZ")
						return "Total Operation Time laut AP pro Stück (min)";
					if(language == "AL")
						return "Total Operation Time laut AP pro Stück (min)";
					// English
					return "Total Operation Time laut AP pro Stück (min)";
				case ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_P3000_in_min:
					if(language == "DE")
						return "Total Operation Time laut AP pro Losgröße P3000 (min)";
					if(language == "TN")
						return "Total Operation Time laut AP pro Losgröße P3000 (min)";
					if(language == "CZ")
						return "Total Operation Time laut AP pro Losgröße P3000 (min)";
					if(language == "AL")
						return "Total Operation Time laut AP pro Losgröße P3000 (min)";
					// English
					return "Total Operation Time laut AP pro Losgröße (min)";
				case ExcelColumnNumber.Total_Operation_Time_laut_AP_pro_Losgrosse_in_FA_in_min:
					if(language == "DE")
						return "Total Operation Time laut AP pro Losgröße in FA (min)";
					if(language == "TN")
						return "Total Operation Time laut AP pro Losgröße in FA (min)";
					if(language == "CZ")
						return "Total Operation Time laut AP pro Losgröße in FA (min)";
					if(language == "AL")
						return "Total Operation Time laut AP pro Losgröße in FA (min)";
					// English
					return "Total Operation Time laut AP pro Losgröße in FA (min)";
				case ExcelColumnNumber.Differenz_Time_pro_Losgrosse_P3000_vs_Prod:
					if(language == "DE")
						return "Differenz Time pro Losgröße P3000 vs Vorgabezeit";
					if(language == "TN")
						return "Differenz Time pro Losgröße P3000 vs Vorgabezeit";
					if(language == "CZ")
						return "Differenz Time pro Losgröße P3000 vs Vorgabezeit";
					if(language == "AL")
						return "Differenz Time pro Losgröße P3000 vs Vorgabezeit";
					// English
					return "Differenz Time pro Losgröße P3000 vs Vorgabezeit";
				case ExcelColumnNumber.Differenz_Time_pro_Losgrosse_in_FA_vs_Prod:
					if(language == "DE")
						return "Differenz Time pro Losgröße in FA vs Vorgabezeit";
					if(language == "TN")
						return "Differenz Time pro Losgröße in FA vs Vorgabezeit";
					if(language == "CZ")
						return "Differenz Time pro Losgröße in FA vs Vorgabezeit";
					if(language == "AL")
						return "Differenz Time pro Losgröße in FA vs Vorgabezeit";
					// English
					return "Differenz Time pro Losgröße in FA vs Vorgabezeit";
				default:
					if(language == "DE")
						return "";
					if(language == "TN")
						return "";
					if(language == "CZ")
						return "";
					if(language == "AL")
						return "";
					// English
					return "";
			}

		}
	}
}
