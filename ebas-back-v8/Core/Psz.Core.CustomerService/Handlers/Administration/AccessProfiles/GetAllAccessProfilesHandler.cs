using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Administration.AccessProfiles
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
				var profilesEntity = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Get();
				var response = new List<Models.Administration.AccessProfiles.AccessProfileModel>();


				if(profilesEntity != null)
				{
					foreach(var item in profilesEntity)
					{
						response.Add(new Models.Administration.AccessProfiles.AccessProfileModel(item));
					}

				}
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
