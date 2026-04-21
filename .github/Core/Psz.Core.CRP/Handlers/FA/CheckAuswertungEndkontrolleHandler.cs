using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class CheckAuswertungEndkontrolleHandler: IHandle<Identity.Models.UserModel, ResponseModel<string>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public CheckAuswertungEndkontrolleHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var auswetungEndkontrolleEntity = Infrastructure.Data.Access.Joins.FAPlannung.FAPlannungAccess.GetAuswertingEndkontrolle();
				if(auswetungEndkontrolleEntity == null || auswetungEndkontrolleEntity.Count == 0)
					return ResponseModel<string>.FailureResponse("Data Empty");

				return ResponseModel<string>.SuccessResponse("");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}
			return ResponseModel<string>.SuccessResponse("");
		}
	}
}