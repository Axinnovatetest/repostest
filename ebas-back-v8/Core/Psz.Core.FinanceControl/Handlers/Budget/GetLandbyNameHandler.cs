using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetLandbyNameHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.GetLandsModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }


		public GetLandbyNameHandler(Identity.Models.UserModel user, string land)
		{
			this._user = user;
			this._data = land;


		}
		public ResponseModel<Models.Budget.GetLandsModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.GetLandsModel();

				//var budgetLand = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.GetByName(this._data);
				//responseBody.ID = budgetLand.ID;
				//responseBody.Land_name = budgetLand.Land_name;

				return ResponseModel<Models.Budget.GetLandsModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.GetLandsModel> Validate()
		{

			return ResponseModel<Models.Budget.GetLandsModel>.SuccessResponse();


		}

	}
}
