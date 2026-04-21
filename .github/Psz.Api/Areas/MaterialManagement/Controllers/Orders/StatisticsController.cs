using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Api.Areas.MaterialManagement.Controllers.Orders
{
	[Authorize]
	[Area("MaterialManagement")]
	[Route("api/[area]/[controller]/[action]")]
	[ApiController]
	public class StatisticsController: Controller
	{
		private const string MODULE = "Material Management | Orders";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.GetDispows120ResponseModel>>), 200)]
		public IActionResult Getws120(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetDispows120RequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Dispo120Handler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.MaterialManagement.Orders.Models.Statistics.Dispows120DetailsModel>), 200)]
		public IActionResult Getws120Details(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetDispows120DetailsRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Get120DetailsHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.Dispows120DetailsLieferantenModel>>), 200)]
		public IActionResult Getws120DetailsLieferanten(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetDispows120DetailsLieferantenRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Dispows120DetailsLieferantenHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.Dispows120DetailsBedarfeModel>>), 200)]
		public IActionResult Getws120DetailsBedarfe(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetDispows120DetailsBedarfeRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetDispows120DetailsBedarfHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.GetOpenOrdersPerSupplierResponseModel>>), 200)]
		public IActionResult Getws120DetailsOffnenBestellungen(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetOpenOrdersPerSupplierRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.DispoGetOpenOrdersPerSupplierHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Getws120XLS(int dispo)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.ExportFaultyArticlesXLSHandler(this.GetCurrentUser(false), dispo)
				.Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<GetCurrencySymbolModel>), 200)]
		public IActionResult GetCurrencySymbol(int Number)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetCurrencySymbolHandler(this.GetCurrentUser(false), Number)
				.Handle();

				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Core.MaterialManagement.Orders.Models.Statistics.GetArtikelNummerModel>>), 200)]
		public IActionResult GetFaultyArtikelNummers(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetArtikelNummerRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetArtikelNummersHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#region Fehlermaterial
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetFehlermaterial(FehlmaterialRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.FehlmaterialHandler(this.GetCurrentUser(false), data)
				   .Handle();
				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings,
				});


			} catch(Exception e)
			{

				return this.HandleException(e, data);
			}

		}
		#endregion Fehlermaterial

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AutoCompleteArtikelnummer(string searchtext)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.AutoCompleteArtikelnummerHandler(this.GetCurrentUser(), searchtext).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult AutoCompleteArtikelnummerBedarf(string searchtext)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.AutocompleteArtikelnummerBedarfHandler(this.GetCurrentUser(), searchtext).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetFehlerMaterialFALager()
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.GetFehlermaterialFALagerHandler(this.GetCurrentUser()).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz.Core.MaterialManagement.Orders.Models.Statistics.FehlerMaterialFAResponseModel>>), 200)]
		public IActionResult GetFehlerMaterialFA(Psz.Core.MaterialManagement.Orders.Models.Statistics.FehlerMaterialFARequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.GetFehlerMaterialFAHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<FAVerschiebungResponseModel>), 200)]
		public IActionResult GetFAVerschiebung(FAVerschiebungRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.GetFAVerschiebungHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetFAVerschiebungExcel(FAVerschiebungRequestModel data)
		{
			try
			{
				var results = new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.GetFAVerschiebungHandler(this.GetCurrentUser(), data).GetExcel();
				if(results.Length > 0)
				{
					return File(results, "application/xlsx", $"FA Verschiebung-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}

		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.OffenematbstModel>>), 200)]
		public IActionResult GetOffeneMatBst(Psz.Core.MaterialManagement.Orders.Models.Statistics.OffenematbstRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetOffeneMatBstHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetOffeneMatBstXLS(Psz.Core.MaterialManagement.Orders.Models.Statistics.OffenematbstRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.ExporterOffeneMatBstToXLSHandler(this.GetCurrentUser(false), data)
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.GeschlmatbstModel>>), 200)]
		public IActionResult GetGeschlMatBst(Psz.Core.MaterialManagement.Orders.Models.Statistics.OffenematbstRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetGeschlMatBstHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetGeschlMatBstXLS(Psz.Core.MaterialManagement.Orders.Models.Statistics.OffenematbstRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.ExportGeschlMatBstXLS(this.GetCurrentUser(false), data)
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetUngebuchteMatBstXLS()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetUngebuchteMatBstXLSHandler(this.GetCurrentUser(false))
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBruttoBedarfPDF(BedarfRequestModel model)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetBruttoBedarfHandler(this.GetCurrentUser(), model)
					.GetPDF();
				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings,
				});

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e);
			}

		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<BedarfPDFResponseModel>), 200)]
		public IActionResult GetBruttoBedarf(BedarfRequestModel model)
		{
			try
			{
				return Ok(new Psz.Core.MaterialManagement.Orders.Handlers.Statistics.GetBruttoBedarfHandler(this.GetCurrentUser(), model).Handle());
			} catch(System.Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<HighRunnerArticleResponseModel>), 200)]
		public async Task<IActionResult> GetHighRunner(HighRunnerArticleRequestModel data)
		{
			try
			{
				var response = await new Core.MaterialManagement.Orders.Handlers.Statistics.GetHighRunnerHandler(this.GetCurrentUser(), data)
					.HandleAsync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> GetHighRunner_XLS(HighRunnerArticleRequestModel data)
		{
			try
			{
				var results = await new Core.MaterialManagement.Orders.Handlers.Statistics.GetHighRunnerHandler(this.GetCurrentUser(), data)
					.SaveToExcelFile();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Highrunner-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetBestelleOhneFAReport(string plant)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GenerateBestelleOhneFAReportHandler(this.GetCurrentUser(false), plant)
				   .Handle();

				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings,
				});

			} catch(Exception e)
			{

				return this.HandleException(e, plant);
			}

		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetArtikel_StatistikXLS(int prd)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetArtikel_StatistikHandler(this.GetCurrentUser(false), prd)
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetBestandProWerkohneBedarfXLS(int prd)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetBestandProWerkohneBedarfHandler(this.GetCurrentUser(false), prd)
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<string>), 200)]
		public IActionResult GetBestandProWerkohneBedarfReport(int prd)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetBestandProWerkohneBedarfReportHandler(this.GetCurrentUser(false), prd)
				   .Handle();

				return Ok(new Psz.Core.Common.Models.ResponseModel<string>
				{
					Body = response.Body != null && response.Body.Length > 0 ? Convert.ToBase64String(response.Body) : "",
					Errors = response.Errors.Select(x => new Psz.Core.Common.Models.ResponseModel<string>.ResponseError(x.Value))?.ToList(),
					Infos = response.Infos,
					Success = response.Success,
					Warnings = response.Warnings,
				});

			} catch(Exception e)
			{

				return this.HandleException(e, prd);
			}

		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<SupplierHitListResponseModel>), 200)]
		public async Task<IActionResult> GetSupplierHitList(SupplierHitListRequestModel data)
		{
			try
			{
				var response = await new Core.MaterialManagement.Orders.Handlers.Statistics.GetSupplierHitListHandler(this.GetCurrentUser(), data)
					.HandleAsync();

				return Ok(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		public async Task<IActionResult> GetSupplierHitList_XLS(SupplierHitListRequestModel data)
		{
			try
			{
				var results = await new Core.MaterialManagement.Orders.Handlers.Statistics.GetSupplierHitListHandler(this.GetCurrentUser(), data)
					.SaveToExcelFile();

				if(results != null && results.Length > 0)
				{
					return File(results, "application/xlsx", $"Supplier_HitList-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.GetUngebuchteMatBstModel>>), 200)]
		public IActionResult GetUngebuchteMatBstdata(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetUngebuchteMatBstRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetUngebuchteMatBstdataHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<string>>), 200)]
		public IActionResult GetPlantsAndLagers()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetPlantsAndLagersHandler(this.GetCurrentUser(false))
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<int>>), 200)]
		public IActionResult GetMainLagers()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetMainLagersHandler(this.GetCurrentUser(false))
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Core.Common.Models.IPaginatedResponseModel<Core.MaterialManagement.Orders.Models.Statistics.GetArtikelStatisticsModel>>), 200)]
		public IActionResult GetArtikel_Statistik(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetArtikelStatisticsModelRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetArtikel_StatistikdataHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.GetArtikleNrOrArtikelNummerModel>), 200)]
		public IActionResult GetArtikelNrOrArtikelNummer(Psz.Core.MaterialManagement.Orders.Models.Statistics.GetArtikleNrOrArtikelNummerRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetArtikelNrOrArtikelNummerHandler(this.GetCurrentUser(false), data)
				   .Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.PrioEinkaufResponseModel>), 200)]
		public IActionResult PrioEinkauf(Psz.Core.MaterialManagement.Orders.Models.Statistics.PrioEinkaufRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.PrioEinkaufHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult PrioEinkaufPDF(PrioEinkaufPDFRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.PrioEinkaufPDFHandler(this.GetCurrentUser(false), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<KeyValuePair<int, string>>>), 200)]
		public IActionResult GetHauptLagers()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetHauptLagersHandler(this.GetCurrentUser(false))
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		#region Suppliers - 2023-11-28 
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<KeyValuePair<int, string>>), 200)]
		public IActionResult RefreshSuppliersStatistics()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.RefreshOverviewHandler(this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<List<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierStufeResponseModel>>), 200)]
		public IActionResult GetSuppliersStufe()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetSufeHandler(this.GetCurrentUser())
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierOverviewResponseModel>), 200)]
		public IActionResult GetSuppliersStatistics(int? id)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetOverviewHandler(this.GetCurrentUser(), id)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, id);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierOverviewResponseModel>), 200)]
		public IActionResult GetSuppliersActiveArticles(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierArticleRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetActiveArticlesHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderResponseModel>), 200)]
		public IActionResult GetSuppliersHistoryStatistics(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetHistoryHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderResponseModel>), 200)]
		public IActionResult GetSuppliersSummaryStatistics(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderRequestModel data)
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetSummaryHandler(this.GetCurrentUser(), data)
					.Handle();

				return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetSuppliersActiveArticlesXLS(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierArticleRequestModel data)
		{
			try
			{
				// - 
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetActiveArticlesHandler(this.GetCurrentUser(), data)
					.GetExcelData();

				if(response != null && response.Count() > 10)
				{
					return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"ArticleCustoms{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderResponseModel>), 200)]
		public IActionResult GetSuppliersSummaryStatisticsXLS(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderRequestModel data)
		{
			try
			{
				// - 
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetSummaryHandler(this.GetCurrentUser(), data)
					.GetExcelData();

				if(response != null && response.Count() > 10)
				{
					return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"ArticleCustoms{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Common.Models.ResponseModel<Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderResponseModel>), 200)]
		public IActionResult GetSuppliersHistoryStatisticsXLS(Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderRequestModel data)
		{
			try
			{
				// - 
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers.GetHistoryHandler(this.GetCurrentUser(), data)
					.GetExcelData();

				if(response != null && response.Count() > 10)
				{
					return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"ArticleCustoms{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		#endregion
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetEkForecastXLS()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.GetEkForecastHandler(this.GetCurrentUser(false))
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetStdSupplierViolationXLS()
		{
			try
			{
				var response = new Core.MaterialManagement.Orders.Handlers.Statistics.ExportStdSupplierViolationXLS(this.GetCurrentUser(false))
				   .Handle();

				if(response.Success && response.Body.Length > 0)
				{
					return File(response.Body, "application/xlsx", $"data-{DateTime.Now.ToString("yyyyMMddTHHmm")}.xlsx");
				}
				else
				{
					return Ok("Empty file sent.");
				}

				//return Ok(response);
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
	}
}
