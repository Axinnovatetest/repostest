using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<string, string>>> GetAvailableUsernames(Core.Identity.Models.UserModel user)
		{
			try
			{
				#region > Access Verification
				if(user == null
					|| user.Access == null
					|| (!user.Access.Settings.UsersUpdate && !user.Access.WorkPlan.AdministrationUser))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion

				var usersDbUsernames = Infrastructure.Data.Access.Tables.COR.UserAccess.Get()
					.Select(e => e.Username)
					.ToList();

				var usersAd = new List<KeyValuePair<string, string>>();

				if(!Core.Program.ActiveDirectoryManager.IsActivated)
				{//if AD not Activated
					for(int i = 0; i < 40; i++)
					{
						usersAd.Add(new KeyValuePair<string, string>("not_ad_user_" + (i + 1), "Not AD User " + (i + 1)));
					}
				}
				else
				{
					usersAd = Core.Program.ActiveDirectoryManager.GetADUsers();
				}

				//usersAd = usersAd.FindAll(e => !usersDbUsernames.Contains(e.Key));

				return Core.Models.ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(usersAd);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
