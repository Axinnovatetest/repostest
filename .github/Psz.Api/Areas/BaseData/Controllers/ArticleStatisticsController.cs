using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis;
using Psz.Core.BaseData.Handlers.Article.Statistics.Sales;
using Psz.Core.BaseData.Models.Article.Statistics.Logistics;
using Psz.Core.Common.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Psz.Core.BaseData.Models.Article.Statistics.Sales.ArticleMinStockDetailedAnalysisModels;

namespace Psz.Api.Areas.BaseData.Controllers
{
	[Authorize]
	[Area("BaseData")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ArticleStatisticsController: ControllerBase
	{
		private const string MODULE = "BaseData | Articles | STATS";

		public ArticleStatisticsController()
		{
		}

		#region >>>>>> Logistics <<<<<<
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.InResponseModel>>), 200)]
		public IActionResult GetLogisticsIn(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetInHandler(this.GetCurrentUser(), articleNumber)
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
		public ActionResult GetLogisticsIn_XLS(string articleNumber)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetInHandler(this.GetCurrentUser(), articleNumber)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"{articleNumber.ToUpper()}_IN-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, articleNumber);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Logistics.OutResponseModel>), 200)]
		public IActionResult GetLogisticsOut(Core.BaseData.Models.Article.Statistics.Logistics.OutRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetOutHandler(this.GetCurrentUser(), data)
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
		public ActionResult GetLogisticsOut_XLS(Core.BaseData.Models.Article.Statistics.Logistics.OutRequestModel reqdata)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetOutHandler(this.GetCurrentUser(), reqdata)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"{reqdata.ArticleNr}-OUT-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, reqdata);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.InOutDetailItem>>), 200)]
		public IActionResult GetLogisticsInOutDetails(Core.BaseData.Models.Article.Statistics.Logistics.InOutDetailsRequestModel articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetInOutDetailsHandler(this.GetCurrentUser(), articleNumber)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleNumber);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult GetLogisticsInOutDetails_XLS(Core.BaseData.Models.Article.Statistics.Logistics.InOutDetailsRequestModel articleNumber)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetInOutDetailsHandler(this.GetCurrentUser(), articleNumber)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"INOUT-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, articleNumber);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.FaStatusResponseModel>>), 200)]
		public IActionResult GetLogisticsFaStatus(FaStatusRequestModel request)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetFaStatusHandler(this.GetCurrentUser(), request)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Logistics.DeliveryListResponseModel>), 200)]
		public IActionResult GetLogisticsDeliveryList(Core.BaseData.Models.Article.Statistics.Logistics.DeliveryListRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetDeliveryListHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.TN_AL_LogisticsResponseModel>>), 200)]
		public IActionResult GetLogistics_TN_AL_Logistics(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.Get_TN_AL_LogisticsHandler(this.GetCurrentUser(), articleNumber)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.DeliveryOverviewResponseModel>>), 200)]
		public IActionResult GetLogisticsDeliveryOverview()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetDeliveryOverviewHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.PreferencesResponseModel>>), 200)]
		public IActionResult GetLogisticsPreferences(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetPreferencesHandler(this.GetCurrentUser(), articleNumber)
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
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult Download_TN_AL_Logistics(string site)
		{
			try
			{
				var data = Core.BaseData.Handlers.Article.Statistics.Logistics.Get_TN_AL_LogisticsHandler.GetLogistics(site);
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"{site.ToUpper()}_Logistics-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public ActionResult DownloadDeliveryOverview()
		{
			try
			{
				var data = Core.BaseData.Handlers.Article.Statistics.Logistics.GetDeliveryOverviewHandler.SaveToExcelFile();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"DeliveryOverview-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Logistics.TranspotResponseModel>>), 200)]
		public IActionResult GetLogisticsTransPot()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetTransPotHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLogisticsTransPot_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Logistics.GetTransPotHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		#endregion >>>>>> Logistics <<<<<<

		#region >>>>>> Controlling <<<<<<
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Controlling.DBResponseModel>>), 200)]
		public IActionResult GetControllingDB(int articleNr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Controlling.GetDBHandler(this.GetCurrentUser(), articleNr)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleNr);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetControllingDB_XLS(int articleNr)
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Controlling.GetDBHandler(this.GetCurrentUser(), articleNr)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"DB-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, articleNr);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Controlling.HistoryModel>>), 200)]
		public IActionResult GetControllingHistory(int articleNr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Controlling.GetHistoryHandler(this.GetCurrentUser(), articleNr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Controlling.LangtextResponseModel>), 200)]
		public IActionResult GetControllingLangtext(int articleNr)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Controlling.GetLangtextHandler(this.GetCurrentUser(), articleNr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Controlling.BomReportModel>>), 200)]
		public IActionResult GetControllingBomReport(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Controlling.GetBomReportHandler(this.GetCurrentUser(), articleNumber)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion >>>>>> Controlling <<<<<<

		#region >>>>>> Controlling Analysis <<<<<<
		[AllowAnonymous]
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadControllingAnalysis_VKIncludingCopper(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKIncludingCopperRequestModel inputData)
		{
			try
			{
				var data = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKIncludingCopperHandler.GetData(inputData);
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AverageMaterialContent_GetCustomers()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetAverageMaterialContentGetCustomerHandler(this.GetCurrentUser())
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
		public ActionResult DownloadControllingAnalysis_AverageMaterialContent(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.AverageMaterialContentRequestModel data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetAverageMaterialContentHandler.GetData(data);
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>), 200)]
		public IActionResult GetProjectMessage(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetProjectMessageHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>), 200)]
		public IActionResult AddProjectMessage(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.AddProjectMessageHandler(this.GetCurrentUser(), data)
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
		public IActionResult AddProjectMessageMulti([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemRequestModel> data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.AddProjectMessageMultiHandler(this.GetCurrentUser(), data).Handle();
				return Ok(Psz.Core.Common.Models.ResponseModel<bool>.SuccessResponse(results.Success));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(byte[]), 200)]
		public IActionResult AddnPrintProjectMessageMulti([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemRequestModel> data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.AddProjectMessageMultiHandler(this.GetCurrentUser(), data).Handle();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/pdf", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.pdf");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult EditProjectMessage(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.EditProjectMessageHandler(this.GetCurrentUser(), data)
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
		public IActionResult DeleteProjectMessage(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.DeleteProjectMessageHandler(this.GetCurrentUser(), id)
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
		public ActionResult DownloadControllingAnalysis_ProjectMessage([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel> data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetProjectMessagePDFDataHandler.GetData(data, null);
				if(results != null && results.Length > 0)
				{
					return File(results, "application/pdf", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.pdf");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetActiveStucklistedArticleNumbers(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ActiveStucklistedANumberRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetActiveStucklistedArticleNumbersHandler(this.GetCurrentUser(), data)
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
		public IActionResult GetStandortMuster()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(1, "PSZ CZ"),
					new KeyValuePair<int, string>(2, "PSZ TN"),
					new KeyValuePair<int, string>(3, "PSZ AL"),
					new KeyValuePair<int, string>(4, "SC CZ"),
				});

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
		public IActionResult GetStandortSerie()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(1, "PSZ CZ"),
					new KeyValuePair<int, string>(2, "PSZ TN"),
					new KeyValuePair<int, string>(3, "PSZ AL"),
					new KeyValuePair<int, string>(4, "SC CZ"),
				});

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
		public IActionResult GetContactCsPsz()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(1,"Dominic Häffner"),
					new KeyValuePair<int, string>(2,"Michael Herrmann"),
					new KeyValuePair<int, string>(3,"Irina Neuber"),
					new KeyValuePair<int, string>(4,"Veronika Ertl"),
					new KeyValuePair<int, string>(5,"Maria Petzi"),
					new KeyValuePair<int, string>(6,"Sabine Schremmer"),
					new KeyValuePair<int, string>(7,"Christian Summer"),
				});

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
		public IActionResult GetContactAvPsz()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(1,"Thomas Zipproth"),
					new KeyValuePair<int, string>(2,"Julian Berger"),
					new KeyValuePair<int, string>(3,"Patrick Luff"),
					new KeyValuePair<int, string>(4,"Stefan Zeus"),
					new KeyValuePair<int, string>(5,"Markus Sax"),
					new KeyValuePair<int, string>(6,"Julia Frischholz"),
				});

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
		public IActionResult GetContactTechnicPsz()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(1,"Martin Vater"),
					new KeyValuePair<int, string>(2,"Habib Mokchah"),
					new KeyValuePair<int, string>(3,"Estela Kuteli"),
					new KeyValuePair<int, string>(4,"Leonardo Ceraku"),
					new KeyValuePair<int, string>(5,"David Steffel"),
					new KeyValuePair<int, string>(6,"Sabeur Ghezal"),
				});

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
		public IActionResult GetCosts()
		{
			try
			{
				var response = Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(new List<KeyValuePair<int, string>> {
					new KeyValuePair<int, string>(25,"25 €"),
					new KeyValuePair<int, string>(50,"50 €"),
					new KeyValuePair<int, string>(75,"75 €"),
					new KeyValuePair<int, string>(100,"100 €"),
				});

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadControllingAnalysis_MaterialbestandSpezifischLautNummernkreis_Purchase(List<string> data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetMaterialbestandSpezifischLautNummernkreisHandler.GetDataPurchase(data);
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public ActionResult DownloadControllingAnalysis_MaterialbestandSpezifischLautNummernkreis(string data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetMaterialbestandSpezifischLautNummernkreisHandler.GetDataEngineering(data);
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public ActionResult DownloadControllingAnalysis_PSZPrioeinkaufAlleNiederlassungen1()
		{
			try
			{
				//var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf1PDFHandler.GetData();
				//if (results != null && results.Length > 0)
				//{
				//    return File(results, "application/pdf", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.pdf");
				//}
				//else
				//{
				//    return Ok("Empty file sent.");
				//}

				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf1PDFHandler.GetData();
				if(results != null && results.Length > 0)
				{
					string tempFolder = "StaticFiles";
					string dirPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), tempFolder);
					if(!System.IO.Directory.Exists(dirPath))
						System.IO.Directory.CreateDirectory(dirPath);
					string filename = $"{DateTime.Now.ToString("yyyyMMdd")}-PSZPrioeinkauf1-{DateTime.Now.ToString("yyyyMMddTHHmmssfff")}.pdf";
					System.IO.File.WriteAllBytes(System.IO.Path.Combine(dirPath, filename), results);

					return Ok(Psz.Core.Common.Models.ResponseModel<string>.SuccessResponse($"{tempFolder}/{filename}"));
				}
				else
				{
					return Ok(Psz.Core.Common.Models.ResponseModel<string>.FailureResponse("Empty file sent."));
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult DownloadControllingAnalysis_PSZPrioeinkaufAlleNiederlassungen2()
		{
			try
			{
				//var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf2PDFHandler.GetData();
				//if (results != null && results.Length > 0)
				//{
				//    return File(results, "application/pdf", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.pdf");
				//}
				//else
				//{
				//    return Ok("Empty file sent.");
				//}

				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf2PDFHandler.GetData();
				if(results != null && results.Length > 0)
				{
					string tempFolder = "StaticFiles";
					string dirPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), tempFolder);
					if(!System.IO.Directory.Exists(dirPath))
						System.IO.Directory.CreateDirectory(dirPath);
					string filename = $"{DateTime.Now.ToString("yyyyMMdd")}-PSZPrioeinkauf2-{DateTime.Now.ToString("yyyyMMddTHHmmssfff")}.pdf";
					System.IO.File.WriteAllBytes(System.IO.Path.Combine(dirPath, filename), results);

					return Ok(Psz.Core.Common.Models.ResponseModel<string>.SuccessResponse($"{tempFolder}/{filename}"));
				}
				else
				{
					return Ok(Psz.Core.Common.Models.ResponseModel<string>.FailureResponse("Empty file sent."));
				}
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>), 200)]
		public ActionResult GetControllingAnalysis_PSZPrioeinkaufAlleNiederlassungen1(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf1PDFHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufResponseModel>), 200)]
		public ActionResult GetControllingAnalysis_PSZPrioeinkaufAlleNiederlassungen2(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf2PDFHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_PSZPrioeinkaufAlleNiederlassungen1_XLS(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf1PDFHandler.GetData(data);
				if(results != null)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_PSZPrioeinkaufAlleNiederlassungen2_XLS(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.PrioEinkaufRequestModel data)
		{
			try
			{
				var results = Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetPszPrioeinkauf2PDFHandler.GetData(data);
				if(results != null)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>), 200)]
		public IActionResult GetControllingAnalysis_HighRunner(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.HighRunnerRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetHighRunnerHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateControllingAnalysis_FixDelNote(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.FixDelNoteRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.UpdateFixDelNoteHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateControllingAnalysis_SalesPriceWCopperOrders(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.SalesPriceCopperOrdersRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.UpdateSalesPriceWCopperOrdersHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateControllingAnalysis_SalesPriceWoCopperOrders(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.SalesPriceCopperOrdersRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.UpdateSalesPriceWoCopperOrdersHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		// - 

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>), 200)]
		public IActionResult ControllingAnalysis_SuperbillFA_darft()
		{
			try
			{
				var result = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillDraftHandler(this.GetCurrentUser()).Handle();
				if(result != null && result.Success)
					return File(result.Body, "application/xlsx", $"Template-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				else
					return Ok(result);

			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>), 200)]
		public IActionResult ControllingAnalysis_SuperbillFA_check([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillFaHandler(this.GetCurrentUser(), data?.ToBusinessModel()).Check());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>), 200)]
		public IActionResult ControllingAnalysis_SuperbillFA_sum([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillFaHandler(this.GetCurrentUser(), data?.ToBusinessModel()).GetSumExcelData();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"SumData-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok(results);
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>), 200)]
		public IActionResult ControllingAnalysis_SuperbillFA_details([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillFaHandler(this.GetCurrentUser(), data?.ToBusinessModel()).GetDetailExcelData();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"detailData-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok(results);
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		//  - 203-05-22 

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_SuperbillROH>), 200)]
		public IActionResult ControllingAnalysis_SuperbillArt_darft()
		{
			try
			{
				var result = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillArticleDraftHandler(this.GetCurrentUser()).Handle();
				if(result != null && result.Success)
					return File(result.Body, "application/xlsx", $"Template-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				else
					return Ok(result);

			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult ControllingAnalysis_SuperbillArt_check([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillArtHandler(this.GetCurrentUser(), data?.ToBusinessModel()).Check());
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_SuperbillArt_sum([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillArtHandler(this.GetCurrentUser(), data?.ToBusinessModel()).GetSumExcelData();
			if(results.Success && results.Body != null && results.Body.Length > 0)
			{
				return File(results.Body, "application/xlsx", $"SumData-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
			}
			else
			{
				return Ok(results);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_SuperbillArt_details([FromForm] Psz.Core.Common.Models.ImportFileRequestModel data)
		{
			var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.ImportSuperbillArtHandler(this.GetCurrentUser(), data?.ToBusinessModel()).GetDetailExcelData();
			if(results.Success && results.Body != null && results.Body.Length > 0)
			{
				return File(results.Body, "application/xlsx", $"detailData-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
			}
			else
			{
				return Ok(results);
			}
		}
		// -
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ControllingAnalysis_DownloadDBzuArtikel(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.GetDBzuArtikelRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetDBzuArtikelHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadBestellungenProDisponent(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.GetBestellungenProDisponentRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetBestellungenProDisponentHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadRahmenliste()
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetArtikellisteRahmenHandler(this.GetCurrentUser()).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadProjectMeldungAnalyse()
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetProjectMeldungAnalyseHandler(this.GetCurrentUser()).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadInventurROH(int minStock)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetInventurROHHandler(this.GetCurrentUser(), minStock).Handle();
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
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>>), 200)]
		public IActionResult ControllingAnalysis_GetVKSimulationIn(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKSimulationInRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKSimulationInHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadVKSimulationIn([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKSimulationInXLSHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>), 200)]
		public IActionResult ControllingAnalysis_UpdateVKOnly([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.UpdateVKOnlyHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.UpdateVKOnlyResponseModel>>), 200)]
		public IActionResult ControllingAnalysis_UpdateVKandAB([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.UpdateVKandABHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryResponseModel>), 200)]
		public IActionResult ControllingAnalysis_GetVKUpdateHistory(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKUpdateHistoryHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateHistoryResponseModel>), 200)]
		public IActionResult ControllingAnalysis_GetMinStockUpdateHistory(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateHistoryRequestModel data)
		{
			try
			{
				return Ok(new GetMinStockUpdateHistoryHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ArticleMinStockDetailedAnalysisResponseModel>), 200)]

		public IActionResult MinStockDetailedAnalysis(ArticleMinStockDetailedAnalysisRequestModel data)
		{
			try
			{
				return Ok(new GetMinStockDetailedAnalysisHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<byte[]>), 200)]
		public IActionResult MinStockDetailedAnalysis_XLS(ArticleMinStockDetailedAnalysisRequestModel data)
		{
			try
			{

				var results = new GetMinStockDetailedAnalysisHandler().GetDataXLS(this.GetCurrentUser(), data);
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
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]

		public IActionResult ControllingAnalysis_GetVKUpdateTemplateFile()
		{
			try
			{
				var file = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKUpdateTemplateFileHandler().Handle()?.Body;
				switch(file?.FileExtension)
				{
					case ".png":
						return new FileContentResult(file.FileData, "application/png")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMDDHHmmss")}{file.FileExtension}"
						};
					case ".jpg":
					case ".jpeg":
						return new FileContentResult(file.FileData, "application/jpeg")
						{
							FileDownloadName = $"img-{DateTime.Now.ToString("yyyyMMDDHHmmss")}{file.FileExtension}"
						};
					default:
						return new FileContentResult(file.FileData, "application/blob")
						{
							FileDownloadName = $"{file.FileName}"
						};
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]

		public IActionResult ControllingAnalysis_GETMinStockFGTemplatFile()
		{

			try
			{
				var data = new GetMinStockFGTemplateFileHandler().Handle()?.Body;
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>), 200)]
		public IActionResult ControllingAnalysis_VKUpdate([FromForm] Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateRequestModel data)
		{
			try
			{
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
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmssfff_") + file.FileName);

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
						new Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.VKUpdateHandler(user,
						new Core.Common.Models.ImportFileModel
						{
							Id = -1,
							FilePath = filePath,
							Overwrite = false
						}).Handle());
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is < 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateResponseModel>), 200)]
		public IActionResult ControllingAnalysis_MinStockUpdate([FromForm] Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateRequestModel data)
		{
			try
			{
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
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmssfff_") + file.FileName);

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
				new UpdateMinStockHandler(user,
				new ImportFileModel
				{
					Id = -1,
					FilePath = filePath,
					Overwrite = false
				}).Handle());

				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is < 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_VKUpdate_XLS([FromBody] List<Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel> data)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}
				var results = VKUpdateHandler.GetDataXLS(data);
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_VKUpdateHistory_XLS(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.VKUpdateHistoryRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetVKUpdateHistoryHandler(this.GetCurrentUser(), data)
					.GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]

		public IActionResult ControllingAnalysis_MinStockUpdateHistory_XLS(Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateHistoryRequestModel data)
		{
			try
			{
				var result = new GetMinStockUpdateHistoryHandler(this.GetCurrentUser(), data).GetDataXLS();
				if(result != null && result.Length > 0)
				{
					return File(result, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion >>>>>> Controlling Analysis <<<<<<

		#region Technic
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Technic_DownloadPlanningOrders(Core.BaseData.Models.Article.Statistics.Technic.PlanningOrderRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.Technic.GetPlanningOrdersHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.TechnicianResponseModel>>), 200)]
		public IActionResult GetTechnicTechnicians()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetTechniciansHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.TechnicianResponseModel>>), 200)]
		public IActionResult GetTechnicTechniciansForAdd(string filter)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetTechniciansForAddHandler(this.GetCurrentUser(), filter)
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
		public IActionResult AddTechnicTechnician(Core.BaseData.Models.Article.Statistics.Technic.TechnicianModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.AddTechnicianHandler(this.GetCurrentUser(), data)
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
		public IActionResult EditTechnicTechnician(Core.BaseData.Models.Article.Statistics.Technic.TechnicianModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.EditTechnicianHandler(this.GetCurrentUser(), data)
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
		public IActionResult DeleteTechnicTechnician(int id)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.DeleteTechnicianHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.ProjectDataResponseModel>>), 200)]
		public IActionResult GetTechnicProjectData(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetProjectDataHandler(this.GetCurrentUser(), articleNumber)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>), 200)]
		public IActionResult GetTechnicQuickAreaBestand_CZ()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetQuickAreaBestand_CZHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>), 200)]
		public IActionResult GetTechnicQuickAreaBestand_TN()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetQuickAreaBestand_TNHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Technic.ProdTNResponseModel>>), 200)]
		public IActionResult GetTechnicProdTN()
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Technic.GetProdTNHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetTechnicProdTN_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Technic.GetProdTNHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		#endregion Technic

		#region CS
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_DownloadPlanningTechnicOrder(Core.BaseData.Models.Article.Statistics.CustomerService.PlanningTechnicOrderRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetPlanningTechnicOrderHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_DownloadStatusPArticle(Core.BaseData.Models.Article.Statistics.CustomerService.StatusPArticleRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetStatusPArticleHandler(this.GetCurrentUser(), data).Handle();
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
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_DownloadRepairEvaluation(string data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetRepairEvaluationHandler(this.GetCurrentUser(), data).Handle();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.CustomerService.AVEvaluationResponseModel>>), 200)]
		public IActionResult CS_GetAVEvaluation(Core.BaseData.Models.Article.Statistics.CustomerService.AVEvaluationRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetAVEvaluationHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult CS_GetAVEvaluation_XLS(Core.BaseData.Models.Article.Statistics.CustomerService.AVEvaluationRequestModel data)
		{
			try
			{
				var xls = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetAVEvaluationHandler(this.GetCurrentUser(), data).GetData();
				if(xls != null && xls.Length > 0)
				{
					return File(xls, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>), 200)]
		public IActionResult GetHUBGStockStatus(int? lagerId)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetHUBGStockStatusHandler(this.GetCurrentUser(), lagerId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, lagerId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.CustomerService.StockStatusItemResponseModel>), 200)]
		public IActionResult GetHUBGStockStatusItem(Core.BaseData.Models.Article.Statistics.CustomerService.StockStatusItemRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.CustomerService.GetHUBGStockStatusItemHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		#endregion // CS

		#region Basics
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.MinimumStockResponseModel>>), 200)]
		public IActionResult BS_GetMinimumStock()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetMinimumStockHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult BS_GetMinimumStock_XLS()
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.Basics.GetMinimumStockHandler(this.GetCurrentUser())
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"MinimumStock-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult BS_GetCartonsCirculationSites()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetCartonsCirculationSitesHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.CirculationResponseModel>>), 200)]
		public IActionResult BS_GetCirculations(Core.BaseData.Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetCirculationsHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult BS_GetCartonsPDF(Core.BaseData.Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			try
			{
				return Ok(
					new Core.BaseData.Handlers.Article.Statistics.Basics.GetCartonsPDFHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult BS_GetCirculationsPDF(Core.BaseData.Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			try
			{
				return Ok(
					new Core.BaseData.Handlers.Article.Statistics.Basics.GetCirculationsPDFHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.CartonResponseModel>>), 200)]
		public IActionResult BS_GetCartons(Core.BaseData.Models.Article.Statistics.Basics.CartonsCirculationRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetCartonsHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>), 200)]
		public IActionResult BS_GetMaterialStockProd(Core.BaseData.Models.Article.Statistics.Basics.MaterialStockProdRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetMaterialStockProdHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>), 200)]
		public IActionResult BS_GetMinimumStockAnalyse(int lager)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetMinimumStockAnalyseHandler(this.GetCurrentUser(), lager)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>), 200)]
		public IActionResult BS_GetOpenFaEsd(int lagerId)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetOpenFaEsdHandler(this.GetCurrentUser(), lagerId)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel>), 200)]
		public IActionResult BS_GetArticleProjectType(Core.BaseData.Models.Article.Statistics.Basics.ArticleProjectTypeRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetArticleProjectTypeHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ProofOfUsageResponseModel>), 200)]
		public IActionResult BS_GetProofOfUsage(Core.BaseData.Models.Article.Statistics.Basics.ProofOfUsageRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetProofOfUsageHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ProofOfUsageResponseModel>), 200)]
		public IActionResult BS_GetProofOfUsage_XLS(Core.BaseData.Models.Article.Statistics.Basics.ProofOfUsageRequestModel data)
		{
			var results = new Core.BaseData.Handlers.Article.Statistics.Basics.GetProofOfUsageHandler(this.GetCurrentUser(), data)
					.GetData();
			if(results != null && results.Length > 0)
			{
				return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
			}
			else
			{
				return Ok("Empty file sent.");
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.BedarfResponseModel>), 200)]
		public IActionResult BS_GetBedarf(Core.BaseData.Models.Article.Statistics.Basics.BedarfRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetBedarfHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult BS_GetBedarf_XLS(Core.BaseData.Models.Article.Statistics.Basics.BedarfRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.Basics.GetBedarfHandler(this.GetCurrentUser(), data)
					   .GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Bedarf-{System.DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.BinshResponseModel>), 200)]
		public IActionResult BS_GetBinsh(Core.BaseData.Models.Article.Statistics.Basics.BinshRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Basics.GetBinshHandler(this.GetCurrentUser(), data)
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
		public IActionResult BS_GetBinsh_XLS(Core.BaseData.Models.Article.Statistics.Basics.BinshRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.Basics.GetBinshHandler(this.GetCurrentUser(), data)
					.GetDataXLS();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Binsh-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		#endregion // BS

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.BomTzResponseModel>), 200)]
		public IActionResult BS_GetBomTz(string data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Basics.GetBomTzHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.BomTzResponseModel>), 200)]
		public IActionResult BS_GetBomTz_XLS(string data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.Basics.GetBomTzHandler(this.GetCurrentUser(), data)
					.GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Fibu-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		// - 2022-03-07
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ProductivityResponseModel>), 200)]
		public IActionResult BS_GetProductivity(Core.BaseData.Models.Article.Statistics.Basics.ProductivityRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Basics.GetProductivityHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>), 200)]
		public IActionResult BS_GetProductivityDetails(Core.BaseData.Models.Article.Statistics.Basics.ProductivityRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Basics.GetProductivityDetailsHandelr(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ToolsResponseModel>), 200)]
		public IActionResult BS_GetArticleTools(string articleNumber)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.Basics.GetToolsHandler(this.GetCurrentUser(), articleNumber)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, articleNumber);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.HighRunnerArticleResponseModel>), 200)]
		public async Task<IActionResult> CA_GetHighRunner(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.HighRunnerArticleRequestModel data)
		{
			try
			{
				var response = await new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetHighRunnerArticleHandler(this.GetCurrentUser(), data)
					.HandleAsync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> CA_GetHighRunner_XLS(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.HighRunnerArticleRequestModel data)
		{
			try
			{
				var results = await new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetHighRunnerArticleHandler(this.GetCurrentUser(), data)
					.SaveToExcelFile();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Highrunner-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.SupplierHitCountResponseModel>), 200)]
		public async Task<IActionResult> CA_GetSupplierHitCount(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.SupplierHitCountRequestModel data)
		{
			try
			{
				var response = await new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetSupplierHitCountHandler(this.GetCurrentUser(), data)
					.HandleAsync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> CA_GetSupplierHitCount_XLS(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.SupplierHitCountRequestModel data)
		{
			try
			{
				var results = await new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetSupplierHitCountHandler(this.GetCurrentUser(), data)
					.SaveToExcelFile();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Highrunner-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.AnalyseFibuResponseModel>>), 200)]
		public IActionResult GetAnalyseFibu(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.AnalyseFibuRequestModel data)
		{
			try
			{
				var response = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetAnalyseFibuHandler(this.GetCurrentUser(), data)
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
		public ActionResult GetAnalyseFibu_XLS(Core.BaseData.Models.Article.Statistics.ControllingAnalysis.AnalyseFibuRequestModel data)
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetAnalyseFibuHandler(this.GetCurrentUser(), data)
					.GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Fibu-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ArticleLogisticsResponseModel>), 200)]
		public async Task<IActionResult> BS_GetArticleLogistics(Core.BaseData.Models.Article.Statistics.Basics.ArticleLogisticsRequestModel data)
		{
			try
			{
				var response = await new Core.BaseData.Handlers.Article.Statistics.Basics.GetArticleLogisticsHandler(this.GetCurrentUser(), data)
					.HandleAsync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> BS_GetArticleLogistics_XLS(Core.BaseData.Models.Article.Statistics.Basics.ArticleLogisticsRequestModel data)
		{
			try
			{
				var results = await new Core.BaseData.Handlers.Article.Statistics.Basics.GetArticleLogisticsHandler(this.GetCurrentUser(), data)
					.GetDataXLS();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"ArticleLogistics-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}


		// - 2022-03-29 - test front PDF print
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(string), 200)]
		public IActionResult getPDF(int data)
		{
			try
			{
				return Ok(
					Core.Common.Models.ResponseModel<string>.SuccessResponse(""));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(int), 200)]
		public IActionResult UpdateDesignationXLS([FromForm] Models.Articles.Bom.PositionImportXLSModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article UpdateDesignationXLS");
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
						new Core.BaseData.Handlers.Article.Statistics.Basics.UpdateDesignationXLSHandler(user, filePath)
						.Handle());
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

		// - 2023-04-20
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetALArticles_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Basics.GetALArticlesHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetALBoms_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Basics.GetALBomsHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Basics.ToolsResponseModel>), 200)]
		public IActionResult GetHbgUbg()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Basics.GetHbgUbgHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetHbgUbg_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Basics.GetHbgUbgHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetOpenSalesRa_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Basics.GetOpenSalesRaHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetObsoleteArticles_XLS()
		{
			try
			{
				var data = new Core.BaseData.Handlers.Article.Statistics.Basics.GetOpenSalesRaHandler(this.GetCurrentUser()).GetData();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ControllingAnalysis_QuickPo(int id)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.QuickPoHandler(this.GetCurrentUser(), id)
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>), 200)]
		public IActionResult GetArticleROHNeedStock(Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeedStockHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetArticleROHNeedStock_Stufe()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeed_StufeHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>), 200)]
		public IActionResult GetArticleROHNeedStock_SupplierStufe()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeed_SupplierStufeHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>), 200)]
		public IActionResult GetArticleROHNeedStock_Sync()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeed_SyncHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockResponseModel>), 200)]
		public IActionResult GetArticleROHNeedStockSync(Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeedStockSyncHandler(this.GetCurrentUser(), data)
					.Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult GetArticleROHNeedStockSync_XLS(Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
		{
			try
			{
				var _data = new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeedStockSyncHandler(this.GetCurrentUser(), data)
					.GetXLS();
				if(_data != null && _data.Length > 0)
				{
					return File(_data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult GetArticleROHNeedStockSyncExternal_XLS(Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockRequestModel data)
		{
			try
			{
				var _data = new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeedStockSyncHandler(this.GetCurrentUser(), data)
					.GetXLS(true);
				if(_data != null && _data.Length > 0)
				{
					return File(_data, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.BaseData.Models.Article.Statistics.Sales.ArticleROHNeedStockSyncParamsResponseModel>), 200)]
		public IActionResult GetArticleROHNeedStock_SyncParams()
		{
			try
			{
				return Ok(new Core.BaseData.Handlers.Article.Statistics.Sales.GetArticleROHNeedStockSyncParamsHandler(this.GetCurrentUser())
					.Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//ArtikelPricesHistoryHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<IPaginatedResponseModel<Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesChangesHistoryModel>>), 200)]
		public IActionResult GetArtikelChangesHistory(Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesChangesHistoryRequestModel data)
		{
			try
			{
				var response = new Psz.Core.BaseData.Handlers.Article.ArticlesPricesHistory.GetArtikelsPricesChangesHistoryHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesChangesHistoryModel>>), 200)]
		public IActionResult ArtikelPricesHistoryChart(Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesHistoryRequestModel data)
		{
			try
			{
				var response = new Psz.Core.BaseData.Handlers.Article.ArticlesPricesHistory.ArtikelPricesHistoryChartHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<IPaginatedResponseModel<Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesChangesHistoryModel>>), 200)]
		public IActionResult GetArtikelPriceHistory(Psz.Core.BaseData.Models.Article.ArticlesPricesHistory.ArtikelsPricesHistoryRequestModel data)
		{
			try
			{
				var response = new Psz.Core.BaseData.Handlers.Article.ArticlesPricesHistory.ArtikelPricesHistoryHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(List<Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateResponseModel>), 200)]
		public IActionResult ControllingAnalysis_MinStockUpdateRoh([FromForm] Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis.MinStockUpdateRequestModel data)
		{
			try
			{
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
					var filePath = System.IO.Path.Combine(tempFilePath, DateTime.Now.ToString("yyyyMMddTHHmmssfff_") + file.FileName);

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
				new UpdateMinStockRohHandler(user,
				new ImportFileModel
				{
					Id = -1,
					FilePath = filePath,
					Overwrite = false
				}).Handle());

				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is < 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ControllingAnalysis_DownloadRohArticlesSuppliers()
		{
			try
			{
				var results = new Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis.GetRohArticlesSuppliersHanlder(this.GetCurrentUser()).Handle();
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
				return this.HandleException(e);
			}
		}
	}
}
