using Psz.Core.Common.Models;


namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<Models.Administration.Users.GetModel> Get(Identity.Models.UserModel user, int data)
		{
			try
			{
				var validationResponse = this.ValidateGet(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var userItem = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data);
				var uprofileEntities = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByUserId(new List<int> { data })
					?.FindAll(x => x.UserId == userItem.Id);
				var profileEntites = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId ?? -1)?.ToList())
					?.Where(x => x.ModuleActivated == true)
					?.Where(x => uprofileEntities?.FindIndex(y => y.AccessProfileId == x.Id) >= 0)?.ToList();

				var companyItem = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userItem?.CompanyId ?? -1);
				var departmentItem = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userItem?.DepartmentId ?? -1);

				return ResponseModel<Models.Administration.Users.GetModel>.SuccessResponse(new Models.Administration.Users.GetModel(userItem, companyItem, departmentItem, profileEntites));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<Models.Administration.Users.GetModel> ValidateGet(Identity.Models.UserModel user, int data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Administration.Users.GetModel>.AccessDeniedResponse();
			}

			// - 
			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id) == null)
				return ResponseModel<Models.Administration.Users.GetModel>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data) == null)
				return ResponseModel<Models.Administration.Users.GetModel>.FailureResponse(key: "2", value: "User not found");

			return ResponseModel<Models.Administration.Users.GetModel>.SuccessResponse();
		}
	}
}