using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateDepartementBudgetHandler: IHandle<Models.Budget.GetDepartementAssignementModel, ResponseModel<int>>
	{
		private Models.Budget.GetDepartementAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateDepartementBudgetHandler(Models.Budget.GetDepartementAssignementModel data, Identity.Models.UserModel user)
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

				var Deptbudgetentity = new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity()
				{
					ID = _data.ID,
					Land_name = _data.Land_name,
					Departement_name = _data.Departement_name,
					budget = _data.budget,
					B_year = _data.B_year,
					LandId = _data.LandId,
					TotalSpent = _data.TotalSpent,
					DepartmentId = _data.DepartmentId
				};
				var updatedBudgetbudget = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Update(Deptbudgetentity);
				return ResponseModel<int>.SuccessResponse(updatedBudgetbudget);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignEditDept)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var BudgetDeptID = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Get(this._data.ID);
			var IDLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetIDbyName(this._data.Land_name, this._data.B_year);
			var CHKDept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.CheckBudgetDept(this._data.Land_name, this._data.B_year, IDLand.ID, this._data.Departement_name);
			var CHKUsers = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.GetSumBudgetUsers(this._data.Land_name, this._data.Departement_name, this._data.B_year);
			var LandBudget = CHKDept.LandBudget;
			var SupplementLandBudget = CHKDept.LandBudgetSupplement;
			var SUMDeptBudget = CHKDept.SOMME_DEPT;
			var TotalBudgetLand = LandBudget + SupplementLandBudget;
			double? EntredValue = this._data.budget;
			//****
			var sum = (SUMDeptBudget - BudgetDeptID.budget) + EntredValue;
			//var reste = LandBudget - SUMDeptBudget;
			var reste = TotalBudgetLand - SUMDeptBudget;
			if(reste < 0)
			{ reste = 0; }
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(BudgetDeptID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Departement assignement not found"}
					}
				};
			}
			// if (sum > LandBudget)
			if(sum > TotalBudgetLand)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Land Budget Exceeded,This Land Still Have "+reste}
					}
				};
			}
			if(CHKUsers > EntredValue)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "New Departement budget ("+EntredValue+") is less than the sum of the budgets of its Users ("+CHKUsers+")"}
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
