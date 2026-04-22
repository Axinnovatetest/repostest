using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFaUserEmailsHandler: IHandle<UserModel, ResponseModel<List<FaUserEmailResponseModel>>>
	{
		protected UserModel _user;
		public GetFaUserEmailsHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<FaUserEmailResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				var allFaUsersEmails = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess.Get().ToList();

				if(allFaUsersEmails.Count > 0)
				{
					return ResponseModel<List<FaUserEmailResponseModel>>.SuccessResponse(allFaUsersEmails.Select(x => new FaUserEmailResponseModel(x)).ToList());
				}
				return ResponseModel<List<FaUserEmailResponseModel>>.SuccessResponse(new List<FaUserEmailResponseModel>());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<FaUserEmailResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<FaUserEmailResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<FaUserEmailResponseModel>>.SuccessResponse();
		}
	}
}
