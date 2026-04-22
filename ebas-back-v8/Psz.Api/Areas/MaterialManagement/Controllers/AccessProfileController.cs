using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


namespace Psz.Api.Areas.MaterialManagement.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfileController: Controller
	{
		private const string MODULE = "Material Management | Access Profile";


		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.MaterialManagement.Models.Administration.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.AddHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.MaterialManagement.Models.Administration.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditName(Core.MaterialManagement.Models.Administration.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
					.Handle());
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
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
					 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Models.Administration.AccessProfiles.AccessProfileAddRequestModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
				 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUsers([FromBody] List<int> ids)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
					.Handle());
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
		public IActionResult AddUsers(Core.MaterialManagement.Models.Administration.AccessProfiles.AddUsersRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
					 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveUsers(Core.MaterialManagement.Models.Administration.AccessProfiles.AddUsersRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddToUser(Core.MaterialManagement.Models.Administration.AccessProfiles.AddToUserRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditForUser(Core.MaterialManagement.Models.Administration.AccessProfiles.AddToUserRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
					.Handle());
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Models.Administration.Users.GetResponseModel>>), 200)]
		public IActionResult UsersGetAll()
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.MaterialManagement.Models.Administration.Users.GetResponseModel>), 200)]
		public IActionResult UsersGet(int id)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Handlers.Administration.Users.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Users
	}
}
