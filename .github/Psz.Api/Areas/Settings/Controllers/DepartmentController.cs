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
	public class DepartmentController: ControllerBase
	{
		private const string MODULE = "Settings";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		public IActionResult GetByName(string data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.FilterByNameHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		public IActionResult GetByUser()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.GetByUserHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.Settings.Models.Department.GetModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.GetHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.Apps.Settings.Models.Department.UpdateModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.AddHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.Apps.Settings.Models.Department.UpdateModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.UpdateHandler(this.GetCurrentUser(), data).Handle());
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
				return Ok(new Core.Apps.Settings.Handlers.Department.DeleteHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		public IActionResult GetAllowedLeasing()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Department.GetAllowedLeasingHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}