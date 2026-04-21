using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<int> CreateWPL(Models.Users.CreateWPLModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.UsersLock)
			{
				try
				{
					#region > Access Verification
					if(user == null
						|| user.Access == null
						|| !user.Access.WorkPlan.AdministrationUserUpdate)
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

					/// Add countries & halls
					Infrastructure.Data.Access.Tables.COR.UserAccess.UpdateAccessProfile(insertedId, data.AccessProfileId);

					foreach(var hallId in data.Halls)
					{
						Infrastructure.Data.Access.Tables.WPL.UserHallAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.UserHallEntity()
						{
							Id = -1,
							HallId = hallId,
							UserId = insertedId
						});
					}

					foreach(var countryId in data.Countries)
					{
						Infrastructure.Data.Access.Tables.WPL.UserCountryAccess.Insert(new Infrastructure.Data.Entities.Tables.WPL.UserCountryEntity()
						{
							Id = -1,
							CountryId = countryId,
							UserId = insertedId
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
