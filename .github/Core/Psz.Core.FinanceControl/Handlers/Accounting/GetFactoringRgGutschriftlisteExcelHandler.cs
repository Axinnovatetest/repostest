using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetFactoringRgGutschriftlisteExcelHandler: IHandle<GetFactoringRgGutschriftlisteRequestModel, ResponseModel<byte[]>>
{

	private GetFactoringRgGutschriftlisteRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	private DateTime _fromdate { get; set; }
	private DateTime _todate { get; set; }
	private string _typ { get; set; }

	public GetFactoringRgGutschriftlisteExcelHandler(UserModel user, string typ, DateTime fromdate, DateTime todate)
	{
		this._user = user;
		this._fromdate = fromdate;
		this._todate = todate;
		this._typ = typ;

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
		List<FactoringRgGutschriftlisteEntity> fetchedData = new();

		fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetFactoringRgGutschriftlist(_typ, _fromdate, _todate, null, null, 1);
		return SaveToExcelFile(fetchedData);
	}

	internal byte[] SaveToExcelFile(
		List<Infrastructure.Data.Entities.Joins.FNC.Accounting.FactoringRgGutschriftlisteEntity> entities
		)
	{

		string XLS_FORMAT_NUMBER = "0.0#####";
		string XLS_FORMAT_DATE = "dd/MM/yyyy";
		try
		{
			var chars = new char[] { ' ', '#' };
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"FactoringRgGutschriftliste-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

			var file = new FileInfo(filePath);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Create the package and make sure you wrap it in a using statement
			using(var package = new ExcelPackage(file))
			{
				// add a new worksheet to the empty workbook
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Factoring Rg Gutschrift liste");

				// Keep track of the row that we're on, but start with four to skip the header
				var headerRowNumber = 2;
				var startColumnNumber = 1;
				var numberOfColumns = 11; // updated
				var numberOfColumnstomerge = 11; // updated

				// Add some formatting to the worksheet
				worksheet.TabColor = Color.Yellow;
				worksheet.DefaultRowHeight = 11;
				worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
				worksheet.Row(2).Height = 20;
				worksheet.Row(1).Height = 30;
				worksheet.Row(headerRowNumber).Height = 20;

				// Pre Header
				worksheet.Cells[1, 1, 1, numberOfColumnstomerge].Merge = true;
				worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				worksheet.Cells[1, 1].Value = $"Factoring Rg Gutschrift liste";
				worksheet.Cells[1, 1].Style.Font.Size = 20;


				//var shiftCols = 0;


				headerRowNumber += 1;


				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Debitor";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Beleg-Nr";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Typ";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Datum";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Betrag";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Währung";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "MwSt-Satz";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Fällig am";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Netto-Laufzeit";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Skontotage 1";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kondition 1";//



				var rowNumber = headerRowNumber + 1;
				if(entities is not null && entities.Count > 0)
				{
					// Loop through 
					foreach(var w in entities)
					{
						// -
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Debitor;//
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Beleg_Nr;//
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Typ;//
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Datum;//
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Betrag;//
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Wahrung;//
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.MwSt_Satz;//
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Fallig_am;//
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Netto_Laufzeit;//
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Skontotage_1;//
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Kondition_1;//


						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}
				}

				#region Makeup
				//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
				using(var range = worksheet.Cells[1, 1, headerRowNumber - 2, numberOfColumns])
				{
					range.Style.Font.Bold = true;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
					range.Style.Font.Color.SetColor(Color.Black);

					range.Style.ShrinkToFit = false;
				}
				// Darker Blue in Top cell
				worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));

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
				package.Workbook.Properties.Title = $"Factoring Rg Gutschrift liste";
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
