using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFATechnickPlanungHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private string Techniker { get; set; }
		private string Produktionsort { get; set; }
		//private Identity.Models.UserModel _user { get; set; }
		public GetFATechnickPlanungHandler(string Techniker, string Produktionsort)
		{
			this.Techniker = Techniker;
			this.Produktionsort = Produktionsort;
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
				var techniker = Techniker == "null" ? "" : Techniker;
				var lager = Produktionsort == "null" ? "" : Produktionsort;
				var planungTechnik = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetTechnickPlannung(techniker, lager);

				var articleExtensionEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNr(planungTechnik?.Select(x=> x.ArtikelNr));

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(planungTechnik, articleExtensionEntities));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: Techniker:{Techniker},Produktionsort:{Produktionsort}");
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
		   List<Infrastructure.Data.Entities.Joins.FAPlannung.FATechnikPlanungEntity> technikPlannung, List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> articleExtensionEntities)
		{
			try
			{
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung-Technik");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 27;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Lagerort_id";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Sonderfertigung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Techniker";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "AB_Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Plan";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Termin besprochen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Offen_Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Zeit in min pro Stück";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Prüfstatus TN Ware";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Status intern";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Bemerkung_Technik";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Info CS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Quick_Area";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Kommisioniert_teilweise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Kommisioniert_komplett";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Kabel_geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Kabel_geschnitten_Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "FA_Gestartet";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Urs-Artikelnummer";

					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "First Sample time / PCE";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "Prototype time / PCE";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Serie time / PCE";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(technikPlannung != null && technikPlannung.Count > 0)
					{
						foreach(var p in technikPlannung)
						{
							//
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Erstmuster;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Sonderfertigung;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Techniker;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.AB_Termin;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Plan;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Termin_besprochen;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.PSZ;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Menge;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Offen_Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.FA;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Zeit_in_min_pro_Stück;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Status;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Prüfstatus_TN_Ware;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Status_intern;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.Bemerkung_Technik;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = p.Info_CS;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = p.Quick_Area;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = p.Kommisioniert_teilweise;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = p.Kommisioniert_komplett;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = p.Kabel_geschnitten;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = p.Kabel_geschnitten_Datum;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = p.FA_Gestartet;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = p.Urs_Artikelnummer;

							worksheet.Cells[rowNumber, startColumnNumber + 24].Value = articleExtensionEntities?.FirstOrDefault(x => x.ArticleNr == p.ArtikelNr && x.ArticleSalesType?.Trim()?.ToLower()== "first sample")?.Profuktionszeit; /* First Samole */
							worksheet.Cells[rowNumber, startColumnNumber + 25].Value = articleExtensionEntities?.FirstOrDefault(x => x.ArticleNr == p.ArtikelNr && x.ArticleSalesType?.Trim()?.ToLower() == "prototype")?.Profuktionszeit; /* Prototype */
							worksheet.Cells[rowNumber, startColumnNumber + 26].Value = articleExtensionEntities?.FirstOrDefault(x => x.ArticleNr == p.ArtikelNr && x.ArticleSalesType?.Trim()?.ToLower() == "serie")?.Profuktionszeit; /* Serie */

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(technikPlannung != null && technikPlannung.Count > 0)
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
					package.Workbook.Properties.Title = "Plannung Technik";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}