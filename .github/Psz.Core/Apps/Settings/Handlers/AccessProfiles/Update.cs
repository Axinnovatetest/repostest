using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<object> Update(Models.AccessProfiles.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						//|| !user.Access.Settings.ModuleActivated
						//|| !user.Access.Settings.AccessProfilesUpdate
						)
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

					accessProfileDb.Name = data.Name;
					accessProfileDb.Description = data.Name;
					accessProfileDb.LastUpdateId = user.Id;
					accessProfileDb.LastUpdateTime = DateTime.Now;
					Infrastructure.Data.Access.Tables.AccessProfileAccess.Update(accessProfileDb);

					#region > Settings
					var settingsAccessProfileDb = Infrastructure.Data.Access.Tables.STG.AccessProfileAccess
						.GetByMainAccessProfilesIds(new List<int>() { accessProfileDb.Id })
						.FirstOrDefault();
					if(settingsAccessProfileDb == null)
					{
						settingsAccessProfileDb = data.Settings.ToDbEntity(-1, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.Insert(settingsAccessProfileDb);
					}
					else
					{
						settingsAccessProfileDb = data.Settings.ToDbEntity(settingsAccessProfileDb.Id, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.Update(settingsAccessProfileDb);
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

					#region > Purchase
					var ediAccessProfileDb = Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess
						.GetByMainAccessProfilesIds(new List<int>() { accessProfileDb.Id })
						.FirstOrDefault();
					if(ediAccessProfileDb == null)
					{
						ediAccessProfileDb = data.Purchase.ToDbEntity(-1, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.Insert(ediAccessProfileDb);
					}
					else
					{
						ediAccessProfileDb = data.Purchase.ToDbEntity(ediAccessProfileDb.Id, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.Update(ediAccessProfileDb);
					}
					#endregion

					#region > Budget
					var BudgetAccessProfileDb = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess
						.GetByMainAccessProfilesIds(new List<int>() { accessProfileDb.Id })
						.FirstOrDefault();
					if(BudgetAccessProfileDb == null)
					{
						BudgetAccessProfileDb = data.Budget.ToDbEntity(-1, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Insert(BudgetAccessProfileDb);
					}
					else
					{
						BudgetAccessProfileDb = data.Budget.ToDbEntity(BudgetAccessProfileDb.Id, accessProfileDb.Id);
						Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Update(BudgetAccessProfileDb);
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
