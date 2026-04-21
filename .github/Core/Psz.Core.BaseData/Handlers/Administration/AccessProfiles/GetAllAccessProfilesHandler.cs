using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class GetAllAccessProfilesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllAccessProfilesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var profilesEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get();
				var response = new List<Models.Administration.AccessProfiles.AccessProfileModel>();
				/// 
				var bsdLagerEntity = Enum.GetValues(typeof(Identity.Enums.BaseDataEmuns.BaseDataLager)).Cast<Identity.Enums.BaseDataEmuns.BaseDataLager>().ToList();
				var Lagers = bsdLagerEntity
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();

				if(profilesEntity != null)
				{
					foreach(var item in profilesEntity)
					{
						response.Add(new Models.Administration.AccessProfiles.AccessProfileModel(item));
					}
					foreach(var item in response)
					{
						var lagers = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(new List<int> { item.Id });
						List<KeyValuePair<int, string>> profileLagers = new List<KeyValuePair<int, string>>();
						foreach(var l in lagers)
						{
							profileLagers.Add(new KeyValuePair<int, string>(
								(int)l.LagerId,
								Lagers.FirstOrDefault(x => x.Key == l.LagerId).Value
								));
						}
						item.LagerIds = profileLagers;
					}
				}
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.SuccessResponse();
		}
	}
}
