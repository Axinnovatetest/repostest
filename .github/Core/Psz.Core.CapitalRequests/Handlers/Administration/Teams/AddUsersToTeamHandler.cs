using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> AddUsersToTeam(UserModel user, TeamUsersModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			if(data.Ids == null)
				return ResponseModel<int>.FailureResponse("no users to add.");

			try
			{
				var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Ids);
				Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Insert(users?.Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_teamsEntity
				{
					AddTime = DateTime.Now,
					AddUser = user.Id,
					PlantId = x.CompanyId,
					Team = data.Team,
					UserId = x.Id,
					Username = x.Name
				}).ToList());
				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}