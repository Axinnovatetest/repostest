using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Handlers.Statistics;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StatisticsController: ControllerBase
	{
		private const string MODULE = "CustomerService";
		private readonly ICrpStatisticsService _crpStatisticsService;

		public StatisticsController(ICrpStatisticsService crpStatisticsService)
		{
			_crpStatisticsService = crpStatisticsService;
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLagerBestandFG_CRP()
		{
			try
			{
				var results = _crpStatisticsService.GetLagerBestandFG_CRPExcel(this.GetCurrentUser());
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"LagerBestand FG-CRP-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<OrderProcessingSearchLogsModel>), 200)]
		public IActionResult GetOrderProcessingLogs(OPSearchLogsModel data)
		{
			try
			{
				var results = _crpStatisticsService.GetOrderProcessingLogs(this.GetCurrentUser(), data);

				return Ok(results);

			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<System.Collections.Generic.List<string>>), 200)]
		public IActionResult getAutoCompliteOP(AutoCompliteOP data)
		{
			try
			{
				var results = _crpStatisticsService.AutoComplete(this.GetCurrentUser(), data.columnValue, data.SearchValue);

				return Ok(results);

			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadExcelOrderProcessingLogs(OPSearchLogsModel data)
		{
			try
			{
				var results = _crpStatisticsService.DownloadExcelOrderProcessingLogs(this.GetCurrentUser(), data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Order Processing Logs-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CRPAuswertungRahmenFGArtikel()
		{
			try
			{
				var results = _crpStatisticsService.GetCRPAuswertungRahmenFGArtikel(this.GetCurrentUser());
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"CRP-Auswertung Rahmen FG Artikel-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		#region CRP Dashboard
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CRP.Models.Dashboard.CRPDashboardStatisticsModel>), 200)]
		public IActionResult GetDashboardStatistics(Core.CRP.Models.Dashboard.CRPDashboardRequestModel data)
		{
			try
			{
				var response = _crpStatisticsService.GetDashboardStatistics(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CRP.Models.Dashboard.CRPDashboardKenzahllenModel>), 200)]
		public IActionResult GetDashboardKenzahllen(int year)
		{
			try
			{
				var response = _crpStatisticsService.GetDashboardKenzahllen(this.GetCurrentUser(), year);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetDashboardStockFG()
		{
			try
			{
				var response = _crpStatisticsService.GetDashboardStockFG(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult UpdateSerieFaProdTimeCosts()
		{
			try
			{
				var response = _crpStatisticsService.UpdateSerieFaProdTimeCosts(this.GetCurrentUser());
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