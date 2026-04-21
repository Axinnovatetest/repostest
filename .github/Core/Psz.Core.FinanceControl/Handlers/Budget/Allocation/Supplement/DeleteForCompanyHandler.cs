using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation.Supplement
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
				var supplementEntity = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.Get(this._data);
				var allocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(supplementEntity.CompanyId, supplementEntity.Year);
				allocationEntity.AmountSupplements = (allocationEntity.AmountSupplements ?? 0) - supplementEntity.AmountInitial;
				allocationEntity.LastEditTime = DateTime.Now;
				allocationEntity.LastEditUserId = this._user.Id;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(allocationEntity);


				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.Delete(this._data));
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

			var supplementEntity = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.Get(this._data);
			if(supplementEntity == null)
				return ResponseModel<int>.FailureResponse("Supplement not found");

			var allocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(supplementEntity.CompanyId, supplementEntity.Year);
			if(allocations == null)
			{
				return ResponseModel<int>.FailureResponse($"Company not allocated for year {supplementEntity.Year}");
			}

			var supplements = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { supplementEntity.CompanyId }, supplementEntity.Year)?.Sum(x => x.AmountInitial) ?? 0m;
			var companyUnallocatedAmount = allocations.AmountInitial + supplements - (allocations.AmountAllocatedToDepartments + allocations.AmountAllocatedToProjects);
			if(companyUnallocatedAmount < supplementEntity.AmountInitial)
				return ResponseModel<int>.FailureResponse($"Cannot delete supplement, company unallocated budget not enough [{companyUnallocatedAmount}]");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
