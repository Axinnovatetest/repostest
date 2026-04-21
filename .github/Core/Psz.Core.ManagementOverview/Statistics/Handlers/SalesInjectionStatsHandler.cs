using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Statistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static Infrastructure.Services.Reporting.IText;

namespace Psz.Core.ManagementOverview.Statistics.Handlers
{


	public class SalesInjectionStatsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<SalesInjectionStatsResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private SalesInjectionStatsRequestModel _data { get; set; }

		public SalesInjectionStatsHandler(Identity.Models.UserModel user, SalesInjectionStatsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<SalesInjectionStatsResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					//return validationResponse;
				}
				///
				var reasonEntities = Infrastructure.Data.Access.Tables.Statistics.MGO.StatisticsAccess.GetSalesInjection(
					_data.From, _data.To);

				var responseBody = new List<SalesInjectionStatsResponseModel> { };
				if(reasonEntities is not null)
				{
					foreach(var item in reasonEntities)
					{
						responseBody.Add(new SalesInjectionStatsResponseModel
						{
							AnzahlErledigt = item.AnzahlErledigt,

							ArticleNummer = item.ArticleNummer,
							Datum = item.Datum.HasValue ? item.Datum.Value.ToShortDateString() : string.Empty,
							Produktionskosten = item.Produktionskosten,
							Ausdr = item.Ausdr,
							Bezeichnung = item.Bezeichnung,
							FertigungsNummer = item.FertigungsNummer,
							Menge = item.Menge,
							Originalanzahl = item.Originalanzahl,
							Preis = item.Preis,
							VK = item.VK,
							Produktionsbereich = item.Produktionsbereich
						});
					}
				}

				return ResponseModel<List<SalesInjectionStatsResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"UmsatzSpritzguss-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"UmsatzSpritzguss");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 9;

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
					worksheet.Cells[1, 1].Value = $"UmsatzSpritzguss {this._data.From}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items
					if(data.Success == true)
					{

						headerRowNumber = rowNumber + 1;
						// Start adding the header


						worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "FA-Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Menge";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anzahl Erledigt";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Verkaufspreis";
						worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Produktionskosten";
						worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Datum";
						worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Artikelnummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bezeichnung";
						worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Produktionsbereich";

						rowNumber = headerRowNumber + 1;
						if(data.Body != null && data.Body.Count > 0)
						{
							//Loop through
							foreach(var w in data.Body)
							{

								worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

								worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.FertigungsNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Menge;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.AnzahlErledigt;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.VK;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Produktionskosten;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Datum;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.ArticleNummer;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bezeichnung;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Produktionsbereich;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}
					#endregion items

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
					if(data.Body != null && data.Body.Count > 0)
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
					package.Workbook.Properties.Title = $"UmsatzSpritzguss";
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


		record PdfHelper
		{
			public string HTML { get; set; }
			public decimal TotalProductionCost { get; set; }
			public decimal TotalVKPreis { get; set; }

		}

		private PdfHelper createTable(List<SalesInjectionStatsResponseModel> inList)
		{
			string res = string.Empty;
			decimal TotalProductionCost = 0;
			decimal TotalVKPreis = 0;
			foreach(var line in inList)
			{
				string trLine = "<tr style='text-align:center'>";
				trLine += $"<td align='left'>{line.FertigungsNummer}</td>";
				trLine += $"<td align='left'>{line.ArticleNummer}</td>";
				trLine += $"<td align='right'>{line.Menge}</td>";
				trLine += $"<td align='right'>{line.Produktionskosten}</td>";
				trLine += $"<td align='right'>{line.VK}</td>";
				trLine += "</tr>";
				res += trLine;
				TotalProductionCost += line.Produktionskosten ?? 0;
				TotalVKPreis += line.VK ?? 0;
			}
			return new PdfHelper
			{
				HTML = res,
				TotalProductionCost = TotalProductionCost,
				TotalVKPreis = TotalVKPreis
			};
		}
		public byte[] GetPDFXLS()
		{
			try
			{
				byte[] responseBody = null;
				var data = this.Handle();
				string Table1Lines = string.Empty;
				string Table1TotalProduktionkosten = string.Empty;
				string Table1TotalVerkaufspreis = string.Empty;

				string Table2Lines = string.Empty;
				string Table2TotalProduktionkosten = string.Empty;
				string Table2TotalVerkaufspreis = string.Empty;

				string TotalVerkaufspreis = string.Empty;
				string TotalProduktionkosten = string.Empty;

				if(data.Success)
				{
					var t1 = createTable(data.Body.Where(x => x.Produktionsbereich == "Werkzeugbau").ToList());
					Table1Lines = t1.HTML;
					Table1TotalProduktionkosten = t1.TotalProductionCost.ToString("F2");
					Table1TotalVerkaufspreis = t1.TotalVKPreis.ToString("F2");

					var t2 = createTable(data.Body.Where(x => x.Produktionsbereich == "Spritzguß").ToList());
					Table2Lines = t2.HTML;
					Table2TotalProduktionkosten = t2.TotalProductionCost.ToString("F2");
					Table2TotalVerkaufspreis = t2.TotalVKPreis.ToString("F2");

					TotalProduktionkosten = (t1.TotalProductionCost + t2.TotalProductionCost).ToString("F2");
					TotalVerkaufspreis = (t1.TotalVKPreis + t2.TotalVKPreis).ToString("F2");
				}
				string body = HtmlPdfHelper.LoadTemplate("MGO", "SalesInjection", new List<PdfTag> {
					new PdfTag("<%Table1Lines%>", Table1Lines),
					new PdfTag("<%Table1Lines.TotalProduktionkosten%>", Table1TotalProduktionkosten),
					new PdfTag("<%Table1Lines.TotalVerkaufspreis%>", Table1TotalVerkaufspreis),

					new PdfTag("<%Table2Lines%>", Table2Lines),
					new PdfTag("<%Table2Lines.TotalProduktionkosten%>", Table2TotalProduktionkosten),
					new PdfTag("<%Table2Lines.TotalVerkaufspreis%>", Table2TotalVerkaufspreis),

					new PdfTag("<%TotalProduktionkosten%>", TotalProduktionkosten),
					new PdfTag("<%TotalVerkaufspreis%>", TotalVerkaufspreis),

					new PdfTag("<%from%>", this._data.From.ToShortDateString().Replace('/','.')),
					new PdfTag("<%to%>", this._data.To.ToShortDateString().Replace('/','.')),

					});
				responseBody = HtmlPdfHelper.Convert(body);
				return responseBody;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<List<SalesInjectionStatsResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<SalesInjectionStatsResponseModel>>.AccessDeniedResponse();
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<SalesInjectionStatsResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<SalesInjectionStatsResponseModel>>.SuccessResponse();
		}
	}
}
