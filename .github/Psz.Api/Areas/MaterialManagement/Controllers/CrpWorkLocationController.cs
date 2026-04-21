using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CrpWorkLocationController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.WorkLocationsModel>), 200)]
		public IActionResult Get(bool filterByUser = true)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetHandler(this.GetCurrentUser(false), filterByUser)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.OperationModel>), 200)]
		public IActionResult GetOperations()
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetOperationsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.HallModel>), 200)]
		public IActionResult GetHalls(int? countryId)
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetHallsHandler(countryId, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.DepartementModel>), 200)]
		public IActionResult GetDepartements()
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetDepartementsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.WorkAreaModel>), 200)]
		public IActionResult GetWorkAreas()
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetWorkAreasHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.WorkLocation.WorkStationModel>), 200)]
		public IActionResult GetWorkStations()
		{
			try
			{
				var response = new Psz.Core.MaterialManagement.CRP.Handlers.WorkLocation.GetWorkStationsHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
