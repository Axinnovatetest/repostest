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

	public class GetAnalyseFibuHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.AnalyseFibuRequestModel _data { get; set; }
		public GetAnalyseFibuHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.AnalyseFibuRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>>.SuccessResponse(this.GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>>.SuccessResponse();
		}

		public List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel> GetData(Models.Article.Statistics.ControllingAnalysis.AnalyseFibuRequestModel data)
		{
			var responseBody = new List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>();
			if(data.IncludeDetails)
			{
				var invoices = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetAnalyseFibu(true, data.DateFrom, data.DateTo, data.CustomerNumber)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibu>();

				var credits = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetAnalyseFibu(false, data.DateFrom, data.DateTo, data.CustomerNumber)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibu>();


				var customers = new List<KeyValuePair<string, string>>();
				if(invoices.Count > 0)
					customers.AddRange(invoices.Select(x => new KeyValuePair<string, string>(x.Debitorennummer?.Trim(), x.Debitorenname?.Trim())).ToList());
				if(credits.Count > 0)
					customers.AddRange(credits.Select(x => new KeyValuePair<string, string>(x.Debitorennummer?.Trim(), x.Debitorenname?.Trim())).ToList());

				customers = customers?.DistinctBy(x => x.Key).OrderBy(x => x.Key).ToList();


				foreach(var c in customers)
				{
					var cData = new Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel
					{
						CustomerNumber = c.Key,
						CustomerName = c.Value,
						Invoices = invoices.Where(i => i.Debitorennummer.ToLower().Trim() == c.Key)
						?.Select(x => new Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel.AnalyseItem(x))?.ToList(),
						Credits = credits.Where(i => i.Debitorennummer.ToLower().Trim() == c.Key)
						?.Select(x => new Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel.AnalyseItem(x))?.ToList(),
					};

					cData.SumInvoices = cData.Invoices?.Sum(x => x.Gesamtkupferzuschlag ?? 0);
					cData.SumCredits = cData.Credits?.Sum(x => x.Gesamtkupferzuschlag ?? 0);

					// -
					responseBody.Add(cData);
				}
			}
			else
			{
				// - Headers ONLY
				var invoices = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetAnalyseFibuHeader(true, data.DateFrom, data.DateTo)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibuHeader>();

				var credits = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetAnalyseFibuHeader(false, data.DateFrom, data.DateTo)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CA_AnalyseFibuHeader>();


				var customers = new List<KeyValuePair<string, string>>();
				if(invoices.Count > 0)
					customers.AddRange(invoices.Select(x => new KeyValuePair<string, string>(x.Debitorennummer?.Trim(), x.Debitorenname?.Trim())).ToList());
				if(credits.Count > 0)
					customers.AddRange(credits.Select(x => new KeyValuePair<string, string>(x.Debitorennummer?.Trim(), x.Debitorenname?.Trim())).ToList());

				customers = customers?.DistinctBy(x => x.Key).OrderBy(x => x.Key).ToList();

				// - only row per Customer & RG Type should be returned
				foreach(var c in customers)
				{
					var inv = invoices.Where(i => i.Debitorennummer.ToLower().Trim() == c.Key)?.ToList();
					var crd = credits.Where(i => i.Debitorennummer.ToLower().Trim() == c.Key)?.ToList();
					var cData = new Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel
					{
						CustomerNumber = c.Key,
						CustomerName = c.Value,
						InvoicesCount = inv?.Count > 0 ? inv[0].RechnungsCount.Value : 0,
						SumInvoices = inv?.Count > 0 ? inv[0].Gesamtkupferzuschlag.Value : 0,
						CreditsCount = crd.Count > 0 ? crd[0].RechnungsCount.Value : 0,
						SumCredits = crd.Count > 0 ? crd[0].Gesamtkupferzuschlag.Value : 0,
					};

					// -
					responseBody.Add(cData);
				}
			}
			// -
			return responseBody;
		}
		public byte[] GetDataXLS()
		{
			try
			{
				// - retrieve RG data
				this._data.CustomerNumber = null;
				this._data.IncludeDetails = true;
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"FibuAnalyse-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Alle - Fibu Analyse");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 5;

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
					worksheet.Cells[1, 1].Value = $"Fibu Analyse - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					decimal totalCopperSurcharge = 0m;
					int totalRowNumber = headerRowNumber;
					worksheet.Cells[totalRowNumber, startColumnNumber + 0].Value = "Summe Gesamtkupferzuschlag (Rechnung + Gutschrift):";
					worksheet.Cells[totalRowNumber, startColumnNumber + 1].Value = totalCopperSurcharge;
					headerRowNumber++;
					headerRowNumber++;

					if(data.Success && data.Body != null && data.Body.Count > 0)
					{
						for(int i = 0; i < data.Body.Count; i++)
						{
							var customer = data.Body[i];

							// -
							worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Kunde: {customer.CustomerNumber}";
							worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = $"{customer.CustomerName}";
							worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Summe Gesamtkupferzuschlag (Rechnung + Gutschrift):";
							worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = (customer.SumInvoices ?? 0) + (customer.SumCredits ?? 0);

							// >>>>>> - Invoices
							if(customer.Invoices != null && customer.Invoices.Count > 0)
							{
								headerRowNumber++;
								worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Rechnung";


								// Start adding the header
								headerRowNumber++;
								worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Typ";
								worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "RG Datum";
								worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "RG Nummer";
								worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamtkupferzuschlag";


								var rowNumber = headerRowNumber + 1;
								// Loop through 
								foreach(var w in customer.Invoices)
								{
									worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Typ;
									worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Rechnungsdatum.HasValue == true ? w.Rechnungsdatum.Value : "";
									worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
									worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Rechnungsnummer;
									worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Gesamtkupferzuschlag;

									worksheet.Row(rowNumber).Height = 18;
									rowNumber += 1;
								}

								//-
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = "Summe (Gesamtkupferzuschlag):";
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = customer.SumInvoices;
								rowNumber += 1;
								headerRowNumber = rowNumber + 1;
								totalCopperSurcharge += (customer.SumInvoices ?? 0);
							}


							// >>>>>> - Credits
							if(customer.Credits != null && customer.Credits.Count > 0)
							{
								headerRowNumber++;
								worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Gutschrift";


								// Start adding the header
								headerRowNumber++;
								worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Typ";
								worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "RG Datum";
								worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "RG Nummer";
								worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamtkupferzuschlag";


								var rowNumber = headerRowNumber + 1;
								// Loop through 
								foreach(var w in customer.Credits)
								{
									worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Typ;
									worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Rechnungsdatum.HasValue == true ? w.Rechnungsdatum.Value : "";
									worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
									worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Rechnungsnummer;
									worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Gesamtkupferzuschlag;

									worksheet.Row(rowNumber).Height = 18;
									rowNumber += 1;
								}
								//-
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = "Summe (Gesamtkupferzuschlag):";
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = customer.SumCredits;
								rowNumber += 1;
								totalCopperSurcharge += (customer.SumCredits ?? 0);
							}

							// - Customer per Sheet
							addWorksheet(package, i, data.Body);
						}
					}

					// -
					worksheet.Cells[totalRowNumber, startColumnNumber + 1].Value = totalCopperSurcharge;

					////Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					//using (var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					//{
					//    range.Style.Font.Bold = true;
					//    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					//    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
					//    range.Style.Font.Color.SetColor(Color.Black);
					//    range.Style.ShrinkToFit = false;
					//}
					// Darker Blue in Top cell
					//worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					//if (data.Success == true)
					//{
					//    // Doc content
					//    if (data.Body != null && data.Body.Count > 0)
					//    {
					//        using (var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
					//        {
					//            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					//            range.Style.Fill.BackgroundColor.SetColor(Color.White);
					//            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					//        }
					//    }

					//    // - format DATE column
					//    int dateColumnNumber = 6;
					//    using (var range = worksheet.Cells[headerRowNumber + 1, dateColumnNumber, rowNumber - 1, dateColumnNumber])
					//    {
					//        range.Style.Numberformat.Format = "dd/MM/yyyy";
					//    }

					//    // - format DeliveryDate column
					//    dateColumnNumber = 8;
					//    using (var range = worksheet.Cells[headerRowNumber + 1, dateColumnNumber, rowNumber - 1, dateColumnNumber])
					//    {
					//        range.Style.Numberformat.Format = "dd/MM/yyyy";
					//    }

					//    // - format ConfirmDate column
					//    dateColumnNumber = 10;
					//    using (var range = worksheet.Cells[headerRowNumber + 1, dateColumnNumber, rowNumber - 1, dateColumnNumber])
					//    {
					//        range.Style.Numberformat.Format = "dd/MM/yyyy";
					//    }
					//}

					//// Thick countour
					//using (var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					//{
					//    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					//}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"FibuAnalyse";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();
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
		void addWorksheet(ExcelPackage package, int customerIdx, List<Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel> data)
		{
			if(data == null || data.Count <= customerIdx)
			{
				return;
			}

			// -
			var customer = data[customerIdx];

			// add a new worksheet to the empty workbook
			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{customer.CustomerNumber} - {truncateForSheetName(customer.CustomerName)}");

			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 2;
			var startColumnNumber = 1;
			var numberOfColumns = 5;

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
			worksheet.Cells[1, 1].Value = $"Fibu Analyse - {DateTime.Now.ToString("dd/MM/yyyy")}";
			worksheet.Cells[1, 1].Style.Font.Size = 16;


			decimal totalCopperSurcharge = 0m;
			int totalRowNumber = headerRowNumber;
			worksheet.Cells[totalRowNumber, startColumnNumber + 0].Value = "Gesamtkupferzuschlag:";
			worksheet.Cells[totalRowNumber, startColumnNumber + 1].Value = totalCopperSurcharge;
			headerRowNumber++;
			headerRowNumber++;
			// if (data.Success && data.Body != null && data.Body.Count > 0)
			{
				//for (int i = 0; i < data.Count; i++)
				{

					// -
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Kunde: {customer.CustomerNumber}";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = $"{customer.CustomerName}";

					// >>>>>> - Invoices
					if(customer.Invoices != null && customer.Invoices.Count > 0)
					{
						headerRowNumber++;
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Rechnung";


						// Start adding the header
						headerRowNumber++;
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Typ";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "RG Datum";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "RG Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamtkupferzuschlag";


						var rowNumber = headerRowNumber + 1;
						// Loop through 
						foreach(var w in customer.Invoices)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Typ;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Rechnungsdatum.HasValue == true ? w.Rechnungsdatum.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Rechnungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Gesamtkupferzuschlag;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						//-
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = "Summe (Gesamtkupferzuschlag):";
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = customer.SumInvoices;
						headerRowNumber = rowNumber + 1;
						totalCopperSurcharge += (customer.SumInvoices ?? 0);
					}


					// >>>>>> - Credits
					if(customer.Credits != null && customer.Credits.Count > 0)
					{
						headerRowNumber++;
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = $"Gutschrift";


						// Start adding the header
						headerRowNumber++;
						worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Typ";
						worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "RG Datum";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "RG Nummer";
						worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamtkupferzuschlag";


						var rowNumber = headerRowNumber + 1;
						// Loop through 
						foreach(var w in customer.Credits)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Typ;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Rechnungsdatum.HasValue == true ? w.Rechnungsdatum.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Rechnungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Gesamtkupferzuschlag;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
						//-
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = "Summe Gesamtkupferzuschlag (Rechnung + Gutschrift):";
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = customer.SumCredits;
						totalCopperSurcharge += (customer.SumCredits ?? 0);
					}
				}
			}
			// -
			worksheet.Cells[totalRowNumber, startColumnNumber + 1].Value = totalCopperSurcharge;

			// Fit the columns according to its content
			for(int i = 1; i <= numberOfColumns; i++)
			{
				worksheet.Column(i).AutoFit();
			}

		}
		string truncateForSheetName(string value, int maxLength = 21)
		{
			if(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value.Length < maxLength)
				return value?.Trim();

			return value.Substring(0, maxLength);
		}
	}
}
