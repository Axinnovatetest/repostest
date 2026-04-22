using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<int> Create(Models.Users.UpdateModel data,
			Core.Identity.Models.UserModel user)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			var botransactionFNC = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.FNC);
			var botransactionMTM = new Infrastructure.Services.Utils.TransactionsManager(Infrastructure.Services.Utils.TransactionsManager.Database.MTM);
			try
			{

				#region // -- transaction-based logic -- //
				#region > Access Verification
				if(user == null
					|| user.Access == null
					|| !user.Access.Settings.UsersUpdate)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}
				#endregion

				#region > Validation
				var errors = new List<string>();
				if(string.IsNullOrWhiteSpace(data.Name))
				{
					errors.Add("Name is empty");
				}
				if(string.IsNullOrWhiteSpace(data.Username))
				{
					errors.Add("Username is empty");
				}
				if(string.IsNullOrWhiteSpace(data.Email))
				{
					errors.Add("Email is empty");
				}
				if(!Infrastructure.Services.Email.Helpers.IsValidEmail(data.Email))
				{
					errors.Add("Email is invalid");
				}
				if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(data.CompanyId) == null)
				{
					errors.Add("Company is invalid");
				}
				if(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(data.DepartmentId) == null)
				{
					errors.Add("Department is invalid");
				}
				if(errors.Count > 0)
				{
					return new Core.Models.ResponseModel<int>()
					{
						Success = false,
						Errors = errors
					};
				}

				data.Username = data.Username.Trim();

				var usernameUsed = Infrastructure.Data.Access.Tables.COR.UserAccess.CheckExists(data.Username);
				if(usernameUsed)
				{
					return new Core.Models.ResponseModel<int>()
					{
						Success = false,
						Errors = new List<string>() { "Username already used" }
					};
				}

				if(Core.Program.ActiveDirectoryManager.IsActivated)
				{
					var userAd = Program.ActiveDirectoryManager.GetUser(data.Username);
					if(string.IsNullOrEmpty(userAd.Key))
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "User not found in ActiveDirectory");
						return new Core.Models.ResponseModel<int>()
						{
							Success = false,
							Errors = new List<string>()
									{
										"User not found in ActiveDirectory"
									}
						};
					}
				}
				var asUser = new Infrastructure.Data.Entities.Tables.COR.UserEntity();
				if(data.SameProfileAs.HasValue && data.SameProfileAs.Value)
				{
					asUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.SameProfileUserId ?? -1, false);
				}
				var id = (data.SameProfileAs.HasValue && data.SameProfileAs.Value) ? asUser.AccessProfileId : data.AccessProfileId;
				var accessProfileDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(id);
				if(accessProfileDb == null || accessProfileDb.IsArchived)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Access Profile Not Found");
					return new Core.Models.ResponseModel<int>()
					{
						Success = false,
						Errors = new List<string>()
								{
									"Access Profile Not Found"
								}
					};
				}
				#endregion

				botransaction.beginTransaction();
				botransactionFNC.beginTransaction();
				botransactionMTM.beginTransaction();

				var userDb = new Infrastructure.Data.Entities.Tables.COR.UserEntity()
				{
					Username = data.Username,
					Name = data.Name,
					AccessProfileId = (data.SameProfileAs.HasValue && data.SameProfileAs.Value) ? asUser.AccessProfileId : accessProfileDb.Id,
					CreationUserId = user.Id,
					CreationTime = DateTime.Now,
					IsActivated = true,
					Password = "-",
					Email = data.Email,
					CompanyId = data.CompanyId,
					DepartmentId = data.DepartmentId,
					CountryId = data.CountryId,
					SelectedLanguage = "en",
					// -
					SalesDistributionApp = data.SalesDistributionApp,
					CustomerServiceApp = data.CustomerServiceApp,
					FinanceControlApp = data.FinanceControlApp,
					LogisticsApp = data.LogisticsApp,
					HumanResourcesApp = data.HumanResourcesApp,
					MaterialManagementApp = data.MaterialManagementApp,
					MasterDataApp = data.MasterDataApp,
					SettingsApp = data.SettingsApp
				};
				//temporary (to check later)
				if(data.SalesDistributionApp.HasValue && data.SalesDistributionApp.Value)
				{
					var defaultWPProfile = Infrastructure.Data.Access.Tables.WPL.AccessProfileAccess.GetDefaultProfiles(botransaction.connection, botransaction.transaction);
					if(defaultWPProfile != null && defaultWPProfile.Count > 0)
						userDb.AccessProfileId = defaultWPProfile[0].Id;
				}
				var insertedId = Infrastructure.Data.Access.Tables.COR.UserAccess.InsertWithTransaction(userDb, botransaction.connection, botransaction.transaction);

				if(data.SameProfileAs == true)
				{
					//var sameAsUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(data.SameProfileUserId ?? 0, botransaction.connection, botransaction.transaction);
					Psz.Core.Identity.Handlers.AccessProfile.InitProfilesAsUser(asUser?.Id ?? 0, insertedId, user.Id, botransaction, botransactionFNC, botransactionMTM);
				}
				else
				{
					//adding default access profiles
					//cts
					if(data.CustomerServiceApp.HasValue && data.CustomerServiceApp.Value)
					{
						var defaultCTSProfiles = Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetDefaultProfiles( botransaction.connection, botransaction.transaction);
						var apUserEntites = defaultCTSProfiles?.Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileUsersEntity
						{
							AccessProfileId = x.Id,
							AccessProfileName = x.AccessProfileName,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = data.Email,
							UserId = insertedId,
							UserName = data.Username
						}).ToList();
						Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.InsertWithTransaction(apUserEntites, botransaction.connection, botransaction.transaction);
					}
					//mtm
					if(data.MaterialManagementApp.HasValue && data.MaterialManagementApp.Value)
					{
						var defaultMTMProfiles = Infrastructure.Data.Access.Tables.MTM.AccessProfileAccess.GetDefaultProfiles( botransactionMTM.connection, botransactionMTM.transaction);
						var apUserEntites = defaultMTMProfiles?.Select(x => new Infrastructure.Data.Entities.Tables.MTM.AccessProfileUsersEntity
						{
							AccessProfileId = x.Id,
							AccessProfileName = x.AccessProfileName,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = data.Email,
							UserId = insertedId,
							UserName = data.Username,
						}).ToList();
						Infrastructure.Data.Access.Tables.MTM.AccessProfileUsersAccess.InsertWithTransaction(apUserEntites, botransactionMTM.connection, botransactionMTM.transaction);
					}
					//bsd
					if(data.MasterDataApp.HasValue && data.MasterDataApp.Value)
					{
						var defaultBSDProfiles = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.GetDefaultProfiles( botransaction.connection, botransaction.transaction);
						var apUserEntites = defaultBSDProfiles?.Select(x => new Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity
						{
							AccessProfileId = x.Id,
							AccessProfileName = x.AccessProfileName,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = data.Email,
							UserId = insertedId,
							UserName = data.Username,
						}).ToList();
						Infrastructure.Data.Access.Tables.BSD.AccessProfileUsersAccess.InsertWithTransaction(apUserEntites, botransaction.connection, botransaction.transaction);
					}
					//lgt
					if(data.LogisticsApp.HasValue && data.LogisticsApp.Value)
					{
						var defaultLGTProfiles = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.GetDefaultProfiles( botransaction.connection, botransaction.transaction);
						var apUserEntites = defaultLGTProfiles?.Select(x => new Infrastructure.Data.Entities.Tables.Logistics.AccessProfileUsersEntity
						{
							AccessProfileId = x.Id,
							AccessProfileName = x.AccessProfileName,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = data.Email,
							UserId = insertedId,
							UserName = data.Username,
						}).ToList();
						Infrastructure.Data.Access.Tables.Logistics.AccessProfileUsersAccess.InsertWithTransaction(apUserEntites, botransaction.connection, botransaction.transaction);
					}
					//fnc
					if(data.FinanceControlApp.HasValue && data.FinanceControlApp.Value)
					{
						var defaultFNCProfiles = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetDefaultProfiles(botransactionFNC.connection, botransactionFNC.transaction);
						var apUserEntites = defaultFNCProfiles?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.UserAccessProfilesEntity
						{
							AccessProfileId = x.Id,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
							UserEmail = data.Email,
							UserId = insertedId,
							UserName = data.Username,
							AccessProfileName = x.AccessProfileName,
						}).ToList();
						Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.InsertWithTransaction(apUserEntites, botransactionFNC.connection, botransactionFNC.transaction);
					}
				}
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit() && botransactionFNC.commit() && botransactionMTM.commit())
				{
					return Core.Models.ResponseModel<int>.SuccessResponse(insertedId);
				}
				else
				{
					return Core.Models.ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				botransactionFNC.rollback();
				botransactionMTM.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
