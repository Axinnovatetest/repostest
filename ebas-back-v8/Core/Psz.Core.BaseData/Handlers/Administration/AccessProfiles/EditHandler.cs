using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AccessProfileModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model)
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

				Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Update(accessProfileEntity);
				if((this._data.EditLagerStock.HasValue && !this._data.EditLagerStock.Value)
					&& (this._data.EditLagerMinStock.HasValue && !this._data.EditLagerMinStock.Value)
					&& (this._data.EditLagerCCID.HasValue && !this._data.EditLagerCCID.Value)
					&& (this._data.EditLagerOrderProposal.HasValue && !this._data.EditLagerOrderProposal.Value))
				{
					var lagersEntity = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(new List<int> { this._data.Id });
					Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.Delete(lagersEntity.Select(x => x.Id).ToList());
				}

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

			if(Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data.AccessProfileName.Trim()}] not found.");

			var accessProfileEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.GetByName(this._data.AccessProfileName);
			if(accessProfileEntity != null && accessProfileEntity.Id != this._data.Id)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{this._data.AccessProfileName.Trim()}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
