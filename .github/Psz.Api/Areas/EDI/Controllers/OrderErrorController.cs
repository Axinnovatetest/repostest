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
	public class OrderErrorController: ControllerBase
	{
		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public ActionResult DownloadCorruptedFile(int id)
		{
			try
			{
				var fileBytes = Core.Apps.EDI.Handlers.Order.GetErrorFile(id);
				if(fileBytes == null || fileBytes.Length == 0)
				{
					return NotFound();
				}

				return File(fileBytes, "application/xml", "Corrupted XML.xml");
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return NotFound();
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var result = Core.Apps.EDI.Handlers.OrderError.OrderError.Get(null/*this.GetCurrentUser()*/);

				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult GetValidated()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.OrderError.OrderError.GetValidated(null/*this.GetCurrentUser()*/);

				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult GetUnValidated()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.OrderError.OrderError.GetNotValidated(null/*this.GetCurrentUser()*/);

				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetCount()
		{
			try
			{
				var result = Psz.Core.Apps.EDI.Handlers.OrderError.OrderError.CountNotValidated(null/*this.GetCurrentUser()*/);

				return Ok(Core.Models.ResponseModel<int>.SuccessResponse(result));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult Validate(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.OrderError.OrderError.Validate(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}

		#region Filtered by MyCustomers
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerOrderErrorsCountModel>>), 200)]
		public IActionResult GetMyCustomerValidatedCount()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(this.GetCurrentUser(), isEDI: true);
				var userCustomersIds = userCustomers.Select(e => e.Id).ToList();

				return Ok(Core.Apps.EDI.Handlers.OrderError.OrderError.GetCountByCustomers(userCustomersIds, true, user));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerOrderErrorsCountModel>>), 200)]
		public IActionResult GetMyCustomerUnvalidatedCount()
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(this.GetCurrentUser(), isEDI: true);
				return Ok(Core.Apps.EDI.Handlers.OrderError.OrderError.GetCountByCustomers(userCustomers.Select(x => x.Id).ToList(), false, user));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult GetErrorsByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.OrderError.OrderError.GetByCustomer(customerId, null, user, null);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult GetValidatedByCustomer(Core.Apps.EDI.Models.OrderError.GetRequestModel data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var response = Core.Apps.EDI.Handlers.OrderError.OrderError.GetByCustomer(data.CustomerId, true, user, data);

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.OrderError.OrderErrorModel>>), 200)]
		public IActionResult GetUnvalidatedByCustomer(int customerId)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null || !user.Access.CustomerService.EDI)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return Ok(Core.Apps.EDI.Handlers.OrderError.OrderError.GetByCustomer(customerId, false, user, null));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult ReloadFile(int id)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.OrderError.OrderError.ReloadFile(id, this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return StatusCode(500, "Unknown error");
			}
		}
	}
}