using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AccessProfilesController: ControllerBase
	{
		private const string MODULE = "CRP | AccessProfiles";
		private const string ACCESS_PROFILE = "ADMIN AccessProfile";
		private readonly ICrpAdministrationService _crpAdministrationService;
		public AccessProfilesController(ICrpAdministrationService crpAdministrationService)
		{
			_crpAdministrationService = crpAdministrationService;
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Psz.Core.CRP.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.Add(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddToUser(Core.CRP.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.AddToUser(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileAddUsers(Core.CRP.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.AddUsers(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileRemoveUsers(Core.CRP.Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.RemoveUsers(this.GetCurrentUser(), data));

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
				return Ok(_crpAdministrationService.Delete(this.GetCurrentUser(), id));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEditForUser(Core.CRP.Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.EditForUser(this.GetCurrentUser(), data));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AccessProfileEdit(Core.CRP.Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				return Ok(_crpAdministrationService.Edit(this.GetCurrentUser(), data));

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
				return Ok(_crpAdministrationService.GetUsers(this.GetCurrentUser(), ids));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateProfileHorizons(Psz.Core.CRP.Models.Administration.AccessProfiles.Horizonsmodel model)
		{
			try
			{
				return Ok(_crpAdministrationService.UpdateProfileHorizons(this.GetCurrentUser(), model));
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ACCESS_PROFILE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CRP.Models.Administration.AccessProfiles.AccessProfileModel>>), 200)]
		public IActionResult AccessProfileGetAll()
		{
			try
			{
				return Ok(_crpAdministrationService.GetAllAccessProfiles(this.GetCurrentUser()));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#region Users
		private const string ADMIN_USERS = "ADMIN Users";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CRP.Models.Administration.Users.GetModel>>), 200)]
		public IActionResult AdminUsersGetAll()
		{
			try
			{
				return Ok(_crpAdministrationService.GetAll(this.GetCurrentUser()));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CRP.Models.Administration.Users.GetModel>), 200)]
		public IActionResult AdminUsersGet(int id)
		{
			try
			{
				return Ok(_crpAdministrationService.Get(this.GetCurrentUser(), id));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
	}
}