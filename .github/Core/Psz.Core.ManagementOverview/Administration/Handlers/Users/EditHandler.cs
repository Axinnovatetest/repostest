using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.Administration.Models.Users;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.ManagementOverview.Administration.Handlers.Users
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel>>
	{

		private Identity.Models.UserModel _user { get; set; }
		private Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel data { get; set; }
		public EditHandler(Identity.Models.UserModel user, Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel data)
		{
			this._user = user;
			this.data = data;
		}

		public ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel> Handle()
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
			if(userDb is null)
			{
				return ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel>.FailureResponse("User Not found");
			}

			//userDb.Nummer = data.Nummer;
			//userDb.LegacyUsername = data.LegacyUsername;

			Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userDb);

			return ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel>.SuccessResponse();
		}

		public ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Psz.Core.ManagementOverview.Administration.Models.Users.GetResponseModel>.SuccessResponse();
		}
	}
}
