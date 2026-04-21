using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


namespace Psz.Api.Areas.ManagementOverview.Controllers
{
	[Authorize]
	[Area("ManagementOverview")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfileController: Controller
	{
		private const string MODULE = "Management Overview | Access Profile";


		#region User Profiles
		private const string ACCESS_PROFILE = "";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.AddHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.EditHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditName(Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.EditNameHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileDelete(int id)
		{
			try
			{
				return Ok(new Psz.Core.ManagementOverview.Administration.Handlers.AccessProfiles.DeleteHandler(this.GetCurrentUser(), id)
					 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Administration.Models.AccessProfiles.AccessProfileAddRequestModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.GetAllAccessProfilesHandler(this.GetCurrentUser())
				 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUsers([FromBody] List<int> ids)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.GetUsersHandler(this.GetCurrentUser(), ids)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddUsers(Core.ManagementOverview.Administration.Models.AccessProfiles.AddUsersRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.AddUsersHandler(this.GetCurrentUser(), data)
					 .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveUsers(Core.ManagementOverview.Administration.Models.AccessProfiles.AddUsersRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.RemoveUsersHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddToUser(Core.ManagementOverview.Administration.Models.AccessProfiles.AddToUserRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.AddToUserHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ACCESS_PROFILE}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditForUser(Core.ManagementOverview.Administration.Models.AccessProfiles.AddToUserRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.AccessProfiles.EditForUserHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion User Profiles

		#region Users
		private const string ADMIN_USERS = "";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ADMIN_USERS}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Administration.Models.Users.GetResponseModel>>), 200)]
		public IActionResult UsersGetAll()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { $"{MODULE}{ADMIN_USERS}" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.Administration.Models.Users.GetResponseModel>), 200)]
		public IActionResult UsersGet(int id)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.Users.GetHandler(this.GetCurrentUser(), id)
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
