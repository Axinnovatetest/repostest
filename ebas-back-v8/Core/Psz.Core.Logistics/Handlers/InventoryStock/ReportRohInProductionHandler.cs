using iText.Layout.Font;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Models.InverntoryStockModels;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ReportRohInProductionHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel>>
	{
		private Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionRequestModel _data;
		private Core.Identity.Models.UserModel _user;
		public ReportRohInProductionHandler(Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionRequestModel request, Core.Identity.Models.UserModel user)
		{
			_data = request;
			_user = user;
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

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
						case "idspule":
							sortFieldName = "[IdSpule]";
							break;
						case "statusspule":
							sortFieldName = "[StatusSpule]";
							break;
					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}

				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(this._data.LagerId ?? -1);
				int totalCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportROHinProductionAccess.CountData(searchValue,_data.LagerId ?? -1, prodWarehouseIds, _data.WarenType);
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.FullData ? totalCount : this._data.PageSize
				};

				var r = this._data.PageSize > 0 ? totalCount / decimal.Parse((this._data.PageSize).ToString()) : 0;
				var result = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportROHinProductionAccess.GetData(searchValue, _data.LagerId ?? -1, prodWarehouseIds, _data.WarenType, null, dataSorting, dataPaging)
					?.Select(x => new RohInProductionResponseModel(x))?.ToList();

				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel>.SuccessResponse(new Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel
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
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.RohInProductionPaginatedReponseModel>.SuccessResponse();
		}
		public byte[] GetReport3()
		{
			try
			{
				var lagerId = this._data.LagerId ?? -1;
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(lagerId);
				var lagerIds = LagerHelperHandler.GetWarehouseIds(lagerId);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Report 3");

					int headerRowNumber = 2;
					int startColumnNumber = 1;
					int numberOfColumns = 4;

					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now:yyyy MM dd}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Report 3: ROH Surplus (T1) [{DateTime.Now:dd/MM/yyyy}]";
					worksheet.Cells[1, 1].Style.Font.Size = 13;


					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "PL-Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Fa Bedarf";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Gefundene Menge In Produktion";

					int rowNumber = headerRowNumber + 1;

					// -
					var data = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetRohSurplusProduction( lagerIds, prodWarehouseIds);

					if(data != null && data.Count > 0)
					{
						foreach(var w in data.OrderBy(x=> x.Artikelnummer))
						{
							if(w is null)
							{
								continue;
							}
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w.MengeInProduktion;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w.BedarfFa;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w.GefundeneMengeInProduktion;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber++;
						}

						// Doc content
						if(data != null && data.Count > 0)
						{
							using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber - 1, numberOfColumns])
							{
								range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								range.Style.Fill.BackgroundColor.SetColor(Color.White);
								range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
								range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							}
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

					// -
					for(int i = 1; i <= numberOfColumns; i++)
						worksheet.Column(i).AutoFit();

					package.Workbook.Properties.Title = "Report 3";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public byte[] GetReportXls()
		{
			try
			{
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(this._data.LagerId ?? -1);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Report 3");

					int headerRowNumber = 2;
					int startColumnNumber = 1;
					int numberOfColumns = 4;

					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now:yyyy MM dd}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Report4: Rohmaterialliste Typ 2 in Produktion [{DateTime.Now:dd/MM/yyyy}]";
					worksheet.Cells[1, 1].Style.Font.Size = 13;


					// - first table
					var contactsData = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetRohInProduction(_data.LagerId ?? -1, true);
					int startHeaderRowNumber = 3;
					addTable("Teil I (Bericht Rohmaterial – Part 1: Kontakte-Auswertung)", "In diesem Abschnitt werden alle Spulen und Bestände vom Typ 2 für Kontakte angezeigt. Diese können im Produktionslager verbleiben, müssen jedoch inventarisiert werden.\r\nAlle inventarisierten Spulen werden mit „OK“ / grün markiert.", worksheet, startHeaderRowNumber, startColumnNumber, numberOfColumns, contactsData);

					// - second table
					startHeaderRowNumber = 3  + (contactsData?.Count ?? 0) + 6;
					headerRowNumber = startHeaderRowNumber + 2;
					var noneContactsData = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetRohInProduction( _data.LagerId ?? -1,  false);
					addTable("Teil II: Non Contacts", "Hier werden alle Typ-2-Bestände (keine Kontakte) dargestellt. Diese müssen zurückgebucht und ins Hauptlager überführt werden.", worksheet, startHeaderRowNumber, startColumnNumber, numberOfColumns, noneContactsData);

					// -
					for(int i = 2; i <= numberOfColumns; i++)
						worksheet.Column(i).AutoFit();

					package.Workbook.Properties.Title = "Report 3";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		private void addTable(string title, string subtitle, ExcelWorksheet worksheet, int startHeaderRowNumber, int startColumnNumber, int numberOfColumns, List<Infrastructure.Data.Entities.Joins.Logistics.RohInProdEntity> data)
		{
			worksheet.Cells[startHeaderRowNumber, 1, startHeaderRowNumber, numberOfColumns].Merge = true;
			worksheet.Cells[startHeaderRowNumber, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			worksheet.Cells[startHeaderRowNumber, 1].Value = $"{title}";
			worksheet.Cells[startHeaderRowNumber, 1].Style.Font.Size = 14;
			worksheet.Cells[startHeaderRowNumber, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			worksheet.Cells[startHeaderRowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));

			startHeaderRowNumber++;
			worksheet.Cells[startHeaderRowNumber, 1, startHeaderRowNumber, numberOfColumns].Merge = true;
			worksheet.Cells[startHeaderRowNumber, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
			worksheet.Cells[startHeaderRowNumber, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
			worksheet.Cells[startHeaderRowNumber, 1].Value = $"{subtitle}";
			worksheet.Cells[startHeaderRowNumber, 1].Style.Font.Size = 11;
			worksheet.Column(1).Width = 35;
			worksheet.Cells[startHeaderRowNumber, 1].Style.WrapText = true;
			worksheet.Cells[startHeaderRowNumber, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			worksheet.Cells[startHeaderRowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT));
			worksheet.Row(startHeaderRowNumber).Height = 60;
			startHeaderRowNumber++;
			var headerRowNumber = startHeaderRowNumber + 0;
			// 

			// Header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "ID-Spule";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Menge";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Status";

			int rowNumber = headerRowNumber + 1;

			// -
			if(data != null && data.Count > 0)
			{
				foreach(var w in data.OrderBy(x=> x.Artikelnummer))
				{
					if(w is null)
					{
						continue;
					}
					worksheet.Cells[rowNumber, startColumnNumber+0].Value = w?.Artikelnummer;
					worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w.SpuleId > 0 ? w.SpuleId.ToString() : "";
					worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Menge?.ToString("0.##");
					worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.SpuleStatus;
					// - 
					worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					if(w.SpuleStatus == "OK")
					{
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Fill.BackgroundColor.SetColor(Color.Green);
					}
					else
					{
						worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Fill.BackgroundColor.SetColor(Color.Orange);
					}

					// Format décimal
					for(int col = startColumnNumber + 1; col <= startColumnNumber + 3; col++)
						worksheet.Cells[rowNumber, col].Style.Numberformat.Format = "#,##0.00";

					worksheet.Row(rowNumber).Height = 18;
					rowNumber++;
				}

				// Doc content
				if(data != null && data.Count > 0)
				{
					using(var range = worksheet.Cells[FromRow: headerRowNumber, FromCol: 1, ToRow: rowNumber - 1, ToCol: numberOfColumns])
					{
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}
				}
			}

			//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
			using(var range = worksheet.Cells[startHeaderRowNumber, 1, headerRowNumber, numberOfColumns])
			{
				range.Style.Font.Bold = true;
				range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
				range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN_LIGHT));
				range.Style.Font.Color.SetColor(Color.Black);
				range.Style.ShrinkToFit = false;
			}
			// Darker  in Top cell
			//worksheet.Cells[startHeaderRowNumber, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));


			// Thick countour
			using(var range = worksheet.Cells[startHeaderRowNumber-2, 1, rowNumber - 1, numberOfColumns])
			{
				range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
			}
		}
	}
}
