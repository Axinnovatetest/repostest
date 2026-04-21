using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAProduktionPlannungHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private DateTime Datum_bis { get; set; }
		private string Produktionsort { get; set; }
		private bool? Technikauftrag { get; set; }
		private string Artikelnummer { get; set; }
		public GetFAProduktionPlannungHandler(DateTime Datum_bis, string Produktionsort, bool? Technikauftrag, string Artikelnummer)
		{
			this.Datum_bis = Datum_bis;
			this.Produktionsort = Produktionsort;
			this.Technikauftrag = Technikauftrag;
			this.Artikelnummer = Artikelnummer;
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
				string myTime = Datum_bis.ToString("dd/MM/yyyy");
				var lager = this.Produktionsort == "null" ? "" : this.Produktionsort;
				var produktionPlannungEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetProduktionPlannung(Artikelnummer, myTime, lager, (bool)Technikauftrag);

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(produktionPlannungEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: Datum_bis:{Datum_bis},Produktionsort:{Produktionsort},Technikauftrag:{Technikauftrag},Artikelnummer:{Artikelnummer}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal static byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FAProduktionPlannungEntity> produktionPlannung)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Produktions-Plannung-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Produktions-Plannung");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 23;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "StavPlanovani/Status";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Zákaznik/Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Atribut";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "?isloZakázky/FA#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "TerminZákaznika/Kundentermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "TerminVýroba/PlanterminTermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Komentá?1/Bemerkung1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Komentá?2/Bemerkung2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Sonderfertigung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Komentá?ZS/BemerkungCS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "OriginalMnožství/Originalmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "VyvezenéMnožství/Mengeerledigt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "OtMnožství/Mengeoffen";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "?isloSysmo/Sysmo#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "?isloPSZ/PSZNummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Stav/Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "?asnaZakázku/FAZeit";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Peníze/FALohnPeníze/FALohn";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "VyvezenéMnožstvíman";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Index";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Indexdatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Technik";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Prio";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						foreach(var p in produktionPlannung)
						{
							//
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Status;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Atribut;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.FA.HasValue ? p.FA.Value : 0;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Kundentermin.HasValue ? p.Kundentermin.Value.ToString("dd/MM/yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Plantermin.HasValue ? p.Plantermin.Value.ToString("dd/MM/yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Bemerkung1;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Bemerkung2;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Sonderfertigung.HasValue ? p.Sonderfertigung.Value ? "YES" : "NO" : "NO";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Bemerkung_CS;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Originalmenge.HasValue ? p.Originalmenge.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Menge_erledigt.HasValue ? p.Menge_erledigt.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Menge_offen.HasValue ? p.Menge_offen.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Sysmo;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.PSZ_Nummer;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = p.FA_Zeit.HasValue ? p.FA_Zeit.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = p.FA_Lohn.HasValue ? p.FA_Lohn.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = p.man.HasValue ? p.man.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = p.Index;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = p.Indexdatum.HasValue ? p.Indexdatum.Value.ToString("dd/MM/yyyy") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = p.Technik.HasValue ? p.Technik.Value ? "YES" : "NO" : "NO";
							if(p.Prio.HasValue && p.Prio.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								worksheet.Cells[rowNumber, startColumnNumber + 22].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 22].Style.Font.Color.SetColor(Color.White);
								worksheet.Cells[rowNumber, startColumnNumber + 22].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
								worksheet.Cells[rowNumber, startColumnNumber + 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
							}

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							//range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							//range.Style.Fill.BackgroundColor.SetColor(Color.White);
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
					package.Workbook.Properties.Title = "Produktion Plannung";
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