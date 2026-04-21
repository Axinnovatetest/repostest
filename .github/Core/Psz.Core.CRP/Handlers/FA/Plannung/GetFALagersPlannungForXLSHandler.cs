using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;


namespace Psz.Core.CRP.Handlers.FA
{
	public enum LieferlistTables
	{
		[Description("[PSZTN_Lieferliste täglich]")]
		TN = 7,
		[Description("[PSZBETN_Lieferliste täglich]")]
		BETN = 60,
		[Description("[PSZKsarHelal_Lieferliste täglich]")]
		WSTN = 42,
		[Description("[PSZGZTN_Lieferliste täglich]")]
		GZTN = 102,
		[Description("[PSZAL_Lieferliste täglich]")]
		AL = 26,
		[Description("[PSZ_Lieferliste täglich]")]
		CZ = 6,
		[Description("[PSZ_Einlagerung_täglich]")]
		DE = 15,
	}
	public class GetFALagersPlannungForXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private DateTime Datum_bis { get; set; }
		private string Produktionsort { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFALagersPlannungForXLSHandler(DateTime Datum_bis, string Produktionsort, Identity.Models.UserModel user)
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
				var lagersPlannungControlledQtyEntities = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetLagersProduktionPlannungControlledQty(int.TryParse(lager, out int r) ? r : null, lagersPlannungEntity?.Select(x => x.FA_Number ?? -1)?.Distinct());

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(lagersPlannungEntity, lagersPlannungControlledQtyEntities));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: Datum_bis:{Datum_bis},Produktionsort:{Produktionsort}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		internal byte[] SaveToExcelFile(
			List<Infrastructure.Data.Entities.Joins.FAPlannung.FALagersPlannungEntity> produktionPlannung,
			IEnumerable<KeyValuePair<int, decimal>> lagersPlannungControlledQtyEntities)
		{
			// - 
			try
			{
				ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Plannung Werke");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 18;

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
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Customer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Short";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "FA Number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Comment 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Controlled Qty"; // - new data
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Open Qty";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "PN PSZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Status TN";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Order Time";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Kommisioniert_teilweise";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kommisioniert_komplett";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Kabel_geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Ack Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "KW";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Wish Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Techniker";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Prio";

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
							var qty = lagersPlannungControlledQtyEntities?.Where(x => x.Key == p.FA_Number)?.Sum(x => x.Value);
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = p.Customer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.Short;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.FA_Number.HasValue ? p.FA_Number.Value : 0;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Comment_1;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = qty.HasValue ? qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.Open_Qty.HasValue ? p.Open_Qty.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.PN_PSZ;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.Status_TN;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.Order_Time.HasValue ? p.Order_Time.Value : 0m;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Kommisioniert_teilweise.HasValue && p.Kommisioniert_teilweise.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Kabel_geschnitten.HasValue && p.Kabel_geschnitten.Value ? "VRAI" : "FAUX";
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Ack_Date.HasValue ? p.Ack_Date.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.KW.HasValue ? p.KW.Value : 0;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Freigabestatus;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = p.Wish_Date.HasValue ? p.Wish_Date.Value : null;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.YearMonthPattern.Trim().Replace("\n", "");
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = p.Techniker;
							if(p.Prio.HasValue && p.Prio.Value)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								worksheet.Cells[rowNumber, startColumnNumber + 17].Value = "\u2713";
								worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Color.SetColor(Color.White);
								worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
								worksheet.Cells[rowNumber, startColumnNumber + 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
								;
							}
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Fill.BackgroundColor.SetColor(p.Kommisioniert_teilweise.HasValue && p.Kommisioniert_teilweise.Value ? Color.LightGreen : Color.OrangeRed);

							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Fill.BackgroundColor.SetColor(p.Kommisioniert_komplett.HasValue && p.Kommisioniert_komplett.Value ? Color.LightGreen : Color.OrangeRed);

							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Fill.BackgroundColor.SetColor(p.Kabel_geschnitten.HasValue && p.Kabel_geschnitten.Value ? Color.LightGreen : Color.OrangeRed);


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
					worksheet.Column(3).Width = 50;

					// Set some document properties
					package.Workbook.Properties.Title = "Plannung Werke";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!
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