using Microsoft.AspNetCore.Mvc.Formatters;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.MaterialManagement.Enums.StatisticsEnums;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System;
	using System.Drawing;
	using System.Linq;
	public class GetSummaryHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.SupplierHistoryOrderRequestModel _data { get; set; }
		public GetSummaryHandler(Identity.Models.UserModel user, Models.Statistics.SupplierHistoryOrderRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				string sortFieldName = "";
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					switch(this._data.SortField.ToLower())
					{
						case "anzahl":
							sortFieldName = "Anzahl";
							break;
						case "bestellung_nr":
							sortFieldName = "[Bestellung-Nr]";
							break;
						case "datum":
							sortFieldName = "Datum";
							break;
						case "einzelpreis":
							sortFieldName = "Einzelpreis";
							break;
						case "gesamtpreis":
							sortFieldName = "Gesamtpreis";
							break;
						case "lagerid":
							sortFieldName = "LagerId";
							break;
						case "lieferant":
							sortFieldName = "Lieferant";
							break;
						case "artikelnummer":
							sortFieldName = "Artikelnummer";
							break;
						case "position":
							sortFieldName = "Position";
							break;
						case "orderid":
							sortFieldName = "OrderId";
							break;
						case "articleid":
							sortFieldName = "ArticleId";
							break;
						case "supplierid":
							sortFieldName = "SupplierId";
							break;
						case "lager":
							sortFieldName = "Lager";
							break;
						default:
						case "date":
							sortFieldName = "[Datum]";
							break;
					}
				}

				#endregion

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierSummaryByYear(this._data.AddressNr.HasValue && this._data.AddressNr.Value > 0 ? this._data.AddressNr.Value : null, this._data.Year, this._data.LagerId, this._data.QueryTerm,
					(Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.SupplierOrderStatType)(this._data.OrderType ?? 1), sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierSummaryByYear_Count(this._data.AddressNr.HasValue && this._data.AddressNr.Value > 0 ? this._data.AddressNr.Value : null, this._data.Year, this._data.LagerId, this._data.QueryTerm,
					(Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.SupplierOrderStatType)(this._data.OrderType ?? 1));

				//- 
				var responseBody = new Models.Statistics.SupplierHistoryOrderResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new Models.Statistics.SupplierHistoryItem(x))?.ToList();


				// -
				return ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Statistics.SupplierHistoryOrderResponseModel>.SuccessResponse();
		}
		public byte[] GetExcelData()
		{
			try
			{
				// -
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierSummaryByYear_Count(this._data.AddressNr.HasValue && this._data.AddressNr.Value > 0 ? this._data.AddressNr.Value : null, this._data.Year, this._data.LagerId, this._data.QueryTerm,
					(Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.SupplierOrderStatType)(this._data.OrderType ?? 1));
				#region > Data sorting & paging
				string sortFieldName = "";
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = 0,
					RequestRows = allCount
				};
				#endregion

				var data = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierSummaryByYear(this._data.AddressNr.HasValue && this._data.AddressNr.Value > 0 ? this._data.AddressNr.Value : null, this._data.Year, this._data.LagerId, this._data.QueryTerm,
					(Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.SupplierOrderStatType)(this._data.OrderType ?? 1), sortFieldName, this._data.SortDesc, 0, allCount);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Orders");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 9;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					using(var range = worksheet.Cells[headerRowNumber, startColumnNumber, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Position";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestellung-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Einzelpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Gesamtpreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Lieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Lager";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(data != null && data.Count > 0)
					{
						foreach(var l in data)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.Position;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Bestellung_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.Anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Einzelpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Lieferant;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Datum?.ToString("dd/MM/yyyy");
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Lager;

							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = "0.00";

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}

						// Doc content
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
					package.Workbook.Properties.Title = $"Orders";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
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
	}
}
