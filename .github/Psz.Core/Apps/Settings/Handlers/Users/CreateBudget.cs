using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<int> CreateBudget(Models.Users.CreateBudgetModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.UsersLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| !user.Access.Financial.Budget.AdministrationUserUpdate)
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

					var accessProfileDb = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(data.AccessProfileId);
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

					var userDb = new Infrastructure.Data.Entities.Tables.COR.UserEntity()
					{
						Username = data.Username,
						Name = data.Name,
						AccessProfileId = accessProfileDb.Id,
						CreationUserId = user.Id,
						CreationTime = DateTime.Now,
						IsActivated = true,
						Password = "-",
						Email = data.Email
					};
					var insertedId = Infrastructure.Data.Access.Tables.COR.UserAccess.Insert(userDb);

					/// Add lands & depts
					Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateAccessProfile(insertedId, data.AccessProfileId);

					foreach(var deptId in data.Depts)
					{
						Infrastructure.Data.Access.Tables.FNC.Departement_User_JointAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Departement_User_JointEntity()
						{
							ID = -1,
							ID_Departement = deptId,
							ID_user = insertedId
						});
					}

					foreach(var landId in data.Lands)
					{
						Infrastructure.Data.Access.Tables.FNC.Land_User_JointAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Land_User_JointEntity()
						{
							ID = -1,
							ID_land = landId,
							ID_user = insertedId
						});
					}
					///
					return Core.Models.ResponseModel<int>.SuccessResponse(insertedId);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
