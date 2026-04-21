using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA.Plannung
{
	public class GetFAAnalayseGewerk1Handler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private int _lager { get; set; }
		private DateTime _date { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAAnalayseGewerk1Handler(Identity.Models.UserModel user, int lager, DateTime date)
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
						var analyseGewerk1ALEntity = Helpers.CalculatorHelper.CalculateRestbestandAL(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1AL(myTime));
						response = SaveToExcelFileAL(analyseGewerk1ALEntity);
						break;
					case Enums.FAEnums.FaLands.CZ:
						var analyseGewerk1CZEntity = Helpers.CalculatorHelper.CalculateRestbestandCZ(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1CZ(myTime));
						response = SaveToExcelFileCZ(analyseGewerk1CZEntity);
						break;
					case Enums.FAEnums.FaLands.TN:
						var analyseGewerk1TNEntity = Helpers.CalculatorHelper.CalculateRestbestandTN(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1TN(myTime));
						response = SaveToExcelFileTunisia(analyseGewerk1TNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.WS:
						var analyseGewerk1KHTNEntity = Helpers.CalculatorHelper.CalculateRestbestandTN(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1KHTN(myTime));
						response = SaveToExcelFileTunisia(analyseGewerk1KHTNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.BETN:
						var analyseGewerk1BETNEntity = Helpers.CalculatorHelper.CalculateRestbestandTN(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1BETN(myTime));
						response = SaveToExcelFileTunisia(analyseGewerk1BETNEntity, this._lager);
						break;
					case Enums.FAEnums.FaLands.GZTN:
						response = SaveToExcelFileTunisia(Helpers.CalculatorHelper.CalculateRestbestandTN(Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetFAAnalyseGewerk1GZTN(myTime)), this._lager);
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
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1ALEntity> gewerk1Entity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk1 AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk1  AL");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Cislo_Material1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA_Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "FA Sasia";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Numeri i artikullit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bedarf Nevoje";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "In P3000";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Im Lager Ne magazine";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "In Produktion Ne prodhim";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Restbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Afati i prestarise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Typ_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "FA begonnen";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
					{
						var groupebByMaterial = gewerk1Entity.Select(x => x.Numeri_i_materialit).Distinct().ToList();
						foreach(var p in groupebByMaterial)
						{
							var list = gewerk1Entity.Where(x => x.Numeri_i_materialit == p).ToList();
							worksheet.Cells[rowNumber, startColumnNumber].Value = p + ":";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Size = 13;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Color.SetColor(Color.Blue);
							worksheet.Row(rowNumber).Height = 25;
							rowNumber += 1;
							foreach(var l in list)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.FA_Nr;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Freigabestatus;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.FA_Sasia;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Numeri_i_artikullit;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = (l.Bedarf_Nevoje.HasValue) ? string.Format("{0:n}", Math.Round(l.Bedarf_Nevoje.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = (l.In_P3000.HasValue) ? string.Format("{0:n}", Math.Round(l.In_P3000.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (l.Im_Lager_Ne_magazine.HasValue) ? string.Format("{0:n}", Math.Round(l.Im_Lager_Ne_magazine.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (l.In_Produktion_Ne_prodhim.HasValue) ? string.Format("{0:n}", Math.Round(l.In_Produktion_Ne_prodhim.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.RestBestand;
								//(l.Im_Lager_Ne_magazine.HasValue && l.Bedarf_Nevoje.HasValue) ?string.Format("{0:n}", l.Im_Lager_Ne_magazine.Value - l.Bedarf_Nevoje.Value) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = (l.Afati_i_prestarise.HasValue) ? l.Afati_i_prestarise.Value.ToString("dd-MMMM-yyyy") : "";
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Material;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.Typ_Material;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.Gewerk_1;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = l.Gewerk_2;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = l.Gewerk_3;
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = l.FA_begonnen.HasValue ? l.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}

					// Doc content
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
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
					package.Workbook.Properties.Title = "Plannung Gewerk1  AL";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(8).Width = 20;
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 20;
					worksheet.Column(11).Width = 50;
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
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1CZEntity> gewerk1Entity)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk1 CZ-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");


				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk1  CZ");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Cislo_Material1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA_Cislo";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "FA Mnostvi";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Cislo Zbozi";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "P3000";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Im Lager";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "In derProduktion";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Restbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Termin Rezarna";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Typ_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "FA begonnen";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
					{
						var groupebByMaterial = gewerk1Entity.Select(x => x.Cislo_Material).Distinct().ToList();
						foreach(var p in groupebByMaterial)
						{
							var list = gewerk1Entity.Where(x => x.Cislo_Material == p).ToList();
							worksheet.Cells[rowNumber, startColumnNumber].Value = p + ":";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Size = 13;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Color.SetColor(Color.Blue);
							worksheet.Row(rowNumber).Height = 25;
							rowNumber += 1;
							foreach(var l in list)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.FA_Cislo;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Freigabestatus;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.FA_Mnostvi;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Cislo_Zbozi;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = string.Format("{0:n}", Math.Round(l.Bedarf.Value));
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = (l.P3000.HasValue) ? string.Format("{0:n}", Math.Round(l.P3000.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (l.Im_Lager.HasValue) ? string.Format("{0:n}", Math.Round(l.Im_Lager.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (l.In_derProduktion.HasValue) ? string.Format("{0:n}", Math.Round(l.In_derProduktion.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.RestBestand;
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = (l.Termin_Rezarna.HasValue) ? l.Termin_Rezarna.Value.ToString("dd-MMMM-yyyy") : "";
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Material;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.Typ_Material;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.Gewerk_1;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = l.Gewerk_2;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = l.Gewerk_3;
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = l.FA_begonnen.HasValue ? l.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}

					// Doc content
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
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
					package.Workbook.Properties.Title = "Plannung Gewerk1  CZ";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(8).Width = 20;
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 20;
					worksheet.Column(11).Width = 50;
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
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAAnalayseGewerk1TNEntity> gewerk1Entity, int lager)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				string lg = ((Enums.FAEnums.FaLands)lager).GetDescription();

				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung Gewerk1 {lg}-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");


				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Gewerk1 {lg}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 17;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "ROHArtikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Fertigungsnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamt Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "P3000";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Ve_skladu";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Ve_vyrobe";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Restbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Termin_Schneiderei";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Typ_Material";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Gewerk 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Gewerk 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Gewerk 3";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "FA begonnen";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
					{
						var groupebByMaterial = gewerk1Entity.Select(x => x.ROH_Artikelnummer).Distinct().ToList();
						foreach(var p in groupebByMaterial)
						{
							var list = gewerk1Entity.Where(x => x.ROH_Artikelnummer == p).OrderBy(y => y.Termin_Schneiderei).ToList();
							worksheet.Cells[rowNumber, startColumnNumber].Value = p + ":";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Size = 13;
							worksheet.Cells[rowNumber, startColumnNumber].Style.Font.Color.SetColor(Color.Blue);
							worksheet.Row(rowNumber).Height = 25;
							rowNumber += 1;
							foreach(var l in list)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Fertigungsnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Freigabestatus;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Gesamt_Menge;
								worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Artikelnummer;
								worksheet.Cells[rowNumber, startColumnNumber + 5].Value = string.Format("{0:n}", l.Bedarf.Value);
								worksheet.Cells[rowNumber, startColumnNumber + 6].Value = (l.In_P3000.HasValue) ? string.Format("{0:n}", Math.Round(l.In_P3000.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 7].Value = (l.Ve_skladu.HasValue) ? string.Format("{0:n}", Math.Round(l.Ve_skladu.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 8].Value = (l.Ve_vyrobe.HasValue) ? string.Format("{0:n}", Math.Round(l.Ve_vyrobe.Value)) : "";
								worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.RestBestand != 0 ? l.RestBestand.ToString() : "";
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = (l.Termin_Schneiderei.HasValue) ? l.Termin_Schneiderei.Value.ToString("dd-MMMM-yyyy") : "";
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Material;
								worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.Typ_Material;
								worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.Gewerk_1;
								worksheet.Cells[rowNumber, startColumnNumber + 14].Value = l.Gewerk_2;
								worksheet.Cells[rowNumber, startColumnNumber + 15].Value = l.Gewerk_3;
								worksheet.Cells[rowNumber, startColumnNumber + 16].Value = l.FA_begonnen.HasValue ? l.FA_begonnen.Value.ToString("dd-MMMM-yyyy") : "";

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
						}
					}

					// Doc content
					if(gewerk1Entity != null && gewerk1Entity.Count > 0)
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
					worksheet.Column(8).Width = 20;
					worksheet.Column(9).Width = 20;
					worksheet.Column(10).Width = 20;
					worksheet.Column(11).Width = 50;
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