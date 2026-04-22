using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Main.Handlers
{
	public partial class Users
	{
		public static Models.UserModel Get(int id, bool includeAccess)
		{
			try
			{
				return Get(new List<int>() { id }, includeAccess).FirstOrDefault();
				;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//public static List<Models.UserModel> Get(bool includeAccess)
		//{
		//    try
		//    {
		//        return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(), includeAccess);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		public static List<Models.UserModel> Get(List<int> ids, bool includeAccess)
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(ids), includeAccess);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static List<Models.UserModel> Get(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> usersDb,
			bool includeAccess)
		{
			try
			{
				if(usersDb == null || usersDb.Count == 0)
				{
					return new List<Models.UserModel>();
				}

				var response = new List<Models.UserModel>();
				foreach(var userDb in usersDb)
				{
					var user = new Models.UserModel();

					user.Id = userDb.Id;
					user.Username = userDb.Username;
					user.CreationTime = userDb.CreationTime;
					user.Name = userDb.Name;
					user.SelectedLanguage = userDb.SelectedLanguage;

					if(includeAccess)
					{
						//user.Access = Core.Identity.Handlers.AccessProfile.Get(userDb.AccessProfileId);
						user.Access = Core.Identity.Handlers.AccessProfile.GetUserProfiles(user.Id);
					}
					else
					{
						user.Access = null;
					}

					response.Add(user);
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
