using NLog.Targets;
using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> RemoveUserFromTeam(UserModel user, UserTeamRemoveModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			var userTeam = Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.GetByUserIdAndTeam(data.Id, data.Team);
			return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Delete(userTeam?.Id ?? -1));
		}
	}
}