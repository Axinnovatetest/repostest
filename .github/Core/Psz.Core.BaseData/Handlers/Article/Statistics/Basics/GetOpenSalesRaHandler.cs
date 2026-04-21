using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;

	public class GetOpenSalesRaHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		private UserModel _user { get; set; }
		public GetOpenSalesRaHandler(UserModel user)
		{
			this._user = user;
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

				return ResponseModel<byte[]>.SuccessResponse(GetData());
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

		public byte[] GetData()
		{
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetOpenSalesRa();
			if(statisticsEntities != null && statisticsEntities.Count > 0)
			{
				return SaveToExcelFile(statisticsEntities);
			}

			return null;
		}

		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_SalesRa> data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"data-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Sales-RA");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

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
					worksheet.Cells[1, 1].Value = $"Open Sales RA - {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Article Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Rahmen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Rahmen_Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Rahmenmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Rahmenauslauf";

					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Rahmen 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Rahmen_Nr 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Rahmenmenge 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Rahmenauslauf 2";

					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Lieferant";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var w in data)
					{
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Rahmen;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Rahmen_Nr;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Rahmenmenge;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Rahmenauslauf;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Rahmen2;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Rahmen_Nr2;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Rahmenmenge2;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Rahmenauslauf2;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Einkaufspreis;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Name1;

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
					package.Workbook.Properties.Title = $"Open Sales RA Articles";
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
