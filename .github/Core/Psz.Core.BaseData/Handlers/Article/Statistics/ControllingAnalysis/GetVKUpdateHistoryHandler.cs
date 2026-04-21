using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class GetVKUpdateHistoryHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryRequestModel _data { get; set; }
		public GetVKUpdateHistoryHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var totalCount = Infrastructure.Data.Access.Tables.BSD.Tbl_Historie_VK_UpdateAccess.Get_Count(this._data.ArticleNumber,
						this._data.From, this._data.To);
				var results = Infrastructure.Data.Access.Tables.BSD.Tbl_Historie_VK_UpdateAccess.Get(
						new Infrastructure.Data.Access.Settings.SortingModel
						{
							SortDesc = this._data.SortDesc,
							SortFieldName = this._data.SortField
						},
						new Infrastructure.Data.Access.Settings.PaginModel
						{
							FirstRowNumber = this._data.RequestedPage * this._data.PageSize,
							RequestRows = this._data.isXLS ? totalCount : this._data.PageSize
						},
						this._data.ArticleNumber,
						this._data.From, this._data.To);

				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel>.SuccessResponse(
				   new Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel()
				   {
					   Items = results?.Select(x => new Models.Article.Statistics.ControllingAnalysis.VKUpdateHistory(x))?.ToList(),
					   PageSize = this._data.PageSize,
					   PageRequested = this._data.RequestedPage,
					   TotalCount = totalCount,
					   TotalPageCount = (int)Math.Ceiling((decimal)totalCount / this._data.PageSize)
				   });
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				this._data.isXLS = true;
				var response = this.Handle();
				var data = response.Success ? response.Body.Items : null;
				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"VKUpdate-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"VKUpdate");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 4;

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
					worksheet.Cells[1, 1].Value = $"VK-Update - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Alter Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Neuer Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Datum";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Alte_Preis;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Neue_Preis;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Datum;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

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
					package.Workbook.Properties.Title = $"VKUpdate - OUT";
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
