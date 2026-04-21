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
	public class DownloadExcelInventurlisteRohmaterialHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private RohmaterialSearch _data { get; set; }
		public DownloadExcelInventurlisteRohmaterialHandler(RohmaterialSearch _data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = _data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var response = new List<Rohmaterial>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetInventurlisteRohmaterial(
					this._data.Mindestlagerbestand,
					this._data.Lagerort_id,
					null,
					null,
					  null

					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
				{
					response = PackingListEntity.Select(k => new Rohmaterial(k)).ToList();
				}
				int allCount = response.Select(x => x.totalRows).First();
				return ResponseModel<byte[]>.SuccessResponse(getExcel(response, _data, "Inventurliste ROHmaterial"));
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
				throw new NotImplementedException();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

		static byte[] getExcel(List<Rohmaterial> MainRequestModel, RohmaterialSearch _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"PSZ_Inventurliste Rohmaterial {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"PSZ_Inventurliste Rohmaterial - {DateTime.Now.ToString("yyyy/MM/dd")}");


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
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = "Artikel-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = "EK";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = "EK_Summe";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = "Gesamtgewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = "Zolltarif_nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = "Ursprungsland";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = "Lieferanten Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellNr].Value = "Bestell Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.BezeichnungAL].Value = "BezeichnungAL";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Praferenz].Value = "Präferenz";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = item.ArtikelNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = item.Bezeichnung1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = item.Bestand;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = item.Lagerort;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = item.EK;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Style.Numberformat.Format = "#,##0.00 €";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = item.EK_Summe;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Style.Numberformat.Format = "#,##0.00 €";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = item.Gewicht;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Style.Numberformat.Format = "#,##0.00";
						if(!item.Gewicht.Contains("E"))
						{
							worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Style.Numberformat.Format = "#,##0.00";
						}
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = item.Gesamtgewicht;

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Style.Numberformat.Format = "#,##0.00";


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = item.Zolltarif_nr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = item.Ursprungsland;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = item.LieferantenNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.BestellNr].Value = item.BestellNr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.BezeichnungAL].Value = item.BezeichnungAL;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Praferenz].Value = item.Praferenz;

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

		public ResponseModel<byte[]> getProforma()
		{
			try
			{
				var response = new List<Rohmaterial>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetInventurlisteRohmaterial(
					this._data.Mindestlagerbestand,
					this._data.Lagerort_id,
					null,
					null,
					null

					);
				if(PackingListEntity != null && PackingListEntity.Count > 0)
				{
					response = PackingListEntity.Select(k => new Rohmaterial(k)).ToList();
				}
				int allCount = response.Select(x => x.totalRows).First();
				return ResponseModel<byte[]>.SuccessResponse(getExcel_ProformaRechnung(response, _data, ""));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		static byte[] getExcel_ProformaRechnung(List<Rohmaterial> MainRequestModel, RohmaterialSearch _data, string title)
		{
			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(MainRequestModel.Select(x => (int?)x.ArtikelNr ?? -1)?.ToList())
				?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			var warenTypeEntities = Infrastructure.Data.Access.Tables.BSD.WarentypAccess.Get();
			warenTypeEntities.ForEach(x =>
			{
				if(x.Warentyp?.Trim()?.ToLower() == "meterware")
				{
					x.Warentyp = "Meter";
				}
				else if(x.Warentyp?.Trim()?.ToLower() == "stückware")
				{
					x.Warentyp = "Stück";
				}
			});
			string filePath = Path.Combine(Path.GetTempPath(), $"PSZ_InventurlisteRohmaterialProformaRechnung{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"InventurlisteRoh Proforma");


					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 3;
					var startColumnNumber = 0;
					var numberOfColumns = Enum.GetNames(typeof(ExcelColumnNumber)).Length - 3;
					using(var range = worksheet.Cells[1, 1, 1, 13])
					{
						range.Value = "Packliste nach Artikelnummern zur Proforma-Rechnung Nr. ";
						range.Style.Font.Size = 25;
						range.Style.Font.Bold = true;
						range.Style.Font.Italic = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Font.Color.SetColor(Color.Gray);
						range.Merge = true;

						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
					}
					using(var range = worksheet.Cells[1, 2])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = "Nr.";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = "Artikel-Nr.";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = "Artikel Bezeichnung";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = "Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = "Einheit";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = "EK Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = "EK Summe";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = "Gew. g";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = "Ges. KG";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = "ZTN";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = "UL";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = "Firmenname";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Präferenz";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					int idx = 0;
					foreach(var item in MainRequestModel)
					{
						idx++;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.ArtikelNr].Value = idx;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Artikelnummer].Value = item.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung1].Value = item.Bezeichnung1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bestand].Value = item.Bestand;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = warenTypeEntities.FirstOrDefault(y => y.Warentyp_ID == articleEntities.FirstOrDefault(x => x.ArtikelNr == item.ArtikelNr)?.Warentyp)?.Warentyp;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Value = item.EK;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK].Style.Numberformat.Format = "#,##0.00";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Value = item.EK_Summe;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.EK_Summe].Style.Numberformat.Format = "#,##0.00";
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Value = item.Gewicht;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Style.Numberformat.Format = "#,##0.00";
						if(!item.Gewicht.Contains("E"))
						{
							worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gewicht].Style.Numberformat.Format = "#,##0.00";
						}
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Value = item.Gesamtgewicht;

						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Gesamtgewicht].Style.Numberformat.Format = "#,##0.00";


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Zolltarif_nr].Value = item.Zolltarif_nr;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Ursprungsland].Value = item.Ursprungsland;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.LieferantenNr].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Praferenz;

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
						range.Style.Font.Bold = false;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}

					worksheet.Cells[headerRowNumber, 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
					worksheet.Cells[headerRowNumber, numberOfColumns].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Inventurliste ROH";
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
			Lagerort = 5,
			EK = 6,
			EK_Summe = 7,
			Gewicht = 8,
			Gesamtgewicht = 9,
			Zolltarif_nr = 10,
			Ursprungsland = 11,
			LieferantenNr = 12,
			Name1 = 13,
			BestellNr = 14,
			BezeichnungAL = 15,
			Praferenz = 16
		}
	}
}
