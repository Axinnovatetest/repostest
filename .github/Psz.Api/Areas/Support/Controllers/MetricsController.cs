using Infrastructure.Data.Entities.Tables.SPR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Support.Handlers;
using Psz.Core.Support.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Support.Controllers
{
	[Authorize]
	[Area("Support")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class MetricsController: ControllerBase
	{
		private const string MODULE = "Support";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetAreas(int Id)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiAreasHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetMethods(GetMethodsRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiMethodsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetControllers(GetControllersRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiControllersHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ApiALLAreaCallsModel>>), 200)]
		public IActionResult GetAreaCalls(int id)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiAreaCallsHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ApiAreaCallsModel>>), 200)]
		public IActionResult GetSingleAreaCalls(GetSingleAreaRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetSingleApiAreaCallsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ApiAllControllerCallsModel>>), 200)]
		public IActionResult GetAllControllerCalls(GetApiControllersCallsRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiControllerCallsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ApiSingleControllerCallsModel>>), 200)]
		public IActionResult GetSingleControllerCalls(GetApiSingleControllersCallsRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiSingleControllerCallsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetMethodsCalls(GetApiMethodCallsRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetApiMethodCallsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//GetMostUsedAPIHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetFirstAndLastCalls(GetFirstAndLastCallRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetFirstAndLastCallsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetMostUsedAPI()
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetMostUsedAPIHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetFrequentERPUsers()
		{
			try
			{
				return Ok(new Psz.Core.Support.Handlers.GetFrequentERPUsersHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}
