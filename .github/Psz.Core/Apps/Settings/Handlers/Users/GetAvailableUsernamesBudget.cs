using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<List<Models.Users.GetModel>> GetAvailableUsernamesBudget(Core.Identity.Models.UserModel user)
		{
			try
			{
				#region > Access Verification
				/* if (user == null
					 || user.Access == null
					 || (!user.Access.Settings.UsersCreate && !user.Access.Settings.UsersUpdate && !user.Access.WorkPlan.AdministrationUser))
				 {
					 throw new Core.Exceptions.UnauthorizedException();
				 }*/
				#endregion


				var usersAd = new List<Models.Users.GetModel>();

				if(!Core.Program.ActiveDirectoryManager.IsActivated)
				{//if AD not Activated
					for(int i = 0; i < 40; i++)
					{
						usersAd.Add(new Models.Users.GetModel
						{
							Id = -1,
							Email = "",
							Username = "not_ad_user_" + (i + 1),
							Name = "Not AD User " + (i + 1),
						});
					}
				}
				else
				{
					var usersDbUsernames = Infrastructure.Data.Access.Tables.COR.UserAccess.Get()?
						.Select(e => e.Username)?
						.ToList();

					usersAd = Core.Program.ActiveDirectoryManager.GetUsersInfo()
							?.FindAll(x => !usersDbUsernames.Exists(y => y.ToLower().Trim() == x.Username.ToLower().Trim()))
							?.Select(x => new Models.Users.GetModel(x))?.ToList();
				}

				return Core.Models.ResponseModel<List<Models.Users.GetModel>>.SuccessResponse(usersAd);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
