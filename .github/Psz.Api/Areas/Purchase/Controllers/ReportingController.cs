using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.Purchase.Controllers
{
	[Authorize]
	[Area("Purchase")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ReportingController: ControllerBase
	{
		[HttpPost]
		[SwaggerOperation(Tags = new[] { "Purchase" })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.Analyse.CalculateTurnoverResponseModel>), 200)]
		public IActionResult Create(Core.Apps.Purchase.Models.Analyse.CalculateTurnoverRequestModel data)
		{
			try
			{
				return Ok(Core.Apps.Purchase.Handlers.Analyse.CalculateTurnover(data, this.GetCurrentUser()));
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}