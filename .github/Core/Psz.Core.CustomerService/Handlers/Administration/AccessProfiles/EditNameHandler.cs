using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Administration.AccessProfiles
{
	public class EditNameHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AccessProfileModel _data { get; set; }

		public EditNameHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model)
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
				//accessProfileEntity.LastEditTime = DateTime.Now;
				//accessProfileEntity.LastEditUserId = this._user.Id;

				Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.UpdateName(accessProfileEntity);

				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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

			if(Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data.AccessProfileName.Trim()}] not found.");

			var accessProfileEntity = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetByName(this._data.AccessProfileName);
			if(accessProfileEntity != null && accessProfileEntity.Id != this._data.Id)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data.AccessProfileName.Trim()}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
