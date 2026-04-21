using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetAssignLandByNameHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.GetLandAssignementModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data1 { get; set; }

		public GetAssignLandByNameHandler(Identity.Models.UserModel user, string land, int year)
		{
			this._user = user;
			this._data = land;
			this._data1 = year;

		}
		public ResponseModel<Models.Budget.GetLandAssignementModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.GetLandAssignementModel();

				var budgetLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetIDbyName(this._data, this._data1);
				responseBody.ID = budgetLand.ID;
				responseBody.Land_name = budgetLand.Land_name;
				responseBody.budget = budgetLand.budget;
				responseBody.B_year = budgetLand.B_year;
				return ResponseModel<Models.Budget.GetLandAssignementModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.GetLandAssignementModel> Validate()
		{

			return ResponseModel<Models.Budget.GetLandAssignementModel>.SuccessResponse();


		}

	}
}
