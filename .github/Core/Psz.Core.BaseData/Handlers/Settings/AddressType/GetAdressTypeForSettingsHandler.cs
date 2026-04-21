using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.AddressType
{
	public class GetAdressTypeForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.AddressType.AdreesTypeModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAdressTypeForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.AddressType.AdreesTypeModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var addressTypeEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get();

				var responseBody = new List<Models.AddressType.AdreesTypeModel>();

				foreach(var addressTypeEntity in addressTypeEntities)
				{
					responseBody.Add(new Models.AddressType.AdreesTypeModel(addressTypeEntity));
				}

				return ResponseModel<List<Models.AddressType.AdreesTypeModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.AddressType.AdreesTypeModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.AddressType.AdreesTypeModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.AddressType.AdreesTypeModel>>.SuccessResponse();
		}
	}
}
