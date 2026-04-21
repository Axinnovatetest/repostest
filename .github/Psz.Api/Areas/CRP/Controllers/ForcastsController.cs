using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Handlers.FAPlannung;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.Logistics.Handlers.PlantBookings;
using Psz.Core.Logistics.Models.PlantBookings;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;


namespace Psz.Api.Areas.CRP.Controllers
{
	[Authorize]
	[Area("CRP")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class ForcastsController: ControllerBase
	{
		private const string MODULE = "CRP | FORECASTS";
		private readonly ICrpForecastsService _crpForcastsService;

		public ForcastsController(ICrpForecastsService crpForcastsService)
		{
			_crpForcastsService = crpForcastsService;
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<IEnumerable<ForecastPositionModel>>), 200)]
		public IActionResult ImportForcastFromXLS([FromForm] Core.Common.Models.IAttachmentRequestModel data)
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
						_crpForcastsService.ImportForecastFromXLS(user,
						new Core.Common.Models.ImportFileModel
						{
							FilePath = filePath,
							ForcastDraftTypeId = data.ForcastDraftTypeId,
							CheckFrequency = data.CheckFrequency,
							CommaSeperator = data.CommaSeperator,
						}));
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult AddForcast(ForecastModel data)
		{
			try
			{
				var response = _crpForcastsService.AddForecast(this.GetCurrentUser(), data);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
				throw;
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<IEnumerable<ForecastHeaderModel>>), 200)]
		public IActionResult GetForecasts()
		{
			try
			{
				var response = _crpForcastsService.GetForecasts(this.GetCurrentUser());
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
				throw;
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ForecastHeaderModel>), 200)]
		public IActionResult GetForecastHeader(int id)
		{
			try
			{
				var response = _crpForcastsService.GetForecastHeader(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<ForecastHeaderModel>), 200)]
		public IActionResult GetForecastPositions(ForecastPositonsRequestModel model)
		{
			try
			{
				var response = _crpForcastsService.GetForecastPositions(this.GetCurrentUser(), model);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		//[HttpGet]
		//[SwaggerOperation(Tags = new[] { MODULE })]
		//[ProducesResponseType(typeof(ResponseModel<ForecastModel>), 200)]
		//public IActionResult GetForecast(int id)
		//{
		//	try
		//	{
		//		var response = _crpForcastsService.GetForecast(this.GetCurrentUser(), id);
		//		return Ok(response);
		//	} catch(Exception e)
		//	{
		//		return this.HandleException(e);
		//		throw;
		//	}
		//}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult DeleteForecast(int id)
		{
			try
			{
				var response = _crpForcastsService.DeleteForecast(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
				throw;
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadForecastExcel(CRPExcelRequestModel data)
		{
			try
			{
				var results = _crpForcastsService.GetForecastsExcel(this.GetCurrentUser(), data);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Forecast_Auswertung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public IActionResult DownloadForecastDraft(int type)
		{
			try
			{
				var results = _crpForcastsService.GetForecastsDraft(this.GetCurrentUser(), type);
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"Forecast_Draft-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(ResponseModel<int>), 200)]
		public IActionResult ToggleForecastPositionOrdered(int id)
		{
			try
			{
				var response = _crpForcastsService.ToggleForecastPositionOrdered(this.GetCurrentUser(), id);
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
				throw;
			}
		}

	}
}