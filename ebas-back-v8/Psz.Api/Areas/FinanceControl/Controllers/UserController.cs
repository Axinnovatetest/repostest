using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.FinanceControl.Controllers
{
	[Authorize]
	[Area("FinanceControl")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UserController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]

		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Budget.Models.User.UserModel>>), 200)]
		public IActionResult GetUsers()
		{
			try
			{
				return Ok(Core.Apps.Budget.Handlers.User.Get(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Budget.Models.User.UserModel>), 200)]
		public IActionResult GetUser(int id)
		{
			try
			{
				return Ok(Core.Apps.Budget.Handlers.User.Get(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Apps.Budget.Models.User.UserModel>>), 200)]
		public IActionResult GetUserHeader(int id)
		{
			try
			{
				//var response = new Core.Apps.Budget.Handlers.GetCurrentHeaderByIdHandler(this.GetCurrentUser(), id).Handle();
				var response = new Core.Apps.Budget.Handlers.GetCurrentHeaderByIdHandler(id).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Budget.Models.User.UserModel>), 200)]
		public IActionResult GetUserListAssign(int id)
		//public IActionResult GetUserListAssign()
		{
			try
			{
				var connectedUser = this.GetCurrentUser();
				//return Ok(Core.Apps.Budget.Handlers.User.GetCurrentToAssign(connectedUser, connectedUser.Id));
				return Ok(Core.Apps.Budget.Handlers.User.GetCurrentToAssign(connectedUser.Id));
				// return Ok(Core.Apps.Budget.Handlers.User.GetCurrentToAssign());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}



		[HttpPost]
		[SwaggerOperation(Tags = new[] { "FinanceControl" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdatePermissions(Core.Apps.Budget.Models.User.UpdatePermissionsModel data)
		{
			try
			{
				return Ok(Core.Apps.Budget.Handlers.User.UpdatePermissions(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}