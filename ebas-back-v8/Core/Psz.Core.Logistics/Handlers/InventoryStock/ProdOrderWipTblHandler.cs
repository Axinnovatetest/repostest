using System.Buffers;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities;
using iText.StyledXmlParser.Jsoup.Select;
using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ProdOrderWipTblHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model>>
	{
		private ProdWip_Tabl_RequestModel _data;
		private Core.Identity.Models.UserModel _user;
		public ProdOrderWipTblHandler(ProdWip_Tabl_RequestModel request, Core.Identity.Models.UserModel user)
		{
			_data = request;
			_user = user;
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var ProdWipList = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.ProductionWipEntity>();
			string searchValue = this._data.SearchValue;

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
			Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
			if(!string.IsNullOrWhiteSpace(this._data.SortField))
			{
				var sortFieldName = "";
				switch(this._data.SortField?.ToLower())
				{
					default:
					case "id":
						sortFieldName = "[Id]";
						break;
					case "fa":
						sortFieldName = "[FA]";
						break;
					case "item":
						sortFieldName = "[Item]";
						break;
					case "openqty":
						sortFieldName = "[OpenQty]";
						break;
					case "due":
						sortFieldName = "[Due]";
						break;
					case "vinspected":
						sortFieldName = "[ElectricalInspected]";
						break;
					case "oinspected":
						sortFieldName = "[OpticalInspected]";
						break;
				}
				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};
			}
			int totalCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.CountWipProdData(searchValue, _data.LagerId ?? -1);
			int requestRows;
			if(this._data.FullData)
			{
				requestRows = totalCount > 0 ? totalCount : 1;
			}
			else
			{
				requestRows = this._data.PageSize > 0 ? this._data.PageSize : 1;
			}

			dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
				RequestRows = requestRows
			};

			ProdWipList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetWipProdData(searchValue, _data.LagerId ?? -1, dataSorting, dataPaging);
			var result = ProdWipList.Select(x => new ProdWipResponseModel(x)).ToList();


			return ResponseModel<ProdWip_Tabl_Reponse_Model>.SuccessResponse(new ProdWip_Tabl_Reponse_Model
			{
				Items = result,
				PageRequested = this._data.RequestedPage,
				PageSize = this._data.PageSize,
				TotalCount = totalCount,
				TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(totalCount > 0 ? totalCount : 0) / this._data.PageSize)) : 0,
			});
		}
		public ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model>.AccessDeniedResponse();
			}

			return ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model>.SuccessResponse();
		}
		public byte[] GetReportFourXls()
		{
			try
			{
				// Retrieve data from the database
				int lagerId = this._data.LagerId ?? 0;
				var prodWipList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetXlsWipProdData(lagerId);


				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Report 4");

					int startColumn = 1;
					int rowNumber = 1;

					worksheet.TabColor = Color.LightGreen;
					worksheet.DefaultRowHeight = 15;

					// Pre-header at the top of the sheet
					var preHeader = worksheet.Cells[rowNumber, startColumn, rowNumber, 6];
					preHeader.Merge = true;
					preHeader.Value = $"WIP Results - Generated {DateTime.Now:dd/MM/yyyy}";
					preHeader.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					preHeader.Style.Font.Size = 16;
					preHeader.Style.Font.Bold = true;
					preHeader.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					preHeader.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(Module.XLS_COLOR_GREEN));
					rowNumber += 2;

					// Headers
					string[] headers = new string[] { "Fertigungsauftrag", "FA-Offene-Menge pro Bereich", "Artikel-Nr", "Work-Area", "Fertigungsgrad %", "Value (€)" };
					var workareaPercents = Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.GetWorkareas(lagerId);

					// Step dictionary with value and % (replace with your actual % fields if available)
					var stepFuncs = new Dictionary<string, Func<ProductionWipEntity, (decimal value, decimal percent)>>()
					{
						{ "Kommissionierung", x => (x.UserPicked, x.FaPicked) },
						{ "Schneiderei", x => (x.UserCut, x.FaCut) },
						{ "Vorbereitung", x => (x.UserPreped , x.FaPreped) },
						{ "Montage", x => (x.UserAssembled , x.FaAssembled) },
						{ "Krimp", x => (x.UserCrimped , x.FaCrimped) },
						{ "Elektrische Kontrolle", x => (x.UserOpticalInspected , x.FaOpticalInspected) },
						{ "Optische Kontrolle", x => (x.UserOpticalInspected , x.FaOpticalInspected) }
					};

					// Process each FA
					foreach(var fa in prodWipList.Select(x => x.FA).Distinct())
					{
						var faRows = prodWipList.Where(x => x.FA == fa)?.DistinctBy(x=> x.IdFa);

						// Header row
						for(int i = 0; i < headers.Length; i++)
						{
							var cell = worksheet.Cells[rowNumber, startColumn + i];
							cell.Value = headers[i];
							cell.Style.Font.Bold = true;
							cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							cell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BDD7EE"));
							cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
							cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						}
						rowNumber++;
						var immm = faRows.ToList();
						// Step rows
						foreach(var row in faRows)
						{
							foreach(var step in stepFuncs)
							{
								var (setpCompletionPercent, percent) = step.Value(row);
								var workareaPercent = workareaPercents.FirstOrDefault(x => x.Step == step.Key);
								//if(value > 0)
								{
									worksheet.Cells[rowNumber, startColumn + 0].Value = row.FA;
									worksheet.Cells[rowNumber, startColumn + 1].Value = row.OpenQty;
									worksheet.Cells[rowNumber, startColumn + 2].Value = row.Item;
									worksheet.Cells[rowNumber, startColumn + 3].Value = step.Key;
									worksheet.Cells[rowNumber, startColumn + 4].Value = percent;
									worksheet.Cells[rowNumber, startColumn + 5].Value = row.OpenQty * setpCompletionPercent;

									// Format row
									for(int i = startColumn; i <= startColumn + 5; i++)
									{
										var cell = worksheet.Cells[rowNumber, i];
										cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
										cell.Style.Fill.BackgroundColor.SetColor(Color.White);
										cell.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
										cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									}

									rowNumber++;
								}
							}
						}

						// Total row
						int faStartRow = rowNumber - faRows.Count() * stepFuncs.Count;
						worksheet.Cells[rowNumber, startColumn].Value = "WIP-Gesamtwert";
						worksheet.Cells[rowNumber, startColumn].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, startColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						worksheet.Cells[rowNumber, startColumn].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#808080"));
						worksheet.Cells[rowNumber, startColumn].Style.Font.Color.SetColor(Color.White);
						worksheet.Cells[rowNumber, startColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

						string GetColumnLetter(int col) => ExcelCellAddress.GetColumnLetter(col);
						worksheet.Cells[rowNumber, startColumn + 5].Formula = $"SUM({GetColumnLetter(startColumn + 5)}{faStartRow}:{GetColumnLetter(startColumn + 5)}{rowNumber - 1})";
						worksheet.Cells[rowNumber, startColumn + 5].Style.Numberformat.Format = "#,##0.00 €";
						worksheet.Cells[rowNumber, startColumn + 5].Style.Font.Bold = true;
						worksheet.Cells[rowNumber, startColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						worksheet.Cells[rowNumber, startColumn + 5].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#808080"));
						worksheet.Cells[rowNumber, startColumn + 5].Style.Font.Color.SetColor(Color.White);
						worksheet.Cells[rowNumber, startColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

						rowNumber += 3;
					}

					// Auto-fit all columns
					for(int i = startColumn; i <= startColumn + headers.Length - 1; i++)
						worksheet.Column(i).AutoFit();

					// Workbook properties
					package.Workbook.Properties.Title = "Report 4";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					package.Save();
					return package.GetAsByteArray();
				}
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex.StackTrace);
				throw;
			}
		}
	}
}
