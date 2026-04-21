using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CapitalRequests.Models;
using Psz.Core.CapitalRequests.Services;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Psz.Api.Areas.CapitalRequests
{
	[Authorize]
	[Area("Capital")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CapitalRequestsController: ControllerBase
	{
		private const string MODULE = "Capital";
		private readonly ICapitalRequestsService _capitalRequestsService;

		public CapitalRequestsController(ICapitalRequestsService capitalRequestsService)
		{
			_capitalRequestsService = capitalRequestsService;
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<RequestHeaderModel>>), 200)]
		public IActionResult GetCapitalRequests()
		{
			try
			{
				var response = _capitalRequestsService.GetRequests(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<CapitalRequestModel>), 200)]
		public IActionResult GetCapitalRequestById(int id)
		{
			try
			{
				var response = _capitalRequestsService.GetRequestById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddCapitalRequest(CapitalRequestModel data)
		{
			try
			{
				var response = _capitalRequestsService.AddRequest(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateCapitalRequest(CapitalRequestModel data)
		{
			try
			{
				var response = _capitalRequestsService.UpdateRequest(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCapitalRequestCategories()
		{
			try
			{
				var response = _capitalRequestsService.GetRequestCategories(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<string>), 200)]
		public IActionResult GetFAArtikelnummer(int fa)
		{
			try
			{
				var response = _capitalRequestsService.GetFAArtikelnummer(this.GetCurrentUser(), fa);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, fa);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<CapitalRequestsLogsResponseModel>>), 200)]
		public IActionResult GetRequestsLogs(CapitalRequestsLogsRequestModel data)
		{
			try
			{
				var response = _capitalRequestsService.GetRequestsLogs(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<RequestStatsModel>), 200)]
		public IActionResult GetRequestsStats(int plantId)
		{
			try
			{
				var response = _capitalRequestsService.GetRequestsStats(this.GetCurrentUser(), plantId);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UnvalidatePositionCapital(int id)
		{
			try
			{
				var response = _capitalRequestsService.UnvalidatePositionCapital(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UnvalidatePositionEngeneering(int id)
		{
			try
			{
				var response = _capitalRequestsService.UnvalidatePositionEngeneering(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetPlants()
		{
			try
			{
				var response = _capitalRequestsService.GetPlants(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}