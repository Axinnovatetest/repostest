using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;

	public class GetVKIncludingCopperHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.VKIncludingCopperRequestModel _data { get; set; }
		public GetVKIncludingCopperHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.VKIncludingCopperRequestModel data)
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

				return ResponseModel<byte[]>.SuccessResponse(GetData(this._data));
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

		public static byte[] GetData(Models.Article.Statistics.ControllingAnalysis.VKIncludingCopperRequestModel data)
		{
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetVKIncludingCopper(data.ArticleNumber, data.DateFrom, data.DateTill);

			return getExcelData(data, statisticsEntities);
		}

		internal static byte[] getExcelData(Models.Article.Statistics.ControllingAnalysis.VKIncludingCopperRequestModel data,
			List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKIncludingCopper> dataEntities)
		{
			try
			{

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{data.ArticleNumber} VK - Preisliste Inkl. Kupfer");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 41;

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
					worksheet.Cells[1, 1].Value = $"[Artikel/Kreis: {data.ArticleNumber}\t von:{data.DateFrom.ToString("dd.MM.yyyy")}\t bis:{data.DateTill.ToString("dd.MM.yyyy")}] VK - Preisliste Inkl. Kupfer";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Cu Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "DEL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "DEL fixiert";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Gewicht (gr)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Hubmastleitungen";
					// -
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Index Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Index Kunde Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Sysmonummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Ursprungsland";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Jahresmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Jahresumsatz";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Kupferzuschlag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Materialkosten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Preiseinheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Stundensatz";

					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Preistyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Verkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "VK inkl Kupfer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Losgroesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "Bis Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "Produktionszeit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Produktionskosten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "Marge mit CU";
					worksheet.Cells[headerRowNumber, startColumnNumber + 28].Value = "DB I mit CU";
					worksheet.Cells[headerRowNumber, startColumnNumber + 29].Value = "Marge ohne CU";
					worksheet.Cells[headerRowNumber, startColumnNumber + 30].Value = "DB I ohne CU";

					worksheet.Cells[headerRowNumber, startColumnNumber + 31].Value = "VK_Festpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 32].Value = "Zolltarif_nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 33].Value = "Verpackungsmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 34].Value = "Warengruppe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 35].Value = "Std EDI";
					worksheet.Cells[headerRowNumber, startColumnNumber + 36].Value = "UBG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 37].Value = "Produktionslosgröße";
					worksheet.Cells[headerRowNumber, startColumnNumber + 38].Value = "Detail 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 39].Value = "Detail 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 40].Value = "Artikelfamilie";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bezeichnung_2;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bezeichnung_3;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Cu_Gewicht;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.DEL;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.DEL_fixiert;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Gewicht_in_gr;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Hubmastleitungen;
							// -
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Index_Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Index_Kunde_Datum;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Sysmonummer;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Ursprungsland;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Jahresmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Jahresumsatz;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Kupferzuschlag;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Materialkosten;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.Preiseinheit;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Stundensatz;

							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = w?.PriceType;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Price;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = w?.PriceInclCu;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = w?.LotSize;
							worksheet.Cells[rowNumber, startColumnNumber + 24].Value = w?.BisMenge;
							worksheet.Cells[rowNumber, startColumnNumber + 25].Value = w?.ProductionTime;
							worksheet.Cells[rowNumber, startColumnNumber + 26].Value = w?.ProductionCosts;
							worksheet.Cells[rowNumber, startColumnNumber + 27].Value = w?.MargeMitCu;
							worksheet.Cells[rowNumber, startColumnNumber + 28].Value = w?.DBMitCu;
							worksheet.Cells[rowNumber, startColumnNumber + 29].Value = w?.Marge;
							worksheet.Cells[rowNumber, startColumnNumber + 30].Value = w?.DB;

							worksheet.Cells[rowNumber, startColumnNumber + 31].Value = w?.VK_Festpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 32].Value = w?.Zolltarif_nr;
							worksheet.Cells[rowNumber, startColumnNumber + 33].Value = w?.Verpackungsmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 34].Value = w?.Warengruppe;
							worksheet.Cells[rowNumber, startColumnNumber + 35].Value = (w?.EdiDefault.HasValue == true && w?.EdiDefault.Value == true ? "Yes" : "No");
							worksheet.Cells[rowNumber, startColumnNumber + 36].Value = (w?.UBG.HasValue == true && w?.UBG.Value == true ? "Yes" : "No");
							worksheet.Cells[rowNumber, startColumnNumber + 37].Value = w?.ProductionLotSize;
							worksheet.Cells[rowNumber, startColumnNumber + 38].Value = w?.Artikelfamilie_Kunde_Detail1;
							worksheet.Cells[rowNumber, startColumnNumber + 39].Value = w?.Artikelfamilie_Kunde_Detail2;
							worksheet.Cells[rowNumber, startColumnNumber + 40].Value = w?.Artikelfamilie_Kunde;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
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
					using(var range = worksheet.Cells[1, 21, headerRowNumber, 31])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));
					}

					// Doc content
					if(dataEntities != null && dataEntities.Count > 0)
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
					package.Workbook.Properties.Title = $"{data} VK - Preisliste";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

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
