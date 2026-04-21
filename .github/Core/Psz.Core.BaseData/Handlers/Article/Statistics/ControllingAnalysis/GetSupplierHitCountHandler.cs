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
	using System.Threading.Tasks;

	public class GetSupplierHitCountHandler: IHandleAsync<UserModel, ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.SupplierHitCountRequestModel _data { get; set; }
		public GetSupplierHitCountHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.SupplierHitCountRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public async Task<ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var results = await Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSupplierHitCounts(this._data.ArticleNumber, this._data.DateFrom, this._data.DateTo, this._data.RequestedPage, this._data.PageSize);
				var allCount = await Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetSupplierHitCounts_Count(this._data.ArticleNumber, this._data.DateFrom, this._data.DateTo);

				//- 
				var responseBody = new Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / this._data.PageSize);
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new Models.Article.Statistics.ControllingAnalysis.SupplierHitCountItem(x))?.ToList();

				return await ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.SuccessResponseAsync(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				if(exception.Message.Contains("Timeout expired"))
					return await ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.FailureResponseAsync("Request timeout expired. Please reduce the date range.");
				else
					throw;
			}
		}
		public async Task<ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>> ValidateAsync()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return await ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.AccessDeniedResponseAsync();
			}

			//if (Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) == null)
			//    return ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.FailureResponse("Article not found");

			if(this._data.DateTo >= this._data.DateFrom && (this._data.DateTo - this._data.DateFrom).TotalDays > 370)
				return await ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.FailureResponseAsync("Date range too big");

			return await ResponseModel<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>.SuccessResponseAsync();
		}
		public async Task<byte[]> SaveToExcelFile()
		{
			try
			{
				var dateNow = DateTime.Now;
				this._data.PageSize = 10000;
				this._data.RequestedPage = 0;
				var data = await this.HandleAsync();
				var dataEntities = new List<Models.Article.Statistics.ControllingAnalysis.SupplierHitCountItem>();
				if(data.Success)
				{
					dataEntities = data.Body.Items;
				}

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Hitliste-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Hitliste");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 2;

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
					worksheet.Cells[1, 1].Value = $"Hitliste Article [{this._data.ArticleNumber}] from [{this._data.DateFrom.ToString("dd/MM/yyyy")}] to [{this._data.DateTo.ToString("dd/MM/yyyy")}]";
					worksheet.Cells[1, 1].Style.Font.Size = 16;
					worksheet.Column(1).AutoFit();


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Einkaufsvolumen";


					var rowNumber = headerRowNumber + 1;
					if(dataEntities != null && dataEntities.Count > 0)
					{
						// Loop through 
						foreach(var w in dataEntities)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Einkaufsvolumen.HasValue == true ? w?.Einkaufsvolumen.Value : 0m; // - worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = "#";

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
					package.Workbook.Properties.Title = $"Hitliste";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
					var _byteData = await File.ReadAllBytesAsync(filePath);

					var ddd = (DateTime.Now - dateNow).TotalMinutes;
					// - 2023-02-03 - send to user via email
					if(dataEntities != null && dataEntities.Count > 0 && (DateTime.Now - dateNow).TotalMinutes > 3 && this._user != null && Infrastructure.Services.Email.Helpers.IsValidEmail(this._user.Email))
					{
						Module.EmailingService.SendEmailAsync(
								"Lieferant Hitliste",
								   $"Lieferant Hitliste from {dateNow.ToString("dd/MM/yyyy HHmm")}.",
									new List<string> { this._user.Email },
									new List<KeyValuePair<string, Stream>> {
										new KeyValuePair<string, Stream> ("lieferantHitliste.xlsx", new MemoryStream(_byteData))
									});
					}
					return _byteData;
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
