using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using System;
using System.Collections.Generic;
using Psz.Core.CRP.Models.Preview;
using Swashbuckle.AspNetCore.Annotations;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class PreviewController: ControllerBase
	{
		private const string MODULE = "CRP | Preview";

		private IPreviewService iPreviewService;
		public PreviewController(IPreviewService iPreviewService)
		{
			this.iPreviewService = iPreviewService;
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<PreviewHeaderWeekResponseModel>>), 200)]
		public IActionResult GetHeaders()
		{
			try
			{
				return Ok(this.iPreviewService.GetHeadersHandler( this.GetCurrentUser()));

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<PreviewArticleResponseModel>), 200)]
		public IActionResult GetArticle(int articleId)
		{
			try
			{
				return Ok(this.iPreviewService.GetArticleHandler(this.GetCurrentUser(), articleId));
			} catch(Exception e)
			{
				this.HandleException(e, articleId);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult GetHorizon1()
		{
			try
			{
				return Ok(this.iPreviewService.GetHorizon1(this.GetCurrentUser()));

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<PreviewWeekResponseModel>>), 200)]
		public IActionResult GetEntitiesByArticleYearWeek(string entityType, int articleId, int year, int week)
		{
			try
			{
				return Ok(this.iPreviewService.GetEntitiesByArticleYearWeekHandler(this.GetCurrentUser(), entityType, articleId, year, week));

			} catch(Exception e)
			{
				return this.HandleException(e, new { EntityType = entityType, ArticleId = articleId, Year = year, Week = week });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticleNumbers(string searchTerm, int page, int pageSize)
		{
			try
			{
				return Ok(this.iPreviewService.GetArticleNumbersHandler(this.GetCurrentUser(), searchTerm, page, pageSize));

			} catch(Exception e)
			{
				return this.HandleException(e, new { searchTerm= searchTerm, page= page, pageSize= pageSize });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateSnapshot()
		{
			try
			{
				return Ok(this.iPreviewService.UpdateSnapshot(this.GetCurrentUser()));

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}