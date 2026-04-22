using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetPlants(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();

			var plants = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
			var response = plants?.Select(x => new KeyValuePair<int, string>(x.Id, x.Name)).ToList();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);

		}
	}
}