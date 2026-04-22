using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteForCompanyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteForCompanyHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				//var allocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Get(this._data);
				//Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.DeleteByCompaniesAndYear(new List<int> { allocationEntity.CompanyId }, allocationEntity.Year);
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Delete(this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || !(this._user.IsGlobalDirector || this._user.IsCorporateDirector))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<int>.FailureResponse("user not found");

			var allocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Get(this._data);
			if(allocationEntity == null)
				return ResponseModel<int>.FailureResponse("Allocation not found");

			var departmentEntites = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompany(allocationEntity.CompanyId);
			var departmentAllocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentsAndYear(departmentEntites?.Select(x => (int)x.Id)?.ToList(), allocationEntity.Year);
			if(departmentAllocations != null && departmentAllocations.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete Company Allocation with departments already allocated");

			var supplementEntities = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { allocationEntity.CompanyId }, allocationEntity.Year);
			if(supplementEntities != null && supplementEntities.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete Company Allocation with supplements");

			//var userInCompany = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByCompanyId(allocationEntity.CompanyId);
			//var userAllocated = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(userInCompany?.Select(x => (int)x.Id)?.ToList(), allocationEntity.Year);
			//foreach (var userAllocationItem in userAllocated)
			//{
			//    if (userAllocationItem.AmountSpent > 0)
			//    {
			//        return ResponseModel<int>.FailureResponse("Expenses already exist in Allocation");
			//    }

			//}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
