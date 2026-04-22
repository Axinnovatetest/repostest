using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistics")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class PrincipalController: ControllerBase
	{
		private const string MODULE = "Logistics | Principal";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Principal.LagerBestandModel>>), 200)]
		public IActionResult GetListLagerBestand()
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Principal.GetListLagerBestand(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Principal.LagerBestandModel>>), 200)]
		public IActionResult GetPaginationListLagerBestand(Psz.Core.Logistics.Models.Principal.LagerBestandSearchModel data)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Principal.GetPaginationListLagerBestand(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
