using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	using Psz.Core.CustomerService.Interfaces;
	using Psz.Core.CustomerService.Models.InsideSalesChecks;
	using System.Collections.Generic;
	using static Psz.Core.CustomerService.Models.InsideSalesCustomerSummary.InsideSalesCustomerSummaryModel;

	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class InsideSalesController: ControllerBase
	{
		private const string MODULE = "Customer Service | Inside Sales";
		private IInsideSalesChecks _insideSalesChecks;
		private IInsideSalesTotalDemandPlanning _insideSalesTotalDemandPlanning;
		public InsideSalesController(IInsideSalesChecks insideSalesChecks, IInsideSalesTotalDemandPlanning insideSalesTotalDemandPlanning)
		{
			this._insideSalesChecks = insideSalesChecks;
			this._insideSalesTotalDemandPlanning = insideSalesTotalDemandPlanning;
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCheck(InsideSalesChecksUpdateRequestModel data)
		{
			try
			{
				return Ok(this._insideSalesChecks.UpdateInstructions(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data); // - logger to save context in case of exception
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<SearchInsideSaleResponseModel>), 200)]
		public IActionResult Get(InsideSalesChecksSearchRequestModel data)
		{
			try
			{
				return Ok(this._insideSalesChecks.GetInsideSalesChecks(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data); // - logger to save context in case of exception
			}
		}


		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]

		//public IActionResult UpdateComment(UpdateCommentRequestModel data)
		//{
		//	try
		//	{
		//		return Ok(this._insideSalesChecks.UpdateComment(this.GetCurrentUser(), data));

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, data);
		//	}
		//}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<GetCustomerSummaryResponseModel>), 200)]
		public IActionResult GetCustomerSummaryList(GetCustomerSummaryRequestModel data)
		{
			try
			{
				return Ok(this._insideSalesTotalDemandPlanning.GetCustomerSummary(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data); // - logger to save context in case of exception
			}


		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetCustomerSummaryList_XLS(GetCustomerSummaryRequestModel data)
		{
			try
			{
				var results = _insideSalesTotalDemandPlanning.GetCustomerSummary_XLS(this.GetCurrentUser(), data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetNextNWeeks(int n)
		{
			try
			{
				return Ok(this._insideSalesTotalDemandPlanning.GetNextNWeeks(this.GetCurrentUser(), n));
			} catch(Exception e)
			{
				this.HandleException(e, n);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<InsideSalesChecksUpdateLogResponseModel>), 200)]
		public IActionResult GetLogs(InsideSalesChecksUpdateLogRequestModel data)
		{
			try
			{
				return Ok(this._insideSalesChecks.GetLogs(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				return this.HandleException(e, data); // - logger to save context in case of exception
			}
		}
	}
}
