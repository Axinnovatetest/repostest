using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation.Supplement
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddForCompanyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Allocation.Company.SupplementUpdateModel _data { get; set; }

		public AddForCompanyHandler(Identity.Models.UserModel user, Models.Budget.Allocation.Company.SupplementUpdateModel model)
		{
			this._user = user;
			this._data = model;
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

				var allocationEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.CompanyId, this._data.Year);
				allocationEntity.AmountSupplements = (allocationEntity.AmountSupplements ?? 0) + this._data.AmountInitial;
				allocationEntity.LastEditTime = DateTime.Now;
				allocationEntity.LastEditUserId = this._user.Id;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(allocationEntity);

				this._data.CreationTime = DateTime.Now;
				this._data.CreationUserId = this._user.Id;
				this._data.ComapnyName = allocationEntity.ComapnyName;
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.Insert(this._data.ToEntity()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId) == null)
				return ResponseModel<int>.FailureResponse("Company not found");

			var allocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.CompanyId, this._data.Year);
			if(allocations == null)
			{
				return ResponseModel<int>.FailureResponse($"Company not allocated for year {this._data.Year}");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
