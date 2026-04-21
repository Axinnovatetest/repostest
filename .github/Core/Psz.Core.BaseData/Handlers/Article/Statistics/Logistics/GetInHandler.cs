using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetInHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetInHandler(UserModel user, string number)
		{
			this._user = user;
			this._data = number;
		}
		public ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetIn(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Logistics.InResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data) == null)
				return ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<Models.Article.Statistics.Logistics.InResponseModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Logistics-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Logistics IN");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 16;

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
					worksheet.Cells[1, 1].Value = $"Logistics - IN - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Projekt Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Typ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Öffen/Aktuell";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Einheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Liefertermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Lieferant AB Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bestätigter Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Bemerkung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Bestellung Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Erledigt Pos";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gebucht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Fertigung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Rahmenbestellung";



					var rowNumber = headerRowNumber + 1;
					if(data.Success == true)
					{
						// Loop through 
						foreach(var w in data.Body)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ProjectNr;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Type;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = !string.IsNullOrWhiteSpace(w?.OpenCurrent) ? decimal.TryParse(w?.OpenCurrent, out var e) ? e : 0m : 0m;
							// - worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Unit;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Date.HasValue == true ? w.Date.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Name;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.DeliveryDate.HasValue == true ? w.DeliveryDate.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.SupplierABNr;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.ConfirmDate.HasValue == true ? w.ConfirmDate.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Comments;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.OrderNr;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.CompletePos?.Trim()?.ToLower() == "true" ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Booked?.Trim()?.ToLower() == "true" ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Production;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.FrameworkOrder?.Trim()?.ToLower() == "true" ? "Yes" : "No";

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

					if(data.Success == true)
					{
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
					package.Workbook.Properties.Title = $"Logistics - IN";
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
	}
}
