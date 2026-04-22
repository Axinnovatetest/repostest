namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Identity.Models;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System;
	using System.Drawing;

	public class GetMinStockFGTemplateFileHandler: IHandle<UserModel, ResponseModel<byte[]>>
	{
		public GetMinStockFGTemplateFileHandler() { }
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				return ResponseModel<byte[]>.SuccessResponse(getMinStockFMEmptyFile());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public byte[] getMinStockFMEmptyFile()
		{
			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Min.Stock- FG");

					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 3;

					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(headerRowNumber).Height = 20;
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lager";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Neu Sicherheitsbestand";

					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						//worksheet.Column(i).AutoFit();
						worksheet.Column(i).Width = 25;
					}
					// Set some document properties
					package.Workbook.Properties.Title = $"Min Stock Update";
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

		public ResponseModel<byte[]> Validate()
		{
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
