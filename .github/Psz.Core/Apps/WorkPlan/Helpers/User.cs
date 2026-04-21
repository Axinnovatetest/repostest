using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.WorkPlan.Helpers
{
	public class User
	{
		public static string GetUserNameById(int Id)
		{
			try
			{
				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(Id);
				return userDb != null ? userDb.Username : "";
			} catch(Exception e)
			{
				return e.Message;
			}
		}

		public static string GetWorkAreaNameById(int Id)
		{
			try
			{
				var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(Id);
				return workAreaDb != null ? workAreaDb.Name : "";
			} catch(Exception e)
			{
				return e.Message;
			}
		}

		public static string getCountryNameById(int country_Id)
		{
			var countryDb = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(country_Id);
			return countryDb == null ? "" : countryDb.Name;
		}

		public static string getHallNameById(int hall_Id)
		{
			var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(hall_Id);
			return hallDb == null ? "" : hallDb.Name;

		}

		public static List<int> GetUserHalls(int userId)
		{
			var userHalls = Infrastructure.Data.Access.Tables.WPL.UserHallAccess.GetByUserId(userId);

			return userHalls.Count == 0 ? null : userHalls.Select(h => h.HallId)?.Distinct()?.ToList();
		}
		public static List<int> GetAllHalls()
		{
			var userHalls = Infrastructure.Data.Access.Tables.WPL.UserHallAccess.Get();

			return userHalls.Count == 0 ? null : userHalls.Select(h => h.HallId).ToList();
		}

		public static List<int> GetUserCoutntries(int userId)
		{
			var userCountries = Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.GetByUserId(userId);
			return userCountries.Count == 0 ? null : userCountries.Select(c => c.CountryId).ToList();
		}
		public static List<int> GetAllCoutntries()
		{
			var userCountries = Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.Get();
			return userCountries.Count == 0 ? null : userCountries.Select(c => c.CountryId)?.Distinct()?.ToList();
		}
		public static string getDepartmentNameById(int? id)
		{
			if(id == null)
				return "";

			var entity = Infrastructure.Data.Access.Tables.WPL.DepartmentAccess.Get((int)id);
			return entity == null ? "" : entity.Name;

		}
	}
}
