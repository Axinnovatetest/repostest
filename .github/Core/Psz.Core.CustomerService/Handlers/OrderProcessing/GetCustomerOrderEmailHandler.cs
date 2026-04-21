using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
    public class GetCustomerOrderEmailHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCustomerOrderEmailHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<string>.SuccessResponse(
				Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data)?.OrderAddress ?? "");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse();
		}
	}
}
