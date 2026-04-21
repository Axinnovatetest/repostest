using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class ValidateGutschriftHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private RechnungPositionsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ValidateGutschriftHandler(Identity.Models.UserModel user, RechnungPositionsModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			lock(Locks.Locks.OrdersLock)
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				botransaction.beginTransaction();
				try
				{
					var gutshriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Nr);
					var rechnungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int)gutshriftEntity.Nr_rec);
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)gutshriftEntity.Kunden_Nr);
					var checkedItems = _data.Items.Where(i => i.Quantity.HasValue && i.Quantity.Value > 0).ToList();
					if(checkedItems != null && checkedItems.Count > 0)
					{
						var Errors = new List<string>();
						var calculatedPositions = CalculateGutschriftPosition(checkedItems, _data.Nr, out Errors);
						if(Errors != null && Errors.Count > 0)
							return ResponseModel<int>.FailureResponse(Errors);
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(calculatedPositions, botransaction.connection, botransaction.transaction);
						//log and stats
						foreach(var item in calculatedPositions)
						{
							//1
							Infrastructure.Data.Access.Tables.CTS.Statistiken_AngeboteAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity
							{
								Adress_Nr = gutshriftEntity.Kunden_Nr,
								Typ = gutshriftEntity.Typ,
								Datum = gutshriftEntity.Datum,
								Personal_Nr = gutshriftEntity.Personal_Nr,
								Artikel_Nr = item.ArtikelNr,
								Anzahl = item.Anzahl,
								Gesamtpreis = (customerDb.Bruttofakturierung.HasValue && customerDb.Bruttofakturierung.Value) ?
								(Math.Truncate(item.Gesamtpreis ?? 0) / (1 + item.USt ?? 0)) : item.Gesamtpreis,
								Angebot_Nr = gutshriftEntity.Angebot_Nr,
								Projekt_Nr = int.TryParse(gutshriftEntity.Projekt_Nr, out var v) ? v : 0,
								USt = item.USt,
								Lagerort_ID = item.Lagerort_id,
								Liefertermin = item.Liefertermin,
								Mandant = gutshriftEntity.Mandant,
							}, botransaction.connection, botransaction.transaction);
							//2
							var MahnwesenID = Infrastructure.Data.Access.Tables.CTS.MahnwesenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity
							{
								Adress_id = gutshriftEntity.Kunden_Nr,
								Projekt_Nr = int.TryParse(gutshriftEntity.Projekt_Nr, out var v2) ? v2 : 0,
								Belegnummer = gutshriftEntity.Angebot_Nr,
								Belegdatum = gutshriftEntity.Datum,
								Belegtyp = gutshriftEntity.Typ,
								Zahlungsfrist = gutshriftEntity.Falligkeit,
								Betrag = calculatedPositions?.Sum(p => p.Gesamtpreis ?? 0 * (1 + p.USt ?? 0)),
								Betrag_FW = 0,
								Mahnstufe = 0,
								Anrede = gutshriftEntity.Anrede,
								Vorname_NameFirma = gutshriftEntity.Vorname_NameFirma,
								Name2 = gutshriftEntity.Name2,
								Name3 = gutshriftEntity.Name3,
								Strasse_Postfach = gutshriftEntity.Straße_Postfach,
								Land_PLZ_Ort = gutshriftEntity.Land_PLZ_Ort,
								Mandant = gutshriftEntity.Mandant,
							}, botransaction.connection, botransaction.transaction);
							//3
							Infrastructure.Data.Access.Tables.CTS.Mahnwesen_ArchivAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity
							{
								Adress_id = gutshriftEntity.Kunden_Nr,
								Projekt_Nr = int.TryParse(gutshriftEntity.Projekt_Nr, out var v3) ? v3 : 0,
								Belegnummer = gutshriftEntity.Angebot_Nr,
								Belegdatum = gutshriftEntity.Datum,
								Zahlungsfrist = gutshriftEntity.Falligkeit,
								Soll_DM = calculatedPositions?.Sum(p => p.Gesamtpreis ?? 0 * (1 + p.USt ?? 0)),
								Haben_DM = 0,
								Datum = gutshriftEntity.Datum,
							}, botransaction.connection, botransaction.transaction);
							//4
							Infrastructure.Data.Access.Tables.CTS.Mahnwesen_ZahlungenAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity
							{
								Mahn_ID = MahnwesenID,
								Datum = gutshriftEntity.Datum,
								Soll_DM = calculatedPositions?.Sum(p => p.Gesamtpreis ?? 0 * (1 + p.USt ?? 0)),
								Soll_FW = 0,
								Haben_DM = 0,
								Haben_FW = 0,
								Text = "Belegbuchung",
								gebucht = true,
							}, botransaction.connection, botransaction.transaction);
						}
					}
					gutshriftEntity.Gebucht = true;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(gutshriftEntity, botransaction.connection, botransaction.transaction);
					if(botransaction.commit())
					{
						//logging
						var InsertedItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data.Nr);
						var _ToLog = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
						foreach(var item in InsertedItems)
						{
							var gutschrift = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int)item.AngebotNr);
							_ToLog.Add(new LogHelper(gutschrift.Nr, (int)gutschrift.Angebot_Nr, int.TryParse(gutschrift.Projekt_Nr, out var val) ? val : 0, gutschrift.Typ, LogHelper.LogType.CREATIONPOS, "CTS", _user)
								.LogCTS(null, null, null, (int)item.Position, item.Nr));
						}
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_ToLog);
						return ResponseModel<int>.SuccessResponse(1);
					}
					else
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
				} catch(Exception e)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var gutshriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Nr);
			if(gutshriftEntity == null)
				return ResponseModel<int>.FailureResponse("Gutshrift not found.");
			var rechnungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int)gutshriftEntity.Nr_rec);
			if(rechnungEntity == null)
				return ResponseModel<int>.FailureResponse("Rechnung not found.");

			if(_data.Items != null && _data.Items.Count > 0)
			{
				var errors = new List<string>();
				//var technicArticles = Module.BSD.TechnicArticleIds;
				errors.AddRange(_data.Items.Where(x => x.Quantity > 0 && x.FixedPrice && x.NewUnitPrice <= 0)?.Select(x => $"Position {x.Position}: invalid fixed price [{x.NewUnitPrice}]")?.ToList());
				foreach(var item in _data.Items)
				{
					var horizonCheck = Helpers.HorizonsHelper.userHasGSPosHorizonRight(item.DeliveryDate ?? new DateTime(1900, 1, 1), _user, out List<string> messages);
					if(!horizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(item.ItemNumber ?? -1))
						errors.AddRange(messages);
				}
				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(errors);
				}
			}
			return ResponseModel<int>.SuccessResponse();
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> CalculateGutschriftPosition(List<GutschriftItemModel> items, int gutschriftNr,
			out List<string> Errors)
		{
			var gsPosEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngeboteNrs(new List<int> { gutschriftNr });
			var currPosNumbers = new List<int>();

			var calculatedItems = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			var rechnungItemEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(items?.Select(x => x.Nr)?.ToList());
			var ArticleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(items?.Select(x => x.ItemNumber ?? -1)?.ToList());
			Errors = new List<string>();
			foreach(var item in items)
			{
				var rechnungItemEntity = rechnungItemEntities?.FirstOrDefault(x => x.Nr == item.Nr); // - Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(item.Nr);
				if(rechnungItemEntity == null)
					Errors.Add("Rechnung Item not found .");
				var availbleQty = CalculateRechnungAvailableQty(item.Nr);
				if(item.Quantity > availbleQty)
					Errors.Add($"Gutschrift requested quantity [{item.Quantity}] is bigger then rechnung available quantity [{availbleQty}] at position [{item.Position}].");
				if(item.Quantity <= 0)
					Errors.Add($"Invalid quantity [{item.Quantity}].");
				var ArticleEntity = ArticleEntities?.FirstOrDefault(x => x.ArtikelNr == item.ItemNumber); // - Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)item.ItemNumber);
																										  //calculating prices
				var discount = 0m;
				var unitPriceBasis = rechnungItemEntity?.Preiseinheit ?? 1;
				var fixedPrice = item.FixedPrice; // - 2023-03-22 rechnungItemEntity.VKFestpreis ?? false;
				var cuWeight = Convert.ToDecimal(ArticleEntity.CuGewicht ?? 0);
				var del = rechnungItemEntity.DEL ?? 0;
				var me1 = 0m;
				var me2 = 0m;
				var me3 = 0m;
				var me4 = 0m;
				var pm1 = 0m;
				var pm2 = 0m;
				var pm3 = 0m;
				var pm4 = 0m;
				var verkaufspreis = 0m;
				var itemPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(ArticleEntity.ArtikelNr);
				if(itemPricingGroupsDb != null)
				{
					me1 = Convert.ToDecimal(itemPricingGroupsDb.ME1 ?? 0m);
					me2 = Convert.ToDecimal(itemPricingGroupsDb.ME2 ?? 0m);
					me3 = Convert.ToDecimal(itemPricingGroupsDb.ME3 ?? 0m);
					me4 = Convert.ToDecimal(itemPricingGroupsDb.ME4 ?? 0m);
					pm1 = Convert.ToDecimal(itemPricingGroupsDb.PM1 ?? 0m);
					pm2 = Convert.ToDecimal(itemPricingGroupsDb.PM2 ?? 0m);
					pm3 = Convert.ToDecimal(itemPricingGroupsDb.PM3 ?? 0m);
					pm4 = Convert.ToDecimal(itemPricingGroupsDb.PM4 ?? 0m);
				}
				verkaufspreis = item.FixedPrice ? item.NewUnitPrice : rechnungItemEntity.VKEinzelpreis ?? 0; // - 2022-08-04 - allow fixed Price // rechnungItemEntity.VKEinzelpreis ?? 0;//Convert.ToDecimal(itemPricingGroupsDb.Verkaufspreis ?? 0m);


				// - 202210-14 - w/o copper option
				var singleCopperSurcharge = item.WithoutCopper == true ? 0 : Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
						del,
						cuWeight);
				var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
					(decimal)item.Quantity,
					singleCopperSurcharge);
				var vkUnitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
					verkaufspreis,
					(decimal)item.Quantity,
					me1,
					me2,
					me3,
					me4,
					pm2,
					pm3,
					pm4);
				var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedPrice,
					unitPriceBasis,
					(decimal)item.Quantity,
					vkUnitPrice,
					verkaufspreis,
					singleCopperSurcharge,
					me1,
					me2,
					me3,
					me4,
					pm2,
					pm3,
					pm4);
				var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
					unitPrice,
					(decimal)item.Quantity,
					discount);
				var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
					vkUnitPrice,
					(decimal)item.Quantity);
				var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight((decimal)item.Quantity,
					cuWeight);

				// - get new Pos number in case same RE Pos has multiple Pos in  GS
				var pos = getPositionNumber(gsPosEntities, rechnungItemEntity.Position ?? 0, currPosNumbers);
				//
				calculatedItems.Add(new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
				{
					Typ = null,
					AngebotNr = gutschriftNr,
					Position = pos, // - rechnungItemEntity.Position,
					Wunschtermin = null,
					Anzahl = item.Quantity,
					Abladestelle = "",
					OriginalAnzahl = item.Quantity,
					Freies_Format_EDI = "",
					EDI_Historie_Nr = null,
					LieferanweisungP_FTXDIN_TEXT = "",
					Bemerkungsfeld1 = rechnungItemEntity.Bemerkungsfeld2,
					Bemerkungsfeld2 = rechnungItemEntity.Bemerkungsfeld1,
					Bezeichnung1 = rechnungItemEntity.Bezeichnung1,
					Bezeichnung2 = rechnungItemEntity.Bezeichnung2,
					Bezeichnung3 = rechnungItemEntity.Bezeichnung3,
					Einheit = rechnungItemEntity.Einheit,
					ArtikelNr = rechnungItemEntity.ArtikelNr,
					Kupferbasis = 150,
					Preiseinheit = Convert.ToDecimal(rechnungItemEntity.Preiseinheit ?? 0),
					DELFixiert = rechnungItemEntity.DELFixiert ?? false,
					DEL = rechnungItemEntity.DEL ?? 0,
					EinzelCuGewicht = Convert.ToDecimal(ArticleEntity.CuGewicht ?? 0),
					VKFestpreis = fixedPrice,
					USt = rechnungItemEntity.USt,
					Einzelkupferzuschlag = singleCopperSurcharge,
					GesamtCuGewicht = totalCuWeight,
					Einzelpreis = unitPrice,
					VKEinzelpreis = vkUnitPrice,
					Gesamtpreis = totalPrice,
					Gesamtkupferzuschlag = totalCopperSurcharge,
					VKGesamtpreis = vKTotalPrice,
					erledigt_pos = false,
					POSTEXT = "",
					Geliefert = 0,
					Rabatt = rechnungItemEntity.Rabatt,
					Index_Kunde = rechnungItemEntity.Index_Kunde,
					Index_Kunde_Datum = rechnungItemEntity.Index_Kunde_Datum,
					Fertigungsnummer = 0,
					Lagerort_id = rechnungItemEntity.Lagerort_id,
					REPoszuGSPos = item.Nr,
					Liefertermin = rechnungItemEntity.Liefertermin,
					GSInternComment = item.InternComment,
					GSExternComment = item.Comment,
					AnfangLagerBestand = rechnungItemEntity.AnfangLagerBestand,
					Preisgruppe = rechnungItemEntity.Preisgruppe,
					Lagerbewegung = rechnungItemEntity.Lagerbewegung,
					Lagerbewegung_rückgängig = rechnungItemEntity.Lagerbewegung_rückgängig,
					Auswahl = rechnungItemEntity.Auswahl,
					Summenberechnung = rechnungItemEntity.Summenberechnung,
					Preis_ausweisen = rechnungItemEntity.Preis_ausweisen,
					Stückliste = rechnungItemEntity.Stückliste,
					Stückliste_drucken = rechnungItemEntity.Stückliste_drucken,
					Langtext_drucken = rechnungItemEntity.Langtext_drucken,
					Seriennummern_drucken = rechnungItemEntity.Seriennummern_drucken,
					LSPoszuKBPos = 0,
					LSPoszuABPos = 0,
					RAPoszuBVPos = 0,
					KBPoszuBVPos = 0,
					ABPoszuBVPos = 0,
					KBPoszuRAPos = 0,
					ABPoszuRAPos = 0,
					Loschen = false,
					InBearbeitung = false,
					Packstatus = false,
					Versandstatus = false,
					LS_von_Versand_gedruckt = false,
					termin_eingehalten = false,
					RP = rechnungItemEntity.RP,
					Bezeichnung2_Kunde = rechnungItemEntity.Bezeichnung2_Kunde,
					EDI_Quantity_Ordered = rechnungItemEntity.EDI_Quantity_Ordered,
					VDA_gedruckt = rechnungItemEntity.VDA_gedruckt,
					EDI_PREIS_KUNDE = rechnungItemEntity.EDI_PREIS_KUNDE,
					EDI_PREISEINHEIT = rechnungItemEntity.EDI_PREISEINHEIT,
					EKPreise_Fix = fixedPrice,
					// - 2022-10-16
					GSWithoutCopper = item.WithoutCopper,
				});
				currPosNumbers.Add(pos);
			}
			return calculatedItems;
		}
		public static decimal CalculateRechnungAvailableQty(int NrRechnungPos)
		{
			var rechnungPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRechnungPos);
			var gsPosRechnungList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRechnungPositions(new List<int> { NrRechnungPos }) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			var rechnungOriginalQty = rechnungPosEntity.Anzahl ?? 0;
			var gutschriftLinksTakenQty = gsPosRechnungList?.Sum(x => x.Anzahl) ?? 0;
			var _avaialable = (rechnungOriginalQty - gutschriftLinksTakenQty);
			return _avaialable;
		}
		internal static int getPositionNumber(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> gsPosEntities, int gsCandidatePosNumber, List<int> gsCurrentPosNumbers)
		{
			gsCurrentPosNumbers = gsCurrentPosNumbers ?? new List<int>();
			gsPosEntities = gsPosEntities ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var allPosNumbers = new List<int>();
			allPosNumbers.AddRange(gsCurrentPosNumbers);
			allPosNumbers.AddRange(gsPosEntities.Select(x => x.Position ?? 0));

			// -
			var idx = allPosNumbers.FindIndex(x => x == gsCandidatePosNumber);
			if(idx < 0)
			{
				return gsCandidatePosNumber;
			}

			// - Get the first Number bigger than Original (RE) Pos and not existing in GS Pos
			while(allPosNumbers.Exists(x => x == gsCandidatePosNumber))
			{
				gsCandidatePosNumber++;
			}

			// - 
			return gsCandidatePosNumber;
		}
	}
}
