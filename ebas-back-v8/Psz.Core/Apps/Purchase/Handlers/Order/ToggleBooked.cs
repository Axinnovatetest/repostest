using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> ToggleBooked(int data,
			Core.Identity.Models.UserModel user)
		{
			if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
			{
				throw new Core.Exceptions.UnauthorizedException();
			}

			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data);
					var orderDbItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb?.Nr ?? -1);
					var LSArticlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderDbItems?.Select(a => a.ArtikelNr ?? -1).ToList() ?? new List<int> { -1 });

					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					// - 2022-07-04 - block changes for LS w/ Rechnung
					if(/*orderDb.Erledigt == true &&*/ orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY)
					{
						var invoiceEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoiceByLieferschein(orderDb.Nr);
						if(invoiceEntities != null && invoiceEntities.Count > 0)
						{
							return Core.Models.ResponseModel<object>.FailureResponse(new List<string> { "ACHTUNG: Lieferschein ist erledigt.", "Buchung rückgängig nicht möglich!", "Rechnung stornieren notwendig?" });
						}
					}

					// - 2023-05-10 - Heidenreich - allow only ONCE to toggle Book for RG
					if(orderDb.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
					{
						if(orderDb.Datum < DateTime.Today || orderDb.Gebucht == true)
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Invoice edit is not allowed");
						}
					}

					var _oldvalue = orderDb.Gebucht;
					orderDb.Gebucht = orderDb.Gebucht.HasValue ? !orderDb.Gebucht.Value : true;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
					if(orderDb.Typ == "Lieferschein" && (!_oldvalue.HasValue || (_oldvalue.HasValue && !_oldvalue.Value)))
						generateDATFile(orderDb, orderDbItems, LSArticlesEntities);
					//logging
					var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user)
						.LogCTS("Gebucht", (!orderDb.Gebucht).ToString(), orderDb.Gebucht.ToString(), 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static void generateDATFile(
		   Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
		   List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
		   List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
			var addressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angeboteEntity.Kunden_Nr.Value);
			var WmsAngebotNr = angeboteEntity.Angebot_Nr;
			var path = $@"{Program.CTS.DeliveryNoteFilesPath}\WA{DateTime.Now.ToString("yyyyMMddhhmmss")}.dat";
			var content = $"AG;1;1;{WmsAngebotNr};{angeboteneArtikelEntities?.Count};50;{angeboteEntity.Datum.Value.ToString("yyyyMMdd")};{angeboteEntity.Versanddatum_Auswahl?.ToString("yyyyMMdd")};1;0;0;1;1;{addressenEntity.Kundennummer.Value};{angeboteEntity.Vorname_NameFirma.Substring(0, Math.Min(angeboteEntity.Vorname_NameFirma.Length, 37))}";
			foreach(var angeboteneArtikelEntity in angeboteneArtikelEntities)
			{
				var artikelEntity = artikelEntities.Where(x => x.ArtikelNr == angeboteneArtikelEntity.ArtikelNr)?.ToList().FirstOrDefault();

				if(artikelEntity != null && (artikelEntity.Warengruppe.ToUpper() == "EF" || artikelEntity.Warengruppe.ToUpper() == "ROH"))
				{
					content += $"\nAG;2;1;{WmsAngebotNr};{angeboteneArtikelEntity.Position};{artikelEntity.ArtikelNr};{angeboteneArtikelEntity.Anzahl}";
				}
			}

			File.WriteAllText(path, content);
		}
	}
}
