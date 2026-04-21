using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.CustomerService.Models.SendGridEmailsManagement;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class OrderProcessingController: Controller
	{
		private const string MODULE = "CustomerService";
		#region AB
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateOrder(Core.CustomerService.Models.OrderProcessing.CreateOrderModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.CreateOrderHandler(data, this.GetCurrentUser())
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
		public IActionResult CreateOrderItem(Core.CustomerService.Models.OrderProcessing.CreateOrderItemModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.CreateOrderItemHandler(data, this.GetCurrentUser())
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
		public IActionResult UpdateOrderItem(Core.CustomerService.Models.OrderProcessing.UpdateOrderItemModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.UpdateOrderItemHandler(data, this.GetCurrentUser())
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
		public IActionResult UpdateOrderGlobalData(Core.CustomerService.Models.OrderProcessing.UpdateGlobalDataModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.UpdateGlobalDataHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.OrderModel>), 200)]
		public IActionResult GetOrder(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetOrderHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(List<Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.OrderItemModel>>), 200)]
		public IActionResult GetOrderItems(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetOrderItemsHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(List<Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.OrderItemModel>>), 200)]
		public IActionResult GetUserCustomers(string searchText)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetUserCustomersHandler(searchText, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<object>), 200)]
		public IActionResult ToggleBooked(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.ToggleBookedHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.CustomerService.Models.DeliveryNote.OrderElementModel>), 200)]
		public IActionResult GetDelieveryArtikel(Psz.Core.CustomerService.Models.DeliveryNote.GetArticleModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.DeliveryNote.GetArtikelHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult AddLSValidatdatedPosition(Core.Apps.Purchase.Models.DeliveryNote.ItemModel data)
		{

			try
			{
				/*[LS Pos zu AB Pos] for undo and update*/
				return Ok(new Core.Apps.Purchase.Handlers.DeliveryNote.AddLSValidatdatedPositionHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<KeyValuePair<DateTime?, DateTime?>>), 200)]
		public IActionResult UpdatePositionDates(Psz.Core.CustomerService.Models.OrderProcessing.PositionsDateEntryModel model)
		{

			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.OrderProcessing.UpdatePositionDatesHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, JsonSerializer.Serialize(model));
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Psz.Core.CustomerService.Models.OrderProcessing.OrderCustomerModel>), 200)]
		public IActionResult GetABNewCustomer(Psz.Core.CustomerService.Models.OrderProcessing.GeNewCustomerEntryModel model)
		{

			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.OrderProcessing.GetABNewCustomerHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.StoragesAndIndexesModel>), 200)]
		public IActionResult GetStoragesAndIndexes(string article)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetStoragesAndIndexesHandler(this.GetCurrentUser(), article)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.OrderProcessing.StoragesAndIndexesModel>), 200)]
		public IActionResult GetCustomerOrderEmail(int addressId)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetCustomerOrderEmailHandler(this.GetCurrentUser(), addressId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region Gutshrift
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateGutshrift(Core.CustomerService.Models.Gutshrift.GutshriftCreateModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.CreateGutshriftHandler(this.GetCurrentUser(), data)
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
		public IActionResult ValidateGutschrift(Core.CustomerService.Models.Gutshrift.RechnungPositionsModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.ValidateGutschriftHandler(this.GetCurrentUser(), data)
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
		public async Task<IActionResult> GetGutshriftReportFile(int typeId, int languageId, int gutshriftId)
		{
			try
			{
				var response = await new Psz.Core.CustomerService.Handlers.Gutshrift.GetGutshriftReportHandler(
						this.GetCurrentUser(), new Psz.Core.CustomerService.Models.Gutshrift.GutshriftReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							GutshriftId = gutshriftId
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
			//try
			//{
			//	var response = new Psz.Core.CustomerService.Handlers.Gutshrift.GetGutshriftReportHandler(
			//			this.GetCurrentUser(),
			//			new Psz.Core.CustomerService.Models.Gutshrift.GutshriftReportRequestModel
			//			{
			//				LanguageId = languageId,
			//				TypeId = typeId,
			//				GutshriftId = gutshriftId
			//			})
			//		.Handle();

			//	if(response.Success)
			//	{
			//		return new FileContentResult(response.Body, "application/pdf")
			//		{
			//			FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
			//		};
			//	}
			//	else
			//	{
			//		return Ok(response);
			//	}
			//} catch(Exception e)
			//{
			//	return this.HandleException(e);
			//}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetRechnungsList(int customerId, string searchText)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.GetKundenRechnungListHandler(this.GetCurrentUser(), customerId, searchText)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Gutshrift.GutschriftItemModel>>), 200)]
		public IActionResult GetGutshriftItems(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.GetGutschriftItems(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Gutshrift.RechnungPositionsModel>), 200)]
		public IActionResult GetRechnungPositions(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.GetRechnungPositionsHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateGutschriftPosition(Core.CustomerService.Models.Gutshrift.UpdateGutschriftPositionRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Gutshrift.UpdateGutschriftQuantityHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(List<KeyValuePair<int, int>>), 200)]
		public IActionResult GetGutschriftListFromRechnung(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Gutshrift.GetGutschriftListFromRechnungHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteGutschriftPosition(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Gutshrift.DeleteGutschriftPositionHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteGutschrift(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Gutshrift.DeleteGutschriftHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ApplyRahmenToABPosition(Core.CustomerService.Models.OrderProcessing.ApplayRahmenToABPosEntryModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.ApplyRahmenToABPostionsHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region Forcast
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CreateForcast(Core.CustomerService.Models.OrderProcessing.CreateOrderModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Forcast.CreateForcastHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region E_Rechnung
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddE_RechnungTypedCustomer(Core.CustomerService.Models.E_Rechnung.E_RechnungUntypedCustomerAddModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.E_Rechnung.AddTypedCustomerHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Core.CustomerService.Models.E_Rechnung.ERechnungConfigModel>), 200)]
		public IActionResult GetE_RechnungConfigList()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetE_RechnungConfigHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateE_RechnungConfig(Core.CustomerService.Models.E_Rechnung.E_RechnungConfigRequestModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.E_Rechnung.UpdateE_RechnungConfigHandler(this.GetCurrentUser(), data)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.E_Rechnung.E_RechnungTypedCustomerModel>>), 200)]
		public IActionResult DeleteE_RechnungConfig(int Id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.DeleteE_RechnungConfigHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.E_Rechnung.E_RechnungTypedCustomerModel>>), 200)]
		public IActionResult GetE_RechnungConfigItem(int Id)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetE_RechnungConfigSinglePositionHandler(this.GetCurrentUser(), Id).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetKundenList()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetKundenListE_RechnungConfigHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetKundenListForSeach()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetKundenListForSeachHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetRechnungTypes()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetRechnungTypesHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.OrderProcessing.CustomerModel>>), 200)]
		public IActionResult GetCustomersList(int type)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetKundenListForCreationHandler(this.GetCurrentUser(), type).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public async Task<IActionResult> ForceRechnungAutoCreation()
		{
			try
			{
				var response = await new Core.CustomerService.Handlers.E_Rechnung.ForceRechnungAutoCreationHandler(this.GetCurrentUser())
				   .HandleAsync();

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
		public IActionResult CreateManualRechnung(Core.CustomerService.Models.OrderProcessing.CreateOrderModel data)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.E_Rechnung.CreateRechnungManualHandler(this.GetCurrentUser(), data).Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult CheckBeforeMailSend(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.CheckBeforteMailSendHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult SendRechnungEmail(int Nr)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.E_Rechnung.SendRechnungEmailHandler(
						this.GetCurrentUser(), Nr).Handle();

				if(response.Success)
				{
					return new FileContentResult(response.Body, "application/vnd.ms-outlook")
					{
						FileDownloadName = $"REMail-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.eml"
					};
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>), 200)]
		public async Task<IActionResult> SendRechnungEmailDirectAsync(int Nr)
		{
			try
			{
				var result = await new Psz.Core.CustomerService.Handlers.E_Rechnung.SendRechnungEmailSubHandler(this.GetCurrentUser(), Nr)
					.HandleAsync();
				return Ok(result);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetRechnungReportFile(int typeId, int languageId, int rechnungId)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.E_Rechnung.GetRechnungReportHandler(
						this.GetCurrentUser(),
						new Psz.Core.CustomerService.Models.E_Rechnung.RechnungReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							RechnungId = rechnungId
						})
					.Handle();

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
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadRechnungPDF(int typeId, int languageId, int rechnungId)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.E_Rechnung.GetRechnungReportHandler(
						this.GetCurrentUser(),
						new Psz.Core.CustomerService.Models.E_Rechnung.RechnungReportRequestModel
						{
							LanguageId = languageId,
							TypeId = typeId,
							RechnungId = rechnungId
						})
					.Handle();
				if(response.Success)
				{
					return new FileContentResult(response.Body, "application/pdf")
					{
						FileDownloadName = $"Rechnung_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
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
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadRechnungPDFs(List<RechnungReportRequestModel> data)
		{
			try
			{
				var response = new Psz.Core.CustomerService.Handlers.E_Rechnung.GetRechnungReportsHandler(
						this.GetCurrentUser(), data).Handle();
				if(response.Success)
				{
					return new FileContentResult(response.Body, "application/pdf")
					{
						FileDownloadName = $"Rechnung_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
					};
				}
				else
				{
					return Ok(response);
				}

			} catch(System.Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult DeleteRechnung(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.DeleteRechnungHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.CTSLoggingModel>>), 200)]
		public IActionResult GetLogs(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Logging.BlanketLoggingsHandler(this.GetCurrentUser(), Nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Gutshrift.RechnungPositionsModel>), 200)]
		public IActionResult GetRechnungEmailParameters(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.E_Rechnung.EmailPresendhandler(this.GetCurrentUser(), Nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.E_RechnungCreatedResponseModel>), 200)]
		public IActionResult GetE_RechnungCreated()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetERechnungCreatedHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.CheckForceCreationAllowdModel>), 200)]
		public IActionResult GetForceCreationAllowed()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.CheckForceCreationAllowdHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult ToogleRechnungValidated(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.ToogleRechnungValidatedHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateE_RechnungTypedCustomer(Psz.Core.CustomerService.Models.E_Rechnung.E_RechnungTypedCustomerModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.UpdateTypedCustomerHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult CheckBeforRechnungForceCreation()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.CheckForceRechnungTimeHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.E_Rechnung.ERechnungReprintModel>>), 200)]
		public IActionResult GetE_RechnungHistory()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.E_Rechnung.GetERechnungHistoryHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<DeliveryNoteResponseModel>), 200)]
		public IActionResult GetDeliveryNotes(DeliveryNoteRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.OrderProcessing.GetDeliveryNotesHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
		#region >>>> Orders EMail Management <<<<
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(ResponseModel<GetPSZ_SendGrid_Email_Not_DeliveredByUserResponseModel>), 200)]
		public IActionResult GetPSZ_SendGrid_Email_Not_DeliveredByUser(GetPSZ_SendGrid_Email_Not_DeliveredByUserModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.SendGridEmailsManagement.GetPSZ_SendGrid_Email_Not_DeliveredByUserHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		#endregion
	}
}