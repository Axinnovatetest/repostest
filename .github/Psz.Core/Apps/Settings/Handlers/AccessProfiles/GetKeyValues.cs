using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<int, string>>> GetKeyValues(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| user.Access == null
					|| !user.Access.Settings.ModuleActivated)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var data = Infrastructure.Data.Access.Tables.AccessProfileAccess.GetIdsNames();

				return Core.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
