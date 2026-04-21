using System;
using System.Collections.Generic;
using Infrastructure.Data.Entities.Tables.Logistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Logistics.Handlers.ControlProcedure;
using Psz.Core.Logistics.Handlers.ControlProcedure.ArticleControlProcedures;
using Psz.Core.Logistics.Handlers.PlantBookings;
using Psz.Core.Logistics.Interfaces;
using Psz.Core.Logistics.Models.ControlProcedure;
using Psz.Core.Logistics.Models.ControlProcedure.NewFolder;
using Psz.Core.Logistics.Models.PlantBookings;
using Swashbuckle.AspNetCore.Annotations;

namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistics")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class PlantBookingController: ControllerBase
	{
		private const string MODULE = "Logistics | PlantBooking";

		public IPlantBookingService _plantBookingService { get; }

		public PlantBookingController(IPlantBookingService plantBookingService)
		{
			_plantBookingService = plantBookingService;
		}

		// POST: PlantBookingController/Create


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetPlantResponseModel>), 200)]

		public IActionResult CreateTableByLagerID(GetPlantBookingsRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.GetPlantBookingsByLagerStrategyHandler(data, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListOfIdSBookings(int filter)
		{
			try
			{
				return Ok(new GetBookingIdsHandler(filter, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetListTransferOfIdSBookings(int filter, int lagerNach)
		{
			try
			{
				return Ok(new GetBookingIdsTransferHandler(filter,lagerNach, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult GetUserLager()

		{
			try
			{
				var response = _plantBookingService.GetUserLager(this.GetCurrentUser());

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CreatePlantBooking(CreatePlantBookingRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.PostBookingHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CreatePlantBookingWithAllData(CreatePlantBookingRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.PostPlantBookingWithAllDataHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<PlantBookingDetailsResponseModel>), 200)]
		public IActionResult GetPlantBookingDetails(PlantBookingRequestPrintModel data)
		{
			try
			{
				var response = new GetPlantbookingDetailsHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}


		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdatePlantBookingWithPassedData(PlantBookingUpdateModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.UpdatePlantBookingHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeletePlantBookingArtikel(int Id,int LagerId)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.DeletePlantBookingArtikelHandler(this.GetCurrentUser(), Id ,LagerId).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<LogsResponseModel>), 200)]
		public IActionResult GetLogsPlantBookingsData(LogsDataRequestModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.PlantBookings.GetLogsDataHandler(data, this.GetCurrentUser()).Handle();

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
		public IActionResult GetLogsPlantBookingsData_XLS(LogsDataRequestModel data)
		{
			try
			{
				var results = new GetLogsDataHandler(data, this.GetCurrentUser()).GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetPlantBookingsData_XLS(GetPlantBookingsRequestModel data)
		{
			try
			{
				var results = new GetPlantBookingsByLagerStrategyHandler(data, this.GetCurrentUser()).GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<PlantBookingDetailsResponseModel>), 200)]
		public IActionResult CopyWithVerpackungNr(int NummerVerpackung,int lagerId)
		{
			try
			{
				var response = new CopyWithVerpackungNrHandler(NummerVerpackung, lagerId, this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}


		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int,string>>>), 200)]
		public IActionResult GetLagersList()
		{
			try
			{
				var response = new GetLagersListHandler(this.GetCurrentUser()).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
	     }

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<PlantBookingDetailsResponseModel>), 200)]
		public IActionResult SelectSuivPrecPlantBookingsLager(SelectPlantBookingMaxMinModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.GetSuivantPrecedentPlantBookingByLagerHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<PlantBookingDetailsResponseModel>), 200)]
		public IActionResult SelectPlantBookingsLagerMaxMin(SelectPlantBookingMaxMinModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.GetPalntBookingsMinMaxByLagerIDHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.ArticleSearchMHDResponseModel>>), 200)]
		public IActionResult SearchArtikelMhd(Core.Logistics.Models.Lagebewegung.ArticleSearchMhdModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.SearchArtikelByArtikelnummerAndMhdHandler(this.GetCurrentUser(), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult GetPrintedData(PrintedDataRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.GetPlantBookingPrintDataHandler(this.GetCurrentUser(false), data)
				   .Handle();

				if(response.Success)
				{
					var res = new Psz.Core.Common.Models.ResponseModel<string>(Convert.ToBase64String(response.Body));
					res.Success = true;
					return Ok(res);
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult GetSupplierList(SupplierRequestModel data)
		{
			try
			{
				var response = new SupplierListHandler(this.GetCurrentUser(false),data )
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetSupplierData_XLS(SupplierRequestModel data)
		{
			try
			{
				var results = new SupplierListHandler(this.GetCurrentUser(), data).GetSupplierDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetControlProcedureResponseModel>), 200)]
		public IActionResult GetArticeProcedurelList(CreateControlProcedureRequestModel _data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.ControlProcedure.GetAllControlProcedureHandler(this.GetCurrentUser(), _data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}


		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ControlProcedureResponseModel>>), 200)]
		public IActionResult GetArtikleControl(string artNr)
		{
			try
			{
				var response = new GetControlProcedureHandler(this.GetCurrentUser(), artNr).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		//
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetControlProcedureResponseModel>), 200)]
		public IActionResult GetAllArticleControlProcedure(SupplierCAQControlProcedureRequestModel _data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.ControlProcedure.GetAllArticleControlProcedureHandler(this.GetCurrentUser(), _data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}


		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateArticleControlProcedure(UpdateArticleControlProcedureModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.UpdateArticelControlProcedureHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult DeleteArtikelControlProcedure(int Id)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.ControlProcedure.DeleteArticleControlProcedureHandler(this.GetCurrentUser(), Id).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CreateArtikleControlProcedure(CreateControlProcedureVM data)
		{
			try
			{
				
					var response = new Psz.Core.Logistics.Handlers.ControlProcedure.InsertArtikleControlProcedureHandler(this.GetCurrentUser(), data).Handle();
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
		public IActionResult GetCAQ_XLS(CreateControlProcedureRequestModel data)
		{
			try
			{
				var results = new GetAllControlProcedureHandler(this.GetCurrentUser(), data).GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetArtikelByWarengruppe(string artikelNummer)
		{
			try
			{
				return Ok(new GetArtikelByWarrengruppeHandler(this.GetCurrentUser(), artikelNummer).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GeListSupplierNameCAQ(string suppliername)
		{
			try
			{
				return Ok(new GetBySupplierNameCAQHandler(suppliername,this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetProcedureDescription(GetProcedurNameModel data)
		{
			try
			{
				var response = new GetControlProcedureDescriptionHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetProcedureName(GetProcedurNameModel data)
		{
			try
			{
				var response = new GetControlProcedureNameHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<string>), 200)]
		public IActionResult GetArtikelId(String artikelNr)

		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.PlantBookings.GetArtikelIdPlantBookingHandler(this.GetCurrentUser(), artikelNr).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetLogsTicketResponseModel>), 200)]
		public IActionResult GetTicketLogsPlantBookings(LogsTicketRequestModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.PlantBookings.LogsTicketTableHandler(this.GetCurrentUser(),data).Handle();

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
		public IActionResult GetTicketLogsPlantBookings_XLS(LogsTicketRequestModel data)
		{
			try
			{
				var results = new LogsTicketTableHandler(this.GetCurrentUser(),data).GetDataXLS();
				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<TicketCountResponseModel>), 200)]
		public IActionResult GetTicketCountLogsPlantBookings(PostTicketLogsModel data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.PlantBookings.LogsTicketSearchResponseHandler(this.GetCurrentUser(), data).Handle();
				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
		public IActionResult CreatePlantBookingTransferWithAllData(CreatePlantBookingRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.PostPlantBookingTransferWithAllDataHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<ArticleReceivedResponseModel>>), 200)]
		public IActionResult GetArticlesReceived(ArticleReceivedRequestModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.PlantBookings.GetArticlesReceivedHandler(this.GetCurrentUser(), data).Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
	}
}