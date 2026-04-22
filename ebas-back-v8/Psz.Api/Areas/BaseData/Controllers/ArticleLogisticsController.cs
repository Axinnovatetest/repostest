using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Models.Article.Logistics;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleLogisticsController: ControllerBase
	{
		private const string MODULE = "BaseData";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Logistics.LagerStatusModel>>), 200)]
		public IActionResult GetLagerStatus(int articleId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Logistics.GetLagerStatusHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Logistics.ArticleLogisticsModel>), 200)]
		public IActionResult GetArticleLogistics(int ArticleID)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Logistics.GetArticleLogisticsHandler(this.GetCurrentUser(), ArticleID)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticleLogistics(Core.BaseData.Models.Article.Logistics.ArticleLogisticsModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateArticleLogisticsHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<KeyValuePair<bool, string>>), 200)]
		public IActionResult CheckForOriginalLandDiffrence(Core.BaseData.Models.Article.Logistics.ArticleLogisticsModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.CheckForOriginalLandDiffrenceHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Logistics.LagerStatusGeneralModel>), 200)]
		public IActionResult GetArticleLagerStatus(LagerRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Logistics.GetLagerStatusHandler_2(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetLagerList()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.GetLagerListHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetDispoformelList()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.GetDispoFormelHandler(this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLagerStatus(Core.BaseData.Models.Article.Logistics.LagerStatusModel_2 data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateLagerStatusHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticlePricePackaging(Core.BaseData.Models.Article.SalesExtension.SalesItemModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateArticlePricePackagingHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLagerStock(Core.BaseData.Models.Article.Logistics.UpdateStockRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateLagerStockHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLagerMinStock(Core.BaseData.Models.Article.Logistics.LagerStatusModel_2 data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateLagerMinStockHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLagerCCID(Core.BaseData.Models.Article.Logistics.UpdateCCIDRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateLagerCCIDHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateLagerProposal(Core.BaseData.Models.Article.Logistics.UpdateProposalRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.UpdateLagerProposalHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SetLagerMinStock(Core.BaseData.Models.Article.Logistics.SetLagerMinStockRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Logistics.SetLagerMinStockHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
	}
}