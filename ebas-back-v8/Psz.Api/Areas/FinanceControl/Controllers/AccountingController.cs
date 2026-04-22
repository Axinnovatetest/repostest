using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Psz.Core.Common.Models;
using Psz.Core.FinanceControl.Models.Accounting;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;

namespace Psz.Api.Areas.FinanceControl.Controllers;

[Authorize]
[Area("FinanceControl")]
[Route("api/[area]/[controller]/[action]")]
[ApiController]
public class AccountingController: ControllerBase
{
	private const string MODULE = "Finance Control";


	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.LiquiditatsplanungSkontozahlerModel>>), 200)]
	public IActionResult GetLiquiditatsplanungSkontozahler()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.LiquiditatsplanungSkontozahlerAccountingHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetLiquiditatsplanungSkontozahlerXLS()
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.LiquiditatsplanungSkontozahlerAccountingExcelHandler(this.GetCurrentUser()).Handle();

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
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.LiquiditatsplanungOffeneMaterialbestellungenModel>>), 200)]
	public IActionResult GetLiquiditatsplanungOffeneMaterialbestellungen(Psz.Core.FinanceControl.Models.Accounting.LiquiditatsplanungOffeneMaterialbestellungenRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetLiquiditatsplanungOffeneMaterialbestellungenHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetLiquiditatsplanungOffeneMaterialbestellungenXLS(DateTime fromdate, DateTime todate)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.LiquiditatsplanungOffeneMaterialbestellungenExcelHandler(this.GetCurrentUser(), fromdate, todate).Handle();

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
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.GetZahlungskonditionenKundenModel>>), 200)]
	public IActionResult GetZahlungskonditionenKunden()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetZahlungskonditionenKundenHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.GetZahlungskonditionenLieferantenModel>>), 200)]
	public IActionResult GetZahlungskonditionenLieferanten()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetZahlungskonditionenLieferantenHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.GetFactoringRgGutschriftlisteModel>>), 200)]
	public IActionResult GetFactoringRgGutschriftliste(Psz.Core.FinanceControl.Models.Accounting.GetFactoringRgGutschriftlisteRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetFactoringRgGutschriftlisteHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetFactoringRgGutschriftlisteXLS(string typ, DateTime fromdate, DateTime todate)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetFactoringRgGutschriftlisteExcelHandler(this.GetCurrentUser(), typ, fromdate, todate).Handle();

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
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.AusfuhrModel>>), 200)]
	public IActionResult GetAusfuhr(Psz.Core.FinanceControl.Models.Accounting.AusfuhrRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetAusfuhrHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetAusfuhrXLS(Psz.Core.FinanceControl.Models.Accounting.AusfuhrRequestModel data)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetAusfuhrExcelHandler(this.GetCurrentUser(), data).Handle();

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
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.StammdatenkontrolleWareneingangeModel>>), 200)]
	public IActionResult GetStammdatenkontrolleWareneingange(Psz.Core.FinanceControl.Models.Accounting.StammdatenkontrolleWareneingangeRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.StammdatenkontrolleWareneingangeHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.RMDCZModel>>), 200)]
	public IActionResult GetRMDCZ(Psz.Core.FinanceControl.Models.Accounting.RMDCZRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetRMDCZHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetRMDCZXLS(Psz.Core.FinanceControl.Models.Accounting.RMDCZRequestModel data)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetRMDCZExcelHandler(this.GetCurrentUser(), data).Handle();

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
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.EinfuhrModel>>), 200)]
	public IActionResult GetEinfuhr(Psz.Core.FinanceControl.Models.Accounting.EinfuhrRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetEinfuhrHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetEinfuhrXLS(Psz.Core.FinanceControl.Models.Accounting.EinfuhrRequestModel data)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetEinfuhrExcelHandler(this.GetCurrentUser(), data).Handle();

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
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.LieferantenGruppeModel>>), 200)]
	public IActionResult GetLieferantenGruppe()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.LieferantenGruppeHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.RechnungstransferModel>>), 200)]
	public IActionResult GetRechnungstransfer()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.RechnungstransferHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.RechnungstransferModel>>), 200)]
	public IActionResult GetRechnungstransferHistory(Psz.Core.FinanceControl.Models.Accounting.RechnungstransferHistoryRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.RechnungstransferHistoryHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e, data);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetRechnungstransferLastXLS()
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetRechnungstransferLastExcelHandler(this.GetCurrentUser()).Handle();

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

	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult GetRechnungstransferHistoryXLS(Psz.Core.FinanceControl.Models.Accounting.RechnungstransferHistoryRequestModel data)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.GetRechnungstransferExcelHandler(this.GetCurrentUser(), data).Handle();

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
			return this.HandleException(e, data);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult Create_PSZ_BH_Kontenrahmen(Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_CreateModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.PSZ_BH_Kontenrahmen_CreateHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult Delete_PSZ_BH_Kontenrahmen(int id)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.PSZ_BH_Kontenrahmen_RemoveHandler(this.GetCurrentUser(), id).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult Update_PSZ_BH_Kontenrahmen(Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_CreateModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.PSZ_BH_Kontenrahmen_UpdateHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_Model>>), 200)]
	public IActionResult Get_PSZ_BH_Kontenrahmen()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.PSZ_BH_Kontenrahmen_GetHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogModel>>), 200)]
	public IActionResult GetPSZ_BH_Kontenrahmen_Log(Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.PSZ_BH_Kontenrahmen_LogHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<byte[]>), 200)]
	public IActionResult StammdatenkontrolleWareneingangeXLS(System.DateTime fromdate, System.DateTime todate, string gruppe)
	{
		try
		{
			var response = new Core.FinanceControl.Handlers.Accounting.StammdatenkontrolleWareneingangeExcelHandler(this.GetCurrentUser(), fromdate, todate, gruppe).Handle();

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
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.WarengruppenModel>>), 200)]
	public IActionResult GetAllWarengruppen()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetAllWarengruppenHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.Fibu_kunden_rahmenModel>>), 200)]
	public IActionResult GetFibu_kunden_rahmen()
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.Fibu_kunden_rahmenHandler(this.GetCurrentUser()).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	/*[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult Update_ZahlungskonditionenLieferanten(Psz.Core.FinanceControl.Models.Accounting.ZahlungskonditionenLieferantenUpdateModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.ZahlungskonditionenLieferantenUpdateHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}*/
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogModel>>), 200)]
	public IActionResult GetAdressen_Log(Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetAdressen_LogHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	/*[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult Update_ZahlungskonditionenKunden(Psz.Core.FinanceControl.Models.Accounting.ZahlungskonditionenKundenUpdateModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.ZahlungskonditionenKundenUpdateHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}*/
	[HttpPost]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogModel>>), 200)]
	public IActionResult GetZahlungskonditionenKunden_Log(Psz.Core.FinanceControl.Models.Accounting.PSZ_BH_Kontenrahmen_LogRequestModel data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.GetZahlungskonditionenKunden_FNC_LogHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<int>>), 200)]
	public IActionResult KundenNummerAutoComplete(string data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.AutoCompletekundennummerHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<List<int>>), 200)]
	public IActionResult LieferantenNummerAutoComplete(string data)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.AutoCompleteLieferantennummerHandler(this.GetCurrentUser(), data).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}
	/*[HttpGet]
	[SwaggerOperation(Tags = new[] { MODULE })]
	[ProducesResponseType(typeof(Core.Models.ResponseModel<int>), 200)]
	public IActionResult AdressenDeleteById(int id)
	{
		try
		{
			return Ok(new Core.FinanceControl.Handlers.Accounting.AdressenDeleteByIdHandler(this.GetCurrentUser(), id).Handle());
		} catch(Exception e)
		{
			return this.HandleException(e);
		}
	}*/
}
