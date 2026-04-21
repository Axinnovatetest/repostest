using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan.Helpers;
using Psz.Core.Apps.WorkPlan.Models.WorkPlan;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Psz.Api.Areas.MaterialManagement.Controllers.WorkPlan
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class WorkScheduleController: ControllerBase
	{
		private const string MODULE = "Material Management | Work Plan";

		[HttpGet("{HallId}/{ArticleId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByHallArticle(int HallId, int ArticleId)
		{
			var errors = new List<string>();

			try
			{
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIdArticleId(HallId, ArticleId);
				if(workScheduleDb == null)
				{
					errors.Add("Work schedule not found");
				}
				else
				{
					workScheduleDb.ForEach(wsdb => new List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>() { new Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel(wsdb) });
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>>() { Success = true, ResponseBody = new List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>(), Errors = errors } });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get()
		{
			var errors = new List<string>();

			try
			{
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get();
				if(workScheduleDb == null)
				{
					errors.Add("Work Schdule not found.");
				}
				else
				{
					workScheduleDb.ForEach(wsdb => new List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>() { new Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel(wsdb) });
				}

				if(errors.Count == 0)
				{
					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>>() { Success = true, ResponseBody = new List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>(), Errors = errors } });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Get(int Id)
		{
			var errors = new List<string>();

			try
			{

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(Id);
				if(workScheduleDb == null)
				{
					errors.Add("Work Schdule not found.");
				}
				else
				{
					var workScheduleVM = new Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel(workScheduleDb);

					return Ok(new { response = new Models.Response<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>() { Success = true, ResponseBody = workScheduleVM, Errors = errors } });
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Add(Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel data)
		{
			var errors = new List<string>();
			try
			{
				var name = 1;
				var addedId = 0;

				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok(new
					{
						response = new Models.Response<string>
						{
							Success = false,
							Errors = new List<string>() { "Authentification: User not found" }
						}
					});
				}

				var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(data.HallId);
				var articleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetArtikel(data.ArticleId);

				var workScheduleListDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIdArticleId(data.HallId, data.ArticleId);
				if(workScheduleListDb.Count != 0)
				{
					name += workScheduleListDb.Max(ws => int.TryParse(ws.Name.Substring(2, ws.Name.Length - 2), out var _max) ? _max : workScheduleListDb.Count);
				}

				//Article.EditArticle(user, data.ArticleId);

				if(data.IsActive)
				{
					if(workScheduleListDb.Count != 0)
					{
						workScheduleListDb.ForEach(wsdb => wsdb.IsActive = false);
						Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleListDb);
					}
				}

				var workScheduleDb = new Infrastructure.Data.Entities.Tables.WPL.WorkPlanEntity()
				{
					Name = "AP" + data.Name,
					ArticleId = data.ArticleId,
					CreationTime = DateTime.Now,
					CreationUserId = user.Id,
					HallId = data.HallId,
					IsActive = data.IsActive,
				};

				addedId = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Insert(workScheduleDb);

				Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
					$"{user.Username} Added a Work Schedule  AP {name} for {hallDb?.Name} and {articleDb?.ArtikelNummer}",
					user.Id);

				return Ok(new
				{
					response = new Models.Response<string>()
					{
						Success = true,
						ResponseBody = addedId.ToString()
					}
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Edit(Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel model)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can not find User.Authentification error");
				}

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(model.Id);
				if(workScheduleDb == null)
				{
					errors.Add("Work Schedule not found.");
				}
				else
				{
					if(model.IsActive == true)
					{
						var workScheduleListDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIdArticleId(model.HallId, model.ArticleId);
						if(workScheduleListDb.Count != 0)
						{
							workScheduleListDb.ForEach(ws => ws.IsActive = false);

							Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleListDb);
						}

						if(errors.Count == 0)
						{
							Article.EditArticle(user, model.ArticleId);

							workScheduleDb.IsActive = true;
							Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleDb);
						}
					}
				}

				if(errors.Count == 0)
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(model.HallId);
					var articleDb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(model.ArticleId);

					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
						$"{user.Username} Edited a Work Schedule {workScheduleDb?.Name} for {hallDb?.Name} and {articleDb?.Name}.",
						user.Id);

					return Ok(new { response = new Models.Response<string>(true, $"Edit Success.Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{WorkScheduleId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult ActiveState(int WorkScheduleId)
		{
			var errors = new List<string>();

			try
			{
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(WorkScheduleId);
				if(workScheduleDb == null)
				{
					errors.Add("Work Schdule not found.");
				}
				else
				{

					return Ok(new { response = new Models.Response<bool>() { Success = true, ResponseBody = workScheduleDb.IsActive, Errors = errors } });
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{WorkScheduleId}/{IsActive}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult SetActive(int WorkScheduleId, int IsActive)
		{

			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can not find User.Authentification error");
				}

				var type = "";

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(WorkScheduleId);
				if(workScheduleDb == null)
				{
					errors.Add("Work Schedule not found.");
				}
				else
				{
					var workScheduleListDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIdArticleId(workScheduleDb.HallId, workScheduleDb.ArticleId);

					if(IsActive == 1)
					{
						type = "activated";

						if(workScheduleListDb.Count != 0)
						{
							workScheduleListDb.ForEach(ws => ws.IsActive = false);

							var editDone = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleListDb);
							if(editDone == -1)
							{
								errors.Add("Can't edit old WorkSchedule(Is Active). DB error");
							}
						}

						if(errors.Count == 0)
						{
							Article.EditArticle(user, workScheduleDb.ArticleId);
							workScheduleDb.IsActive = true;
							Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleDb);
						}
					}
					else
					{
						type = "disactivated";
						workScheduleDb.IsActive = false;
						Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Update(workScheduleDb);
					}
				}

				if(errors.Count == 0)
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(workScheduleDb.HallId);
					var articledb = Infrastructure.Data.Access.Tables.WPL.ArticleAccess.Get(workScheduleDb.ArticleId);
					var state = workScheduleDb.IsActive ? "active" : "inactiv";

					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
						$"{user.Username} {type} has set the Work Schedule{workScheduleDb.Name} for {hallDb.Name}/{articledb.Name} to {state}.",
						user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Edit Success.Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult Delete(int Id)
		{
			var errors = new List<string>();
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					errors.Add("Can not find User.Authentification error");
				}

				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get(Id);
				if(workScheduleDb == null)
				{
					errors.Add("Work Schedule not found.");
				}
				else
				{
					//Delete workSchedule details first
					var workScheduleDetailsDb = Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.GetByWorkScheduleId(workScheduleDb.Id);
					if(workScheduleDetailsDb != null && workScheduleDetailsDb.Count != 0)
					{
						var workScheduleDetailIds = workScheduleDetailsDb.Select(wsd => wsd.Id).ToList();
						Infrastructure.Data.Access.Tables.WPL.WorkScheduleDetailsAccess.Delete(workScheduleDetailIds);
					}
					//if(Article.EditArticle(user, workScheduleDb.ArticleId))
						Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Delete(workScheduleDb.Id);
					//else
					//	errors.Add("Article is locked");

				}

				if(errors.Count == 0)
				{
					var hallDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(workScheduleDb.HallId);
					var articleDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(workScheduleDb.ArticleId);

					Log.NewLog(Core.Apps.WorkPlan.Enums.LogEnums.LogType.WorkSchedule,
						$"{user.Username} deleted a Work Schedule {workScheduleDb.Name} for {hallDb.Name} and {articleDb?.ArtikelNummer}.",
						user.Id);
					return Ok(new { response = new Models.Response<string>(true, $"Edit Success. Log Add success", errors) });
				}
				else
				{
					return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet("{CountryId}/{ArticleId}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult GetByCountryIdArticleId(int CountryId, int ArticleId)
		{
			var errors = new List<string>();

			try
			{
				var hallsDb = Infrastructure.Data.Access.Tables.WPL.HallAccess.GetByCountryId(CountryId);
				var hallsIds = hallsDb.Select(h => h.Id).ToList();
				var workScheduleDb = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.GetByHallIds(hallsIds);
				workScheduleDb = workScheduleDb.FindAll(ws => ws.ArticleId == ArticleId);

				if(workScheduleDb == null)
				{
					errors.Add("Work schedule not found");
				}
				else
				{
					var workScheduleVM = new List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>();
					workScheduleDb.ForEach(wsdb => workScheduleVM.Add(new Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel(wsdb)));

					return Ok(new { response = new Models.Response<List<Core.Apps.WorkPlan.Models.WorkPlan.WorkScheduleViewModel>>() { Success = true, ResponseBody = workScheduleVM, Errors = errors } });
				}

				return Ok(new { response = new Models.Response<string>() { Success = false, ResponseBody = "", Errors = errors } });
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpGet("{Id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportScheduleToExcel(int Id, string lang)
		{
			try
			{
				var fileBytesResponse = Core.Apps.WorkPlan.Handlers.WorkSchedule.ExportToExcel(Id, lang/* this.GetCurrentUser()?.SelectedLanguage*/);
				if(fileBytesResponse.Success)
				{
					return File(fileBytesResponse.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"WorkSchedule-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[AllowAnonymous]
		[HttpGet("{Id}")]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportScheduleFAToExcel(int Id, string lang, int faQuantity)
		{
			try
			{
				var fileBytesResponse = Core.Apps.WorkPlan.Handlers.WorkSchedule.ExportToExcel(Id, lang, faQuantity/* this.GetCurrentUser()?.SelectedLanguage*/);
				if(fileBytesResponse.Success)
				{
					return File(fileBytesResponse.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"WorkSchedule-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult ImportScheduleFromExcel([FromForm] Models.WorkSchedule.ImportExcelModel model)
		{
			try
			{
				// AllowAnonymous <<<<<<<
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}
				// var user = new Core.Identity.Models.UserModel{
				//    Id =-1
				//};

				var file = model.WPLFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return Ok("No file sent.");
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

					return Ok(Core.Apps.WorkPlan.Handlers.WorkSchedule.ExtractFromExcel(filePath, model.Id, user));
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
				//return StatusCode(500, "Internal server error");
			}
		}

		[AllowAnonymous]
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public ActionResult ExportScheduleAllToExcel()
		{
			try
			{
				var wsd = Infrastructure.Data.Access.Tables.WPL.WorkPlanAccess.Get();
				if(wsd != null)
				{
					var tempFolder = System.IO.Path.GetTempPath();
					var zipFilePath = System.IO.Path.Combine(tempFolder, $"WorkSchedules.zip");
					var files = new List<string>();
					foreach(var item in wsd)
					{
						var fileBytesResponse = Core.Apps.WorkPlan.Handlers.WorkSchedule.ExportToExcel(item.Id, this.GetCurrentUser()?.SelectedLanguage);
						if(fileBytesResponse.Success)
						{
							var filePath = System.IO.Path.Combine(tempFolder, $"WorkSchedule-{item.Id}-{DateTime.Now.ToString("yyyyMMddTHHmmss.fff")}.xlsx");
							System.IO.File.WriteAllBytes(filePath, fileBytesResponse.Body);
							files.Add(filePath);
						}
					}

					if(files.Count > 0)
					{
						foreach(var file in files)
						{
							// Create and open a new ZIP file
							using(var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
							{
								// Add the entry for each file
								zip.CreateEntryFromFile(file, System.IO.Path.GetFileName(file), CompressionLevel.Optimal);
							}
						}
					}
					return File(System.IO.File.ReadAllBytes(zipFilePath), "application/zip", $"WorkSchedules.zip");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.WorkPlan.Models.WorkSchedule.GetSimulationResponseModel>), 200)]
		public IActionResult GetSimulation(Core.Apps.WorkPlan.Models.WorkSchedule.GetSimulationRequestModel data)
		{
			try
			{
				return Ok(new Core.Apps.WorkPlan.Handlers.GetSimulationHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// - 
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.Apps.WorkPlan.Models.WorkSchedule.ArticlesWoWPLModel>>), 200)]
		public ActionResult GetArticleswoWPL(Core.Apps.WorkPlan.Models.WorkSchedule.ArticlesWoWorkPlanRequestModel data)
		{
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.GetArticlesWoWPLHandler(this.GetCurrentUser(), data)
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
		public ActionResult ExportArticleswoWPL(Core.Apps.WorkPlan.Models.WorkSchedule.ArticlesWoWorkPlanRequestModel data)
		{
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.ExportArticlesWoWPLHandler(this.GetCurrentUser(), data)
				   .Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
						, $"ArticleswoWPL-{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.Apps.WorkPlan.Models.WorkSchedule.ArticleWPLTimeDiffModel>>), 200)]
		public ActionResult GetArticlesWPLTimeDiff(int? countryId, int? hallId, decimal? minDiff)
		{
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.GetArticleWPLTimeDiffHandler(this.GetCurrentUser(), countryId, hallId, minDiff)
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
				var response = new Core.Apps.WorkPlan.Handlers.ExportArticleWPLTimeDiffHandler(this.GetCurrentUser(), countryId, hallId, minDiff)
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
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.Apps.WorkPlan.Models.WorkSchedule.ArticleFaTimeDiffModel>>), 200)]
		public ActionResult GetArticlesFATimeDiff(int? lager)
		{
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.GetArticleFaTimeDiffHandler(this.GetCurrentUser(), lager)
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
				var response = new Core.Apps.WorkPlan.Handlers.ExportArticleFaTimeDiffHandler(this.GetCurrentUser(), lager)
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

		// -
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.Apps.WorkPlan.Models.WorkSchedule.CountryModel>>), 200)]
		public ActionResult GetCountries()
		{
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.GetCountriesHandler(this.GetCurrentUser())
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}


		//Import xls file 
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult ImportWSExcel([FromForm] Models.WorkSchedule.ImportExcelModel model)
		{
			try
			{
				var user = this.GetCurrentUser();
				if(user == null)
				{
					return Ok("Authentication: User not found");
				}

				var file = model.WPLFile;
				if(file == null || file.Length == 0)
				{
					return Ok("No file sent.");
				}

				
				var tempFilePath = Path.Combine(Path.GetTempPath(),
					DateTime.Now.ToString("yyyyMMddTHHmmss_") + file.FileName);

				using(var fileStream = new FileStream(tempFilePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				
				List<string> errors;
				var excelData = Core.Apps.WorkPlan.Handlers.WorkSchedule.ReadFromExcel(
					tempFilePath,
					user.Id,
					articleNumber: "", 
					out errors,
					string.IsNullOrEmpty(user.SelectedLanguage) ? "EN" : user.SelectedLanguage
				);

				return Ok(new
				{
					Success = errors == null || errors.Count == 0,
					Errors = errors,
					Body = excelData is null ? null : new WorkScheduleExcelModel
					{
						ArtikelNr = excelData.ArtikleNr,
						Artikelnummer = excelData.Artikelnummer,
						Positions = excelData.Positions?.Select(x => new WorkScheduleDetailsModel(x)).ToList()
					}

				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		//public IActionResult ImportWSExcel([FromForm] Models.WorkSchedule.ImportExcelModel model)
		//{
		//	try
		//	{
		//		// AllowAnonymous <<<<<<<
		//		var user = this.GetCurrentUser();
		//		if(user == null)
		//		{
		//			return Ok("Authentication: User not found");
		//		}
		//		// var user = new Core.Identity.Models.UserModel{
		//		//    Id =-1
		//		//};

		//		var file = model.WPLFile;
		//		if(file == null)
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
		//			return Ok("No file sent.");
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

		//			return Ok(Core.Apps.WorkPlan.Handlers.WorkSchedule.ExtractExcel(filePath, model.Id, user));
		//		}
		//		else
		//		{
		//			Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
		//			return Ok("Empty file sent.");
		//		}
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e);
		//		//return StatusCode(500, "Internal server error");
		//	}
		//}

		// create wpl
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public IActionResult CreateWorkPlan(Core.Apps.WorkPlan.Models.WorkPlan.WorkPlanCreateModel data)
		{
			
			try
			{
				var response = new Core.Apps.WorkPlan.Handlers.WorkPlan.CreateWorkPlanHandler(this.GetCurrentUser(),data).Handle();

				return Ok(new
				{
					response = new Models.Response<string>()
					{
						Success = true,
						ResponseBody = response.Body.ToString()
					}
				});
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
