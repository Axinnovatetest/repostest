using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CapitalRequests.Models;
using Psz.Core.CapitalRequests.Services;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CapitalRequests.Controllers
{
	[Authorize]
	[Area("CapitalRequests")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfilesController: ControllerBase
	{
		private readonly ICapitalRequestsAdminstrationService _capitalRequestsAdminstrationService;
		public AccessProfilesController(ICapitalRequestsAdminstrationService capitalRequestsAdminstrationService)
		{
			_capitalRequestsAdminstrationService = capitalRequestsAdminstrationService;
		}
		private const string MODULE = "CapitalRequests | AccessProfiles";
		#region User Profiles
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.CapitalRequests.Models.AccessProfileModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.Add(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEdit(Core.CapitalRequests.Models.AccessProfileModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.Edit(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditName(Core.CapitalRequests.Models.AccessProfileModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.EditName(this.GetCurrentUser(), data));

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
				return Ok(_capitalRequestsAdminstrationService.Delete(this.GetCurrentUser(), id));

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
				return Ok(_capitalRequestsAdminstrationService.GetAllAccessProfiles(this.GetCurrentUser()));

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
				return Ok(_capitalRequestsAdminstrationService.GetUsers(this.GetCurrentUser(), ids));

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
		public IActionResult AccessProfileAddUsers(Core.CapitalRequests.Models.AddUsersModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.AddUsers(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveUsers(Core.CapitalRequests.Models.AddUsersModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.RemoveUsers(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddToUser(Core.CapitalRequests.Models.AddToUserModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.AddToUser(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditForUser(Core.CapitalRequests.Models.AddToUserModel data)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.EditForUser(this.GetCurrentUser(), data));

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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CapitalRequests.Models.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.GetAll(this.GetCurrentUser()));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CapitalRequests.Models.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				return Ok(_capitalRequestsAdminstrationService.Get(this.GetCurrentUser(), id));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Users

		#region Teams
		private const string TEAMS = "Teams";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { TEAMS })]
		[ProducesResponseType(typeof(ResponseModel<KeyValuePair<int, string>>), 200)]
		public IActionResult GetTeam(string team)
		{
			try
			{
				var response = _capitalRequestsAdminstrationService.GetTeam(this.GetCurrentUser(), team);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { TEAMS })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddUsersToTeam(TeamUsersModel data)
		{
			try
			{
				var response = _capitalRequestsAdminstrationService.AddUsersToTeam(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { TEAMS })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult RemoveUserFromTeam(UserTeamRemoveModel data)
		{
			try
			{
				var response = _capitalRequestsAdminstrationService.RemoveUserFromTeam(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}