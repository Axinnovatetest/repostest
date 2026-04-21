using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfilesController: ControllerBase
	{
		private const string MODULE = "CustomerService | AccessProfiles";

		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.CustomerService.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.AddHandler(this.GetCurrentUser(), data)
				   .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEdit(Core.CustomerService.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditName(Core.CustomerService.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileDelete(int id)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
					 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.Administration.AccessProfiles.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetAll()
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
				 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AccessProfileGetUsers([FromBody] List<int> ids)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddUsers(Core.CustomerService.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
					 .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveUsers(Core.CustomerService.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddToUser(Core.CustomerService.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditForUser(Core.CustomerService.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion User Profiles

		#region Users
		private const string ADMIN_USERS = "ADMIN Users";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.Administration.Users.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				/*var response = 1;*/
				return Ok(new Core.CustomerService.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.Administration.Users.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				//var response = 1;
				return Ok(new Core.CustomerService.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
					.Handle());

				//return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProfileHorizons(Psz.Core.CustomerService.Models.Administration.AccessProfiles.Horizonsmodel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Administration.AccessProfiles.UpdateProfileHorizonsHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion Users
	}
}