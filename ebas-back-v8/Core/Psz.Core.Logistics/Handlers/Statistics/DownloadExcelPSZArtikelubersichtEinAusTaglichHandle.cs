using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics
{
	public class DownloadExcelPSZArtikelubersichtEinAusTaglichHandle: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private WEnachDatumSearch _data { get; set; }
		public DownloadExcelPSZArtikelubersichtEinAusTaglichHandle(WEnachDatumSearch _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
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

				var response = new List<PSZArtikelubersichtEinAusTaglichEntityModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetWEnachDatumList(
					this._data.DateBegin,
					this._data.DateEnd,
					null,
					null,
					null

					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new PSZArtikelubersichtEinAusTaglichEntityModel(k)).ToList();

				return ResponseModel<byte[]>.SuccessResponse(getExcel(response, _data, "UbersichtEinAusTaglich"));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		static byte[] getExcel(List<PSZArtikelubersichtEinAusTaglichEntityModel> MainRequestModel, WEnachDatumSearch _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"PSZArtikelubersichtEinAusTaglich - {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZArtikelubersichtEinAusTaglich - {DateTime.Now.ToString("yyyy/MM/dd")}");


					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 4;
					var startColumnNumber = 0;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length;

					using(var range = worksheet.Cells[1, 1])
					{
						range.Value = title + " Search :";
						range.Style.Font.Bold = true;
						range.Style.Font.UnderLine = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Font.Color.SetColor(Color.Green);
					}
					using(var range = worksheet.Cells[1, 2])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 1])
					{
						range.Value = "Vom = " + _data.DateBegin.ToString("dd/MM/yyyy");

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = "Bis = " + _data.DateEnd.ToString("dd/MM/yyyy");
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Typ].Value = "Type";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellungNr].Value = "Bestellung Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Anzahl].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Datum].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Name 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerplatz_von].Value = "Lagerplatz von";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerplatz_nach].Value = "Lagerplatz nach";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Mindestbestellmenge].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Verpackungseinheit].Value = "Verpackungseinheit";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Typ].Value = item.Typ;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellungNr].Value = item.BestellungNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Anzahl].Value = item.Anzahl;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Datum].Value = (item.Datum == null) ? "" : item.Datum.Value;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Datum].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerplatz_von].Value = item.Lagerplatz_von;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerplatz_nach].Value = item.Lagerplatz_nach;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Mindestbestellmenge].Value = item.Mindestbestellmenge;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Verpackungseinheit].Value = item.Verpackungseinheit;


						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					// Doc content
					if(MainRequestModel != null && MainRequestModel.Count > 0)
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

					//// Pre + Header
					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

						//range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						//range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						//range.Style.Font.Color.SetColor(Color.Black);
						//range.Style.ShrinkToFit = true;
					}

					//// Totals



					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Faulty FAs";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
				}
				// -
				return File.ReadAllBytes(filePath);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public enum ExcelColumnNumber
		{
			Artikelnummer = 1,
			Typ = 2,
			BestellungNr = 3,
			Anzahl = 4,
			Datum = 5,
			Name1 = 6,
			Lagerplatz_von = 7,
			Lagerplatz_nach = 8,
			Mindestbestellmenge = 9,
			Verpackungseinheit = 10
		}

	}
}
