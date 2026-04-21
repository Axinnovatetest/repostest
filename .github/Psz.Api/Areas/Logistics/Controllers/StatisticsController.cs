using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Logistics.Models.Statistics;
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
	public class StatisticsController: Controller
	{
		private const string MODULE = "Logistics | Statistics";
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.MainRequestModelSearch>>), 200)]
		public IActionResult GetBestandlistLager(Core.Logistics.Models.Statistics.BestandlistLagerSearchModel _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetBestandlistLager(this.GetCurrentUser(), _data).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public ActionResult DownloadExcelBestandlistLager(int LagerID)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.MainHandler(this.GetCurrentUser(), LagerID).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Inventurliste komplett {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, LagerID);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.lagerorteModel>>), 200)]
		public IActionResult GetLagerOrt()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.lagerorteHandler(this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.LagerOrt_IdModel>>), 200)]
		public IActionResult GetLagerId()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetLagerIdHandler(this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.GetAuschusskosten_Technik_InfoSearchModel>>), 200)]
		public IActionResult GetAuschusskostenTechnikInfo(Core.Logistics.Models.Statistics.AusschusskostenInfoSearchModel _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetAuschusskosten_Technik_InfoHandler(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.GetAuschusskosten_Technik_InfoSearchModel>>), 200)]
		public IActionResult DownloadExcelAuschusskostenTechnikInfo(Core.Logistics.Models.Statistics.AusschusskostenInfoSearchModel _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelAuschusskostenTechnikInfoHandle(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"Auschusskosten_Technik {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetAuschusskosten_Technik_InfoReportFile(AusschusskostenInfoSearchModel model)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.GetAuschusskosten_Technik_InfoPDFHandler(model, this.GetCurrentUser()).Handle();
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
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, model);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<InventurListRohmaterialSearch>), 200)]
		public IActionResult GetInventurlisteRohmaterial(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.InventurlisteRohmaterialHandler(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public ActionResult DownloadExcelInventurlisteRohmaterial(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelInventurlisteRohmaterialHandler(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Inventurliste Rohmaterial {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public ActionResult DownloadExcelInventurlisteRohmaterial_ProformaRechnung(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelInventurlisteRohmaterialHandler(_data, this.GetCurrentUser()).getProforma();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Inventurliste Rohmaterial {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<InventurlisteEFModelSearch>), 200)]
		public IActionResult GetInventurlisteEF(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetGetInventurlisteEFHandle(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public ActionResult DownloadExceltInventurlisteEFHandle(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExceltInventurlisteEFHandle(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Inventurliste EF {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<InventurlistePetraModelSearch>), 200)]
		public IActionResult GetInventurlistePetra(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.InventurlistePetraHandle(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<byte[]>>), 200)]
		public ActionResult DownloadExceltInventurlistePetra(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelInventurlistePetraHandle(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Inventurliste Bestand CZ {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ProformaModelSearch>), 200)]
		public IActionResult GetProformaList(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetProformaListHandler(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		//DownloadExcelProformaListHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelProformaList(Core.Logistics.Models.Statistics.RohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelProformaListHandler(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Proforma {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");
			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>), 200)]
		public ActionResult GetPSZArtikelubersichtEinAusTaglich(WEnachDatumSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.PSZArtikelubersichtEinAusTaglichHandle(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<DraftInventoryListModelSearch>), 200)]
		public ActionResult GetDraftInventoryList(DraftInventory _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetDraftInventoryListHandle(_data, this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelDraftInventoryList(DraftInventory _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelDraftInventoryHandle(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"DRAFT_INVENTUR - {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpGet, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult BestandSperrlagerPDF()
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.BestandSperrlagerPDFHandle(this.GetCurrentUser()).Handle();
				//return new FileContentResult(response.Body, "application/pdf")
				//{
				//	FileDownloadName = $"Lagerbestand_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
				//};
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
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<PSZArtikelubersichtEinAusTaglichEntityModelSearch>), 200)]
		public ActionResult DownloadExcelPSZArtikelubersichtEinAusTaglich(WEnachDatumSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelPSZArtikelubersichtEinAusTaglichHandle(_data, this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZArtikelubersichtEinAusTaglich {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<BestandSperrlagerListReportModel>>), 200)]
		public IActionResult GetBestandSperrlager()
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.GetBestandSperrlagerHandle(this.GetCurrentUser()).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadExcelBestandSperrlager()
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.DownloadExcelBestandSperrlager(this.GetCurrentUser()).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"BestandSperrlager {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");

			} catch(Exception e)
			{
				return this.HandleException(e);
			}
		}


		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<CableWithoutOrderModel>>), 200)]
		public IActionResult GetCableWithoutOrder(int Do)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.GetCableWithoutOrderHandle(this.GetCurrentUser(), Do).Handle();

				return Ok(response);

			} catch(Exception e)
			{
				return this.HandleException(e, Do);
			}
		}
		//DownloadExcelCableWithoutOrderHandle
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadExcelCableWithoutOrder(int Do)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.DownloadExcelCableWithoutOrderHandle(this.GetCurrentUser(), Do).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_Kabel_Bedarfsanalyse_III {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");

			} catch(Exception e)
			{
				return this.HandleException(e, Do);
			}
		}

		//ROHOhneBedarfHandle
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ROHOhneBedarfModel>), 200)]
		public ActionResult GetROHOhneBedarfHandle(ROHOhneBedarfSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.ROHOhneBedarfHandle(this.GetCurrentUser(), _data).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult DownloadExcelROHOhneBedarf(ROHOhneBedarfSearch _data)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.DownloadExcelROHOhneBedarfHandle(this.GetCurrentUser(), _data).Handle();
				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"ROHOhneBedarf {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");

			} catch(Exception e)
			{
				return this.HandleException(e, _data);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ScannerRohmaterialModel>), 200)]
		public ActionResult GetScannerRohmaterial(ScannerRohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.ScannerRohmaterialHandle(this.GetCurrentUser(), _data).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ScannerRohmaterialModel>), 200)]
		public ActionResult GetScannerRohmaterialPDF(ScannerRohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.ScannerRohmaterialPDFHandle(this.GetCurrentUser(), _data).Handle();
				//return new FileContentResult(response.Body, "application/pdf")
				//{
				//	FileDownloadName = $"ScannerRohmaterial_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
				//};
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
					return Ok("Empty file sent.");
				}


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		#region APIs using transaction
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<int>), 200)]
		public ActionResult DeleteScannerRohmaterial(int IdVersand)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DeleteScannerRohmaterialHandle(this.GetCurrentUser(), IdVersand).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, IdVersand);
				return Ok(e.Message);
			}
		}
		#endregion
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelScannerRohmaterial(ScannerRohmaterialSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelScannerRohmaterialHandle(this.GetCurrentUser(), _data).Handle();

				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"Scanner Rohmaterial {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ExcessRohmaterialSearchListModel>), 200)]
		public ActionResult GetExcessRohmaterial(ExcessRohmaterialSearchModel _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.ExcessRohmaterialHandle(this.GetCurrentUser(), _data).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelExcessRohmaterial(ExcessRohmaterialSearchModel _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelExcessRohmaterialHandle(this.GetCurrentUser(), _data).Handle();

				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"ExcessRohmaterial {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		//LieferantMaterialFertigungFGHandler
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<LieferantMaterialFertigungFGModel>), 200)]
		public ActionResult GetLieferantMaterialFertigungFG()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.LieferantMaterialFertigungFGHandler(this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		//PSZ_PV_ListeHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<PSZ_PV_ListeSearchModel>), 200)]
		public ActionResult GetPSZ_PV_Liste(PVModelSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.PSZ_PV_ListeHandler(this.GetCurrentUser(), _data).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}

		//DownloadExcelPSZ_PV_ListeHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelPSZ_PV_Liste(PVModelSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelPSZ_PV_ListeHandler(this.GetCurrentUser(), _data).Handle();

				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"PSZ_PV_Liste {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		//UmbuchungslisteHandler
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>), 200)]
		public ActionResult GetUmbuchungslisteList(UmbuchungslisteSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.UmbuchungslisteHandler(this.GetCurrentUser(), _data).Handle();

				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		//DownloadExcelUmbuchungslisteSearch
		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelUmbuchungslisteSearch(UmbuchungslisteSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.DownloadExcelUmbuchungslisteHandler(_data, this.GetCurrentUser()).Handle();

				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"Umbuchungsliste {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}


		[HttpPost]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_GenericResult>), 200)]
		public ActionResult GetUmbuchungslisteDetails(UmbuchungslisteSearch _data)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.UmbuchungslisteDetailsHandler(this.GetCurrentUser(), _data).Handle();

				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, _data);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.lagerorteModel>>), 200)]
		public IActionResult GetLagerOrt_appsettings()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetLagerOrt_appsettings_handler(this.GetCurrentUser()).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}

		[HttpGet, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Get_CCMat_ReportPDF(int Lager)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.Get_CCMat_ReportPDF_Handler(this.GetCurrentUser(), Lager).Handle();

				//return new FileContentResult(response.Body, "application/pdf")
				//{
				//	FileDownloadName = $"PSZ_CC_Artikeltabelle_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
				//};
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
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, Lager);
			}
		}
		[HttpGet, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult Get_CCFG_ReportPDF(int Lager)
		{
			try
			{
				var response = new Psz.Core.Logistics.Handlers.Statistics.Get_CCFG_ReportPDF_Handler(this.GetCurrentUser(), Lager).Handle();

				//return new FileContentResult(response.Body, "application/pdf")
				//{
				//	FileDownloadName = $"PSZ_CC_FGTabelle_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf",
				//};
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
					return Ok("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, Lager);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse>>), 200)]
		public ActionResult Reset_de_list_des_Article_CC(int lager, string type)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.Reset_de_list_des_Article_CC_Handler(this.GetCurrentUser(), lager, type).Handle();

				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, "lager : " + lager + " , type : " + type);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.LieferantMitAnzahModel>>), 200)]
		public IActionResult GetListLieferantenMitAnzahlWareneingang(DateTime d1, DateTime d2)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetLieferanrantenMitAnzahlHandler(this.GetCurrentUser(), d1, d2).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, "Date Debut : " + d1 + " , Date Fin : " + d2);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<ListWareneingangLieferantHeadersModel>), 200)]
		public IActionResult GetListWareneingangVonLieferant(DateTime d1, DateTime d2, string name1)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetListWareneingangByLieferantenHandler(this.GetCurrentUser(), d1, d2, name1).Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e, "Date Debut : " + d1 + " , Date Fin : " + d2 + " ,Name1 : " + name1);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public ActionResult DownloadExcelListWareneingangVonLieferant(DateTime d1, DateTime d2, string name1)
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.GetListWareneingangByLieferantenExcelHandler(this.GetCurrentUser(), d1, d2, name1).Handle();

				if(response.Success)
				{
					return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
					, $"WareneingangVon Lieferant {DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
				}
				return Ok("Empty file sent.");


			} catch(Exception e)
			{
				this.HandleException(e, "Date Debut : " + d1 + " , Date Fin : " + d2 + " ,Name1 : " + name1);
				return Ok(e.Message);
			}
		}
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult GetWareneingangLieferantReport(DateTime d1, DateTime d2, string name1)
		{
			//try
			//{
			//	var response = new Psz.Core.Logistics.Handlers.Statistics.GetListWareneingangByLieferantenRapportHandler(this.GetCurrentUser(), d1,  d2,  name1)
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
				var response = new Psz.Core.Logistics.Handlers.Statistics.GetListWareneingangByLieferantenRapportHandler(this.GetCurrentUser(), d1, d2, name1)
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

		#region Article Customs
		[HttpGet]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<List<Core.Logistics.Models.Statistics.ArticleCustomsCheckResponseModels>>), 200)]
		public IActionResult GetAllArticleCustomsCheck()
		{
			try
			{
				var response = new Core.Logistics.Handlers.Statistics.ArticleCustomsCheckGetAllHandler(this.GetCurrentUser())
					.Handle();
				return Ok(response);


			} catch(Exception e)
			{
				this.HandleException(e);
				return Ok(e.Message);
			}
		}
		[HttpPost, DisableRequestSizeLimit]
		[SwaggerOperation(Tags = new[] { MODULE })]
		[ProducesResponseType(typeof(Psz.Core.Common.Models.ResponseModel<byte[]>), 200)]
		public IActionResult AddArticleCustomsCheckFromXLS([FromForm] Infrastructure.Services.Files.Models.FileAttachmentModel data)
		{
			try
			{
				var file = data.AttachedFile;
				if(file == null)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "File to upload is null.");
					return BadRequest("No file sent.");
				}

				if(file.Length > 0)
				{
					using(var fileStream = file.OpenReadStream())
					{
						byte[] bytes = new byte[file.Length];
						fileStream.Read(bytes, 0, (int)file.Length);
						// - 
						var response = new Core.Logistics.Handlers.Statistics.ArticleCustomsCheckAddHandler(this.GetCurrentUser(), bytes)
							.Handle();

						if(response.Success)
						{
							return File(response.Body, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
							, $"ArticleCustoms{DateTime.Now.ToString("yyyy-MM-ddTHHmmss.fff")}.xlsx");
						}
						return Ok("Empty file sent.");
					}
				}
				else
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "Length of file to upload is > 0.");
					return BadRequest("Empty file sent.");
				}
			} catch(Exception e)
			{
				return this.HandleException(e, data);
			}
		}

		#endregion Article Customs
	}
}
