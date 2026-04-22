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
	public class UsersController: ControllerBase
	{
		private const string MODULE = "Material Management";

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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + " | " + ADMIN_USERS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.MaterialManagement.Models.Administration.Users.GetResponseModel>), 200)]
		public IActionResult GenerateUserNummer(Core.MaterialManagement.Models.Administration.Users.GetResponseModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.Administration.Users.EditHandler(this.GetCurrentUser(false), data)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Users
	}
}
