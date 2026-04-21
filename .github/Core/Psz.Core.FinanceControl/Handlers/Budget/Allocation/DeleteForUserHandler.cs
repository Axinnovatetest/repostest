using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteForUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public DeleteForUserHandler(Identity.Models.UserModel user, int data)
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
				var allocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Get(this._data);
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(allocation?.UserId ?? -1);
				try
				{
					var departmentAlloactions = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear((int)userEntity.DepartmentId, allocation.Year);

					departmentAlloactions.AmountAllocatedToUsers = departmentAlloactions.AmountAllocatedToUsers - allocation.AmountYear;
					departmentAlloactions.LastEditTime = DateTime.Now;
					departmentAlloactions.LastEditUserId = this._user.Id;
					// -
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(departmentAlloactions);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
					throw;
				}

				allocation.UserId = -999;
				allocation.UserName = $"{allocation.UserName}_deleted_{DateTime.Today.ToString("dd-MM-yyyy")}";
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(allocation));
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
				return ResponseModel<int>.FailureResponse("User not found");

			var userAllocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Get(this._data);
			if(userAllocationEntity == null)
				return ResponseModel<int>.FailureResponse("Allocation not found");

			if(userAllocationEntity.AmountSpent > 0)
				return ResponseModel<int>.FailureResponse($"Cannot delete User Allocation. User has already spent [{userAllocationEntity.AmountSpent}].");

			var userOrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenByUser(userAllocationEntity.UserId);
			if(userOrderEntities != null && userOrderEntities.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete User Allocation. User has open orders");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
