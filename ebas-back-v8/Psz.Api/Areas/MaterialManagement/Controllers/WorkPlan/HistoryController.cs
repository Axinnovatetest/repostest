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
	public class HistoryController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult WorkSchedule()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule);
				if(logVm != null)
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Administration()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.Administration);
				if(logVm != null)
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult StandardDefinition()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition);
				if(logVm != null)
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult DefinitionByCountry()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry);
				if(logVm != null)
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
