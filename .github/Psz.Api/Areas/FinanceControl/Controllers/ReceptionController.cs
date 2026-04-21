using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.FinanceControl.Controllers
{
	[Authorize]
	[Area("FinanceControl")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ReceptionController: ControllerBase
	{
		private const string MODULE = "FinanceControl";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]

		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Budget.Reception.UpdateModel>>), 200)]
		public IActionResult GetAll()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult Get(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.GetHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Edit(Core.FinanceControl.Models.Budget.Reception.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.UpdateHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.FinanceControl.Models.Budget.Reception.UpdateModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.AddHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Book(Core.FinanceControl.Models.Budget.Reception.BookModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.BookHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}

		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult BookService(Core.FinanceControl.Models.Budget.Reception.BookModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.BookServiceHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}

		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Reception.HistoryModel>>), 200)]
		public IActionResult GetHistory(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.GetHistoryHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}



		// - Search
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.SearchResponseModel<Core.FinanceControl.Models.Budget.Reception.GetMinimalModel>>), 200)]
		public IActionResult Search(Core.FinanceControl.Models.Budget.Reception.SearchRequestModel data)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult SearchNumbers(string query, bool inPrgressOnly, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchNumbersHandler(this.GetCurrentUser(), new Core.FinanceControl.Models.Budget.Reception.FilterRequestModel { Value = query, InProgressOnly = inPrgressOnly, OrderType = type }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult SearchOrderNumbers(string query, bool inPrgressOnly, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchOrderNsHandler(this.GetCurrentUser(), new Core.FinanceControl.Models.Budget.Reception.FilterRequestModel { Value = query, InProgressOnly = inPrgressOnly, OrderType = type }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult SearchSupplierNumbers(string query, bool inPrgressOnly, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchSupplierNsHandler(this.GetCurrentUser(), new Core.FinanceControl.Models.Budget.Reception.FilterRequestModel { Value = query, InProgressOnly = inPrgressOnly, OrderType = type }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult SearchProjectNumbers(string query, bool inPrgressOnly, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchProjectNsHandler(this.GetCurrentUser(), new Core.FinanceControl.Models.Budget.Reception.FilterRequestModel { Value = query, InProgressOnly = inPrgressOnly, OrderType = type }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.FinanceControl.Models.Budget.Reception.UpdateModel>), 200)]
		public IActionResult SearchArticleNumbers(string query, bool inPrgressOnly, int type)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.SearchArticleNrsHandler(this.GetCurrentUser(), new Core.FinanceControl.Models.Budget.Reception.FilterRequestModel { Value = query, InProgressOnly = inPrgressOnly, OrderType = type }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		// - Articles
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.FinanceControl.Models.Budget.Reception.Article.GetModel>>), 200)]
		public IActionResult GetArticles(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.Article.GetByReceptionHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetStorageLocations()
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.Article.GetStorageLocationsHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		// - 2022-06-27
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UnbookFull(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.UnbookFullHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Unbook(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.Budget.Reception.UnbookHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetProjectNrByBes(int id)
		{
			try
			{
				return Ok(new Core.FinanceControl.Handlers.GetBestellungHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
	}
}