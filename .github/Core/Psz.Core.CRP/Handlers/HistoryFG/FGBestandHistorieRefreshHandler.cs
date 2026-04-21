using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class FGBestandHistorieRefreshHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user;

		public FGBestandHistorieRefreshHandler(UserModel userModel)
		{
			this._user = userModel;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var date = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.GetHistorieFGForcedAgentLastExcutionTime();

				// Vérification si la date est non null avant d'accéder à .Value
				if(date.HasValue)
				{
					TimeSpan difference = DateTime.Now - date.Value;

					if(difference.TotalHours < Module.CRPAgentsRefreshThreshold)
					{
						return ResponseModel<int>.FailureResponse("Please retry later, you already refreshed it recently.");
					}
				}

				var response = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryHeaderFGBestandAccess.FGHistorieRefreshData(_user.Id, _user.Name,
					 (int)Enums.CRPEnums.HistoryFGImportTypes.ByUserForcingAgent);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				new ResponseModel<int>.ResponseError("An error occurred while processing the request.");
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null /* || _user.Access.____ */)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse(default);
		}
	}
}
