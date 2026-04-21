using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.Bearbeiter
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				//var allusers = Infrastructure.Data.Access.Tables.MTM.PSZ_BearbeiterAccess.Get();

				return ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Models.Bearbeiter.BearbeiterResponseModel>>.SuccessResponse();
		}
	}
}
