using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Handlers.Statistics;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FAPlanning;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class FAPlannungController: ControllerBase
	{
		private const string MODULE = "CRP";
		private readonly ICrpFAPlannung _crpFAPlannung;

		public FAPlannungController(ICrpFAPlannung crpFAPlannung)
		{
			_crpFAPlannung = crpFAPlannung;
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FaultyFAReponseModel>), 200)]
		public IActionResult GetFaultyFA(FaultyFARequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetFaultyFA(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FaultyNeedsResponseModel>), 200)]
		public IActionResult GetFaultyNeeds(FaultyNeedsResquestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetFaultyNeeds(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ArticleInfoModel>), 200)]
		public IActionResult GetArticleInformation(int articleNr)
		{
			try
			{
				var response = _crpFAPlannung.GetArticleInformation(this.GetCurrentUser(), articleNr);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FaultyNeedsResponseModel>), 200)]
		public IActionResult GetFASystem(FASystemRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetFaSystem(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ArticlesResponseModel>), 200)]
		public IActionResult GetArticles(ArticlesRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetArticles(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<NumberKreisResponseModel>>), 200)]
		public IActionResult GetNumberKreis()
		{
			try
			{
				var response = _crpFAPlannung.GetNumberKreis(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArticleUBG(int articleNr)
		{
			try
			{
				var response = _crpFAPlannung.GetArticleUBG(this.GetCurrentUser(), articleNr);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUnits()
		{
			try
			{
				var response = _crpFAPlannung.GetUnits(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ArticleKwDetailResponseModel>), 200)]
		public IActionResult GetKwDetailHandler(ArticleKwDetailRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetKwDetailHandler(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult ExecuteFAPlannugAgent()
		{
			try
			{
				var response = _crpFAPlannung.ActivateFAPlannugAgent(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<FaPlanningLogRequestModel>>), 200)]
		public IActionResult GetLogs()
		{
			try
			{
				var response = _crpFAPlannung.GetLogs(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<FaPlanningLogRequestModel>), 200)]
		public IActionResult GetLastLog()
		{
			try
			{
				var response = _crpFAPlannung.GetLastLog(this.GetCurrentUser());
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		#region Historie
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.CRP.Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>>), 200)]
		public IActionResult ImportFaPlannungHistorieFromXLS([FromForm] Models.FaPlannungHistorieImportXLSModel data)
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
						_crpFAPlannung.GetHistorieFromExcel(user,
						new Core.Common.Models.ImportFileModel { FilePath = filePath, }));
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult SaveFaPlannungHistorie(Core.CRP.Models.FAPlanning.Historie.HistorieFaPlannungSaveRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.SaveFaPlannungHistorie(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CRP.Models.FAPlanning.Historie.FaPlannungHistorieHeadersResponseModel>), 200)]
		public IActionResult GetHistorieFaPlannungHeaders(Core.CRP.Models.FAPlanning.Historie.FaPlannungHistorieHeadersRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetHistorieFaPlannungHeaders(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.CRP.Models.FAPlanning.Historie.FaPlannungHistorieDetailsResponsetModel>), 200)]
		public IActionResult GetHistorieFaPlannungDetails(Core.CRP.Models.FAPlanning.Historie.FaPlannungHistorieDetailsRequestModel data)
		{
			try
			{
				var response = _crpFAPlannung.GetHistorieFaPlannungDetails(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[AllowAnonymous]
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(FileContentResult), 200)]
		public IActionResult GetHistorieFaPlannungExcel(Core.CRP.Models.FAPlanning.Historie.FAPlannungHistorieExcelRequestModel model)
		{
			try
			{
				var data = _crpFAPlannung.GetFaPlannungHistoryExcel(this.GetCurrentUser(), model);
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"FA Plannung Historie-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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
		[ProducesResponseType(typeof(FileContentResult), 200)]
		public IActionResult GetHistorieFaPlannungDraft()
		{
			try
			{
				var data = _crpFAPlannung.GetHsitorieFAPlannungDraft(this.GetCurrentUser());
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"FA Plannung Historie Draft-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult FaPlannungHistorieRefreshData()
		{
			try
			{
				var response = _crpFAPlannung.FAPlannungHistorieRefreshData(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<string>), 200)]
		public IActionResult GetHistorieFaPlannungAgentLastExcutionTime()
		{
			try
			{
				var response = _crpFAPlannung.GetHistorieFaPlannungAgentLastExcutionTime(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<List<Core.CRP.Models.FAPlanning.Historie.FAPlannungHistorieAgentLogModel>>), 200)]
		public IActionResult GetFaPlannungHistorieAgentFullLog()
		{
			try
			{
				var reeponse = _crpFAPlannung.GetFaPlannungHistorieAgentFullLog(this.GetCurrentUser());
				return Ok(reeponse);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<Core.CRP.Models.FAPlanning.Historie.FaPlannungHistorieHeadersModel>), 200)]
		public IActionResult GetHistorieFAPlannungHeaderById(int id)
		{
			try
			{
				var response = _crpFAPlannung.GetHistorieFAPlannungheaderById(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#endregion
	}
}
