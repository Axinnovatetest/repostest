using System;

namespace Psz.Core.ManagementOverview.Administration.Handlers.AccessProfiles
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel model)
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
				var accessProfileEntity = new Infrastructure.Data.Entities.Tables.MGO.AccessProfileEntity
				{
					AccessProfileName = this._data.AccessProfileName,
					CreationUserId = this._user.Id,
					CreationTime = DateTime.Now,
					ModuleActivated = true
				};
				Infrastructure.Data.Access.Tables.MGO.AccessProfileAccess.Insert(accessProfileEntity);

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

			if(Infrastructure.Data.Access.Tables.MGO.AccessProfileAccess.GetByName(this._data.AccessProfileName) != null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
