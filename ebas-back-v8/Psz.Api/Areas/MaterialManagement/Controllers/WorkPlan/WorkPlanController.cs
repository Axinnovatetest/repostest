using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Models.FA;
using Psz.Core.MaterialManagement.WorkPlan.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class WorkPlanController : ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<WorkPlanResponseModel>), 200)]
		public IActionResult WorkPlanSearch(WorkPlanSearchModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.WorkPlan.Handlers.WorkPlansSearchHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetAutocompleteActiveEFArticles(string article)
		{
			try
			{
				var response = new Core.MaterialManagement.WorkPlan.Handlers.AutocompleteWorkPlanArticleHandler(article, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}
