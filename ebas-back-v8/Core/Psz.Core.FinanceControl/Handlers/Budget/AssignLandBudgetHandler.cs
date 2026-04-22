using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class AssignLandBudgetHandler
	{
		private Models.Budget.GetLandAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public AssignLandBudgetHandler(Models.Budget.GetLandAssignementModel data, Identity.Models.UserModel user)
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
				var landsBudgetEntity = new Infrastructure.Data.Entities.Tables.FNC.Assign_budget_landEntity()
				{
					Land_name = _data.Land_name,
					budget = _data.budget,
					B_year = _data.B_year,
					LandId = _data.LandId,
					TotalSpent = _data.TotalSpent

				};
				var InsertedBudgetLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Insert(landsBudgetEntity);
				return ResponseModel<int>.SuccessResponse(InsertedBudgetLand);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.AssignCreateLand)
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
