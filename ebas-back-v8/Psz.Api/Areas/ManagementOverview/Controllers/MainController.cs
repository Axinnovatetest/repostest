using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.ManagementOverview.CTS.Handlers;
using Psz.Core.ManagementOverview.CTS.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.ManagementOverview.Controllers
{
	[Authorize]
	[Area("ManagementOverview")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class MainController: ControllerBase
	{
		private const string MODULE = "Management Overview | Main";

		#region CTS
		private const string CTS = "";
		/// <summary>
		/// Returns list of all users allowed to view MGO dashboard
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Administration.Models.Users.GetResponseModel>>), 200)]
		public IActionResult GetDashboard()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Administration.Handlers.Users.GetAllHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCTSDashboardCustomers()
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardCustomersHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCTSDashboardArticlesByCustomer(Core.ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardArticlesByCustomerHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.DashboardResponseModel>), 200)]
		public IActionResult GetCTSDashboard(Core.ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.CTS.Models.CTSDashboardItemModel>>), 200)]
		public IActionResult GetCTSDashboardByCustomer(Core.ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardByCustomerHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetCTSDashboardByCustomer_XLS(Core.ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardByCustomerHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"CTSDashboard-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.DashboardSummaryResponseModel>), 200)]
		public IActionResult GetCTSDashboardSummaryByCustomer(Core.ManagementOverview.CTS.Models.DashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardSummeryByCustomerHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.DashboardSummaryTopResponseModel>), 200)]
		public IActionResult GetCTSDashboardSummaryByCustomerTop(Core.ManagementOverview.CTS.Models.DashboardTopRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardSummaryTopHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.CTS.Models.CTSDashboardItemKwModel>>), 200)]
		public IActionResult GetCTSDashboardByCustomerKw(Core.ManagementOverview.CTS.Models.DashboardKwRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardByCustomerKwHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetCTSDashboardByCustomerKw_XLS(Core.ManagementOverview.CTS.Models.DashboardKwRequestModel data)
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetCTSDashboardByCustomerKwHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"CTSDashboardKw-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}


		// - Article VK
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.CTS.Models.ArticleVKMarginResponseModel>>), 200)]
		public IActionResult GetCTSArticleVK(int margin)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSArticleVKMarginHandler(this.GetCurrentUser(), margin)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, margin);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetCTSArticleVK_XLS(int margin)
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetCTSArticleVKMarginHandler(this.GetCurrentUser(), margin)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"ArticleVK-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, margin);
			}
		}

		// - Production Reschedulings
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.ProductionReschedulingResponseModel>), 200)]
		public IActionResult GetCTSProductionRescheduling(Core.ManagementOverview.CTS.Models.ProductionReschedulingRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetProductionReschedulingHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data.fzKwCount);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetCTSProductionRescheduling_XLS(int? fzKwCount, Core.ManagementOverview.CTS.Enums.CTSDashboard productType)
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetProductionReschedulingHandler(this.GetCurrentUser(), new Core.ManagementOverview.CTS.Models.ProductionReschedulingRequestModel { productType = Core.ManagementOverview.CTS.Enums.CTSDashboard.InFrozen })
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"ProductionRescheduling-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, fzKwCount);
			}
		}

		// - Bedarf Analyse
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.BedarfBestandResponseModel>), 200)]
		public IActionResult GetCTSBedarfAnalyse(Core.ManagementOverview.CTS.Models.BedarfBestandRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetCTSAnalyseBedarfHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.DashboardRefreshResponseModel>), 200)]
		public IActionResult RefreshData(Core.ManagementOverview.CTS.Models.RefreshDashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.RefreshDashboardHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.CTS.Models.DashboardRefreshResponseModel>), 200)]
		public IActionResult CheckLastRefreshData(Core.ManagementOverview.CTS.Models.RefreshDashboardRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.RefreshDashboardHandler(this.GetCurrentUser(), data)
				   .HandleCheck());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetCTSBedarfAnalyse_XLS(Core.ManagementOverview.CTS.Models.BedarfBestandRequestModel data)
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetCTSAnalyseBedarfHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"BedarfAnalyse-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.CTS.Models.GetProductionOrderChangeHistoryResponseModel>>), 200)]
		public IActionResult GetProductionOrderChangeHistory(Core.ManagementOverview.CTS.Models.GetProductionOrderChangeHistoryRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetProductionOrderChangeHistoryHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.CTS.Models.FaChangesWeekYearHoursLeftResponseModel>>), 200)]
		public IActionResult GetProductionOrderChangeHistoryChart(Core.ManagementOverview.CTS.Models.GetProductionOrderChangeHistoryChartRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetProductionOrderChangeHistoryChartHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}		
        
        [HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<GetProductionOrderChangeHistoryDetailResponseModel>>), 200)]
		public IActionResult GetProductionOrderChangeHistoryDetails(GetProductionOrderChangeHistoryDetailRequestModel data)
		{
			try
			{
				return Ok(new Core.ManagementOverview.CTS.Handlers.GetProductionOrderChangeHistoryDetailsHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE + CTS })]
		public IActionResult GetProductionOrderChangeHistoryDetails_XLS(GetProductionOrderChangeHistoryDetailRequestModel data)
		{
			try
			{
				var results = new GetProductionOrderChangeHistoryDetailsHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Production-Orders-History-Details-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		#endregion CTS
	}
}
