using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteBudgetDepartementHandler: IHandle<Models.Budget.GetDepartementAssignementModel, ResponseModel<int>>
	{
		private Models.Budget.GetDepartementAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteBudgetDepartementHandler(Models.Budget.GetDepartementAssignementModel data, Identity.Models.UserModel user)
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
				var Landbudgetentity = new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity()
				{
					ID = _data.ID,
					Land_name = _data.Land_name,
					Departement_name = _data.Departement_name,
					budget = _data.budget,
					B_year = _data.B_year,
				};
				var budgetdeptentity = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Get(this._data.ID);
				if(budgetdeptentity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Delete(budgetdeptentity.ID));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignDeleteDept)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var BudgetDeptID = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Get(this._data.ID);
			var CountUsers = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.GetUsersCount(this._data.Land_name, this._data.Departement_name, this._data.B_year);
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

			if(CountUsers > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "You can't Delete A Departement with assigned Users" }
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
