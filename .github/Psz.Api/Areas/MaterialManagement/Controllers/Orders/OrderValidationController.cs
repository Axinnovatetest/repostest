using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrderValidationController: Controller
	{
		private const string MODULE = "Material Management | Orders Validation";

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderValidation.ValidateResponseModel>), 200)]
		public IActionResult Validate(Core.MaterialManagement.Orders.Models.OrderValidation.ValidateRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.ValidateHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderValidation.ClientModel.ClientRequestModel>), 200)]
		public IActionResult GetClient(Core.MaterialManagement.Orders.Models.OrderValidation.ClientModel.ClientRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.GetClientHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetBestellungReport(int bestellungNr)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.GenerateReportHandler(this.GetCurrentUser(false), bestellungNr)
				   .Handle();

				if(response.Success)
				{
					return Ok(new Psz.Core.Common.Models.ResponseModel<string>(Convert.ToBase64String(response.Body)));
				}
				else
				{
					return Ok(response);
				}

			} catch(Exception e)
			{

				return this.HandleException(e, bestellungNr);
			}

		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<bool>), 200)]
		public IActionResult UnValidate(Core.MaterialManagement.Orders.Models.OrderValidation.UnValidateRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.UnValidateOrderHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{

				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderValidation.PlaceOrderResponseModel>), 200)]
		//[Consumes("multipart/form-data")]
		public IActionResult PlaceOrder([FromForm] Core.MaterialManagement.Orders.Models.OrderValidation.placeOrderRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.PlaceOrderHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}


		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.OrderValidation.PlaceOrderInformationResponseModel>), 200)]
		public IActionResult PlaceOrderInformation(Core.MaterialManagement.Orders.Models.OrderValidation.PlaceOrderInformationRequestModel data = null)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.OrderinformationHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.OrderValidation.GetPlacementHistoryResponseModel>>), 200)]
		public IActionResult getOrderHistory(int data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.OrderValidation.GetPlacementHistoryHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.OrderValidation.GetPlacementHistoryResponseModel>>), 200)]

		public IActionResult getAttachedFile(int data)
		{
			try
			{
				var results = Psz.Core.Common.Helpers.ImageFileHelper.getFileData(data);

				switch(results?.FileExtension)
				{
					case ".png":
						return new FileContentResult(results.FileBytes, "application/png")
						{
							FileDownloadName = $"{results.FileName}.{results.FileExtension}"
						};
					case ".jpg":
					case ".jpeg":
						return new FileContentResult(results.FileBytes, "application/jpeg")
						{
							FileDownloadName = $"{results.FileName}.{results.FileExtension}"
						};
					default:
						return new FileContentResult(results.FileBytes, "application/blob")
						{
							FileDownloadName = $"{results.FileName}.{results.FileExtension}"
						};
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


	}


}
