using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CrpCapacityController: ControllerBase
	{
		private const string MODULE = "Material Management | CRP";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.CapacityModel>>), 200)]
		public IActionResult Search(Core.MaterialManagement.CRP.Models.Capacity.SearchRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.SearchHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.CapacityModel>), 200)]
		public IActionResult SingleUpdate(Core.MaterialManagement.CRP.Models.Capacity.SingleCapacityUpdateModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.SingleUpdateHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportResponseModel>), 200)]
		public IActionResult GetAnalyseReport(Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.GetAnalyseReportHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportResponseModel>), 200)]
		public IActionResult GetAnalyseDepartment(Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AnalyseDepartmentHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportResponseModel>), 200)]
		public IActionResult GetAnalyseWorkArea(Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AnalyseWorkAreaHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportResponseModel>), 200)]
		public IActionResult GetAnalyseWorkStation(Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AnalyseWorkStationHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportResponseModel>), 200)]
		public IActionResult GetAnalyseOperation(Core.MaterialManagement.CRP.Models.Capacity.GetAnalyseReportRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AnalyseOperationHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportFaultyFAToExcel(int year, int weekFrom, int? weekUntil, int countryId, int? hallId)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportFaultyFAExcelHandler(year, weekFrom, weekUntil, countryId, hallId, this.GetCurrentUser())
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"FaultyFA-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportFaultyArticlesToExcel(int year, int weekFrom, int? weekUntil, int countryId, int? hallId)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportFaultyFAExcelHandler(year, weekFrom, weekUntil, countryId, hallId, this.GetCurrentUser())
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"FaultyFA-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportFaultyTimeArticleFAToExcel(int year, int weekFrom, int? weekUntil, int countryId, int? hallId)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportFaultyFAExcelHandler(year, weekFrom, weekUntil, countryId, hallId, this.GetCurrentUser())
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"FaultyFA-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.ArticlesWoWPLResponseModel>>), 200)]
		public ActionResult GetArticleswoWPL(Core.MaterialManagement.CRP.Models.Capacity.ArticlesWoWPLRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.GetArticlesWoWPLHandler(this.GetCurrentUser(), data)
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportArticleswoWPL(Core.MaterialManagement.CRP.Models.Capacity.ArticlesWoWPLRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportArticlesWoWPLHandler(this.GetCurrentUser(), data)
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"ArticleswoWPL-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.ArticleWPLTimeDiffModel>>), 200)]
		public ActionResult GetArticlesWPLTimeDiff(int? countryId, int? hallId, decimal? minDiff)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.GetArticleWPLTimeDiffHandler(this.GetCurrentUser(), countryId, hallId, minDiff)
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportArticlesWPLTimeDiff(int? countryId, int? hallId, decimal? minDiff)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportArticleWPLTimeDiffHandler(this.GetCurrentUser(), countryId, hallId, minDiff)
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"ArticleWPL-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.ArticleFaTimeDiffModel>>), 200)]
		public ActionResult GetArticlesFATimeDiff(int? lager)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.GetArticleFaTimeDiffHandler(this.GetCurrentUser(), lager)
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportArticlesFATimeDiff(int? lager)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.ExportArticleFaTimeDiffHandler(this.GetCurrentUser(), lager)
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"FA-WPL-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult AddCapacities(Core.MaterialManagement.CRP.Models.CapacityPlan.SetCapacityPlanModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.AddCapacitiesHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteCapacity(int id)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.DeleteCapacityHandler(id, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.CRP.Models.Capacity.SpecialShiftResponseModel>>), 200)]
		public ActionResult GetSpecialShifts(Core.MaterialManagement.CRP.Models.Capacity.SpecialShiftRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.CRP.Handlers.Capacity.GetSpecialShiftsHandler(this.GetCurrentUser(), data)
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
	}
}
