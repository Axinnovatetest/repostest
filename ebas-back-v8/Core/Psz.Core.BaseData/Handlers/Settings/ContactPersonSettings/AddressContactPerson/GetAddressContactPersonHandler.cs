using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson
{
	public class GetAddressContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAddressContactPersonHandler(Identity.Models.UserModel user)
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
				var addressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity>();
				// - 
				if(refreshAnreden(addressContactPersonEntities))
				{
					addressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get();
				}

				// -
				if(addressContactPersonEntities != null && addressContactPersonEntities.Count > 0)
				{
					addressContactPersonEntities = addressContactPersonEntities.OrderBy(x => x.ID).ToList();
					foreach(var addressContactPersonEntity in addressContactPersonEntities)
					{
						responseBody.Add(new KeyValuePair<string, string>(addressContactPersonEntity.Anrede.Trim(), $"{addressContactPersonEntity.ID} || {addressContactPersonEntity.Anrede.Trim()}"));
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
		public static bool refreshAnreden(
			List<Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity> currentEntities)
		{
			var newEntities = new List<Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity>();

			// - from Ansprechpartner
			List<string> data = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetAnreden()
				?? new List<string>();
			foreach(var item in data)
			{
				if(!currentEntities.Exists(x => x.Anrede.Trim().ToLower() == item.Trim().ToLower()))
				{
					newEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity
					{
						Anrede = item
					});
				}
			}

			// - from address
			data = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAnreden()
				?? new List<string>();
			foreach(var item in data)
			{
				if(!currentEntities.Exists(x => x.Anrede.Trim().ToLower() == item.Trim().ToLower()))
				{
					newEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.Adressen_anredenEntity
					{
						Anrede = item
					});
				}
			}

			// - 
			if(newEntities.Count > 0)
			{
				Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Insert(newEntities.DistinctBy(x => x.Anrede?.Trim())?.ToList());
				return true;
			}

			// - 
			return false;
		}
	}
}
