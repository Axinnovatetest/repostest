using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class AddLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddLagerModel _data { get; set; }

		public AddLagerHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddLagerModel data)
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
				var profileEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId);
				var lagers = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.GetByAccessProfileIds(new List<int> { this._data.ProfileId });
				var newLagers = this._data.LagerIds.FindAll(x => lagers.FindIndex(y => y.LagerId == x) < 0);
				var insertedLagers = new List<Infrastructure.Data.Entities.Tables.BSD.BSD_AccessProfileLagerEntity>();
				var response = 0;

				if(newLagers != null && newLagers.Count > 0)
				{
					foreach(var item in newLagers)
					{
						insertedLagers.Add(
							new Infrastructure.Data.Entities.Tables.BSD.BSD_AccessProfileLagerEntity
							{
								Id = -1,
								AccessProfileId = this._data.ProfileId,
								AccessProfileName = profileEntity.AccessProfileName,
								LagerId = item,
							}
							);
					}
					response = Infrastructure.Data.Access.Tables.BSD.BSD_AccessProfileLagerAccess.Insert(insertedLagers);
				}

				return ResponseModel<int>.SuccessResponse(response);
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
			var profileEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId);

			if((profileEntity.EditLagerStock.HasValue && !profileEntity.EditLagerStock.Value)
				&& (!profileEntity.EditLagerMinStock)
				&& (profileEntity.EditLagerCCID.HasValue && !profileEntity.EditLagerCCID.Value)
				&& (profileEntity.EditLagerOrderProposal.HasValue && !profileEntity.EditLagerOrderProposal.Value))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Please save the changes in the access profile before hand");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
