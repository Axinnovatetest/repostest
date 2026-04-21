using System;

namespace Psz.Core.FinanceControl.Handlers.Administration.AccessProfile
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class RemoveUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfile.AddUsersModel _data { get; set; }

		public RemoveUsersHandler(Identity.Models.UserModel user, Models.Administration.AccessProfile.AddUsersModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.DeleteUsers(this._data.ProfileId, this._data.UserIds));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(this._data.ProfileId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
