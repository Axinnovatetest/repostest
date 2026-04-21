using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetOverviewHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var validatedNonPlacedCount = 0;
				var placedNonSupplierConfirmedCount = 0;
				var supplierDeliveryOverdueCount = 0;
				var upcomingDeliveriesCount = 0;
				var bookedCount = 0;
				var openLeasingCount = 0;
				if(this._user.IsGlobalDirector)
				{
					validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count();
					placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count();
					supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count();
					bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count();
					upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count();
					openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count();
				}
				else
				{
					var userEntiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

					// - if user is director, show all PO in company
					var companyDirectorEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntiy.Id });
					if(companyDirectorEntities != null && companyDirectorEntities.Count > 0)
					{
						validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
					}

					// - if user has Purchase profile, show all PO in Company
					var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
						?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
					var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntiy.CompanyId ?? -1);
					if(companyExtensionEntity != null && userProfileIds.Exists(x => x == companyExtensionEntity.PurchaseProfileId))
					{
						validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
					}

					// - if user is Head of Dept, show all PO in department
					var departmentHeadEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntiy.Id);
					if(departmentHeadEntities != null && departmentHeadEntities.Count > 0)
					{
						validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
					}

					// - add user issued PO
					validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
				}

				var responseBody = new List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel> {
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="validatedNonPlacedCount", Value=$"{validatedNonPlacedCount}" },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="placedNonSupplierConfirmedCount", Value=$"{placedNonSupplierConfirmedCount}" },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="supplierDeliveryOverdueCount", Value=$"{supplierDeliveryOverdueCount}" },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="upcomingDeliveriesCount", Value=$"{upcomingDeliveriesCount}" },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="bookedCount", Value=$"{bookedCount}" },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="openLeasingCount", Value=$"{openLeasingCount}" },
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
