using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetRechnungstransferLastExcelHandler: IHandle<UserModel, ResponseModel<byte[]>>
{

	private UserModel _user { get; set; }
	public GetRechnungstransferLastExcelHandler(UserModel user)
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

		if(_user == null)
		{
			return ResponseModel<byte[]>.AccessDeniedResponse();
		}
		return ResponseModel<byte[]>.SuccessResponse();
	}

	public byte[] GetData()
	{
		List<RechnungstransferEntity> fetchedData = new();

		fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetRechnungstransferLast();
		return SaveToExcelFile(fetchedData);
	}

	internal byte[] SaveToExcelFile(
		List<Infrastructure.Data.Entities.Joins.FNC.Accounting.RechnungstransferEntity> entities
		)
	{
		string XLS_FORMAT_NUMBER = "0.0#####";
		string XLS_FORMAT_DATE = "dd/MM/yyyy";
		try
		{
			var chars = new char[] { ' ', '#' };
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Rechnungstransfer-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

			var file = new FileInfo(filePath);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Create the package and make sure you wrap it in a using statement
			using(var package = new ExcelPackage(file))
			{
				// add a new worksheet to the empty workbook
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Rechnungstranfer");

				// Keep track of the row that we're on, but start with four to skip the header
				var headerRowNumber = 1;
				var startColumnNumber = 1;
				var numberOfColumns = 8; // updated

				// Add some formatting to the worksheet
				worksheet.TabColor = Color.Yellow;
				worksheet.DefaultRowHeight = 11;
				worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
				worksheet.Row(2).Height = 20;
				worksheet.Row(1).Height = 30;
				worksheet.Row(headerRowNumber).Height = 20;

				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Belegdatum";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Belegnummer";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Buchungstext";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Betrag";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Whrg";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Sollkto";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Habenkto";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Dokument-Nr";//

				var rowNumber = headerRowNumber + 1;
				if(entities is not null && entities.Count > 0)
				{
					// Loop through 
					foreach(var w in entities.OrderBy(x => x.Belegnummer))
					{
						// -
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Belegdatum;//
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Belegnummer;//
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Buchungstext;//
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Betrag;//
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Whrg;//
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Sollkto;//
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Habenkto;//
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bezug;//

						worksheet.Cells[rowNumber, startColumnNumber + 0].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}
				}

				#region Makeup
				// Darker Blue in Top cell
				using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
				{
					range.Style.Font.Bold = true;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3D3D3"));
					range.Style.Font.Color.SetColor(Color.Black);
					range.Style.ShrinkToFit = false;
					range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
					range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
					range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
					range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
				}

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
				#endregion Makeup

				// Set some document properties
				package.Workbook.Properties.Title = $"Rechnungstranfer";
				package.Workbook.Properties.Author = "PSZ ERP FNC";
				package.Workbook.Properties.Company = "PSZ ERP FNC";

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
