using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Settings.Controllers
{
	[Authorize]
	[Area("Settings")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CurrencyController: ControllerBase
	{
		private const string MODULE = "Settings";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Currency.GetModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Currency.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Psz.Core.Apps.Settings.Models.Currency.GetModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Currency.AddHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Update(Psz.Core.Apps.Settings.Models.Currency.GetModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Currency.UpdateHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Delete(int id)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Currency.DeleteHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.Settings.Models.Currency.GetModel>), 200)]
		public IActionResult GetByUser()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Currency.GetByUserHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}