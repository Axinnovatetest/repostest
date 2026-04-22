using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson
{
	public class GetAddressContactPersonForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAddressContactPersonForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Settings.AddressContactPerson.AddressContactPersonModel>();
				var addressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get();

				if(addressContactPersonEntities != null && addressContactPersonEntities.Count > 0)
				{
					foreach(var addressContactPersonEntity in addressContactPersonEntities)
					{
						responseBody.Add(new Models.Settings.AddressContactPerson.AddressContactPersonModel(addressContactPersonEntity));
					}
				}

				return ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.AddressContactPerson.AddressContactPersonModel>>.SuccessResponse();
		}
	}
}
