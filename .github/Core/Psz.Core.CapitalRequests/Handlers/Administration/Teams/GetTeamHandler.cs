using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetTeam(UserModel user, string team)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var entites = Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.Get();
				var response = entites?.Where(e => e.Team == team)
					.Select(x => new KeyValuePair<int, string>(x.UserId ?? -1, x.Username)).ToList();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}