using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UserController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.WorkPlan.Models.User.UserModel>>), 200)]
		public IActionResult GetUsers()
		{
			try
			{
				return Ok(Core.Apps.WorkPlan.Handlers.User.Get(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.WorkPlan.Models.User.UserModel>), 200)]
		public IActionResult GetUser(int id)
		{
			try
			{
				return Ok(Core.Apps.WorkPlan.Handlers.User.Get(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdatePermissions(Core.Apps.WorkPlan.Models.User.UpdatePermissionsModel data)
		{
			try
			{
				return Ok(Core.Apps.WorkPlan.Handlers.User.UpdatePermissions(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}