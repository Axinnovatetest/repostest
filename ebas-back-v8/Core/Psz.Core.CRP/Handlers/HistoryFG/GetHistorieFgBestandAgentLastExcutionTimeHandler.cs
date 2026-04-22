using System;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class GetHistorieFgBestandAgentLastExecutionTimeHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private readonly UserModel _user;

		public GetHistorieFgBestandAgentLastExecutionTimeHandler(UserModel userModel)
		{
			_user = userModel ?? throw new ArgumentNullException(nameof(userModel));
		}

		public ResponseModel<string> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var date = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.GetHistorieFGAgentLastExcutionTime();

				var response = date.HasValue ? date.Value.ToString("dd/MM/yyyy HH:mm:ss") : " ";

				return ResponseModel<string>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return ResponseModel<string>.FailureResponse("An error occurred while fetching execution time.");
			}
		}

		public ResponseModel<string> Validate()
		{
			if(_user == null /* || _user.Access.____ */)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			return ResponseModel<string>.SuccessResponse(" ");
		}
	}
}
