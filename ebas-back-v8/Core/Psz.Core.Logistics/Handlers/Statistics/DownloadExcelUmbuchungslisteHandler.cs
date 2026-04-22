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
	public class DownloadExcelUmbuchungslisteHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private UmbuchungslisteSearch _data { get; set; }
		public DownloadExcelUmbuchungslisteHandler(UmbuchungslisteSearch _data, Identity.Models.UserModel user)
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
				//----------------------------Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse------------------------
				_data.Lieferant = (_data.Lieferant == null) ? "" : _data.Lieferant;
				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager == _data.Lager).ToList();
				var tbl_Planung_gestartet_Lagerort_ID = Module.LGT.tbl_Planung_gestartet_Lagerort_ID.ToList();
				var response = new List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>();
				var Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity = Infrastructure.Data.Access.Joins.Logistics.StatisticsAccess.GetPsz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity(
					_data.withFG, _data.withoutFG, _data.Lieferant, _data.bis, ModelLGT.Select(x => x.Lager).First(), ModelLGT.Select(x => x.Lager_Id).First(), ModelLGT.Select(x => x.Lager_P_Id).First()
					, ModelLGT.Select(x => x.Lagerorte_Lagerort_id).First(), ModelLGT.Select(x => x.bestellte_Artikel_Lagerort_id).First(), tbl_Planung_gestartet_Lagerort_ID
					, null, null);
				if(Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity != null && Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity.Count > 0)
					response = Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity.Select(k => new Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse(k)).ToList();


				return ResponseModel<byte[]>.SuccessResponse(getExcel(response, _data, "Umbuchungsliste"));
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
				return ResponseModel<byte[]>.UnexpectedErrorResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		static byte[] getExcel(List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse> MainRequestModel, UmbuchungslisteSearch _data, string title)
		{

			string filePath = Path.Combine(Path.GetTempPath(), $"Umbuchungsliste - {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
			try
			{
				var file = new FileInfo(filePath);

				// Create the package and make sure you wrap it in a using statement
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Umbuchungsliste - {DateTime.Now.ToString("yyyy/MM/dd")}");


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
					using(var range = worksheet.Cells[1, 3])
					{
						range.Value = "Filtern nach FG = " + _data.withFG;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[1, 2])
					{
						range.Value = "Lager = " + _data.Lager;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
					}
					using(var range = worksheet.Cells[2, 1])
					{
						range.Value = "Lieferant = " + _data.Lieferant;

						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 2])
					{
						range.Value = "Bis = " + _data.bis.ToString("dd/MM/yyyy");
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
					using(var range = worksheet.Cells[2, 3])
					{
						range.Value = "Ohne FG = " + _data.withoutFG;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
					}
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = "Name 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Stucklisten_Artikelnummer].Value = "Stücklisten Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung_des_Bauteils].Value = "Bezeichnung des Bauteils";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.SummevonBruttobedarf].Value = "SummevonBruttobedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort_id].Value = "Lagerort id";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.SummevonBestand].Value = "SummevonBestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.MaxvonTermin_Materialbedarf].Value = "MaxvonTermin Materialbedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz].Value = "Differenz";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					foreach(var item in MainRequestModel)
					{


						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Name1].Value = item.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Stucklisten_Artikelnummer].Value = item.Stucklisten_Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Bezeichnung_des_Bauteils].Value = item.Bezeichnung_des_Bauteils;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.SummevonBruttobedarf].Value = item.SummevonBruttobedarf;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort_id].Value = item.Lagerort_id;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Lagerort].Value = item.Lagerort;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.SummevonBestand].Value = item.SummevonBestand;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.MaxvonTermin_Materialbedarf].Value = (item.MaxvonTermin_Materialbedarf == null) ? "" : item.MaxvonTermin_Materialbedarf.Value;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.MaxvonTermin_Materialbedarf].Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
						worksheet.Cells[rowNumber, startColumnNumber + (int)ExcelColumnNumber.Differenz].Value = item.Differenz;


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
			Name1 = 1,
			Stucklisten_Artikelnummer = 2,
			Bezeichnung_des_Bauteils = 3,
			SummevonBruttobedarf = 4,
			Lagerort_id = 5,
			Lagerort = 6,
			SummevonBestand = 7,
			MaxvonTermin_Materialbedarf = 8,
			Differenz = 9
		}
	}
}
