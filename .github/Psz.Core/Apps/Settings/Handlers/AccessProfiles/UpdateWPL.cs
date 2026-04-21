using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<object> UpdateWPL(Models.AccessProfiles.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| (!user.Access.WorkPlan.ModuleActivated
							&& !user.Access.WorkPlan.AdministrationAccessProfilesUpdate))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}
					#endregion

					#region > Validation
					var accessProfileDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(data.Id);
					if(accessProfileDb == null || accessProfileDb.IsArchived)
					{
						throw new Core.Exceptions.NotFoundException();
					}

					if(accessProfileDb.Name.Trim() != data.Name.Trim())
					{
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
					}
					#endregion

					#region > WorkPlan
					var WorkPlanAccessProfileDb = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess
						.GetByMainAccessProfilesIds(new List<int>() { accessProfileDb.Id })
						.FirstOrDefault();
					if(WorkPlanAccessProfileDb == null)
					{
						WorkPlanAccessProfileDb = data.WorkPlan.ToDbEntity(-1, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.Insert(WorkPlanAccessProfileDb);
					}
					else
					{
						WorkPlanAccessProfileDb = data.WorkPlan.ToDbEntity(WorkPlanAccessProfileDb.Id, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.Update(WorkPlanAccessProfileDb);
					}
					#endregion

					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
