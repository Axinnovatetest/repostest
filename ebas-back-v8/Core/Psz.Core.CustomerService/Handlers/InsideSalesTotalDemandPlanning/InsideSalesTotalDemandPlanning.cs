using Infrastructure.Data.Access.Joins.CTS;
using Microsoft.Extensions.Logging.Abstractions;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using static Psz.Core.CustomerService.Models.InsideSalesCustomerSummary.InsideSalesCustomerSummaryModel;

namespace Psz.Core.CustomerService.Handlers.InsideSalesTotalDemandPlanning
{
	public class InsideSalesTotalDemandPlanning: IInsideSalesTotalDemandPlanning
	{
		public ResponseModel<GetCustomerSummaryResponseModel> GetCustomerSummary(UserModel user, GetCustomerSummaryRequestModel data)
		{
			try
			{
				var allCount = 0;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;

				#region Validations 
				if(user == null || (!user.SuperAdministrator && user.Access.CustomerService.InsideSalesCustomerSummary != true))
				{
					return ResponseModel<GetCustomerSummaryResponseModel>.AccessDeniedResponse();
				}

				#endregion

				#region > Data sorting & paging

				if(!data.FullData)
				{
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
						RequestRows = data.PageSize
					};
				}

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "customer":
							sortFieldName = "[CustomerName]";
							break;
						case "documenttype":
							sortFieldName = "[DocumentType]";
							break;
						case "documentnumber":
							sortFieldName = "[DocumentNumber]";
							break;
						case "articlenumber":
							sortFieldName = "[ArticleNumber]";
							break;

						case "articledesignation":
							sortFieldName = "[ArticleDesignation]";
							break;
						case "openquantity":
							sortFieldName = "[OpenQuantity]";
							break;
						case "fanumber":
							sortFieldName = "[FANumber]";
							break;
						case "date":
							sortFieldName = "[Date]";
							break;
						case "week":
							sortFieldName = "[Week]";
							break;
						case "year":
							sortFieldName = "[Year]";
							break;
						case "unitprice":
							sortFieldName = "[UnitPrice]";
							break;
						case "totalprice":
							sortFieldName = "[TotalPrice]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var week = Helpers.DelforHelper.GetISOWeek(DateTime.Now);
				var startDate = Helpers.DelforHelper.FirstDateOfWeek(DateTime.Today.Year, week, new CultureInfo("de-DE"));
				var endDate = Helpers.DelforHelper.FirstDateOfWeek(data.Year, data.Week, new CultureInfo("de-DE")).AddDays(6);
				var customerPlanningFromDb = TotalDemandPlanningAccess.Get(data.InputFilter, startDate, endDate, data.DocumentType, dataPaging, dataSorting);

				allCount = customerPlanningFromDb.FirstOrDefault() == null ? 0 : customerPlanningFromDb.FirstOrDefault().TotalCount;

				return ResponseModel<GetCustomerSummaryResponseModel>.SuccessResponse(new GetCustomerSummaryResponseModel
				{

					Items = customerPlanningFromDb.Select(x => new CustomerSummaryResponseModel(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = allCount,
					TotalPageCount = customerPlanningFromDb != null && customerPlanningFromDb.Count > 0 ?
					data.PageSize > 0 ? (int)Math.Ceiling(((decimal)allCount / data.PageSize)) : 0 : 0,
				});

			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> GetCustomerSummary_XLS(UserModel user, GetCustomerSummaryRequestModel data)
		{
			try
			{
				data.FullData = true;
				var results = GetCustomerSummary(user, data);
				if(!results.Success)
				{
					return ResponseModel<byte[]>.FailureResponse(results.Errors?.Select(x => x.Value)?.ToList());
				}

				if(results.Body.Items?.Count <= 0)
				{
					return ResponseModel<byte[]>.FailureResponse("Empty data");
				}

				var _data = results.Body.Items;
				// - 
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"InsideSales Total demand planning");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 13;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Kundenname";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "DokummentTyp";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Dokumentnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Dokument Angebote NR";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Artikelbezeichnung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Offene Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "FA-Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Datum";

					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Woche";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Jahr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Einzelpreis";

					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Gesamtpreis";


					var rowNumber = headerRowNumber;
					//Loop through
					foreach(var l in _data.OrderBy(a => a.CustomerName))
					{
						rowNumber++;
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.CustomerName;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.DocumentType;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.DocumentNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.DocumentAngebotNr;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.ArticleNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.ArticleDesignation;

						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.OpenQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.FANumber;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.Date.HasValue == true ? l.Date.Value : "";

						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.Week;

						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.Year;
						worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.UnitPrice;

						worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.TotalPrice;


						worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;


						worksheet.Row(rowNumber).Height = 18;
					}
					using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					}

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						//worksheet.Column(i).AutoFit();
						worksheet.Column(i).Width = 15;
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Format headers
					worksheet.Row(1).Height = 15;
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Set some document properties
					package.Workbook.Properties.Title = $"INS-TotalDemanndPlanning -{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!


					return ResponseModel<byte[]>.SuccessResponse(package.GetAsByteArray());
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, int>>> GetNextNWeeks(UserModel user, int data)
		{
			if(data <= 0)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(null);
			}

			List<KeyValuePair<int, int>> weeks = new List<KeyValuePair<int, int>>();
			Calendar calendar = CultureInfo.CurrentCulture.Calendar;
			var current = DateTime.Now;
			var endDate = DateTime.Now.AddDays(7 * data);

			while(current <= endDate)
			{
				// Get ISO Week Number and Year
				int isoWeekNumber = System.Globalization.ISOWeek.GetWeekOfYear(current);
				int isoYear = System.Globalization.ISOWeek.GetYear(current);

				var weekYearPair = new KeyValuePair<int, int>(isoWeekNumber, isoYear);
				if(!weeks.Contains(weekYearPair))
				{
					weeks.Add(weekYearPair);
				}

				current = current.AddDays(1);
			}

			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(weeks);
		}
	}
}