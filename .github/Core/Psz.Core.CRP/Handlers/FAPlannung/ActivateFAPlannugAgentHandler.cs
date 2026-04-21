using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<int> ActivateFAPlannugAgent(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			var blocs = Core.Common.Helpers.blockHelper.GetBlockState();
			if(blocs.FA_Plannug_Agents)
				return ResponseModel<int>.FailureResponse("The FA Plannung automatic agent is already executing, please try again in few seconds.");
			try
			{
				var H1StartWeek = Helpers.HorizonsHelper.GetIsoWeekNumber(DateTime.Now);
				var endDate = DateTime.Today.AddMonths(4);

				var response = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.FaPlanningRefreshData(endDate, user.Id);
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.FA_Plannug_Agents, false);
				throw;
			}
		}
	}
}