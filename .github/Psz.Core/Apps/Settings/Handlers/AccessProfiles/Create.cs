using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	using Psz.Core.Apps.Settings.Models.AccessProfiles;
	using Psz.Core.Identity.Models;
	using Psz.Core.Models;
	public partial class AccessProfiles
	{
		public static ResponseModel<object> Create(CreationModel data,
			UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| !user.Access.Settings.ModuleActivated
						|| !user.Access.Settings.AccessProfilesCreate)
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

					return create(data, user);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		internal static ResponseModel<object> create(CreationModel data, UserModel user)
		{
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
			Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Insert(BudgetAccessProfileDb);
			#endregion



			return Core.Models.ResponseModel<object>.SuccessResponse();
		}
	}
}
