using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Handlers.Administration.AccessProfiles
{
	public class RemoveUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddUsersRequestModel _data { get; set; }

		public RemoveUsersHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersRequestModel data)
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
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.DeleteUsers(this._data.ProfileId, this._data.UserIds?.Select(x => x ?? -1)?.ToList()));
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

			if(Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
