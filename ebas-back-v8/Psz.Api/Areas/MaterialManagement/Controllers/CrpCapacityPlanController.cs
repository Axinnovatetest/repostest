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
	public class CrpCapacityPlanController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult SetCapacityPlan(Core.MaterialManagement.CRP.Models.CapacityPlan.SetCapacityPlanModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.SetCapacityPlanHandler(data, this.GetCurrentUser())
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
		public IActionResult SetCapacityPlanRow(Core.MaterialManagement.CRP.Models.CapacityPlan.SetCapacityPlanModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.SetCapacityPlanRowHandler(data, this.GetCurrentUser())
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
		public IActionResult SetCapacityPlanKw(Core.MaterialManagement.CRP.Models.CapacityPlan.SetCapacityPlanKwModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.SetCapacityPlanKwHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.CapacityPlan.OverviewModel>), 200)]
		public IActionResult GetOverview(Core.MaterialManagement.CRP.Models.CapacityPlan.OverviewRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.GetOverviewHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.CapacityPlan.CapacityPlanItemModel>>), 200)]
		public IActionResult GetPlanItem(Core.MaterialManagement.CRP.Models.CapacityPlan.GetPlanItemRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.GetPlanItemHandler(data, this.GetCurrentUser())
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
		public IActionResult DeleteItem(Core.MaterialManagement.CRP.Models.CapacityPlan.DeleteItemModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.DeleteItemHandler(data, this.GetCurrentUser())
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
		public IActionResult Delete(Core.MaterialManagement.CRP.Models.CapacityPlan.DeleteModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.DeleteHandler(data?.Items, this.GetCurrentUser())
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
		public IActionResult GetNewPlanFirstWeekHandler(Core.MaterialManagement.CRP.Models.CapacityPlan.NewPlanFirstWeekRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.GetNewPlanFirstWeekHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
