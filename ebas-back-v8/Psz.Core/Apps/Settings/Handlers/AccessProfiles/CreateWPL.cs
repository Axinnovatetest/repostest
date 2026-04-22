
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<object> CreateWPL(Models.AccessProfiles.CreationModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| !user.Access.WorkPlan.ModuleActivated
						|| !user.Access.WorkPlan.AdministrationAccessProfiles
						|| !user.Access.WorkPlan.AdministrationAccessProfilesUpdate)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}
					#endregion

					#region > Validation
					var existantAccessProfileDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.GetByName(data.Name.Trim(), true);
					if(existantAccessProfileDb != null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>()
							{
								"Name already used in an other access profile"
							}
						};
					}
					#endregion

					data.Settings.DenyAll();
					data.Purchase.DenyAll();

					return create(data, user);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

	}
}
