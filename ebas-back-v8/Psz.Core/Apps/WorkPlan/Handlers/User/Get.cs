using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class User
	{
		//public static Core.Models.ResponseModel<Models.User.UserModel> Get(int id, 
		//    Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || !user.Access.WorkPlan.Access)
		//        {
		//            throw new Exceptions.UnauthorizedException();
		//        }

		//        var requestedUserResponse = Apps.Settings.Handlers.Users.Get(id);
		//        if (!requestedUserResponse.Success)
		//        {
		//            return new Core.Models.ResponseModel<Models.User.UserModel>()
		//            {
		//                Errors = requestedUserResponse.Errors
		//            };
		//        }


		//        return ;
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		//public static Core.Models.ResponseModel<List<Models.User.UserModel>> GetUsers(Core.Identity.Models.UserModel user)
		//{
		//    try
		//    {
		//        if (user == null
		//            || !user.Access.WorkPlan.Access)
		//        {
		//            throw new Exceptions.UnauthorizedException();
		//        }

		//        return Apps.Settings.Handlers.Users.Get();
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}

		public static Core.Models.ResponseModel<Models.User.UserModel> Get(int id,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| !user.Access.WorkPlan.Access)
				{
					throw new Exceptions.UnauthorizedException();
				}

				return Get(id);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static Core.Models.ResponseModel<Models.User.UserModel> Get(int id)
		{
			try
			{
				return Core.Models.ResponseModel<Models.User.UserModel>
					.SuccessResponse(Get(new List<int>() { id }).Body.FirstOrDefault());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static Core.Models.ResponseModel<List<Models.User.UserModel>> Get(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
				   || (!user.Access.WorkPlan.Access && !user.Access.WorkPlan.AdministrationUser && !user.Access.WorkPlan.AdministrationAccessProfiles))
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
		internal static Core.Models.ResponseModel<List<Models.User.UserModel>> Get()
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

		internal static Core.Models.ResponseModel<List<Models.User.UserModel>> Get(List<int> ids)
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

				var usersHallsDb = Infrastructure.Data.Access.Tables.WPL.UserHallAccess.GetByUsersIds(usersIds);
				var hallsIds = usersHallsDb.Select(e => e.HallId).Distinct().ToList();
				var hallsDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(hallsIds);

				var usersCountriesDb = Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.GetByUsersIds(usersIds);
				var countriesIds = usersCountriesDb.Select(e => e.CountryId).Distinct().ToList();
				var countriesDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(countriesIds);

				foreach(var userDb in usersDb)
				{
					var countries = new List<KeyValuePair<int, string>>();
					var userCountriesIds = usersCountriesDb
						.Where(e => e.UserId == userDb.Id)
						.Select(e => e.CountryId)
						.ToList();
					foreach(var countryDb in countriesDb.FindAll(e => userCountriesIds.Contains(e.Id)))
					{
						countries.Add(new KeyValuePair<int, string>(countryDb.Id, countryDb.Name));
					}

					var halls = new List<KeyValuePair<int, string>>();
					var userHallsIds = usersHallsDb
						.Where(e => e.UserId == userDb.Id)
						.Select(e => e.HallId)
						.ToList();
					foreach(var hallDb in hallsDb.FindAll(e => userHallsIds.Contains(e.Id)))
					{
						halls.Add(new KeyValuePair<int, string>(hallDb.Id, hallDb.Name));
					}

					response.Add(new Models.User.UserModel()
					{
						Id = userDb.Id,
						AccessProfileId = userDb.AccessProfileId,
						AccessProfileName = accessProfilesDb.Find(e => e.Id == userDb.AccessProfileId)?.Name,
						Name = userDb.Name,
						Username = userDb.Username,
						Countries = countries,
						Halls = halls
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
