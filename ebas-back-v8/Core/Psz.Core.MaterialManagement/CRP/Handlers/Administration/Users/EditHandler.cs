using Psz.Core.MaterialManagement.Models.Administration.Users;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Administration.Users
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel data { get; set; }
		public EditHandler(Identity.Models.UserModel user, Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel data)
		{
			this._user = user;
			this.data = data;
		}

		public ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return Perform(this._user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}

		private ResponseModel<GetResponseModel> Perform(Identity.Models.UserModel user, GetResponseModel data)
		{
			var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Id);
			var maxNummer = Infrastructure.Data.Access.Tables.COR.UserAccess.GetMaxNummer(Module.ModuleSettings.SpecialUserNummers);

			userDb.Nummer = maxNummer + 10;
			userDb.LegacyUsername = $"wts_{userDb.Username.ToLower()}";

			Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateLegacy(userDb);

			return ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel>.SuccessResponse(new GetResponseModel(userDb, null, null, null));
		}

		public ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel>.AccessDeniedResponse();
			}

			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Id);
			if(user is null)
			{
				return ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel>.FailureResponse("User do not exist");
			}

			return ResponseModel<Psz.Core.MaterialManagement.Models.Administration.Users.GetResponseModel>.SuccessResponse();
		}
	}
}
