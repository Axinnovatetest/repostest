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
	public class DownloadExcelPSZ_PV_ListeHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private PVModelSearch _data { get; set; }
		public DownloadExcelPSZ_PV_ListeHandler(Identity.Models.UserModel user, PVModelSearch _data)
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

				var response = new List<PSZ_PV_ListeModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPSZ_PV_ListeEntity(
					this._data.PVSendungsnummer,
					this._data.Lagernummer,
					null,
					null, null
					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
					response = PackingListEntity.Select(k => new PSZ_PV_ListeModel(k)).ToList();
				return ResponseModel<byte[]>.SuccessResponse(getExcel(response, _data, "PSZ_PV_Liste"));
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

		static byte[] getExcel(List<PSZ_PV_ListeModel> MainRequestModel, PVModelSearch _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"PSZ_PV_Liste {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZ_PV_Liste - {DateTime.Now.ToString("yyyy/MM/dd")}");


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
						range.Value = "PV-Sendungsnummer = " + _data.PVSendungsnummer;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = "Lagernummer = " + _data.Lagernummer;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = "ArtikelNr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = "Bezeichnung1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Einheit].Value = "Einheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = "EK";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = "EK_Summe";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = "Gesamtgewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = "Zolltarif_nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = "Ursprungsland";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = "LieferantenNr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellNr].Value = "BestellNr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.BezeichnungAL].Value = "BezeichnungAL";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Praferenz].Value = "Präferenz";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Sendungsnummer].Value = "Sendungsnummer";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = item.ArtikelNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = item.Bezeichnung1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = item.Bestand;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Einheit].Value = item.Einheit;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = item.Lagerort;
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
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = item.LieferantenNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellNr].Value = item.BestellNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.BezeichnungAL].Value = item.BezeichnungAL;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Praferenz].Value = item.Praferenz;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Sendungsnummer].Value = item.Sendungsnummer;


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
			ArtikelNr = 1,
			Artikelnummer = 2,
			Bezeichnung1 = 3,
			Bestand = 4,
			Einheit = 5,
			Lagerort = 6,
			EK = 7,
			EK_Summe = 8,
			Gewicht = 9,
			Gesamtgewicht = 10,
			Zolltarif_nr = 11,
			Ursprungsland = 12,
			LieferantenNr = 13,
			Name1 = 14,
			BestellNr = 15,
			BezeichnungAL = 16,
			Praferenz = 17,
			Sendungsnummer = 18
		}
	}
}
