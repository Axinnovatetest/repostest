using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Settings
{
	public class FAWunshWerkDraftHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _type { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public FAWunshWerkDraftHandler(Identity.Models.UserModel user, int type)
		{
			this._user = user;
			this._type = type;
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

				byte[] response = null;
				var tempFolder = System.IO.Path.GetTempPath();
				var _name = this._type == 1 ? "Wunsh Update DRAFT" : "Werk Update DRAFT";
				var filePath = System.IO.Path.Combine(tempFolder, $"{_name}--{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{_name} Example");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = this._type == 1 ? 2 : 3;
					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.DefaultRowHeight = 15;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#70AD47"));
					worksheet.Cells[headerRowNumber, startColumnNumber].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[headerRowNumber, startColumnNumber].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					//
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#70AD47"));
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					//
					if(this._type == 2)
					{
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bemerkung2";
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#70AD47"));
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						worksheet.Cells[headerRowNumber, startColumnNumber + 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
					}

					for(int i = 0; i <= numberOfColumns - 1; i++)
					{
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"{_name}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					package.Save();

					response = File.ReadAllBytes(filePath);
				}
				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _type:{_type}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}