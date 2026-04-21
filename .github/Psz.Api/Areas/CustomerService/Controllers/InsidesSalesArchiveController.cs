using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.InsideSalesChecksArchive;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class InsidesSalesArchiveController: ControllerBase
	{
		private const string MODULE = "Customer Service | Inside Sales Archive";
		private IInsideSalesChecksArchive _insideSalesChecksArchive;
		public InsidesSalesArchiveController(IInsideSalesChecksArchive insideSalesChecksArchive)
		{
			this._insideSalesChecksArchive = insideSalesChecksArchive;
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<GetInsideSalesHistoryResponseModel>), 200)]
		public IActionResult GetSalesHistoryList(GetInsideSalesHistoryRequestModel data)
		{
			try
			{
				return Ok(_insideSalesChecksArchive.GetInsideSalesChecksHistories(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SendBack(int id)
		{
			try
			{
				return Ok(this._insideSalesChecksArchive.SendInstructionBack(this.GetCurrentUser(), id));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetSalesHistoryList_XLS(GetInsideSalesHistoryRequestModel data)
		{
			try
			{
				var results = _insideSalesChecksArchive.GetInsideSalesChecksHistories_XLS(this.GetCurrentUser(), data);
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
	}
}
