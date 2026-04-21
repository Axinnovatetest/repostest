using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	public class PackagingController: Controller
	{
		private const string MODULE = "Logistics | Packaging";

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Core.Logistics.Models.Verpackung.PackingModel>>), 200)]
		public IActionResult GetListPacking()
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListPackingHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateGedrucktPacking(long idArtikelAngebote)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateGedrucktVerpackung(idArtikelAngebote, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, idArtikelAngebote);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Verpackung.PackingGlobalChooseModel>), 200)]
		public IActionResult GetListChoosePacking(DateTime? Datum, string Kunde, string Verpackungsart)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListChoosePackingHandler(Datum, Kunde, Verpackungsart, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { Datum, Kunde, Verpackungsart });
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<DateTime?>>), 200)]
		public IActionResult GetDateChoosePacking()
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListeDatePackingHandler(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateGedruktPackstatus(Psz.Core.Logistics.Models.Verpackung.ListeUpdatePacking data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Verpackung.UpdateGedruktPackstatusHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetListeVerpackungReport1(Psz.Core.Logistics.Models.Verpackung.ListeUpdatePacking data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Verpackung.PDFReports.GetChoosePacking1Handler(this.GetCurrentUser(), data)
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
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetListeVerpackungReport2(Psz.Core.Logistics.Models.Verpackung.ListeUpdatePacking data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Verpackung.PDFReports.GetChoosePacking2Handler(this.GetCurrentUser(), data)
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Verpackung.ListeUpdatePacking>), 200)]
		public IActionResult GetListLieferscheinDrucken()
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListLSGedruckt(this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLSDrucken(int LS, int languageId, int orderTypeId)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetLSDruckHandler(this.GetCurrentUser(), LS, languageId, orderTypeId)
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, new
				{
					LS,
					languageId,
					orderTypeId
				});
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Verpackung.HistoriePackingResponseModel>), 200)]
		public IActionResult GetPaginationListHistorieVerpackung(Psz.Core.Logistics.Models.Verpackung.HistoriePackingSearchModel data)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetPaginationListHistoriePackingHandler(data, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateGedruktLS(Psz.Core.Logistics.Models.Verpackung.ListeUpdatePacking data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Verpackung.UpdateGedruktLSHandler(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Core.Models.ResponseModel<List<DateTime?>>), 200)]
		public IActionResult GetListeKurtzKundenVDA(int typeVDA)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListKundenKurtzVDAHandler(typeVDA, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, typeVDA);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Verpackung.PackingGlobalChooseModel>), 200)]
		public IActionResult GetListVDAByKunde(string Kunde)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListEtikettenVDAByKundeHandler(Kunde, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, Kunde);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<Core.Logistics.Models.Verpackung.PackingGlobalChooseModel>), 200)]
		public IActionResult GetListVDAByLS(long ls)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListEtikettenVDAByLSHandler(ls, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, ls);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<string, string>>>), 200)]
		public IActionResult GetMitarbeiterLogesticDE()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Verpackung.GetMitarbeitetLogisticDEHandler(this.GetCurrentUser())
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
		public IActionResult UpdateMitarbeiterPacking(long idArtikelAngebote, string mitarbeiter)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateMitarbeiterVerpackung(idArtikelAngebote, mitarbeiter, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { idArtikelAngebote, mitarbeiter });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateZeitPunktPacking(long idArtikelAngebote, DateTime? zeitPunkt)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateZeitPunktVerpackung(idArtikelAngebote, zeitPunkt, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { idArtikelAngebote, zeitPunkt });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateZeitPunktNowPacking(long idArtikelAngebote, bool packstatus)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateZeitPunktNowVerpackungHandler(idArtikelAngebote, packstatus, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { idArtikelAngebote });
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateVersandsartPacking(long idArtikelAngebote, string versandsart)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateVersandsartHandler(idArtikelAngebote, versandsart, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { idArtikelAngebote, versandsart });
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateVersandStatusPacking(long idArtikelAngebote, bool vesandStatus)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.UpdateVersandStatusHandler(idArtikelAngebote, vesandStatus, this.GetCurrentUser()).Handle());
			} catch(Exception e)
			{
				return this.HandleException(e, new { idArtikelAngebote, vesandStatus });
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public IActionResult UpdateGedruktVDA(Psz.Core.Logistics.Models.Verpackung.ListeUpdatePacking data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Verpackung.UpdateGedrucktVDA(data, this.GetCurrentUser())
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
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLSEtikettenDrucken(int LS)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Verpackung.GetListEtikettenVDAByLSDruckerHandler(LS, this.GetCurrentUser())
				  .Handle();


				return new FileContentResult(response.Body, "application/pdf")
				{
					FileDownloadName = $"report-{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf"
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, LS);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetLigneLSEtikettenDrucken(int nr)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListEtikettenVDAByNrAngArtDruckerHandler(nr, this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, nr);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetListeVDADruckenByListNrAngArt(Psz.Core.Logistics.Models.Verpackung.ListeUpdateVDA data)
		{
			try
			{
				return Ok(new Core.Logistics.Handlers.Verpackung.GetListEtikettenVDAByListNrAngArtDruckerHandler(data, this.GetCurrentUser())
				   .Handle());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
	}
}
