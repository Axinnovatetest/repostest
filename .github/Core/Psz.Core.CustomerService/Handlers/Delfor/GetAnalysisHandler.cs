using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetAnalysisHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<AnalysisResponseModel>>
	{

		private AnalysisRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetAnalysisHandler(Identity.Models.UserModel user, AnalysisRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public async Task<ResponseModel<AnalysisResponseModel>> HandleAsync()
		{
			try
			{
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				var sortFieldName = "";
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					switch(this._data.SortField.ToLower())
					{
						default:
						case "artikelnummer":
							sortFieldName = "[BuyerPartyName]";
							break;
					}
				}
				#endregion

				var results = Infrastructure.Data.Access.Joins.CTS.DelforStatistics.GetAnalysis(this._data.IsManual, this._data.CustomerNumber, this._data.DocumentNumber, this._data.OnlyLastVersion ?? true, this._data.OnlyOpen ?? true, sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = await Infrastructure.Data.Access.Joins.CTS.DelforStatistics.GetAnalysis_count(this._data.IsManual, this._data.CustomerNumber, this._data.DocumentNumber, this._data.OnlyLastVersion ?? true, this._data.OnlyOpen ?? true);
				//- 
				var responseBody = new AnalysisResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new AnalysisResponseModel.AnalysisItem(x))?.ToList();

				return ResponseModel<AnalysisResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public Task<ResponseModel<AnalysisResponseModel>> ValidateAsync()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<AnalysisResponseModel>.AccessDeniedResponseAsync();
			}

			return ResponseModel<AnalysisResponseModel>.SuccessResponseAsync();
		}
		public async Task<byte[]> GetDataXLS()
		{
			try
			{
				// - XLS retrieve all data
				this._data.PageSize = -1;
				var data = await this.HandleAsync();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Analysis-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					if(this._data.SplitCustomers == true)
					{
						var uniqueCustomers = data.Body.Items?.Select(x => x.BuyerPartyName).Distinct().ToList();
						if(uniqueCustomers != null && uniqueCustomers.Count > 0)
						{
							foreach(var item in uniqueCustomers)
							{
								AddWorkSheet(data.Body.Items?.Where(x => x.BuyerPartyName == item)?.ToList(), item, package);
							}
						}
						else
						{
							AddWorkSheet(null, "", package);
						}
					}
					else
					{
						AddWorkSheet(data.Body.Items, "", package);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"LP Auswertung";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

					// save our new workbook and we are done!
					package.Save();

					return await File.ReadAllBytesAsync(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

		private static void AddWorkSheet(List<AnalysisResponseModel.AnalysisItem> items, string customerName, ExcelPackage package)
		{
			// add a new worksheet to the empty workbook
			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{(!string.IsNullOrWhiteSpace(customerName) ? $"{customerName.Substring(0, Math.Min(customerName.Length, 30))}" : "LP Auswertung")}");

			// Keep track of the row that we're on, but start with four to skip the header
			var headerRowNumber = 2;
			var startColumnNumber = 1;
			var numberOfColumns = 19;

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
			worksheet.Cells[1, 1].Value = $"LP Auswertung - {DateTime.Now.ToString("dd/MM/yyyy")} {(!string.IsNullOrWhiteSpace(customerName) ? $" - {customerName}" : "")}";
			worksheet.Cells[1, 1].Style.Font.Size = 18;



			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Kunde";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Dokument-Nr./Delfor Nr.";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Position";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "RSD";
			worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikelnummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bezeichnung 1 = Kundenartikelnummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Menge";
			worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Kumulierte Menge";
			worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Jahr";
			worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "KW";
			worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "VKE";
			worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Gesamtpreis";
			worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Letzte ASN Nr.";
			worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Zuletzt erhaltene Menge ";
			worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Letzte ASN Datum ";
			worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Datum ";
			worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "AB Total Qty";
			worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Remainder";
			worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "Consignee";


			var rowNumber = headerRowNumber + 1;
			if(items != null && items.Count > 0)
			{
				// Loop through 
				foreach(var w in items)
				{
					worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.BuyerPartyName;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.DocumentNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.PositionNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
					worksheet.Cells[rowNumber, startColumnNumber + 3].Formula = w?.PlanningQuantityRequestedShipmentDate.HasValue == true ? $"{Common.Helpers.Formatters.XLS.GetDateFormula((DateTime)w?.PlanningQuantityRequestedShipmentDate.Value)}" : "";
					worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.SuppliersItemMaterialNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Bezeichnung1;
					worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.PlanningQuantityQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.PlanningQuantityCumulativeQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.RSDYear;
					worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.RSDWeek;
					worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.UnitPrice;
					worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.TotalPrice;
					worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.LastASNNumber;
					worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.LastReceivedQuantity;
					worksheet.Cells[rowNumber, startColumnNumber + 14].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
					worksheet.Cells[rowNumber, startColumnNumber + 14].Formula = w?.LastASNDate.HasValue == true ? $"{Common.Helpers.Formatters.XLS.GetDateFormula((DateTime)w?.LastASNDate.Value)}" : "";
					worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
					worksheet.Cells[rowNumber, startColumnNumber + 15].Formula = w?.ReceivingDate.HasValue == true ? $"{Common.Helpers.Formatters.XLS.GetDateFormula((DateTime)w?.ReceivingDate.Value)}" : "";
					worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.AbTotalQty;
					worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					worksheet.Cells[rowNumber, startColumnNumber + 17].Value = (w?.PlanningQuantityQuantity ?? 0) - (w?.AbTotalQty ?? 0);
					worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
					worksheet.Cells[rowNumber, startColumnNumber + 18].Value = w?.ConsigneePartyName;

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
			if(items != null && items.Count > 0)
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
			worksheet.Column(2).Width = 50; // force width for 2nd column 
		}
	}
}
