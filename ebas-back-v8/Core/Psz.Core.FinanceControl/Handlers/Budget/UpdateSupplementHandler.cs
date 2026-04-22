using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateSupplementHandler: IHandle<Models.Budget.SupplementLandModel, ResponseModel<int>>
	{
		private Models.Budget.SupplementLandModel _data { get; set; }
		//private Models.Budget.GetLandAsiignementModel _dataLand { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateSupplementHandler(Models.Budget.SupplementLandModel data, Identity.Models.UserModel user)
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

				var Supplemententity = new Infrastructure.Data.Entities.Tables.FNC.Supplement_Budget_LandEntity()
				{
					Id = _data.Id,
					Id_AL = _data.Id_AL,
					Supplement_Budget = _data.Supplement_Budget,
					Creation_Date = _data.Creation_Date
				};
				var updatedSupplement = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.Update(Supplemententity);
				return ResponseModel<int>.SuccessResponse(updatedSupplement);
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

			var BudgetLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data.Id_AL);
			var sum_budget_depts = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetDepartements(BudgetLand.LandId, BudgetLand.B_year);
			//with the last value
			var sum_budget_LandSupplements = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetLandSupplements(this._data.Id_AL);

			var SupplementID = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.Get(this._data.Id);

			var entred_new_budgetSupplemet = this._data.Supplement_Budget;
			var previousSupplement = SupplementID.Supplement_Budget;
			var budgetLand = BudgetLand.budget;
			var lastbudget = (sum_budget_LandSupplements - previousSupplement) + budgetLand + entred_new_budgetSupplemet;


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
                        new ResponseModel<int>.ResponseError {Key = "", Value = "Can't update this supplement to value less then ("+previousSupplement+") ,The New Total land budget ("+lastbudget+") is less than the already used Amount in the Land ("+sum_budget_depts+")" }
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
