using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.Apps.Settings.Handlers
{
	public class DeleteProfileHandler: IHandle<Models.AccessProfiles.AccessProfileModel, ResponseModel<int>>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreationUser { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteProfileHandler(int id, Identity.Models.UserModel user)
		{
			this.Id = id;
			this._user = user;
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

				var AccessProfile_entity = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.Get(Id);
				if(AccessProfile_entity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.Delete(AccessProfile_entity.Id));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AdministrationAccessProfilesUpdate)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
