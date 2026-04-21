using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CustomerService.Models.FA;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class FAController: ControllerBase
	{
		private const string MODULE = "CustomerService";

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.FA.FASearchResponseModel>>), 200)]
		//public IActionResult Search(Core.CustomerService.Models.FA.FASearchModel data)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.SearchFAHandler(data, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.FA.FADetailsModel>), 200)]
		//public IActionResult GetFaDetails(int order)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFADetailsHandler(order, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetAutocompleteArticle(string article)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.AutocompleteArticleHandler(article, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetAutocompleteArticleForUpdate(string article)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Autocomplete.AutocompleteArticleForUpdateHandler(article, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetAutocompleteClient(string client)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.AutocompleteClientHandler(client, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetAutocompleteFA(string fa)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.AutocompleteFAHandler(fa, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		//public IActionResult GetAutocompleteFAForAnalyse(int lager, string term)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Autocomplete.AutoCompleteShneidereiKabelGeschnittenHandler(lager, term, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetTechniciens()
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetTechniciensHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetFaStatus()
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetListFAStatusHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetMandantList()
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetMandantListHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetArticlesListForFA(string text)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAStucklistArticlesHandler(text, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetLagersListForFA()
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetLagersForFAStucklistHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetHauptLager()
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetHauptlagersHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.FA.FAStucklistModel>>), 200)]
		//public IActionResult GetFaStucklist(int faId)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAStucklistHandler(faId, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost, DisableRequestSizeLimit]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>), 200)]
		//public IActionResult ImportWerkFromXLS([FromForm] Psz.Core.CustomerService.Models.FA.FAImportExcelModel data)
		//{
		//	try
		//	{
		//		// AllowAnonymous <<<<<<<
		//		var user = this.GetCurrentUser();
		//		if(user == null)
		//		{
		//			return Ok("Authentication: User not found");
		//		}

		//		var file = data.AttachmentFile;
		//		if(file == null)
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
		//			return BadRequest("No file sent.");
		//		}

		//		if(file.Length > 0)
		//		{
		//			// Save file to temp dir
		//			var tempFilePath = System.IO.Path.GetTempPath();
		//			var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

		//			var fileName = System.IO.Path.GetFileName(file.FileName);
		//			if(!System.IO.Directory.Exists(tempFilePath))
		//			{
		//				System.IO.Directory.CreateDirectory(tempFilePath);
		//			}

		//			using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
		//			{
		//				file.CopyTo(fileStream);
		//			}
		//			return Ok(
		//				new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.ImportWerkFromXLSHandler(user,
		//				new Core.Common.Models.ImportFileModel
		//				{
		//					FilePath = filePath,
		//				}).Handle());
		//		}
		//		else
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
		//			return BadRequest("Empty file sent.");
		//		}
		//	} catch(System.Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetWerkUpdateReport(int Idupadate)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetWerkUpdateReportHandler(this.GetCurrentUser(), Idupadate)
		//		   .Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost, DisableRequestSizeLimit]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAGeneralWunchUpdateModel>), 200)]
		//public IActionResult ImportWunshFromXLS([FromForm] Psz.Core.CustomerService.Models.FA.FAImportExcelModel data)
		//{
		//	try
		//	{
		//		// AllowAnonymous <<<<<<<
		//		var user = this.GetCurrentUser();
		//		if(user == null)
		//		{
		//			return Ok("Authentication: User not found");
		//		}

		//		var file = data.AttachmentFile;
		//		if(file == null)
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
		//			return BadRequest("No file sent.");
		//		}

		//		if(file.Length > 0)
		//		{
		//			// Save file to temp dir
		//			var tempFilePath = System.IO.Path.GetTempPath();
		//			var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

		//			var fileName = System.IO.Path.GetFileName(file.FileName);
		//			if(!System.IO.Directory.Exists(tempFilePath))
		//			{
		//				System.IO.Directory.CreateDirectory(tempFilePath);
		//			}

		//			using(var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
		//			{
		//				file.CopyTo(fileStream);
		//			}
		//			//var response =
		//			return Ok(
		//				new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.ImportWunshHandler(user,
		//				new Core.Common.Models.ImportFileModel
		//				{
		//					FilePath = filePath,
		//				}).Handle());
		//		}
		//		else
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
		//			return BadRequest("Empty file sent.");
		//		}
		//	} catch(System.Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetWunshUpdateReport(int Idupadate)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetWunshUpdateReportHandler(this.GetCurrentUser(), Idupadate).Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		////[HttpPost]
		////[SwaggerOperation(Tags = new[] { MODULE })]
		////[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Infrastructure.Services.Reporting.Models.CTS.FAGeneralDruckModel>>), 200)]
		////public IActionResult CreateFA(Core.CustomerService.Models.FA.FACreateModel data)
		////{
		////    try
		////    {
		////        var response = new Core.CustomerService.Handlers.FA.CreateFAHandler(data, this.GetCurrentUser())
		////           .Handle();

		////        return Ok(response);
		////    }
		////    catch (Exception e)
		////    {
		////        Infrastructure.Services.Logging.Logger.Log(e);
		////        return this.HandleException(e);
		////    }
		////}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAReport(int fa)
		//{
		//	try
		//	{
		//		var x = this.GetCurrentUser();
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFADruckHandler(this.GetCurrentUser(), fa)
		//		   .Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAStapple(int fa)
		//{
		//	try
		//	{
		//		return Ok(new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFADruckHandler(this.GetCurrentUser(), fa)
		//		   .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//#region Return Excel
		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetProduktionPlannung(DateTime Datum_bis, string Produktionsort, bool? Technikauftrag, string Artikelnummer)
		//{
		//	try
		//	{
		//		var data = new Psz.Core.CustomerService.Handlers.FA.GetFAProduktionPlannungHandler(Datum_bis, Produktionsort, Technikauftrag, Artikelnummer).Handle();
		//		if(data.Body != null)
		//		{
		//			return File(data.Body, "application/xlsx", $"ProduktionPlannung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetLagersPlannung(DateTime Datum_bis, string Produktionsort)
		//{
		//	try
		//	{
		//		var data = new Psz.Core.CustomerService.Handlers.FA.GetFALagersPlannungHandler(Datum_bis, Produktionsort, this.GetCurrentUser()).Handle();
		//		if(data.Body != null)
		//		{
		//			return File(data.Body, "application/xlsx", $"Plannung TN-WS-CZ-AL-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetPlannungTechnik(string techniker, string Produktionsort)
		//{
		//	try
		//	{
		//		var data = new Psz.Core.CustomerService.Handlers.FA.GetFATechnickPlanungHandler(techniker, Produktionsort).Handle();
		//		if(data.Body != null)
		//		{
		//			return File(data.Body, "application/xlsx", $"Plannung Technik-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}


		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetFAKommisionert(string date)
		//{
		//	try
		//	{
		//		var data = new Psz.Core.CustomerService.Handlers.FA.GetFAKommisionertHandler(date).Handle();
		//		if(data.Body != null)
		//		{
		//			return File(data.Body, "application/xlsx", $"Kommisionert-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetFAAnalyseGewerk1(int lager, DateTime date)
		//{
		//	try
		//	{
		//		var results = new Psz.Core.CustomerService.Handlers.FA.Plannung.GetFAAnalayseGewerk1Handler(this.GetCurrentUser(), lager, date).Handle();
		//		if(results.Success && results.Body.Length > 0)
		//		{
		//			return File(results.Body, "application/xlsx", $"Analyse gewerk 1-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//public IActionResult GetDraft(int type)
		//{
		//	try
		//	{
		//		var results = new Psz.Core.CustomerService.Handlers.FA.Settings.FAWunshWerkDraftHandler(this.GetCurrentUser(), type).Handle();
		//		if(results.Success && results.Body.Length > 0)
		//		{
		//			return File(results.Body, "application/xlsx", $"DRAFT--{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}
		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAAnalyseGewerk2(int lager, DateTime date)
		//{

		//	try
		//	{
		//		var results = new Psz.Core.CustomerService.Handlers.FA.Plannung.GetFAAnalayseGewerk2Handler(this.GetCurrentUser(), lager, date).Handle();
		//		if(results.Success && results.Body.Length > 0)
		//		{
		//			return File(results.Body, "application/xlsx", $"Analyse gewerk 2-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}

		//[AllowAnonymous]
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAAnalyseGewerk3(int lager, DateTime date)
		//{
		//	try
		//	{
		//		var results = new Psz.Core.CustomerService.Handlers.FA.Plannung.GetFAAnalayseGewerk3Handler(this.GetCurrentUser(), lager, date).Handle();
		//		if(results.Success && results.Body != null && results.Body.Length > 0)
		//		{
		//			return File(results.Body, "application/xlsx", $"Analyse gewerk3-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		this.HandleException(e);
		//		return Ok(e.Message);
		//	}
		//}
		//#endregion

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAAnalyseFehlmaterial(int fa, DateTime date, int id)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFAAnalyseFehlmaterialHandler(this.GetCurrentUser(), fa, date, id)
		//		   .Handle();

		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, date);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult CheckAuswertungEndkontrolle()
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.CheckAuswertungEndkontrolleHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetAuswertungEndkontrolle()
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetAuswertungEndkontrolleHandler(this.GetCurrentUser())
		//		   .Handle();

		//		if(response.Body != null)
		//		{
		//			return new FileContentResult(response.Body, "application/pdf")
		//			{
		//				FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//			};
		//		}
		//		else
		//			return new FileContentResult(new byte[] { }, "application/pdf")
		//			{
		//				FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//			};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		//public IActionResult VerifyLaufkarteSchneiderei(int fa)
		//{
		//	try
		//	{
		//		return Ok(new Psz.Core.CustomerService.Handlers.FA.VerifyFALaufkarteHandler(fa, this.GetCurrentUser())
		//		   .Handle());

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetLaufkarteSchneiderei(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.LaufkarteSchneidereiHandler(this.GetCurrentUser(), fa)
		//		   .Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		//public IActionResult CheckBeforeStornoFA(int fa)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.CheckBeforeStornoHandler(fa, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		////[HttpPost]
		////[SwaggerOperation(Tags = new[] { MODULE })]
		////[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		////public IActionResult StornoFA(Psz.Core.CustomerService.Models.FA.FAStornoModel model)
		////{
		////    try
		////    {
		////        var response = new Core.CustomerService.Handlers.FA.FAStornoHandler(model, this.GetCurrentUser())
		////           .Handle();

		////        return Ok(response);
		////    }
		////    catch (Exception e)
		////    {
		////        Infrastructure.Services.Logging.Logger.Log(e);
		////        return this.HandleException(e);
		////    }
		////}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult GetFaErlidigt(int id)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAErlidigtHandler(id, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		//public IActionResult GetOpenFAsLagersForUpdate(int articleID)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAUpdateLagersHandler(articleID, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		//public IActionResult GetIndexesForUpdate(string articleID)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAUpdateIndexesHandler(articleID, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		//public IActionResult GetBomVersionsForUpdate(string articleID, string index)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetFAUpdateBomVersionsHandler(articleID, index, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.FA.Update.AllOpenFAForUpdateModel>), 200)]
		//public IActionResult GetOpenFAForUpdate(string article)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Update.GetAllOpenFAForUpdateHandler(article, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetUpdatedPDF(Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetUpdatedFaPDFHandler(model, this.GetCurrentUser())
		//		   .Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult GetLastCPVersionForUpdate(string article, int bomversion)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.GetFAUpdateLastCPVersionHandler(article, bomversion, this.GetCurrentUser())
		//		   .Handle();


		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}


		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFAauftragsbearbeitung(Psz.Core.CustomerService.Models.FA.FADetailsModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.UpdateFAauftragsbearbeitungHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFAPlannung(Psz.Core.CustomerService.Models.FA.FADetailsModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.UpdateFAPlannungHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}


		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.FA.FAUpdateTerminModel>), 200)]
		//public IActionResult GetFATerminBeforeUpdate(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.GetFATerminUpdateHandler(fa, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFATermin(Psz.Core.CustomerService.Models.FA.FAUpdateTerminModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.UpdateFATerminHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}


		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetFAFehlematerial(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFAFehlematerialHandler(this.GetCurrentUser(), fa)
		//		   .Handle();


		//		return new FileContentResult(response.Body, "application/pdf")
		//		{
		//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
		//		};

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		//public IActionResult GetPlannungSchneiderei(int land)
		//{
		//	try
		//	{
		//		var results = new Psz.Core.CustomerService.Handlers.FA.Plannung.GetPlannungSchneidereiHandler(this.GetCurrentUser(), land).Handle();
		//		if(results.Success && results.Body?.Length > 0)
		//		{
		//			return File(results.Body, "application/xlsx", $"Plannung Schneiderei-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
		//		}
		//		else
		//		{
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetPlannungSchneidereiLands()
		//{
		//	try
		//	{
		//		return Ok(new Psz.Core.CustomerService.Handlers.FA.GetPlannungSchneidereiLandsHandler(this.GetCurrentUser())
		//		   .Handle());
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFATechnik(Psz.Core.CustomerService.Models.FA.FADetailsModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Technik.UpdateFATechnikHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}


		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult AddTechnik(Psz.Core.CustomerService.Models.FA.FATechnikModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Technik.AddTechnikHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateTechnik(Psz.Core.CustomerService.Models.FA.FATechnikModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Technik.UpdateTechnikHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult DeleteTechnik(int id)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Technik.DeleteTechnikHandler(id, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.FA.FAAnalyseShneidereiKabelGeschnittenDataModel>), 200)]
		//public IActionResult GetFAAnalyseData(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Plannung.FAAnalyseShneidereiKabelGeschnittenDataHandler(fa, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFAAnalyse(Psz.Core.CustomerService.Models.FA.FAAnalyseShneidereiKabelGeschnittenDataModel model)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Update.AnalyseFAUpdateHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, model);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel>), 200)]
		//public IActionResult FAStapleDruck2(Psz.Core.CustomerService.Models.FA.StappleDruckInputModel model)
		//{
		//	var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFAStappleDruckListHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//	return Ok(response);
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		//public IActionResult GetFAStappleFinalReport(Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel model)
		//{
		//	var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.GetFAStappleDruckFinalReportHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//	return Ok(response);
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.FA.FAWunchWerkLogModel>>), 200)]
		//public IActionResult GetWunshWerkFullLog()
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Settings.GetWunchWerkLogHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.FA.FAErlidgtLogModel>>), 200)]
		//public IActionResult GetErlidgtFullLog()
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Settings.FAErlidgtLogHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, int>>>), 200)]
		//public IActionResult GetTerminUpdateFAList()
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Settings.TerminUpdateFAListHandler(this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.FA.TerminFAUpdateLogModel>>), 200)]
		//public IActionResult GetTerminUpdateFALog(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.Settings.GetTerminFAUpdateLogHandler(this.GetCurrentUser(), fa)
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFAUBG(Psz.Core.CustomerService.Models.ChangeFAUBGEntryModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Update.UpdateFAUBGHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, model);
		//	}
		//}
		//#region APIs using transaction
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(int), 200)]
		//public IActionResult CreateFA_2(Core.CustomerService.Models.FA.FACreateModel data)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.CreateFA2Handler(data, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, data);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<PotentialHBGFaResponseModel>>), 200)]
		//public IActionResult GetPotentialHBGFA(Psz.Core.CustomerService.Models.FA.PotentialHBGFaRequestModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetPotentialHBGFaHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<PotentialUBGFaResponseModel>>), 200)]
		//public IActionResult GetPotentialUBGFA(Psz.Core.CustomerService.Models.FA.PotentialHBGFaRequestModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetPotentialUBGFaHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, model);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult LinkFaPositionToUBG(Psz.Core.CustomerService.Models.FA.LinkFaPositionHBGRequestModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.LinkFaPositionToUBGHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, model);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UnlinkFaPositionFromUBG(Psz.Core.CustomerService.Models.FA.LinkFaPositionHBGRequestModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.UnlinkFaPositionFromUBGHandler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, model);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<UBGProductionResponseModel>>), 200)]
		//public IActionResult GetUBGFromBOM(int articleId)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetUBGFromBOMHandler(articleId, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, articleId);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<UBGProductionResponseModel>>), 200)]
		//public IActionResult GetUBGFromBOMForFA(UBGProductionRequestModel data)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetUBGFromBOMForFAHandler(data, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, data);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetUBGFromFABOM(int faId)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetUBGFromFABOMHandler(new UBGFromFaBOMRequestModel { FaId = faId }, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, faId);
		//	}
		//}
		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		//public IActionResult GetUBGFromFABOM(Core.CustomerService.Models.FA.UBGFromFaBOMRequestModel data)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.GetUBGFromFABOMHandler(data, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e, data);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult UpdateFA_2(Psz.Core.CustomerService.Models.FA.FAUpdateModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Update.FAUpdate2Handler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAUpdateByArticleFinalModel>), 200)]
		//public IActionResult UpdateFAByArticle_2(Psz.Core.CustomerService.Models.FA.Update.AllOpenFAForUpdateModel model)
		//{
		//	try
		//	{
		//		return Ok(new Psz.Core.Apps.Purchase.Handlers.CustomerService.Fertigung.FAUpdateByArticle2Handler(model, this.GetCurrentUser())
		//		   .Handle());

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult FaErlidigt_2(Psz.Core.CustomerService.Models.FA.FAErlidigtEntryModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.FAErlidigt2Handler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpPost]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult StornoFA_2(Psz.Core.CustomerService.Models.FA.FAStornoModel model)
		//{
		//	try
		//	{
		//		var response = new Core.CustomerService.Handlers.FA.Update.FAStorno2Handler(model, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.FA.TerminFAUpdateLogModel>>), 200)]
		//public IActionResult GetFAArticleProductionSite(int articleId)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.GetFAArticleProductionSiteHandler(this.GetCurrentUser(), articleId)
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}


		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult VerifyFABeforeDruck(int fa)
		//{
		//	try
		//	{
		//		var response = new Psz.Core.CustomerService.Handlers.FA.VerifyFABeforeDruckHandler(fa, this.GetCurrentUser())
		//		   .Handle();

		//		return Ok(response);

		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		return this.HandleException(e);
		//	}
		//}

		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		//public IActionResult TestHorizon(DateTime date)
		//{
		//	try
		//	{
		//		return Ok(new Psz.Core.CustomerService.Handlers.CheckHorizonTestHandler(this.GetCurrentUser(), date).Handle());
		//	} catch(System.Exception e)
		//	{
		//		return this.HandleException(e);
		//	}
		//}
		//#endregion
	}
}
