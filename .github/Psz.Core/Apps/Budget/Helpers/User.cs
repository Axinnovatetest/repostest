using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Budget.Helpers
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

		//public static string GetWorkAreaNameById(int Id)
		//{
		//    try
		//    {
		//        var workAreaDb = Infrastructure.Data.Access.Tables.WPL.WorkAreaAccess.Get(Id);
		//        return workAreaDb != null ? workAreaDb.Name : "";
		//    }
		//    catch (Exception e)
		//    {
		//        return e.Message;
		//    }
		//}

		public static string getLandNameById(int land_Id)
		{
			//var LandDb = Infrastructure.Data.Access.Tables.FNC.Budget_landsAccess.Get(land_Id);
			//return LandDb == null ? "" : LandDb.Land_name;
			return "";
		}

		public static string getDeptNameById(int Dept_Id)
		{
			var DeptDb = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(Dept_Id);
			return DeptDb == null ? "" : DeptDb.Name;

		}

		public static List<int> GetUserDepts(int userId)
		{
			var userDepts = Infrastructure.Data.Access.Tables.FNC.Departement_User_JointAccess.GetByUserId(userId);

			return userDepts.Count == 0 ? null : userDepts.Select(h => h.ID_Departement).ToList();
		}
		public static List<int> GetUserDeptsJointed(int userId)
		{
			var userDepts = Infrastructure.Data.Access.Tables.FNC.Departement_User_JointAccess.GetByUserId(userId);

			return userDepts.Count == 0 ? null : userDepts.Select(h => h.ID_Departement).ToList();
		}


		public static List<int> GetUserLands(int userId)
		{
			var userLands = Infrastructure.Data.Access.Tables.FNC.Land_User_JointAccess.GetByUserId(userId);
			return userLands.Count == 0 ? null : userLands.Select(c => c.ID_land).ToList();
		}
		//Assign User
		public static List<int> GetUserAssign(int userId)
		{
			var userAssign = Infrastructure.Data.Access.Tables.FNC.Assign_User_JointAccess.GetByUserId(userId);

			return userAssign.Count == 0 ? null : userAssign.Select(h => h.ID_AssignUser).ToList();
		}
	}
}
