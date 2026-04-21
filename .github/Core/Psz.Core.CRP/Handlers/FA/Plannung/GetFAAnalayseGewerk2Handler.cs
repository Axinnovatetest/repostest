using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Plannung
{
	public class GetFAAnalayseGewerk2Handler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _lager { get; set; }
		private DateTime _date { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAAnalayseGewerk2Handler(Identity.Models.UserModel user, int lager, DateTime date)
		{
			this._user = user;
			this._lager = lager;
			this._date = date;
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
				byte[] resposnse = null;
				string myTime = this._date.ToString("dd/MM/yyyy");
				switch((Enums.FAEnums.FaLands)this._lager)
				{
					case Enums.FAEnums.FaLands.AL:
						var analyseGewerk2ALEntity = Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_ALAccess.AnalyseGewerk2AL(myTime);
						resposnse = SaveToExcelFileAL(analyseGewerk2ALEntity);
						break;
					case Enums.FAEnums.FaLands.CZ:
						var analyseGewerk2CZEntity = Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2Access.Analysegewerk2CZ(myTime);
						resposnse = SaveToExcelFileCZ(analyseGewerk2CZEntity);
						break;
					default:
						break;
					case Enums.FAEnums.FaLands.TN:
						var analyseGewerk2TNEntity = Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_TNAccess.Analysegewerk2TN(myTime);
						resposnse = SaveToExcelFileTunisia(analyseGewerk2TNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.WS:
						var analyseGewerk2KHTNEntity = Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_KHTNAccess.Analysegewerk2KHTN(myTime);
						resposnse = SaveToExcelFileTunisia(analyseGewerk2KHTNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.BETN:
						var analyseGewerk2BETNEntity = Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_BETNAccess.Analysegerwerk2BETN(myTime);
						resposnse = SaveToExcelFileTunisia(analyseGewerk2BETNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.GZTN:
						resposnse = SaveToExcelFileTunisia(Infrastructure.Data.Access.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_BETNAccess.Analysegerwerk2GZTN(myTime), this._lager);
						break;
				}
				return ResponseModel<byte[]>.SuccessResponse(resposnse);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _lager:{_lager},_date:{_date}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			//if (this._user == null/*
			//    || this._user.Access.____*/)
			//{
			//    return ResponseModel<byte[]>.AccessDeniedResponse();
			//}
			return ResponseModel<byte[]>.SuccessResponse();
		}

		internal static byte[] SaveToExcelFileAL(
			List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_ALEntity> gewerk2Entity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk2 AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk2  AL");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA_begonnen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Afati i prestarise";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
					{
						foreach(var p in gewerk2Entity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.PSZ_Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Halle;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = string.Format("{0:n}", p.Anzahl.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = (p.Ack_Date.HasValue) ? p.Ack_Date.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = string.Format("{0:n}", p.Order_Time.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (p.FA_begonnen.HasValue) ? p.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Gewerk_3;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = (p.Ack_Date.HasValue) ? p.Ack_Date.Value.AddDays(-21).ToString("dd-MMMM-yyyy") : "";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Plannung Gewerk2 AL";
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
		internal static byte[] SaveToExcelFileCZ(
			List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2Entity> gewerk2Entity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk2 CZ-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk2  CZ");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 11;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA_begonnen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gewerk 3";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
					{
						foreach(var p in gewerk2Entity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.PSZ_Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Halle;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = string.Format("{0:n}", p.Anzahl.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = (p.Ack_Date.HasValue) ? p.Ack_Date.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = string.Format("{0:n}", p.Order_Time.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (p.FA_begonnen.HasValue) ? p.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Gewerk_3;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Plannung Gewerk2  CZ";
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
		internal static byte[] SaveToExcelFileTunisia(
			List<Infrastructure.Data.Entities.Tables.CTS.Fertigungs_Planung_Sicht_Gewerk_2_TNEntity> gewerk2Entity, int lager)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				string lg = ((Enums.FAEnums.FaLands)lager).GetDescription();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk2 {lg}-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk2 {lg}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "PSZ Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA_begonnen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Termin_Schneiderei";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
					{
						foreach(var p in gewerk2Entity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.PSZ_Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Halle;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = string.Format("{0:n}", p.Anzahl.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = (p.Ack_Date.HasValue) ? p.Ack_Date.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = string.Format("{0:n}", p.Order_Time.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (p.FA_begonnen.HasValue) ? p.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Gewerk_3;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = (p.Ack_Date.HasValue) ? p.Ack_Date.Value.AddDays(-21).ToString("dd-MMMM-yyyy") : "";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(gewerk2Entity != null && gewerk2Entity.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Plannung Gewerk2 {lg}";
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
	}
}