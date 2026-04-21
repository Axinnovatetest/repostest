using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetsupplementByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.SupplementLandModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetsupplementByIdHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;

		}
		public ResponseModel<Models.Budget.SupplementLandModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.SupplementLandModel();

				var supplementLand = Infrastructure.Data.Access.Tables.FNC.Supplement_Budget_LandAccess.GetSupplementLandBdgID(this._data);
				responseBody.Id = supplementLand.Id;
				responseBody.Id_AL = supplementLand.Id_AL;
				responseBody.Supplement_Budget = supplementLand.Supplement_Budget;
				responseBody.Creation_Date = supplementLand.Creation_Date;

				return ResponseModel<Models.Budget.SupplementLandModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.SupplementLandModel> Validate()
		{

			return ResponseModel<Models.Budget.SupplementLandModel>.SuccessResponse();


		}

	}
}
