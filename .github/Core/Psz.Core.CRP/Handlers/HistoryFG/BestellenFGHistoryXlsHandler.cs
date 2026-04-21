using System.Drawing;
using FastReport.Data;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class BestellenFGHistoryXlsHandler
	{
		private readonly FgHistoryBestandXlsStatisticsModel? _data;
		private Identity.Models.UserModel _user { get; set; }
		public BestellenFGHistoryXlsHandler(FgHistoryBestandXlsStatisticsModel? data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				return ResponseModel<byte[]>.SuccessResponse(GetData());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

		public byte[] GetData()
		{
			var BestellenFgHistoryEntity = Infrastructure.Data.Access.Joins.CRP.BestellenFGHistoryAccess.GetBestellenFG(_data.From, _data.To, _data.AdressCustomerNumber);

			return SaveToExcelFile(BestellenFgHistoryEntity);
		}

		internal byte[] SaveToExcelFile(List<Infrastructure.Data.Entities.Joins.CRP.BestellenFgHistoryEntity> BestellenFgHistoryEntity
			)
		{
			try
			{
				var chars = new char[] { ' ', '#' };
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Historie-FG-Bestand-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Historie FG Bestand");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 15;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Historie FG Bestand {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung  1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Freigabestatus";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "CS Kontakt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Lagerort";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "VK Gesamt";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Kosten gesamt (mit CU)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Kosten gesamt (ohne CU)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "VKE";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "UBG";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "STD EDI";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Datum";

					var rowNumber = headerRowNumber + 1;

					if(BestellenFgHistoryEntity != null && BestellenFgHistoryEntity.Count > 0)
					{
						// Loop through data
						foreach(var w in BestellenFgHistoryEntity)
						{

							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.ArticleNumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.CustomerName;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.ArticleDesignation1;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.ArticleDesignation2;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.ArticleReleaseStatus;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.CsContact ?? "N/V";
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.WarehouseName ?? "N/V";
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.StockQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.TotalSalesPrice;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.TotalCostsWithCu ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.TotalCostsWithoutCu ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.UnitSalesPrice ?? 0;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.UBG.HasValue == true ? (w.UBG.Value ? "Yes" : "No") : "N/V";
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.EdiStandard.HasValue == true ? (w.EdiStandard.Value ? "Yes" : "No") : "N/V";
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = (bool)(w?.ImportDate.HasValue) ? w.ImportDate.Value.ToString("dd.MM.yyyy") : "N/V";
							worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}


					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Doc content
					if(BestellenFgHistoryEntity != null && BestellenFgHistoryEntity.Count > 0)
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
					package.Workbook.Properties.Title = $"Historie FG Bestand";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

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



