using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ReportTwoTblHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model>>
	{
		private Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwoRequestTblModel _data;
		private Core.Identity.Models.UserModel _user;
		public ReportTwoTblHandler(Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwoRequestTblModel request, Core.Identity.Models.UserModel user)
		{
			_data = request;
			_user = user;
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var ReportTwoDataList = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportTwoTblEntity>();
				string searchValue = this._data.SearchValue;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					var sortFieldName = "";
					switch(this._data.SortField.ToLower())
					{
						default:
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "mengeinproduktion":
							sortFieldName = "[MengeInProduktion]";
							break;
						case "lagerbestand":
							sortFieldName = "[Lagerbestand]";
							break;
						case "gefundenemengeinproduktion":
							sortFieldName = "[GefundeneMengeInProduktion]";
							break;
						case "rueckbuchungbestaetigt":
							sortFieldName = "[RueckbuchungBestaetigt]";
							break;

					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				int prodLagerId = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionLager((Lager)(this._data.LagerId ?? -1));
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(this._data.LagerId ?? -1);
				int totalCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportTwoTblAccess.CountReportTwoData(searchValue, LagerHelperHandler.GetWarehouseIds(this._data.LagerId ?? -1), prodWarehouseIds);
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.FullData ? totalCount : this._data.PageSize

				};

				ReportTwoDataList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportTwoTblAccess.GetReportTwoData(searchValue, LagerHelperHandler.GetWarehouseIds(this._data.LagerId ?? -1), prodWarehouseIds, dataSorting, dataPaging);
				var r = this._data.PageSize > 0 ? totalCount / decimal.Parse((this._data.PageSize).ToString()) : 0;
				var result = ReportTwoDataList.Select(x => new ReportTwo_Tabl_Model(x)).ToList();

				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model>.SuccessResponse(new Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model
				{
					Items = result,
					PageRequested = this._data.RequestedPage,
					PageSize = this._data.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] GetReportTwoXls()
		{
			try
			{
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(this._data.LagerId ?? -1);
				var	ReportXlsTwoDataList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportTwoTblAccess.GetReportXlsTwoDataWithoutPag(LagerHelperHandler.GetWarehouseIds( this._data.LagerId ?? -1), prodWarehouseIds);
				var result = ReportXlsTwoDataList.Select(x => new ReportTwo_Tabl_Model(x)).ToList();
				
	        

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Report2-{DateTime.Now:yyyyMMddTHHmmff}.xlsx");

				var file = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					var worksheet = package.Workbook.Worksheets.Add("Report 2");

					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 5;

					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now:yyyy MM dd}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Report 2: ROH ohne Bedarf [{DateTime.Now:dd/MM/yyyy}]";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					// Header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Menge In Produktion";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Lagerbestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gefundene Menge In Produktion";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Rueckbuchung Bestaetigt";

					int rowNumber = headerRowNumber + 1;

					if(result != null && result.Count > 0)
					{
						foreach(var w in result.OrderBy(x=> x.Artikelnummer))
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.MengeInProduktion;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Lagerbestand;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.GefundeneMengeInProduktion;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.RueckbuchungBestaetigt;

							// formater les colonnes décimales
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = "#,##0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = "#,##0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = "#,##0.00";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber++;
						}
					}

					// Style header
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));

					// Doc content border
					if(result != null && result.Count > 0)
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

					// Thick contour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// AutoFit
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Document properties
					package.Workbook.Properties.Title = "Report 2";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model>.AccessDeniedResponse();
			}

			return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportTwo_tbl_Reponse_Model>.SuccessResponse();
		}
	}
}
