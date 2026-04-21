using OfficeOpenXml;
using OfficeOpenXml.Style;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetBacklogFGExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BacklogReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBacklogFGExcelHandler(Identity.Models.UserModel user, BacklogReportEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			byte[] response = null;
			// - 2023-01-04 - allow all - Schremmer
			var w = new GetWarehouseHandler(this._user).Handle();
			var backLogFGEntity = PDFReports.GetBacklogFGPDFHandler.GetData(this._data.Lager, _data.From, _data.To, w.Success ? w.Body.Select(x => x.Key)?.ToList() : null);

			var _finalData = new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
			if(backLogFGEntity != null && backLogFGEntity.Count > 0)
				_finalData = this._data.Lager is null
					? backLogFGEntity
					: backLogFGEntity.Where(a => !string.IsNullOrEmpty(a.Lagerort) && !string.IsNullOrWhiteSpace(a.Lagerort))?.OrderBy(b => b.Liefertermin ?? DateTime.MaxValue)?.ToList();

			response = _user.Access.CustomerService.StatsBacklogFGAdmin ? SaveToExcelFile_1(_finalData, _data) : SaveToExcelFile_2(_finalData, _data);

			return ResponseModel<byte[]>.SuccessResponse(response);

		}

		internal static byte[] SaveToExcelFile_1(
			List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> backlogFGEntity, BacklogReportEntryModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"BackLog FG-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BackLog FG");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 21;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Cells["A1"].Value = data.From;//.ToString("dd/MM/yyyy");
					worksheet.Cells["B1"].Value = data.To;//.ToString("dd/MM/yyyy");
					worksheet.Cells["C1"].Value = data.Lager <= 0 ? "" : data.Lager;

					List<string> cells = new List<string> { "A1", "B1", "C1" };


					foreach(var item in cells)
					{
						worksheet.Cells[item].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
						worksheet.Cells[item].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						worksheet.Cells[item].Style.WrapText = true;
						worksheet.Cells[item].Style.Font.Color.SetColor(Color.Black);
						worksheet.Cells[item].Style.Font.Bold = true;
						worksheet.Cells[item].Style.Font.Size = 15;
						worksheet.Cells[item].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Fill.PatternType = ExcelFillStyle.Solid;
						worksheet.Cells[item].Style.Fill.BackgroundColor.SetColor(Color.White);
						worksheet.Cells[item].Style.Font.Bold = true;
					}
					worksheet.Column(1).Width = 30;
					worksheet.Column(2).Width = 30;
					worksheet.Column(3).Width = 30;
					worksheet.Row(1).Height = 15;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Produktionsort:";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "AB Dokument-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "AB#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bez.1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Anzahl Bestellit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Anzahl Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "FA#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "VK/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "VK/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Mat.&Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Lohn/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Mat.&Lohn/Auftrag";

					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "DB1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "DB%";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(backlogFGEntity != null && backlogFGEntity.Count > 0)
					{
						var groupebByPrort = backlogFGEntity.Select(x => x.PRORT).Distinct().ToList();
						foreach(var p in groupebByPrort)
						{
							var _list = backlogFGEntity.Where(x => x.PRORT == p)?.ToList()
								?? new List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity>();
							worksheet.Cells[rowNumber, startColumnNumber].Value = p;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Size = 13;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Color.SetColor(Color.Blue);
							worksheet.Row(rowNumber).Height = 25;
							rowNumber += 1;
							foreach(var l in _list)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Name1;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.DOC_Number;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Angebot_Nr;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Bezeichnung1;
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.OriginalAnzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Anzahl;
								worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Fertigungsnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Liefertermin.HasValue == true ? l.Liefertermin.Value : "";
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Einzelpreis;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Gesamtpreis;
								worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = "";
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.Kosten;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = l.Betrag;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = l.Gesamtpersonalkosten;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = "";

								worksheet.Cells[rowNumber, startColumnNumber + 17].Value = l.Gesamtkosten;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 19].Value = (l.Gesamtpreis ?? 0) - (l.Gesamtkosten ?? 0);
								worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
								worksheet.Cells[rowNumber, startColumnNumber + 20].Value = l.Gesamtpreis.HasValue && l.Gesamtkosten.HasValue && l.Gesamtkosten.Value > 0 ?
								   String.Format("{0:N3}", (l.Gesamtpreis - l.Gesamtkosten) / l.Gesamtkosten) + "%" : 0;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
							worksheet.Cells[rowNumber, 12].Value = "Gesamtpreis:";
							worksheet.Cells[rowNumber, 12].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 12].Style.Font.Bold = true;

							worksheet.Cells[rowNumber, 13].Value = _list.Sum(a => a.Gesamtpreis);
							worksheet.Cells[rowNumber, 13].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 13].Style.Font.Bold = true;
							worksheet.Cells[rowNumber, 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Cells[rowNumber, 16].Value = "Personalkosten:";
							worksheet.Cells[rowNumber, 16].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 16].Style.Font.Bold = true;

							worksheet.Cells[rowNumber, 17].Value = _list.Sum(a => a.Gesamtpersonalkosten);
							worksheet.Cells[rowNumber, 17].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 17].Style.Font.Bold = true;
							worksheet.Cells[rowNumber, 17].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Cells[rowNumber, 18].Value = "Materialkosten:";
							worksheet.Cells[rowNumber, 18].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 18].Style.Font.Bold = true;

							worksheet.Cells[rowNumber, 19].Value = _list.Sum(a => a.Gesamtkosten);
							worksheet.Cells[rowNumber, 19].Style.Font.Size = 12;
							worksheet.Cells[rowNumber, 19].Style.Font.Bold = true;
							worksheet.Cells[rowNumber, 19].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							rowNumber += 1;
						}
						worksheet.Cells[rowNumber, 12].Value = "Summe Gesamtpreis:";
						worksheet.Cells[rowNumber, 12].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 12].Style.Font.Bold = true;

						worksheet.Cells[rowNumber, 13].Value = backlogFGEntity.Sum(a => a.Gesamtpreis);
						worksheet.Cells[rowNumber, 13].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 13].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

						worksheet.Cells[rowNumber, 16].Value = "Total Personalkosten:";
						worksheet.Cells[rowNumber, 16].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 16].Style.Font.Bold = true;

						worksheet.Cells[rowNumber, 17].Value = backlogFGEntity.Sum(a => a.Gesamtpersonalkosten);
						worksheet.Cells[rowNumber, 17].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 17].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 17].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

						worksheet.Cells[rowNumber, 18].Value = "Total Materialkosten:";
						worksheet.Cells[rowNumber, 18].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 18].Style.Font.Bold = true;

						worksheet.Cells[rowNumber, 19].Value = backlogFGEntity.Sum(a => a.Gesamtkosten);
						worksheet.Cells[rowNumber, 19].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 19].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 19].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					}

					// Set some document properties
					package.Workbook.Properties.Title = "BackLog FG";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 20;
					worksheet.Column(11).Width = 20;
					worksheet.Column(12).Width = 25;
					worksheet.Column(13).Width = 25;
					worksheet.Column(16).Width = 25;
					worksheet.Column(17).Width = 25;
					worksheet.Column(18).Width = 25;
					worksheet.Column(19).Width = 25;

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

		internal static byte[] SaveToExcelFile_2(
			List<Infrastructure.Data.Entities.Joins.CTS.BackLogFGEntity> backlogFGEntity, BacklogReportEntryModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"BackLog FG-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BackLog FG-{DateTime.Now.ToString("yyyyMMddTHHmmss")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 18;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Cells["A1"].Value = data.From;//.ToString("dd/MM/yyyy");
					worksheet.Cells["B1"].Value = data.To;//.ToString("dd/MM/yyyy");
					worksheet.Cells["C1"].Value = data.Lager <= 0 ? "" : data.Lager;

					List<string> cells = new List<string> { "A1", "B1", "C1" };


					foreach(var item in cells)
					{
						worksheet.Cells[item].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
						worksheet.Cells[item].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						worksheet.Cells[item].Style.WrapText = true;
						worksheet.Cells[item].Style.Font.Color.SetColor(Color.Black);
						worksheet.Cells[item].Style.Font.Bold = true;
						worksheet.Cells[item].Style.Font.Size = 15;
						worksheet.Cells[item].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						worksheet.Cells[item].Style.Fill.PatternType = ExcelFillStyle.Solid;
						worksheet.Cells[item].Style.Fill.BackgroundColor.SetColor(Color.White);
						worksheet.Cells[item].Style.Font.Bold = true;
					}
					worksheet.Column(1).Width = 30;
					worksheet.Column(2).Width = 30;
					worksheet.Column(3).Width = 30;
					worksheet.Row(1).Height = 15;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "AB Dokument-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "AB#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bez.1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Anzahl Bestellit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Anzahl Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "VK/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "VK/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Mat.&Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Lohn/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Mat.&Lohn/Auftrag";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(backlogFGEntity != null && backlogFGEntity.Count > 0)
					{
						foreach(var p in backlogFGEntity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.DOC_Number;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Angebot_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.OriginalAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Liefertermin.HasValue == true ? p.Liefertermin.Value : ""; //.Value.ToString("dd-MM-yyyy");
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Einzelpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Kosten;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Betrag;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Gesamtpersonalkosten;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Gesamtkosten;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						worksheet.Cells[rowNumber, 11].Value = backlogFGEntity.Sum(a => a.Gesamtpreis);
						worksheet.Cells[rowNumber, 11].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 11].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 14].Value = backlogFGEntity.Sum(a => a.Gesamtpersonalkosten);
						worksheet.Cells[rowNumber, 14].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 14].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 14].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 15].Value = backlogFGEntity.Sum(a => a.Gesamtkosten);
						worksheet.Cells[rowNumber, 15].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 15].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, 15].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"BackLog FG-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(11).Width = 20;
					worksheet.Column(14).Width = 25;
					worksheet.Column(15).Width = 25;

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
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
