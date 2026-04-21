using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<int, string>>> GetByAccessProfiles(List<int> accessProfilesIds,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				#region > Access Verification
				if(user == null
					|| user.Access == null
						//|| (!user.Access.Settings.UsersCreate && !user.Access.Settings.UsersUpdate 
						//    && !user.Access.WorkPlan.AdministrationUser && !user.Access.WorkPlan.SuperAdministrator  
						//    && !user.Access.Settings.SuperAdministrator)
						)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion

				var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(accessProfilesIds) ?? new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();

				var responseBody = new List<KeyValuePair<int, string>>();
				foreach(var userDb in usersDb)
				{
					responseBody.Add(new KeyValuePair<int, string>(userDb.Id, userDb.Username));
				}

				return Core.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
