using Infrastructure.Services.BackgroundWorker;
using iText.StyledXmlParser.Jsoup.Select;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Apps.EDI.Models.OrderResponse;
using Psz.Core.ManagementOverview.CTS.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Api.Areas.ManagementOverview.Controllers
{
	[Authorize]
	[Area("ManagementOverview")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StatisticsController: ControllerBase
	{
		private const string MODULE = "Management Overview | Statistics";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Administration.Models.Users.GetResponseModel>>), 200)]
		public IActionResult GetDashboard()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.PeriodicSalesHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<InvoiceAmountResponseModel>), 200)]
		public IActionResult GetInvoiceAmount()
		{
			try
			{
				var results = new Core.ManagementOverview.CTS.Handlers.GetInvoiceAmountHandler(this.GetCurrentUser())
					 .Handle();
				return Ok(results);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#region DaysOff
		[HttpGet]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult FillDaysOff()
		{
			try
			{
				List<Infrastructure.Data.Entities.Tables.MGO.DayOff> DbDaysOff =
					new List<Infrastructure.Data.Entities.Tables.MGO.DayOff>();
				DaysOff daysOff = DaysOffService.SyncDaysOff();
				foreach(DayOff dayoff in daysOff.feiertage)
				{
					if(dayoff.date.Year == System.DateTime.Now.Year)
					{
						DbDaysOff.Add(new Infrastructure.Data.Entities.Tables.MGO.DayOff
						{
							Date = dayoff.date,
							BY = dayoff.getDay(dayoff.by),
							BB = dayoff.getDay(dayoff.bb),
							BE = dayoff.getDay(dayoff.be),
							BW = dayoff.getDay(dayoff.bw),
							HB = dayoff.getDay(dayoff.hb),
							AllStates = dayoff.getDay(dayoff.all_states),
							HE = dayoff.getDay(dayoff.he),
							HH = dayoff.getDay(dayoff.hh),
							MV = dayoff.getDay(dayoff.mv),
							NI = dayoff.getDay(dayoff.ni),
							NW = dayoff.getDay(dayoff.nw),
							RP = dayoff.getDay(dayoff.rp),
							SH = dayoff.getDay(dayoff.sh),
							SL = dayoff.getDay(dayoff.sl),
							SN = dayoff.getDay(dayoff.sn),
							ST = dayoff.getDay(dayoff.st),
							TH = dayoff.getDay(dayoff.th),
							Name = dayoff.fname
						});
					}
				};
				Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.SyncDaysOff(DbDaysOff);
				return Ok();

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<DayOffResponseModel>), 200)]
		public IActionResult GetDaysOff()
		{
			try
			{

				var results = new Core.ManagementOverview.CTS.Handlers.GetDaysOffHandler(this.GetCurrentUser())
					 .Handle();
				return Ok(results);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		#region ReasonChangeCommittee

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult UpdateGrunde(Core.ManagementOverview.Statistics.Models.UpdateGrundeRequestModel Request)
		{
			try
			{
				var response = new Core.ManagementOverview.Statistics.Handlers.UpdateGrundeHandler(this.GetCurrentUser(), Request)
							  .Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Statistics.Models.GrundeResponseModel>>), 200)]
		[AllowAnonymous]
		public IActionResult GetGrunde()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.GetGrundeHandler(this.GetCurrentUser())
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Statistics.Models.ReasonChangeCommitteeResponseModel>>), 200)]
		[AllowAnonymous]
		public IActionResult GetReasonChangeCommitteeStats(Core.ManagementOverview.Statistics.Models.ReasonChangeCommitteeRequestModel Request)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.GetReasonChangeCommitteeStatsHandler(this.GetCurrentUser(), Request)
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[AllowAnonymous]
		public IActionResult GetReasonChangeCommitteeStats_XLS(Core.ManagementOverview.Statistics.Models.ReasonChangeCommitteeRequestModel Request)
		{
			try
			{
				var results = new Core.ManagementOverview.Statistics.Handlers.GetReasonChangeCommitteeStatsHandler(this.GetCurrentUser(), Request)
							  .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"ReasonChangeCommittee-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion


		#region GetSalesInjection
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Statistics.Models.SalesInjectionStatsResponseModel>>), 200)]
		[AllowAnonymous]
		public IActionResult GetSalesInjectionStats(Core.ManagementOverview.Statistics.Models.SalesInjectionStatsRequestModel Request)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.SalesInjectionStatsHandler(this.GetCurrentUser(), Request)
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[AllowAnonymous]
		public IActionResult GetSalesInjectionStats_XLS(Core.ManagementOverview.Statistics.Models.SalesInjectionStatsRequestModel data)
		{
			try
			{
				var results = new Core.ManagementOverview.Statistics.Handlers.SalesInjectionStatsHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"SalesInjection-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[SwaggerOperation(Tags = new[] { MODULE })]
		[AllowAnonymous]
		public IActionResult GetSalesInjectionStats_PDF(Core.ManagementOverview.Statistics.Models.SalesInjectionStatsRequestModel data)
		{
			try
			{
				var results = new Core.ManagementOverview.Statistics.Handlers.SalesInjectionStatsHandler(this.GetCurrentUser(), data)
					   .GetPDFXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/pdf", $"SalesInjection-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.pdf");
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
		#endregion

		#region PSZ FG TN


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetPSZFGTNStats_XLS(Core.ManagementOverview.Statistics.Models.PSZFGTNRequestModel Request)
		{
			try
			{
				var results = new Core.ManagementOverview.Statistics.Handlers.PSZFGTNStatsHandler(this.GetCurrentUser(), Request)
							  .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"PSZFGTN-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Statistics.Models.PSZFGTNResponseModel>>), 200)]
		public IActionResult GetPSZFGTNStats(Core.ManagementOverview.Statistics.Models.PSZFGTNRequestModel Request)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.PSZFGTNStatsHandler(this.GetCurrentUser(), Request)
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult UpdateArticle(Core.ManagementOverview.Statistics.Models.UpdateArticleRequestModel Request)
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.UpdateArticleHandler(this.GetCurrentUser(), Request)
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Statistics.Models.ArticleHistoryResponseModel>>), 200)]
		public IActionResult GetArticleHistory()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.GetArticleHistoryHandler(this.GetCurrentUser())
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<int, int>>), 200)]
		public IActionResult GetDelKupfer()
		{
			try
			{
				return Ok(new Core.ManagementOverview.Statistics.Handlers.GetDelKupferHandler(this.GetCurrentUser())
							  .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[AllowAnonymous]
		public IActionResult GetWeekSummary_XLS()
		{
			try
			{
				var results = new Core.ManagementOverview.Statistics.Handlers.GetWeekSummaryHandler(this.GetCurrentUser())
							  .Handle();
				if(results != null && results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}
