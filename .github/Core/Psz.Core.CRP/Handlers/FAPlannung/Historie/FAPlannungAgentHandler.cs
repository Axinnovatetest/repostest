using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning.Historie;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<int> FAPlannungHistorieRefreshData(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var lastForcedExecution = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.GetHistorieFaPlannungAgentLastExcutionTime(true);
			var executionThreshold = Module.CRPAgentsRefreshThreshold;
			if(lastForcedExecution is not null)
			{
				if(DateTime.Now < lastForcedExecution.Value.AddHours(executionThreshold))
					return ResponseModel<int>.FailureResponse($"Max Data refresh reached, try again in {GetDateDifference(DateTime.Now, lastForcedExecution.Value.AddHours(executionThreshold))} .");
			}

			try
			{
				var response = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.FaPlannungHistorieRefreshData(user.Id, user.Name,
					(int)Enums.CRPEnums.FaPlannungHistorieImportType.ForcedAgent, Enums.CRPEnums.FaPlannungHistorieImportType.ForcedAgent.GetDescription());
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<string> GetHistorieFaPlannungAgentLastExcutionTime(UserModel user)
		{
			if(user == null)
				return ResponseModel<string>.AccessDeniedResponse();

			try
			{
				var date = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.GetHistorieFaPlannungAgentLastExcutionTime();
				var response = date is not null
					? date.Value.ToString("dd/MM/yyyy HH:mm:ss")
					: "";
				return ResponseModel<string>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<FAPlannungHistorieAgentLogModel>> GetFaPlannungHistorieAgentFullLog(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<FAPlannungHistorieAgentLogModel>>.AccessDeniedResponse();

			try
			{
				var headers = Infrastructure.Data.Access.Tables.CRP.__crp_historie_fa_plannung_headerAccess.Get();
				var response = headers?.Select(h => new FAPlannungHistorieAgentLogModel(h)).ToList();

				return ResponseModel<List<FAPlannungHistorieAgentLogModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static string GetDateDifference(DateTime startDate, DateTime endDate)
		{
			// Calculate the difference between the two dates
			TimeSpan difference = endDate - startDate;

			// Extract days, hours, and minutes from the TimeSpan
			int days = difference.Days;
			int hours = difference.Hours;
			int minutes = difference.Minutes;

			// Build the result string based on the values
			string result = "";

			if(days > 0)
			{
				result += $"{days} day{(days > 1 ? "s" : "")}, ";
			}

			if(hours > 0)
			{
				result += $"{hours} hour{(hours > 1 ? "s" : "")}, ";
			}

			if(minutes > 0 || (days == 0 && hours == 0))
			{
				result += $"{minutes} minute{(minutes > 1 ? "s" : "")}";
			}

			// Remove trailing comma and space if present
			result = result.TrimEnd(',', ' ');
			return result;
		}
	}
}