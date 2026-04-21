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
	public class SiteController: ControllerBase
	{
		private const string MODULE = "FinanceControl";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Site.GetModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Site.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Site.GetModel>>), 200)]
		public IActionResult GetAllPublic()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Site.GetAllPublicHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Site.GetModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Site.GetHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.FinanceControl.Models.Budget.Site.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Site.UpdateHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Site.GetModel>>), 200)]
		public IActionResult GetByUser()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Site.GetByUserHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}