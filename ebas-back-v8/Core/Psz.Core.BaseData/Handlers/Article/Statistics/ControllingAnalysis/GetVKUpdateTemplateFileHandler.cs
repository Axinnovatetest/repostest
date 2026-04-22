using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using OfficeOpenXml;
	using System.Drawing;

	public class GetVKUpdateTemplateFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>>
	{
		public GetVKUpdateTemplateFileHandler()
		{
		}

		public ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>.SuccessResponse(new Models.Article.BillOfMaterial.ImportFileTemplateModel
				{
					CreationTime = DateTime.Now,
					FileData = getTemplateFile(), // System.IO.File.ReadAllBytes(Module.AppSettings.ArticlesStatisticsVKUpdate),
					FileName = "",//System.IO.Path.GetFileName(Module.AppSettings.ArticlesStatisticsVKUpdate),
					FileExtension = ".xlsx", //System.IO.Path.GetExtension(Module.AppSettings.ArticlesStatisticsVKUpdate)
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel> Validate()
		{
			return ResponseModel<Models.Article.BillOfMaterial.ImportFileTemplateModel>.SuccessResponse();
		}
		byte[] getTemplateFile()
		{
			try
			{

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"VK - Update");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 32;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "P1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bis 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "P2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bis 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "P3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bis 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "P4";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "MOQ Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Lieferzeit Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Arbeitszeit Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "MOQ Nullserie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Lieferzeit Nullserie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Arbeitszeit Nullserie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "MOQ Prototyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Lieferzeit Prototyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Arbeitszeit Prototyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "MOQ Serie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Lieferzeit Serie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Arbeitszeit Serie";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "MOQ S1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Lieferzeit S1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Arbeitszeit S1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "MOQ S2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "Lieferzeit S2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "Arbeitszeit S2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "MOQ S3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "Lieferzeit S3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 28].Value = "Arbeitszeit S3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 29].Value = "MOQ S4";
					worksheet.Cells[headerRowNumber, startColumnNumber + 30].Value = "Lieferzeit S4";
					worksheet.Cells[headerRowNumber, startColumnNumber + 31].Value = "Arbeitszeit S4";


					var rowNumber = headerRowNumber + 1;

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, 1, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// - Erstmuster
					using(var range = worksheet.Cells[1, 9, 1, 11])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC000"));
					}
					// - Nullserie
					using(var range = worksheet.Cells[1, 12, 1, 14])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFC0F0"));
					}
					// - Prototyp
					using(var range = worksheet.Cells[1, 15, 1, 17])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#00F0F0"));
					}
					// - Serie
					using(var range = worksheet.Cells[1, 18, 1, 20])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#0FC0F0"));
					}

					// - S1
					using(var range = worksheet.Cells[1, 21, 1, 23])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#F0F0F0"));
					}
					// - S2
					using(var range = worksheet.Cells[1, 24, 1, 26])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#C0C0C0"));
					}
					// - S3
					using(var range = worksheet.Cells[1, 27, 1, 29])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#A0A0A0"));
					}
					// - S4
					using(var range = worksheet.Cells[1, 30, 1, 32])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#909090"));
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
					package.Workbook.Properties.Title = $"VK - Update";
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
