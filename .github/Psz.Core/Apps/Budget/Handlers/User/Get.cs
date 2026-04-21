using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Budget.Handlers
{
	public partial class User
	{

		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }


		public static Core.Models.ResponseModel<Models.User.UserModel> Get(int id,
	 Core.Identity.Models.UserModel user)
		{
			try
			{
				/*if (user == null
                    || !user.Access.Budget.Administration)
                {
                    throw new Exceptions.UnauthorizedException();
                }*/

				return Get(id);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		//public static Psz.Core.FinanceControl.Models.ResponseModel<Models.User.UserModel> Get(int id,
		//    Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || !user.Access.Budget.Administration)
		//        {
		//            throw new Exceptions.UnauthorizedException();
		//        }

		//        return Get(id);
		//    }
		//    catch (Exception e)
		//    {
		//       // Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}


		internal static Psz.Core.Models.ResponseModel<Models.User.UserModel> Get(int id)
		{
			try
			{
				return Psz.Core.Models.ResponseModel<Models.User.UserModel>
					.SuccessResponse(Get(new List<int>() { id }).Body.FirstOrDefault());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static Psz.Core.Models.ResponseModel<List<Models.User.UserModel>> Get(Core.Identity.Models.UserModel user)
		{
			try
			{
				//if (user == null
				// || (!user.Access.Budget.Administration && !user.Access.Budget.AdministrationUser && !user.Access.Budget.AdministrationAccessProfiles))
				if(user == null
				 || (!user.Access.Financial.Budget.Assign && !user.Access.Financial.Budget.AssignCreateUser && !user.Access.Financial.Budget.AssignEditUser))
				{
					throw new Exceptions.UnauthorizedException();
				}

				return Get(Infrastructure.Data.Access.Tables.COR.UserAccess.Get());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//public static Psz.Core.Models.ResponseModel<List<Models.User.UserModel>> GetCurrentToAssign(/*Core.Identity.Models.UserModel user,*/int id)
		public static Psz.Core.Models.ResponseModel<List<Models.User.UserModel>> GetCurrentToAssign(int id)
		{
			try
			{
				//if (user == null
				// || (!user.Access.Budget.Administration && !user.Access.Budget.AdministrationUser && !user.Access.Budget.AdministrationAccessProfiles))
				/* if (user == null
                    || (!user.Access.Budget.Assign && !user.Access.Budget.AssignCreateUser && !user.Access.Budget.AssignEditUser))
                   {
                       throw new Exceptions.UnauthorizedException();
                   }*/

				return Get(new List<Infrastructure.Data.Entities.Tables.COR.UserEntity> { Infrastructure.Data.Access.Tables.COR.UserAccess.Get(id) });
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		internal static Psz.Core.Models.ResponseModel<List<Models.User.UserModel>> Get()
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

		internal static Psz.Core.Models.ResponseModel<List<Models.User.UserModel>> Get(List<int> ids)
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
		internal static Core.Models.ResponseModel<List<Models.User.UserModel>> Get(List<Infrastructure.Data.Entities.Tables.COR.UserEntity> usersDb)
		{
			try
			{
				var response = new List<Models.User.UserModel>();

				var accessProfilesIds = usersDb.Select(e => e.AccessProfileId).ToList();
				var accessProfilesDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(accessProfilesIds);

				var usersIds = usersDb.Select(e => e.Id).ToList();

				var deptsDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(usersDb?.Select(x => (long?)x.DepartmentId ?? -1)?.ToList());
				var landsDb = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(usersDb?.Select(x => (long)x.CompanyId)?.ToList()) ?? new List<Infrastructure.Data.Entities.Tables.STG.CompanyEntity>();

				var usersAssignDb = Infrastructure.Data.Access.Tables.FNC.Assign_User_JointAccess.GetByUsersIds(usersIds);
				var usersAssignIds = usersAssignDb.Select(e => e.ID_AssignUser)?.Distinct()?.ToList() ?? new List<int>();
				var userAssignDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(usersAssignIds) ?? new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				foreach(var userDb in usersDb)
				{
					var lands = landsDb
						.Where(e => e.Id == userDb.CompanyId)
						.Select(x => new KeyValuePair<int, string>(x.Id, x.Name))
						.ToList();

					var depts = deptsDb?
						.Where(e => e.Id == userDb.DepartmentId)?
						.Select(e => new KeyValuePair<int, string>((int)e.Id, e.Name))?
						.ToList();

					var usersAssign = new List<KeyValuePair<int, string>>();
					var userAssignsIds = usersAssignDb?
						.Where(e => e.ID_user == userDb.Id)?
						.Select(e => e.ID_AssignUser)?
						.ToList();
					foreach(var userassignDb in userAssignDb.FindAll(e => userAssignsIds.Contains(e.Id)))
					{
						usersAssign.Add(new KeyValuePair<int, string>(userassignDb.Id, userassignDb.Username));
					}


					response.Add(new Models.User.UserModel()
					{
						Id = userDb.Id,
						AccessProfileId = userDb.AccessProfileId,
						AccessProfileName = accessProfilesDb.Find(e => e.Id == userDb.AccessProfileId)?.Name,
						Name = userDb.Name,
						Username = userDb.Username,
						Lands = lands,
						Depts = depts,
						UsersAssign = usersAssign ?? new List<KeyValuePair<int, string>>(),
						Email = userDb.Email
					});
				}

				return Core.Models.ResponseModel<List<Models.User.UserModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
