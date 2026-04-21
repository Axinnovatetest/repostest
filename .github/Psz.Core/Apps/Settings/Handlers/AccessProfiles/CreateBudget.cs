using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<object> CreateBudget(Models.AccessProfiles.CreationModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						//|| !user.Access.Budget.ModuleActivated
						//|| !user.Access.Budget.AdministrationAccessProfiles
						//|| !user.Access.Budget.AdministrationAccessProfilesUpdate
						)
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

					var accessProfileDb = new Infrastructure.Data.Entities.Tables.AccessProfileEntity()
					{
						Id = -1,
						Name = data.Name,
						Description = data.Name,
						CreationTime = DateTime.Now,
						CreationUserId = user.Id,
						IsArchived = false,
					};
					var insertedId = Infrastructure.Data.Access.Tables.AccessProfileAccess.Insert(accessProfileDb);

					#region > Settings
					var settingsAccessProfileDb = data.Settings.ToDbEntity(-1, insertedId);
					Infrastructure.Data.Access.Tables.STG.AccessProfileAccess.Insert(settingsAccessProfileDb);
					#endregion

					#region > WorkPlan
					var workPlanAccessProfileDb = data.WorkPlan.ToDbEntity(-1, insertedId);
					workPlanAccessProfileDb.ModuleActivated = true;
					Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.Insert(workPlanAccessProfileDb);
					#endregion

					#region > Purchase
					var ediAccessProfileDb = data.Purchase.ToDbEntity(-1, insertedId);
					Infrastructure.Data.Access.Tables.PRS.AccessProfileAccess.Insert(ediAccessProfileDb);
					#endregion

					#region > Budget
					var BudgetAccessProfileDb = data.Budget.ToDbEntity(-1, insertedId);
					BudgetAccessProfileDb.ModuleActivated = true;
					BudgetAccessProfileDb.Budget = true;
					Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Insert(BudgetAccessProfileDb);
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
