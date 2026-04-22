using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteBudgetLandHandler: IHandle<Models.Budget.GetLandAssignementModel, ResponseModel<int>>
	{
		private Models.Budget.GetLandAssignementModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteBudgetLandHandler(Models.Budget.GetLandAssignementModel data, Identity.Models.UserModel user)
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
					TotalSpent = _data.TotalSpent
				};
				var budgetlandentity = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(_data.ID);
				var budgetlandsupplemententity = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.GetSupplementLandBdg(_data.ID);
				if(budgetlandentity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Delete(budgetlandentity.ID));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignDeleteLand)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var BudgetLandID = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data.ID);
			var CountDepts = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetDeptsCount(this._data.Land_name, this._data.B_year);
			var landname = this._data.Land_name;
			var year = this._data.B_year;
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

			if(CountDepts > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "You can't Delete A land with assigned departements" }
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
