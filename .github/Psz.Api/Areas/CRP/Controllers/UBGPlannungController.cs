using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FAPlanning;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class UBGPlannungController: ControllerBase
	{
		private const string MODULE = "CRP";
		private readonly ICrpUBGPlannung _crpUBGPlannung;
		public UBGPlannungController(ICrpUBGPlannung crpUBGPlannung)
		{
			_crpUBGPlannung = crpUBGPlannung;
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ArticlesResponseModel>), 200)]
		public IActionResult GetArticles(ArticlesRequestModel model)
		{
			try
			{
				var response = _crpUBGPlannung.GetArticles(this.GetCurrentUser(), model);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FASystemRequestModel>), 200)]
		public IActionResult GetFaSystem(FASystemRequestModel model)
		{
			try
			{
				var response = _crpUBGPlannung.GetFaSystem(this.GetCurrentUser(), model);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}