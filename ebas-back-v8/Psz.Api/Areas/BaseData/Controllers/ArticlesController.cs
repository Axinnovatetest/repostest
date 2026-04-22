using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Interfaces.Article;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.BaseData.Models.Article.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Api.Areas.BaseData.Controllers
{
	using System.Runtime.Versioning;
	using Core.BaseData.Interfaces.Article;
	using Psz.Core.BaseData.Handlers.ROH.OfferRequests;
	using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
	using Psz.Core.BaseData.Models.Files;
	using Psz.Core.BaseData.Models.ROH;
	using Psz.Core.Common.Models;
	using Psz.Core.ManagementOverview.Production.Models;

	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticlesController: ControllerBase
	{
		private const string MODULE = "BaseData";
		private readonly IArticleService _articleService;

		public ArticlesController(IArticleService articleService)
		{
			this._articleService = articleService;
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleModel>), 200)]
		public IActionResult GetSingle(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.GetHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleModel>), 200)]
		public IActionResult Delete(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.DeleteHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetMinimal()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.GetMinimalHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult Create(Core.BaseData.Models.Article.CreateRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article Create");
				var response = new Core.BaseData.Handlers.Article.CreateHandler(this.GetCurrentUser(), data)
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
		public IActionResult Update(Core.BaseData.Models.Article.ArticleModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article Update");
				var response = new Core.BaseData.Handlers.Article.UpdateHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#region Overview
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleOverviewModel>), 200)]
		public IActionResult GetOverview(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetArticleOverviewHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleOverviewModel.Blanket>>), 200)]
		public IActionResult GetOverviewBlankets(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetBlanketsHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleOverviewModel.BlanketDetail>>), 200)]
		public IActionResult GetOverviewBlanketDetails(string id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetBlanketDetailHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateOverview(Core.BaseData.Models.Article.ArticleOverviewMinimalModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateOverview");
				var response = new Core.BaseData.Handlers.Article.UpdateOverviewHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddOverviewLager(int articleId)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article AddOverviewLager");
				var response = new Core.BaseData.Handlers.Article.Overview.AddLagerHandler(this.GetCurrentUser(), articleId).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GenerateWmsNewFile(int articleId)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article GenerateWmsNewFile");
				Core.BaseData.Handlers.Article.CreateHandler.generateFileDAT(articleId, isNew: true);
				return Ok(Psz.Core.Common.Models.ResponseModel<int>.SuccessResponse(1));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>), 200)]
		public IActionResult GetOverviewPurchasePriceDetails(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetPurchaseDetailsHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		// -

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleOverviewBlanket(Core.BaseData.Models.Article.ArticleOverviewModel.Blanket data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ToggleOverviewBlanket");
				var response = new Core.BaseData.Handlers.Article.Overview.ToggleBlanketHandler(this.GetCurrentUser(), data).Handle();
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
		public IActionResult EditOverviewBlanket(Core.BaseData.Models.Article.ArticleOverviewModel.Blanket data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article EditOverviewBlanket");
				var response = new Core.BaseData.Handlers.Article.Overview.UpdateBlanketHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>), 200)]
		public IActionResult GetOverviewBlanketHistory(int articleId)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article GetOverviewBlanketHistory");
				var response = new Core.BaseData.Handlers.Article.Overview.GetBlanketHistoryHandler(this.GetCurrentUser(), articleId).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddOverviewBlanketHistory(Core.BaseData.Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article AddOverviewBlanketHistory");
				var response = new Core.BaseData.Handlers.Article.Overview.AddBlanketHistoryHandler(this.GetCurrentUser(), data).Handle();
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
		public IActionResult EditOverviewBlanketHistory(Core.BaseData.Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article EditOverviewBlanketHistory");
				var response = new Core.BaseData.Handlers.Article.Overview.UpdateBlanketHistoryHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteOverviewBlanketHistory(int id)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article DeleteOverviewBlanketHistory");
				var response = new Core.BaseData.Handlers.Article.Overview.DeleteBlanketHistoryHandler(this.GetCurrentUser(), id).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		#endregion
		#region Data
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleDataModel>), 200)]
		public IActionResult GetArticleData(int ArticleNr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetArticleDataHandler(this.GetCurrentUser(), ArticleNr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetArticleCuGewicht(int nr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetArticleCuGewichtHandler(this.GetCurrentUser(), nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetArticleCuPreis(int nr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetArticleCuPreisHandler(this.GetCurrentUser(), nr)
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
		public IActionResult UpdateArticleData(Core.BaseData.Models.Article.ArticleDataModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateData");
				var response = new Core.BaseData.Handlers.Article.Data.UpdateArticleDataHandler(this.GetCurrentUser(), data)
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
		public IActionResult GetCustomerContacts(int? articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetCustomerContactsHandler(this.GetCurrentUser(), articleId)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		#endregion
		#region Production
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Production.ArticleProductionModel>), 200)]
		public IActionResult GetArticleProduction(int articleID)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Production.GetArticleProductionHandler(this.GetCurrentUser(), articleID)
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
		public IActionResult UpdateArticleProduction/*UpdateData*/(Core.BaseData.Models.Article.Production.ArticleProductionModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateProduction");
				var response = new Core.BaseData.Handlers.Article.Production.UpdateArticleProductionHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		#endregion

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateData(Core.BaseData.Models.Article.ArticleDataModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateData");
				var response = new Core.BaseData.Handlers.Article.UpdateDataHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdatePartial(Core.BaseData.Models.Article.ArticlePartialModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdatePartial");
				var response = new Core.BaseData.Handlers.Article.UpdatePartialHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleSearchResponseModel>>), 200)]
		public IActionResult Search(Core.BaseData.Models.Article.ArticleSearchModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleSearchResponseModel>>), 200)]
		public IActionResult SearchAdvanced(Core.BaseData.Models.Article.ArticleSearchAdvancedModel data)
		{
			try
			{
				return Ok(_articleService.SearchArticleAdvanced(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArtikelOutilCossModel>>), 200)]
		public IActionResult GetOutilCossByArtikel(int data)
		{
			try
			{
				return Ok(_articleService.GetDetailsOutilCossByArtikel(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetOutilCossByArtikelExcel(int data)
		{
			try
			{
				var results = _articleService.GetListeOutilCossByArtikelExcel(this.GetCurrentUser(),  data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Crimping_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleActiveStatus(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ToggleActiveStatusHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SearchNr(string nr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchNrHandler(this.GetCurrentUser(), nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult SearchNummer(string nummer)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchNummerHandler(this.GetCurrentUser(), nummer)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.SearchNummerResponseModel>>), 200)]
		public IActionResult SearchNummerPlus(Core.BaseData.Models.Article.SearchNummerRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchNummerPlusHandler(this.GetCurrentUser(), data)
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
		public IActionResult SearchDesignationPlus(Core.BaseData.Models.Article.SearchNummerRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchDesignationPlusHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SearchDesignation(string query)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SearchDesignationHandler(this.GetCurrentUser(), query)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// >>>>>> Logs
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.ObjectLog.ObjectLogModel>), 200)]
		public IActionResult GetLog(int artikelNr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ObjectLog.GetHandler(this.GetCurrentUser(), artikelNr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.ObjectLog.ObjectLogModel>), 200)]
		public IActionResult GetFullLog(int artikelNr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ObjectLog.GetFullHandler(this.GetCurrentUser(), artikelNr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		// Manager Users        
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ManagerUser.ManagerUserModel>), 200)]
		public IActionResult SearchManagerUsers(string userText)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.SearchHandler(this.GetCurrentUser(), userText)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ManagerUser.ManagerUserModel>), 200)]
		public IActionResult GetAllManagerUsers()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ManagerUser.ManagerUserModel>), 200)]
		public IActionResult GetManagerUsers(int artikelNr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.GetByArticleHandler(this.GetCurrentUser(), artikelNr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddManagerUser(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.AddHandler(this.GetCurrentUser(), id)
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
		public IActionResult SetManagerUser(Core.BaseData.Models.Article.ManagerUser.SetManagerUserModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.SetHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateManagerUser(Core.BaseData.Models.Article.ManagerUser.ManagerUserModel data)
		{
			try
			{
				return Ok(new Psz.Core.BaseData.Handlers.Article.Overview.UpdateManagerUsersHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ManagerUser.ManagerUserModel>), 200)]

		public IActionResult DeleteManagerUser(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ManagerUser.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// Image
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticleImage([FromForm] Models.Articles.UpdateImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.BaseData.Handlers.Article.UpdateArticleImageHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		// >>> Product Group
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetListProductGroups()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProductGroup.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListWarentypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Data.GetListWarenTypesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ProductGroup.ProductGroup>>), 200)]
		public IActionResult GetAllProductGroups()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProductGroup.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ProductGroup.ProductGroup>), 200)]
		public IActionResult GetProductGroup(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProductGroup.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProductGroup(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProductGroup.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddProductGroup(Core.BaseData.Models.Article.ProductGroup.ProductGroup data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ProductGroup.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		// >>> Article Classes
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListArticleClasses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ArticleClass.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleClass.ArticleClass>>), 200)]
		public IActionResult GetAllArticleClasses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ArticleClass.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ProductGroup.ProductGroup>), 200)]
		public IActionResult GetArticleClass(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ArticleClass.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArticleClass(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ArticleClass.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddArticleClass(Core.BaseData.Models.Article.ArticleClass.ArticleClass data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ArticleClass.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		// >>> Packaging
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListPackagings()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Packaging.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Packaging.Packaging>>), 200)]
		public IActionResult GetAllPackagings()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Packaging.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Packaging.Packaging>), 200)]
		public IActionResult GetPackaging(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Packaging.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePackaging(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Packaging.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddPackaging(Core.BaseData.Models.Article.Packaging.Packaging data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Packaging.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// - Project Classes
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ProjectClass.AddRequestModel>>), 200)]
		public IActionResult GetAllProjectClasses()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectClass.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProjectClass(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectClass.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddProjectClass(Core.BaseData.Models.Article.ProjectClass.AddRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ProjectClass.AddHandler(this.GetCurrentUser(), data)
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
		public IActionResult EditProjectClass(Core.BaseData.Models.Article.ProjectClass.AddRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ProjectClass.UpdateHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		// >>> ProjectType
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListProjectTypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectType.GetListHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ProjectType.ProjectType>>), 200)]
		public IActionResult GetAllProjectTypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectType.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ProjectType.ProjectType>), 200)]
		public IActionResult GetProjectType(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectType.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteProjectType(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.ProjectType.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddProjectType(Core.BaseData.Models.Article.ProjectType.ProjectType data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.ProjectType.AddHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		#region Unit of measure
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<UnitOfMeasureResponseModel>>), 200)]
		public IActionResult GetUnitOfMeasures()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure.GetAllHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult AddUnitOfMeasure(UnitOfMeasureRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure.AddHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult EditUnitOfMeasure(UnitOfMeasureRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure.EditHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult DeleteUnitOfMeasure(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Configuration.UnitOfMeasure.DeleteHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		#endregion UoM


		// >>> SalesExtension
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListTypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetListTypesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.SalesExtension.SalesItemModel>>), 200)]
		public IActionResult GetAllSalesItemTypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetAllHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.SalesExtension.SalesItemModel>>), 200)]
		public IActionResult GetSalesItemsByArticle(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetByArticleHandler(this.GetCurrentUser(), nr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetMaterialCostByArticle(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetArticleMaterialCostHandler(this.GetCurrentUser(), nr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetMaterialCostDBwoCUByArticle(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetArticleMaterialCostDBwoCUHandler(this.GetCurrentUser(), nr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<decimal>), 200)]
		public IActionResult GetMaterialCostDBwoCUPercentByArticle(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetArticleMaterialCostDBwoCUPercentHandler(this.GetCurrentUser(), nr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.SalesExtension.SalesItemModel>), 200)]
		public IActionResult GetSalesItem(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.GetHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSalesItem(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.DeleteHandler(this.GetCurrentUser(), id)
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
		public IActionResult AddSalesItem(Core.BaseData.Models.Article.SalesExtension.SalesItemModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SalesExtension.AddHandler(this.GetCurrentUser(), data)
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
		public IActionResult EditSalesItem(Core.BaseData.Models.Article.SalesExtension.SalesItemModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateSalesItem");
				var response = new Core.BaseData.Handlers.Article.SalesExtension.UpdateSalesItemsHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// >>>>> CustomPrice // StaffelPreis
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.SalesExtension.CustomPrice.CustomPriceModel>>), 200)]
		public IActionResult GetCustomPriceByArticle(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.CustomPrice.GetByArticleHandler(this.GetCurrentUser(), nr)
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
		public IActionResult AddCustomPrice(Core.BaseData.Models.Article.SalesExtension.CustomPrice.CustomPriceModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.SalesExtension.CustomPrice.AddCustomPriceHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateCustomPrice(Core.BaseData.Models.Article.SalesExtension.CustomPrice.CustomPriceModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateCustomPrice");
				var response = new Core.BaseData.Handlers.Article.SalesExtension.CustomPrice.UpdateCustomPriceHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListCustomPriceTypes()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.CustomPrice.GetListTypesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult DeleteCustomPrice(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.SalesExtension.CustomPrice.DeleteHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#region quality
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateQualityAttachment([FromForm] Models.Articles.UpdateAttachmentModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.BaseData.Handlers.Article.UpdateAttachmentHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Quality.ArticleQualityModel>), 200)]
		public IActionResult GetArticleQuality(int nr)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Quality.GetArticleQualityHandler(this.GetCurrentUser(), nr)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Quality.ArticleQualityModel>), 200)]
		public IActionResult UpdateArticleQuality(Core.BaseData.Models.Article.Quality.ArticleQualityModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateQuality");
				var response = new Core.BaseData.Handlers.Article.Quality.UpdateArticleQualityHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		#endregion
		#region Article Logistics
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Logistics.ArticleLogisticsModel>), 200)]
		//public IActionResult GetArticleLogistics(int ArticleID)
		//{
		//    try
		//    {
		//        return Ok(new Core.BaseData.Handlers.Article.Logistics.GetArticleLogisticsHandler(this.GetCurrentUser(), ArticleID)
		//            .Handle());
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Quality.ArticleQualityModel>), 200)]
		//public IActionResult UpdateArticleLogistics(Core.BaseData.Models.Article.Logistics.ArticleLogisticsModel data)
		//{
		//    try
		//    {
		//        var response = new Core.BaseData.Handlers.Article.Logistics.UpdateArticleLogisticsHandler(this.GetCurrentUser(), data)
		//           .Handle();

		//        return Ok(response);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        return this.HandleException(e);
		//    }
		//}
		#endregion


		// Logistics
		// Data

		// - 2022-03-10
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetProductionLagers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.GetProductionLagersHandler(this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// - CustomerService

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Cts.ArticleCtsModel>), 200)]
		public IActionResult GetArticleCts(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Cts.GetArticleCtsHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Cts.ArticleOpenFA>>), 200)]
		public IActionResult GetArticleCtsOpenFas(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Cts.GetArticleOpenFAsHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateCts(Core.BaseData.Models.Article.Cts.ArticleCtsUpdateRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Cts.UpdateArticleCtsHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		// - 2022-08-09 - copy & update from XLS
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult Copy(Core.BaseData.Models.Article.CreateFromCopyRequestModel data)
		//{
		//	try
		//	{
		//		Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article CreateFromCopy");
		//		var response = new Core.BaseData.Handlers.Article.CreateFromCopyHandler(this.GetCurrentUser(), data)
		//		   .Handle();

		//		return Ok(response);
		//	}
		//	catch (Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult CopyXLS([FromForm] Models.Articles.Bom.PositionImportXLSModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article CopyXLS");
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}

				var file = data.AttachmentFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return BadRequest("No file sent.");
				}

				if(file.Length > 0)
				{
					// Save file to temp dir
					var tempFilePath = System.IO.Path.GetTempPath();
					var filePath = System.IO.Path.Combine(tempFilePath, System.DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

					var fileName = System.IO.Path.GetFileName(file.FileName);
					if(!System.IO.Directory.Exists(tempFilePath))
					{
						System.IO.Directory.CreateDirectory(tempFilePath);
					}

					using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					return Ok(
						new Core.BaseData.Handlers.Article.CreateFromCopyXLSHandler(user,
						new Core.BaseData.Models.Article.CreateFromCopyXLSRequestModel
						{
							ArticleNr = data.ArticleId,
							XLSPath = filePath
						}).Handle());
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		// - 2022-09-01
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<string>>), 200)]
		public IActionResult GetArticleNumberForCreate(Core.BaseData.Models.Article.CreateRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetArticleNumberForCreateHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.CustomerResponseModel>>), 200)]
		public IActionResult GetCustomersForCreate()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetCustomersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.CountryResponseModel>>), 200)]
		public IActionResult GetCountriesForCreate()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetCountriesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.HallResponseModel>>), 200)]
		public IActionResult GetHallsForCreate()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetHallsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticlesForCopy(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetArticlesForCopyHandler(this.GetCurrentUser(), articleNumber)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleNumber);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Common.Models.ResponseModel<string>>), 200)]
		public IActionResult GetArticlesUnique()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetArticlesUniqueHandler(this.GetCurrentUser())
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
		public IActionResult Copy(Core.BaseData.Models.Article.CopyRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article Copy");
				var response = new Core.BaseData.Handlers.Article.CopyHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomerItemNumbers(Core.BaseData.Models.Article.CustomerItemNumbersRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article GetCustomerItemNumbers");
				var response = new Core.BaseData.Handlers.Article.Create.GetCustomerItemNumbersHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomerIndexes(Core.BaseData.Models.Article.CustomerIndexesRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article GetCustomerIndexes");
				var response = new Core.BaseData.Handlers.Article.Create.GetCustomerIndexesHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateCustomerIndex(Core.BaseData.Models.Article.UpdateCustomerIndexRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateCustomerIndex");
				var response = new Core.BaseData.Handlers.Article.Data.UpdateCustomerIndexHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetFaForIndexUpdate(KeyValuePair<int, string> data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetFaForIndexUpdateHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateCustomerIndexDirect(Core.BaseData.Models.Article.UpdateCustomerIndexRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateCustomerIndexDirect");
				var response = new Core.BaseData.Handlers.Article.Data.UpdateCustomerIndexDirectHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Tuple<Core.BaseData.Models.Article.ArticleMinimalModel, List<Core.BaseData.Models.Article.ArticleMinimalModel>>>>), 200)]
		public IActionResult GetUBGNewerIndex(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetUBGNewerIndexHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult IsHBG(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.IsHBGHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Data.UpgradableHUBGResponseModel>), 200)]
		public IActionResult GetUpgradableHUBG(Core.BaseData.Models.Article.Data.UpgradableHUBGRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetUpgradableHUBGHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Data.UpgradableHUBGItem>>), 200)]
		public IActionResult GetUpgradableHBG(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetUpgradableHBGHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Data.UpgradableHUBGItem>>), 200)]
		public IActionResult GetUpgradableUBG(int articleId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.GetUpgradableUBGHandler(this.GetCurrentUser(), articleId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Data.UpgradableHUBGResponseModel>), 200)]
		public IActionResult UpgradeHUBG(Core.BaseData.Models.Article.Data.UpgradableHUBGResponseModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Data.UpgradeHUBGHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateGoodsGroup(Core.BaseData.Models.Article.UpdateGoodsGroupRequestModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateGoodsGroup");
				var response = new Core.BaseData.Handlers.Article.Data.UpdateGoodsGroupHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetEFCustomer(Core.BaseData.Models.Article.Overview.EFByCustomerRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetEFByCustomerHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetEFSiblings(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetEFSiblingsHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetCNumberSiblings(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetCNumberSiblingsHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SetEdiDefault(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.SetEdiDefaultHandler(this.GetCurrentUser(), id).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetIndexSiblings(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetIndexSiblingsHandler(this.GetCurrentUser(), id)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.ArticleMinimalModel>>), 200)]
		public IActionResult GetIndexSiblings(Core.BaseData.Models.Article.IndexSiblingsRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetIndexSiblingsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.CountryFullResponseModel>>), 200)]
		public IActionResult GetAvailableProductionPlaces(Core.BaseData.Models.Article.IndexSiblingsRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Create.GetAvailableProductionPlacesHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Overview.BlanketResponseModel>>), 200)]
		public IActionResult GetBlanketsVnext(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Overview.GetBlanketsVnextHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UploadArticleAttachmentFiles([FromForm] Psz.Core.BaseData.Models.Article.ArticleFilesUploadModel data)
		{
			try
			{

				return Ok(new Core.BaseData.Handlers.Article.ArticleFilesUploadHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//GetFilesNameFromDirectoryHandler

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetFilesNameFromDirectoryHandler(Psz.Core.BaseData.Models.Article.GetArticleAttachmentModel data)
		{
			try
			{

				return Ok(new Core.BaseData.Handlers.Article.GetFilesNameFromDirectoryHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//ReadFileFromDiskHandler
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.DownloadArticleFileResponseModel>), 200)]
		public IActionResult ReadFileFromDisk(string Artiklenummer, string KundeIndex, string filename)
		{
			try
			{
				var data = new Psz.Core.BaseData.Models.Article.DownloadArticleFileModel()
				{
					ArtiKleNummer = Artiklenummer,
					KundenIndex = KundeIndex,
					FileName = filename,
				};
				var file = new Core.BaseData.Handlers.ReadFileFromDiskHandler(this.GetCurrentUser(), data).Handle();

				return new FileContentResult(file.Body.file, file.Body.MIMEtype)
				{
					FileDownloadName = $"{file.Body.FileName}"
				};




			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//GetTechnikerKundeHandler
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.DownloadArticleFileResponseModel>), 200)]
		public IActionResult GetTechnikerKunde(string data)
		{
			try
			{

				var response = new Psz.Core.BaseData.Handlers.Article.GetTechnikerKundeHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);




			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePreviousManufacturerArticle(Core.BaseData.Models.Article.Overview.UpdateManufacturerArticleRequestModel data)
		{
			try
			{
				return Ok(_articleService.UpdatePreviousManufacturerArticle(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateNextManufacturerArticle(Core.BaseData.Models.Article.Overview.UpdateManufacturerArticleRequestModel data)
		{
			try
			{
				return Ok(_articleService.UpdateNextManufacturerArticle(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ResetNextManufacturerArticle(int data)
		{
			try
			{
				return Ok(_articleService.ResetNextManufacturerArticle(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ResetPreviousManufacturerArticle(int data)
		{
			try
			{
				return Ok(_articleService.ResetPreviousManufacturerArticle(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticlePmData(Core.BaseData.Models.Article.ArticleOverviewModel data)
		{
			try
			{
				return Ok(_articleService.UpdateArticlePmData(this.GetCurrentUser(), data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult DeleteFilesNameFromDirectory(Psz.Core.BaseData.Models.Article.DeleteArticleFileModel data)
		{
			try
			{

				return Ok(new Core.BaseData.Handlers.Article.DeleteFilesNameFromDirectoryHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#region Offer request
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<int>>), 200)]
		public IActionResult CreateArticleOfferRequests(OfferrequestVM offers)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.CreateArticleOfferRequestsHandler(this.GetCurrentUser(), offers).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<int>>), 200)]
		public IActionResult VerifySelectedSupplierInformations(List<int> SupplierId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.VerifySelectedSupplierInformationsHandler(this.GetCurrentUser(), SupplierId).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ArticleOfferRequestsModel>>), 200)]
		public IActionResult GetAllArticleOfferRequests(FilterdOfferRequestModel filter)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetAllArticleOfferRequestsHandler(this.GetCurrentUser(), filter).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		/*[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult GenerateEMailForOfferReques(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GenerateEMailForOfferRequestHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}*/
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult GenerateEMailForOfferRequesByUI(string id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GenerateEMailForOfferRequestByUIHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ChangeEmailLanguageModel>), 200)]
		public IActionResult GenerateDefaultEmailByLanguageHandler(GenerateEmailForOneSupplierModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GenerateDefaultEmailByLanguageHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult SendEmailsForOfferReques(List<int> data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.SendOfferRequestEmailToSuppliersIHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//RemoveAttachementFromOfferEmailHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult RemoveAttachementFromOfferEmail(RemoveSingleAttachementFromEmailRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.RemoveAttachementFromOfferEmailHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult AddAttachementToOfferEmail([FromForm] AddAttachementToEmailRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.AddAttachementToOfferEmailHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}



		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult EditEmailsContentForOfferRequest(EditOfferRequestEMail data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.EditEmailContentForOfferRequestHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult GetEMailForOfferRequestByIdsForSingleSupplier(List<int> data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetEMailForOfferRequestByIdsForSingleSupplierHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<OfferRequestEMail>>), 200)]
		public IActionResult GetOfferRequestEmailPerRequestUI(string id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetOfferRequestEmailPerRequestUIHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult EditOfferForSingleManufacturerNumber(EditOfferForSingleManufacturerNumberModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.EditOfferForSingleManufacturerNumberHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<EditOfferForSingleManufacturerNumberModel>), 200)]
		public IActionResult GetSingleArticleOfferRequestbyId(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetSingleArticleOfferRequestbyIdHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteSingleArticleOfferRequestbyId(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.DeleteSingleArticleOfferRequestbyIdHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CancelSingleArticleOfferRequestbyId(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.CancelOfferRequestHanlder(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<CloseOfferMinimalModel>), 200)]
		public IActionResult GetOfferRequestById(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetOfferRequesByIdHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<CloseOfferMinimalModel>), 200)]
		public IActionResult GetOfferRequestById2(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetOfferRequesByIdHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<IPaginatedResponseModel<GetLogChangesOfferRequestWithTotalCountResponseModel>>), 200)]
		public IActionResult GetLogChangesOfferRequest(Psz.Core.BaseData.Models.ROH.GetOfferRequestLogsRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetLogChangesOfferRequestHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CloseOfferForSingleManufacturerNumber(CloseOfferRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.CloseOfferForSingleManufacturerNumberHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ClosedOfferRequestIncludingAttachment([FromForm] CloseOfferRequestIncludingAttachmentModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.ClosedOfferRequestIncludingAttachmentHanlder(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddOfferToEKPriceRelation(OfferToArticleEKModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.AddOfferToEKPriceRelationHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddArtikelCustomerRef(AddArtikelCustomerReferencesRequestModel data)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.AddArtikelCustomerReferencesHandler(this.GetCurrentUser(), data).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArtikelCustomerRef(EditArtikelCustomerReferencesRequestModel data)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.EditArtikelCustomerReferencesHandler(this.GetCurrentUser(), data).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArtikelCustomerRef(int id)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.DeleteArtikelCustomerReferencesHandler(this.GetCurrentUser(), id).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, id);
			}
		}
		//GetLikeReferenceHandler
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<GetArtikelCustomerReferencesModel>>), 200)]
		public IActionResult GetAllArtikelCustomerRef(int ArtikelId)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.GetArtikelCustomerReferencesHandler(this.GetCurrentUser(), ArtikelId).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, ArtikelId);
			}
		}
		//
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<GetLikeCustomerArticleReferenceModel>>), 200)]
		public IActionResult GetLikeReferenceHandler(string searchtext)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.GetLikeReferenceHandler(this.GetCurrentUser(), searchtext).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, searchtext);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetAllROHArtikelUnits()
		{
			try
			{
				var response = new Psz.Core.BaseData.Handlers.Article.GetAllROHArtikelUnitsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult GetFilteredByCustomerReferenceAndCustomerNumber(List<GetLikeCustomerArticleReferenceRequestModel> filter)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.GetFilteredByCustomerReferenceAndCustomerNumberHandler(this.GetCurrentUser(), filter).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, filter);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ArticleInformationMinimalModel>), 200)]
		public IActionResult GetArticleNrByCustomerReferenceAndCustomerNumber(GetLikeCustomerArticleReferenceRequestModel filter)
		{
			try
			{
				var result = new Psz.Core.BaseData.Handlers.Article.ArticleReference.GetArticleNrByCustomerReferenceAndCustomerNumberHandler(this.GetCurrentUser(), filter).Handle();
				return Ok(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, filter);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.ArticleModel>), 200)]
		public IActionResult GetArticleByManNr(string id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.GetByManNrHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult GetSupplierEmail(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.ROH.OfferRequests.GetSupplierEmailHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		[SupportedOSPlatform("windows")]
		public async Task<IActionResult> UploadPackagingsAttachmentFiles([FromForm] PkgPhotoLgtUploadModel data)
		{
			try
			{

				return Ok(await new Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files.PkgPhotoLgtUploadHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		[SupportedOSPlatform("windows")]
		public async Task<IActionResult> GetPackagingsPhotoFilesNameFromDirectory(int ArticleId)
		{
			try
			{
				return Ok(await new Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files.GetPhotoPackagingsFromDirectoryHandler(this.GetCurrentUser(), ArticleId, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> DeletePackagingsPhotoFromDirectory(DeletePkgPhotoFileModel data)
		{
			try
			{
				return Ok(await new Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files.DeleteFilePhotoHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public async Task<IActionResult> ReadPackagingsFromDisk(int Id)
		{
			try
			{
				var data = Id;
				var file = await new Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files.ReadPackagingsFilesHandler(this.GetCurrentUser(), data, this.HttpContext.RequestAborted).HandleAsync();

				if(file.Success && file.Body.file.Length > 0)
				{
					return new FileContentResult(file.Body.file, file.Body.MIMEtype)
					{
						FileDownloadName = $"{file.Body.FileName}"
					};
				}
				else
				{
					return Ok(await ResponseModel<FileContentResult>.FailureResponseAsync("Unable to fetch file"));
				}

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		[SupportedOSPlatform("windows")]
		public async Task<IActionResult> GetIdPackagingsPhotoFilesNameFromDirectory(int ArticleId)
		{
			try
			{
				return Ok(await new Psz.Core.BaseData.Handlers.ArticlePackagingsPhoto.Files.GetPhotoIdPackagingsFromDirectoryHandler(this.GetCurrentUser(), ArticleId, this.HttpContext.RequestAborted).HandleAsync());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
