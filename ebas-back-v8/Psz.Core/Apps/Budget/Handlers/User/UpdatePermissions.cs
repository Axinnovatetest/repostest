using System;
using System.Collections.Generic;


namespace Psz.Core.Apps.Budget.Handlers
{
	public partial class User
	{
		public static Psz.Core.Models.ResponseModel<object> UpdatePermissions(Psz.Core.Apps.Budget.Models.User.UpdatePermissionsModel data,
		   Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| (!user.Access.Financial.Budget.AdministrationUserUpdate && !user.Access.Financial.Budget.AdministrationAccessProfilesUpdate))
				{
					throw new Exceptions.UnauthorizedException();
				}

				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Id);
				if(userDb == null || userDb.IsArchived)
				{
					return new Psz.Core.Models.ResponseModel<object>()
					{
						Errors = new List<string>() { "user not found" }
					};
				}

				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateEmail(new Infrastructure.Data.Entities.Tables.COR.UserEntity
				{
					Id = data.Id,
					Email = data.Email,
					LastEditDate = DateTime.Now,
					LastEditUserId = user.Id
				});
				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateAccessProfile(data.Id, data.AccessProfileId);

				Infrastructure.Data.Access.Tables.FNC.Assign_User_JointAccess.DeleteByUserId(userDb.Id);
				foreach(var userAssignId in data.UsersAssign)
				{
					Infrastructure.Data.Access.Tables.FNC.Assign_User_JointAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Assign_User_JointEntity()
					{
						ID = -1,
						ID_AssignUser = userAssignId,
						ID_user = userDb.Id
					});
				}

				Infrastructure.Data.Access.Tables.FNC.Departement_User_JointAccess.DeleteByUserId(userDb.Id);
				foreach(var deptId in data.Depts)
				{
					Infrastructure.Data.Access.Tables.FNC.Departement_User_JointAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Departement_User_JointEntity()
					{
						ID = -1,
						ID_Departement = deptId,
						ID_user = userDb.Id
					});
				}

				Infrastructure.Data.Access.Tables.FNC.Land_User_JointAccess.DeleteByUserId(userDb.Id);
				foreach(var landId in data.Lands)
				{
					Infrastructure.Data.Access.Tables.FNC.Land_User_JointAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Land_User_JointEntity()
					{
						ID = -1,
						ID_land = landId,
						ID_user = userDb.Id
					});
				}

				return Psz.Core.Models.ResponseModel<object>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
