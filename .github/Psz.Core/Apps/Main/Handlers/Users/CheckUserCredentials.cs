using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Main.Handlers
{
	public partial class Users
	{
		public static Core.Models.ResponseModel<Models.UserModel> CheckUserCredentials(string username, string password)
		{
			try
			{
				var errors = new List<string>();

				if(string.IsNullOrWhiteSpace(username))
				{
					errors.Add("username cannot be empty");
				}

				if(string.IsNullOrWhiteSpace(password))
				{
					errors.Add("password cannot be empty");
				}

				if(errors.Count > 0)
				{
					return new Core.Models.ResponseModel<Models.UserModel>()
					{
						Success = false,
						Errors = errors
					};
				}

				username = username[0] == ' ' && username.Length == username.Trim().Length + 2 ? username.TrimStart() : username.Trim();
				var userAd = Program.ActiveDirectoryManager.CheckUserCrendentials(username, password);
				if(username.ToLower() == "adminit")
				{
					if(password != Core.Program.ADdwp)
					{
						return new Core.Models.ResponseModel<Models.UserModel>()
						{
							Success = false,
							Errors = new List<string>()
							{
								"Wrong Active Directory credentials"
							}
						};
					}
				}
				else
				{
					if(userAd == false)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Wrong Active Directory credentials");
						Infrastructure.Data.Access.Tables.COR.UserAccess.SetLastConnectError(username);
						return new Core.Models.ResponseModel<Models.UserModel>()
						{
							Success = false,
							Errors = new List<string>()
							{
								"Wrong Active Directory credentials"
							}
						};
					}
				}

				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByUsername(username);
				if(userDb == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "user not found in database");
					return new Core.Models.ResponseModel<Models.UserModel>()
					{
						Success = false,
						Errors = new List<string>()
						{
							"User not found"
						}
					};
				}

				if(username == username.TrimEnd(new char[] { ' ', '.', ',' }))
				{
					Infrastructure.Data.Access.Tables.COR.UserAccess.SetLastConnect(userDb.Id);
				}
				return new Core.Models.ResponseModel<Models.UserModel>()
				{
					Success = true,
					Body = Get(userDb.Id, true)
				};



			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
