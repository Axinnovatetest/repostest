using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CrpCapacityPlanToolsController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.CapacityPlan.CalculatedItemModel>), 200)]
		public IActionResult CalculateItem(Core.MaterialManagement.CRP.Models.CapacityPlan.CalculateItemModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.CapacityPlan.CalculateItemHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<decimal>>), 200)]
		public IActionResult Fetch_HrF1PersonPerMachine_Values()
		{
			try
			{
				var values = new List<decimal> { 0m }; // [0,25 : 3,00] +0,25

				var value = 0.25m;
				while(value <= 3.00m)
				{
					values.Add(value);
					value += 0.25m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<decimal>>), 200)]
		public IActionResult Fetch_HrF2UtilisationRate_Values()
		{
			try
			{
				var values = new List<decimal> { 0m }; // [0,50 : 1,00] +0,05		

				var value = 0.50m;
				while(value <= 1.00m)
				{
					values.Add(value);
					value += 0.05m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<decimal>>), 200)]
		public IActionResult Fetch_HrProductivity_Values()
		{
			try
			{
				var values = new List<decimal> { 0m }; // [0,20 : 1,00] +0,01	

				var value = 0.20m;
				while(value <= 1.00m)
				{
					values.Add(value);
					value += 0.01m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<decimal>>), 200)]
		public IActionResult Fetch_SrF3AttendanceLevel_Values()
		{
			try
			{
				var values = new List<decimal> { 0m }; // Dropdown-list [0,50 : 1,00] +0,05		

				var value = 0.50m;
				while(value <= 1.00m)
				{
					values.Add(value);
					value += 0.05m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<decimal>>), 200)]
		public IActionResult Fetch_SrProductivity_Values()
		{
			try
			{
				var values = new List<decimal> { 0m }; // Dropdown-list [0,20 : 1,00] +0,01

				var value = 0.20m;
				while(value <= 1.00m)
				{
					values.Add(value);
					value += 0.01m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult Fetch_ShiftsPerWeek_Values()
		{
			try
			{
				var values = new List<int>(); // Dropdown-list [0:15] +1		

				var value = 0;
				while(value <= 15)
				{
					values.Add(value);
					value++;
				}

				return Ok(Core.Common.Models.ResponseModel<List<int>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult Fetch_SpecialShiftsPerWeek_Values()
		{
			try
			{
				var values = new List<int>(); //Dropdown-list[0:5] +1	

				var value = 0;
				while(value <= 6)
				{
					values.Add(value);
					value++;
				}

				return Ok(Core.Common.Models.ResponseModel<List<int>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult Fetch_SpecialHours_Values()
		{
			try
			{
				var values = new List<decimal>();// Dropdown-list [0:28] +1	 // - 2021.06.10 - Khelil - Hours should go by a step of 0.5

				var value = 0m;
				while(value <= 28)
				{
					values.Add(value);
					value += 0.5m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult Fetch_WorkingHoursPershift_Values()
		{
			try
			{
				var values = new List<decimal>();// Dropdown-list [0:12] +0.25	 // - 2021.06.10 - Khelil - Hours should go by a step of 0.5
												 // - 2021.07.05 - Khelil - Hours should go by a step of 0.25

				var value = 0m;
				while(value <= 12)
				{
					values.Add(value);
					value += 0.25m;
				}

				return Ok(Core.Common.Models.ResponseModel<List<decimal>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// ---

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult FakeProductionOrderArticles(int unitId)
		{
			try
			{
				//var wsDetails = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByHallId(unitId);
				var ws = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallId(unitId);
				var values = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(ws.Select(x => x.ArticleId)?.ToList());

				return Ok(Core.Common.Models.ResponseModel<List<Infrastructure.Data.Entities.Tables.WPL.ArticleEntity>>.SuccessResponse(values));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult FakeProductionOrder(Core.MaterialManagement.CRP.Models.Capacity.FakeFARequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AddFakeFAHandler(data, this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
