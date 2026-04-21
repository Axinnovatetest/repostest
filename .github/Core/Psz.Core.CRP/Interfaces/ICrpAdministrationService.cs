using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Administration.AccessProfiles;


namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpAdministrationService
	{
		ResponseModel<int> Add(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model);
		ResponseModel<int> AddToUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data);
		ResponseModel<int> AddUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data);
		ResponseModel<int> Delete(Identity.Models.UserModel user, int id);
		ResponseModel<int> EditForUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data);
		ResponseModel<int> Edit(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model);
		ResponseModel<int> EditName(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model);
		ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>> GetAllAccessProfiles(Identity.Models.UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetUsers(Identity.Models.UserModel user, List<int> ids);
		ResponseModel<int> RemoveUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data);
		ResponseModel<int> UpdateProfileHorizons(Identity.Models.UserModel user, Horizonsmodel data);
		ResponseModel<List<Models.Administration.Users.GetModel>> GetAll(Identity.Models.UserModel user);
		ResponseModel<Models.Administration.Users.GetModel> Get(Identity.Models.UserModel user, int id);
	}
}
