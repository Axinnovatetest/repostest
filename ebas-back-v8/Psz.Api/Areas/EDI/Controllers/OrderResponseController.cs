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
	public class OrderResponseController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.OrderModel>), 200)]
		public IActionResult GetOrderResponse(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Get(id,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetOrderResponses()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
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
				if(user == null || !user.Access.CustomerService.EDI)
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
				if(user == null || !user.Access.CustomerService.EDI)
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
		public IActionResult GetUnvalidatedCount()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.Order.CountUnvalidated(null/*this.GetCurrentUser()*/);
				return Ok(result);
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
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.Element.OrderElementModel>>()
				{
					Success = true,
					Body = Core.Apps.EDI.Handlers.Order.GetOrderElements(id)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>), 200)]
		public IActionResult GetElement(int id)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = new Core.Models.ResponseModel<Core.Apps.EDI.Models.Order.Element.OrderElementModel>()
				{
					Success = true,
					Body = Core.Apps.EDI.Handlers.Order.GetElement(id)
				};

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
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
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.Validate(data.OrderId,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Unvalidate(int data)
		{
			try
			{
				var user = this.GetCurrentUser();

				var response = Core.Apps.EDI.Handlers.Order.Unvalidate(data,
					user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Inventory.Models.StorageLocation.StorageLocationModel>>), 200)]
		public IActionResult GetSpecificStorageLocations()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.StorageLocation.GetSpecific(this.GetCurrentUser()));
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
				return Ok(Core.Apps.EDI.Handlers.Order.GetCountByCustomers(userCustomers.Select(x => x.CustomerNumber).ToList(), true, user));
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
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
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
				if(user == null || !user.Access.CustomerService.EDI)
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

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetValidatedByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(customerId, true, user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Order.OrderModel>>), 200)]
		public IActionResult GetUnvalidatedByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.Order.GetByCustomer(customerId, false, user);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
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
				return this.HandleException(e);
			}
		}
		#endregion Order Creation
	}
}