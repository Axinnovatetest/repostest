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
	public class CompanyController: ControllerBase
	{
		private const string MODULE = "Settings";

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Department.GetModel>>), 200)]
		//public IActionResult GetByName(string data)
		//{
		//    try
		//    {
		//        return Ok(new Core.Apps.Settings.Handlers.Company.FilterByNameHandler(this.GetCurrentUser(), data).Handle());
		//    }
		//    catch (Exception e)
		//    {
		//        return this.HandleException(e);
		//    }
		//}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Company.GetModel>>), 200)]
		public IActionResult GetByUser()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.GetByUserHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.Settings.Models.Company.GetModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.Settings.Models.Company.GetModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.GetHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.Apps.Settings.Models.Company.GetModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.AddHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.Apps.Settings.Models.Company.GetModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.UpdateHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult EditLogo([FromForm] Models.Company.EditLogoModel data)
		{
			try
			{
				return Ok(new Core.Apps.Settings.Handlers.Company.UpdateLogoHandler(this.GetCurrentUser(), data?.ToBusinessModel()).Handle());
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
				return Ok(new Core.Apps.Settings.Handlers.Company.DeleteHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}