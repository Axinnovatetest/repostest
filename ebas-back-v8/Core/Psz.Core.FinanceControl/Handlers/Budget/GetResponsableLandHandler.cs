using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetResponsableLandHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Land_Responsable_JointModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetResponsableLandHandler(Identity.Models.UserModel user, int land)
		{
			this._user = user;
			this._data = land;


		}
		public ResponseModel<Models.Budget.Land_Responsable_JointModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.Land_Responsable_JointModel();
				if(this._data != -1)
				{
					var budgetLand = Infrastructure.Data.Access.Tables.FNC.Land_Responsable_JointAccess.GetSiteResponsable(this._data);
					responseBody.ID = budgetLand.ID;
					responseBody.ID_Land = budgetLand.ID_Land;
					responseBody.ID_user = budgetLand.ID_user;
					responseBody.Username = budgetLand.Username;
					responseBody.Name = budgetLand.Name;
				}
				return ResponseModel<Models.Budget.Land_Responsable_JointModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Land_Responsable_JointModel> Validate()
		{

			return ResponseModel<Models.Budget.Land_Responsable_JointModel>.SuccessResponse();


		}

	}
}
