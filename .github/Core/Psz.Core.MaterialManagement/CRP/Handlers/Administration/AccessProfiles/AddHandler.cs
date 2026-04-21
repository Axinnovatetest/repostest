using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Handlers.Administration.AccessProfiles
{

	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AccessProfileAddRequestModel _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileAddRequestModel model)
		{
			this._user = user;
			this._data = model;
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
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var accessProfileEntity = this._data.ToEntity();
				accessProfileEntity.ModuleActivated = true;
				accessProfileEntity.CreationTime = DateTime.Now;
				accessProfileEntity.CreationUserId = this._user.Id;
				Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Insert(accessProfileEntity);
				return ResponseModel<int>.SuccessResponse();
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

			if(Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.GetByName(this._data.AccessProfileName) != null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data.AccessProfileName}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}

