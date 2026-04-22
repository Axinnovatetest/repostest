using Infrastructure.Data.Access.Tables.CTS;
using Infrastructure.Data.Entities.Tables.CTS;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Enums;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.InsideSalesChecks;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.CustomerService.Enums.StatEnum;
namespace Psz.Core.CustomerService.Handlers.InsideSalesChecks
{
	public partial class InsideSalesChecks: IInsideSalesChecks
	{
		public ResponseModel<SearchInsideSaleResponseModel> GetInsideSalesChecks(InsideSalesChecksSearchRequestModel data, UserModel user)
		{
			try
			{

				#region validations 

				if(user == null || (!user.SuperAdministrator && user.Access?.CustomerService.InsideSalesChecks != true))
				{
					return ResponseModel<SearchInsideSaleResponseModel>.AccessDeniedResponse();
				}

				#endregion

				var allCount = 0;

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
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var myCustomers = new OrderProcessing.GetMyCustomersHandler(null, user).Handle().Body ?? new List<Models.OrderProcessing.CustomerModel>();
				var salesCheckDB = InsideSalesChecksAccess.Get(user.SuperAdministrator || user.IsGlobalDirector ? false : true, myCustomers.Select(x => x.CustomerNumber)?.ToList(),
				dataSorting,
				dataPaging, data.SearchValue);

				allCount = InsideSalesChecksAccess.Count_By_CustomerName(data.SearchValue);

				var insideSalesList = new List<InsideSalesResponseModel>();


				foreach(var orderDb in salesCheckDB)
				{
					var searchCheckSale = new InsideSalesResponseModel();

					string Department = "";
					string Status = "";
					searchCheckSale.Id = orderDb.Id;
					searchCheckSale.CustomerName = orderDb.CustomerName;
					searchCheckSale.ArticleNumber = orderDb.ArticleNumber;
					searchCheckSale.OrderNumber = orderDb.OrderNumber;
					searchCheckSale.CheckINS = orderDb.CheckINS;
					searchCheckSale.CheckCRP = orderDb.CheckCRP;
					searchCheckSale.CheckStock = orderDb.CheckStock;
					searchCheckSale.CheckFST = orderDb.CheckFST;
					searchCheckSale.CheckPRS = orderDb.CheckPRS;
					searchCheckSale.CustomerOrderNumber = orderDb.CustomerOrderNumber;
					searchCheckSale.OrderId = orderDb.OrderId;
					searchCheckSale.ArticleId = orderDb.ArticleId;
					searchCheckSale.CheckStockComments = orderDb.CheckStockComments;
					searchCheckSale.CheckFSTComments = orderDb.CheckFSTKapaReason;
					searchCheckSale.CheckPRSComments = orderDb.CheckPRSMaterialMissing;
					searchCheckSale.CheckCRPComments = orderDb.CheckCRPComments;
					searchCheckSale.CheckINSComments = orderDb.CheckINSComments;
					searchCheckSale.CheckStockDate = orderDb.CheckStockDate;
					searchCheckSale.CheckFSTDate = orderDb.CheckFSTDate;
					searchCheckSale.CheckPRSDate = orderDb.CheckPRSDate;
					searchCheckSale.CheckCRPDate = orderDb.CheckCRPDate;
					searchCheckSale.CheckINSDate = orderDb.CheckINSDate;
					searchCheckSale.CheckStockUserName = orderDb.CheckStockUserName;
					searchCheckSale.CheckFaUserName = orderDb.CheckFaUserName;
					searchCheckSale.CheckFaDate = orderDb.CheckFaDate;
					searchCheckSale.CheckFSTUserName = orderDb.CheckFSTUserName;
					searchCheckSale.CheckPRSUserName = orderDb.CheckPRSUserName;
					searchCheckSale.CheckCRPUserName = orderDb.CheckCRPUserName;
					searchCheckSale.CheckINSUserName = orderDb.CheckINSUserName;
					searchCheckSale.OpenQuantity = orderDb.OrderOpenQuantity ?? 0;
					searchCheckSale.DeliveryDate = orderDb.OrderDeliveryDate;
					searchCheckSale.CheckFa = orderDb.CheckFa;
					searchCheckSale.CheckFaAvaialable = orderDb.CheckFaAvaialable;
					searchCheckSale.CheckFaDateOk = orderDb.CheckFaDateOk;
					searchCheckSale.CheckFaComments = orderDb.CheckFaComments;
					searchCheckSale.CheckFaWeek = orderDb.CheckFaWeek;
					CheckStatusDepartment(orderDb, ref Department, ref Status);
					searchCheckSale.Department = Department;
					searchCheckSale.Status = Status;
					searchCheckSale.IsCheckedStock = orderDb.IsCheckedStock;
					searchCheckSale.CheckCRPDateAdjusted = orderDb.CheckCRPDateAdjusted;
					searchCheckSale.CheckCRPWeek = orderDb.CheckCRPWeek;
					searchCheckSale.CheckCRPWTCompliedOk = orderDb.CheckCRPWTCompliedOk;
					searchCheckSale.CheckFSTKapaOk = orderDb.CheckFSTKapaOk;
					searchCheckSale.CheckFSTKapaReason = orderDb.CheckFSTKapaReason;
					searchCheckSale.CheckPRSMaterialMissing = orderDb.CheckPRSMaterialMissing;
					searchCheckSale.CheckPRSMaterialOk = orderDb.CheckPRSMaterialOk;
					searchCheckSale.CheckINSAbConfirmed = orderDb.CheckINSAbConfirmed;
					searchCheckSale.CheckFSTKapaWeek = orderDb.CheckFSTKapaWeek ?? 0;
					searchCheckSale.CheckPRSMaterialLastDeliveryDate = orderDb.CheckPRSMaterialLastDeliveryDate;

					insideSalesList.Add(searchCheckSale);
				}

				SearchInsideSaleResponseModel searchInsideSaleResponseModel = new()
				{
					Items = insideSalesList,
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = insideSalesList != null && insideSalesList.Count > 0 ? allCount : 0,
					TotalPageCount = insideSalesList != null && insideSalesList.Count > 0 ?
					data.PageSize > 0 ? (int)Math.Ceiling(((decimal)allCount / data.PageSize)) : 0 : 0,
				};

				return ResponseModel<SearchInsideSaleResponseModel>.SuccessResponse(searchInsideSaleResponseModel);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> UpdateInstructions(UserModel user, InsideSalesChecksUpdateRequestModel data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				//-

				#region Validations 
				if(user == null)
				{
					return ResponseModel<int>.AccessDeniedResponse();
				}
				if(data.Id <= 0)
				{
					return ResponseModel<int>.SuccessResponse(0);
				}
				var saleById = InsideSalesChecksAccess.Get(data.Id);

				if(saleById is null)
				{
					return ResponseModel<int>.FailureResponse("Item not found.");
				}


				switch(data.CheckedType)
				{
					case Enums.InsideSalesEnums.CheckTypes.stock:
						{
							if(saleById.CheckFST == true || saleById.CheckPRS == true || saleById.CheckCRP == true || saleById.CheckINS == true || saleById.CheckFa == true)
							{
								return ResponseModel<int>.FailureResponse("FA should be unchecked first.");
							}
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.FA:
						{
							int faWeek = (int)data.CheckFaWeek;
							bool faAvailable = (bool)data.CheckFaAvaialable;
							bool faDateOk = (bool)data.CheckFaDateOk;
							int currentWeek = DelforHelper.GetISOWeek(DateTime.Now);
							Enums.InsideSalesEnums.CheckTypes type = data.CheckedType;
							if(saleById.CheckStock != true)
							{
								return ResponseModel<int>.FailureResponse("Stock should be checked first.");
							}
							if(saleById.CheckFST == true || saleById.CheckPRS == true || saleById.CheckCRP == true || saleById.CheckINS == true)
							{
								return ResponseModel<int>.FailureResponse("FST should be unchecked first.");
							}
							if(faWeek < 0)
							{
								return ResponseModel<int>.FailureResponse(type + " week value should be positive");
							}
							if(faWeek == 0)
							{
								return ResponseModel<int>.FailureResponse(type + " week cannot be 0.");
							}

							if(faWeek > 52)
							{
								return ResponseModel<int>.FailureResponse(type + " week value should be before week 52");
							}

							if(faWeek < currentWeek)
							{
								return ResponseModel<int>.FailureResponse(type + " week value should be the same or after current week.");
							}

							if(faAvailable && !faDateOk && String.IsNullOrEmpty(data.CheckFaComments))
							{
								return ResponseModel<int>.FailureResponse(type + " remark cannot be empty.");
							}
							if(faAvailable && !faDateOk && faWeek == 0)
							{
								return ResponseModel<int>.FailureResponse(type + " week cannot be 0.");
							}
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.FST:
						{
							int fstKapaWeek = (int)data.CheckFSTKapaWeek;
							bool fastKapaOk = (bool)data.CheckFSTKapaOk.Value;
							int currentWeek = DelforHelper.GetISOWeek(DateTime.Now);
							Enums.InsideSalesEnums.CheckTypes type = data.CheckedType;
							if(saleById.CheckPRS == true || saleById.CheckCRP == true || saleById.CheckINS == true)
							{
								return ResponseModel<int>.FailureResponse("PRS should be unchecked first.");
							}
							if(saleById.CheckStock != true || saleById.CheckFa != true)
							{
								return ResponseModel<int>.FailureResponse("FA should be checked first.");
							}
							if(fstKapaWeek <= 0)
								return ResponseModel<int>.FailureResponse(type + " week value should be positive");

							if(fstKapaWeek > 52)
								return ResponseModel<int>.FailureResponse(type + " week value should be before week 52");

							if(fstKapaWeek < currentWeek)
								return ResponseModel<int>.FailureResponse(type + " week value should be the same or after current week.");

							if(!fastKapaOk && String.IsNullOrEmpty(data.CheckFSTKapaReason))
							{
								return ResponseModel<int>.FailureResponse("FST capacity reason cannot be empty.");
							}
							if(!fastKapaOk && fstKapaWeek == 0)
							{
								return ResponseModel<int>.FailureResponse("FST Kapa week cannot be empty.");
							}
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.PRS:
						{
							bool prsMaterialOk = (bool)data.CheckPRSMaterialOk;
							DateTime? checkPRSMaterialLastDeliveryDate = data.CheckPRSMaterialLastDeliveryDate;
							DateTime nowUtc = DateTime.Now.ToUniversalTime();
							Enums.InsideSalesEnums.CheckTypes type = data.CheckedType;

							if(saleById.CheckStock != true || saleById.CheckFST != true || saleById.CheckFa != true)
							{
								return ResponseModel<int>.FailureResponse("FST should be checked first.");
							}
							if(saleById.CheckCRP == true || saleById.CheckINS == true)
							{
								return ResponseModel<int>.FailureResponse("CRP should be unchecked first");
							}
							if(!((bool)data.CheckPRSMaterialOk) && String.IsNullOrEmpty(data.CheckPRSMaterialMissing))
							{
								return ResponseModel<int>.FailureResponse("PRS missing material reason cannot be empty.");
							}

							if(!prsMaterialOk && checkPRSMaterialLastDeliveryDate <= nowUtc)
							{
								return ResponseModel<int>.FailureResponse("PRS material last delivery date cannot before current date !");
							}
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.CRP:
						{
							int crpWeek = (int)data.CheckCRPWeek;
							bool crpCompliedWithOk = (bool)data.CheckCRPWTCompliedOk;
							int currentWeek = DelforHelper.GetISOWeek(DateTime.Now);
							Enums.InsideSalesEnums.CheckTypes type = data.CheckedType;
							if(saleById.CheckStock != true || saleById.CheckFST != true || saleById.CheckPRS != true)
							{
								return ResponseModel<int>.FailureResponse("PRS should be checked first.");
							}
							if(saleById.CheckINS == true)
							{
								return ResponseModel<int>.FailureResponse("INS should be unchecked first");
							}
							if(crpWeek <= 0)
								return ResponseModel<int>.FailureResponse(type + " week value should be positive");

							if(crpWeek > 52)
								return ResponseModel<int>.FailureResponse(type + " week value should be before week 52");

							if(crpWeek < currentWeek)
								return ResponseModel<int>.FailureResponse(type + " week value should be the same or after current week.");

							if(!crpCompliedWithOk && crpWeek == 0)
							{
								return ResponseModel<int>.FailureResponse("CRP new week cannot be be 0.");
							}
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.INS:
						{
							if(saleById.CheckStock != true || saleById.CheckFST != true || saleById.CheckPRS != true || saleById.CheckCRP != true)
							{
								return ResponseModel<int>.FailureResponse("CRP should be checked first.");
							}
							break;
						}
					default:
						break;
				}
				#endregion


				int result = 0;
				int checkedValue = 0;
				if(data.CheckedTypeValue == false)
				{
					checkedValue = 0;
				}
				else
				{
					checkedValue = 1;
				}
				switch(data.CheckedType)
				{
					case Enums.InsideSalesEnums.CheckTypes.stock:
						{
							if((bool)data.IsCheckedStock)
							{
								InsideSalesChecksAccess.updateStock(true, data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
								InsideSalesChecksAccess.updateFa(data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
								InsideSalesChecksAccess.updateFST(data.Id, checkedValue, 0, null, 0, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
								InsideSalesChecksAccess.updatePRS(data.Id, checkedValue, null, null, null, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
								InsideSalesChecksAccess.updateCRP(data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction, 0, 0, 0);
							}
							else
							{
								InsideSalesChecksAccess.updateStock(false, data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
							}

							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.FA:

						int faAvailableCheckedValue = data.CheckFaAvaialable == true ? 1 : 0;
						int faOkCheckedValue = data.CheckFaDateOk == true ? 1 : 0;

						result = InsideSalesChecksAccess.updateFACheck(data.Id, faAvailableCheckedValue, faOkCheckedValue, data.CheckFaComments, data.CheckFaWeek, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);

						break;
					case Enums.InsideSalesEnums.CheckTypes.FST:
						{
							int fstKapaOkCheckedValue = data.CheckFSTKapaOk.Value == true ? 1 : 0;

							result = InsideSalesChecksAccess.updateFST(data.Id, checkedValue, fstKapaOkCheckedValue, data.CheckFSTKapaReason, data.CheckFSTKapaWeek, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.PRS:
						{
							int prsMaterialOkCheckedValue = data.CheckPRSMaterialOk == true ? 1 : 0;

							result = InsideSalesChecksAccess.updatePRS(data.Id, checkedValue, prsMaterialOkCheckedValue, data.CheckPRSMaterialMissing, data.CheckPRSMaterialLastDeliveryDate, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction);
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.CRP:
						{
							int crpDateAdjusted = data.CheckCRPDateAdjusted == true ? 1 : 0;
							int crpCompliedWith = data.CheckCRPWTCompliedOk == true ? 1 : 0;

							result = InsideSalesChecksAccess.updateCRP(data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction, crpDateAdjusted, crpCompliedWith, (int)data.CheckCRPWeek);
							break;
						}
					case Enums.InsideSalesEnums.CheckTypes.INS:
						{
							int insAbConfirmed = data.CheckINSAbConfirmed == true ? 1 : 0;
							result = InsideSalesChecksAccess.updateINS(data.Id, checkedValue, user.Id, user.Username, DateTime.Now, botransaction.connection, botransaction.transaction, insAbConfirmed);
							var checkEntity = InsideSalesChecksAccess.GetWithTransaction(data.Id, botransaction.connection, botransaction.transaction);
							InsideSalesChecksArchiveAccess.InsertWithTransaction(
								new InsideSalesChecksArchiveEntity
								{
									ArticleId = checkEntity.ArticleId,
									ArticleNumber = checkEntity.ArticleNumber,
									CheckCRP = checkEntity.CheckCRP,
									CheckCRPComments = checkEntity.CheckCRPComments,
									CheckCRPDate = checkEntity.CheckCRPDate,
									CheckCRPUserId = checkEntity.CheckCRPUserId,
									CheckCRPUserName = checkEntity.CheckCRPUserName,
									CheckFST = checkEntity.CheckFST,
									CheckFSTComments = checkEntity.CheckFSTKapaReason,
									CheckFSTDate = checkEntity.CheckFSTDate,
									CheckFSTUserId = checkEntity.CheckFSTUserId,
									CheckFSTUserName = checkEntity.CheckFSTUserName,
									CheckINS = checkEntity.CheckINS,
									CheckINSComments = checkEntity.CheckINSComments,
									CheckINSDate = checkEntity.CheckINSDate,
									CheckINSUserId = checkEntity.CheckINSUserId,
									CheckINSUserName = checkEntity.CheckINSUserName,
									CheckPRS = checkEntity.CheckPRS,
									CheckPRSComments = checkEntity.CheckPRSMaterialMissing,
									CheckPRSDate = checkEntity.CheckPRSDate,
									CheckPRSUserId = checkEntity.CheckPRSUserId,
									CheckPRSUserName = checkEntity.CheckPRSUserName,
									CheckStock = checkEntity.CheckStock,
									CheckStockComments = checkEntity.CheckStockComments,
									CheckStockDate = checkEntity.CheckStockDate,
									CheckStockUserId = checkEntity.CheckStockUserId,
									CheckStockUserName = checkEntity.CheckStockUserName,
									CustomerName = checkEntity.CustomerName,
									CustomerNumber = checkEntity.CustomerNumber,
									CustomerOrderNumber = checkEntity.CustomerOrderNumber,
									Id = checkEntity.Id,
									OrderId = checkEntity.OrderId,
									OrderNumber = checkEntity.OrderNumber,
									OrderPositionId = checkEntity.OrderPositionId,
									IsCheckedStock = checkEntity.IsCheckedStock,
									CheckFa = checkEntity.CheckFa,
									CheckFaUserId = checkEntity.CheckFaUserId,
									CheckFaUserName = checkEntity.CheckFaUserName,
									CheckFaComments = checkEntity.CheckFaComments,
									CheckFaDate = checkEntity.CheckFaDate,
									CheckFaWeek = checkEntity.CheckFaWeek,
									CheckFaAvaialable = checkEntity.CheckFaAvaialable,
									CheckFaDateOk = checkEntity.CheckFaDateOk,
									CheckFSTKapaOk = checkEntity.CheckFSTKapaOk,
									CheckFSTKapaReason = checkEntity.CheckFSTKapaReason,
									CheckFSTKapaWeek = checkEntity.CheckFSTKapaWeek,
									CheckPRSMaterialOk = checkEntity.CheckPRSMaterialOk,
									CheckPRSMaterialMissing = checkEntity.CheckPRSMaterialMissing,
									CheckPRSMaterialLastDeliveryDate = checkEntity.CheckPRSMaterialLastDeliveryDate,
									CheckCRPDateAdjusted = checkEntity.CheckCRPDateAdjusted,
									CheckCRPWTCompliedOk = checkEntity.CheckCRPWTCompliedOk,
									CheckCRPWeek = checkEntity.CheckCRPWeek,
									CheckINSAbConfirmed = checkEntity.CheckINSAbConfirmed,
									RevertArchiveDate = null,
									RevertArchiveUserId = null,
									RevertArchiveUserName = null,
								}, botransaction.connection, botransaction.transaction);
							;
							InsideSalesChecksAccess.DeleteWithTransaction(data.Id, botransaction.connection, botransaction.transaction);
							break;
						}
					default:
						break;
				}
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{

					return ResponseModel<int>.SuccessResponse(result);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}


		}
		private bool VerifyData(bool? value)
		{
			if(value is null)
				return false;
			return value.Value;
		}
		private void CheckStatusDepartment(InsideSalesChecksEntity orderDb, ref string Department, ref string Status)
		{
			// INS Change: 1 & (2 or 3)
			// - 1 - Stock NOT enough
			// - 2 - FA not Available
			// - 2.1 - Kapa (Ok or nOk)
			// - 2.2 - Mat (Ok or nOk)
			// - 2.3 - WTComplied nOk
			// - OR
			// - 3 - FA available
			// - 3.1 - FaDate nOk
			// - 3.2 - Kapa (Ok or nOk)
			// - 3.3 - Mat (Ok or nOk)
			// - 3.4 - WTComplied nOk
			if(
				// - 1
				orderDb.IsCheckedStock == false 
				&&
				(// - 2
				(orderDb.CheckFaAvaialable == false
				&& /*2.1*/ orderDb.CheckFSTKapaOk != null
				&& /*2.2*/ orderDb.CheckPRSMaterialOk != null
				&& /*2.3*/ orderDb.CheckCRPWTCompliedOk == false)
				||// - 3
				(orderDb.CheckFaAvaialable == true
				&& /*3.1*/ orderDb.CheckFaDateOk == false
				&& /*3.2*/ orderDb.CheckFSTKapaOk != null
				&& /*3.3*/ orderDb.CheckPRSMaterialOk != null
				&& /*3.4*/ orderDb.CheckCRPWTCompliedOk == false)
				))
			{
				Department = InsideSalesEnums.CheckTypes.INS.GetDescription();
				Status = InsideSalesStatusEnums.Change.GetDescription();
				return;
			}

			// INS Bestatigung: 0 or (1 & (2 or 3 or 4))
			// - 0 - Stock enough
			// - 1 - Stock NOT enough
			// - 2 - FA not Available
			// - 2.1 - Kapa (Ok or nOk)
			// - 2.2 - Mat (Ok or nOk)
			// - 2.3 - WTComplied Ok
			// - OR
			// - 3 - FA available
			// - 3.1 - FaDate nOk
			// - 3.2 - Kapa (Ok or nOk)
			// - 3.3 - Mat (Ok or nOk)
			// - 3.4 - WTComplied Ok
			// - OR
			// - 4 - FA available
			// - 4.1 - FaDate Ok
			if(
				// - 0 
				orderDb.IsCheckedStock == true
				||
				// - 1
				(orderDb.IsCheckedStock == false 
				&&
				(// - 2
				(orderDb.CheckFaAvaialable == false
				&& /*2.1*/ orderDb.CheckFSTKapaOk != null
				&& /*2.2*/ orderDb.CheckPRSMaterialOk != null
				&& /*2.3*/ orderDb.CheckCRPWTCompliedOk == true)
				||// - 3
				(orderDb.CheckFaAvaialable == true
				&& /*3.1*/ orderDb.CheckFaDateOk == false
				&& /*3.2*/ orderDb.CheckFSTKapaOk != null
				&& /*3.3*/ orderDb.CheckPRSMaterialOk != null
				&& /*3.4*/ orderDb.CheckCRPWTCompliedOk == true)
				||// - 4
				(orderDb.CheckFaAvaialable == true
				&& /*4.1*/ orderDb.CheckFaDateOk == true)
				)))
			{
				Department = InsideSalesEnums.CheckTypes.INS.GetDescription();
				Status = InsideSalesStatusEnums.Bestatigung.GetDescription();
				return;
			}

			// CRP in progress: 0 or 1 or 2
			// - 0 - Stock not checked yet
			// - 1 - Stock NOT enough, FA not checked yet
			// - 2 - Stock not enough
			// - 2.1 - FA NOT available OR FA available & FaDate nOk 
			// - 2.2 - Kapa Ok
			// - 2.3 - Mat Ok
			// - 2.4 - WT not checked yet
			if(
				// - 0
				orderDb.IsCheckedStock == null 
				||
				// - 1
				(orderDb.IsCheckedStock == false && orderDb.CheckFaAvaialable == null)
				||
				// - 2
				(orderDb.IsCheckedStock == false
				&& /*2.1*/ (orderDb.CheckFaAvaialable == false || (orderDb.CheckFaAvaialable == true && orderDb.CheckFaDateOk == false))
				&& /*2.2*/ orderDb.CheckFSTKapaOk == true
				&& /*2.3*/ orderDb.CheckPRSMaterialOk == true
				&& /*2.4*/ orderDb.CheckCRPWTCompliedOk == null)
			)
			{
				Department = InsideSalesEnums.CheckTypes.CRP.GetDescription();
				Status = InsideSalesStatusEnums.InProgress.GetDescription();
				return;
			}

			// CRP change: 0 or 1 or 2
			// - 0 - Stock not checked yet
			// - 1 - Stock NOT enough, FA not checked yet
			// - 2 - Stock not enough
			// - 2.1 - FA NOT available  OR FA available & FaDate nOk 
			// - 2.2 - Kapa (Ok or nOk)
			// - 2.3 - Mat nOk
			// - 2.4 - WT not checked yet
			if(
				// - 0
				orderDb.IsCheckedStock == null
				||
				// - 1
				(orderDb.IsCheckedStock == false && orderDb.CheckFaAvaialable == null)
				||
				// - 2
				(orderDb.IsCheckedStock == false
				&& /*2.1*/ (orderDb.CheckFaAvaialable == false || (orderDb.CheckFaAvaialable == true && orderDb.CheckFaDateOk == false))
				&& /*2.2*/ orderDb.CheckFSTKapaOk != null
				&& /*2.3*/ orderDb.CheckPRSMaterialOk == false
				&& /*2.4*/ orderDb.CheckCRPWTCompliedOk == null)
			)
			{
				Department = InsideSalesEnums.CheckTypes.CRP.GetDescription();
				Status = InsideSalesStatusEnums.Change.GetDescription();
				return;
			}


			// PRS in progress: 1 & 2 & 3 & 4
			// - 1 - Stock not enough
			// - 2 - FA NOT available  OR FA available & FaDate nOk 
			// - 3 - Kapa Ok
			// - 4 - Mat not checked yet
			if(
				/*1*/ orderDb.IsCheckedStock == false
				&& /*2*/ (orderDb.CheckFaAvaialable == false || (orderDb.CheckFaAvaialable == true && orderDb.CheckFaDateOk == false))
				&& /*3*/ orderDb.CheckFSTKapaOk == true
				&& /*4*/ orderDb.CheckPRSMaterialOk == null
			)
			{
				Department = InsideSalesEnums.CheckTypes.PRS.GetDescription();
				Status = InsideSalesStatusEnums.InProgress.GetDescription();
				return;
			}

			// PRS Change: 1 & 2 & 3 & 4
			// - 1 - Stock not enough
			// - 2 - FA NOT available  OR FA available & FaDate nOk 
			// - 3 - Kapa nOk
			// - 4 - Mat not checked yet
			if(
				/*1*/ orderDb.IsCheckedStock == false
				&& /*2*/ (orderDb.CheckFaAvaialable == false || (orderDb.CheckFaAvaialable == true && orderDb.CheckFaDateOk == false))
				&& /*3*/ orderDb.CheckFSTKapaOk == false
				&& /*4*/ orderDb.CheckPRSMaterialOk == null
			)
			{
				Department = InsideSalesEnums.CheckTypes.PRS.GetDescription();
				Status = InsideSalesStatusEnums.Change.GetDescription();
				return;
			}

			// FST in progress: 1 & 2 & 3
			// - 1 - Stock not enough
			// - 2 - FA NOT available  OR FA available & FaDate nOk 
			// - 3 - Kapa not checked yet
			if(
				/*1*/ orderDb.IsCheckedStock == false
				&& /*2*/ (orderDb.CheckFaAvaialable == false || (orderDb.CheckFaAvaialable == true && orderDb.CheckFaDateOk == false))
				&& /*3*/ orderDb.CheckFSTKapaOk == null
			)
			{
				Department = InsideSalesEnums.CheckTypes.FST.GetDescription();
				Status = InsideSalesStatusEnums.InProgress.GetDescription();
				return;
			}

			// - 
			Department = InsideSalesEnums.CheckTypes.CRP.GetDescription();
			Status = InsideSalesStatusEnums.InProgress.GetDescription();
		}
		public ResponseModel<InsideSalesChecksUpdateLogResponseModel> GetLogs(UserModel user, InsideSalesChecksUpdateLogRequestModel data)
		{
			if(!user.IsGlobalDirector && !user.SuperAdministrator)
				return ResponseModel<InsideSalesChecksUpdateLogResponseModel>.AccessDeniedResponse();

			try
			{
				var allCount = 0;
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
						case "newrecordcount":
							sortFieldName = "[NewRecordCount]";
							break;
						case "recordtime":
							sortFieldName = "[recordtime]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion

				var logs = InsideSalesChecksLogsAccess.Get(data.SearchTerms, dataSorting, dataPaging);
				var count = InsideSalesChecksLogsAccess.Get_count(data.SearchTerms);

				return ResponseModel<InsideSalesChecksUpdateLogResponseModel>.SuccessResponse(new InsideSalesChecksUpdateLogResponseModel
				{
					Items = logs.Select(x => new InsideSalesChecksUpdateLogResponseModel.UpdateLogItem(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = logs != null && logs.Count > 0 ? allCount : 0,
					TotalPageCount = logs != null && logs.Count > 0 ?
					data.PageSize > 0 ? (int)Math.Ceiling(((decimal)allCount / data.PageSize)) : 0 : 0
				}
					);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
