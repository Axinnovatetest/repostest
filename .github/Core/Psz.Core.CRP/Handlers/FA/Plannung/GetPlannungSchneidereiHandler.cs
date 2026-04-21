using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Plannung
{
	public class GetPlannungSchneidereiHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetPlannungSchneidereiHandler(Identity.Models.UserModel user, int data)
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
				// - Logique metier
				var list = new List<PlannungSchneidereiResponseModel>();
				byte[] excel = null;
				switch((Enums.FAEnums.FaLands)this._data)
				{
					case Enums.FAEnums.FaLands.AL:
						{
							var results = Infrastructure.Data.Access.Views.CTS.Fertigungs_Planung_Gesamt_übersicht_ohne_Mechanik_AlbanienAccess.GetPlannungAL();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					case Enums.FAEnums.FaLands.CZ:
						{
							var results = Infrastructure.Data.Access.Joins.CTS.PlannungSchneidereiAccess.GetPlannungCZ();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					case Enums.FAEnums.FaLands.TN:
						{
							var results = Infrastructure.Data.Access.Joins.CTS.PlannungSchneidereiAccess.GetPlannungTN();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					case Enums.FAEnums.FaLands.BETN:
						{
							var results = Infrastructure.Data.Access.Joins.CTS.PlannungSchneidereiAccess.GetPlannungBETN();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					case Enums.FAEnums.FaLands.WS:
						{
							var results = Infrastructure.Data.Access.Joins.CTS.PlannungSchneidereiAccess.GetPlannungWS();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					case Enums.FAEnums.FaLands.GZTN:
						{
							var results = Infrastructure.Data.Access.Joins.CTS.PlannungSchneidereiAccess.GetPlannungGZTN();
							//if(results != null && results.Count > 0)
							{
								list = results?.Select(x => new PlannungSchneidereiResponseModel(x))?.ToList();
								excel = SaveToExcelFile(list, this._data);
							}
						}
						break;
					default:
						break;
				}


				// - results
				return ResponseModel<byte[]>.SuccessResponse(excel);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			if(Enum.GetValues(typeof(Enums.FAEnums.FaLands)).Cast<Enums.FAEnums.FaLands>().ToList().FindIndex(x => (int)x == this._data) < 0)
				return ResponseModel<byte[]>.FailureResponse("Land not found");

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal byte[] SaveToExcelFile(
	   List<PlannungSchneidereiResponseModel> plannung, int lager)
		{
			try
			{
				string lg = string.Empty;
				// - 2022-12-22 - 
				foreach(int i in Enum.GetValues(typeof(Enums.FAEnums.FaLands)))
				{
					if(i == lager)
					{
						lg = ((Enums.FAEnums.FaLands)i).GetDescription();
					}
				}
				//switch(lager)
				//{
				//	case 26:
				//		lg = "AL";
				//		break;
				//	case 7:
				//		lg = "TN";
				//		break;
				//	case 42:
				//		lg = "WS";
				//		break;
				//	case 6:
				//		lg = "CZ";
				//		break;
				//	case 60:
				//		lg = "BETN";
				//		break;
				//	default:
				//		break;
				//}
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Schneiderei {lg}-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Schneiderei {lg}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 14;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "PSZ Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Halle";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Termin Schneiderei";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Termin_Bestätigt1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "FA_begonnen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "FA_Gestartet";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(plannung != null && plannung.Count > 0)
					{

						foreach(var l in plannung)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = l.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.PSZ_Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Halle;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = (l.Termin_Schneiderei.HasValue) ? l.Termin_Schneiderei.Value.ToString("dd-MMM-yyyy").Replace(".", "") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = (l.Termin_Planung.HasValue) ? l.Termin_Planung.Value.ToString("dd-MMM-yyyy").Replace(".", "") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Quantity;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (l.Order_Time.HasValue) ? l.Order_Time.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = (l.FA_begonnen.HasValue) ? l.FA_begonnen.Value.ToString("dd-MMM-yyyy").Replace(".", "") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.Gewerk_3;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.FA_Gestartet.HasValue && l.FA_Gestartet.Value ? "YES" : "NO";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(plannung != null && plannung.Count > 0)
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
					package.Workbook.Properties.Title = $"Plannung Gewerk1 {lg}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(4).Width = 50;
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