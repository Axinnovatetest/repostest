using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<Models.Users.UserModel> Get(int id, Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
				   /* || !user.Access.Settings.Users*/)
				{
					throw new Exceptions.UnauthorizedException();
				}

				var response = Get(id);

				if(response == null)
				{
					throw new Exceptions.NotFoundException();
				}

				return Core.Models.ResponseModel<Models.Users.UserModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Users.UserModel>> Get(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
				   /*|| (!user.Access.Settings.Users && !user.Access.WorkPlan.AdministrationUser)*/)
				{
					throw new Exceptions.UnauthorizedException();
				}

				return Core.Models.ResponseModel<List<Models.Users.UserModel>>.SuccessResponse(Get());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Models.Users.UserModel Get(int id)
		{
			try
			{
				return Get(new List<int>() { id }).FirstOrDefault();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Users.UserModel> Get()
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Users.UserModel> Get(List<int> ids)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Users.UserModel> Get(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> usersDb)
		{
			try
			{
				var response = new List<Models.Users.UserModel>();

				var accessProfilesIds = usersDb.Select(e => e.AccessProfileId).ToList();
				var accessProfilesDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(accessProfilesIds);
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(usersDb?.Select(x => (long?)x.CompanyId ?? -1)?.ToList());
				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(usersDb?.Select(x => (long?)x.DepartmentId ?? -1)?.ToList());

				foreach(var userDb in usersDb)
				{
					var companyItem = companyEntities?.Find(x => x.Id == userDb.CompanyId);
					var departmentItem = departmentEntities?.Find(x => x.Id == userDb.DepartmentId);
					var profileEntity = accessProfilesDb.Find(e => e.Id == userDb.AccessProfileId);
					response.Add(new Models.Users.UserModel(userDb, profileEntity, companyItem, departmentItem));
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static List<Models.Users.UserADModel> GetADInfo(int id)
		{
			var usersAd = Core.Program.ActiveDirectoryManager.GetUsersInfo();
			if(usersAd != null && usersAd.Count > 0)
			{
				return usersAd.Select(x => new Models.Users.UserADModel
				{
					Id = -1,
					AccessProfileId = -1,
					AccessProfileName = "",
					Email = x.Email,
					Name = $"{x.FirstName} // {x.LastName}",
					Username = x.Username
				})?.ToList();
			}
			return null;
		}
	}
}
