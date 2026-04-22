using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticlePurchaseController: ControllerBase
	{
		private const string MODULE = "BaseData";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Purchase.GetModel>>), 200)]
		public IActionResult GetByArticle(int articleId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.GetByArticleHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Purchase.GetMinimalModel>>), 200)]
		public IActionResult GetArticlePurchaseList(int articleId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.GetArticlePurchaseListHandler(this.GetCurrentUser(), articleId)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Purchase.GetMinimalModel>>), 200)]
		public IActionResult AddArticlePurchaseList(Psz.Core.BaseData.Models.Article.Purchase.GetModel purchaseItem)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.AddHandler(this.GetCurrentUser(), purchaseItem)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, purchaseItem);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GenerateArticlePurchasePriceGroups(Core.BaseData.Models.Article.Purchase.GeneratePriceGroupRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.GeneratePriceGroupsHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Purchase.GetMinimalModel>>), 200)]
		public IActionResult UpdateArticlePurchase(Psz.Core.BaseData.Models.Article.Purchase.GetModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.UpdateArticlePurchaseHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Purchase.GetMinimalModel>>), 200)]
		public IActionResult DeleteArticlePurchase(int purchaseNr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.DeleteArticlePurchaseHandler(this.GetCurrentUser(), purchaseNr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, purchaseNr);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Purchase.InitValuesResponseModel>>), 200)]
		public IActionResult GetInitValues()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.GetInitValuesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Purchase.GetModel>>), 200)]
		public IActionResult GetByArticleAndSupplierNr(Psz.Core.BaseData.Models.Article.Purchase.GetMinimalRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.Purchase.GetByArticleAndSupplierIdHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult VerifyOfferDateByArticleAndSupplierNr(Psz.Core.BaseData.Models.Article.Purchase.OfferDateMinimalRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.Purchase.VerifyOfferDateByArticleAndSupplierIdHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Purchase.GetModel>>), 200)]
		public IActionResult GetArticleHasStandardSupplier(int id)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.ROH.OfferRequests.GetArticleHasStandardSupplierdHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		#region Purchase Custom Prices
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Purchase.CustomPriceModel>>), 200)]
		public IActionResult GetCustomPrices(int data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.CustomPrice.GetAllHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddCustomPrice(Psz.Core.BaseData.Models.Article.Purchase.CustomPriceModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.CustomPrice.AddHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCustomPrice(Psz.Core.BaseData.Models.Article.Purchase.CustomPriceModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.CustomPrice.UpdateHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCustomPrice(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Purchase.CustomPrice.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion Purchase Custom Prices
	}
}