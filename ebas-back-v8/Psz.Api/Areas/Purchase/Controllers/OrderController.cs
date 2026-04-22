using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Purchase.Controllers
{
	[Authorize]
	[Area("Purchase")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrderController: ControllerBase
	{
		private const string MODULE = "Purchase";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.Order.OrderModel>), 200)]
		public IActionResult GetOrder(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.Purchase.Handlers.Order.Get(id,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.OrderModel>>), 200)]
		public IActionResult ToggleDone(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Psz.Core.Apps.Purchase.Handlers.Order.ToggleDone(id, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.OrderModel>>), 200)]
		public IActionResult ToggleBooked(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Psz.Core.Apps.Purchase.Handlers.Order.ToggleBooked(id, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.OrderModel>>), 200)]
		public IActionResult ToggleItemDone(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Psz.Core.Apps.Purchase.Handlers.Order.ToggleItemDone(id, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult ArchiveOrder(int id)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.Order.Archive(id,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult DeleteOrder(int id)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.Order.Delete(id,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrders()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Psz.Core.Apps.Purchase.Handlers.Order.Get(null, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.Element.OrderItemModel>>), 200)]
		public IActionResult GetOrderElements(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.Element.OrderItemModel>>()
				{
					Success = true,
					Body = Core.Apps.Purchase.Handlers.Order.GetOrderItems(id)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.Order.Element.OrderItemModel>), 200)]
		public IActionResult GetElement(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<Core.Apps.Purchase.Models.Order.Element.OrderItemModel>()
				{
					Success = true,
					Body = Core.Apps.Purchase.Handlers.Order.GetOrderItem(id)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Inventory.Models.StorageLocation.StorageLocationModel>>), 200)]
		public IActionResult GetStorageLocations()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.StorageLocation.Get(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult SearchItems(string searchText)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.SearchItems(searchText,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateElement(Core.Apps.Purchase.Models.Order.UpdateItemModel data)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.Order.UpdateOrderItem(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateElementItem(Core.Apps.Purchase.Models.Order.UpdateElementItemModel data)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.Order.UpdateItemOfOrderItem(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult DeleteElement(Core.Apps.Purchase.Models.Order.Element.DeleteItemModel data)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.Order.DeleteOrderItem(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult SplitElement(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null /*|| !user.Access.Purchase.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return Ok(Core.Apps.Purchase.Handlers.Order.SplitElement(id));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult ChangeCustomer(Core.Apps.Purchase.Models.Order.ChangeCustomerModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.ChangeCustomer(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateGlobalData(Core.Apps.Purchase.Models.Order.UpdateGlobalDataModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.UpdateGlobalData(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult QuickCreate(Core.Apps.Purchase.Models.Order.QuickCreateModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.QuickCreate(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult QuickCreateItem(Core.Apps.Purchase.Models.Order.Element.QuickCreateItemModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.Element.QuickCreateItem(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null/* || !user.Access.EDI.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.Purchase.Handlers.Order.GetByCustomer(customerId, null, user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.CustomerModel>), 200)]
		public IActionResult GetOrderCustomer(int id)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.OrderProcessing.GetOrderCustomerHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.CustomerModel>), 200)]
		public IActionResult UpdateOrderCustomerUst(int id)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.OrderProcessing.UpdateOrderCustomerUstIdHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.Order.SearchResponseModel>), 200)]
		public IActionResult Search(Core.Apps.Purchase.Models.Order.SearchModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.Search(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult SearchProjectNumber(string searchText)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.SearchProjectNumber(searchText, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult SearchPrefailNumber(string searchText)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.SearchPrefailNumber(searchText, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult SearchDocumentNumber(string searchText)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.SearchDocumentNumber(searchText, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetMagicCustomers()
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.GetMagicCustomers(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult Confirm(Core.Apps.Purchase.Models.Order.UpdateGlobalDataModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Order.Confirm(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult MyCustomers()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserCustomers(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult MyCustomersForCreate()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserCustomersForCreate(this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult MyCustomers2ForCreate(string searchText)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserCustomers_2ForCreate(this.GetCurrentUser(), searchText));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult MyCustomers2(string searchText)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserCustomers_2(this.GetCurrentUser(), searchText));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		// BL: DeliveryNote
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.DeliveryNote.CreateResponseModel>), 200)]
		public IActionResult GetDeliveryNote(int id)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.GetDeliveryHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ValidateDeliveryNote2(Core.Apps.Purchase.Models.DeliveryNote.ValidateDeliveryNoteModel data)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.ValidateDeliveryNoteTransactionHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetShippingMethods()
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Order.GetShippingMethodsHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.DeliveryNote.GetModel>>), 200)]
		public IActionResult GetAllDeliveryNote()
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.DeliveryNote.GetModel>), 200)]
		public IActionResult GetDeliveryNoteById(int id)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.GetHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.DeliveryNote.GetModel>), 200)]
		public IActionResult GetDeliveryNoteItemsById(int id)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.GetItemsHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.DeliveryNote.DeliveryNoteHistoryModel>>), 200)]
		public IActionResult GetDeliveryNoteHistory(string project_nr, string type)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetDeliveryNoteHistoryHandler(this.GetCurrentUser(), project_nr, type).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.DeliveryNote.DeliveryNoteHistoryModel>>), 200)]
		public IActionResult GetABHistory(string project_nr)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetABHistoryHandler(this.GetCurrentUser(), project_nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.DeliveryNote.LSInvoiceResponseModel>>), 200)]
		public IActionResult GetLSInvoices(int nr)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetLSInvoicesHandler(this.GetCurrentUser(), nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>), 200)]
		public IActionResult GetDelieveryArtikel(Psz.Core.CustomerService.Models.DeliveryNote.GetArticleModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetArtikelHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AutocompleteArtikelnummer(string article)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.AutocompleteArtilkelnummerHandler(this.GetCurrentUser(), article).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetStorageLocationsList(string article)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetStorageLocationsListHandler(this.GetCurrentUser(), article).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<DateTime?, string>>>), 200)]
		public IActionResult GetKundenIndexList(int articleId)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.OrderProcessing.GetKundenIndexesByArticleHandler(this.GetCurrentUser(), articleId).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, articleId);
			}
		}
		// >>>> API Amna +++++++++++++
		#region >>>> Undo Validation LS   
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddLSValidatdatedPosition(Core.Apps.Purchase.Models.DeliveryNote.ItemModel data)
		{

			try
			{
				/*[LS Pos zu AB Pos] for undo and update*/
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.AddLSValidatdatedPositionHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.DeliveryNote.ItemModel>), 200)]
		public IActionResult GetDeliveryNoteSingleItem(int id)
		{
			try
			{
				return Ok(new Psz.Core.Apps.Purchase.Handlers.DeliveryNote.GetDeliveryNoteSingleItemHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDeliveryNoteSingleItem(int id)
		{
			try
			{
				return Ok(new Psz.Core.Apps.Purchase.Handlers.DeliveryNote.DeleteDeliveryNoteItemHandler(this.GetCurrentUser(), id, true).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLSValidatdatedPosition(Core.Apps.Purchase.Models.DeliveryNote.ItemModel data)
		{

			try
			{
				/*[LS Pos zu AB Pos] for undo and update*/
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.UpdateLSSingleItemHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLSValidatdatedPositionByQte(int id, int Qte)
		{

			try
			{
				/*[LS Pos zu AB Pos] for undo and update*/
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.UpdateLSSingleItemByQteHandler(this.GetCurrentUser(), id, Qte).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDeliveryNote(int id)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.DeleteDeliveryNoteHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddLSPositionsFromAB(Core.Apps.Purchase.Models.DeliveryNote.LSPositionsFromABModel data)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.AddLSPositionsFromABHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.DeliveryNote.DeliveryNoteHistoryModel>>), 200)]
		public IActionResult GetArticleID(string articleNumber)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetArtikelNrFromArtikelnummerHandler(articleNumber, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.CTSLogResponseModel>), 200)]
		public IActionResult GetCSTLogs(Psz.Core.CustomerService.Models.CSTLogSearchModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.Logging.GetCTSLogsHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}
