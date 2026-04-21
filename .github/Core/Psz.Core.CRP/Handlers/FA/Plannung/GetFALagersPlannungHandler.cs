using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.Globalization;


namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFALagersPlannungHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private DateTime Datum_bis { get; set; }
		private string Produktionsort { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFALagersPlannungHandler(DateTime Datum_bis, string Produktionsort, Identity.Models.UserModel user)
		{
			this.Datum_bis = Datum_bis;
			this.Produktionsort = Produktionsort;
			this._user = user;
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
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
				var lagersPlannungEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetLagersProduktionPlannung(myTime, lager);

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(lagersPlannungEntity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: Datum_bis:{Datum_bis},Produktionsort:{Produktionsort}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		internal byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FALagersPlannungEntity> produktionPlannung)
		{
			if(this.Produktionsort == "6")
			{
				return SaveToExcelFile_CZ(produktionPlannung);
			}

			// - 
			try
			{
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung TN-WS-CZ-AL");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 41;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(2).Height = 20;
					using(var range = worksheet.Cells[1, 1, 1, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Werk";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Planungsstatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Customer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Atribut";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Short";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Comment 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Comment 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "FA Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Shipped Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Open Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "PN PSZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Status TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Costs";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Shipped Qty Man";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Kommisioniert_teilweise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "Kommisioniert_komplett";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "Kabel_geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "Kabel_geschnitten_Datum";

					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "Termin Werk";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 24].Value = "KW";
					worksheet.Cells[headerRowNumber, startColumnNumber + 25].Value = "FA_Druckdatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 26].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 27].Value = "Wish Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 28].Value = "Bemerkung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 29].Value = "Gewerk_Teilweise_Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 30].Value = "Verpackungsart";
					worksheet.Cells[headerRowNumber, startColumnNumber + 31].Value = "Verpackungsmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 32].Value = "Losgroesse";
					worksheet.Cells[headerRowNumber, startColumnNumber + 33].Value = "Techniker";

					worksheet.Cells[headerRowNumber, startColumnNumber + 34].Value = "Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 35].Value = "Technik Kontakt TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 36].Value = "Status Intern";
					worksheet.Cells[headerRowNumber, startColumnNumber + 37].Value = "erstelldatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 38].Value = "Bemerkung_Kommissionierung_AL";
					worksheet.Cells[headerRowNumber, startColumnNumber + 39].Value = "Prio";
					worksheet.Cells[headerRowNumber, startColumnNumber + 40].Value = "Typ";


					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR")
						{
							DateTimeFormat = { YearMonthPattern = "dd/MM/yyyy" }
						};
						foreach(var p in produktionPlannung)
						{
							//
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.Werk;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Planungsstatus;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Customer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.CS_Kontakt;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.PB;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Atribut;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Short;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.FA_Number.HasValue ? p.FA_Number.Value : 0;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Comment_1;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Comment_2;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.FA_Qty.HasValue ? p.FA_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Shipped_Qty.HasValue ? p.Shipped_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Open_Qty.HasValue ? p.Open_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.PN_PSZ;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Status_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.Order_Time.HasValue ? p.Order_Time.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = p.Costs.HasValue ? p.Costs.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = p.Shipped_Qty_Man.HasValue ? p.Shipped_Qty_Man.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = p.Kommisioniert_teilweise.HasValue && p.Kommisioniert_teilweise.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 20].Value = p.Kabel_geschnitten.HasValue && p.Kabel_geschnitten.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 21].Value = p.Kabel_geschnitten_Datum.HasValue ? p.Kabel_geschnitten_Datum.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 21].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 22].Value = p.Termin_Werk.HasValue ? p.Termin_Werk.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 22].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 23].Value = p.Ack_Date.HasValue ? p.Ack_Date.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 23].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 24].Value = p.KW.HasValue ? p.KW.Value : 0;
							worksheet.Cells[rowNumber, startColumnNumber + 25].Value = p.FA_Druckdatum.HasValue ? p.FA_Druckdatum.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 25].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 26].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 27].Value = p.Wish_Date.HasValue ? p.Wish_Date.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 27].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 28].Value = p.Bemerkung;
							worksheet.Cells[rowNumber, startColumnNumber + 29].Value = p.Gewerk_Teilweise_Bemerkung;
							worksheet.Cells[rowNumber, startColumnNumber + 30].Value = p.Verpackungsart;
							worksheet.Cells[rowNumber, startColumnNumber + 31].Value = p.Verpackungsmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 32].Value = p.Losgroesse;
							worksheet.Cells[rowNumber, startColumnNumber + 33].Value = p.Techniker;
							worksheet.Cells[rowNumber, startColumnNumber + 34].Value = p.Technik_Kontakt;
							worksheet.Cells[rowNumber, startColumnNumber + 35].Value = p.Technik_Kontakt_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 36].Value = p.Status_Intern;
							worksheet.Cells[rowNumber, startColumnNumber + 37].Value = p.erstelldatum.HasValue ? p.erstelldatum.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 37].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 38].Value = p.Bemerkung_Kommissionierung_AL;
							worksheet.Cells[rowNumber, startColumnNumber + 40].Value = p.FertigungType;
							if(p.Prio.HasValue && p.Prio.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 39].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								worksheet.Cells[rowNumber, startColumnNumber + 39].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 39].Style.Font.Color.SetColor(Color.White);
								worksheet.Cells[rowNumber, startColumnNumber + 39].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
								worksheet.Cells[rowNumber, startColumnNumber + 39].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
							}

							// - 
							worksheet.Cells[rowNumber, startColumnNumber + 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Style.Fill.BackgroundColor.SetColor(p.Kommisioniert_teilweise.HasValue && p.Kommisioniert_teilweise.Value ? Color.LightGreen : Color.OrangeRed);

							worksheet.Cells[rowNumber, startColumnNumber + 19].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Style.Fill.BackgroundColor.SetColor(p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value ? Color.LightGreen : Color.OrangeRed);

							worksheet.Cells[rowNumber, startColumnNumber + 20].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 20].Style.Fill.BackgroundColor.SetColor(p.Kabel_geschnitten.HasValue && p.Kabel_geschnitten.Value ? Color.LightGreen : Color.OrangeRed);


							// -
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
					package.Workbook.Properties.Title = "Plannung TN/WS/CZ/AL";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					worksheet.Column(9).Width = 50;
					worksheet.Column(10).Width = 50;
					worksheet.Column(29).Width = 50;
					// save our new workbook and we are done!
					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		internal byte[] SaveToExcelFile_CZ(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FALagersPlannungEntity> produktionPlannung)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Plannung TN-WS-CZ-AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var countries = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get();

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung TN-WS-CZ-AL");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 20;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.DefaultColWidth = 12;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					using(var range = worksheet.Cells[1, 1, 1, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Customer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "PB";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Prio";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "FA Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Shipped Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Open Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Vývoz";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "PN PSZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Sklad";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Rezarna";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "FA_Druckdatum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Status Intern";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Termin CZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Poznamka";
					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "CW";

					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "CS Kontakt";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR")
						{
							DateTimeFormat = { YearMonthPattern = "dd/MM/yyyy" }
						};
						foreach(var p in produktionPlannung)
						{
							//
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.Customer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.PB;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.FA_Number.HasValue ? p.FA_Number.Value : 0;
							if(p.Prio.HasValue && p.Prio.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								worksheet.Cells[rowNumber, startColumnNumber + 3].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Font.Color.SetColor(Color.White);
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
								worksheet.Cells[rowNumber, startColumnNumber + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
							}

							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.FA_Qty.HasValue ? p.FA_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Shipped_Qty.HasValue ? p.Shipped_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Open_Qty.HasValue ? p.Open_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.PN_PSZ;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Order_Time.HasValue ? p.Order_Time.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value ? "VRAI" : "NE";
							if(p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Font.Color.SetColor(Color.Green);
							}
							else
							{
								worksheet.Cells[rowNumber, startColumnNumber + 10].Value = "NE";
							}
							if(p.Kabel_geschnitten.HasValue && p.Kabel_geschnitten.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Font.Color.SetColor(Color.Green);
							}
							else
							{
								worksheet.Cells[rowNumber, startColumnNumber + 11].Value = "NE";
							}
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Ack_Date.HasValue ? p.Ack_Date.Value : null; //.HasValue ? p.Ack_Date.Value.ToString("dd/MM/yyyy").Trim().Replace("\n", "") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.FA_Druckdatum.HasValue ? p.FA_Druckdatum.Value : null; //.HasValue ? p.FA_Druckdatum.Value.ToString("dd/MM/yyyy").Trim().Replace("\n", "") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.Status_Intern;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = p.Termin_Werk;
							worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 17].Value = p.Comment_1;
							worksheet.Cells[rowNumber, startColumnNumber + 18].Value = p.Ack_Date.HasValue ? Helpers.SpecialHelper.FetIso8601WeekOfYear(p.Ack_Date.Value) : null;
							worksheet.Cells[rowNumber, startColumnNumber + 19].Value = p.CS_Kontakt;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					// Doc content
					if(produktionPlannung != null && produktionPlannung.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
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
						// -2023-12-07 - Hejdukova
						worksheet.Column(i).BestFit = true;
					}
					worksheet.Column(2).Width = 5;
					worksheet.Column(11).Width = 5;
					worksheet.Column(12).Width = 5;

					// Set some document properties
					package.Workbook.Properties.Title = "Plannung TN/WS/CZ/AL";
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