using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CrpConfigurationController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Configuration.AddResponseModel>>), 200)]
		public IActionResult Get()
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.GetHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Add(Core.MaterialManagement.CRP.Models.Configuration.AddResponseModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.AddHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPut]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.CapacityModel>>), 200)]
		public IActionResult Edit(Core.MaterialManagement.CRP.Models.Configuration.AddResponseModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.EditHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult Delete(Core.MaterialManagement.CRP.Models.Configuration.DeleteRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.DeleteHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpDelete]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteById(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.DeleteByIdHandler(id, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Configuration.AddResponseModel>>), 200)]
		public IActionResult GetValidationPendingDepartments()
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.GetValidationPendingDepartmentsHandler(this.GetCurrentUser(false))
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[Consumes("application/json")]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AcceptPendingDepartments([FromBody] List<int> data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Configuration.AcceptPendingDepartmentsHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}