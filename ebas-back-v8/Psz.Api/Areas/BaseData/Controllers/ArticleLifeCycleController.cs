using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.BaseData.Models.LifeCycle;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleLifeCycleController: ControllerBase
	{
		private const string MODULE = "BaseData";
		private readonly IArticleLifeCycleService _articleLifeCycleService;

		public ArticleLifeCycleController(IArticleLifeCycleService articleLifeCycleService)
		{
			_articleLifeCycleService = articleLifeCycleService;
		}

		#region life cycle phases
		[HttpGet]
		[SwaggerOperation(Tags = new[] {MODULE})]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<ArticleLifeCyclePhasesModel>>), 200)]
		public IActionResult GetLifeCyclePhases()
		{
			try
			{
				var response=_articleLifeCycleService.GetLifeCyclesPhases(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] {MODULE})]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddLifeCyclePhase(ArticleLifeCyclePhasesRequestModel data)
		{
			try
			{
				var response=_articleLifeCycleService.AddLifeCyclePhase(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] {MODULE})]
		[ProducesResponseType(typeof(ResponseModel<int>),200)]
		public IActionResult UpdateLifeCyclePhase(ArticleLifeCyclePhasesRequestModel data)
		{
			try
			{
				var response=_articleLifeCycleService.UpdateLifeCyclePhase(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteLifeCyclePhase(int id) 
		{
			try
			{
				var response=_articleLifeCycleService.DeleteLifeCyclePhase(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult CheckLifeCycleBeforeEditOrDelete(int id)
		{
			try
			{
				var response = _articleLifeCycleService.CheckLifeCycleBeforeEditOrDelete(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
		#region article life cycle
		[HttpGet]
		[SwaggerOperation(Tags =new[] { MODULE })]
		[ProducesResponseType(typeof (ResponseModel<List<ArticleLifeCyclesModel>>), 200)]
		public IActionResult GetArticleLifeCycle(int articleId)
		{
			try
			{
				var response=_articleLifeCycleService.GetArticleLifeCycle(this.GetCurrentUser(), articleId);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags =new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddArticleLifeCycle(AddArticleLifeCycleRequestModel data)
		{
			try
			{
				var response=_articleLifeCycleService.AddArticleLifeCycle(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] {MODULE})]
		[ProducesResponseType(typeof(ResponseModel<int>),200)]
		public IActionResult UpdateArticleLifeCycle(AddArticleLifeCycleRequestModel data)
		{
			try
			{
				var response=_articleLifeCycleService.UpdateArticleLifeCycle(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteArticleLifeCycle(int id) 
		{
			try
			{
				var reponse=_articleLifeCycleService.DeleteArticleLifeCycle(this.GetCurrentUser(), id);
				return Ok(reponse);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}
