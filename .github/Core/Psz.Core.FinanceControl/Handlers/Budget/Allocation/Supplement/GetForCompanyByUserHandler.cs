using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation.Supplement
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetForCompanyByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		private int _year { get; set; }
		public GetForCompanyByUserHandler(Identity.Models.UserModel user, int id, int year)
		{
			this._user = user;
			this._data = id;
			this._year = year;
		}

		public ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data);

				var allocationEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
				if(userEntity.IsGlobalDirector == true || this._user.Access?.Financial?.Budget?.AssignAllSites == true || (userCompanies.DirectorId.HasValue && userCompanies.DirectorId.Value == userEntity.Id))
				{
					allocationEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { userCompanies.Id }, _year));
				}

				return ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>.SuccessResponse(allocationEntities?.Select(x => new Models.Budget.Allocation.Company.SupplementUpdateModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>.FailureResponse("user not found");

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>.FailureResponse("company not found");

			return ResponseModel<List<Models.Budget.Allocation.Company.SupplementUpdateModel>>.SuccessResponse();
		}
	}
}
