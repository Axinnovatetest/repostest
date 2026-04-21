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
	public class GetBacklogHWExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private BacklogReportEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBacklogHWExcelHandler(Identity.Models.UserModel user, BacklogReportEntryModel data)
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
			var backLogFGEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetBacklogHW(_data.From, _data.To);

			response = _user.Access.CustomerService.StatsBacklogHWAdmin/*_data.Code == 1*/ ? SaveToExcelFile_1(backLogFGEntity, _data) : SaveToExcelFile_2(backLogFGEntity, _data);

			return ResponseModel<byte[]>.SuccessResponse(response);

		}
		internal static byte[] SaveToExcelFile_1(
	 List<Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity> backlogFGEntity, BacklogReportEntryModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"BackLog HW-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BackLog HW-{DateTime.Now.ToString("yyyyMMddTHHmmss")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

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
					worksheet.Row(1).Height = 15;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "AB#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bez.1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bestellit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "FA#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VK/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "VK/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Mat.&Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Mat.&Lohn/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "DB1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "DB%";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(backlogFGEntity != null && backlogFGEntity.Count > 0)
					{
						foreach(var l in backlogFGEntity.OrderBy(a => a.Liefertermin ?? DateTime.MaxValue))
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = l.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Angebot_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.OriginalAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Liefertermin.HasValue == true ? l.Liefertermin.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Einzelpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Kosten;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Gesamtkosten;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.Gesamtpreis.HasValue && l.Gesamtkosten.HasValue && l.Gesamtkosten.Value < 0.5m ? 0.01m : l.Gesamtpreis - l.Gesamtkosten;

							worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = "#0\\.0%";
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.Gesamtpreis.HasValue && l.Gesamtkosten.HasValue && l.Gesamtkosten.Value < 0.5m
																					   ? 0m : (l.Gesamtpreis - l.Gesamtkosten) / l.Gesamtkosten * 10;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
							rowNumber += 1;
						}

						worksheet.Cells[rowNumber, 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 10].Value = backlogFGEntity.Sum(a => a.Gesamtpreis);
						worksheet.Cells[rowNumber, 10].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 10].Style.Font.Bold = true;

						worksheet.Cells[rowNumber, 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 12].Value = backlogFGEntity.Sum(a => a.Gesamtkosten);
						worksheet.Cells[rowNumber, 12].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 12].Style.Font.Bold = true;
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"BackLog HW-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(4).Width = 23;
					worksheet.Column(8).Width = 20;
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 22;
					worksheet.Column(12).Width = 25;
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
	List<Infrastructure.Data.Entities.Joins.CTS.BacklogHWEntity> backlogFGEntity, BacklogReportEntryModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"BackLog HW-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"BackLog HW");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

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
					worksheet.Row(1).Height = 15;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "AB#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bez.1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bestellit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Offen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "FA#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VK/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "VK/Auftrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Mat.&Lohn/Stk.";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Mat.&Lohn/Auftrag";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(backlogFGEntity != null && backlogFGEntity.Count > 0)
					{
						foreach(var l in backlogFGEntity.OrderBy(a => a.Liefertermin ?? DateTime.MaxValue))
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = l.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Angebot_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.OriginalAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Liefertermin.HasValue == true ? l.Liefertermin.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Einzelpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Kosten;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Gesamtkosten;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						worksheet.Cells[rowNumber, 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 10].Value = backlogFGEntity.Sum(a => a.Gesamtpreis);
						worksheet.Cells[rowNumber, 10].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 10].Style.Font.Bold = true;

						worksheet.Cells[rowNumber, 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, 12].Value = backlogFGEntity.Sum(a => a.Gesamtkosten);
						worksheet.Cells[rowNumber, 12].Style.Font.Size = 12;
						worksheet.Cells[rowNumber, 12].Style.Font.Bold = true;
					}

					// Set some document properties
					package.Workbook.Properties.Title = "BackLog HW";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(4).Width = 23;
					worksheet.Column(8).Width = 20;
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 22;
					worksheet.Column(12).Width = 25;
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
