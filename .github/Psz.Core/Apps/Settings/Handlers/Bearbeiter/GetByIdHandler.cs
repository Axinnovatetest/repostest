using Psz.Core.Apps.Settings.Models.Bearbeiter;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.Settings.Handlers.Bearbeiter
{
	public class GetByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Bearbeiter.BearbeiterResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int data { get; set; }
		public GetByIdHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this.data = data;
		}

		public ResponseModel<Models.Bearbeiter.BearbeiterResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				///
				var user = Infrastructure.Data.Access.Tables.MTM.PSZ_BearbeiterAccess.Get(data);

				return ResponseModel<Models.Bearbeiter.BearbeiterResponseModel>.SuccessResponse(new BearbeiterResponseModel(user));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Bearbeiter.BearbeiterResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Bearbeiter.BearbeiterResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<Models.Bearbeiter.BearbeiterResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<Models.Bearbeiter.BearbeiterResponseModel>.SuccessResponse();
		}
	}
}
