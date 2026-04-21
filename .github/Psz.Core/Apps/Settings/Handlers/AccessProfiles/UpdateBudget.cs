using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class AccessProfiles
	{
		public static Core.Models.ResponseModel<object> UpdateBudget(Models.AccessProfiles.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.AccessProfilesLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| (!user.Access.Financial.Budget.ModuleActivated
							&& !user.Access.Financial.Budget.AdministrationAccessProfilesUpdate))
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
