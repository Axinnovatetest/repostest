using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.FinanceControl.Models.Budget.Order;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOrdersHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderStatisticsResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private OrderStatisticsRequestModel _data { get; set; }

		public GetOrdersHandler(Identity.Models.UserModel user, OrderStatisticsRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<OrderStatisticsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				int totalCount = 0;

				#region > Data sorting & paging
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(_data.SortField))
				{
					var sortFieldName = "";
					switch(_data.SortField.ToLower())
					{
						default:
						case "order_number":
							sortFieldName = "[OrderNumber]";
							break;
						case "type_order":
							sortFieldName = "[PoPaymentType]";
							break;
						case "name_project":
							sortFieldName = "[ProjectName]";
							break;
						case "id_supplier":
							sortFieldName = "[SupplierName]";
							break;
						case "responsable_id":
							sortFieldName = "[IssuerName]";
							break;
						case "totalamount":
							sortFieldName = "[OrderNumber]";
							break;
						case "id_dept":
							sortFieldName = "[DepartmentName]";
							break;
						case "order_date":
							sortFieldName = "[CreationDate]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
						RequestRows = this._data.FullData ? totalCount : this._data.PageSize
					};
				}
				#endregion

				/// 
				var orderEntites = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var statType = Enums.BudgetEnums.getStatisticsFromName(this._data.Type);

				if(this._user.IsGlobalDirector)
				{
					orderEntites.AddRange(getData(_data.Filter, statType, _data.Year, _data.CompanyIds, null, null, dataSorting, dataPaging, out int count));
					totalCount = count;
				}
				else
				{
					var userEntiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

					// - if user is director, show all PO in company
					var companyDirectorEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntiy.Id });
					if(companyDirectorEntities != null && companyDirectorEntities.Count > 0)
					{
						orderEntites.AddRange(getData(_data.Filter, statType, _data.Year, companyDirectorEntities.Select(x => x.Id)?.ToList(), null, null, dataSorting, dataPaging, out int count2));
						totalCount = count2;

					}

					// - if user has Purchase profile, show all PO in Company
					var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
						?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
					var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntiy.CompanyId ?? -1);
					if(companyExtensionEntity != null && userProfileIds.Exists(x => x == companyExtensionEntity.PurchaseProfileId))
					{
						orderEntites.AddRange(getData(_data.Filter, statType, _data.Year, new List<int> { userEntiy.CompanyId ?? -1 }, null, null, dataSorting, dataPaging, out int count3));
						totalCount = count3;

					}

					// - if user is Head of Dept, show all PO in department
					var departmentHeadEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntiy.Id);
					if(departmentHeadEntities != null && departmentHeadEntities.Count > 0)
					{
						orderEntites.AddRange(getData(_data.Filter, statType, _data.Year, null, departmentHeadEntities.Select(x => (int)x.Id)?.ToList(), null, dataSorting, dataPaging, out int count4));
						totalCount = count4;
					}

					orderEntites.AddRange(getData(_data.Filter, statType, _data.Year, _data.CompanyIds, null, this._user.Id, dataSorting, dataPaging, out var count5));
					totalCount = count5;
				}

				orderEntites = orderEntites?.DistinctBy(x => x.Id)?.ToList();
				// - 2025-04-01 - filter by company
				if(_data.CompanyIds?.Count>0)
				{
					orderEntites = orderEntites.Where(x => _data.CompanyIds.Exists(y => y == x.CompanyId))?.ToList();
				}
				totalCount = orderEntites?.Count ?? 0;

				//Orders Budgets Optimisation 11-03-2025 
				List<OrderOptimisedModel> BudgetsList = new List<OrderOptimisedModel>();
				BudgetsList = Helpers.Processings.Budget.Order.GetOrderOptimisedModels(orderEntites, out var errorsBudgets);
				var r = this._data.PageSize > 0 ? totalCount / decimal.Parse((this._data.PageSize).ToString()) : 0;

				if(errorsBudgets != null && errorsBudgets.Count > 0)
					return ResponseModel<OrderStatisticsResponseModel>.FailureResponse(errorsBudgets);

				if(BudgetsList is null || BudgetsList.Count == 0)
				{
					return ResponseModel<OrderStatisticsResponseModel>.SuccessResponse(new OrderStatisticsResponseModel
					{
						Items = new List<OrderOptimisedModel>(),
						PageRequested = this._data.RequestedPage,
						PageSize = this._data.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});
				}



				OrderStatisticsResponseModel orderStatisticsResponseModel = new OrderStatisticsResponseModel
				{
					Items = BudgetsList,
					PageRequested = _data.RequestedPage,
					PageSize = _data.PageSize,
					TotalCount = BudgetsList != null && BudgetsList.Count > 0 ? totalCount : 0,
					TotalPageCount = BudgetsList != null && BudgetsList.Count > 0 ?
					_data.PageSize > 0 ? (int)Math.Ceiling(((decimal)totalCount / _data.PageSize)) : 0 : 0,
				};



				return ResponseModel<OrderStatisticsResponseModel>.SuccessResponse(orderStatisticsResponseModel);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<OrderStatisticsResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<OrderStatisticsResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<OrderStatisticsResponseModel>.SuccessResponse();
		}

		internal List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getData(string filter, Enums.BudgetEnums.StatisticsTypes type, int year, List<int> companyIds, List<int> departmentIds, int? userId, Infrastructure.Data.Access.Settings.SortingModel dataSorting, Infrastructure.Data.Access.Settings.PaginModel dataPaging, out int totalCount)
		{

			var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
			int count = 0;
			// -
			switch(type)
			{
				case Enums.BudgetEnums.StatisticsTypes.Unplaced:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				case Enums.BudgetEnums.StatisticsTypes.Unconfirmed:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				case Enums.BudgetEnums.StatisticsTypes.DeliveryOverdue:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				case Enums.BudgetEnums.StatisticsTypes.NextDeliveries:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				case Enums.BudgetEnums.StatisticsTypes.Booked:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				case Enums.BudgetEnums.StatisticsTypes.OpenLeasing:
					orderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId, sorting: dataSorting, paging: dataPaging) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					count = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(filter: filter, year: year, companyIds: companyIds, departmentIds: departmentIds, employeeId: userId);
					break;
				default:
					break;
			}

			// -
			orderEntities = orderEntities.Distinct()?.ToList();
			totalCount = count;
			return orderEntities;
		}
	}

}
