using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Address.AddressModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Address.AddressModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<Models.Address.AddressModel>.SuccessResponse(new Models.Address.AddressModel(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<Models.Address.AddressModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Address.AddressModel>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<Models.Address.AddressModel>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data) == null)
				return ResponseModel<Models.Address.AddressModel>.FailureResponse(key: "2", value: "Address not found");

			return ResponseModel<Models.Address.AddressModel>.SuccessResponse();
		}

	}
}
