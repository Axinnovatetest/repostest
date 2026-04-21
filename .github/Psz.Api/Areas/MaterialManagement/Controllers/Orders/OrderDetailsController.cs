using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{

	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrderDetailsController: ControllerBase
	{
		private const string MODULE = "Material Management | Orders";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.GetResponseModel>>), 200)]
		public IActionResult Get(Core.MaterialManagement.Orders.Models.OrderDetails.GetRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.UpdateOrderSupplierResponseModel>), 200)]
		public IActionResult UpdateOrderSupplier(Core.MaterialManagement.Orders.Models.OrderDetails.UpdateOrderSupplierRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.UpdateOrderSupplierHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.UpdateOrderDetailsResponseModel>), 200)]
		public IActionResult UpdateOrderDetails(Core.MaterialManagement.Orders.Models.OrderDetails.UpdateOrderDetailsRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.UpdateOrderDetailsHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.SupplierArticleResponseModel>>), 200)]
		public IActionResult GetSupplierArticles(Core.MaterialManagement.Orders.Models.OrderDetails.SupplierArticleRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetSupplierArticlesHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{

				return this.HandleException(e, data);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.SupplierArticlesFilterResponseModel>>), 200)]
		public IActionResult GetSupplierArticlesFilter(Core.MaterialManagement.Orders.Models.OrderDetails.SupplierArticlesFilterRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetSupplierArticleFilterHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.GetArticleInformationResponseModel>), 200)]
		public IActionResult GetArticleInformation(Core.MaterialManagement.Orders.Models.OrderDetails.GetArticleInformationRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetArticleInformationHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.GetArticleInformationResponseModel>), 200)]
		public IActionResult GetPositionInformation(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetPositionInformationHandler(this.GetCurrentUser(false), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleInformationResponseModel>), 200)]
		public IActionResult UpdateArticleInformation(Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleInformationRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.UpdateArticleInformationHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticleConfirmationDate(Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleConfirmationDateRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.UpdateArticleConfirmationDateHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.SupplierResponseModel>), 200)]
		public IActionResult GetSupplier(Core.MaterialManagement.Orders.Models.OrderDetails.SupplierRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.SupplierHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.DeleteArticleRequestModel>), 200)]
		public IActionResult DeleteArticle(Core.MaterialManagement.Orders.Models.OrderDetails.DeleteArticleRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.DeleteArticleHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult ErledigtUpdate(int beteltteAttikel)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.ErledigtUpdateHandler(this.GetCurrentUser(false), beteltteAttikel)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, beteltteAttikel);
				throw;

			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetPositions(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetPositionsHandler(this.GetCurrentUser(false), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleInformationResponseModel>), 200)]
		public IActionResult SplitPosition(Core.MaterialManagement.Orders.Models.OrderDetails.UpdateArticleInformationRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.SplitPositionHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetRightPrice(Core.MaterialManagement.Orders.Models.OrderDetails.PriceRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderDetails.GetRightPriceHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.MaterialManagement.Models.InfoRahmennummerModel>>), 200)]
		public IActionResult GetAvailableRahmens(Psz.Core.MaterialManagement.Models.InfoRahmenRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails.GetAvailableRahmensHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e, model);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<int, int>>), 200)]
		public IActionResult GetProjectNrs(string text)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.GetProjectNrsHandler(this.GetCurrentUser(), text).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<int, int>>), 200)]
		public IActionResult GetVorfailNrs(string text)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.GetVorfailNrsHandler(this.GetCurrentUser(), text).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<string, string>>), 200)]
		public IActionResult GetDocumentsNrs(string text)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.GetDocumentsNrsHandler(this.GetCurrentUser(), text).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<int, string>>), 200)]
		public IActionResult GetSuppliers(string text)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.GetSuppliersHandler(this.GetCurrentUser(), text).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Models.RahmenListResponseModel>), 200)]
		public IActionResult SearchRahmen(Psz.Core.MaterialManagement.Models.RahmenListRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Rahmen.SearchRahmenHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}