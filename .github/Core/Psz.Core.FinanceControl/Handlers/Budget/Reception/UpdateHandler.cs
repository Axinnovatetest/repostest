using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Reception
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Reception.UpdateModel _data { get; set; }

		public UpdateHandler(Identity.Models.UserModel user, Models.Budget.Reception.UpdateModel model)
		{
			this._user = user;
			this._data = model;
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

				// Update Order
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Update(
					this._data.ToEntity()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data.Nr) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
