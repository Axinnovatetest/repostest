using Infrastructure.Data.Access.Tables.CTS;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.InsideSalesChecksArchive;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.CustomerService.Handlers.InsideSalesHistory
{
	using Infrastructure.Data.Entities.Tables.CTS;
	using System.Drawing;
	public class InsideSalesChecksArchive: IInsideSalesChecksArchive
	{
		public ResponseModel<GetInsideSalesHistoryResponseModel> GetInsideSalesChecksHistories(UserModel user, GetInsideSalesHistoryRequestModel data)
		{

			try
			{

				#region Validations 
				if(user == null || (!user.SuperAdministrator && user.Access.CustomerService.InsideSalesChecksArchive != true))
				{
					return ResponseModel<GetInsideSalesHistoryResponseModel>.AccessDeniedResponse();
				}

				#endregion

				var allInsideSalesHistoryCount = 0;

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "customername":
							sortFieldName = "[CustomerName]";
							break;
						case "customerordername":
							sortFieldName = "[CustomerOrderName]";
							break;
						case "articlenumber":
							sortFieldName = "[ArticleNumber]";
							break;
						case "ordernumber":
							sortFieldName = "[OrderNumber]";
							break;
						case "stockcheck":
							sortFieldName = "[CheckStockDate]";
							break;
						case "fstcheck":
							sortFieldName = "[CheckFSTDate]";
							break;
						case "prscheck":
							sortFieldName = "[CheckPRSDate]";
							break;
						case "crpcheck":
							sortFieldName = "[CheckCRPDate]";
							break;
						case "inscheck":
							sortFieldName = "[CheckINSDate]";
							break;
						case "facheckw":
							sortFieldName = "[CheckFaDate]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var myCustomers = new OrderProcessing.GetMyCustomersHandler(null, user).Handle().Body ?? new List<Models.OrderProcessing.CustomerModel>();
				var insideSalesHistoryFromDB = InsideSalesChecksArchiveAccess.GetSalesHistoryBySearchValue(user.SuperAdministrator || user.IsGlobalDirector ? false : true, myCustomers.Select(x => x.CustomerNumber)?.ToList(),
					data.SearchValue, dataSorting, dataPaging);

				allInsideSalesHistoryCount = InsideSalesChecksArchiveAccess.Count_By_SearchValue(data.SearchValue);

				var insideSalesArchiveList = new List<InsideSalesArchiveResponseModel>();


				foreach(var orderDb in insideSalesHistoryFromDB)
				{
					var insideSaleArchive = new InsideSalesArchiveResponseModel();
					insideSaleArchive.Id = orderDb.Id;
					insideSaleArchive.CustomerOrderNumber = orderDb.CustomerOrderNumber;
					insideSaleArchive.OrderNumber = orderDb.OrderNumber;
					insideSaleArchive.ArticleNumber = orderDb.ArticleNumber;
					insideSaleArchive.CustomerName = orderDb.CustomerName;
					insideSaleArchive.CheckCRPComments = orderDb.CheckCRPComments;
					insideSaleArchive.CheckCRPDate = orderDb.CheckCRPDate;
					insideSaleArchive.CheckCRPUserName = orderDb.CheckCRPUserName;
					insideSaleArchive.CheckFST = orderDb.CheckFST;
					insideSaleArchive.CheckFSTComments = orderDb.CheckFSTComments;
					insideSaleArchive.CheckFSTDate = orderDb.CheckFSTDate;
					insideSaleArchive.CheckFSTUserName = orderDb.CheckFSTUserName;
					insideSaleArchive.CheckINS = orderDb.CheckINS;
					insideSaleArchive.CheckINSComments = orderDb.CheckINSComments;
					insideSaleArchive.CheckINSDate = orderDb.CheckINSDate;
					insideSaleArchive.CheckPRS = orderDb.CheckPRS;
					insideSaleArchive.CheckPRSComments = orderDb.CheckPRSComments;
					insideSaleArchive.CheckPRSDate = orderDb.CheckPRSDate;
					insideSaleArchive.CheckPRSUserName = orderDb.CheckPRSUserName;
					insideSaleArchive.CheckStock = orderDb.CheckStock;
					insideSaleArchive.CheckStockComments = orderDb.CheckStockComments;
					insideSaleArchive.CheckStockDate = orderDb.CheckCRPDate;
					insideSaleArchive.CheckStockUserName = orderDb.CheckCRPUserName;
					insideSaleArchive.ArticleId = orderDb.ArticleId;
					insideSaleArchive.CheckINSUserName = orderDb.CheckINSUserName;
					insideSaleArchive.CheckFa = orderDb.CheckFa;
					insideSaleArchive.CheckFaComments = orderDb.CheckFaComments;
					insideSaleArchive.CheckFaUserId = orderDb.CheckFaUserId;
					insideSaleArchive.CheckFaUserName = orderDb.CheckFaUserName;
					insideSaleArchive.CheckFaDate = orderDb.CheckFaDate;


					insideSalesArchiveList.Add(insideSaleArchive);
				}

				var insideSalesHistoryResponse = new GetInsideSalesHistoryResponseModel
				{

					Items = insideSalesArchiveList,
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = insideSalesArchiveList != null && insideSalesArchiveList.Count > 0 ? allInsideSalesHistoryCount : 0,
					TotalPageCount = insideSalesArchiveList != null && insideSalesArchiveList.Count > 0 ?
		data.PageSize > 0 ? (int)Math.Ceiling(((decimal)allInsideSalesHistoryCount / data.PageSize)) : 0 : 0,
				};

				return ResponseModel<GetInsideSalesHistoryResponseModel>.SuccessResponse(insideSalesHistoryResponse);

			} catch(System.Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> SendInstructionBack(UserModel user, int instructionId)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //


				#region Validations 
				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}

				if(instructionId <= 0)
				{
					return ResponseModel<int>.SuccessResponse(0);
				}
				var instructionById = InsideSalesChecksArchiveAccess.Get(instructionId);

				if(instructionById is null)
				{
					return ResponseModel<int>.FailureResponse("Instruction not found.");
				}

				var instructionFromInsideSales = InsideSalesChecksAccess.GetSalesCheckByOrderId(instructionById.OrderId);

				#endregion


				int result = 0;

				result = InsideSalesChecksArchiveAccess.updateRevertIns(instructionId, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
				var archiveHistoryEntity = InsideSalesChecksArchiveAccess.GetWithTransaction(instructionId, botransaction.connection, botransaction.transaction);
				InsideSalesChecksAccess.InsertWithTransaction(
					new InsideSalesChecksEntity
					{
						ArticleId = archiveHistoryEntity.ArticleId,
						ArticleNumber = archiveHistoryEntity.ArticleNumber,
						CheckCRP = false,
						CheckCRPComments = archiveHistoryEntity.CheckCRPComments,
						CheckCRPDate = null,
						CheckCRPUserId = null,
						CheckCRPUserName = null,
						CheckFST = false,
						CheckFSTComments = archiveHistoryEntity.CheckFSTComments,
						CheckFSTDate = null,
						CheckFSTUserId = null,
						CheckFSTUserName = null,
						CheckINS = false,
						CheckINSComments = archiveHistoryEntity.CheckINSComments,
						CheckINSDate = null,
						CheckINSUserId = null,
						CheckINSUserName = null,
						CheckPRS = false,
						CheckPRSComments = archiveHistoryEntity.CheckPRSComments,
						CheckPRSDate = null,
						CheckPRSUserId = null,
						CheckPRSUserName = null,
						CheckStock = false,
						CheckStockComments = archiveHistoryEntity.CheckStockComments,
						CheckStockDate = null,
						CheckStockUserId = null,
						CheckStockUserName = null,
						CheckFa = false,
						CheckFaComments = archiveHistoryEntity.CheckFaComments,
						CheckFaDate = null,
						CheckFaUserName = null,
						CheckFaUserId = null,
						CustomerName = archiveHistoryEntity.CustomerName,
						CustomerNumber = archiveHistoryEntity.CustomerNumber,
						CustomerOrderNumber = archiveHistoryEntity.CustomerOrderNumber,
						Id = archiveHistoryEntity.Id,
						OrderId = archiveHistoryEntity.OrderId,
						OrderNumber = archiveHistoryEntity.OrderNumber,
						OrderPositionId = archiveHistoryEntity.OrderPositionId,
						RevertArchiveDate = archiveHistoryEntity.RevertArchiveDate,
						RevertArchiveUserId = archiveHistoryEntity.RevertArchiveUserId,
						RevertArchiveUserName = archiveHistoryEntity.RevertArchiveUserName,
					}, botransaction.connection, botransaction.transaction);
				InsideSalesChecksArchiveAccess.DeleteWithTransaction(instructionId, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{

					return ResponseModel<int>.SuccessResponse();
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(System.Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> GetInsideSalesChecksHistories_XLS(UserModel user, GetInsideSalesHistoryRequestModel data)
		{
			try
			{
				data.FullData = true;
				var results = GetInsideSalesChecksHistories(user, data);
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
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"InsideSales Checks");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 24;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Bestellung";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "PSZ#";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "AB-Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Offene Menge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Liefertemin";

					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "FST";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "FST-Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "FST-Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "PRS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "PRS-Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "PRS-Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "CRP";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "CRP-Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "CRP-Bemerkung";

					worksheet.Cells[headerRowNumber, startColumnNumber + 18].Value = "INS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 19].Value = "INS-Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 20].Value = "INS-Bemerkung";


					worksheet.Cells[headerRowNumber, startColumnNumber + 21].Value = "FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 22].Value = "FA-Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 23].Value = "FA-Bemerkung";

					var rowNumber = headerRowNumber;
					//Loop through
					foreach(var l in _data.OrderBy(a => a.CustomerName))
					{
						rowNumber++;
						worksheet.Cells[rowNumber, startColumnNumber + 0].Value = l.CustomerName;
						worksheet.Cells[rowNumber, startColumnNumber + 1].Value = l.CustomerOrderNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 2].Value = l.ArticleNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 3].Value = l.OrderNumber;
						worksheet.Cells[rowNumber, startColumnNumber + 4].Value = l.OpenQuantity;
						worksheet.Cells[rowNumber, startColumnNumber + 5].Value = l.DeliveryDate;

						worksheet.Cells[rowNumber, startColumnNumber + 6].Value = l.CheckStock;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Value = l.CheckStockDate.HasValue == true ? l.CheckStockDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 8].Value = l.CheckStockComments;

						worksheet.Cells[rowNumber, startColumnNumber + 9].Value = l.CheckFST;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Value = l.CheckFSTDate.HasValue == true ? l.CheckFSTDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 11].Value = l.CheckFSTComments;

						worksheet.Cells[rowNumber, startColumnNumber + 12].Value = l.CheckPRS;
						worksheet.Cells[rowNumber, startColumnNumber + 13].Value = l.CheckPRSDate.HasValue == true ? l.CheckPRSDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 14].Value = l.CheckPRSComments;

						worksheet.Cells[rowNumber, startColumnNumber + 15].Value = l.CheckCRP;
						worksheet.Cells[rowNumber, startColumnNumber + 16].Value = l.CheckCRPDate.HasValue == true ? l.CheckCRPDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 17].Value = l.CheckCRPComments;

						worksheet.Cells[rowNumber, startColumnNumber + 18].Value = l.CheckINS;
						worksheet.Cells[rowNumber, startColumnNumber + 19].Value = l.CheckINSDate.HasValue == true ? l.CheckINSDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 20].Value = l.CheckINSComments;


						worksheet.Cells[rowNumber, startColumnNumber + 21].Value = l.CheckFa;
						worksheet.Cells[rowNumber, startColumnNumber + 22].Value = l.CheckFaDate.HasValue == true ? l.CheckFaDate.Value : "";
						worksheet.Cells[rowNumber, startColumnNumber + 23].Value = l.CheckFaComments;

						worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
						worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 10].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 19].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
						worksheet.Cells[rowNumber, startColumnNumber + 22].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

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
					package.Workbook.Properties.Title = $"InsideSales -{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
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
	}
}
