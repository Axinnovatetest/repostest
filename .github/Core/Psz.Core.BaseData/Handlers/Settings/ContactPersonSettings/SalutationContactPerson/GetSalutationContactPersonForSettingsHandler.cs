using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson
{
	public class GetSalutationContactPersonForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSalutationContactPersonForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>();
				var adressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity>();

				// - 
				if(refreshBriefanrede(adressContactPersonEntities))
				{
					adressContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get();
				}

				// -
				if(adressContactPersonEntities != null && adressContactPersonEntities.Count > 0)
				{
					adressContactPersonEntities = adressContactPersonEntities.OrderBy(x => x.ID).ToList();
					foreach(var adressContactPersonEntity in adressContactPersonEntities)
					{
						responseBody.Add(new Models.Settings.SalutationContactPerson.SalutationContactPersonModel(adressContactPersonEntity));
					}
				}

				return ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.SalutationContactPerson.SalutationContactPersonModel>>.SuccessResponse();
		}
		public static bool refreshBriefanrede(
			List<Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity> currentEntities)
		{
			var newEntities = new List<Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity>();

			// - from Ansprechpartner
			List<string> data = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetBriefanreden()
				?? new List<string>();
			foreach(var item in data)
			{
				if(!currentEntities.Exists(x => x.Anrede.Trim().ToLower() == item.Trim().ToLower()))
				{
					newEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity
					{
						Anrede = item
					});
				}
			}

			// - from address
			data = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetBriefanreden()
				?? new List<string>();
			foreach(var item in data)
			{
				if(!currentEntities.Exists(x => x.Anrede.Trim().ToLower() == item.Trim().ToLower()))
				{
					newEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.Adressen_briefanredenEntity
					{
						Anrede = item
					});
				}
			}

			// - 
			if(newEntities.Count > 0)
			{
				Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Insert(newEntities.DistinctBy(x => x.Anrede)?.ToList());
				return true;
			}

			// - 
			return false;
		}
	}
}
