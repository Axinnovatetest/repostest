using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<List<Models.GetModel>> ValidateGetAll(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<Models.GetModel>>.AccessDeniedResponse();
			return ResponseModel<List<Models.GetModel>>.SuccessResponse();
		}
		public ResponseModel<List<Models.GetModel>> GetAll(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetAll(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetActive();
				var uprofileEntities = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByUserId(userEntities?.Select(x => x.Id)?.ToList());
				var profileEntites = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId)?.ToList())
					?.Where(x => x.ModuleActivated == true)?.ToList();
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();

				var responseBody = new List<Models.GetModel> { };
				foreach(var userItem in userEntities)
				{
					var uprofileItems = uprofileEntities.FindAll(x => x.UserId == userItem.Id);
					var profileItems = profileEntites.FindAll(x => uprofileItems?.FindIndex(y => y.AccessProfileId == x.Id) >= 0);
					var companyItem = companyEntities.Find(x => x.Id == userItem.CompanyId);
					var departmentItem = departmentEntities.Find(x => x.Id == userItem.DepartmentId);
					responseBody.Add(new Models.GetModel(userItem, companyItem, departmentItem, profileItems));
				}

				return ResponseModel<List<Models.GetModel>>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}