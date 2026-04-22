using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class FGBestandController: ControllerBase
	{
		private const string MODULE = "CRP/FgBestand";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<LogsResponseModel>), 200)]
		public IActionResult GetHistoryFGHeaderData(HistoryFGHeaderRequestModel data)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.HistoryFG.GetHistoryHeaderFGBestandHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<byte[]>), 200)]
		public IActionResult GetFGBestandHeader_XLS()
		{
			try
			{
				var results = new Psz.Core.CRP.Handlers.HeaderFG.GetFGBestandHeaderDraftHandler(this.GetCurrentUser()).Handle();

				return File(results.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");

			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<HistoryFGDetailsResponseModel>), 200)]
		public IActionResult GetHistoryFGDetailsData(HistoryDataFGDetailsRequestModel data)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.HistoryFG.GetDetailsFGBestandHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.CRP.Models.HistoryFG.HistoryDataFGDetailsResponseModel>>), 200)]
		public IActionResult ImportHistoryFGFromXLS([FromForm] Models.HistoryFGImportXLSModel data)
		{
			try
			{
				Infrastructure.Services.Logging.Logger.Log($"{this.GetCurrentUser()?.Id} >>> MST-Article ImportBOM");
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
						new Psz.Core.CRP.Handlers.HistoryFG.ImportHistoryFGByExcelHandler(user,
						new Core.CRP.Models.HistoryFG.ImportFromExcelRequestModel
						{
							Date = data.Date,
							AttachmentFilePath = filePath,
						}).Handle());
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
		public IActionResult PostFGFromXlsIntoDetails(Psz.Core.CRP.Models.HistoryFG.HistoryDataDetailsRequestFGModel data)
		{
			try
			{

				var response = new Psz.Core.CRP.Handlers.HistoryFG.InsertFGXlsIntoDetailsHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<HistoryFGDetailsResponseModel>), 200)]
		public IActionResult GetPositionFGDData(HistoryDataFGDetailsRequestModel data)
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.HistoryFG.GetFGPositionDetailsHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Export_Bestellen_FG(FgHistoryBestandXlsStatisticsModel data)
		{
			try
			{
				var results = new Psz.Core.CRP.Handlers.HistoryFG.BestellenFGHistoryXlsHandler(data, this.GetCurrentUser()).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Bestellen_FG_data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public IActionResult FgBestandHistorieRefreshData()
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.HistoryFG.FGBestandHistorieRefreshHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<string>), 200)]
		public IActionResult GetHistorieFgBestandAgentLastExcutionTime()
		{
			try
			{
				var response = new Psz.Core.CRP.Handlers.HistoryFG.GetHistorieFgBestandAgentLastExecutionTimeHandler(this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

	}
}
