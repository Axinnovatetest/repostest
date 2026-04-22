using System;

namespace Psz.Core.Apps.Settings.Handlers.Currency
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				/// 

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Delete(this._data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data) == null)
				return ResponseModel<int>.FailureResponse($"Currency not found");

			var openOrders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOpenByCurrency(this._data);
			if(openOrders != null && openOrders.Count > 0)
				return ResponseModel<int>.FailureResponse($"Currency is used in in-progress orders");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
