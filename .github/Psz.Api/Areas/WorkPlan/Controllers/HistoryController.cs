using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.WorkPlan.Controllers
{
	[Authorize]
	[Area("WorkPlan")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class HistoryController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult WorkSchedule()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule);
				if(logVm != null)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Api.Models.Response<String>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult Administration()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.Administration);
				if(logVm != null)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Api.Models.Response<String>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult StandardDefinition()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.StandardDefinition);
				if(logVm != null)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Api.Models.Response<String>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "WorkPlan" })]
		public IActionResult DefinitionByCountry()
		{
			var errors = new List<string>();
			try
			{
				var logVm = Helpers.Log.GetLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.DefinitionByCountry);
				if(logVm != null)
					return Ok(new { response = new Api.Models.Response<List<Psz.Core.Apps.WorkPlan.Models.Log.LogViewModel>>() { Success = true, ResponseBody = logVm, Errors = errors } });
				return Ok(new { response = new Api.Models.Response<String>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
