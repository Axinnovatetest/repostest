using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class User
	{
		public static Core.Models.ResponseModel<object> UpdatePermissions(Models.User.UpdatePermissionsModel data,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| (!user.Access.WorkPlan.AccessUpdate && !user.Access.WorkPlan.AdministrationAccessProfilesUpdate))
				{
					throw new Exceptions.UnauthorizedException();
				}

				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Id);
				if(userDb == null || userDb.IsArchived)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Errors = new List<string>() { "user not found" }
					};
				}

				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateAccessProfile(data.Id, data.AccessProfileId);

				Infrastructure.Data.Access.Tables.WPL.UserHallAccess.DeleteByUserId(userDb.Id);
				foreach(var hallId in data.Halls)
				{
					Infrastructure.Data.Access.Tables.WPL.UserHallAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.UserHallEntity()
					{
						Id = -1,
						HallId = hallId,
						UserId = userDb.Id
					});
				}

				Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.DeleteByUserId(userDb.Id);
				foreach(var countryId in data.Countries)
				{
					Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity()
					{
						Id = -1,
						CountryId = countryId,
						UserId = userDb.Id
					});
				}

				return Core.Models.ResponseModel<object>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
