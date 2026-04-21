using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateLandBudgetHandler: IHandle<Models.Budget.GetLandAssignementModel, ResponseModel<int>>
	{
		private Models.Budget.GetLandAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateLandBudgetHandler(Models.Budget.GetLandAssignementModel data, Identity.Models.UserModel user)
		{
			this._data = data;
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

				var Landbudgetentity = new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity()
				{
					ID = _data.ID,
					Land_name = _data.Land_name,
					budget = _data.budget,
					B_year = _data.B_year,
					LandId = _data.LandId,
					TotalSpent = _data.TotalSpent,
				};
				var updatedLandbudget = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Update(Landbudgetentity);
				return ResponseModel<int>.SuccessResponse(updatedLandbudget);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignEditLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var BudgetLandID = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data.ID);
			var sum_budget_depts = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetDepartements(this._data.LandId, this._data.B_year);
			var sum_budget_LandSupplements = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetLandSupplements(this._data.ID);


			var entred_new_budget = this._data.budget;
			var lastbudget = sum_budget_LandSupplements + this._data.budget;
			var errors = new List<ResponseModel<int>.ResponseError>();

			if(BudgetLandID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Land assignement not found"}
					}
				};
			}
			// if (entred_new_budget<sum_budget_depts)
			if(lastbudget < sum_budget_depts)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
                        //new ResponseModel<int>.ResponseError {Key = "", Value = "New land budget ("+entred_new_budget+") is less than the sum of the budgets of its departements ("+sum_budget_depts+")" }
                        new ResponseModel<int>.ResponseError {Key = "", Value = "The New Total land budget ("+lastbudget+") is less than the already used Amount in the Departement ("+sum_budget_depts+")" }
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
