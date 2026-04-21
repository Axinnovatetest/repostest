using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.Administration.Handlers.AccessProfiles
{
	public class GetAllAccessProfilesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllAccessProfilesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var profilesEntity = Infrastructure.Data.Access.Tables.MGO.AccessProfileAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity>();
				var response = new List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>();

				foreach(var item in profilesEntity)
				{
					response.Add(new Administration.Models.AccessProfiles.AccessProfileAddRequestModel(item));
				}
				return ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<Administration.Models.AccessProfiles.AccessProfileAddRequestModel>>.SuccessResponse();
		}
	}
}
