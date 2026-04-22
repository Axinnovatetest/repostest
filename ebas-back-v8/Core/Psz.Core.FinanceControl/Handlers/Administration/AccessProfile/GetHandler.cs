using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Administration.AccessProfile
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get()
						?.Select(x => new Models.Administration.AccessProfile.AccessProfileModel(x))
						?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Models.Administration.AccessProfile.AccessProfileModel>>.SuccessResponse();
		}
	}
}
