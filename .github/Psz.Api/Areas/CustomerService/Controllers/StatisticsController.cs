using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CustomerService.Models.FA;
using Psz.Core.CustomerService.CsStatistics.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Psz.Core.CustomerService.Interfaces;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StatisticsController: ControllerBase
	{
		private const string MODULE = "Customer Service | Statistics";

		public IInsideSalesOveview _insideSalesOveview { get; }

		public StatisticsController(IInsideSalesOveview insideSalesOveview)
		{
			_insideSalesOveview = insideSalesOveview;
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.OrderProcessingResponseModel>), 200)]
		public IActionResult GetOrderProcessingByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetOrderProcessingByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.FAResponseModel>>), 200)]
		public IActionResult GetFAByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetFAByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.OrderProcessingCustomerResponseModel>>), 200)]
		public IActionResult GetOrderProcessingTopCustomer()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetOrderProcessingCustomerHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.ProjectResponseModel>), 200)]
		public IActionResult GetAllProjectsByYearBYMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetAllProjectsByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.ABResponseModel>), 200)]
		public IActionResult GetABByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetABBYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.LSResponseModel>), 200)]
		public IActionResult GetLSByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetLSByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.OrderProcessingCustomerResponseModel>>), 200)]
		public IActionResult GetOrderProcessingTopCustomerByYearNyMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetOrderProcessingCustomerByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.OrderProcessingProjectCustomerResponseModel>>), 200)]
		public IActionResult GetProjectTopCustomerByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetProjectCostomerByYearHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.OrderProcessingABCustomerResponseModel>>), 200)]
		public IActionResult GetABTopCustomerByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetABCustomerByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.OrderProcessingLSCustomerResponseModel>>), 200)]
		public IActionResult GetLSTopCustomerByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetLSCustomerByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.ArticleFAResponseModel>>), 200)]
		public IActionResult GetArticleFAByYearByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetArticleFAByYearBYMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.CustomerFAResponseModel>>), 200)]
		public IActionResult GetTopCustomFAByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTopCustomerFAByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.CustomerFAResponseModel>>), 200)]
		public IActionResult GetTopCustomerFA()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTopCustomerFAHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.TimeFAResponseModel>>), 200)]
		public IActionResult GetTimeFAByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTimeFAByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.TimeArticleResponseModel>>), 200)]
		public IActionResult GetTimeArticleFAByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTimeArticleFAByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Statistics.TimeLagerResponseModel>>), 200)]
		public IActionResult GetTimeLagerByYearByMonth(Core.CustomerService.Models.Statistics.StatRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTimeLagerFAByYearByMonthHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.LastCreatedProectModel>), 200)]
		public IActionResult GetLastCreatedProj()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetLastCreatedProjectHandler(this.GetCurrentUser())
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
		public IActionResult GetType(bool filterByUser = false)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Statistics.GetTypeHandler(this.GetCurrentUser(), filterByUser)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.CTSLoggingModel>>), 200)]
		public IActionResult GetFALogging()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Logging.FALoggingHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.CTSLoggingResponseModel>), 200)]
		public IActionResult GetFALogging(Psz.Core.CustomerService.Models.CTSLoggingRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Logging.GetFaLogsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.CTSLoggingModel>>), 200)]
		public IActionResult GetFADetailsLogging(int fa)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Logging.FADetailsLoggingHandler(this.GetCurrentUser(), fa)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		#region CS-Statistics
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCSTStatsWarehouses()
		{
			try
			{
				var response = new Core.CustomerService.CsStatistics.Handlers.GetWarehouseHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetKapazitatLangReport(Psz.Core.CustomerService.Reporting.Models.KapazitatLangEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetKapazitatLangHandler(model, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.KapazitatKurzModel>>), 200)]
		public IActionResult GetKapzitatKurz(Psz.Core.CustomerService.CsStatistics.Models.KapzitatkurzEntryModel model)
		{
			try
			{
				var response = new Core.CustomerService.CsStatistics.Handlers.GetKapazitatCZKurzHandler(this.GetCurrentUser(), model)
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
		public IActionResult GetKapzitatKurz_XLS(Psz.Core.CustomerService.CsStatistics.Models.KapzitatkurzEntryModel model)
		{
			try
			{
				var data = new Core.CustomerService.CsStatistics.Handlers.GetKapazitatCZKurzHandler(this.GetCurrentUser(), model)
					.GetDataXLS();
				if(data != null && data.Length > 0)
				{
					return File(data, "application/xlsx", $"Kapacitat-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, model);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Reporting.Models.ExportReportModel>), 200)]
		public IActionResult GetExport(Psz.Core.CustomerService.Reporting.Models.ExportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetExportHandler(this.GetCurrentUser(), model)
				   .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};

			} catch(Exception e)
			{
				return this.HandleException(e, model);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBacklogFGPDF(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetBacklogFGPDFHandler(this.GetCurrentUser(), model)
				  .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};
			} catch(Exception e)
			{
				return this.HandleException(e, model);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetBacklogFGPDFExtended(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetBacklogFGPDFHandler(this.GetCurrentUser(), model)
				  .Handle();

				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings
				});
			} catch(Exception e)
			{
				return this.HandleException(e, model);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBacklogFGExcel(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBacklogFGExcelHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Backlog FG-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, model);
				return Ok(e.Message);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBacklogHWPDF(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetBackLogHWPDFHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetBacklogHWPDFExtended(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetBackLogHWPDFHandler(this.GetCurrentUser(), model)
				  .Handle();

				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBacklogHWExcel(Psz.Core.CustomerService.CsStatistics.Models.BacklogReportEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBacklogHWExcelHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Backlog HW-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBestandReport(Psz.Core.CustomerService.CsStatistics.Models.BestandEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetBestandReportHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Reporting.Models.BestandReportModel>), 200)]
		public IActionResult GetBestand(Psz.Core.CustomerService.CsStatistics.Models.BestandEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBestandHandler(this.GetCurrentUser(), model)
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
		public IActionResult GetBestandContacts()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBestandContactsListHandler(this.GetCurrentUser())
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
		public IActionResult GetBestandKunden(string contact)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBestandKundenHandler(this.GetCurrentUser(), contact)
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
		public IActionResult GetBestandLager()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBestandLagerHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Reporting.Models.BestandReportDetailsModel>>), 200)]
		public IActionResult GetBestandAusennLager(int lager)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetBestandAusslagerHandler(this.GetCurrentUser(), lager)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLagerBestandFG()
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetLagerBestandFGExcelHandler(this.GetCurrentUser()).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"LagerBestand FG-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFA_NPEXReport(Psz.Core.CustomerService.Reporting.Models.FA_NPEX_ReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetFA_NPEX_reprotHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.CsStatistics.Models.FA_NPEX_ResponseModel>), 200)]
		public IActionResult GetFA_NPEXArticles(Psz.Core.CustomerService.CsStatistics.Models.FA_NPEX_EntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetFA_NPEXArticlesDetailsHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Reporting.Models.FA_NPEX_ReportDetailsModel>>), 200)]
		public IActionResult GetFA_NPEXOrderDetails(Psz.Core.CustomerService.CsStatistics.Models.FA_NPEX_EntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetFA_NPEXOrderDetailsModelHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.CsStatistics.Models.VersandBerechnetResponseModel>), 200)]
		public IActionResult GetVersandBerechnet(Psz.Core.Common.Models.IDateRangeModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetVersandBerechnetHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLieferPlannungReport(string kunde)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetLierferPlannungPDFHandler(this.GetCurrentUser(), kunde)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLieferPlannungExcel(string kunde)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetLierferPlannungExcelHandler(this.GetCurrentUser(), kunde).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"LIERFERPLANNUNG-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.LieferPlannungModel>>), 200)]
		public IActionResult GetLieferPlannung(string kunde)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetLieferPlannungHandler(this.GetCurrentUser(), kunde)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.LieferPlannung_2Model>>), 200)]
		public IActionResult GetLieferPlannung_2(string kunde)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetLieferPlannung_2Handler(this.GetCurrentUser(), kunde)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.CsStatistics.Models.LieferPlannungDetailsModel>), 200)]
		public IActionResult GetLieferPlannungDetails(string article)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetLieferPlannungDetailsHandler(this.GetCurrentUser(), article)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetAuswertungRechnung(Psz.Core.Common.Models.IDateRangeModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetErstelltRehungenHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Auswertung Rechnung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetAuswertungFA(Psz.Core.CustomerService.CsStatistics.Models.AuswertungFAEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetAuswertungFAHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Auswertung FA-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetContactsFAReport(Psz.Core.Common.Models.IDateRangeModel model)
		{
			try
			{
				//var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.CSStats.GetContactFAHandler(this.GetCurrentUser(), model)
				//  .Handle();
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetContactFAHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetContactsFAReportExcel(Psz.Core.Common.Models.IDateRangeModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetContactFAExcelHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Auswertung FA-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetAuswertungSchneidereiReport(int lager)
		{
			try
			{
				//var response = new Psz.Core.Apps.Purchase.Handlers.CustomerService.CSStats.AuswertungSchneidereiHandler(this.GetCurrentUser(), lager)
				//  .Handle();
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.AuswertungSchneidereiHandler(this.GetCurrentUser(), lager)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.KundenModel>>), 200)]
		public IActionResult GetKundenList()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetKundenListHandler(this.GetCurrentUser())
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
		public IActionResult GetDepartmentEmployees()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetDepartmentEmployeesHandler(this.GetCurrentUser())
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
		public IActionResult GetKundeStufe()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetKundeStufeHandler(this.GetCurrentUser())
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
		public IActionResult AddKunde(Psz.Core.CustomerService.CsStatistics.Models.KundenModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.AddKundeHandler(this.GetCurrentUser(), model)
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
		public IActionResult UpdateKunde(Psz.Core.CustomerService.CsStatistics.Models.KundenModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.UpdateKunde(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.DepartementEmployeeModel>>), 200)]
		public IActionResult GetDepartmentEmployeesList()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetDepartmentEmployeesListHandler(this.GetCurrentUser())
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
		public IActionResult AddDepartmentEmployee(Psz.Core.CustomerService.CsStatistics.Models.DepartementEmployeeModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.AddDepartmentEmployeeHandler(this.GetCurrentUser(), model)
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
		public IActionResult UpdateDepartmentEmployee(Psz.Core.CustomerService.CsStatistics.Models.DepartementEmployeeModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.UpdateDepartmentEmployeeHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.E_RechnungModel>>), 200)]
		public IActionResult GetERechnungList()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetE_rechnungHandler(this.GetCurrentUser())
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
		public IActionResult AddERechnung(Psz.Core.CustomerService.CsStatistics.Models.E_RechnungModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.AddERechungHandler(this.GetCurrentUser(), model)
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
		public IActionResult UpdateERechnung(Psz.Core.CustomerService.CsStatistics.Models.E_RechnungModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.UpdateERechnungHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.CsStatistics.Models.KundenListSelectModel>>), 200)]
		public IActionResult GetKundenListForERechnung()
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.GetKundenListForSelectHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public async Task<IActionResult> GetRechnungPDF(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var response = await new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRechnungPDFHandler(this.GetCurrentUser(), model)
				  .HandleAsync();


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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Reporting.Models.RechnungModel>), 200)]
		public IActionResult GetRechnungData(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRechnungDataHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRechnungExcel(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetRechnungExcelHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Rechnung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRechnungROHPDF(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRechnungROHHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRechnungROHXLS(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetRechnungROHCZHandler(this.GetCurrentUser(), model)
				  .Handle();

				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Rechnung-ROH-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRechnungROHTNExcel(Psz.Core.CustomerService.Reporting.Models.RechnungEntryModel model)
		{
			try
			{
				var results = new Psz.Core.CustomerService.CsStatistics.Handlers.GetRechnungROHTNHandler(this.GetCurrentUser(), model).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Rechnung-ROH-TN-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateRechnungReportLogo([FromForm] Psz.Core.CustomerService.Reporting.Models.ReportLogoImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.UpdateRechnungReportLogoHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateRechnungReporting(Psz.Core.CustomerService.Reporting.Models.RechnungReportParametersModel model)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.PDFReports.UpdateRechnungReportingHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRechnungEndkontrollePDF(Psz.Core.CustomerService.Reporting.Models.RechnungEndkontrolleReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRechnungEndkontrollePDFHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetNachBerechnungTNPDF(Psz.Core.CustomerService.Reporting.Models.RechnungEndkontrolleReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetNachBerechnungTNReportHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRGSpritzgussPDF(Psz.Core.CustomerService.Reporting.Models.RechnungEndkontrolleReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRGSpritzgussReportHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetRGWerkzeugbauPDF(Psz.Core.CustomerService.Reporting.Models.RechnungEndkontrolleReportEntryModel model)
		{
			try
			{
				var response = new Psz.Core.CustomerService.CsStatistics.Handlers.PDFReports.GetRGWerkzeugbauReportHandler(this.GetCurrentUser(), model)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.DeliveryNotesCompilationResponseModel>), 200)]
		public IActionResult GetDeliveryNotesCompilation(Core.CustomerService.Models.OrderProcessing.DeliveryNotesCompilationRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetDeliveryNotesCompilationHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetDeliveryNotesCompilationPDF(Core.CustomerService.Models.OrderProcessing.DeliveryNotesCompilationRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.OrderProcessing.GetDeliveryNotesCompilationHandler(this.GetCurrentUser(), data)
				  .GetPDF());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetDeliveryNotesCompilationXLS(Core.CustomerService.Models.OrderProcessing.DeliveryNotesCompilationRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetDeliveryNotesCompilationHandler(this.GetCurrentUser(), data).GetXLS();
				if(response != null && response.Length > 0)
				{
					return File(response, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.CsStatistics.Models.CapacityResponseModel>>), 200)]
		public IActionResult GetCapacityStatus(Core.CustomerService.CsStatistics.Models.CapacityRequestModel horizon)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.GetCapacityStatusHandler(this.GetCurrentUser(), horizon)
				  .Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, horizon);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetCapacityStatusXLS(Core.CustomerService.CsStatistics.Models.CapacityRequestModel horizon)
		{
			try
			{
				var response = new Core.CustomerService.CsStatistics.Handlers.GetCapacityStatusHandler(this.GetCurrentUser(), horizon).GetExcelData();
				if(response != null && response.Length > 0)
				{
					return File(response, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, horizon);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<FaProductionStatusSearchResponseModel>), 200)]
		public IActionResult GetFaProductionStatus(FaProductionStatusSearchRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.GetFaProductionStatusHandler(data, this.GetCurrentUser())
				  .Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFaProductionStatusXLS(FaProductionStatusSearchRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.CsStatistics.Handlers.GetFaProductionStatusHandler(data, this.GetCurrentUser()).GetExcelData();
				if(response != null && response.Length > 0)
				{
					return File(response, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.CsStatistics.Models.CapacityAbFaResponseModel>>), 200)]
		public IActionResult GetCapacityStatus_AB(Core.CustomerService.CsStatistics.Models.CapacityAbFaRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.GetCapacityStatusABHandler(this.GetCurrentUser(), data)
				  .Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.CsStatistics.Models.CapacityAbFaResponseModel>>), 200)]
		public IActionResult GetCapacityStatus_FA(Core.CustomerService.CsStatistics.Models.CapacityAbFaRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.GetCapacityStatusFAHandler(this.GetCurrentUser(), data)
				  .Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.CsStatistics.Models.CapacityLpResponseModel>>), 200)]
		public IActionResult GetCapacityStatus_LP(Core.CustomerService.CsStatistics.Models.CapacityAbFaRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.CsStatistics.Handlers.GetCapacityStatusLPHandler(this.GetCurrentUser(), data)
				  .Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		#endregion

		#region MOVING FA between Sites
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>), 200)]
		public IActionResult MoveFaToAL([FromForm] Infrastructure.Services.Files.Models.FileAttachmentModel data)
		{
			try
			{
				var file = data.AttachedFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return BadRequest("No file sent.");
				}

				if(file.Length > 0)
				{
					using(var fileStream = file.OpenReadStream())
					{
						byte[] bytes = new byte[file.Length];
						fileStream.Read(bytes, 0, (int)file.Length);
						// - 
						var response = new Psz.Core.Apps.EDI.Handlers.MoveFasToALHandler(this.GetCurrentUser(), bytes)
							.Handle();

						if(response.Success)
						{
							return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
							, $"data-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
						}
						return Ok("Empty file sent.");
					}
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		#endregion

		#region INS Overview charts
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.ChartsAndOlderDatesModel>>), 200)]
		public IActionResult Get_INSOverviewRuckstandige_BestellungenChart(Core.CustomerService.Models.Statistics.INS.Ruckstandige_BestellungenRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewRückständige_BestellungenChart(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(List<Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.DateValueOrderModel>>), 200)]
		public IActionResult Get_INSOverviewUmsatz_Aktuelle_WocheChart()
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewUmsatz_Aktuelle_WocheChart(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.ChartsAndOlderDatesModel>), 200)]
		public IActionResult Get_INSOverviewVK_Summe_Ungebuchte_ABsChart(Core.CustomerService.Models.Statistics.INS.Ruckstandige_BestellungenRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewVK_Summe_Ungebuchte_ABsChart(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.LabelValueModel>), 200)]
		public IActionResult Get_INSOverviewMindesbestand_AuswertungChart(Core.CustomerService.Models.Statistics.INS.Mindesbestand_AuswertungRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewMindesbestand_AuswertungChart(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion

		#region INS Overview tables
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUsersForOverview()
		{
			try
			{
				var response = _insideSalesOveview.GetUsersForOverview(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetCustomersForOverview()
		{
			try
			{
				var response = _insideSalesOveview.GetCustomersForOverview(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetWarehousesForOverview()
		{
			try
			{
				var response = _insideSalesOveview.GetWarehousesForOverview(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.INSOverviewRückständige_BestellungenModelResponseModel>), 200)]
		public IActionResult Get_INSOverviewRuckstandige_Bestellungen(Core.CustomerService.Models.Statistics.INS.INSOverviewRückständige_BestellungenModelRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewRückständige_Bestellungen(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.INSOverviewRückständige_BestellungenModelResponseModel>), 200)]
		public IActionResult Get_INSOverviewVK_Summe_ungebuchte_AB(Core.CustomerService.Models.Statistics.INS.INSOverviewVK_Summe_ungebuchte_ABRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewVK_Summe_ungebuchte_AB(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CustomerService.Models.Statistics.INS.INSOverviewMindesbestand_AuswertungResponseModel>), 200)]
		public IActionResult Get_INSOverviewMindesbestand_Auswertung(Core.CustomerService.Models.Statistics.INS.INSOverviewMindesbestand_AuswertungRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewMindesbestand_Auswertung(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion

		#region INS Overview bar charts click
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.Statistics.INS.INSOverviewRückständige_BestellungenDetailsResponseModel>>), 200)]
		public IActionResult Get_INSOverviewRuckstandige_BestellungenDetails(Core.CustomerService.Models.Statistics.INS.INSOverviewRückständige_BestellungenDetailsRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewRückständige_BestellungenDetails(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.Statistics.INS.INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel>>), 200)]
		public IActionResult Get_INSOverviewUmsatz_Aktuelle_WocheDetails(Core.CustomerService.Models.Statistics.INS.INSOverviewUmsatz_Aktuelle_WocheDetailsRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewUmsatz_Aktuelle_WocheDetails(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CustomerService.Models.Statistics.INS.INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel>>), 200)]
		public IActionResult Get_INSOverviewVK_Summe_ungebuchte_ABDetails(Core.CustomerService.Models.Statistics.INS.INSOverviewVK_Summe_ungebuchte_ABDetailsRequestModel data)
		{
			try
			{
				var response = _insideSalesOveview.Get_INSOverviewVK_Summe_ungebuchte_ABDetails(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetWeeksForChartsFilter()
		{
			try
			{
				var response = _insideSalesOveview.GetWeeksForChartFilter(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticlesForChartFilter(string text)
		{
			try
			{
				var response = _insideSalesOveview.GetArticlesForChartsFilter(this.GetCurrentUser(), text);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}
