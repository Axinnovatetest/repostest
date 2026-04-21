using OfficeOpenXml;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class NeededQuantityOrderedQuantityAnalysisHandler: IHandle<NeededQuantityOrderedQuantityAnalysisRequestModel, ResponseModel<NeedStockPerTypeResponseModel>>
	{
		private NeededQuantityOrderedQuantityAnalysisRequestModel data { get; set; }
		private UserModel user { get; set; }

		public NeededQuantityOrderedQuantityAnalysisHandler(UserModel user, NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<NeedStockPerTypeResponseModel> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}
				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<NeedStockPerTypeResponseModel> Perform(UserModel user, NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			#region > Data sorting & paging
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this.data.PageSize > 0 ? (this.data.RequestedPage * this.data.PageSize) : 0,
				RequestRows = this.data.PageSize
			};

			var sortFieldName = "";
			if(!string.IsNullOrWhiteSpace(this.data.SortField))
			{
				switch(this.data.SortField.ToLower())
				{
					default:
					case "artikelnummer":
						sortFieldName = "[ArtikelNummer]";
						break;
					case "bestell_nr":
						sortFieldName = "[Bestell_Nr]";
						break;
					case "diffprice":
						sortFieldName = "[DiffPrice]";
						break;
					case "diffquantity":
						sortFieldName = "[DiffQuantity]";
						break;
					case "einkaufspreis":
						sortFieldName = "[Einkaufspreis]";
						break;
					case "gesamtpreis":
						sortFieldName = "[Gesamtpreis]";
						break;
					case "name1":
						sortFieldName = "[Name1]";
						break;
					case "roh_stock":
						sortFieldName = "[ROH_Stock]";
						break;
					case "roh_quantity":
						sortFieldName = "[roh_quantity]";
						break;
					case "wert_lagerstockneed":
						sortFieldName = "[Wert_LagerStockNeed]";
						break;
				}
			}
			#endregion

			//  - 2023-02-14 - CHECK
			this.data.UnitId = null;
			this.data.CountryID = null;

			var date = DateTime.Today.AddMonths(data.Months);
			var lagerIds = Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryID, data.UnitId)?.ToList();

			var results = Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysis(this.data.FilterTerms, date, this.data.IsExtra.Value ? true : false, lagerIds, sortFieldName, this.data.SortDesc, this.data.RequestedPage, this.data.PageSize, this.data.FullData);
			var allCount = Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetCTSNeedAnalysis_count(this.data.FilterTerms, date, this.data.IsExtra.Value ? true : false, lagerIds);
			var responseBody = new NeedStockPerTypeResponseModel();

			responseBody.DateTill = date;
			responseBody.TotalCount = allCount;
			responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this.data.PageSize <= 0 ? 1 : this.data.PageSize));
			responseBody.PageSize = this.data.PageSize;
			responseBody.PageRequested = this.data.RequestedPage;
			responseBody.Items = results?.Select(x => new NeedStockItemModel(x))?.ToList();

			return ResponseModel<NeedStockPerTypeResponseModel>.SuccessResponse(responseBody);
		}
		public ResponseModel<NeedStockPerTypeResponseModel> Validate()
		{
			if(this.user is null)
			{
				return ResponseModel<NeedStockPerTypeResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<NeedStockPerTypeResponseModel>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var extraData = new ResponseModel<NeedStockPerTypeResponseModel>();
				var missingData = new ResponseModel<NeedStockPerTypeResponseModel>();
				this.data.FullData = true;
				if(this.data.IsExtra.HasValue == false)
				{
					this.data.IsExtra = true;
					extraData = this.Handle();

					// -
					this.data.IsExtra = false;
					missingData = this.Handle();
				}
				else
				{
					if(this.data.IsExtra.Value)
					{
						extraData = this.Handle();
					}
					else
					{
						missingData = this.Handle();
					}
				}

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"BedarfAnalyse-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					if(extraData != null && extraData.Success == true)
					{
						addWorkSheet(extraData.Body.Items, "Extra ROH", extraData.Body.DateTill, package, true);
					}
					if(missingData != null && missingData.Success == true)
					{
						addWorkSheet(missingData.Body.Items, "Missing ROH", missingData.Body.DateTill, package, false);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"BedarfAnalyse";
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

		private void addWorkSheet(List<NeedStockItemModel> data, string titie, DateTime dateTill, ExcelPackage package, bool IsExtra)
		{
			// add a new worksheet to the empty workbook
			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{titie}");

			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 2;
			var startColumnNumber = 1;
			var numberOfColumns = 10;

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
			worksheet.Cells[1, 1].Value = $"Bedarf Analyse - {titie}  bis {dateTill.ToString("dd-MM-yyyy")}";
			worksheet.Cells[1, 1].Style.Font.Size = 16;

			var rowNumber = 1;
			#region Items
			//if(data.Success == true)
			{
				headerRowNumber = rowNumber + 1;
				// Start adding the header
				worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
				worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "ROH Bestand";
				worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Einkaufspreis";
				worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gesamtpreis";
				worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lieferant";
				worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestell-Nr";
				worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "ROH Quantity";
				worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Wert LagerBestandBedarf";
				worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Diff Quantity";
				worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Diff Price";

				rowNumber = headerRowNumber + 1;
				if(data != null && data.Count > 0)
				{
					var stockAmount = 0m;
					var needAmount = 0m;
					var needMissingAmount = 0m;
					//Loop through
					foreach(var w in data)
					{
						worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.ROH_Stock;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Einkaufspreis;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Gesamtpreis;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Name1;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bestell_Nr;
						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.ROH_Quantity;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Wert_LagerStockNeed;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.DiffQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.DiffPrice;

						stockAmount += w?.Gesamtpreis ?? 0;
						needAmount += w?.Wert_LagerStockNeed ?? 0;
						needMissingAmount += w?.DiffPrice ?? 0;

						worksheet.Row(rowNumber).Height = 18;
						rowNumber += 1;
					}

					if(IsExtra)
					{
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11].Value = "Wert Lager Bestand";
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + 11].Value = $"Wert Bedarf bis {dateTill.ToString("dd-MM-yyyy")} ";
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + 11].Value = "Unnötig";

						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 12].Value = stockAmount;
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + 12].Value = needAmount;
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + 12].Value = stockAmount - needAmount;

						// - 
						using(var range = worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11, headerRowNumber + 4, startColumnNumber + 12])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							// Thick countour
							range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
						}
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						worksheet.Cells[headerRowNumber + 2, startColumnNumber + 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						worksheet.Cells[headerRowNumber + 4, startColumnNumber + 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						// -
						worksheet.Column(startColumnNumber + 11).AutoFit();
						worksheet.Column(startColumnNumber + 12).AutoFit();
					}
					else
					{
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11].Value = "Need/Missing";

						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 12].Value = needMissingAmount;
						// - 
						using(var range = worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11, headerRowNumber + 0, startColumnNumber + 12])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							// Thick countour
							range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
						}
						worksheet.Cells[headerRowNumber + 0, startColumnNumber + 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						// -
						worksheet.Column(startColumnNumber + 11).AutoFit();
						worksheet.Column(startColumnNumber + 12).AutoFit();
					}
				}
			}
			#endregion items

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
			if(data != null && data.Count > 0)
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
		}
	}
}
