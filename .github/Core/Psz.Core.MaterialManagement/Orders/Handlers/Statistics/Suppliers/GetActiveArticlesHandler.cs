using Microsoft.AspNetCore.Mvc.Formatters;
using OfficeOpenXml;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
	public class GetActiveArticlesHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Statistics.SupplierArticleResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.SupplierArticleRequestModel _data { get; set; }
		public GetActiveArticlesHandler(Identity.Models.UserModel user, Models.Statistics.SupplierArticleRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<Models.Statistics.SupplierArticleResponseModel> Handle()
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
						case "aktiv":
							sortFieldName = "a.aktiv";
							break;
						case "address_nr":
							sortFieldName = "d.Nr";
							break;
						case "artikel_nr":
							sortFieldName = "a.[Artikel-Nr]";
							break;
						case "artikelnummer":
							sortFieldName = "a.Artikelnummer";
							break;
						case "bestell_nr":
							sortFieldName = "b.[Bestell-Nr]";
							break;
						case "bezeichnung_1":
							sortFieldName = "a.[Bezeichnung 1]";
							break;
						case "bezeichnung_2":
							sortFieldName = "a.[Bezeichnung 2]";
							break;
						case "einkaufspreis":
							sortFieldName = "b.Einkaufspreis";
							break;
						case "lieferantennummer":
							sortFieldName = "d.Lieferantennummer";
							break;
						case "mindestbestellmenge":
							sortFieldName = "b.Mindestbestellmenge";
							break;
						case "name1":
							sortFieldName = "d.Name1";
							break;
						case "name2":
							sortFieldName = "d.Name2";
							break;
						case "standardlieferant":
							sortFieldName = "b.Standardlieferant";
							break;
						case "wiederbeschaffungszeitraum":
							sortFieldName = "b.Wiederbeschaffungszeitraum";
							break;
						default:
							sortFieldName = "[Artikelnummer]";
							break;
					}
				}

				#endregion

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierActiveArticles(this._data.AddressNr, this._data.IsStandard, this._data.IsActive, this._data.QueryTerm, sortFieldName, this._data.SortDesc, this._data.RequestedPage, this._data.PageSize);
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierActiveArticles_Count(this._data.AddressNr, this._data.IsStandard, this._data.IsActive, this._data.QueryTerm);

				//- 
				var responseBody = new Models.Statistics.SupplierArticleResponseModel();

				responseBody.TotalCount = allCount;
				responseBody.TotalPageCount = (int)Math.Ceiling((decimal)allCount / (this._data.PageSize <= 0 ? 1 : this._data.PageSize));
				responseBody.PageSize = this._data.PageSize;
				responseBody.PageRequested = this._data.RequestedPage;
				responseBody.Items = results?.Select(x => new Models.Statistics.SupplierArticleItem(x))?.ToList();


				// -
				return ResponseModel<Models.Statistics.SupplierArticleResponseModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Statistics.SupplierArticleResponseModel> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Statistics.SupplierArticleResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Statistics.SupplierArticleResponseModel>.SuccessResponse();
		}
		public byte[] GetExcelData()
		{
			try
			{
				// -
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierActiveArticles_Count(this._data.AddressNr, this._data.IsStandard, this._data.IsActive, this._data.QueryTerm);
				#region > Data sorting & paging
				string sortFieldName = "";
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = 0,
					RequestRows = allCount
				};
				#endregion

				var data = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierActiveArticles(this._data.AddressNr, this._data.IsStandard, this._data.IsActive, this._data.QueryTerm, sortFieldName, this._data.SortDesc, 0, allCount);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Articles");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					//worksheet.Row(1).Style.Font.Bold = true;
					//worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					//worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					using(var range = worksheet.Cells[headerRowNumber, startColumnNumber, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					}

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "aktiv";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lieferantennummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Name1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Name2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Wiederbeschaffungszeitraum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Standardlieferant";

					var rowNumber = headerRowNumber + 1;
					// Loop through 
					if(data != null && data.Count > 0)
					{
						foreach(var l in data)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.Bezeichnung_1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.Bezeichnung_2;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.aktiv;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.Lieferantennummer;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.Name2;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = "0.00";
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Mindestbestellmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Wiederbeschaffungszeitraum;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.Standardlieferant.HasValue && l.Standardlieferant.Value ? "YES" : "NO";

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
					package.Workbook.Properties.Title = $"Articles";
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
