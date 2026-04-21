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
	public class CrpHolidayController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Holiday.HolidayModel>), 200)]
		public IActionResult GetDetails(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.GetDetailsHandler(id, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Holiday.HolidayModel>>), 200)]
		public IActionResult Get(Core.MaterialManagement.CRP.Models.Holiday.GetModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.GetHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Holiday.HolidayModel>>), 200)]
		public IActionResult GetSimilar(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.GetSimilarHandler(id, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult Create(Core.MaterialManagement.CRP.Models.Holiday.CreateHolidaysModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.CreateHolidaysHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult Update(Core.MaterialManagement.CRP.Models.Holiday.UpdateModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.UpdateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult Delete(Core.MaterialManagement.CRP.Models.Holiday.DeleteHolidayModel data)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.Holiday.DeleteHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Holiday.PreviousHolidayResponseModel>>), 200)]
		public IActionResult Get2PreviousYearsHoliday(bool distinct)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Holiday.Get2PreviousYearsHolidayHandler(distinct, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
