using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Handlers.Administration.AccessProfiles
{
	public class GetAllAccessProfilesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllAccessProfilesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var profilesEntity = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.MTM.AccessProfileEntity>();
				var response = new List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>();

				foreach(var item in profilesEntity)
				{
					response.Add(new Models.Administration.AccessProfiles.AccessProfileAddRequestModel(item));
				}
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>.SuccessResponse();
		}
	}
}
