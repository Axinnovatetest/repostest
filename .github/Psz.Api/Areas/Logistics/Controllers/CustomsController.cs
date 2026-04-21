using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistics")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CustomsController: Controller
	{
		private const string MODULE = "Logistics | Customs";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Logistics.Models.Statistics.CustomsModel.AusfuhrModel>>), 200)]
		public IActionResult GetAusfuhr(Psz.Core.Logistics.Models.Statistics.CustomsModel.AusfuhrRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.GetAusfuhrHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Logistics.Models.Statistics.CustomsModel.StammdatenkontrolleWareneingangeModel>>), 200)]
		public IActionResult GetStammdatenkontrolleWareneingange(Psz.Core.Logistics.Models.Statistics.CustomsModel.StammdatenkontrolleWareneingangeRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.StammdatenkontrolleWareneingangeHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Logistics.Models.Statistics.CustomsModel.RMDCZModel>>), 200)]
		public IActionResult GetRMDCZ(Psz.Core.Logistics.Models.Statistics.CustomsModel.RMDCZRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.GetRMDCZHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Logistics.Models.Statistics.CustomsModel.EinfuhrModel>>), 200)]
		public IActionResult GetEinfuhr(Psz.Core.Logistics.Models.Statistics.CustomsModel.EinfuhrRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.GetEinfuhrHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Logistics.Models.Statistics.CustomsModel.LieferantenGruppeModel>>), 200)]
		public IActionResult GetLieferantenGruppe()
		{
			try
			{
				return Ok(new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.LieferantenGruppeHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult StammdatenkontrolleWareneingangeXLS(System.DateTime fromdate, System.DateTime todate, string gruppe)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.CustomsHandle.StammdatenkontrolleWareneingangeExcelHandler(this.GetCurrentUser(), fromdate, todate, gruppe).Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}
