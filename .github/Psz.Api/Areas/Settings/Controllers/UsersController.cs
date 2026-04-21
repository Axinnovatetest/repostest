using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Settings.Controllers
{
	[Authorize]
	[Area("Settings")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UsersController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Settings.Models.Users.UserModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				//return Ok(Core.Apps.Settings.Handlers.Users.Get(this.GetCurrentUser()));
				return Ok(new Core.Apps.Settings.Handlers.GetAllUsersHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Settings.Models.Users.UserModel>>), 200)]
		public IActionResult GetByDepartment(int id)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.User.GetByDepartmentHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Settings.Models.Users.UserModel>>), 200)]
		public IActionResult GetByDepartmentDirector()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.User.GetByDepartmentDirectorHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Settings.Models.Users.UserModel>), 200)]
		public IActionResult GetConnected()
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.Get(this.GetCurrentUser()?.Id ?? -1, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Settings.Models.Users.UserModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.Get(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		public IActionResult Delete(int id)
		{
			var errors = new List<string>();
			try
			{
				var connectedUser = this.GetCurrentUser();

				if(connectedUser == null)
				{
					return Ok(new
					{
						response = new Core.Models.ResponseModel<string>
						{
							Success = false,
							Body = null,
							Errors = new List<string>() { "User not connected" }
						}
					});
				}

				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(id);
				if(userDb == null)
				{
					return Ok(new
					{
						response = new Core.Models.ResponseModel<string>
						{
							Success = false,
							Body = null,
							Errors = new List<string>() { "User not found" }
						}
					});
				}

				userDb.IsActivated = false;
				userDb.IsArchived = true;
				userDb.DeleteDate = DateTime.Now;
				userDb.DeleteUserId = connectedUser.Id;

				Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userDb);

				Areas.WorkPlan.Helpers.Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.Administration,
					$"{connectedUser.Username} deleted a User : {userDb.Username}.",
					connectedUser.Id);

				return Ok(new
				{
					response = new Core.Models.ResponseModel<string>
					{
						Success = true,
						Body = "delete Success.Log Add success",
						Errors = errors
					}
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult DeleteUserBDG(int id)
		{
			var errors = new List<string>();
			try
			{
				var connectedUser = this.GetCurrentUser();

				if(connectedUser == null)
				{
					return Ok(new
					{
						response = new Core.Models.ResponseModel<string>
						{
							Success = false,
							Body = null,
							Errors = new List<string>() { "User not connected" }
						}
					});
				}

				var userDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(id);
				if(userDb == null)
				{
					return Ok(new
					{
						response = new Core.Models.ResponseModel<string>
						{
							Success = false,
							Body = null,
							Errors = new List<string>() { "User not found" }
						}
					});
				}

				userDb.IsActivated = false;
				userDb.IsArchived = true;
				userDb.DeleteDate = DateTime.Now;
				userDb.DeleteUserId = connectedUser.Id;

				Infrastructure.Data.Access.Tables.COR.UserAccess.Update(userDb);

				Areas.FinanceControl.Helpers.Log.NewLog(Core.Apps.Budget.Enums.LogEnums.LogType.Administration,
					$"{connectedUser.Username} deleted a User : {userDb.Username}.",
					connectedUser.Id);

				return Ok(new
				{
					response = new Core.Models.ResponseModel<string>
					{
						Success = true,
						Body = $"delete Success.Log Add success",
						Errors = errors
					}
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Create(Core.Apps.Settings.Models.Users.UpdateModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.Create(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Update(Core.Apps.Settings.Models.Users.UpdateModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.Update(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Reactivate(int id)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.Reactivate(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Users.GetModel>>), 200)]
		public IActionResult searchUsers(string searchText)
		{
			try
			{
				return Ok(new Psz.Core.Apps.Settings.Handlers.User.SearchHandler(this.GetCurrentUser(), searchText).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetAvailableUsernames()
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.GetAvailableUsernames(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//Budget users for Project
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetAvailableUsernamesBudget()
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.GetAvailableUsernamesBudget(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.GetModel>>), 200)]
		public IActionResult GetADGroups()
		{
			try
			{
				return Ok(new Core.Apps.Main.Handlers.ActiveDirectory.Groups.GetHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Models.Main.ActiveDirectory.Groups.UserModel>>), 200)]
		public IActionResult GetADGroupUser(string groupName)
		{
			try
			{
				return Ok(new Core.Apps.Main.Handlers.ActiveDirectory.Groups.GetUsersHandler(this.GetCurrentUser(), groupName).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateLanguage(string lang)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.UpdateLanguage(lang, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult CreateWPL(Core.Apps.Settings.Models.Users.CreateWPLModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.CreateWPL(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult CreateBudget(Core.Apps.Settings.Models.Users.CreateBudgetModel data)
		{
			try
			{
				return Ok(Core.Apps.Settings.Handlers.Users.CreateBudget(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Settings" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult GetUsersNamesIds(string filter)
		{
			try
			{
				return Ok(new Psz.Core.Apps.Settings.Handlers.GetUsersNamesIdsHandler(filter, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
