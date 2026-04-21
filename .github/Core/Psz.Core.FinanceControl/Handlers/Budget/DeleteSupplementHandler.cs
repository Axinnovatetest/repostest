using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteSupplementHandler: IHandle<Models.Budget.SupplementLandModel, ResponseModel<int>>
	{
		private int _SupplementID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteSupplementHandler(int supplementid, Identity.Models.UserModel user)
		{
			this._SupplementID = supplementid;
			this._user = user;
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

				var spplemententity = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.Get(_SupplementID);

				if(spplemententity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.Delete(spplemententity.Id));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignCreateLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var SupplementID = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.Get(this._SupplementID);
			var BudgetLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(SupplementID.Id_AL);
			var sum_budget_depts = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetDepartements(BudgetLand.LandId, BudgetLand.B_year);
			//with the last value
			var sum_budget_LandSupplements = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetLandSupplements(SupplementID.Id_AL);

			var previousSupplement = SupplementID.Supplement_Budget;
			var budgetLand = BudgetLand.budget;
			var lastbudget = (sum_budget_LandSupplements - previousSupplement) + budgetLand;

			var errors = new List<ResponseModel<int>.ResponseError>();

			if(SupplementID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Supplement not found"}
					}
				};
			}
			if(lastbudget < sum_budget_depts)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
                        //new ResponseModel<int>.ResponseError {Key = "", Value = "New land budget ("+entred_new_budget+") is less than the sum of the budgets of its departements ("+sum_budget_depts+")" }
                        new ResponseModel<int>.ResponseError {Key = "", Value = "Can't delete this supplement, The New Total land budget ("+lastbudget+") is less than the already used Amount in the Land ("+sum_budget_depts+")" }
					}
				};
			}
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
