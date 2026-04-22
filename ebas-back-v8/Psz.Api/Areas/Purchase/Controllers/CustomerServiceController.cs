
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Areas.Purchase.Controllers
{
	[Authorize]
	[Area("Purchase")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class CustomerServiceController: ControllerBase
	{
		private const string MODULE = "Purchase";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Apps.Purchase.Models.CustomerService.OrderReport.CreateModel>>), 200)]
		public IActionResult GetReports()
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.GetAllHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.CustomerService.OrderReport.CreateModel>), 200)]
		public IActionResult GetReport(int typeId, int languageId)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.GetSingleHandler(this.GetCurrentUser(),
					new Core.Apps.Purchase.Models.RequestModel { LanguageId = languageId, TypeId = typeId }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public async Task<IActionResult> GetReportFile(int typeId, int languageId, int orderId)
		{
			try
			{
				var response = await new Psz.Core.Apps.Purchase.Handlers.CustomerService.GetReportHandler(
						this.GetCurrentUser(), new Core.Apps.Purchase.Models.CustomerService.OrderReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							OrderId = orderId
						}).HandleAsync();

				if(response.Success)
				{
					return Ok(new Psz.Core.Common.Models.ResponseModel<string>
					{
						Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
						Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
						Infos = response.Infos,
						Success = response.Success,
						Warnings = response.Warnings,

					});
				}
				else
				{
					return Ok(response);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public async Task<IActionResult> GetReportFileBase64(int typeId, int languageId, int orderId)
		{
			try
			{
				var response = await new Core.Apps.Purchase.Handlers.CustomerService.GetReportHandler(
						this.GetCurrentUser(),
						new Core.Apps.Purchase.Models.CustomerService.OrderReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							OrderId = orderId
						})
					.HandleAsync();

				if(response.Success)
				{
					return Ok(Core.Common.Models.ResponseModel<string>.SuccessResponse(Convert.ToBase64String(response.Body)));
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

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Apps.Purchase.Models.CustomerService.DeliveryNote.CreateDelieveryNoteModel>), 200)]
		public IActionResult GetReportDelieveryNote(int typeId, int languageId)
		{
			try
			{
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.GetSingleDelieveryNoteHandler(this.GetCurrentUser(),
					new Core.Apps.Purchase.Models.RequestModel { LanguageId = languageId, TypeId = typeId }).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public async Task<IActionResult> GetReportFileDelieveryNote(int typeId, int languageId, int orderId)
		{
			try
			{
				var response = await new Psz.Core.Apps.Purchase.Handlers.CustomerService.GetReportDelieveryNoteHandler(
						this.GetCurrentUser(), new Core.Apps.Purchase.Models.CustomerService.OrderReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							OrderId = orderId
						}).HandleAsync();

				if(response.Success)
				{
					return Ok(new Psz.Core.Common.Models.ResponseModel<string>
					{
						Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
						Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
						Infos = response.Infos,
						Success = response.Success,
						Warnings = response.Warnings,

					});
				}
				else
				{
					return Ok(response);
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
		public IActionResult UpdateReport([FromForm] Models.CustomerService.CreateReportModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.UpdateHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeliveryReport(Models.CustomerService.CreateDeliveryNoteReportModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.UpdateHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeliveryReport2([FromForm] Models.CustomerService.CreateDeliveryNoteReportModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.UpdateHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateReportImportLogo([FromForm] Models.CustomerService.LogoImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.UpdateImportLogoHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeliveryReportImportLogo([FromForm] Models.CustomerService.LogoImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.DeliveryNote.UpdateDeliveryImportLogoHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateReportCompanyLogo([FromForm] Models.CustomerService.LogoImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.UpdateComapnyLogoHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateDeliveryReportCompanyLogo([FromForm] Models.CustomerService.LogoImageModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.Apps.Purchase.Handlers.CustomerService.DeliveryNote.UpdateDeliveryComapnyLogoHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Apps.Purchase.Models.CustomerService.LanguageModel>>), 200)]
		public IActionResult GetLanguages()
		{
			try
			{
				var response = new Core.Apps.Purchase.Handlers.CustomerService.GetLanguagesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Apps.Purchase.Models.CustomerService.OrderTypeModel>>), 200)]
		public IActionResult GetOrderTypes()
		{
			try
			{
				var response = new Core.Apps.Purchase.Handlers.CustomerService.GetOrderTypesHandler(this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(FileContentResult), 200)]
		public IActionResult GenerateProductionReport(int productionId)
		{
			try
			{
				var handlerResponse = new Core.Apps.Purchase.Handlers.Production.Reporting.GenerateReportHandler(productionId,
					this.GetCurrentUser()).Handle();

				if(!handlerResponse.Success)
				{
					return NotFound();
				}

				return new FileContentResult(handlerResponse.Body, "application/pdf")
				{
					FileDownloadName = $"production-{productionId}-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(FileContentResult), 200)]
		public IActionResult GetFile(int productionId)
		{
			try
			{
				var handlerResponse = new Core.Apps.Purchase.Handlers.Production.Reporting.GenerateReportHandler(productionId,
					this.GetCurrentUser()).Handle();

				if(!handlerResponse.Success)
				{
					return NotFound();
				}

				return new FileContentResult(handlerResponse.Body, "application/pdf")
				{
					FileDownloadName = $"production-{productionId}-{DateTime.Now.ToString("yyyyMMDDHHmmss")}"
				};
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}