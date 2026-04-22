using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Services
{
	public interface ICapitalRequestsAdminstrationService
	{
		ResponseModel<int> Add(UserModel user, Models.AccessProfileModel data);
		ResponseModel<int> AddToUser(UserModel user, Models.AddToUserModel data);
		ResponseModel<int> AddUsers(UserModel user, Models.AddUsersModel data);
		ResponseModel<int> Delete(UserModel user, int data);
		ResponseModel<int> EditForUser(UserModel user, Models.AddToUserModel data);
		ResponseModel<int> Edit(UserModel user, Models.AccessProfileModel data);
		ResponseModel<int> EditName(UserModel user, Models.AccessProfileModel data);
		ResponseModel<List<Models.AccessProfileModel>> GetAllAccessProfiles(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetUsers(UserModel user, List<int> data);
		ResponseModel<int> RemoveUsers(UserModel user, Models.AddUsersModel data);
		ResponseModel<List<Models.GetModel>> GetAll(UserModel user);
		ResponseModel<Models.GetModel> Get(UserModel user, int data);
		//Teams
		ResponseModel<List<KeyValuePair<int, string>>> GetTeam(UserModel user, string team);
		ResponseModel<int> AddUsersToTeam(UserModel user, TeamUsersModel data);
		ResponseModel<int> RemoveUserFromTeam(UserModel user, UserTeamRemoveModel data);
	}
}