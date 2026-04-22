using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ReportOneTblHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model>>
	{
		private Psz.Core.Logistics.Models.InverntoryStockModels.ReportOneRequestTblModel _request;
		private Core.Identity.Models.UserModel _user;
		public ReportOneTblHandler(Psz.Core.Logistics.Models.InverntoryStockModels.ReportOneRequestTblModel request, Core.Identity.Models.UserModel user)
		{
			_request = request;
			_user = user;
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model> Handle()
		{

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var DataReportOneList = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.ReportOneTblEntity>();
				string searchValue = this._request.SearchValue;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "fertigungsnummer":
							sortFieldName = "[Fertigungsnummer]";
							break;
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "offenemenge":
							sortFieldName = "[OffeneMenge]";
							break;
						case "fatermin":
							sortFieldName = "[FaTermin]";
							break;
						case "fageschnitten":
							sortFieldName = "[FaGeschnitten]";
							break;
						case "fakommisioniert":
							sortFieldName = "[FaKommisioniert]";
							break;

					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				int totalCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportOneTblAccess.CountReportOneData(searchValue, LagerHelperHandler.GetWarehouseIds(this._request.LagerId ?? -1));
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize

				};

				DataReportOneList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportOneTblAccess.GetReportOneData(searchValue, LagerHelperHandler.GetWarehouseIds(this._request.LagerId ?? -1), dataSorting, dataPaging);
				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;
				var result = DataReportOneList.Select(x => new ReportOne_Tabl_Model(x)).ToList();

				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model>.SuccessResponse(new Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model
				{
					Items = result,
					PageRequested = this._request.RequestedPage,
					PageSize = this._request.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] GetReportOneXls()
		{
			try
			{
				// -
				var DataXlsReportOneList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportOneTblAccess.GetAll(LagerHelperHandler.GetWarehouseIds(this._request.LagerId ?? -1));
				var result = DataXlsReportOneList.Select(x => new ReportOne_Tabl_Model(x)).ToList();

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Report1-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Report 1");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 6;

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
					worksheet.Cells[1, 1].Value = $"Report 1 - Generated {DateTime.Now:dd/MM/yyyy}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Fertigungs nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikel nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Offene Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Fa Termin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Fa Geschnitten";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Fa Kommisioniert";

					int rowNumber = headerRowNumber + 1;

					if(DataXlsReportOneList != null && DataXlsReportOneList.Count > 0)
					{
						foreach(var w in DataXlsReportOneList?.OrderBy(x=> x.Artikelnummer))
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.OffeneMenge;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.FaTermin;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.FaGeschnitten;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.FaKommisioniert;

							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Row(rowNumber).Height = 18;
							rowNumber++;
						}
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));

					// Doc content
					if(DataXlsReportOneList != null && DataXlsReportOneList.Count > 0)
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
					package.Workbook.Properties.Title = "Report 1";
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
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model>.AccessDeniedResponse();
			}

			return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ReportOne_tbl_Reponse_Model>.SuccessResponse();
		}
	}
}
