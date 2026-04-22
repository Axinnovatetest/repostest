using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Support.Models;
using Swashbuckle.AspNetCore.Annotations;

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

		#region API Calls
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Support.Models.ApiCallCountResponseModel>), 200)]
		public IActionResult GetApiCallsCount(ApiCallCountRequestModel data)
		{
			try
			{
				return Ok(new Core.Support.Handlers.ApiCalls.GetApiCallsCountHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Support.Models.ApiCallCountSixMonthModel>>), 200)]
		public IActionResult GetApiCallsCountSixmonthsPrior(string api)
		{
			try
			{
				return Ok(new Core.Support.Handlers.ApiCalls.GetApiCallCountSixMonthPriorHandler(this.GetCurrentUser(), api).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<KeyValuePair<DateTime, DateTime>>), 200)]
		public IActionResult GetApiCallFirstAndLastCall(string api)
		{
			try
			{
				return Ok(new Core.Support.Handlers.ApiCalls.GetFirstAndLastCallHandler(this.GetCurrentUser(), api).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<MostCalledApiModel>), 200)]
		public IActionResult GetMostCalledApi()
		{
			try
			{
				return Ok(new Core.Support.Handlers.ApiCalls.GetMostCalledApiHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}