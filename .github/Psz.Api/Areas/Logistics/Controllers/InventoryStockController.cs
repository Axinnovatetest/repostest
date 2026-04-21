using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.Logistics.Handlers.InventoryStock;
using Psz.Core.Logistics.Handlers.InventoryStockHandlers;
using Psz.Core.Logistics.Handlers.PlantBookings;
using Psz.Core.Logistics.Interfaces;
using Psz.Core.Logistics.Models.InverntoryStockModels;
using Psz.Core.Logistics.Models.PlantBookings;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.CustomerService.Enums.InsideSalesEnums;

namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistic")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class InventoryStockController: Controller
	{
		private const string MODULE = "InventoryStock";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetUserWarehouses()
		{
			try
			{
				var response = new GetUserWarehousesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<InventoryFaStatsModel>), 200)]
		public IActionResult GetWarehouseInventoryStats(int lagerID)
		{
			try
			{
				return Ok(new GetWarehouseInventoryStatsHandler(this.GetCurrentUser(), lagerID).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<InventoryFaStatsModel>), 200)]
		public IActionResult UpdateInventoryOffDate(int lagerId)
		{
			try
			{
				return Ok(new StopInventoryHandler(this.GetCurrentUser(), lagerId).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#region OrdersWipTable
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.ProdWip_Tabl_Reponse_Model>), 200)]
		public IActionResult GetWipProdData(ProdWip_Tabl_RequestModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.ProdOrderWipTblHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetReportFourData_XLS(ProdWip_Tabl_RequestModel data)
		{
			try
			{
				var results = new ProdOrderWipTblHandler(data, this.GetCurrentUser()).GetReportFourXls();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetReport2FourData_XLS(int warehouseId)
		{
			try
			{
				var results = new Psz.Core.Logistics.Handlers.InventoryStock.ProdWip2downloadhandler(this.GetCurrentUser(), warehouseId).Handle();
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
				return this.HandleException(e, warehouseId);
			}
		}

		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UploadWipXLS([FromForm] ImportFromXlsModel data)
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
						new Psz.Core.Logistics.Handlers.InventoryStock.UpdateWipFaHandler(user,
						new Psz.Core.Logistics.Models.InverntoryStockModels.UploadReport4WipRequestModel
						{
							Date = data.Date,
							AttachmentFilePath = filePath,
							LagerId=data.LagerId ?? -1
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


		#endregion OrdersWipTable
		
		#region ReportsTable
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ReportOne_tbl_Reponse_Model>), 200)]
		public IActionResult GetReportOneData(ReportOneRequestTblModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.ReportOneTblHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetReportOneData_XLS(ReportOneRequestTblModel data)
		{
			try
			{
				var results = new ReportOneTblHandler(data, this.GetCurrentUser()).GetReportOneXls();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report 1-{DateTime.Now:yyyyMMddHHmm}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<ReportTwo_tbl_Reponse_Model>), 200)]
		public IActionResult GetReportTwoData(ReportTwoRequestTblModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.ReportTwoTblHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetReportTwoData_XLS(ReportTwoRequestTblModel data)
		{
			try
			{
				var results = new ReportTwoTblHandler(data, this.GetCurrentUser()).GetReportTwoXls();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<RohInProductionPaginatedReponseModel>), 200)]
		public IActionResult GetReportThreeData(RohInProductionRequestModel data)
		{
			try
			{
				data.WarenType = 1;
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.ReportRohInProductionHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetReportThreeData_XLS(RohInProductionRequestModel data)
		{
			try
			{
				data.WarenType = 1;
				var results = new ReportRohInProductionHandler(data, this.GetCurrentUser()).GetReport3();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<RohInProductionPaginatedReponseModel>), 200)]
		public IActionResult GetReport4Data(RohInProductionRequestModel data)
		{
			try
			{
				data.WarenType = 2;
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.ReportRohInProductionHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetReport4Data_XLS(RohInProductionRequestModel data)
		{
			try
			{
				data.WarenType = 2;
				var results = new ReportRohInProductionHandler(data, this.GetCurrentUser()).GetReportXls();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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

		#endregion ReportsTable

		#region TaskByRole
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<UpdateTaskRequestModel>), 200)]
		public IActionResult GetListTaskByRole(int LagerId)
		{
			try
			{
				return Ok(new TaskByRoleHandler(this.GetCurrentUser(), LagerId).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateTaskByStatus(UpdateTaskRequestModel data)
		{
			try
			{
				var response = new UpdateTaskStatusHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion TaskByRole

		#region Logs
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetLogResponseModel>), 200)]
		public IActionResult GetAllIstLogsData(GetLogRequestModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.InventoryStock.getLogsHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Logs

		#region Calculate Start Stop Ist
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CalculateStartStopIst(Psz.Core.Logistics.Models.InverntoryStockModels.UploadChoiceModel data)
		{
			try
			{
				return Ok(new StartInventoryHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion Calculate Start Stop Ist

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetReport_XLS(ReportRequestModel data)
		{
			try
			{
				var results = new GetReportHandler(data, this.GetCurrentUser()).Handle();
				if(results != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report-{DateTime.Now:yyyyMMddHHmm}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetInInventory()
		{
			try
			{
				var response = new GetWarehouseInInventoryHandler( this.GetCurrentUser()).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ResetLast(int warehouseId)
		{
			try
			{
				var response = new ResetLastInventoryHandler(this.GetCurrentUser(), warehouseId).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult GetReleaseReport(int warehouseId)
		{
			try
			{
				var response = new GetReleaseReportHandler(this.GetCurrentUser(), warehouseId).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateWarehouseNotes(Psz.Core.Logistics.Models.InverntoryStockModels.UpdateNotesRequestModel data)
		{
			try
			{
				var response = new UpdateWarehouseNotesHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateHQNotes(Psz.Core.Logistics.Models.InverntoryStockModels.UpdateNotesRequestModel data)
		{
			try
			{
				var response = new HqUpdateNotesHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ApproveInventoryRelease(int warehouseId)
		{
			try
			{
				var response = new HqApproveInventoryReleaseHandler(this.GetCurrentUser(), warehouseId).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult RejectInventoryRelease(KeyValuePair<int, string> data)
		{
			try
			{
				var response = new HqRejectInventoryReleaseHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetInventoryPrices(int warehouseId)
		{
			try
			{
				var results = new GetReportHandler(new ReportRequestModel { LagerId = warehouseId, SearchValue = null, ReportId = 12 }, this.GetCurrentUser()).Handle();
				if(results != null && results.Body.Length > 0)
				{
					return File(results.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report-{DateTime.Now:yyyyMMddHHmm}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e, warehouseId);
			}
		}
	}
}

