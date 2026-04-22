using Infrastructure.Data.Entities.Joins.FNC.Accounting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class LiquiditatsplanungOffeneMaterialbestellungenExcelHandler: IHandle<UserModel, ResponseModel<byte[]>>
{

	private UserModel _user { get; set; }
	private DateTime fromdate { get; set; }
	private DateTime todate { get; set; }
	public LiquiditatsplanungOffeneMaterialbestellungenExcelHandler(UserModel user, DateTime fromdate, DateTime todate)
	{
		this._user = user;
		this.fromdate = fromdate;
		this.todate = todate;
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
		List<LiquiditatsplanungOffeneMaterialbestellungenEntity> fetchedData = new();

		fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetLiquiditatsplanungOffeneMaterialbestellungen(fromdate, todate, null, null, 1);
		return SaveToExcelFile(fetchedData);
	}

	internal byte[] SaveToExcelFile(
		List<Infrastructure.Data.Entities.Joins.FNC.Accounting.LiquiditatsplanungOffeneMaterialbestellungenEntity> entities
		)
	{

		string XLS_FORMAT_NUMBER = "0.0#####";
		string XLS_FORMAT_DATE = "dd/MM/yyyy";
		try
		{
			var chars = new char[] { ' ', '#' };
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"LiquiditatsplanungOffeneMaterialbestellungen-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

			var file = new FileInfo(filePath);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			// Create the package and make sure you wrap it in a using statement
			using(var package = new ExcelPackage(file))
			{
				// add a new worksheet to the empty workbook
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Liquiditats planung Offene Material bestellungen");

				// Keep track of the row that we're on, but start with four to skip the header
				var headerRowNumber = 2;
				var startColumnNumber = 1;
				var numberOfColumns = 22; // updated
				var numberOfColumnstomerge = 5; // updated

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
				worksheet.Cells[1, 1].Value = $"Liquiditats planung Offene Material bestellungen";
				worksheet.Cells[1, 1].Style.Font.Size = 20;


				//var shiftCols = 0;


				headerRowNumber += 1;


				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Benutzer";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Lieferantennr";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lieferant";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bestellung-Nr";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Anzahl";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Mindestbestellmenge";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Verpackungseinheit";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bezeichnung 1";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Artikelnummer";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Bestellnummer";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Einzelpreis";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gesamtpreis";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Anlieferung";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Zahlungsziel Netto";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Fälligkeit";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Produktionsstätte";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Mandant";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Bearbeiter";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Belegdatum";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Wünschtermin";//
				worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Bemerkung_Pos";//


				var rowNumber = headerRowNumber + 1;
				if(entities is not null && entities.Count > 0)
				{
					// Loop through 
					foreach(var w in entities)
					{
						// -
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Benutzer;//
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Lieferantennr;//
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Lieferant;//
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Bestellung_Nr;//
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Anzahl;//
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Mindestbestellmenge;//
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Verpackungseinheit;//
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Bezeichnung_1;//
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Artikelnummer;//
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Bestellnummer;//
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Einzelpreis;//
						worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Gesamtpreis;//
						worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Anlieferung;//
						worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Zahlungsziel_Netto;//
						worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.Falligkeit;//
						worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.Produktionsstatte;//
						worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.Mandant;//
						worksheet.Cells[rowNumber, startColumnNumber + 17].Value = w?.Bearbeiter;//
						worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.Belegdatum;//
						worksheet.Cells[rowNumber, startColumnNumber + 19].Value = w?.Wunschtermin;//
						worksheet.Cells[rowNumber, startColumnNumber + 21].Value = w?.Bemerkung_Pos;//


						worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 18].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 19].Style.Numberformat.Format = XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = XLS_FORMAT_NUMBER;

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
				package.Workbook.Properties.Title = $"Liquiditats planung Skontozahler";
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
