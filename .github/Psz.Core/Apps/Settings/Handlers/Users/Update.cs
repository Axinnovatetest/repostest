using Psz.Core.Apps.EDI.Models.OrderResponse;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<object> Update(Models.Users.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			var botransactionFNC = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.FNC);
			var botransactionMTM = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.MTM);
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				#region > Access Verification
				if(user == null
					|| user.Access == null
					|| (!user.SuperAdministrator && !user.Access.Settings.UsersUpdate))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion


				if(data.SameProfileAs == true)
				{
					if(data.SameProfileUserId == data.Id)
					{
						return Core.Models.ResponseModel<object>.SuccessResponse();
					}

					if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.SameProfileUserId ?? 0) == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"[Same AS] user not found"
						}
						};
					}
				}
				else
				{
					var accessProfileDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(data.AccessProfileId);
					if(accessProfileDb == null || accessProfileDb.IsArchived)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Access Profile Not Found");
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"Access Profile Not Found"
						}
						};
					}
				}

				#region > Validation
				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.Id);
				if(userDb == null || userDb.IsArchived)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"user not found"
						}
					};
				}

				if(string.IsNullOrWhiteSpace(data.Name))
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"Name is empty"
						}
					};
				}


				if(data.Id > 0 && Psz.Core.FinanceControl.Helpers.Processings.Budget.User.HasDifferentAllocation(data.Id, data.DepartmentId, data.CompanyId))
					return new Core.Models.ResponseModel<object>
					{
						Success = false,
						Errors = new List<string> {
						$"User has budget allocation, please remove it before changing company or department."
						}
					};


				if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(data.CompanyId) == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Company is invalid");
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"Company is invalid"
						}
					};
				}
				if(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(data.DepartmentId) == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Department is invalid");
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"Department is invalid"
						}
					};
				}
				#endregion

				botransaction.beginTransaction();
				botransactionFNC.beginTransaction();
				botransactionMTM.beginTransaction();

				userDb.LastEditDate = System.DateTime.Now;
				userDb.LastEditUserId = user.Id;
				userDb.Name = data.Name;
				userDb.Email = data.Email;
				userDb.CompanyId = data.CompanyId;
				userDb.CountryId = data.CountryId;
				userDb.DepartmentId = data.DepartmentId;

				var oldEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(data.Id, botransaction.connection, botransaction.transaction);
				if(data.SameProfileAs == true)
				{
					var sameAsUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(data.SameProfileUserId ?? 0, botransaction.connection, botransaction.transaction);
					Psz.Core.Identity.Handlers.AccessProfile.InitProfilesAsUser(data.SameProfileUserId ?? 0, data.Id, user.Id, botransaction, botransactionFNC, botransactionMTM);
				}
				else
				{
					userDb.AccessProfileId = data.AccessProfileId;
				}

				// - 
				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateWithTransaction(userDb, botransaction.connection, botransaction.transaction);


				// -
				if(oldEntity.CompanyId != data.CompanyId)
					Infrastructure.Data.Access.Tables.CPL.Capital_requests_teamsAccess.UpdateUserPlant(data.Id, data.CompanyId, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit() && botransactionFNC.commit() && botransactionMTM.commit())
				{
					return Core.Models.ResponseModel<object>.SuccessResponse();
				}
				else
				{
					return Core.Models.ResponseModel<object>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static Core.Models.ResponseModel<object> UpdateLanguage(string data,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				#region > Access Verification
				if(user == null
					/*|| user.Access == null
                    || !user.Access.Settings.UsersUpdate*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion

				#region > Validation
				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				if(userDb == null || userDb.IsArchived)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"user not found"
						}
					};
				}
				#endregion

				userDb.LastEditDate = System.DateTime.Now;
				userDb.LastEditUserId = user.Id;
				userDb.SelectedLanguage = !string.IsNullOrWhiteSpace(data) && !string.IsNullOrEmpty(data) ? data : "en";
				Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateLanguage(userDb);

				return Core.Models.ResponseModel<object>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<object> Reactivate(int data, Core.Identity.Models.UserModel user)
		{
			try
			{
				#region > Access Verification
				if(user == null
					|| user.Access == null
					|| !user.Access.Settings.UsersUpdate)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion

				#region > Validation
				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data);
				if(userDb == null)
				{
					return new Core.Models.ResponseModel<object>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"user not found"
						}
					};
				}


				if(Psz.Core.FinanceControl.Helpers.Processings.Budget.User.HasDifferentAllocation(data, userDb.DepartmentId, userDb.CompanyId))
					return new Core.Models.ResponseModel<object>
					{
						Success = false,
						Errors = new List<string>{
							$"User has budget allocation, please remove it before changing company or department."
						}
					};

				#endregion

				userDb.LastEditDate = System.DateTime.Now;
				userDb.LastEditUserId = user.Id;
				userDb.IsActivated = true;
				userDb.IsArchived = false;

				Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userDb);

				return Core.Models.ResponseModel<object>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
