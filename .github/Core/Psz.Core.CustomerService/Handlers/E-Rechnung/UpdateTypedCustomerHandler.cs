using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class UpdateTypedCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private E_RechnungTypedCustomerModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateTypedCustomerHandler(Identity.Models.UserModel user, E_RechnungTypedCustomerModel data)
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

				var entity = _data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.Update(entity);

				return ResponseModel<int>.SuccessResponse(response);
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

			if(!Infrastructure.Services.Email.Helpers.IsValidEmail(_data.Email))
			{
				return ResponseModel<int>.FailureResponse($"Email [{_data.Email}] not valid");
			}

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
