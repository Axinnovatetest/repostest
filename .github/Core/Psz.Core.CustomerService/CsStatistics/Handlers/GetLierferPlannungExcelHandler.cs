using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetLierferPlannungExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private string _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLierferPlannungExcelHandler(Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
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
				var lierferPlannungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLieferPlannung(_data);
				if(lierferPlannungEntity != null && lierferPlannungEntity.Count > 0)
					response = SaveToExcelFile(lierferPlannungEntity, _data);


				return ResponseModel<byte[]>.SuccessResponse(response);
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

		internal byte[] SaveToExcelFile(
	  List<Infrastructure.Data.Entities.Joins.CTS.LierferPlannungEntity> LierferPlannungEntity, string data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"LIERFERPLANNUNG-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BackLog FG");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 14;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";


					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde:";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dokument-Mr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Wunschtermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Liefertermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Angebot-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Menge Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Jahr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "KW";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gesamtpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "CS Internes Bemerkung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Lieferadresse";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(LierferPlannungEntity != null && LierferPlannungEntity.Count > 0)
					{
						foreach(var p in LierferPlannungEntity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Vorname_NameFirma;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Dokumentnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Wunschtermin.HasValue == true ? p.Wunschtermin.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Liefertermin.HasValue == true ? p.Liefertermin.Value : ""; //
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Angebot_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Menge_Offen;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Jahr;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.KW;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.CSInterneBemerkung;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = $"{p.LName2}, {p.LLand_PLZ_Ort}".Trim().Trim(',');

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

					}

					// Set some document properties
					if(LierferPlannungEntity != null && LierferPlannungEntity.Count > 0)
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
					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"LIERFERPLANNUNG";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

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
}
