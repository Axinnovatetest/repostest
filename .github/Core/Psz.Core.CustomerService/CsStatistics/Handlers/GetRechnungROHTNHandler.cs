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
	public class GetRechnungROHTNHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungROHTNHandler(Identity.Models.UserModel user, RechnungEntryModel data)
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
				var Data = new List<RechnungROHTNModel>();
				var _lager = GetRechnungExcelHandler.GetWarehouse(_data.Lager);
				var rohQuery1 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROHTN_1(this._data.From, this._data.To, _lager);
				var rohQuery2 = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnungROHTN_2(this._data.From, this._data.To, _lager);

				var rohEntity = rohQuery1?.Concat(rohQuery2 ?? new List<Infrastructure.Data.Entities.Joins.CTS.RechnungROHTNEntity> { }).ToList();
				if(rohEntity != null && rohEntity.Count > 0)
				{
					var _articleList = rohEntity.Select(a => a.Artikelnummer).Distinct().ToList();
					Data = _articleList.Select(a => new RechnungROHTNModel
					{
						Artikelnummer = a,
						Bezeichnung_1 = rohEntity.FirstOrDefault(x => x.Artikelnummer == a).Bezeichnung_1,
						Anzahl = rohEntity.Where(x => x.Artikelnummer == a).Sum(s => s.Menge) ?? 0,
					}).ToList();
				}

				response = SaveToExcelFile(Data, _data, PrepareRechnungNummer(_data));
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
		   List<RechnungROHTNModel> RechnungROHEntity, RechnungEntryModel data, string RechnungNummer)
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
					var numberOfColumns = 17;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Rechnungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Von";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikelbezeichnung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Menge (für FA und Entnahmen)";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(RechnungROHEntity != null && RechnungROHEntity.Count > 0)
					{
						foreach(var p in RechnungROHEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = RechnungNummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = data.From;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = data.To;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber += 1;
						}
					}
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
		public static string PrepareRechnungNummer(RechnungEntryModel data)
		{
			var Suffix = $"{data.From.Day.ToString()}-{data.To.Day.ToString()}-{data.To.Month.ToString()}-{data.To.Year.ToString()}";
			switch(data.Lager)
			{
				case (int)Enums.OrderEnums.KapazitatLager.CZ:
					return $"CZ-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return $"TN-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.BETN:
					return $"BETN-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return $"WSTN-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.AL:
					return $"AL-{Suffix}";
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return $"GZTN-{Suffix}";
				default:
					return "";
			}
		}
	}
}
