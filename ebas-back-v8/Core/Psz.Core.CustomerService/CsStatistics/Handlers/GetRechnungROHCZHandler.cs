using MoreLinq;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Reporting.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetRechnungROHCZHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungROHCZHandler(Identity.Models.UserModel user, RechnungEntryModel data)
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
				var ReportData = new RechnungROHModel();
				var _lager = Enums.OrderEnums.GetLagerNumber((Enums.OrderEnums.KapazitatLager)_data.Lager);
				var roh1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROH_1(_data.From, _data.To, _lager);
				var roh2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROH_2(_data.From, _data.To, _lager);

				var rohEntity = roh1?.Concat(roh2 ?? new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHEntity> { }).ToList();

				if(rohEntity != null && rohEntity.Count > 0)
				{
					var _zollatarif = rohEntity.Select(z => z.Zolltarif_nr).Distinct().ToList();
					ReportData = new RechnungROHModel
					{
						Header = new List<RechnungROHHeaderModel>
						{
							new RechnungROHHeaderModel
							{
								From=_data.From.ToString("dd-MM-yyyy"),
								To=_data.To.ToString("dd-MM-yyyy"),
								RechnungDatum=_data.RechnungsDatum.HasValue?_data.RechnungsDatum.Value.ToString("dd-MM-yyyy"):"",
							}
						},
						Details = _zollatarif.OrderBy(x => x).Select(a => new RechnungROHDetailsModel
						{
							Warenummer = a,
							Anzahl = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Eingangsmenge ?? 0),
							Gewicht = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Gewicht ?? 0),
							WarenWert = rohEntity.Where(x => x.Zolltarif_nr == a).Sum(y => y.Wert ?? 0),
						}).ToList(),
					};
				}

				response = SaveToExcelFile(ReportData.Details, _data, GetRechnungROHTNHandler.PrepareRechnungNummer(_data));
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
		internal static byte[] SaveToExcelFile(
		   List<RechnungROHDetailsModel> RechnungROHEntity, RechnungEntryModel data, string RechnungNummer)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Rechnung-ROH-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Rechnung-ROH");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 4;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Cells[headerRowNumber, 1].Value = $"Proforma-Rechnung:";
					worksheet.Cells[headerRowNumber, 2].Value = $"{data.To.ToString("dd/MM/yyyy")}";
					worksheet.Row(headerRowNumber).Style.Font.Bold = true;
					headerRowNumber++;
					worksheet.Cells[headerRowNumber, 1].Value = $"Rechnungszeitraum:";
					worksheet.Cells[headerRowNumber, 2].Value = $"{data.From.ToString("dd/MM/yyyy")} - {data.To.ToString("dd/MM/yyyy")}";
					worksheet.Row(headerRowNumber).Style.Font.Bold = true;
					headerRowNumber++;

					worksheet.Row(headerRowNumber).Style.Font.Bold = true;
					worksheet.Row(headerRowNumber).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(headerRowNumber).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Warenummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "WarenWert";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(RechnungROHEntity != null && RechnungROHEntity.Count > 0)
					{
						foreach(var p in RechnungROHEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Warenummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Gewicht;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.WarenWert;
							// - 
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber += 1;
						}
						worksheet.Cells[rowNumber, startColumnNumber].Value = "Gesamtsummen";
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = RechnungROHEntity?.Sum(p => p?.Gewicht ?? 0) ?? 0;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = RechnungROHEntity?.Sum(p => p?.WarenWert ?? 0) ?? 0;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						rowNumber += 1;
					}

					worksheet.Cells[rowNumber, 1, rowNumber, 10].Merge = true;
					worksheet.Cells[rowNumber, startColumnNumber].Value = "Der Ausführer der Ware, auf die sich dieses Handelspapier bezieht, erklärt, dass diese Waren, soweit nicht anders angegeben, präferenzbegünstigte EU-Ursprungswaren sind.";
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					// Set some document properties
					package.Workbook.Properties.Title = "Rechnung-ROH";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!
					package.Save();

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
