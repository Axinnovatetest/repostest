using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Helpers
{
   internal class UserValidationHelper
	{
		public static  bool IsValidUser(Identity.Models.UserModel user)
		{
			try
			{
				return
				user is not null
				&& user.IsAdministrator
				&& (user.Id > 0 || user.IsAdministrator)
				&& !string.IsNullOrWhiteSpace(user.Name)
				&& !string.IsNullOrWhiteSpace(user.Email)
				&& !string.IsNullOrWhiteSpace(user.Username);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
			return false;
		}
	}
}
