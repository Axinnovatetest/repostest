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
	public class DownloadExcelProformaListHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private RohmaterialSearch _data { get; set; }
		public DownloadExcelProformaListHandler(RohmaterialSearch _data, Identity.Models.UserModel user)
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

				var response = new List<ProformaModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetProformaList(
				this._data.Mindestlagerbestand,
				this._data.Lagerort_id,
				null,
				null,
				null

				);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new ProformaModel(k)).ToList();
				int allCount = response.Select(x => x.totalRows).First();
				return ResponseModel<byte[]>.SuccessResponse(
				getExcel(response, _data, "Proforma List")
					);
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

		static byte[] getExcel(List<ProformaModel> MainRequestModel, RohmaterialSearch _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"PSZ_Proforma {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZ_Proforma - {DateTime.Now.ToString("yyyy/MM/dd")}");


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
						range.Value = "Mindestlagerbestand = " + _data.Mindestlagerbestand;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = "LagerOrt = " + _data.Lagerort_id;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Einheit].Value = "Einheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = "EK";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = "EK_Summe";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = "Gesamtgewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = "Zolltarif_nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = "Ursprungsland";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Praeferenz_Aktuelles_jahr].Value = "Praeferenz_Aktuelles_jahr Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Standardlieferant].Value = "Standardlieferant";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = item.Bezeichnung1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = item.Bestand;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Einheit].Value = item.Einheit;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = item.EK;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Style.Numberformat.Format = "#,##0.00 €";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = item.EK_Summe;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Style.Numberformat.Format = "#,##0.00 €";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = item.Gewicht;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Style.Numberformat.Format = "#,##0.00";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = item.Gesamtgewicht;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Style.Numberformat.Format = "#,##0.00";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = item.Zolltarif_nr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = item.Ursprungsland;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Praeferenz_Aktuelles_jahr].Value = item.Praeferenz_Aktuelles_jahr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Standardlieferant].Value = item.Standardlieferant;

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
			Bezeichnung1 = 2,
			Bestand = 3,
			Einheit = 4,
			EK = 5,
			EK_Summe = 6,
			Gewicht = 7,
			Gesamtgewicht = 8,
			Zolltarif_nr = 9,
			Ursprungsland = 10,
			Name1 = 11,
			Praeferenz_Aktuelles_jahr = 12,
			Standardlieferant = 13
		}
	}
}
