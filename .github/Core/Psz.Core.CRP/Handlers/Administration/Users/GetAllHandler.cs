using Psz.Core.Common.Models;


namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<List<Models.Administration.Users.GetModel>> GetAll(Identity.Models.UserModel user)
		{
			try
			{
				var validationResponse = this.ValidateGetAll(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetActive();
				var uprofileEntities = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByUserId(userEntities?.Select(x => x.Id)?.ToList());
				var profileEntites = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(uprofileEntities?.Select(x => x.AccessProfileId ?? -1)?.ToList())
					?.Where(x => x.ModuleActivated == true)?.ToList();
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get();

				var responseBody = new List<Models.Administration.Users.GetModel> { };
				foreach(var userItem in userEntities)
				{
					var uprofileItems = uprofileEntities.FindAll(x => x.UserId == userItem.Id);
					var profileItems = profileEntites.FindAll(x => uprofileItems?.FindIndex(y => y.AccessProfileId == x.Id) >= 0);
					var companyItem = companyEntities.Find(x => x.Id == userItem.CompanyId);
					var departmentItem = departmentEntities.Find(x => x.Id == userItem.DepartmentId);
					responseBody.Add(new Models.Administration.Users.GetModel(userItem, companyItem, departmentItem, profileItems));
				}

				return ResponseModel<List<Models.Administration.Users.GetModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<Models.Administration.Users.GetModel>> ValidateGetAll(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.Users.GetModel>>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<List<Models.Administration.Users.GetModel>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<Models.Administration.Users.GetModel>>.SuccessResponse();
		}
	}
}