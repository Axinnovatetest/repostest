using Infrastructure.Data.Access.Tables;
using OfficeOpenXml;
using Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{

	public class GetMinStockUpdateHistoryHandler: IHandle<UserModel, ResponseModel<MinStockUpdateHistoryResponseModel>>
	{
		private UserModel _user { get; set; }
		private MinStockUpdateHistoryRequestModel _data { get; set; }
		public GetMinStockUpdateHistoryHandler(UserModel user, MinStockUpdateHistoryRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<MinStockUpdateHistoryResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				int totalCount = 0;


				var sortFieldName = "";
				switch(this._data.SortField.ToLower())
				{
					default:
					case "datum":
						sortFieldName = "[UpdateDate]";
						break;
					case "artikelnummer":
						sortFieldName = "[ArticleNumber]";
						break;
					case "lagerort":
						sortFieldName = "[LagerId]";
						break;
					case "altemindestbestand":
						sortFieldName = "[OldMinStock]";
						break;
					case "neuemindestbestand":
						sortFieldName = "[NewMinStock]";
						break;
					case "user":
						sortFieldName = "[UpdateUserName]";
						break;
				}

				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};

				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};


				var results = ArticleMinStockUpdateHistoryAccess.Get(
						dataSorting,
						dataPaging,
						this._data.ArticleNumber,
						this._data.From, this._data.To);

				totalCount = ArticleMinStockUpdateHistoryAccess.Get_Count(this._data.ArticleNumber,
						this._data.From, this._data.To);

				List<MinStockUpdateHistory> minStockHistoryList = results?.Select(x => new MinStockUpdateHistory(x))?.ToList();

				return ResponseModel<MinStockUpdateHistoryResponseModel>.SuccessResponse(
				   new MinStockUpdateHistoryResponseModel()
				   {
					   Items = minStockHistoryList,
					   PageSize = this._data.PageSize,
					   PageRequested = this._data.RequestedPage,
					   TotalCount = minStockHistoryList != null && minStockHistoryList.Count > 0 ? totalCount : 0,
					   TotalPageCount = minStockHistoryList != null && minStockHistoryList.Count > 0 ?
		this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)totalCount / this._data.PageSize)) : 0 : 0
				   });
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<MinStockUpdateHistoryResponseModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<MinStockUpdateHistoryResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<MinStockUpdateHistoryResponseModel>.SuccessResponse();
		}

		public byte[] GetDataXLS()
		{
			try
			{
				this._data.isXLS = true;
				var response = this.Handle();
				var data = response.Success ? response.Body.Items : null;
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"MinStockUpdate");

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
					worksheet.Cells[1, 1].Value = $"MinStockUpdate - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikel Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "alte Mindesbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Neue Mindesbestand";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.UpdateDate;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.ArticleNumber;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Lagerort;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.OldMinStock;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.NewMinStock;

								worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
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
					if(data != null && data.Count > 0)
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
					package.Workbook.Properties.Title = $"Min-Stock-Update - OUT";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

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
