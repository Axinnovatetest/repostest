using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.CustomerService.Models.Blanket;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Areas.CustomerService.Controllers
{
	[Authorize]
	[Area("CustomerService")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class BlanketController: ControllerBase
	{
		private const string MODULE = "Customer Service | Blanket";
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketHeaderModel>), 200)]
		public IActionResult GetBlanketOrdersList()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetAllBlanketHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketModel>), 200)]
		public IActionResult GetBlanketOrderHeader(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetBlanketHeaderHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.BlanketItem>>), 200)]
		public IActionResult GetBlanketOrderPositions(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetBlanketOrderPositionsHandler(this.GetCurrentUser(), id)
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
		public IActionResult UpdateBlanket(Core.CustomerService.Models.Blanket.BlanketModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.UpdateBlanketHandler(this.GetCurrentUser(), data)
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
		public IActionResult UpdateBlanketPosition(Core.CustomerService.Models.Blanket.BlanketItem data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.UpdateBlanketPositionHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.OrderProcessing.CreateOrderModel>), 200)]
		public IActionResult CreateBlanket(Core.CustomerService.Models.OrderProcessing.CreateOrderModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.CreateBlanketHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketItem>), 200)]
		public IActionResult CreateBlanketPosition(Core.CustomerService.Models.Blanket.BlanketItem data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.CreateBlanketElementsHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteBlanket(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.DeleteBlanketHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteBlanketPosition(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.DeleteBlanketPositionHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetWharung()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetWahrungHandler(this.GetCurrentUser())
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
		public IActionResult UpdateBlanketFile([FromForm] Models.BlanketFileModel data)
		{
			try
			{
				var model = data?.ToBusinessModel();
				return Ok(new Core.CustomerService.Handlers.Blanket.UpdateFileBlanketHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketSearchItemsModel>), 200)]
		public IActionResult SearchItemsBlanket(BlanketSearchItemRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.BlanketSearchItemsHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketSearchItemsModel>), 200)]
		public IActionResult SearchItemsBlanket(string ArtikelNummr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.BlanketSearchItemsHandler(this.GetCurrentUser(), new BlanketSearchItemRequestModel
					{
						ArticleId = 0,
						ArticleNumber = ArtikelNummr,
						SupplierAddressId = 0
					})
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, ArtikelNummr);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.BlanketItem>), 200)]
		public IActionResult GetBlanketSinglePosition(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetBlanketSinglePositionHandler(id, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.AbLinkBlanketModel>>), 200)]
		public IActionResult GetAbLinkBlanket(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetABLinkBlanketHandler(this.GetCurrentUser(), Nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.PosRahmenABModel>), 200)]
		public IActionResult GetPosABLinkBlanket(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetPosABLinkBlanketHandler(this.GetCurrentUser(), Nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetBlanketTypes()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetBlanketTypesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteFile(int FileId)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.DeleteFileHandler(this.GetCurrentUser(), FileId)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.CTSLoggingModel>>), 200)]
		public IActionResult GetBlanketDetailsLogging(int Blanket)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Logging.BlanketLoggingsHandler(this.GetCurrentUser(), Blanket)
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
		public IActionResult AddOrderFromBlanket(Psz.Core.CustomerService.Models.Blanket.AddABFromRahmenModel model)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.AddABFromRahmenHandler(this.GetCurrentUser(), model)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddOrderPositionsFromBlanket(Psz.Core.CustomerService.Models.Blanket.AddABFromRahmenModel model)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.AddABPositionsFromBlanketHandler(this.GetCurrentUser(), model)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.BlanketPositionsMinimalModel>>), 200)]
		public IActionResult GetBlanketPositionsForABCreate(int Blanket)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetBlanketPositionsForABCreateHandler(this.GetCurrentUser(), Blanket)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.BlanketPositionsMinimalModel>>), 200)]
		public IActionResult ToogleBlanketPositionDone(int id)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.TooglePosDoneHandler(this.GetCurrentUser(), id)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.CustomerService.Models.Blanket.AdressenListModel>>), 200)]
		public IActionResult GetSuppliersList()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetSuppliersListHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.LinkToABPositionModel>), 200)]
		public IActionResult GetLinkToAbPos(Psz.Core.CustomerService.Models.Blanket.BlanketAbRequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetRahmenLinkToAbPosition(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.UpdateRAStatusModel>), 200)]
		public IActionResult UpdateRaStatus(Psz.Core.CustomerService.Models.Blanket.UpdateRAStatusModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.UpdateRAStatusHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.LogPositionResponseModel>>), 200)]
		public IActionResult GetBlanketPositionLog(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetLogPositionHandler(this.GetCurrentUser(), Nr)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.AbPosBeforPreisUpdateModel>>), 200)]
		public IActionResult GetABPosBeforPriceUpdate(Psz.Core.CustomerService.Models.Blanket.ABPosBeforePriceUpdaterequestModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetAbPosBeforUpdatePreisHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.PositionsColorsResponseModel>), 200)]
		public IActionResult GetRaPosLinkedAbWidthLs()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetPositionsColorsStatsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.PositionsByColorsModel>), 200)]
		public IActionResult GetRaPositionsWithColors()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetPositionsByColorsHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.RAPositionsColorsResponseModel>), 200)]
		public IActionResult SearchInRahmenPositionsColors(Psz.Core.CustomerService.Models.Statistics.RAPositionsColorsSearchModel data)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.SearchInRAPositionsColorsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Statistics.RAPositionsColorsResponseModel>), 200)]
		public IActionResult GetRahmenDashboardData(int typeId)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetDashboardDataHandler(this.GetCurrentUser(), typeId)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public async Task<IActionResult> GetPurchaseRahmenReportFile(int orderId)
		{
			try
			{
				var response = await new Psz.Core.CustomerService.Handlers.Blanket.GetPurchaseBlanketReportHandler(
						this.GetCurrentUser(), orderId).HandleAsync();

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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult AddBestellungenFromBlanket(Psz.Core.CustomerService.Models.Blanket.AddABFromRahmenModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Blanket.AddBestellungenFromBlanketHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.BestellungenLinkBlanketModel>>), 200)]
		public IActionResult GetBestellungenLinkBlanket(int Nr)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Blanket.GetBestellungenLinkBlanketHandler(this.GetCurrentUser(), Nr).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<Psz.Core.CustomerService.Models.Blanket.PosRahmenABModel>), 200)]
		public IActionResult GetPosBestellLinkBlanket(int Nr)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetPosBestellLinkBlanketHandler(this.GetCurrentUser(), Nr)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, Nr);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AutocompleteArtikelnummer(Psz.Core.CustomerService.Models.Blanket.AutoCompleteArtikelModel model)
		{
			try
			{
				return Ok(new Core.CustomerService.Handlers.Blanket.AutoCompleteArtikelnummerHandler(this.GetCurrentUser(), model).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetLagerortForBlanketPurchase()
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.GetLagerortForBlanketPurchaseHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.RahmensToConvertModel>>), 200)]
		public IActionResult GetRahmensToConvert()
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Blanket.GetRahmensToConvertHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.CustomerService.Models.Blanket.ConvertedRahmensModel>>), 200)]
		public IActionResult ConvertArticleRahmen(Psz.Core.CustomerService.Models.Blanket.RahmensToConvertModel model)
		{
			try
			{
				return Ok(new Psz.Core.CustomerService.Handlers.Blanket.ConvertArticleRahmenHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult ExportConvertedRahmensToExcel(List<Psz.Core.CustomerService.Models.Blanket.ConvertedRahmensModel> model)
		{
			try
			{
				var data = new Psz.Core.CustomerService.Handlers.Blanket.ExportConvertedRahmensToExcelHandler(this.GetCurrentUser(), model).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"ConvertedRahmens-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public IActionResult ExportRahmens(bool? ignoreClosed)
		{
			try
			{
				var data = new Psz.Core.CustomerService.Handlers.Blanket.ExportBlanketsHandler(this.GetCurrentUser(), ignoreClosed).Handle();
				if(data.Body != null)
				{
					return File(data.Body, "application/xlsx", $"ConvertedRahmens-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateRahmenPositionComment(Psz.Core.CustomerService.Models.Blanket.UpdateRahmenPositionCommentRequestModel model)
		{
			try
			{
				var response = new Core.CustomerService.Handlers.Blanket.UpdateRahmenPositionCommentHandler(this.GetCurrentUser(), model)
				   .Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, model);
			}
		}
	}
}