using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson
{
	public class GetSalutationContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSalutationContactPersonHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<KeyValuePair<string, string>>();
				var adressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get();
				if(adressContactPersonEntities != null && adressContactPersonEntities.Count > 0)
				{
					foreach(var adressContactPersonEntity in adressContactPersonEntities)
					{
						responseBody.Add(new KeyValuePair<string, string>(adressContactPersonEntity.Anrede.Trim(), $"{adressContactPersonEntity.ID}||{adressContactPersonEntity.Anrede.Trim()}"));
					}
				}

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
