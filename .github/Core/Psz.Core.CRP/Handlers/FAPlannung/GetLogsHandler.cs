using System;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	public partial class CrpFAPlannung
	{
		public ResponseModel<List<Models.FAPlanning.FaPlanningLogRequestModel>> ValidateGetLogs(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<Models.FAPlanning.FaPlanningLogRequestModel>>.AccessDeniedResponse();
			return ResponseModel<List<Models.FAPlanning.FaPlanningLogRequestModel>>.SuccessResponse();
		}
		public ResponseModel<List<Models.FAPlanning.FaPlanningLogRequestModel>> GetLogs(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetLogs(user);
				if(!validationResponse.Success)
					return validationResponse;

				var logs = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetLogs();
				var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(logs?.Select(x=>x.ExecUserId ?? -1)?.ToList());

				return ResponseModel<List<Models.FAPlanning.FaPlanningLogRequestModel>>.SuccessResponse(logs?.Select(x=> new Models.FAPlanning.FaPlanningLogRequestModel(x, users.FirstOrDefault(y=>y.Id==x.ExecUserId)?.Username ?? ""))?.ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		// - 
		public ResponseModel<Models.FAPlanning.FaPlanningLogRequestModel> ValidateGetLastLog(UserModel user)
		{
			if(user == null)
				return ResponseModel<Models.FAPlanning.FaPlanningLogRequestModel>.AccessDeniedResponse();
			return ResponseModel<Models.FAPlanning.FaPlanningLogRequestModel>.SuccessResponse();
		}
		public ResponseModel<Models.FAPlanning.FaPlanningLogRequestModel> GetLastLog(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetLastLog(user);
				if(!validationResponse.Success)
					return validationResponse;

				var log = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetLastLog();
				var logUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(log.ExecUserId ?? -1);

				return ResponseModel<Models.FAPlanning.FaPlanningLogRequestModel>.SuccessResponse(new Models.FAPlanning.FaPlanningLogRequestModel(log, logUser?.Username ?? ""));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
