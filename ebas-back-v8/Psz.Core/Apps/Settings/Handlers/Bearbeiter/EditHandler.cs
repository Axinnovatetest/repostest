using Psz.Core.Apps.Settings.Models.Bearbeiter;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.Apps.Settings.Handlers.Bearbeiter
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Bearbeiter.BearbeiterEditResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Bearbeiter.BearbeiterEditRequestModel data { get; set; }
		public EditHandler(Identity.Models.UserModel user, BearbeiterEditRequestModel data)
		{
			this._user = user;
			this.data = data;
		}
		public ResponseModel<BearbeiterEditResponseModel> Handle()
		{

			var entity = data.ToEntity();
			if(entity == null)
			{
				return ResponseModel<Models.Bearbeiter.BearbeiterEditResponseModel>.FailureResponse(key: "1", value: "Bearbeiter not found");
			}

			Infrastructure.Data.Access.Tables.MTM.PSZ_BearbeiterAccess.Update(entity);
			return ResponseModel<BearbeiterEditResponseModel>.SuccessResponse(new BearbeiterEditResponseModel() { Success = true });
		}

		public ResponseModel<BearbeiterEditResponseModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<Models.Bearbeiter.BearbeiterEditResponseModel>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<Models.Bearbeiter.BearbeiterEditResponseModel>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<Models.Bearbeiter.BearbeiterEditResponseModel>.SuccessResponse();
		}
	}
}
