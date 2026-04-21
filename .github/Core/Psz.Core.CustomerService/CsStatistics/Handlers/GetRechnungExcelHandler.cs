using OfficeOpenXml;
using OfficeOpenXml.Style;
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
	public class GetRechnungExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private RechnungEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungExcelHandler(Identity.Models.UserModel user, RechnungEntryModel data)
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
			var _lager = GetWarehouse(_data.Lager);
			var RechnungEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetRechnung(_data.From, _data.To, _lager);
			response = SaveToExcelFile(RechnungEntity, _data);

			return ResponseModel<byte[]>.SuccessResponse(response);

		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.CTS.RechnungEntity> RechnungEntity, RechnungEntryModel data)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Rechnung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Rechnung");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					worksheet.Cells["A1"].Value = $"CZ-{data.From.Day.ToString()}-{data.To.Day.ToString()}-{data.From.Month.ToString()}-{data.From.Year.ToString()}";


					worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
					worksheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A1"].Style.WrapText = true;
					worksheet.Cells["A1"].Style.Font.Color.SetColor(Color.Black);
					worksheet.Cells["A1"].Style.Font.Bold = true;
					worksheet.Cells["A1"].Style.Font.Size = 15;
					worksheet.Cells["A1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
					worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.White);
					worksheet.Cells["A1"].Style.Font.Bold = true;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Betrag";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Ausdr3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Gewicht";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Zollnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "MinutenKosten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Originalanzahl";

					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(RechnungEntity != null && RechnungEntity.Count > 0)
					{
						foreach(var p in RechnungEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Bezeichnung1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Betrag;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Ausdr3;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Material1;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Gesamtgewicht;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Zolltarif_nr;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.MinutenKosten;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Originalanzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							rowNumber += 1;
						}

						worksheet.Cells[rowNumber, 6].Value = String.Format("{0:N3}", RechnungEntity.Sum(a => a.Ausdr3)) + " €";
						worksheet.Cells[rowNumber, 6].Style.Font.Size = 15;
						worksheet.Cells[rowNumber, 6].Style.Font.Bold = true;
					}
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					// Set some document properties
					package.Workbook.Properties.Title = "Rechnung";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
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

		public static KeyValuePair<int, int> GetWarehouse(int lager)
		{
			switch(lager)
			{
				case (int)Enums.OrderEnums.KapazitatLager.CZ:
					return new KeyValuePair<int, int>(6, 3);
				case (int)Enums.OrderEnums.KapazitatLager.TN:
					return new KeyValuePair<int, int>(7, 4);
				case (int)Enums.OrderEnums.KapazitatLager.BETN:
					return new KeyValuePair<int, int>(60, 62);
				case (int)Enums.OrderEnums.KapazitatLager.WS:
					return new KeyValuePair<int, int>(42, 40);
				case (int)Enums.OrderEnums.KapazitatLager.AL:
					return new KeyValuePair<int, int>(26, 25);
				case (int)Enums.OrderEnums.KapazitatLager.GZTN:
					return new KeyValuePair<int, int>(102, 104);
				default:
					return new KeyValuePair<int, int>();
			}
		}
	}
}
