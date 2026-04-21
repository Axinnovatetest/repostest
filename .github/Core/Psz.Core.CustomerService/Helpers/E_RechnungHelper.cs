using Infrastructure.Services.Utils;
using Psz.Core.CustomerService.Models.E_Rechnung;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Helpers
{
	public class E_RechnungHelper
	{
		public static List<E_RechnungAutoCreationModel> CreateManualRechnung(Enums.E_RechnungEnums.RechnungTyp type, int kundenNr, Identity.Models.UserModel user, out List<string> Errors, out int count, bool IsMultiple = false)
		{
			var result = new List<E_RechnungAutoCreationModel>();
			var insertedIds = new List<int>();
			var insertedAngebotNrs = new List<int>();
			Errors = new List<string> { };
			count = 0;
			var _blockState = Core.Common.Helpers.blockHelper.GetBlockState();
			if(_blockState.RG && !IsMultiple)
			{
				Errors.Add("Another Bill(s) is/are in creation, please try again later.");
				return null;
			}
			try
			{
				var transaction = new TransactionsManager();
				transaction.beginTransaction();

				//blocking creation
				if(!IsMultiple)
					Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.RG, true);

				var deliveryNoteForInvoices = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetOpenDeliveriesForRechnung(kundenNr);

				if(deliveryNoteForInvoices is not null && deliveryNoteForInvoices.Count > 0)
					count = deliveryNoteForInvoices.Count;

				int nextAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE, (int)Core.Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.RE);
				//GetVorfallNrFromRange(Enums.OrderEnums.Types.Invoice, user.Username);
				if(deliveryNoteForInvoices == null || deliveryNoteForInvoices.Count == 0)
				{
					Errors.Add($"Customer [{kundenNr}] have no open deliveries.");
					return null;
				}
				//- DN
				if(deliveryNoteForInvoices != null && deliveryNoteForInvoices.Count > 0)
					result = CreateRechnungInternal(kundenNr, transaction, deliveryNoteForInvoices, nextAngebotNr, type, Errors);


				if(transaction.commit())
				{
					if(!IsMultiple)
						Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.RG, false);
					var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
					//Logging
					result.ForEach(x => _logs.Add(new LogHelper(x.Nr, (int)x.ForfallNr,
						x.ProjectNr, "Rechnung", LogHelper.LogType.CREATIONOBJECT, "CTS", user)
						.LogCTS(null, null, null, 0)));
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);


					return result;
				}
				else
				{
					if(!IsMultiple)
						Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.RG, false);
					transaction.rollback();
					Errors.Add("Error in transaction.");
					return null;
				}
			} catch(Exception e)
			{
				if(!IsMultiple)
					Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.RG, false);
				Infrastructure.Services.Logging.Logger.Log(e);
				Errors.Add(e.Message);
				return null;
			}

		}
		private static List<E_RechnungAutoCreationModel> CreateRechnungInternal(int kundenNr, TransactionsManager transaction, List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> deliveryNoteForInvoices,
			int nextAngebotNr, Enums.E_RechnungEnums.RechnungTyp type, List<string> errors)
		{
			errors = errors ?? new List<string>();

			var angebotInsertionList = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			var angebotArtikelInsertionList = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			var deliveriesToUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity>();
			var result = new List<E_RechnungAutoCreationModel>();
			// - 

			foreach(var item in deliveryNoteForInvoices)
			{
				try
				{
					var deliveryItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(item.Nr);
					// - DO NOT allow Pos with different VAT
					if(deliveryItems?.Count > 0 && deliveryItems?.TrueForAll(x => x.USt == deliveryItems[0]?.USt) != true)
					{
						errors.Add($"Delivery Note [{item.Angebot_Nr}]: All Positions do not have the same VAT");
						continue;
					}
					// insert in table angebote
					var itemToInsert = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity
					{
						Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Invoice),//Rechnung
						Projekt_Nr = item.Projekt_Nr,
						Datum = DateTime.Now.Date,
						Liefertermin = item.Liefertermin,
						Kunden_Nr = item.Kunden_Nr,
						Nr_lie = item.Nr,
						Debitorennummer = item.Debitorennummer,
						Falligkeit = item.Falligkeit,
						Anrede = item.Anrede,
						Vorname_NameFirma = item.Vorname_NameFirma,
						Name2 = item.Name2,
						Name3 = item.Name3,
						Ansprechpartner = item.Ansprechpartner,
						Abteilung = item.Abteilung,
						Straße_Postfach = item.Straße_Postfach,
						Land_PLZ_Ort = item.Land_PLZ_Ort,
						Briefanrede = item.Briefanrede,
						LAnrede = item.LAnrede,
						LVorname_NameFirma = item.LVorname_NameFirma,
						LName2 = item.LName2,
						LName3 = item.LName3,
						LAnsprechpartner = item.LAnsprechpartner,
						LAbteilung = item.LAbteilung,
						LStraße_Postfach = item.LStraße_Postfach,
						LLand_PLZ_Ort = item.LLand_PLZ_Ort,
						LBriefanrede = item.LBriefanrede,
						Personal_Nr = item.Personal_Nr,
						Versandart = item.Versandart,
						Zahlungsweise = item.Zahlungsweise,
						Konditionen = item.Konditionen,
						Zahlungsziel = item.Zahlungsziel,
						USt_Berechnen = item.USt_Berechnen,
						Bezug = item.Bezug,
						Ihr_Zeichen = item.Ihr_Zeichen,
						Unser_Zeichen = item.Unser_Zeichen,
						Freie_Text = null,
						Gebucht = true,
						Gedruckt = false,
						Auswahl = item.Auswahl,
						Mahnung = item.Mahnung,
						Lieferadresse = item.Lieferadresse,
						Reparatur_nr = item.Reparatur_nr,
						Interessent = item.Interessent,
						Ab_id = item.Ab_id,
						Datueber = item.Datueber,
						Nr_ang = item.Nr_ang,
						Nr_auf = item.Nr_auf,
						Status = "offen",
						Bemerkung = item.Bemerkung,
						Belegkreis = item.Belegkreis,
						Bereich = item.Bereich,
						Wunschtermin = item.Wunschtermin,
						Mandant = item.Mandant,
						Angebot_Nr = nextAngebotNr,
						EDI_Order_Neu = true,
						Freitext = item.Freitext,
						Erledigt = false,
						Nr_BV = 0,
						Nr_RA = 0,
						Nr_Kanban = 0,
						Nr_rec = 0,
						Nr_pro = 0,
						Nr_gut = 0,
						Nr_sto = 0,
						Neu = 1,
						Loschen = false,
						In_Bearbeitung = false,
						Offnen = false,
						Termin_eingehalten = false,
						Neu_Order = true,
					};
					var Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(itemToInsert, transaction.connection, transaction.transaction);
					angebotInsertionList.Add(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(Nr, transaction.connection, transaction.transaction));
					//updating open delivery
					var delivrey = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(item.Nr);
					delivrey.Erledigt = true;
					delivrey.Nr_rec = Nr;
					deliveriesToUpdate.Add(delivrey);
					//insert in table angebote Artikel
					angebotArtikelInsertionList.AddRange(deliveryItems?.Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity
					{
						AngebotNr = Nr,
						ArtikelNr = x.ArtikelNr,
						Bezeichnung1 = x.Bezeichnung1,
						Bezeichnung2 = x.Bezeichnung2,
						Bezeichnung3 = x.Bezeichnung3,
						Einheit = x.Einheit,
						Anzahl = x.Anzahl,
						Einzelpreis = x.Einzelpreis,
						Gesamtpreis = x.Gesamtpreis,
						Preisgruppe = x.Preisgruppe,
						Bestellnummer = x.Bestellnummer,
						Rabatt = x.Rabatt,
						USt = x.USt,
						Lagerbewegung = x.Lagerbewegung,
						Lagerbewegung_rückgängig = x.Lagerbewegung_rückgängig,
						Auswahl = x.Auswahl,
						FM_Einzelpreis = x.FM_Einzelpreis,
						FM_Gesamtpreis = x.FM_Gesamtpreis,
						FM = x.FM,
						Summenberechnung = x.Summenberechnung,
						zwischensumme = x.zwischensumme,
						POSTEXT = x.POSTEXT,
						schriftart = x.schriftart,
						sortierung = x.sortierung,
						Preiseinheit = x.Preiseinheit,
						Preis_ausweisen = x.Preis_ausweisen,
						Zeichnungsnummer = x.Zeichnungsnummer,
						Liefertermin = x.Liefertermin,
						erledigt_pos = false,
						Stückliste = x.Stückliste,
						Stückliste_drucken = x.Stückliste_drucken,
						Langtext = x.Langtext,
						Langtext_drucken = x.Langtext_drucken,
						Lagerort_id = x.Lagerort_id,
						Seriennummern_drucken = x.Seriennummern_drucken,
						Wunschtermin = x.Wunschtermin,
						Fertigungsnummer = x.Fertigungsnummer,
						Position = x.Position,
						VKFestpreis = x.VKFestpreis,
						Kupferbasis = x.Kupferbasis,
						DEL = x.DEL,
						EinzelCuGewicht = x.EinzelCuGewicht,
						GesamtCuGewicht = x.GesamtCuGewicht,
						Einzelkupferzuschlag = x.Einzelkupferzuschlag,
						Gesamtkupferzuschlag = x.Gesamtkupferzuschlag,
						VKEinzelpreis = x.VKEinzelpreis,
						VKGesamtpreis = x.VKGesamtpreis,
						DELFixiert = x.DELFixiert,
						RP = x.RP,
						OriginalAnzahl = 0,
						Geliefert = 0,
						AktuelleAnzahl = 0,
						EndeLagerBestand = x.EndeLagerBestand,
						AnfangLagerBestand = x.AnfangLagerBestand,
						LSPoszuKBPos = 0,
						LSPoszuABPos = 0,
						RAPoszuBVPos = 0,
						KBPoszuBVPos = 0,
						KBPoszuRAPos = 0,
						ABPoszuBVPos = 0,
						ABPoszuRAPos = 0,
						Loschen = false,
						InBearbeitung = false,
						RA_OriginalAnzahl = 1,
						RA_Abgerufen = 1,
						RA_Offen = 1,
						Packstatus = false,
						Versandstatus = false,
						Versand_gedruckt = false,
						LS_von_Versand_gedruckt = false,
						termin_eingehalten = false,
						VDA_gedruckt = false,
						Index_Kunde = "",
						CSInterneBemerkung = "",

					}).ToList());
					var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(angebotArtikelInsertionList.Select(x => x.ArtikelNr ?? -1).ToList(), transaction.connection, transaction.transaction);
					for(int i = 0; i < angebotArtikelInsertionList.Count; i++)
					{
						var article = articles.FirstOrDefault(x => x.ArtikelNr == angebotArtikelInsertionList[i].ArtikelNr);
						angebotArtikelInsertionList[i].Zuschlag_VK = article.Zuschlag_VK;
					}

					if(type != Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung)
						nextAngebotNr++;
				} catch(Exception e)
				{
					errors.Add($"Invoice Failed to be created for  LS #{item.Angebot_Nr} for the customer {item.Vorname_NameFirma}");
				}
			}

			Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(angebotArtikelInsertionList, transaction.connection, transaction.transaction);
			Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveriesToUpdate, transaction.connection, transaction.transaction);
			var kunden = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(angebotInsertionList.Select(x => x.Kunden_Nr ?? 0)?.ToList());
			result = angebotInsertionList.Select(x => new E_RechnungAutoCreationModel
			{
				Nr = x.Nr,
				ForfallNr = x.Angebot_Nr ?? -1,
				ProjectNr = int.TryParse(x.Projekt_Nr, out var s) ? s : 0,
				Type = x.Typ,
				LSNr = x.Nr_lie ?? 0,
				LsAngebotNr = deliveriesToUpdate.FirstOrDefault(a => a.Nr == x.Nr_lie)?.Angebot_Nr ?? -1,
				Customer = kunden.FirstOrDefault(y => y.Nr == x.Kunden_Nr)?.Name1 ?? "",
				CustomerNr = kundenNr,
				CustomerNumber = kunden.FirstOrDefault(y => y.Nr == x.Kunden_Nr)?.Kundennummer ?? 0,
			}).ToList();
			return result;
		}
		public static int GetVorfallNrFromRange(Enums.OrderEnums.Types type, string user)
		{
			int FinalVofallNr = 0;
			int MaxCurrentValue = 0;
			int MinNewValue = 0;
			int MaxNewValue = 0;
			switch(type)
			{
				case Enums.OrderEnums.Types.Confirmation:
					MaxCurrentValue = Module.CTS.abMaxCurrentValue;
					MinNewValue = Module.CTS.abMinNewValue;
					MaxNewValue = Module.CTS.abMaxNewValue;
					break;
				case Enums.OrderEnums.Types.forecast:
					break;
				case Enums.OrderEnums.Types.Contract:
					MaxCurrentValue = Module.CTS.raMaxCurrentValue;
					MinNewValue = Module.CTS.raMinNewValue;
					MaxNewValue = Module.CTS.raMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Kanban:
					break;
				case Enums.OrderEnums.Types.Delivery:
					MaxCurrentValue = Module.CTS.lsMaxCurrentValue;
					MinNewValue = Module.CTS.lsMinNewValue;
					MaxNewValue = Module.CTS.lsMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Invoice:
					MaxCurrentValue = Module.CTS.reMaxCurrentValue;
					MinNewValue = Module.CTS.reMinNewValue;
					MaxNewValue = Module.CTS.reMaxNewValue;
					break;
				case Enums.OrderEnums.Types.Credit:
					MaxCurrentValue = Module.CTS.gsMaxCurrentValue;
					MinNewValue = Module.CTS.gsMinNewValue;
					MaxNewValue = Module.CTS.gsMaxNewValue;
					break;
				default:
					break;
			}
			var maxNr = GetNextAngebotNr(type);
			if((int.TryParse(maxNr, out var val) ? val : 0) < MaxCurrentValue)
				FinalVofallNr = int.TryParse(maxNr, out var val2) ? val2 : 0;
			else
				FinalVofallNr = MinNewValue;
			//alert max reached
			if(FinalVofallNr == MaxNewValue - Module.CTS.Delta)
			{
				string title = "MAX VALUE VORFALL NR LS REACHED";
				var addresses = new List<string>();
				addresses.Add("Mohamed.Souilmi@psz-electronic.com");
				addresses.Add(Module.EmailingService.EmailParamtersModel.AdminEmail);
				var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'><strong>{user.ToUpper()}</strong> has reached the naximum range in vorfall nr in {type} creation [{FinalVofallNr}]</strong>."
				+ $"</span><br/><br/>The change is applyed and Logged"
				+ "<br/><br/>Regards, <br/>IT Department </div>";

				Module.EmailingService.SendEmailAsync(title, content, addresses);
			}
			return FinalVofallNr;
		}
		public static string GetNextAngebotNr(Enums.OrderEnums.Types type)
		{
			var maxAngebotNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxAngebotNrString)) + 1}";
		}
		public static int archiveRechnung(List<int> insertedAngebotNrs)
		{
			var transaction = new TransactionsManager();
			transaction.beginTransaction();
			try
			{
				//*****Mahnwesen table
				var _querys = HelpQuerys(insertedAngebotNrs);
				var _archive = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetForEinzelrechnung_Archives(_querys.Key, _querys.Value,
					transaction.connection, transaction.transaction);
				var _archivesToInsert = _archive?.Select(x => new Infrastructure.Data.Entities.Tables.CTS.MahnwesenEntity
				{
					Adress_id = x.Kunden_Nr,
					Projekt_Nr = int.TryParse(x.Projekt_Nr, out var a) ? a : 0,
					Belegnummer = x.Angebot_Nr,
					Belegdatum = x.Datum,
					Belegtyp = x.Typ,
					Zahlungsfrist = x.Falligkeit,
					Betrag = x.Betrag_MWSt,
					Betrag_FW = 0,
					Mahnstufe = 0,
					Anrede = x.Anrede,
					Vorname_NameFirma = x.Vorname_NameFirma,
					Name2 = x.Name2,
					Name3 = x.Name3,
					Strasse_Postfach = x.Straße_Postfach,
					Land_PLZ_Ort = x.Land_PLZ_Ort,
				}).ToList();
				Infrastructure.Data.Access.Tables.CTS.MahnwesenAccess.InsertWithTransaction(_archivesToInsert, transaction.connection, transaction.transaction);
				//*****Mahnwesen_Archiv table
				var _archivesToInsert2 = _archive?.Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ArchivEntity
				{
					Adress_id = x.Kunden_Nr,
					Projekt_Nr = int.TryParse(x.Projekt_Nr, out var a) ? a : 0,
					Belegnummer = x.Angebot_Nr,
					Belegdatum = x.Datum,
					Zahlungsfrist = x.Falligkeit,
					Soll_DM = x.Betrag_MWSt,
					Haben_DM = 0,
					Datum = x.Datum?.Date,
				}).ToList();
				Infrastructure.Data.Access.Tables.CTS.Mahnwesen_ArchivAccess.InsertWithTransaction(_archivesToInsert2, transaction.connection, transaction.transaction);
				//*****Mahnwesen_Zahlungen table
				var _archives3 = Infrastructure.Data.Access.Joins.CTS.Divers.GetForEinzelrechnung_Archives3(HelpQuerys(insertedAngebotNrs).Key,
					transaction.connection, transaction.transaction);
				var _archivesToInsert3 = _archives3.Select(x => new Infrastructure.Data.Entities.Tables.CTS.Mahnwesen_ZahlungenEntity
				{
					Mahn_ID = x.ID,
					Datum = x.Belegdatum,
					Soll_DM = x.Betrag,
					Soll_FW = x.Ausdr1,
					Haben_DM = x.Ausdr2,
					Haben_FW = x.Ausdr3,
					Text = x.Ausdr4,
					gebucht = x.Ausdr5,
				}).ToList();
				Infrastructure.Data.Access.Tables.CTS.Mahnwesen_ZahlungenAccess.InsertWithTransaction(_archivesToInsert3, transaction.connection, transaction.transaction);
				//******Statistiken Angebote table
				var _archives4 = Infrastructure.Data.Access.Joins.CTS.Divers.GetForEinzelrechnung_Archives4(transaction.connection, transaction.transaction, insertedAngebotNrs);
				var _archivesToInsert4 = _archives4?.Select(x => new Infrastructure.Data.Entities.Tables.CTS.Statistiken_AngeboteEntity
				{
					Adress_Nr = x.Kunden_Nr,
					Typ = x.Typ,
					Datum = x.Datum,
					Personal_Nr = x.Personal_Nr,
					Artikel_Nr = x.Artikel_Nr,
					Anzahl = x.Anzahl,
					Gesamtpreis = x.gesamt,
					Angebot_Nr = x.Angebot_Nr,
					Projekt_Nr = int.TryParse(x.Projekt_Nr, out var n) ? n : 0,
					USt = x.USt,
					Lagerort_ID = x.Lagerort_id,
					Liefertermin = x.Liefertermin,
					Mandant = x.Mandant,
				}).ToList();
				Infrastructure.Data.Access.Tables.CTS.Statistiken_AngeboteAccess.InsertWithTransaction(_archivesToInsert4, transaction.connection, transaction.transaction);
				if(transaction.commit())
					return 1;
				else
				{
					transaction.rollback();
					return -1;
				}

			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

		}
		public static KeyValuePair<string, string> HelpQuerys(List<int> angebotNr)
		{
			string q1 = $@"SELECT Angebote.[Angebot-Nr], Angebote.Typ,Angebote.Nr as ID, Angebote.nr_lie as [ID-LS],Angebote.Mandant, 0 AS Gedruckt
                        FROM Angebote WHERE (((Angebote.Typ)='Rechnung') AND 
                        ((Angebote.Mandant)='PSZ electronic') AND ((Angebote.[Angebot-Nr]) IN ({string.Join(",", angebotNr)})))";

			string q2 = $@"select X.Typ,X.[Angebot-Nr], Sum([Gesamtpreis]*(1+[USt])) AS Betrag_MWSt
                      from (
                      SELECT Angebote.[Angebot-Nr], Angebote.Typ,Angebote.Nr as ID, Angebote.nr_lie as [ID-LS],Angebote.Mandant, 0 AS Gedruckt
                      FROM Angebote WHERE (((Angebote.Typ)='Rechnung') AND 
                      ((Angebote.Mandant)='PSZ electronic') AND ((Angebote.[Angebot-Nr]) IN ({string.Join(",", angebotNr)})))
                      ) X
                      INNER JOIN [angebotene Artikel] ON [X].ID = [angebotene Artikel].[Angebot-Nr]
                      GROUP BY [X].Typ, [X].[Angebot-Nr]";

			return new KeyValuePair<string, string>(q1, q2);
		}

	}
}
