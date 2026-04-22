using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOverviewAmountHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetOverviewAmountHandler(Identity.Models.UserModel user)
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
				var validatedNonPlacedAmount = 0m;
				var placedNonSupplierConfirmedAmount = 0m;
				var supplierDeliveryOverdueAmount = 0m;
				var upcomingDeliveriesAmount = 0m;
				var bookedAmount = 0m;
				var openLeasingAmount = 0m;
				/// 
				var validatedNonPlacedCount = 0;
				var placedNonSupplierConfirmedCount = 0;
				var supplierDeliveryOverdueCount = 0;
				var upcomingDeliveriesCount = 0;
				var bookedCount = 0;
				var openLeasingCount = 0;
				if(this._user.IsGlobalDirector)
				{
					validatedNonPlacedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Amount();
					placedNonSupplierConfirmedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Amount();
					supplierDeliveryOverdueAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Amount();
					bookedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Amount();
					upcomingDeliveriesAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Amount();
					openLeasingAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Amount();
					//-
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
						validatedNonPlacedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						placedNonSupplierConfirmedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						supplierDeliveryOverdueAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						bookedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						upcomingDeliveriesAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						openLeasingAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Amount(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList());
						// -
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
						validatedNonPlacedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						placedNonSupplierConfirmedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						supplierDeliveryOverdueAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						bookedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						upcomingDeliveriesAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						openLeasingAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Amount(companyIds: new List<int> { userEntiy.CompanyId ?? -1 });
						// -
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
						validatedNonPlacedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						placedNonSupplierConfirmedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						supplierDeliveryOverdueAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						bookedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						upcomingDeliveriesAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						openLeasingAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Amount(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						// - 
						validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
						openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList());
					}

					// - add user issued PO
					validatedNonPlacedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					placedNonSupplierConfirmedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					supplierDeliveryOverdueAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					bookedAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					upcomingDeliveriesAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					openLeasingAmount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Amount(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					// -
					validatedNonPlacedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					placedNonSupplierConfirmedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetPlacedNonSupplierConfirmed_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					supplierDeliveryOverdueCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetSupplierDeliveryOverdue_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					bookedCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBooked_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					upcomingDeliveriesCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetUpcomingDeliveries_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
					openLeasingCount += Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenLeasing_Count(companyIds: null, departmentIds: null, employeeId: this._user.Id);
				}

				var responseBody = new List<Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel> {
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Un-Placed", Value=$"{validatedNonPlacedAmount}", Count=validatedNonPlacedCount },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Unconfirmed", Value=$"{placedNonSupplierConfirmedAmount}", Count=placedNonSupplierConfirmedCount },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Delivery Overdue", Value=$"{supplierDeliveryOverdueAmount}", Count=supplierDeliveryOverdueCount },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Next Deliveries", Value=$"{upcomingDeliveriesAmount}", Count=upcomingDeliveriesCount },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Booked", Value=$"{bookedAmount}", Count=bookedCount },
				   new Models.Budget.Order.Statistics.OverviewModel.OverviewItemModel { Name="Open Leasings", Value=$"{openLeasingAmount}", Count=openLeasingCount },
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
