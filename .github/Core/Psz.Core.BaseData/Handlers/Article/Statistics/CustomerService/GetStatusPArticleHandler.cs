using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;

	public class GetStatusPArticleHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.CustomerService.StatusPArticleRequestModel _data { get; set; }
		public GetStatusPArticleHandler(UserModel user, Models.Article.Statistics.CustomerService.StatusPArticleRequestModel data)
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

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.CustomerService.GetStatusPArticle(this._data.EmployeeName, this._data.Status)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StatusPArticle>();
				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(statisticsEntities));
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
		internal static byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StatusPArticle> entities)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"StatusPArticle-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"StatusPArticle");

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
					worksheet.Cells[1, 1].Value = $"StatusPArticle - {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Kunde#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Index";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Index Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Freigabestatus";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var w in entities)
					{
						worksheet.Cells[rowNumber, startColumnNumber].Value = w?.CS_Kontakt;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Kunde;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.PSZ;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Kunde_;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Index;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Index_Datum;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Freigabestatus;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
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
					if(entities != null && entities.Count > 0)
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
					package.Workbook.Properties.Title = $"StatusPArticle";
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
