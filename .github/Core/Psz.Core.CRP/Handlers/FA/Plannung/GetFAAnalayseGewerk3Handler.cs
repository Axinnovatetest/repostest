using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Plannung
{
	public class GetFAAnalayseGewerk3Handler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _lager { get; set; }
		private DateTime _date { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAAnalayseGewerk3Handler(Identity.Models.UserModel user, int lager, DateTime date)
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
				byte[] response = null;
				string myTime = this._date.ToString("dd/MM/yyyy");
				switch((Enums.FAEnums.FaLands)this._lager)
				{
					case Enums.FAEnums.FaLands.AL:
						var analyseGewerk3ALEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3AL(myTime);
						response = SaveToExcelFileAL(analyseGewerk3ALEntity);
						break;
					case Enums.FAEnums.FaLands.CZ:
						var analyseGewerk3CZEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3CZ(myTime);
						response = SaveToExcelFileCZ_TN(analyseGewerk3CZEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.TN:
						var analyseGewerk3TNEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3TN(myTime);
						response = SaveToExcelFileCZ_TN(analyseGewerk3TNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.WS:
						var analyseGewerk3KHTNEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3KHTN(myTime);
						response = SaveToExcelFileCZ_TN(analyseGewerk3KHTNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.BETN:
						var analyseGewerk3BETNEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3BETN(myTime);
						response = SaveToExcelFileCZ_TN(analyseGewerk3BETNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.GZTN:
						response = SaveToExcelFileCZ_TN(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk3GZTN(myTime), this._lager);
						break;
					default:
						break;
				}
				return ResponseModel<byte[]>.SuccessResponse(response);
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
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3ALEntity> gewerk3Entity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk3 AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk3 AL");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 15;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "FA_Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "FA Sasia";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Numeri i artikullit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Numeri i materialit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bedarf Nevoje";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Ne magazine";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Ne prodhim";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Afati i prestarise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Typ_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "FA begonnen";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk3Entity != null && gewerk3Entity.Count > 0)
					{
						foreach(var p in gewerk3Entity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.FA_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.FA_Sasia;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Numeri_i_artikullit;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Numeri_i_materialit;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = string.Format("{0:n}", p.Nevoje.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = string.Format("{0:n}", p.Ne_magazine.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = string.Format("{0:n}", p.Ne_prodhim.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (p.Afati_i_prestarise.HasValue) ? p.Afati_i_prestarise.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Material;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Typ_Material;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Gewerk_3;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.FA_begonnen.HasValue ? p.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";

							rowNumber += 1;
							worksheet.Row(rowNumber).Height = 18;
						}
					}

					// Doc content
					if(gewerk3Entity != null && gewerk3Entity.Count > 0)
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
					package.Workbook.Properties.Title = "Plannung Gewerk3 AL";
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
		internal static byte[] SaveToExcelFileCZ_TN(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk3CZEntity> gewerk3Entity, int lager)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				string lg = ((Enums.FAEnums.FaLands)lager).GetDescription();

				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk3 {lg}-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk3  {lg}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 15;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "FA_Cislo";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "FA_Mnostvi";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Cislo_Zbozi";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Cislo_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Potreba";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Ve_skladu";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Ve_vyrobe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Termin_Rezarna";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Typ_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "FA begonnen";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk3Entity != null && gewerk3Entity.Count > 0)
					{
						foreach(var p in gewerk3Entity)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.FA_Cislo;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.FA_Mnostvi;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Cislo_Zbozi;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Cislo_Material;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = string.Format("{0:n}", p.Potreba.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = string.Format("{0:n}", p.Ve_skladu.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = string.Format("{0:n}", p.Ve_vyrobe.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (p.Termin_Rezarna.HasValue) ? p.Termin_Rezarna.Value.ToString("dd-MMMM-yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Material;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Typ_Material;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Gewerk_1;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Gewerk_2;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Gewerk_3;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.FA_begonnen.HasValue ? p.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";

							rowNumber += 1;
							worksheet.Row(rowNumber).Height = 18;
						}
					}

					// Doc content
					if(gewerk3Entity != null && gewerk3Entity.Count > 0)
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
					package.Workbook.Properties.Title = $"Plannung Gewerk3  {lg}";
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