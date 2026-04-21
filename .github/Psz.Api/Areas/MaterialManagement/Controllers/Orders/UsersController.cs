using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.MaterialManagement.Orders.Models.Users;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{

	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UsersController: ControllerBase
	{
		private const string MODULE = "Material Management | Orders";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<GetResponseModel>>), 200)]
		public IActionResult Get(GetRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Users.GetHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

	}
}
