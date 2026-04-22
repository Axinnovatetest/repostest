using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetAssignLandByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.GetLandAssignementModel>>

	{

		private int _data { get; set; }

		public GetAssignLandByIdHandler(int id)
		{

			this._data = id;

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

				var budgetLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.Get(this._data);
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
