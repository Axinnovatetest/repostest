using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.Purchase.Controllers
{
	[Authorize]
	[Area("Purchase")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ProductionController: ControllerBase
	{
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Production.ManufacturingFacilityModel>>), 200)]
		public IActionResult GetManufacturingFacilities()
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.GetManufacturingFacilitiesHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Production.OriginalArticleModel>>), 200)]
		public IActionResult GetOriginalArticles(string searchQuery)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.GetOriginalAritclesHandler(this.GetCurrentUser()).Handle(searchQuery));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetPositionManufacturingFacility(int PositionStorageLocationId)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.GetPositionManufacturingFacilityHandler(this.GetCurrentUser(), PositionStorageLocationId).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { "Purchase" })]
		//[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		//public IActionResult ValidateProductionCommand(Core.Apps.Purchase.Models.Production.ValidateCommandModel command)
		//{
		//	try
		//	{
		//		return Ok(new Core.Apps.Purchase.Handlers.Production.ValidateCommandHandler(this.GetCurrentUser(), command).Handle());
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ValidateProductionCommandWUbgFa(Core.Apps.Purchase.Models.Production.ValidateCommandWUbgFaRequestModel data)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.ValidateCommandWUbgFaHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CancelProductionCommand(int Id, string notes)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.CancelCommandHandler(this.GetCurrentUser(), new Core.Apps.Purchase.Models.Production.CancelCommandModel { Id = Id, Notes = notes }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CancelProductionCommandWUBGFa(int Id, string notes, bool cancelUbg = true)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.CancelCommandWUbgFaHandler(this.GetCurrentUser(), new Core.Apps.Purchase.Models.Production.CancelCommandModel { Id = Id, Notes = notes, CancelUBG = cancelUbg }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.Production.TechnicianModel>>), 200)]
		public IActionResult GetTechnicians()
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.Production.GetTechinicansHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}