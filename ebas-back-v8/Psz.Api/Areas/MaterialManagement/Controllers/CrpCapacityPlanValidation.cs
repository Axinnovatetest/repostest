using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CrpCapacityPlanValidationController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.CapacityPlanValidation.GetModel>>), 200)]
		public IActionResult GetUsers()
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.GetAllHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddUsers(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.AddUserModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.AddUserHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveUser(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.RemoveUserModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.RemoveUserHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Validate(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.ValidateModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.ValidateHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Unvalidate(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.ValidateModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.UnvalidateHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Reject(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.ValidateModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.RejectHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.CapacityPlanValidation.HistoryResponseModel>), 200)]
		public IActionResult GetHistory(Core.MaterialManagement.CRP.Models.CapacityPlanValidation.HistoryRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlanValidation.GetHistoryHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
