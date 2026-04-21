using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Apps.EDI.Models.Customers.Users;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.EDI.Controllers
{
	[Authorize]
	[Area("EDI")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CustomersController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Get(isEDIActive: true);

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.Users.UserModel>>), 200)]
		public IActionResult GetUsers()
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Users.Get(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<AppointmentCustomerUserResponseModel>>), 200)]
		public IActionResult GetUserCustomersList(AppointmentCustomerUserRequestModel data)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Users.GetUserCustomersList(this.GetCurrentUser(), data);

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerUsersModel>>), 200)]
		public IActionResult GetCustomerUsers(int customerId)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Users.GetCustomerUsers(customerId);

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult Search(string searchText = null)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Search(searchText);

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.Users.UserModel>>), 200)]
		public IActionResult SearchUsers(string searchText = null)
		{
			try
			{
				var response = Core.Apps.EDI.Handlers.Customers.Users.Search(searchText, this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.ReplacementModel>>), 200)]
		public IActionResult MyReplacements()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserReplacements(this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.EDI.Models.Customers.CustomerModel>>), 200)]
		public IActionResult MyCustomers()
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.GetUserCustomers(this.GetCurrentUser(),
					isEDI: true));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult SetPrimaryUser(Core.Apps.EDI.Models.Customers.SetPrimaryUserModel data)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.SetPrimaryUser(data,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult RemovePrimaryUser(int customerId)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.RemovePrimaryUserHandler(this.GetCurrentUser(), customerId).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, customerId);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult SetReplacements(Core.Apps.EDI.Models.Customers.SetReplacementsModel data)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.SetReplacements(data,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult UpdateReplacement(Core.Apps.EDI.Models.Customers.UpdateReplacementModel data)
		{
			try
			{
				return Ok(Core.Apps.EDI.Handlers.Customers.UpdateReplacement(data,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { "EDI" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<object>), 200)]
		public IActionResult DeleteReplacement(Core.Apps.EDI.Models.Customers.DeleteReplacementModel data)
		{
			try
			{

				return Ok(Core.Apps.EDI.Handlers.Customers.DeleteReplacement(data,
					this.GetCurrentUser()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}