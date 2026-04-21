using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteForDepartmentHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteForDepartmentHandler(Identity.Models.UserModel user, int data)
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
				try
				{
					var department = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Get(this._data);
					var dept = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(department.DepartmentId);
					var allocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Get(this._data);
					var companyAlloaction = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear((int)dept.CompanyId, allocation.Year);

					companyAlloaction.AmountAllocatedToDepartments = companyAlloaction.AmountAllocatedToDepartments - allocation.AmountInitial;
					companyAlloaction.LastEditTime = DateTime.Now;
					companyAlloaction.LastEditUserId = this._user.Id;
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyAlloaction);

					// -
					var userInDepartment = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { department.DepartmentId });
					var userAllocated = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.DeleteByUsersAndYear(userInDepartment?.Select(x => (int)x.Id)?.ToList(), department.Year);
				} catch(Exception)
				{

				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Delete(this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<int>.FailureResponse("user not found");

			var allocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Get(this._data);
			if(allocationEntity == null)
				return ResponseModel<int>.FailureResponse("Allocation not found");

			var userInDepartment = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { allocationEntity.DepartmentId });
			var userAllocated = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(userInDepartment?.Select(x => (int)x.Id)?.ToList(), allocationEntity.Year);
			if(userAllocated != null && userAllocated.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete Department Allocation with user already allocated");

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
