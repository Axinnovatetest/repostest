using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.EDI.Controllers
{
	[Authorize]
	[Area("EDI")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrderController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.OrderModel>), 200)]
		public IActionResult GetOrder(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Get(id,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrders()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Psz.Core.Apps.EDI.Handlers.Order.Get(null, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersValidated()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Get(true,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersUnvalidated()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Get(false,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult GetMyUnvalidatedCount()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.Order.CountUnvalidated(this.GetCurrentUser());
				return Ok(Core.Models.ResponseModel<int>.SuccessResponse(result));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult GetUnvalidatedCount()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.Order.CountUnvalidated(null/*this.GetCurrentUser()*/);
				return Ok(Core.Models.ResponseModel<int>.SuccessResponse(result));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.Element.OrderElementModel>>), 200)]
		public IActionResult GetOrderElements(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.Element.OrderElementModel>>()
				{
					Success = true,
					Body = Core.Apps.EDI.Handlers.Order.GetOrderElements(id, ignoreDeleted: false)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>), 200)]
		public IActionResult GetElement(int id, bool forUpdate = false)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>()
				{
					Success = true,
					Body = Core.Apps.EDI.Handlers.Order.GetElement(id, forUpdate)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Validate(Models.OrderValidationModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Validate(data.OrderId,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Unvalidate(int id)
		{
			try
			{
				var user = this.GetCurrentUser();

				var response = Core.Apps.EDI.Handlers.Order.Unvalidate(id,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
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
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult SearchItems(string searchText)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.SearchItems(searchText,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, searchText);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>>), 200)]
		public IActionResult SearchArtikels(string searchText)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.SearchArtikelsItems(searchText,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, searchText);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateElement(Core.Apps.EDI.Models.Order.UpdateElementModel data)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Order.UpdateElement(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.Purchase.Models.DeliveryNote.UpdateDeliveryItemModel>), 200)]
		public IActionResult UpdateDeliveryTemporaryElement(Psz.Core.Apps.Purchase.Models.DeliveryNote.UpdateDeliveryItemModel data)
		{
			try
			{
				var response = Core.Apps.Purchase.Handlers.DeliveryNote.UpdateDeliveryItemTemporaryHandler.UpdateDeliveryItem(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateElementItem(Core.Apps.EDI.Models.Order.UpdateElementItemModel data)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Order.UpdateElementItem(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult DeleteElement(Core.Apps.EDI.Models.Order.Element.DeleteElementModel data)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Order.DeleteElement(data,
					this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		#region Filtered by MyCustomers
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerOrdersCountModel>>), 200)]
		public IActionResult GetMyCustomerOrdersValidatedCount()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user, isEDI: true);
				return Ok(Core.Apps.EDI.Handlers.Order.GetCountByCustomers(userCustomers.Select(e => e.CustomerNumber).ToList(), true, user));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerOrdersCountModel>>), 200)]
		public IActionResult GetMyCustomerOrdersUnvalidatedCount()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user, isEDI: true);
				var usersCustomersIds = userCustomers.Select(x => x.CustomerNumber).ToList();

				return Ok(Core.Apps.EDI.Handlers.Order.GetCountByCustomers(usersCustomersIds, false, user));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrdersByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(customerId, null, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetValidatedByCustomer(Core.Apps.EDI.Models.Order.GetRequestModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(data.CustomerIds, true, user, data);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { "EDI" })]
		//[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		//public IActionResult GetUnvalidatedByCustomer(int customerId)
		//{
		//	try
		//	{
		//		var user = this.GetCurrentUser();
		//		if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
		//		{
		//			throw new Core.Exceptions.UnauthorizedException();
		//		}

		//		var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(customerId, false, user);

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e, customerId);
		//	}
		//}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetUnvalidatedByCustomer(Core.Apps.EDI.Models.Order.GetRequestModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(data.CustomerIds, false, user, data);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		#endregion

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Change.OrderItemsChangesModel>), 200)]
		public IActionResult GetPendingItemsChanges(int orderId)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.GetPendingItemsChanges(orderId,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, orderId);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult AcceptGlobalChange(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.AcceptGlobal(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult IgnoreGlobalChange(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.IgnoreGlobal(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AcceptElementChange(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.AcceptItemChange(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult IgnoreElementChange(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.IgnoreItemChange(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult AcceptElementCancel(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.AcceptItemCancel(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult IgnoreElementCancel(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.IgnoreItemCancel(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AcceptNewElement(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.AcceptNewItem(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult IgnoreNewElement(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Order.Change.IgnoreNewItem(id,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		#region Order Creation
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult CreateOrder([FromBody] Psz.Core.Apps.EDI.Models.Order.CreateModel data)
		{
			try
			{
				data.IsManualCreation = true;

				return Ok(Psz.Core.Apps.EDI.Handlers.Order.Create(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<int>>), 200)]
		public IActionResult CreateOrderElements([FromBody] Psz.Core.Apps.EDI.Models.Order.Element.NotCalculatedOrderElementsModel data)
		{
			try
			{
				return Ok(Psz.Core.Apps.EDI.Handlers.Order.Element.CreateOrderElements(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		#endregion Order Creation

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult SplitElement(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.CustomerService.EDI && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return Ok(Core.Apps.EDI.Handlers.Order.SplitElement(id));
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.Element.OrderElementModel>>), 200)]
		public IActionResult GetValidateHistory(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.ValidateHistoryHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>), 200)]
		public IActionResult GetElementValidateHistory(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.ValidateHistoryElementHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateInvoicingAddress(Core.Apps.EDI.Models.Order.OrderAddressModel data)
		{
			try
			{
				var response = new Core.Apps.EDI.Handlers.Order.UpdateInvoicingAddressHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeliveryAddress(Core.Apps.EDI.Models.Order.OrderAddressModel data)
		{
			try
			{
				var response = new Core.Apps.EDI.Handlers.Order.UpdateDeliveryAddressHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(List<KeyValuePair<int, int>>), 200)]
		public IActionResult GetLSListFromAB(int Nr)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.GetListLSFromABHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, Nr);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(List<Psz.Core.Apps.Purchase.Models.DeliveryNote.QuickSearchResponseModel>), 200)]
		public IActionResult QuickSearch(Psz.Core.Apps.Purchase.Models.DeliveryNote.QuickSearchInputModel model)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.DeliveryNoteQuickSearchHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, model);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Export_XLS(int id)
		{
			try
			{
				var results = new Core.Apps.EDI.Handlers.Order.ExportXLSHandler(this.GetCurrentUser(), id).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ExportUnvalidated_XLS(List<int> ids)
		{
			try
			{
				var results = new Core.Apps.EDI.Handlers.Order.ExportUnvalidatedXLSHandler(this.GetCurrentUser(), ids).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, ids);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult ValidateFiledsAuto(Models.OrderValidationModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || (!user.Access.Purchase.EDI && !user.Access.CustomerService.OrderProcessing))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.ValidateUpdate(data.OrderId,user,data.UpdateChoice);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Export_Fehle_AB_XLS()
		{
			try
			{
				var results = new Psz.Core.CustomerService.Handlers.Statistics.Fehler_AuswertungABAccessHandler(this.GetCurrentUser()).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Fehler_Auswertung_AB_data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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