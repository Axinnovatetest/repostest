using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CustomerService.Models.Delfor;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Psz.Api.Areas.EDI.Controllers
{
	[Authorize]
	[Area("EDI")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class DelforController: ControllerBase
	{
		private const string MODULE = "EDI | DELFOR";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.GetCustomerModel>>), 200)]
		public IActionResult GetCustomers(string searchText)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetCustomersHandler(this.GetCurrentUser(), searchText).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, searchText);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.GetCustomerModel>>), 200)]
		public IActionResult GetCustomerStatus(bool? validated)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetCustomersStatusHandler(this.GetCurrentUser(), validated).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, validated);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.GetForecastModel>>), 200)]
		public IActionResult GetCustomerForecasts(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastByCustomerHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforDocumentResponseModel>>), 200)]
		public IActionResult GetCustomerFullForecasts(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastFullByCustomerHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.EDI.Models.Delfor.GetForecastModel>), 200)]
		public IActionResult GetForecastHeader(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastHeaderHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemModel>>), 200)]
		public IActionResult GetForecastLineItemsByHeader(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemsHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemModel>), 200)]
		public IActionResult GetForecastLineItem(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetLineItemHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemModel>), 200)]
		public IActionResult GetForecastLineItemPrevVersion(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetLineItemPrevVesionHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemModel>), 200)]
		public IActionResult GetForecastLineItemNextVersion(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetLineItemNextVesionHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemPlanModel>>), 200)]
		public IActionResult GetForecastLineItemPlansByLine(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemPlansByLineHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemPlanModel>>), 200)]
		public IActionResult GetForecastLineItemPlansByHeader(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemPlansByHeaderHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.LineItemPlanOrderModel>>), 200)]
		public IActionResult GetForecastLineItemPlanOrders(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemPlanOrdersHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.LineItemPlanDeliveryNoteModel>>), 200)]
		public IActionResult GetForecastLineItemPlanDeliveryNotes(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemPlanDeliveryNotesHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemPlanFullModel>>), 200)]
		public IActionResult GetForecastLineItemPlansFullByHeader(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetForecastLineItemPlansFullByHeaderHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.Apps.EDI.Models.Delfor.XMLLineItemPlanModel>), 200)]
		public IActionResult GetForecastLineItemPlan(int id)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.GetLineItemPlanHandler(this.GetCurrentUser(), id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public async Task<IActionResult> GetForecastPDFByLineItem(int id)
		{
			try
			{
				var response = await new Core.Apps.EDI.Handlers.Delfor.GetForecastPDFHandler(this.GetCurrentUser(), id).HandleAsync();
				if(response.Success)
				{
					return Ok(Core.Models.ResponseModel<string>.SuccessResponse(Convert.ToBase64String(response.Body)));
				}
				else
				{
					return Ok(response);
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// - Production
		private const string MODULE_PROD = "EDI | DELFOR | PRODUCTION";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.Production.ManufacturingFacilityModel>>), 200)]
		public IActionResult GetManufacturingFacilities()
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.Production.GetManufacturingFacilitiesHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.Apps.EDI.Models.Delfor.Production.TechnicianModel>>), 200)]
		public IActionResult GetTechinicians()
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.Production.GetTechiniciansHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult ConfirmCommand(Psz.Core.Apps.EDI.Models.Delfor.Production.ValidateCommandModel data)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.Production.ConfirmCommandHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CancelCommand(Psz.Core.Apps.EDI.Models.Delfor.Production.CancelCommandModel data)
		{
			try
			{
				return Ok(/*new Core.Apps.EDI.Handlers.Delfor.Production.CancelCommandHandler(this.GetCurrentUser(), data).Handle()*/);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		// -
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddOrder(Psz.Core.Apps.EDI.Models.Delfor.Production.ValidateCommandModel data)
		{
			try
			{
				return Ok(new Core.Apps.EDI.Handlers.Delfor.Production.AddOrderHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_PROD })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteOrder(Psz.Core.Apps.EDI.Models.Delfor.Production.CancelCommandModel data)
		{
			try
			{
				return Ok(/*new Core.Apps.EDI.Handlers.Delfor.Production.DeleteOrderHandler(this.GetCurrentUser(), data).Handle()*/);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		// - Customer Service
		private const string MODULE_CTS = "EDI | DELFOR | CTS";
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE_CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<DeliveryForcastLineItemModel>>), 200)]
		public IActionResult ImportDelforFromXLS([FromForm] Psz.Core.Common.Models.IAttachmentRequestModel data)
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
						new Psz.Core.CustomerService.Handlers.Delfor.ImportDeflorFromExcelHandler(user,
						new Core.Common.Models.ImportFileModel
						{
							FilePath = filePath,
							CheckFrequency = data.CheckFrequency,
							CommaSeperator = data.CommaSeperator,
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

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_CTS })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<DeliveryForcastHeaderModel>), 200)]
		public IActionResult GetCustomerInformations(int Id)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.Delfor.GetCustomerInformationsHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE_CTS })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>), 200)]
		public IActionResult GetCustomersList()
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.Delfor.GetCustomersForDelforHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE_CTS })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult SaveDelfor(Psz.Core.CustomerService.Models.Delfor.DeliveryForcastModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.SaveDelforHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetDelforDraft()
		{
			var data = new Psz.Core.CustomerService.Handlers.Delfor.DelforDraftHandler(this.GetCurrentUser()).Handle();
			if(data.Body != null)
			{
				return File(data.Body, "application/xlsx", $"Delfor.xlsx");
			}
			else
			{
				return Ok("Empty file sent.");
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateABFromDelfor(Psz.Core.CustomerService.Models.Delfor.CreateABFromDelforModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.CreateABFromDelforHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Delfor.DelforDocumentResponseModel>), 200)]
		public IActionResult SearchManualDelfor(Psz.Core.CustomerService.Models.Delfor.SearchManualDelforRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.SearchManualDeflorHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Delfor.AnalysisResponseModel>), 200)]
		public async Task<IActionResult> GetAnalysisDelfor(AnalysisRequestModel data)
		{
			try
			{
				return Ok(await new Psz.Core.CustomerService.Handlers.Delfor.GetAnalysisHandler(this.GetCurrentUser(), data)
					.HandleAsync());
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> GetAnalysisDelforXLS(AnalysisRequestModel data)
		{
			try
			{
				// - XLS get all items
				var results = await new Psz.Core.CustomerService.Handlers.Delfor.GetAnalysisHandler(this.GetCurrentUser(), data)
					.GetDataXLS();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"DLF-Analysis-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforItemsResponseModel>>), 200)]
		public IActionResult GetDelforItemsByVersion(Psz.Core.CustomerService.Models.Delfor.DelforItemsRequsetModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetDelforItemsByVersionHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforItemsResponseModel>>), 200)]
		public IActionResult GetDelfrVersions(Psz.Core.CustomerService.Models.Delfor.DelforVersionsRequsetModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetDelforVersionsHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDelfor(Psz.Core.CustomerService.Models.Delfor.DeleteDelforModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.DeleteDelforHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDelforInList(Psz.Core.CustomerService.Models.Delfor.DeleteDelforInListmodel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.DeleteDelforInListhandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteDelforItemPlan(int id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.DeleteDelforItemPlanHandler(this.GetCurrentUser(), id).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforErrorsModel>>), 200)]
		public IActionResult GetDelforCustomerErrors(Psz.Core.CustomerService.Models.Delfor.DelforErrorsRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetCustomerErrorsHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforValidatedErrorsModel>>), 200)]
		public IActionResult GetDelforCustomerValidatedErrors(Psz.Core.CustomerService.Models.Delfor.DelforErrorsRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetCustomerValidatedErrorsHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadErrorFile(int id)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.Delfor.DownloadErrorFileHandler(this.GetCurrentUser(), id).Handle();
				if(response.Success)
				{
					return new FileContentResult(response.Body.Key, "application/xml")
					{
						FileDownloadName = $"{response.Body.Value}.xml"
					};
				}
				else
				{
					return Ok(response);
				}
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ReloadDelforFile(int id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.ReloadDelforFileHandler(this.GetCurrentUser(), id).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Delfor.DelforItemsResponseModel>>), 200)]
		public IActionResult GetDelforItemsForNavigation(int id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetDelforPositinsForNavigationHandler(this.GetCurrentUser(), id).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult GetDelforErrorsCount()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetDelforErrorsCountHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ValidateDelforError(int id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.ValidateErrorHandler(this.GetCurrentUser(), id).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToggleStatus(ToggleStatusRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.ToggleStatusHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Delfor.DelforCustomerResponseModel>), 200)]
		public IActionResult GetArchived(bool data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetArchivedHeaderHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Delfor.DelforDocumentResponseModel>), 200)]
		public IActionResult GetArchivedDocuments(Psz.Core.CustomerService.Models.Delfor.DelforDocumentRequestModel data)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetArchivedDocumentsHandler(this.GetCurrentUser(), data).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.CTSLoggingModel>), 200)]
		public IActionResult GetClosedLogs()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Delfor.GetClosedLogHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>), 200)]
		public IActionResult GetMyCustomers(string searchText)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.Delfor.GetMyCustomersForDelforHandler(this.GetCurrentUser(), searchText).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}