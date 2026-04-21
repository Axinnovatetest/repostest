using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Handlers.FA;
using Psz.Core.CRP.Handlers.FA.DatesHistoryChanges;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FA;
using Psz.Core.CustomerService.Interfaces;
using Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class FAController: ControllerBase
	{
		private const string MODULE = "CRP";

		private IInsideSalesWerksterminUpdates _insideSalesWerksterminUpdate;
		private ICrpFaChangesHistoryService _crpFaChangesHistoryService;
		public FAController(IInsideSalesWerksterminUpdates insideSalesWerksterminUpdate, ICrpFaChangesHistoryService crpFaChangesHistoryService)
		{
			this._insideSalesWerksterminUpdate = insideSalesWerksterminUpdate;
			this._crpFaChangesHistoryService = crpFaChangesHistoryService;
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<FASearchResponseModel>>), 200)]
		public IActionResult Search(FASearchModel data)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.SearchFAHandler(data, this.GetCurrentUser())
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
		public IActionResult GetAutocompleteArticleForUpdate(string article)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Autocomplete.AutocompleteArticleForUpdateHandler(article, this.GetCurrentUser())
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
		public IActionResult GetAutocompleteArticle(string article)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Autocomplete.AutocompleteArticleHandler(article, this.GetCurrentUser())
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
		public IActionResult GetAutocompleteClient(string client)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Autocomplete.AutocompleteClientHandler(client, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetAutocompleteFA(string fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Autocomplete.AutocompleteFAHandler(fa, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetAutocompleteFAForAnalyse(int lager, string term)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Autocomplete.AutoCompleteShneidereiKabelGeschnittenHandler(lager, term, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<FAAnalyseShneidereiKabelGeschnittenDataModel>), 200)]
		public IActionResult GetFAAnalyseData(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Plannung.FAAnalyseShneidereiKabelGeschnittenDataHandler(fa, this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetFAAnalyseGewerk1(int lager, DateTime date)
		{
			try
			{
				var results = new Core.CRP.Handlers.FA.Plannung.GetFAAnalayseGewerk1Handler(this.GetCurrentUser(), lager, date).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Analyse gewerk 1-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAAnalyseGewerk2(int lager, DateTime date)
		{

			try
			{
				var results = new Core.CRP.Handlers.FA.Plannung.GetFAAnalayseGewerk2Handler(this.GetCurrentUser(), lager, date).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Analyse gewerk 2-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAAnalyseGewerk3(int lager, DateTime date)
		{
			try
			{
				var results = new Core.CRP.Handlers.FA.Plannung.GetFAAnalayseGewerk3Handler(this.GetCurrentUser(), lager, date).Handle();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Analyse gewerk3-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetFAKommisionert(DateTime date)
		{
			try
			{
				var data = new Core.CRP.Handlers.FA.GetFAKommisionertHandler(date).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"Kommisionert-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetLagersPlannung(DateTime Datum_bis, string Produktionsort)
		{
			try
			{
				var data = new Core.CRP.Handlers.FA.GetFALagersPlannungHandler(Datum_bis, Produktionsort, this.GetCurrentUser()).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"Plannung TN-WS-CZ-AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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
		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetLagersPlannungForXLS(DateTime Datum_bis, string Produktionsort)
		{
			try
			{
				var data = new Core.CRP.Handlers.FA.GetFALagersPlannungForXLSHandler(Datum_bis, Produktionsort, this.GetCurrentUser()).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"Plannung TN-WS-CZ-AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetProduktionPlannung(DateTime Datum_bis, string Produktionsort, bool? Technikauftrag, string Artikelnummer)
		{
			try
			{
				var data = new Core.CRP.Handlers.FA.GetFAProduktionPlannungHandler(Datum_bis, Produktionsort, Technikauftrag, Artikelnummer).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"ProduktionPlannung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetPlannungTechnik(string techniker, string Produktionsort)
		{
			try
			{
				var data = new Core.CRP.Handlers.FA.GetFATechnickPlanungHandler(techniker, Produktionsort).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"Plannung Technik-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetPlannungSchneiderei(int land)
		{
			try
			{
				var results = new Core.CRP.Handlers.FA.Plannung.GetPlannungSchneidereiHandler(this.GetCurrentUser(), land).Handle();
				if(results.Success && results.Body?.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Plannung Schneiderei-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<FAErlidgtLogModel>>), 200)]
		public IActionResult GetErlidgtFullLog()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Settings.FAErlidgtLogHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetDraft(int type)
		{
			try
			{
				var results = new Core.CRP.Handlers.FA.Settings.FAWunshWerkDraftHandler(this.GetCurrentUser(), type).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"DRAFT--{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<TerminFAUpdateLogModel>>), 200)]
		public IActionResult GetTerminUpdateFALog(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Settings.GetTerminFAUpdateLogHandler(this.GetCurrentUser(), fa)
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<FAWunchWerkLogModel>>), 200)]
		public IActionResult GetWunshWerkFullLog()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Settings.GetWunchWerkLogHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetTerminUpdateFAList()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Settings.TerminUpdateFAListHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddTechnik(FATechnikModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Technik.AddTechnikHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteTechnik(int id)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Technik.DeleteTechnikHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFATechnik(FADetailsModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Technik.UpdateFATechnikHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateTechnik(FATechnikModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Technik.UpdateTechnikHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFAAnalyse(FAAnalyseShneidereiKabelGeschnittenDataModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Update.AnalyseFAUpdateHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult StornoFA_2(FAStornoModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Update.FAStorno2Handler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFA_2(FAUpdateModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Update.FAUpdate2Handler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.CRP.Models.FA.Update.AllOpenFAForUpdateModel>), 200)]
		public IActionResult GetOpenFAForUpdate(string article)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Update.GetAllOpenFAForUpdateHandler(article, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFAauftragsbearbeitung(FADetailsModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.UpdateFAauftragsbearbeitungHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFAPlannung(FADetailsModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.UpdateFAPlannungHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFATermin(FAUpdateTerminModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.UpdateFATerminHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateFAUBG(ChangeFAUBGEntryModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Update.UpdateFAUBGHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CheckAuswertungEndkontrolle()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.CheckAuswertungEndkontrolleHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult CheckBeforeStornoFA(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.CheckBeforeStornoHandler(fa, this.GetCurrentUser())
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
		public IActionResult CreateFA_2(FACreateModel data)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.CreateFA2Handler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult FaErlidigt_2(FAErlidigtEntryModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.FAErlidigt2Handler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetFAArticleProductionSite(int articleId)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAArticleProductionSiteHandler(this.GetCurrentUser(), articleId)
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<FADetailsModel>), 200)]
		public IActionResult GetFaDetails(int order)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFADetailsHandler(order, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetFaErlidigt(int id)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAErlidigtHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticlesListForFA(string text)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAStucklistArticlesHandler(text, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<FAStucklistModel>>), 200)]
		public IActionResult GetFaStucklist(int faId)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAStucklistHandler(faId, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<FAUpdateTerminModel>), 200)]
		public IActionResult GetFATerminBeforeUpdate(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFATerminUpdateHandler(fa, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetBomVersionsForUpdate(string articleID, string index)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAUpdateBomVersionsHandler(articleID, index, this.GetCurrentUser())
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
		public IActionResult GetIndexesForUpdate(string articleID)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAUpdateIndexesHandler(articleID, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		public IActionResult GetOpenFAsLagersForUpdate(int articleID)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAUpdateLagersHandler(articleID, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetLastCPVersionForUpdate(string article, int bomversion)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetFAUpdateLastCPVersionHandler(article, bomversion, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetHauptLager()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetHauptlagersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetLagersListForFA()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetLagersForFAStucklistHandler(this.GetCurrentUser())
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
		public IActionResult GetFaStatus()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetListFAStatusHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetMandantList()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetMandantListHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetPlannungSchneidereiLands()
		{
			try
			{
				return Ok(new Core.CRP.Handlers.FA.GetPlannungSchneidereiLandsHandler(this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<PotentialHBGFaResponseModel>>), 200)]
		public IActionResult GetPotentialHBGFA(PotentialHBGFaRequestModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetPotentialHBGFaHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<PotentialUBGFaResponseModel>>), 200)]
		public IActionResult GetPotentialUBGFA(PotentialHBGFaRequestModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetPotentialUBGFaHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetTechniciens()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetTechniciensHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<UBGProductionResponseModel>>), 200)]
		public IActionResult GetUBGFromBOMForFA(UBGProductionRequestModel data)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetUBGFromBOMForFAHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<UBGProductionResponseModel>>), 200)]
		public IActionResult GetUBGFromBOM(int articleId)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetUBGFromBOMHandler(articleId, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUBGFromFABOM(UBGFromFaBOMRequestModel data)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.GetUBGFromFABOMHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult LinkFaPositionToUBG(LinkFaPositionHBGRequestModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.LinkFaPositionToUBGHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UnlinkFaPositionFromUBG(LinkFaPositionHBGRequestModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.UnlinkFaPositionFromUBGHandler(model, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult VerifyFABeforeDruck(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.VerifyFABeforeDruckHandler(fa, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult VerifyLaufkarteSchneiderei(int fa)
		{
			try
			{
				return Ok(new Core.CRP.Handlers.FA.VerifyFALaufkarteHandler(fa, this.GetCurrentUser())
				   .Handle());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel>), 200)]
		public IActionResult UpdateFAByArticle_2(Core.CRP.Models.FA.Update.AllOpenFAForUpdateModel model)
		{
			try
			{
				return Ok(new Core.CRP.Handlers.FA.Purchase.FAUpdateByArticle2Handler(model, this.GetCurrentUser())
				   .Handle());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetAuswertungEndkontrolle()
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetAuswertungEndkontrolleHandler(this.GetCurrentUser())
				   .Handle();

				if(response.Body != null)
				{
					return new FileContentResult(response.Body, "application/pdf")
					{
						FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf"
					};
				}
				else
					return new FileContentResult(new byte[] { }, "application/pdf")
					{
						FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf"
					};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAAnalyseFehlmaterial(int fa, DateTime date, int id)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetFAAnalyseFehlmaterialHandler(this.GetCurrentUser(), fa, date, id)
				   .Handle();

				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, date);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAReport(int fa)
		{
			try
			{
				var x = this.GetCurrentUser();
				var response = new Core.CRP.Handlers.FA.Purchase.GetFADruckHandler(this.GetCurrentUser(), fa)
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAFehlematerial(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetFAFehlematerialHandler(this.GetCurrentUser(), fa)
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public IActionResult GetFAStappleFinalReport(Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel model)
		{
			var response = new Core.CRP.Handlers.FA.Purchase.GetFAStappleDruckFinalReportHandler(model, this.GetCurrentUser())
				   .Handle();

			return Ok(response);
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel>), 200)]
		public IActionResult FAStapleDruck2(StappleDruckInputModel model)
		{
			var response = new Core.CRP.Handlers.FA.Purchase.GetFAStappleDruckListHandler(model, this.GetCurrentUser())
				   .Handle();

			return Ok(response);
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetUpdatedPDF(Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel model)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetUpdatedFaPDFHandler(model, this.GetCurrentUser())
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetWerkUpdateReport(int Idupadate)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetWerkUpdateReportHandler(this.GetCurrentUser(), Idupadate)
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetWunshUpdateReport(int Idupadate)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.GetWunshUpdateReportHandler(this.GetCurrentUser(), Idupadate).Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>), 200)]
		public IActionResult ImportWerkFromXLS([FromForm] FAImportExcelModel data)
		{
			try
			{
				// AllowAnonymous <<<<<<<
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
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

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
						new Core.CRP.Handlers.FA.Purchase.ImportWerkFromXLSHandler(user,
						new Core.Common.Models.ImportFileModel
						{
							FilePath = filePath,
						}).Handle());
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>), 200)]
		public IActionResult ImportWunshFromXLS([FromForm] FAImportExcelModel data)
		{
			try
			{
				// AllowAnonymous <<<<<<<
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
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

					var fileName = System.IO.Path.GetFileName(file.FileName);
					if(!System.IO.Directory.Exists(tempFilePath))
					{
						System.IO.Directory.CreateDirectory(tempFilePath);
					}

					using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					//var response =
					return Ok(
						new Core.CRP.Handlers.FA.Purchase.ImportWunshHandler(user,
						new Core.Common.Models.ImportFileModel
						{
							FilePath = filePath,
						}).Handle());
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLaufkarteSchneiderei(int fa)
		{
			try
			{
				var response = new Core.CRP.Handlers.FA.Purchase.LaufkarteSchneidereiHandler(this.GetCurrentUser(), fa)
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<InsideSalesWerkserminUpdatesModel>>), 200)]
		public IActionResult getFAWithChangedWerkstermin(bool? insConfirmation)
		{
			try
			{
				return Ok(this._insideSalesWerksterminUpdate.getFAWithChangedWerkstermin(this.GetCurrentUser(), insConfirmation));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult updateIns(InsConfirmationWerkterminModel data)
		{
			try
			{
				return Ok(this._insideSalesWerksterminUpdate.UpdateInsConfirmation(data, this.GetCurrentUser()));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<byte[]>), 200)]
		public IActionResult getFAWithChangedWerkterminHistory_XLS(bool? insConfirmation)
		{
			try
			{
				var results = _insideSalesWerksterminUpdate.getFAWithChangedWerkterminHistory_XLS(this.GetCurrentUser(), insConfirmation);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<int>>), 200)]
		public IActionResult GetFaByArticleNr(KeyValuePair<int, string> data)
		{
			try
			{
				var response = new GetFaByArticleNrHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomersForFACration()
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.GetCustomersForFACreationHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<KeyValuePair<int, string>>), 200)]
		public IActionResult GetTypesForFACreation(int artikelNr)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.GetTypesForFACreationHandler(this.GetCurrentUser(), artikelNr).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<decimal>), 200)]
		public IActionResult GetArticleProductionCostForFACreation(Psz.Core.CRP.Models.FA.ArticleProductionCostForFACreationRequestModel data)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.GetArticleProductionCostForFACreationHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<string>), 200)]
		public IActionResult GetArticleCustomerContactForFACreation(int artikelNr)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.GetArticleCustomerContactForFACreationHandler(this.GetCurrentUser(), artikelNr).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<Core.CRP.Models.FA.ArticleWarehouseAndCustomerForFACreationModel>), 200)]
		public IActionResult GetArticleWarehouseAndCustomerForFACreation(int artikelNr)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.GetArticleWarehouseAndCustomerForFACreationHandler(this.GetCurrentUser(), artikelNr).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<string>), 200)]
		public IActionResult AddFaUserEmail(AddFaUserRequestModel request)
		{
			try
			{
				var response = new AddFaUserEmailHandler(this.GetCurrentUser(), request).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, request);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<FaUserEmailResponseModel>>), 200)]
		public IActionResult GetFaUserEmails()
		{
			try
			{
				var response = new GetFaUserEmailsHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult RemoveFaUserEmail([FromQuery] int userId)
		{
			try
			{
				return Ok(new RemoveFaUserEmailHandler(this.GetCurrentUser(), userId)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, userId);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<GetFaChangesHistoryResponseModel>), 200)]
		public IActionResult GetFaDatesChangeHistory(FaChangesHistoryRequestModel _data)
		{
			try
			{
				var response = this._crpFaChangesHistoryService.GetFaDatesChangeHistory(this.GetCurrentUser(), _data);

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<GetFaHoursChangesResponseModel>), 200)]
		public IActionResult GetFaHoursMovement(FaHoursChangesRequestModel _data)
		{
			try
			{
				var response = this._crpFaChangesHistoryService.GetFaHoursMovement(this.GetCurrentUser(), _data);

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<byte[]>), 200)]
		public IActionResult GetFaDatesChangesExcel(FaChangesHistoryRequestModel request)
		{
			try
			{
				var result = this._crpFaChangesHistoryService.GetFaDatesChangesHistoryXLS(this.GetCurrentUser(), request);

				if(result.Success && result.Body != null && result.Body.Length > 0)
				{
					return File(result.Body, "application/xlsx", $"Get-Fa-Dates-changes-history-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FaDataChartResponseModel>), 200)]
		public IActionResult GetFaHoursMovementChartData(FaChartDataRequestModel data)
		{
			try
			{
				var response = _crpFaChangesHistoryService.GetFaMovementChartData(this.GetCurrentUser(), data);

				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}		

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<FASearchResponseModel>>), 200)]
		public IActionResult GetFaPlanningViolation(FaPlanningViolationRequestModel data)
		{
			try
			{
				var response = this._crpFaChangesHistoryService.GetFaPlanningViolation(this.GetCurrentUser(), data);

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult UpdateFAPrio(Psz.Core.CRP.Models.FA.Update.UpdateFAPrioRequestModel data)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.FA.Update.UpdateFaPrioHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}