using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ProdWip2downloadhandler
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public ProdWip2downloadhandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			_data = data;
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

				var ProdWipList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetWipP2rodDataForXls(this._data)
					?.DistinctBy(x=> x.FA)?.ToList();

				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(ProdWipList));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}
		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity> steps)
		{
			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage())
				{
					// create worksheet
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("WIP History");

					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 9; // Removed "Fertigungsgrad %"

					// formatting
					worksheet.TabColor = Color.Green;
					worksheet.DefaultRowHeight = 15;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now:yyyy-MM-dd}";
					worksheet.Row(1).Height = 28;
					worksheet.Row(headerRowNumber).Height = 22;

					// pre-header title
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"WIP History {DateTime.Now:dd/MM/yyyy}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;
					worksheet.Cells[1, 1].Style.Font.Bold = true;
					worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));

					// header row
					string[] headers =
					{
						"Fertigungsauftrag",
						"FA-Offene-Menge pro Bereich",
						"Artikel-Nr",
						"FA Date",
						"Bereit (%)",
						"Montage",
						"Crimp",
						"Electrical Inspection",
						"Optical Inspection"
					};

					for(int i = 0; i < headers.Length; i++)
					{
						worksheet.Cells[headerRowNumber, startColumnNumber + i].Value = headers[i];
					}

					// style header row
					using(var range = worksheet.Cells[headerRowNumber, startColumnNumber, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT)); // light green
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// fill rows
					var rowNumber = headerRowNumber + 1;
					if(steps != null && steps.Count > 0)
					{
						foreach(var step in steps)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = step.FA;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = step.OpenQty != 0 ? step.OpenQty.ToString() : "";
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = step.Item ?? "";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = step.Due != DateTime.MinValue ? step.Due.ToString("yyyy-MM-dd") : "";
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = step.UserPrepedPercent != 0 ? step.UserPrepedPercent.ToString() : "";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = step.UserAssembledPercent != 0 ? step.UserAssembledPercent.ToString() : "";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = step.UserCrimpedPercent != 0 ? step.UserCrimpedPercent.ToString() : "";
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = step.UserElectricalInspectedPercent != 0 ? step.UserElectricalInspectedPercent.ToString() : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = step.UserOpticalInspectedPercent != 0 ? step.UserOpticalInspectedPercent.ToString() : "";


							worksheet.Row(rowNumber).Height = 18;
							rowNumber++;
						}
					}

					// borders + autofit
					using(var range = worksheet.Cells[headerRowNumber, startColumnNumber, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// - Legend
					worksheet.Cells["L2"].Style.Font.Bold = true;
					worksheet.Cells["L2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Cells["L2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 239, 206)); // light green
					worksheet.Cells["L2"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					worksheet.Cells["L2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					worksheet.Cells["L2"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					worksheet.Cells["L2"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

					worksheet.Cells["L2"].Value = "Legend";
					worksheet.Cells["L3"].Value = "Bereit";
					worksheet.Cells["L4"].Value = "Montage";
					worksheet.Cells["L5"].Value = "Crimp";
					worksheet.Cells["L6"].Value = "Electrical Inspection";
					worksheet.Cells["L7"].Value = "Optical Inspection";
					// -
					worksheet.Cells["M3"].Value = "Prozentanteil eingabe";
					worksheet.Cells["M4"].Value = "Menge eingabe";
					worksheet.Cells["M5"].Value = "Menge eingabe";
					worksheet.Cells["M6"].Value = "Menge eingabe";
					worksheet.Cells["M7"].Value = "Menge eingabe";
					using(var range = worksheet.Cells["L3:M7"])
					{
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.AutoFitColumns();
					}

					for(int i = 1; i <= numberOfColumns; i++)
						worksheet.Column(i).AutoFit();

					// properties
					package.Workbook.Properties.Title = "WIP History Report";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					return package.GetAsByteArray(); // direct byte array, no temp file
				}
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
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
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
