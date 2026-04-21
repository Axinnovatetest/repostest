using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<RequestStatsModel> GetRequestsStats(UserModel user, int plantId)
		{
			if(user == null)
				return ResponseModel<RequestStatsModel>.AccessDeniedResponse();

			var byStatus = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.GetStatsByStatus(plantId);
			var byPlant = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.GetStatsByPlant(plantId);
			var byCategory = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.GetStatsByCategory(plantId);

			return ResponseModel<RequestStatsModel>.SuccessResponse(new RequestStatsModel
			{
				StatsByCategory = byCategory?.Select(x => new StatsModel(x)).ToList(),
				StatsByPlant = byPlant?.Select(x => new StatsModel(x)).ToList(),
				StatsByStatus = byStatus?.Select(x => new StatsModel(x)).ToList()
			});
		}
	}
}
