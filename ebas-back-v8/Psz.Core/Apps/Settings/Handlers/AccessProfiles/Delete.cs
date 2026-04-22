using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<int> Delete(int id, Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| !user.Access.Settings.AccessProfiles)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var accessProfile = Get(id);
				if(accessProfile == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				var users = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(new List<int> { accessProfile.Id });
				if(users != null && users.Count > 0)
				{
					return new Core.Models.ResponseModel<int>
					{
						Success = false,
						Errors = new List<string> { "Cannot delete Access Profile in use" }
					};
				}

				Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.DeleteByMainAccessProfilesId(accessProfile.Id);
				Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.DeleteByMainAccessProfilesId(accessProfile.Id);
				Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.DeleteByMainAccessProfilesId(accessProfile.Id);

				if(Infrastructure.Data.Access.Tables.AccessProfileAccess.Delete(accessProfile.Id) > 0)
				{
					return Core.Models.ResponseModel<int>.SuccessResponse(1);
				}

				return new Core.Models.ResponseModel<int>
				{
					Success = false,
					Errors = new List<string> { "Cannot delete Access Profile" }
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
