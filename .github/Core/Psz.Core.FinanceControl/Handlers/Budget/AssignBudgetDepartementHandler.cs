using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class AssignBudgetDepartementHandler
	{
		private Models.Budget.GetDepartementAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public AssignBudgetDepartementHandler(Models.Budget.GetDepartementAssignementModel data, Identity.Models.UserModel user)
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
				var deptBudgetEntity = new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_departementEntity()
				{
					Land_name = _data.Land_name,
					Departement_name = _data.Departement_name,
					budget = _data.budget,
					B_year = _data.B_year,
					LandId = _data.LandId,
					TotalSpent = _data.TotalSpent,
					DepartmentId = _data.DepartmentId

				};
				var InsertedBudgetDept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Insert(deptBudgetEntity);
				return ResponseModel<int>.SuccessResponse(InsertedBudgetDept);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.AssignCreateDept)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(this._data.B_year < DateTime.Now.Year)
			{
				return ResponseModel<int>.FailureResponse("Allocation not allowed for previous year");
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
