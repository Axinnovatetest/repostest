using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<Models.GetModel> ValidateGet(UserModel user)
		{
			if(user == null)
				return ResponseModel<Models.GetModel>.AccessDeniedResponse();
			return ResponseModel<Models.GetModel>.SuccessResponse();
		}
		public ResponseModel<Models.GetModel> Get(UserModel user, int data)
		{
			try
			{
				var validationResponse = ValidateGet(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userItem = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data);
				var uprofileEntities = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByUserId(new List<int> { data })
					?.FindAll(x => x.UserId == userItem.Id);
				var profileEntites = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId)?.ToList())
					?.Where(x => x.ModuleActivated == true)
					?.Where(x => uprofileEntities?.FindIndex(y => y.AccessProfileId == x.Id) >= 0)?.ToList();

				var companyItem = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userItem?.CompanyId ?? -1);
				var departmentItem = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userItem?.DepartmentId ?? -1);

				return ResponseModel<Models.GetModel>.SuccessResponse(new Models.GetModel(userItem, companyItem, departmentItem, profileEntites));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}