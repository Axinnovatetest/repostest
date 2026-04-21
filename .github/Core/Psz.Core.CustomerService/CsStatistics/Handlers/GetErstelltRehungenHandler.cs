using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetErstelltRehungenHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private IDateRangeModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetErstelltRehungenHandler(Identity.Models.UserModel user, IDateRangeModel data)
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

				var AuswertungRechnungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetErstelltRehugen(_data.From, _data.To);
				var response = this.SaveToExcelFile(AuswertungRechnungEntity);


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
		   List<Infrastructure.Data.Entities.Joins.CTS.ErstelltRehugenEntity> ErstelltFGEntity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Auswertung Rechnung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Auswertung Rechnung");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 10;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Vorname/NameFirma";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Rgnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Angebotnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "VKGesamtpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Gesamtkupferzuschlag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "NettoBetrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Bezug";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "CS Kontakt";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(ErstelltFGEntity != null && ErstelltFGEntity.Count > 0)
					{
						foreach(var p in ErstelltFGEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Vorname_NameFirma;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Datum.HasValue == true ? p.Datum.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Rgnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Angebotnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.VKGesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Gesamtkupferzuschlag;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.NettoBetrag;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Bezug;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.CSKontakt;
							rowNumber += 1;
						}
					}
					// Doc content
					if(ErstelltFGEntity != null && ErstelltFGEntity.Count > 0)
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
					// Set some document properties
					package.Workbook.Properties.Title = "Auswertung Rechnung";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					worksheet.Column(1).Width = 25;
					worksheet.Column(2).Width = 15;
					worksheet.Column(9).Width = 25;
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
