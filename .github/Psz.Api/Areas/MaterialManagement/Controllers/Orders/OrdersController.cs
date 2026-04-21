using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrdersController: ControllerBase
	{
		private const string MODULE = "Material Management | Orders";
		private readonly Core.MaterialManagement.Interfaces.IOrderService _orderService;

		public OrdersController(Core.MaterialManagement.Interfaces.IOrderService orderService)
		{
			_orderService = orderService;
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Orders.GetResponseModel>>), 200)]
		public IActionResult Get(Core.MaterialManagement.Orders.Models.Orders.GetRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.GetOrderNrReponseModel>>), 200)]
		public IActionResult GetOrderNR(Core.MaterialManagement.Orders.Models.Orders.GetOrderNrRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetOrderNrHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.GetOrderNrReponseModel>>), 200)]
		public IActionResult GetProjectNr(Core.MaterialManagement.Orders.Models.Orders.GetOrderNrRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetProjectNrHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.GetByIdResponseModel>), 200)]
		public IActionResult GetById(Core.MaterialManagement.Orders.Models.Orders.GetByIdRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetByIdHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.AddOrderResponseModel>), 200)]
		public IActionResult AddOrder(Core.MaterialManagement.Orders.Models.Orders.AddOrderRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.AddOrderHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.GetByIdResponseModel>), 200)]
		public IActionResult DeleteById(Core.MaterialManagement.Orders.Models.Orders.DeleteByIdRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.DeleteByIdHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.GetByIdResponseModel>), 200)]
		public IActionResult QuickPo(Core.MaterialManagement.Orders.Models.Orders.QuickPORequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.QuickPoHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.TypesResponseModel>>), 200)]
		public IActionResult GetOrderTypes()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetTypesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.GetResponseModel>>), 200)]
		public IActionResult Search(Core.MaterialManagement.Orders.Models.Orders.SearchRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetForSearchHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleErledigt(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.ErledigtUpdateHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.GetArtikelNrReponseModel>>), 200)]
		public IActionResult GetArtikelNr(Core.MaterialManagement.Orders.Models.Orders.GetArtikelNrRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetAtikelNrHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Orders.LogResponseModel>>), 200)]
		public IActionResult GetFullLog(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Orders.GetFullLogHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult TogglePurchaseProject(int id)
		{
			try
			{
				return Ok(this._orderService.TogglePurchaseProject(this.GetCurrentUser(), id));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.OrderPrioViewResponseModel>), 200)]
		public IActionResult GetOrdersPrioView(Core.MaterialManagement.Orders.Models.Orders.GetRequestModel data)
		{
			try
			{
				var response = this._orderService.GetOrdersPrioView(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.OrderPrioViewResponseModel>), 200)]
		public IActionResult GetOrdersAnomalies(Core.MaterialManagement.Orders.Models.Orders.OrdersAnomaliesRequestModel data)
		{
			try
			{
				var response = this._orderService.GetOrdersAnomalies(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdatePositionConfirmedDateAndComment(Core.MaterialManagement.Orders.Models.Orders.ConfirmedDateAndCommentModel data)
		{
			try
			{
				var response = _orderService.UpdateConfirmedDateAndComment(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticlesForRahmenFilter(string searchText)
		{
			try
			{
				var response = _orderService.GetArticlesForRahmenFilter(this.GetCurrentUser(), searchText);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, searchText);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.ROHArticleRahmenNeedsResponseModel>), 200)]
		public IActionResult GetROHRahmenNeeds(int artikelNr)
		{
			try
			{
				var response = _orderService.GetROHRahmenNeeds(this.GetCurrentUser(), artikelNr);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, artikelNr);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Orders.ROHArticleRahmenNeedsResponseModel>), 200)]
		public IActionResult GetNeedsInRahmenSaleDetails(int artikelNr)
		{
			try
			{
				var response = _orderService.GetNeedsInRahmenSaleDetails(this.GetCurrentUser(), artikelNr);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, artikelNr);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AcitvateROH_RA_NeedsAgent()
		{
			try
			{
				var response = _orderService.AcitvateROH_RA_NeedsAgent(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<DateTime?>), 200)]
		public IActionResult GetROH_RA_NeedsAgent_LastExecution()
		{
			try
			{
				var response = _orderService.GetROH_RA_NeedsAgent_LastExecution(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<DateTime, string>>>), 200)]
		public IActionResult GetROH_RA_NeedsAgentLogs()
		{
			try
			{
				var response = _orderService.GetROH_RA_NeedsAgentLogs(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
