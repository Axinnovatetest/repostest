using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetE_RechnungConfigSinglePositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<E_RechnungTypedCustomerModel>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetE_RechnungConfigSinglePositionHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<E_RechnungTypedCustomerModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var item = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.Get(_data);
				var result = new E_RechnungTypedCustomerModel(item);
				return ResponseModel<E_RechnungTypedCustomerModel>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<E_RechnungTypedCustomerModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<E_RechnungTypedCustomerModel>.AccessDeniedResponse();
			}

			return ResponseModel<E_RechnungTypedCustomerModel>.SuccessResponse();
		}
	}
}
