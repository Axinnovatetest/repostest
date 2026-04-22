using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOverviewAmountDistinctHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetOverviewAmountDistinctHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var validatedNonPlacedEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var placedNonSupplierConfirmedEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var supplierDeliveryOverdueEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var upcomingDeliveriesEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var bookedEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				var openLeasingEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { };
				/// 
				if(this._user.IsGlobalDirector)
				{
					validatedNonPlacedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					placedNonSupplierConfirmedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					supplierDeliveryOverdueEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					bookedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					upcomingDeliveriesEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					openLeasingEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(year: _data) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
				}
				else
				{
					var userEntiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

					// - if user is director, show all PO in company
					var companyDirectorEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntiy.Id });
					if(companyDirectorEntities != null && companyDirectorEntities.Count > 0)
					{
						validatedNonPlacedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						placedNonSupplierConfirmedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						supplierDeliveryOverdueEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						bookedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						upcomingDeliveriesEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						openLeasingEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(year: _data, companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					}

					// - if user has Purchase profile, show all PO in Company
					var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
						?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
					var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntiy.CompanyId ?? -1);
					if(companyExtensionEntity != null && userProfileIds.Exists(x => x == companyExtensionEntity.PurchaseProfileId))
					{
						validatedNonPlacedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced( year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						placedNonSupplierConfirmedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						supplierDeliveryOverdueEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						bookedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						upcomingDeliveriesEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						openLeasingEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(year: _data, companyIds: new List<int> { userEntiy.CompanyId ?? -1 }) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					}

					// - if user is Head of Dept, show all PO in department
					var departmentHeadEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntiy.Id);
					if(departmentHeadEntities != null && departmentHeadEntities.Count > 0)
					{
						validatedNonPlacedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						placedNonSupplierConfirmedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						supplierDeliveryOverdueEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						bookedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						upcomingDeliveriesEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
						openLeasingEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(year: _data, companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					}

					// - add user issued PO
					validatedNonPlacedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					placedNonSupplierConfirmedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					supplierDeliveryOverdueEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					bookedEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					upcomingDeliveriesEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
					openLeasingEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing(year: _data, companyIds: null, departmentIds: null, employeeId: this._user.Id) ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { });
				}

				// - 
				validatedNonPlacedEntities = validatedNonPlacedEntities.Distinct()?.ToList();
				placedNonSupplierConfirmedEntities = placedNonSupplierConfirmedEntities.Distinct()?.ToList();
				supplierDeliveryOverdueEntities = supplierDeliveryOverdueEntities.Distinct()?.ToList();
				bookedEntities = bookedEntities.Distinct()?.ToList();
				upcomingDeliveriesEntities = upcomingDeliveriesEntities.Distinct()?.ToList();
				openLeasingEntities = openLeasingEntities.Distinct()?.ToList();

				var responseBody = new List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel> {
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.Unplaced, Name=$"{Enums.BudgetEnums.StatisticsTypes.Unplaced}", Value=$"{validatedNonPlacedEntities}", Count=validatedNonPlacedEntities?.Count() ?? 0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.Unconfirmed, Name=$"{Enums.BudgetEnums.StatisticsTypes.Unconfirmed}", Value=$"{placedNonSupplierConfirmedEntities}", Count=placedNonSupplierConfirmedEntities?.Count() ?? 0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.DeliveryOverdue, Name=$"{Enums.BudgetEnums.StatisticsTypes.DeliveryOverdue}", Value=$"{supplierDeliveryOverdueEntities}", Count=supplierDeliveryOverdueEntities?.Count() ?? 0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.NextDeliveries, Name=$"{Enums.BudgetEnums.StatisticsTypes.NextDeliveries}", Value=$"{upcomingDeliveriesEntities}", Count=upcomingDeliveriesEntities?.Count() ?? 0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.Booked, Name=$"{Enums.BudgetEnums.StatisticsTypes.Booked}", Value=$"{bookedEntities}", Count=bookedEntities?.Count() ?? 0 },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Id = (int)Enums.BudgetEnums.StatisticsTypes.OpenLeasing, Name=$"{Enums.BudgetEnums.StatisticsTypes.OpenLeasing}", Value=$"{openLeasingEntities}", Count=openLeasingEntities?.Count() ?? 0 },
				};
				return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>.SuccessResponse();
		}
	}

}
