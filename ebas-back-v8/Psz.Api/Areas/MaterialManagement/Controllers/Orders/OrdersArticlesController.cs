using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrdersArticlesController: ControllerBase
	{
		private const string MODULE = "Material Management | Orders";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Psz.Core.MaterialManagement.Orders.Models.DashBoard.ArticleAndFaultyWeek>>), 200)]
		public async Task<IActionResult> GetArticlesInFasWithNeededQuantities(Psz.Core.MaterialManagement.Orders.Models.DashBoard.FaultyArticlesInOpenFasRequestModel data)
		{
			try
			{
				if(ModelState.IsValid)
				{
					var response = await new Core.MaterialManagement.Orders.Handlers.DashBoard.GetFaultyArticlesInOpenFaByWeekHandler(this.GetCurrentUser(false), data)
									.Handle();
					return Ok(response);
				}
				return Ok("Invalid data Input");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<IPaginatedResponseModel<Psz.Core.MaterialManagement.Orders.Models.DashBoard.GetArticleInFaResponseModel>>), 200)]
		public IActionResult GetArticlesInFA(Core.MaterialManagement.Orders.Models.DashBoard.GetArticleInFaRequestModel data)
		{
			try
			{
				if(ModelState.IsValid)
				{
					var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetArticlesInFAHandler(this.GetCurrentUser(false), data)
									.Handle();
					return Ok(response);
				}
				return Ok("Invalid data Input");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetLagersListForOrders()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetLagersForOrdersHandler(this.GetCurrentUser(false))
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.NeededAndOrderedQunatityWithStockResponseModel>>), 200)]
		public IActionResult GetNeedAndOrders(Core.MaterialManagement.Orders.Models.DashBoard.NeededQuantityOrderedQuantityRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.NeededQuantityOrderedQuantityHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//FaultyOrdersAndFas
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersAndFasResponseModel>>), 200)]
		public IActionResult GetFaultyOrdersAndFasCount(Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersAndFasRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.FaultyFaOrdersCountsHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<SearchFaultyArticleByArticlenummerResponseModel>>), 200)]
		public IActionResult SearchNummer(Psz.Core.MaterialManagement.Orders.Models.DashBoard.SearchFaultyArticleByArticlenummerRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.SearchNummerHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersResponseModel>>), 200)]
		public IActionResult GetFaultyOrders(Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetFaultyOrdersHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersResponseModel>>), 200)]
		public IActionResult GetNotNeededOrders(Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrdersRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetNotNeededOrdersHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.FaultyOrdersResponseModel>>), 200)]
		public IActionResult GetNotNeededOrdersArticle(Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrdersArticleRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetNotNeededOrdersArticleHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.GetFaultyFasResponseModel>>), 200)]
		public IActionResult GetFaultyFas(Core.MaterialManagement.Orders.Models.DashBoard.GetFaultyFasRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetFaultyFasHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.GetFasInTimeSpanResponseModel>>), 200)]
		public IActionResult GetFasInTimeSPan(Core.MaterialManagement.Orders.Models.DashBoard.GetFasInTimeSpanRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetFasInTimeSpanHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.GetOrdersInTimeSpanResponseModel>>), 200)]
		public IActionResult GetOrdersInTimeSPan(Core.MaterialManagement.Orders.Models.DashBoard.GetOrdersInTimeSpanRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetOrdersInTimeSpanHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.GetArtikelstamm_KlassifizierungTrimmedRequestModel>>), 200)]
		public IActionResult GetArticlesFamilies(Core.MaterialManagement.Orders.Models.DashBoard.GetArtikelstamm_KlassifizierungTrimmedRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.DashBoard.GetArtikelstamm_KlassifizierungHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.GetUserHallAndCountryResponseModel>>), 200)]
		public IActionResult GetUserHallAndCountry(Core.MaterialManagement.Orders.Models.DashBoard.GetUserHallAndCountryRequestModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.Orders.Models.DashBoard.GetUserHallAndCountryHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrdersAllResponseModel>>), 200)]
		public IActionResult GetNotNeededOrdersAll(Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrdersAllRequestModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.Orders.Handlers.DashBoard.GetNotNeededOrdersAllHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrderArticleDetailsResponseModel>>), 200)]
		public IActionResult GetNotNeededOrderArticleDetails(Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrderArticleDetailsRequestModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.Orders.Handlers.DashBoard.GetNotNeededOrderArticleDetailsHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrderDetailsAllResponseModel>>), 200)]
		public IActionResult GetNotNeededOrderDetailsAll(Core.MaterialManagement.Orders.Models.DashBoard.NotNeededOrderDetailsAllRequestModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.Orders.Handlers.DashBoard.GetNotNeededOrderDetailsAllHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		// - Bedarf/Bestand Analyse
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.DashBoard.NeedStockPerTypeResponseModel>), 200)]
		public IActionResult GetCTSNeedAnalyse(Core.MaterialManagement.Orders.Models.DashBoard.NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Orders.Handlers.DashBoard.NeededQuantityOrderedQuantityAnalysisHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetCTSNeedAnalyse_XLS(Core.MaterialManagement.Orders.Models.DashBoard.NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			try
			{
				var results = new Core.MaterialManagement.Orders.Handlers.DashBoard.NeededQuantityOrderedQuantityAnalysisHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"NeedAnalyse-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.DashBoard.NeedStockSummaryItemModel>>), 200)]
		public IActionResult GetCTSNeedAnalyseSummary(Core.MaterialManagement.Orders.Models.DashBoard.NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			try
			{
				return Ok(new Core.MaterialManagement.Orders.Handlers.DashBoard.NeededQuantityOrderedQuantityAnalysisSummaryHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetCTSNeedStockMaterial_XLS(Core.MaterialManagement.Orders.Models.DashBoard.NeededQuantityOrderedQuantityAnalysisRequestModel data)
		{
			try
			{
				var results = new Core.MaterialManagement.Orders.Handlers.DashBoard.NeedStockMaterialHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"NeedStockMaterial-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
	}

}
