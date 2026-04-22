using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Services
{
	public interface ICapitalRequestsService
	{
		ResponseModel<IEnumerable<RequestHeaderModel>> GetRequests(UserModel user);
		ResponseModel<CapitalRequestModel> GetRequestById(UserModel user, int id);
		ResponseModel<int> AddRequest(UserModel user, CapitalRequestModel request);
		ResponseModel<int> UpdateRequest(UserModel user, CapitalRequestModel request);
		ResponseModel<IEnumerable<KeyValuePair<int, string>>> GetRequestCategories(UserModel user);
		ResponseModel<string> GetFAArtikelnummer(UserModel user, int fa);
		ResponseModel<List<CapitalRequestsLogsResponseModel>> GetRequestsLogs(UserModel user, CapitalRequestsLogsRequestModel data);
		ResponseModel<RequestStatsModel> GetRequestsStats(UserModel user, int plantId);
		ResponseModel<int> UnvalidatePositionCapital(UserModel user, int positionId);
		ResponseModel<int> UnvalidatePositionEngeneering(UserModel user, int positionId);
		ResponseModel<List<KeyValuePair<int, string>>> GetPlants(UserModel user);
	}
}
