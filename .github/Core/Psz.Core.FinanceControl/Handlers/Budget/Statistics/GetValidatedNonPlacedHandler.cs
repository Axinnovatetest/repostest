using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetValidatedNonPlacedHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetValidatedNonPlacedHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var leasingOrderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				if(this._user.IsGlobalDirector)
					leasingOrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced();
				else
				{
					var userEntiy = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);

					// - if user is director, show all PO in company
					var companyDirectorEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntiy.Id });
					if(companyDirectorEntities != null && companyDirectorEntities.Count > 0)
					{
						leasingOrderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(companyIds: companyDirectorEntities.Select(x => x.Id)?.ToList()));
					}

					// - if user has Purchase profile, show all PO in Company
					var userFNCProfileIds = (Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
						?? new List<Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity>()).Select(x => x.AccessProfileId).ToList();
					var userProfileIds = (Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(userFNCProfileIds)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity>())?.Select(x => x.MainAccessProfileId).ToList();
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntiy.CompanyId ?? -1);
					if(companyExtensionEntity != null && userProfileIds.Exists(x => x == companyExtensionEntity.PurchaseProfileId))
					{
						leasingOrderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(companyIds: new List<int> { userEntiy.CompanyId ?? -1 }));
					}

					// - if user is Head of Dept, show all PO in department
					var departmentHeadEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntiy.Id);
					if(departmentHeadEntities != null && departmentHeadEntities.Count > 0)
					{
						leasingOrderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(companyIds: null, departmentIds: departmentHeadEntities.Select(x => (int)x.Id)?.ToList()));
					}

					// - add user issued PO
					leasingOrderEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetValidatedNonPlaced(companyIds: null, departmentIds: null, employeeId: this._user.Id));
				}
				leasingOrderEntities = leasingOrderEntities?.DistinctBy(x => x.Id)?.ToList();

				var responseBody = Helpers.Processings.Budget.Order.GetOrderModels(leasingOrderEntities, this._user, out var errors);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(errors);

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}
	}

}
