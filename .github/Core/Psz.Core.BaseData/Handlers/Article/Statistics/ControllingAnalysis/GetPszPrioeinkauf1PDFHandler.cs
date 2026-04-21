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
	using System.Linq;

	public class GetPszPrioeinkauf1PDFHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel _data { get; set; }
		public GetPszPrioeinkauf1PDFHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetPszPrioeinkauf_report1(this._data.SearchTerms, this._data.RequestedPage, this._data.ItemsPerPage)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report1>();

				// - 
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetPszPrioeinkauf_report1_count(this._data.SearchTerms);

				//- 
				Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel responseBody = new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel();

				responseBody.Title = $"Keine AB vorhanden";
				responseBody.AllCount = allCount;
				responseBody.AllPagesCount = (int)Math.Ceiling((decimal)allCount / this._data.ItemsPerPage);
				responseBody.ItemsPerPage = this._data.ItemsPerPage;
				responseBody.RequestedPage = this._data.RequestedPage;
				responseBody.Suppliers = statisticsEntities
					.DistinctBy(x => new { Name1 = x.Name1.ToLower(), x.Telefon, x.Fax })
					.Select(x => new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseData.Supplier
					{
						Name1 = x.Name1,
						Telefon = x.Telefon,
						Fax = x.Fax
					})
					.OrderBy(x => x.Name1.ToLower())?.ToList();

				for(int i = 0; i < responseBody.Suppliers.Count; i++)
				{
					responseBody.Suppliers[i].Items = statisticsEntities
					.Where(x =>
						responseBody.Suppliers[i].Name1.ToLower() == x.Name1.ToLower()
						&& responseBody.Suppliers[i].Telefon == x.Telefon
						&& responseBody.Suppliers[i].Fax == x.Fax)
					.Select(x => new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseData.Item
					{
						Datum = x.Datum?.ToString("dd.MM.yyyy"),
						Bestellung_Nr = x.Bestellung_Nr,
						Lagerort_id = x.Lagerort_id,
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						Bezeichnung_1 = x.Bezeichnung_1,
						Liefertermin = x.Liefertermin?.ToString("dd.MM.yyyy"),
						Name1 = x.Name1
					})?.OrderBy(x => x.Name1.ToLower())?.ThenBy(x => x.Datum)?.ToList();
				}

				// - 
				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>.SuccessResponse();
		}

		public static byte[] GetData(Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel _data = null)
		{
			if(_data == null)
			{
				return SaveToPdfFile(
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetPszPrioeinkauf_report1());
			}
			else
			{
				// - 
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetPszPrioeinkauf_report1_count(_data.SearchTerms);
				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetPszPrioeinkauf_report1(_data.SearchTerms, _data.RequestedPage, allCount)
					   ?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report1>();


				//- 
				Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel responseBody = new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel();
				responseBody.Title = $"Keine AB vorhanden";
				responseBody.AllCount = allCount;
				responseBody.AllPagesCount = (int)Math.Ceiling((decimal)allCount / _data.ItemsPerPage);
				responseBody.ItemsPerPage = _data.ItemsPerPage;
				responseBody.RequestedPage = _data.RequestedPage;
				responseBody.Suppliers = statisticsEntities
					.DistinctBy(x => new { Name1 = x.Name1.ToLower(), x.Telefon, x.Fax })
					.Select(x => new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseData.Supplier
					{
						Name1 = x.Name1,
						Telefon = x.Telefon,
						Fax = x.Fax
					})
					.OrderBy(x => x.Name1.ToLower())?.ToList();

				for(int i = 0; i < responseBody.Suppliers.Count; i++)
				{
					responseBody.Suppliers[i].Items = statisticsEntities
					.Where(x =>
						responseBody.Suppliers[i].Name1.ToLower() == x.Name1.ToLower()
						&& responseBody.Suppliers[i].Telefon == x.Telefon
						&& responseBody.Suppliers[i].Fax == x.Fax)
					.Select(x => new Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseData.Item
					{
						Datum = x.Datum?.ToString("dd.MM.yyyy"),
						Bestellung_Nr = x.Bestellung_Nr,
						Lagerort_id = x.Lagerort_id,
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						Bezeichnung_1 = x.Bezeichnung_1,
						Liefertermin = x.Liefertermin?.ToString("dd.MM.yyyy"),
						//Bestatigter_Termin = x.Bestatigter_Termin?.ToString("dd.MM.yyyy"),
						Name1 = x.Name1
					})?.OrderBy(x => x.Name1.ToLower())?.ThenBy(x => x.Datum)?.ToList();
				}

				// - 
				return SaveToXlsFile(responseBody);
			}
		}

		internal static byte[] SaveToPdfFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_PszPrioeinkauf_report1> dataEntities)
		{
			try
			{
				if(dataEntities == null || dataEntities.Count <= 0)
					return null;

				var data = new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsPszPrio1DataModel();

				// -
				data.ReportData = new List<Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsPszPrio1DataModel.Header> {
					new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsPszPrio1DataModel.Header {Title= $"Keine AB vorhanden" }
				};

				// - Suppliers 
				data.Suppliers = dataEntities.Select(x => new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsPszPrio1DataModel.Supplier
				{
					Name1 = x.Name1,
					Telefon = x.Telefon,
					Fax = x.Fax
				}).Distinct()
				//?.Take(Math.Min(2, dataEntities?.Count ?? 0))
				?.OrderBy(x => x.Name1)?.ToList();

				// - Items
				data.Items = dataEntities
					//.Where(x=> data.Suppliers.Exists(y=> y.Name1 == x.Name1))
					.Select(x => new Infrastructure.Services.Reporting.Models.BSD.Articles.StatisticsPszPrio1DataModel.Item
					{
						Datum = x.Datum?.ToString("dd.MM.yyyy"),
						Bestellung_Nr = x.Bestellung_Nr,
						Lagerort_id = x.Lagerort_id,
						Anzahl = x.Anzahl,
						Artikelnummer = x.Artikelnummer,
						Bezeichnung_1 = x.Bezeichnung_1,
						Liefertermin = x.Liefertermin?.ToString("dd.MM.yyyy"),
						Name1 = x.Name1
					})?.OrderBy(x => x.Name1)?.ThenBy(x => x.Datum)?.ToList();

				// -
				return Module.ReportingService.Generate_BSD_ArticlesStatisticsPszPrioeinkauf1(Infrastructure.Services.Reporting.Helpers.ReportType.BSD_ART_STATS_PSZPRIO1, data);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		internal static byte[] SaveToXlsFile(Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"PszPrioeinkauf1-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Keine AB vorhanden");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 7;

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
					worksheet.Cells[1, 1].Value = $"Keine AB vorhanden";
					worksheet.Cells[1, 1].Style.Font.Size = 16;


					var supplierHeaderRows = new List<int>();
					var itemsHeaderRows = new List<int>();
					// Start adding the header
					var dataEntities = data.Suppliers;
					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							// Start adding the header
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = "Name:";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = "Telefon:";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Telefon;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = "Fax:";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Fax;
							supplierHeaderRows.Add(rowNumber);

							// - data
							int n = 1;
							worksheet.Cells[rowNumber + n, startColumnNumber + 0].Value = "Datum";
							worksheet.Cells[rowNumber + n, startColumnNumber + 1].Value = "Bestellung-Nr";
							worksheet.Cells[rowNumber + n, startColumnNumber + 2].Value = "Lagerort";
							worksheet.Cells[rowNumber + n, startColumnNumber + 3].Value = "Anzahl";
							worksheet.Cells[rowNumber + n, startColumnNumber + 4].Value = "Artikelnummer";
							worksheet.Cells[rowNumber + n, startColumnNumber + 5].Value = "Bezeichnung";
							worksheet.Cells[rowNumber + n, startColumnNumber + 6].Value = "Liefertermin";
							//worksheet.Cells[rowNumber + n, startColumnNumber + 7].Value = "Bestätiger Termin";
							itemsHeaderRows.Add(rowNumber + n);
							n++;
							foreach(var d in w?.Items)
							{
								worksheet.Cells[rowNumber + n, startColumnNumber + 0].Value = d?.Datum;
								worksheet.Cells[rowNumber + n, startColumnNumber + 1].Value = d?.Bestellung_Nr;
								worksheet.Cells[rowNumber + n, startColumnNumber + 2].Value = d?.Lagerort_id;
								worksheet.Cells[rowNumber + n, startColumnNumber + 3].Value = d?.Anzahl;
								worksheet.Cells[rowNumber + n, startColumnNumber + 4].Value = d?.Artikelnummer;
								worksheet.Cells[rowNumber + n, startColumnNumber + 5].Value = d?.Bezeichnung_1;
								worksheet.Cells[rowNumber + n, startColumnNumber + 6].Value = d?.Liefertermin;
								//worksheet.Cells[rowNumber + n, startColumnNumber + 7].Value = d?.Bestatigter_Termin;
								n++;
							}
							// -

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1 + n;
						}
					}

					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

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

						foreach(var r in supplierHeaderRows)
						{
							using(var range = worksheet.Cells[r, startColumnNumber, r, numberOfColumns])
							{
								range.Style.Font.Bold = true;
								range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
								range.Style.Font.Color.SetColor(Color.Black);
								range.Style.ShrinkToFit = false;
							}
						}

						foreach(var r in itemsHeaderRows)
						{
							//Formatter // - [FromRow, FromCol, ToRow, ToCol]
							using(var range = worksheet.Cells[r, startColumnNumber, r, numberOfColumns])
							{
								range.Style.Font.Bold = true;
								range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#dce0e6"));
								range.Style.Font.Color.SetColor(Color.Black);
								range.Style.ShrinkToFit = false;
							}
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
					package.Workbook.Properties.Title = $"Keine AB vorhanden";
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
