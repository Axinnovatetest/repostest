using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.ManagementOverview.Production.Interfaces;
using Psz.Core.ManagementOverview.Production.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.ManagementOverview.Controllers
{
	[Authorize]
	[Area("ManagementOverview")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ProductionController: ControllerBase
	{
		private const string MODULE = "Management Overview | Production";
		private readonly IProductionService _productionService;
		public ProductionController(IProductionService productionService)
		{
			this._productionService = productionService;
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.Production.Models.PackEmployeeProductionModel>), 200)]
		public IActionResult GetListeEmployeeProduction (int lager,int typeLoading)
		{
			try
			{
				return Ok(_productionService.GetListeEmployeeProduction(this.GetCurrentUser(), lager, typeLoading));

				
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult InsertEmployeeProduction(Core.ManagementOverview.Production.Models.EmployeeProductionModel data)
		{
			try
			{
				return Ok(_productionService.AddEmployeeProduction(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteEmployeeProduction( int id)
		{
			try
			{
				return Ok(_productionService.DeleteEmployeeProduction(this.GetCurrentUser(), id));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateEmployeeProduction(Core.ManagementOverview.Production.Models.EmployeeProductionModel data)
		{
			try
			{
				return Ok(_productionService.UpdateEmployeeProduction(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.Production.Models.PackGeplanteStundenModel>), 200)]
		public IActionResult GetPackPlanungStunden(int lager, int typeLoading)
		{
			try
			{
				return Ok(_productionService.GetListePlanungStunden(this.GetCurrentUser(), lager, typeLoading));


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetPackPlanungStundenByBloc(LagerRequest request)
		{
			try
			{
				var results = _productionService.GetListePlanungStundenByBlocExcel(this.GetCurrentUser(), request);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"PlanungStunden_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.ManagementOverview.Production.Models.GeplantStundenByTypModel>>), 200)]
		public IActionResult GetListPlanungStundenByTyp(int lager, int typeLoading)
		{
			try
			{
				return Ok(_productionService.GetListePlanungStundenByTyp(this.GetCurrentUser(), lager, typeLoading));


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.ManagementOverview.Production.Models.PackWertProductionModel>), 200)]
		public IActionResult GetListWertLager(int lager)
		{
			try
			{
				return Ok(_productionService.GetListeWertLager(this.GetCurrentUser(), lager));


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

	}
}
