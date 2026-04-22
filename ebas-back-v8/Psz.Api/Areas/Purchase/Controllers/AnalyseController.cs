using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Apps.Purchase.Models.Analyse;
using Psz.Core.Common.Models;
using Psz.Core.Purchase.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Purchase.Controllers
{
	[Authorize]
	[Area("Purchase")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class AnalyseController: ControllerBase
	{
		private readonly IPRSService _pRSService;

		public AnalyseController(IPRSService pRSService)
		{
			this._pRSService = pRSService;
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.Analyse.CalculateTurnoverResponseModel>), 200)]
		public IActionResult Turnover(Core.Apps.Purchase.Models.Analyse.CalculateTurnoverRequestModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Analyse.CalculateTurnover(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		#region Stock Warning
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Purchase.Models.StockWarnings.ArticlesResponseModel>), 200)]
		public IActionResult GetArticles(Core.Purchase.Models.StockWarnings.ArticlesRequestModel data)
		{
			try
			{
				var response = _pRSService.GetArticles(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ActivatePRSStockWarningsAgent()
		{
			try
			{
				var response = _pRSService.ForceComputeStockWarningsAgent(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<DateTime>), 200)]
		public IActionResult GetLastPRSStockWarningsAgentExecutionTime()
		{
			try
			{
				var response = _pRSService.GetLastComputeAgentExecutionTime(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Purchase.Models.StockWarnings.StockWarningsResponseModel>), 200)]
		public IActionResult GetStockWarnings(Core.Purchase.Models.StockWarnings.StockWarningsRequesteModel data)
		{
			try
			{
				var response = _pRSService.GetStockWarnings(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCoutries()
		{
			try
			{
				var response = _pRSService.GetCountries(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUnits(int countryId)
		{
			try
			{
				var response = _pRSService.GetUnits(this.GetCurrentUser(), countryId);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.StcoWarningsNeedsInOtherPlantsModel>>), 200)]
		public IActionResult GetNeedsInOtherPlants(Core.Purchase.Models.StockWarnings.ArtikelUnitRequestModel data)
		{
			try
			{
				var response = _pRSService.GetNeedsInOtherPlants(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.StockWarningsFaViewModel>>), 200)]
		public IActionResult GetFaView(Core.Purchase.Models.StockWarnings.ArtikelUnitRequestModel data)
		{
			try
			{
				var response = _pRSService.GetFaView(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.StockWarningsPoViewModel>>), 200)]
		public IActionResult GetPoView(Core.Purchase.Models.StockWarnings.ArtikelUnitRequestModel data)
		{
			try
			{
				var response = _pRSService.GetPoView(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.StockWarningsLogsModel>>), 200)]
		public IActionResult GetStockWarningsAgentLogs()
		{
			try
			{
				var response = _pRSService.GetStockWarningsAgentLogs(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Purchase.Models.StockWarnings.FaultyOrdersResponseModel>), 200)]
		public IActionResult GetFaultyOrders(Core.Purchase.Models.StockWarnings.FaultyRequestModel data)
		{
			try
			{
				var response = _pRSService.GetFaultyOrders(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Purchase.Models.StockWarnings.FaultyFasResponseModel>), 200)]
		public IActionResult GetFaultyFas(Core.Purchase.Models.StockWarnings.FaultyRequestModel data)
		{
			try
			{
				var response = _pRSService.GetFaultyFas(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.StockWarningsPoViewModel>>), 200)]
		public IActionResult GetUnconfirmedOrders(Core.Purchase.Models.StockWarnings.ArtikelUnitRequestModel data)
		{
			try
			{
				var response = _pRSService.GetUnconfirmedOrders(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Purchase.Models.StockWarnings.ExtraOrdersNeedsInOtherPlantsRequestModel>>), 200)]
		public IActionResult GetExtraOrdersNeedsInOtherPlants(Core.Purchase.Models.StockWarnings.ExtraOrdersNeedsInOtherPlantsRequestModel data)
		{
			try
			{
				var response = _pRSService.GetExtraOrdersNeedsInOtherPlants(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetStockWarningAuswertungExcel(Core.Purchase.Models.StockWarnings.StockWarningAuswertungRequestModel data)
		{
			try
			{
				var results = _pRSService.GetStockWarningAuswertungExcel(this.GetCurrentUser(), data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Stock warning auswertung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetExtraOrdersAuswertungExcel(Core.Purchase.Models.StockWarnings.ExtraOrdersNeedsInOtherPlantsRequestModel data)
		{
			try
			{
				var results = _pRSService.GetExtraOrdersAuswertungExcel(this.GetCurrentUser(), data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Extra orders auswertung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		#endregion

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(ResponseModel<List<int>>), 200)]
		public IActionResult GetWeeksNumberByYear(WeekRequestModel request)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Analyse.GenerateWeeks(request));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}