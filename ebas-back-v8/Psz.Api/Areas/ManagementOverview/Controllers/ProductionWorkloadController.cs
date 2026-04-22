using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Data;
using Psz.Core.ManagementOverview.ProductionWorkload.Models.Warehouse;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using static SkiaSharp.SKPath;

namespace Psz.Api.Areas.ManagementOverview.Controllers
{
	[Authorize]
	[Area("ManagementOverview")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ProductionWorkloadController: ControllerBase
	{
		private const string MODULE = "Management Overview | Production Workload";

		#region data
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProductionWarehouses()
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.GetProductionWarehousesHandler(this.GetCurrentUser())
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult RefreshWorkload(int warehouseId)
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.RefreshWorkloadHandler(this.GetCurrentUser(false), warehouseId)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<WorkloadResponseModel>), 200)]
		public IActionResult GetWorkload(int warehouseId)
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.GetWorkloadHandler(this.GetCurrentUser(false), warehouseId)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<WorkloadHistoryResponseModel>>), 200)]
		public IActionResult GetWorkloadHistory(WorkloadHistoryRequestModel data)
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.GetWorkloadHistoryHandler(this.GetCurrentUser(false), data)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<WeekFaResponseModel>>), 200)]
		public IActionResult GetWeekFas(WorkloadHistoryRequestModel data)
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.GetWeekFasHandler(this.GetCurrentUser(false), data)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<WorkloadBacklogResponseModel>>), 200)]
		public IActionResult GetWorkloadBacklogWeeks(int warehouseId)
		{
			try
			{
				var response = new Core.ManagementOverview.ProductionWorkload.Handlers.GetWorkloadBacklogWeeksHandler(this.GetCurrentUser(false), warehouseId)
			   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}
		#endregion
	}
}
