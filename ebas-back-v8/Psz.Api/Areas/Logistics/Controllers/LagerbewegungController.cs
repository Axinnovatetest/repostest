using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Logistics.Models.Lagebewegung;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Api.Areas.Logistics.Controllers
{
	[Authorize]
	[Area("Logistics")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class LagerbewegungController: Controller
	{
		private const string MODULE = "Logistics | Lagerbewegung";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungCompletModel>), 200)]
		public IActionResult SelectLagerbewegungByID(long idLagerbewegung)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetLagerbewegungByIdHandler(this.GetCurrentUser(), idLagerbewegung)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungCompletModel>), 200)]
		public IActionResult SelectLagerbewegungMaxMin(string type, int order)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetMinMaxLagerBCompByTypHandler(this.GetCurrentUser(), type, order)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungCompletModel>), 200)]
		public IActionResult SelectLagerbewegungSuivantePrecedent(string type, int order, long idLagerBewegung)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SelectSuivantPrecedentHandler(this.GetCurrentUser(), type, order, idLagerBewegung)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungHeaderModel>), 200)]
		public IActionResult CreateLagerbewegung(Core.Logistics.Models.Lagebewegung.LagerbewegungHeaderModel data, int lager)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateLagerbewegungHeaderHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungHeaderModel>), 200)]
		public IActionResult CreatelistPositionUmbuchung(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel> data, int lager)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateUmbuchungDetailsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel>), 200)]
		public IActionResult CreatelistPositionEntnahme(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel> data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateEntnahmeDetailsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungHeaderModel>), 200)]
		public IActionResult CreatelistPositionZugang(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel> data, int lager)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateZugangDetailsHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.ListeArtikelLager>), 200)]
		public IActionResult GetListArtikelLager(int lagerID)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetListArtikelLagerHandler(this.GetCurrentUser(), lagerID)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.Fertigung.FASearchResponseModel>>), 200)]
		public IActionResult SearchFALager(Core.Logistics.Models.Lagebewegung.Fertigung.FALagerSearchModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.Fertigung.SearchFALagerHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.ArticleSearchResponseModel>>), 200)]
		public IActionResult SearchArtikel(Core.Logistics.Models.Lagebewegung.ArticleSearchModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SearchArtikelHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel>), 200)]
		public IActionResult CreateUmbuchungComplete(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel> data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateUmbuchungCompletHandler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetDruckerLagerbewegung(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsModel> data)
		{
			//try
			//{
			//	var response = new Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports.GetDruckerLagerbewegungHandler(this.GetCurrentUser(), data)
			//	  .Handle();


			//	return new FileContentResult(response.Body, "application/pdf")
			//	{
			//		FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
			//	};
			//} catch(Exception e)
			//{
			//	Infrastructure.Services.Logging.Logger.Log(e);
			//	return this.HandleException(e);
			//   }
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports.GetDruckerLagerbewegungHandler(this.GetCurrentUser(), data)
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
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.EntnahmeWertTreeModel>>), 200)]
		public IActionResult GetEntnahmeWert(DateTime D1, DateTime D2, int L1, int Type)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.EntnahmeWertHandler(D1, D2, L1, Type, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetEntnahmeWertExcel(DateTime D1, DateTime D2, int L1, int Type, string Artikelnummer)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.EntnahmeWertExcelHandler(D1, D2, L1, Type, Artikelnummer, this.GetCurrentUser()).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"EntnahmeWert-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		public IActionResult GetDruckerEntnahmeWert(DateTime D1, DateTime D2, int L1, int Type, string Artikelnummer)
		{
			//try
			//{
			//	var response = new Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports.GetDruckerEntnahmeWertHandler(this.GetCurrentUser(), D1, D2, L1, Type)
			//	  .Handle();


			//	return new FileContentResult(response.Body, "application/pdf")
			//	{
			//		FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
			//	};
			//} catch(Exception e)
			//{
			//	Infrastructure.Services.Logging.Logger.Log(e);
			//	return this.HandleException(e);
			//}
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Lagerbewegung.PDFReports.GetDruckerEntnahmeWertHandler(this.GetCurrentUser(), D1, D2, L1, Type, Artikelnummer)
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
				return this.HandleException(e, new { D1, D2, L1, Type });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.InterTransferKompletModel>>), 200)]
		public IActionResult GetInternTransfer()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.InternTransferHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.InterTransferKompletModel>>), 200)]
		public IActionResult GetInternTransfer2(Core.Logistics.Models.Lagebewegung.InterTransferSearchModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.InternTransfer2Handler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetTransfer2DetailsExcel(int Type)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.InternTransfer2DetailsExcelHandler(Type, this.GetCurrentUser()).Handle();
				if(results.Success && results.Body.Length > 0)
				{
					return File(results.Body, "application/xlsx", $"InternTransfer-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.EntnahmeWertTreeModel>>), 200)]
		public IActionResult GetEntnahmeWert2(DateTime D1, DateTime D2, int L1, int Type, string Artikelnummer)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.Entnahme2WertHandler(D1, D2, L1, Type, Artikelnummer, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.EntnahmeWertTreeModel>>), 200)]
		public IActionResult GetEntnahmeWert2Details(Psz.Core.Logistics.Models.Lagebewegung.EntnahmeDetailsSearchModel data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.EntnahmeDetailsHandler(data, this.GetCurrentUser())
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);

			}
		}
		// - 2024-02-06 - FormatSoftware
		[HttpPost]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFormatXmlFile(Psz.Core.Logistics.Models.Lagebewegung.FormatXmlFileRequestModel data)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.GetFormatXmlFileHandler(data, this.GetCurrentUser())
					.Handle();
				if(results.Success && results.Body is not null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xml", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xml");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult SendFormatXmlFile(Psz.Core.Logistics.Models.Lagebewegung.FormatXmlFileRequestModel data)
		{

			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SendFormatXmlFileHandler(data, this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFormatXmlFileBySite(Psz.Core.Logistics.Models.Lagebewegung.FormatXmlFileRequestModel data)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.GetFormatXmlFileBySiteHandler(data, this.GetCurrentUser())
					.Handle();
				if(results.Success && results.Body is not null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xml", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xml");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult SendFormatXmlFileBySite(Psz.Core.Logistics.Models.Lagebewegung.FormatXmlFileRequestModel data)
		{

			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SendFormatXmlFileBySiteHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Lagebewegung.FormatRecentTransfer>>), 200)]
		public IActionResult GetFormatRecentTransfers()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetFormatRecentTransfersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetFormatTransferLagers()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetFormatTransferLagersHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<FormatTransferSiteResponseModel>>), 200)]
		public IActionResult GetFormatTransferSites()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetFormatTransferSitesHandler(this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<FormatRecentTransfersBySiteResponseModel>), 200)]
		public IActionResult GetFormatRecentTransfersBySite(int siteId)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetFormatRecentTransfersBySiteHandler(this.GetCurrentUser(), siteId)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);

			}
		}
		[HttpPost]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFormatXmlDayFileBySite(FormatXmlDayFileBySiteRequestModel data)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.GetFormatXmlDayFileBySiteHandler(data, this.GetCurrentUser())
					.Handle();
				if(results.Success && results.Body is not null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xml", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xml");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult SendFormatXmlDayFileBySite(FormatXmlDayFileBySiteRequestModel data)
		{

			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SendFormatXmlDayFileBySiteHandler(data, this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[AllowAnonymous]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFormatXmlMonthFileBySite(FormatXmlMonthFileBySiteRequestModel data)
		{

			try
			{
				var results = new Core.Logistics.Handlers.Lagerbewegung.GetFormatXmlMonthFileBySiteHandler(data, this.GetCurrentUser())
					.Handle();
				if(results.Success && results.Body is not null && results.Body.Length > 0)
				{
					return File(results.Body, "application/xml", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xml");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				this.HandleException(e, data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<bool>), 200)]
		public IActionResult SendFormatXmlMonthFileBySite(FormatXmlMonthFileBySiteRequestModel data)
		{

			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.SendFormatXmlMonthFileBySiteHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.ListLagerArtikelPlantBooking>), 200)]
		public IActionResult GetListArtikelPlantBookingLager(int lagerID)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.GetListArtikelPlantBookingLagerHandler(this.GetCurrentUser(), lagerID)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsPlantBookingModel>), 200)]
		public IActionResult CreateUmbuchungCompletePlantBooking(List<Core.Logistics.Models.Lagebewegung.LagerbewegungDetailsPlantBookingModel> data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Lagerbewegung.CreateUmbuchungPlantBookingHandler(this.GetCurrentUser(), data)
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
					response.Success = false;
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Lagebewegung.LagerbewegungCompletModel>), 200)]
		public IActionResult GetTranferedQuantity(int wereingangId, int Lagernach)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Lagerbewegung.GetTransferedLagerbewegungenArtikelHandler(this.GetCurrentUser(), wereingangId, Lagernach)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}
		}
	}
}