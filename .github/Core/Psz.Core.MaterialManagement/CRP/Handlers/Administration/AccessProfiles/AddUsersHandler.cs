using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Handlers.Administration.AccessProfiles
{
	public class AddUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddUsersRequestModel _data { get; set; }

		public AddUsersHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersRequestModel data)
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
				var profileEntity = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get(this._data.ProfileId);
				var profileUsers = Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.GetByAccessProfileIds(new List<int> { this._data.ProfileId });
				var newUsers = this._data.UserIds.FindAll(x => profileUsers.FindIndex(y => y.UserId == x) < 0)?.Select(x => x ?? -1)?.ToList();
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(newUsers);
					if(userEntities != null && userEntities.Count > 0)
					{
						insertedUsers = Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.Insert(
							userEntities.Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileUsersEntity
							{
								AccessProfileId = profileEntity.Id,
								AccessProfileName = profileEntity.AccessProfileName,
								CreationTime = DateTime.Now,
								CreationUserId = this._user.Id,
								Id = -1,
								UserEmail = x.Email,
								UserId = x.Id,
								UserName = x.Username
							})?.ToList());
					}
				}

				return ResponseModel<int>.SuccessResponse(insertedUsers);
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

			if(Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get(this._data.ProfileId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
