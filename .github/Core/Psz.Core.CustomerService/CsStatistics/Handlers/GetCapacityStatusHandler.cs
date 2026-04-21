using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	using Psz.Core.CustomerService.CsStatistics.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using System.IO;
	using System.Drawing;

	public class GetCapacityStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CapacityResponseModel>>>
	{
		private CapacityRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCapacityStatusHandler(Identity.Models.UserModel user, CapacityRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CapacityResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			// -
			DateTime? dateFrom = null;
			DateTime? dateTo = null;
			string kwh = "";
			switch(_data.Horizon)
			{
				case 1:
					{
						dateFrom = null;
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
						kwh = $"before {DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1).ToString("dd.MM.yyyy")}";
						break;
					}
				case 2:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1); // 2024-01-25 - Khelil change H1 to 41 days
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7);
						kwh = $"{(_data.Cumulative == true ? "before " : $"between {DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1).ToString("dd.MM.yyyy")} and ")}{DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7).ToString("dd.MM.yyyy")}";
						break;
					}
				case 3:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7 + 1);
						dateTo = null;
						kwh = _data.Cumulative == true ? "kein Datumsfilter" : $"after {DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7).ToString("dd.MM.yyyy")}";
						break;
					}
				default:
					{
						dateFrom = null;
						dateTo = null;
						kwh = "kein Datumsfilter";
						break;
					}
			}
			return ResponseModel<List<CapacityResponseModel>>.SuccessResponse(
				   Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetCapacityStatus(dateFrom, dateTo)
				   ?.Select(x => new CapacityResponseModel(x, dateFrom, dateTo,
					kwh))
				   ?.ToList());

		}

		public ResponseModel<List<CapacityResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<List<CapacityResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CapacityResponseModel>>.SuccessResponse();
		}

		public byte[] GetExcelData()
		{
			var data = this.Handle();
			if(data == null || !data.Success || data.Body == null || data.Body.Count <= 0)
			{
				return null;
			}
			DateTime? dateFrom = null;
			DateTime? dateTo = null;
			string title = "";
			// 2024-01-25 - Khelil change H1 to 41 days
			switch(_data.Horizon)
			{
				case 1:
					{
						dateFrom = null;
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);
						title = $"bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 2:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + 1);
						dateTo = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7);
						title = $"{(_data.Cumulative == true ? "" : $"von {dateFrom.Value.ToString("dd.MM.yyyy")} - ")}bis {dateTo.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				case 3:
					{
						dateFrom = _data.Cumulative == true ? null : DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays + Module.CTS.FAHorizons.H2KWLength * 7 + 1);
						dateTo = null;
						title = _data.Cumulative == true ? "kein Datumsfilter" : $"von {dateFrom.Value.ToString("dd.MM.yyyy")}";
						break;
					}
				default:
					{
						dateFrom = null;
						dateTo = null;
						title = "kein Datumsfilter";
						break;
					}
			}

			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Kapa-{DateTime.Now.ToString("yyyyMMddTHHmmss")}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 8;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Column(1).Width = 30;
					worksheet.Column(2).Width = 30;
					worksheet.Row(1).Height = 15;

					using(var range = worksheet.Cells[headerRowNumber, startColumnNumber + 0, headerRowNumber, numberOfColumns])
					{
						range.Merge = true;
					}
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Style.Font.Bold = true;
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Style.Font.Size = 20;
					worksheet.Row(headerRowNumber).Height = 26;
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = $"Horizon {_data.Horizon}{(_data.Cumulative == true ? " kumulativ" : "")}: {title}";
					headerRowNumber++;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "AB Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "LP Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "FC Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Mindestbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Summe";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(data.Body != null && data.Body.Count > 0)
					{
						foreach(var l in data.Body/*.OrderBy(a => a.Artikelnummer)*/)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.faAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.abAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.lpAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.frcAnzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Mindestbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Summe;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}
					// Doc content
					if(data.Body != null && data.Body.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber - 1, numberOfColumns])
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i + startColumnNumber).AutoFit();
					}
					// Set some document properties
					package.Workbook.Properties.Title = $"Kapa-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
