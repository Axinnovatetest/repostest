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
	public class UsersController: ControllerBase
	{
		private const string MODULE = "Management Overview | Access Profile";

		#region Users
		private const string ADMIN_USERS = "";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
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
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.Administration.Models.Users.GetResponseModel>), 200)]
		public IActionResult UserEdit(Core.ManagementOverview.Administration.Models.Users.GetResponseModel data)
		{
			try
			{
				var response = new Psz.Core.ManagementOverview.Administration.Handlers.Users.EditHandler(this.GetCurrentUser(false), data)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetLagerUser(int id)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.Users.GetLagerUserHandler(this.GetCurrentUser())
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
