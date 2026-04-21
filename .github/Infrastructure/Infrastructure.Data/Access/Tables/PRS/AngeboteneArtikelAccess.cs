using Infrastructure.Data.Entities.Tables.PRS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class AngeboteneArtikelAccess
	{
		#region Default Methods
		public static AngeboteneArtikelEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebotene Artikel] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new AngeboteneArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<AngeboteneArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Angebotene Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<AngeboteneArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<AngeboteneArtikelEntity>();
		}
		private static List<AngeboteneArtikelEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [Angebotene Artikel] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			}
			return new List<AngeboteneArtikelEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Angebotene Artikel] ([AB Pos zu BV Pos],[AB Pos zu RA Pos],[Abladestelle],[Aktuelle Anzahl],[AnfangLagerBestand],[Angebot-Nr],[Anzahl],[Artikel-Nr],[Auswahl],[Bemerkungsfeld1],[Bemerkungsfeld2],[Bestellnummer],[Bezeichnung1],[Bezeichnung2],[Bezeichnung2_Kunde],[Bezeichnung3],[CSInterneBemerkung],[DEL],[DEL fixiert],[EDI_Historie_Nr],[EDI_PREIS_KUNDE],[EDI_PREISEINHEIT],[EDI_Quantity_Ordered],[Einheit],[EinzelCu-Gewicht],[Einzelkupferzuschlag],[Einzelpreis],[EKPreise_Fix],[EndeLagerBestand],[erledigt_pos],[Fertigungsnummer],[FM],[FM_Einzelpreis],[FM_Gesamtpreis],[Freies_Format_EDI],[Geliefert],[Gepackt_von],[Gepackt_Zeitpunkt],[GesamtCu-Gewicht],[Gesamtkupferzuschlag],[Gesamtpreis],[GSExternComment],[GSInternComment],[GSWithoutCopper],[In Bearbeitung],[Index_Kunde],[Index_Kunde_datum],[KB Pos zu BV Pos],[KB Pos zu RA Pos],[Kupferbasis],[Lagerbewegung],[Lagerbewegung_rückgängig],[Lagerort_id],[Langtext],[Langtext_drucken],[Lieferanweisung (P_FTXDIN_TEXT)],[Liefertermin],[Löschen],[LS Pos zu AB Pos],[LS Pos zu KB Pos],[LS_von_Versand_gedruckt],[OriginalAnzahl],[Packinfo_von_Lager],[Packstatus],[Position],[PositionZUEDI],[POSTEXT],[Preis_ausweisen],[Preiseinheit],[Preisgruppe],[RA Pos zu BV Pos],[RA_Abgerufen],[RA_Offen],[RA_OriginalAnzahl],[Rabatt],[RE Pos zu GS Pos],[RP],[schriftart],[Seriennummern_drucken],[sortierung],[Stückliste],[Stückliste_drucken],[Summenberechnung],[termin_eingehalten],[Typ],[USt],[VDA_gedruckt],[Versand_gedruckt],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Versanddienstleister],[Versandinfo_von_CS],[Versandinfo_von_Lager],[Versandnummer],[Versandstatus],[VKEinzelpreis],[VK-Festpreis],[VKGesamtpreis],[Wunschtermin],[Zeichnungsnummer],[Zuschlag_VK],[zwischensumme]) OUTPUT INSERTED.[Nr] VALUES (@AB_Pos_zu_BV_Pos,@AB_Pos_zu_RA_Pos,@Abladestelle,@Aktuelle_Anzahl,@AnfangLagerBestand,@Angebot_Nr,@Anzahl,@Artikel_Nr,@Auswahl,@Bemerkungsfeld1,@Bemerkungsfeld2,@Bestellnummer,@Bezeichnung1,@Bezeichnung2,@Bezeichnung2_Kunde,@Bezeichnung3,@CSInterneBemerkung,@DEL,@DEL_fixiert,@EDI_Historie_Nr,@EDI_PREIS_KUNDE,@EDI_PREISEINHEIT,@EDI_Quantity_Ordered,@Einheit,@EinzelCu_Gewicht,@Einzelkupferzuschlag,@Einzelpreis,@EKPreise_Fix,@EndeLagerBestand,@erledigt_pos,@Fertigungsnummer,@FM,@FM_Einzelpreis,@FM_Gesamtpreis,@Freies_Format_EDI,@Geliefert,@Gepackt_von,@Gepackt_Zeitpunkt,@GesamtCu_Gewicht,@Gesamtkupferzuschlag,@Gesamtpreis,@GSExternComment,@GSInternComment,@GSWithoutCopper,@In_Bearbeitung,@Index_Kunde,@Index_Kunde_datum,@KB_Pos_zu_BV_Pos,@KB_Pos_zu_RA_Pos,@Kupferbasis,@Lagerbewegung,@Lagerbewegung_ruckgangig,@Lagerort_id,@Langtext,@Langtext_drucken,@Lieferanweisung__P_FTXDIN_TEXT_,@Liefertermin,@Loschen,@LS_Pos_zu_AB_Pos,@LS_Pos_zu_KB_Pos,@LS_von_Versand_gedruckt,@OriginalAnzahl,@Packinfo_von_Lager,@Packstatus,@Position,@PositionZUEDI,@POSTEXT,@Preis_ausweisen,@Preiseinheit,@Preisgruppe,@RA_Pos_zu_BV_Pos,@RA_Abgerufen,@RA_Offen,@RA_OriginalAnzahl,@Rabatt,@RE_Pos_zu_GS_Pos,@RP,@schriftart,@Seriennummern_drucken,@sortierung,@Stuckliste,@Stuckliste_drucken,@Summenberechnung,@termin_eingehalten,@Typ,@USt,@VDA_gedruckt,@Versand_gedruckt,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Versanddienstleister,@Versandinfo_von_CS,@Versandinfo_von_Lager,@Versandnummer,@Versandstatus,@VKEinzelpreis,@VK_Festpreis,@VKGesamtpreis,@Wunschtermin,@Zeichnungsnummer,@Zuschlag_VK,@zwischensumme); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos", item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos", item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1", item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2", item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde", item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
					sqlCommand.Parameters.AddWithValue("Bezeichnung3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("CSInterneBemerkung", item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
					sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
					sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr", item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
					sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE", item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
					sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT", item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
					sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered", item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht", item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
					sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag", item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EKPreise_Fix", item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("FM", item.FM == null ? (object)DBNull.Value : item.FM);
					sqlCommand.Parameters.AddWithValue("FM_Einzelpreis", item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
					sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis", item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Freies_Format_EDI", item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
					sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Gepackt_von", item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
					sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt", item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
					sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht", item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
					sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag", item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GSExternComment", item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
					sqlCommand.Parameters.AddWithValue("GSInternComment", item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
					sqlCommand.Parameters.AddWithValue("GSWithoutCopper", item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos", item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos", item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung", item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig", item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken", item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
					sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_", item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos", item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos", item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
					sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt", item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("OriginalAnzahl", item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager", item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Packstatus", item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("PositionZUEDI", item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
					sqlCommand.Parameters.AddWithValue("POSTEXT", item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
					sqlCommand.Parameters.AddWithValue("Preis_ausweisen", item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos", item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("RA_Abgerufen", item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RA_Offen", item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
					sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl", item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos", item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
					sqlCommand.Parameters.AddWithValue("RP", item.RP == null ? (object)DBNull.Value : item.RP);
					sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Seriennummern_drucken", item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
					sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
					sqlCommand.Parameters.AddWithValue("Stuckliste_drucken", item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
					sqlCommand.Parameters.AddWithValue("Summenberechnung", item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("VDA_gedruckt", item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versand_gedruckt", item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddienstleister", item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS", item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager", item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Versandnummer", item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
					sqlCommand.Parameters.AddWithValue("Versandstatus", item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
					sqlCommand.Parameters.AddWithValue("VKEinzelpreis", item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("VKGesamtpreis", item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
					sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					sqlCommand.Parameters.AddWithValue("zwischensumme", item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 104; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Angebotene Artikel] ([AB Pos zu BV Pos],[AB Pos zu RA Pos],[Abladestelle],[Aktuelle Anzahl],[AnfangLagerBestand],[Angebot-Nr],[Anzahl],[Artikel-Nr],[Auswahl],[Bemerkungsfeld1],[Bemerkungsfeld2],[Bestellnummer],[Bezeichnung1],[Bezeichnung2],[Bezeichnung2_Kunde],[Bezeichnung3],[CSInterneBemerkung],[DEL],[DEL fixiert],[EDI_Historie_Nr],[EDI_PREIS_KUNDE],[EDI_PREISEINHEIT],[EDI_Quantity_Ordered],[Einheit],[EinzelCu-Gewicht],[Einzelkupferzuschlag],[Einzelpreis],[EKPreise_Fix],[EndeLagerBestand],[erledigt_pos],[Fertigungsnummer],[FM],[FM_Einzelpreis],[FM_Gesamtpreis],[Freies_Format_EDI],[Geliefert],[Gepackt_von],[Gepackt_Zeitpunkt],[GesamtCu-Gewicht],[Gesamtkupferzuschlag],[Gesamtpreis],[GSExternComment],[GSInternComment],[GSWithoutCopper],[In Bearbeitung],[Index_Kunde],[Index_Kunde_datum],[KB Pos zu BV Pos],[KB Pos zu RA Pos],[Kupferbasis],[Lagerbewegung],[Lagerbewegung_rückgängig],[Lagerort_id],[Langtext],[Langtext_drucken],[Lieferanweisung (P_FTXDIN_TEXT)],[Liefertermin],[Löschen],[LS Pos zu AB Pos],[LS Pos zu KB Pos],[LS_von_Versand_gedruckt],[OriginalAnzahl],[Packinfo_von_Lager],[Packstatus],[Position],[PositionZUEDI],[POSTEXT],[Preis_ausweisen],[Preiseinheit],[Preisgruppe],[RA Pos zu BV Pos],[RA_Abgerufen],[RA_Offen],[RA_OriginalAnzahl],[Rabatt],[RE Pos zu GS Pos],[RP],[schriftart],[Seriennummern_drucken],[sortierung],[Stückliste],[Stückliste_drucken],[Summenberechnung],[termin_eingehalten],[Typ],[USt],[VDA_gedruckt],[Versand_gedruckt],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Versanddienstleister],[Versandinfo_von_CS],[Versandinfo_von_Lager],[Versandnummer],[Versandstatus],[VKEinzelpreis],[VK-Festpreis],[VKGesamtpreis],[Wunschtermin],[Zeichnungsnummer],[Zuschlag_VK],[zwischensumme]) VALUES ( "

							+ "@AB_Pos_zu_BV_Pos" + i + ","
							+ "@AB_Pos_zu_RA_Pos" + i + ","
							+ "@Abladestelle" + i + ","
							+ "@Aktuelle_Anzahl" + i + ","
							+ "@AnfangLagerBestand" + i + ","
							+ "@Angebot_Nr" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Auswahl" + i + ","
							+ "@Bemerkungsfeld1" + i + ","
							+ "@Bemerkungsfeld2" + i + ","
							+ "@Bestellnummer" + i + ","
							+ "@Bezeichnung1" + i + ","
							+ "@Bezeichnung2" + i + ","
							+ "@Bezeichnung2_Kunde" + i + ","
							+ "@Bezeichnung3" + i + ","
							+ "@CSInterneBemerkung" + i + ","
							+ "@DEL" + i + ","
							+ "@DEL_fixiert" + i + ","
							+ "@EDI_Historie_Nr" + i + ","
							+ "@EDI_PREIS_KUNDE" + i + ","
							+ "@EDI_PREISEINHEIT" + i + ","
							+ "@EDI_Quantity_Ordered" + i + ","
							+ "@Einheit" + i + ","
							+ "@EinzelCu_Gewicht" + i + ","
							+ "@Einzelkupferzuschlag" + i + ","
							+ "@Einzelpreis" + i + ","
							+ "@EKPreise_Fix" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@erledigt_pos" + i + ","
							+ "@Fertigungsnummer" + i + ","
							+ "@FM" + i + ","
							+ "@FM_Einzelpreis" + i + ","
							+ "@FM_Gesamtpreis" + i + ","
							+ "@Freies_Format_EDI" + i + ","
							+ "@Geliefert" + i + ","
							+ "@Gepackt_von" + i + ","
							+ "@Gepackt_Zeitpunkt" + i + ","
							+ "@GesamtCu_Gewicht" + i + ","
							+ "@Gesamtkupferzuschlag" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@GSExternComment" + i + ","
							+ "@GSInternComment" + i + ","
							+ "@GSWithoutCopper" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@Index_Kunde_datum" + i + ","
							+ "@KB_Pos_zu_BV_Pos" + i + ","
							+ "@KB_Pos_zu_RA_Pos" + i + ","
							+ "@Kupferbasis" + i + ","
							+ "@Lagerbewegung" + i + ","
							+ "@Lagerbewegung_ruckgangig" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Langtext" + i + ","
							+ "@Langtext_drucken" + i + ","
							+ "@Lieferanweisung__P_FTXDIN_TEXT_" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Loschen" + i + ","
							+ "@LS_Pos_zu_AB_Pos" + i + ","
							+ "@LS_Pos_zu_KB_Pos" + i + ","
							+ "@LS_von_Versand_gedruckt" + i + ","
							+ "@OriginalAnzahl" + i + ","
							+ "@Packinfo_von_Lager" + i + ","
							+ "@Packstatus" + i + ","
							+ "@Position" + i + ","
							+ "@PositionZUEDI" + i + ","
							+ "@POSTEXT" + i + ","
							+ "@Preis_ausweisen" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@Preisgruppe" + i + ","
							+ "@RA_Pos_zu_BV_Pos" + i + ","
							+ "@RA_Abgerufen" + i + ","
							+ "@RA_Offen" + i + ","
							+ "@RA_OriginalAnzahl" + i + ","
							+ "@Rabatt" + i + ","
							+ "@RE_Pos_zu_GS_Pos" + i + ","
							+ "@RP" + i + ","
							+ "@schriftart" + i + ","
							+ "@Seriennummern_drucken" + i + ","
							+ "@sortierung" + i + ","
							+ "@Stuckliste" + i + ","
							+ "@Stuckliste_drucken" + i + ","
							+ "@Summenberechnung" + i + ","
							+ "@termin_eingehalten" + i + ","
							+ "@Typ" + i + ","
							+ "@USt" + i + ","
							+ "@VDA_gedruckt" + i + ","
							+ "@Versand_gedruckt" + i + ","
							+ "@Versandarten_Auswahl" + i + ","
							+ "@Versanddatum_Auswahl" + i + ","
							+ "@Versanddienstleister" + i + ","
							+ "@Versandinfo_von_CS" + i + ","
							+ "@Versandinfo_von_Lager" + i + ","
							+ "@Versandnummer" + i + ","
							+ "@Versandstatus" + i + ","
							+ "@VKEinzelpreis" + i + ","
							+ "@VK_Festpreis" + i + ","
							+ "@VKGesamtpreis" + i + ","
							+ "@Wunschtermin" + i + ","
							+ "@Zeichnungsnummer" + i + ","
							+ "@Zuschlag_VK" + i + ","
							+ "@zwischensumme" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos" + i, item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos" + i, item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1" + i, item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
						sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2" + i, item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde" + i, item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
						sqlCommand.Parameters.AddWithValue("Bezeichnung3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
						sqlCommand.Parameters.AddWithValue("CSInterneBemerkung" + i, item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
						sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr" + i, item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
						sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE" + i, item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
						sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT" + i, item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
						sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered" + i, item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht" + i, item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
						sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag" + i, item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EKPreise_Fix" + i, item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("FM" + i, item.FM == null ? (object)DBNull.Value : item.FM);
						sqlCommand.Parameters.AddWithValue("FM_Einzelpreis" + i, item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
						sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis" + i, item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Freies_Format_EDI" + i, item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
						sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
						sqlCommand.Parameters.AddWithValue("Gepackt_von" + i, item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
						sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt" + i, item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
						sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht" + i, item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
						sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag" + i, item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("GSExternComment" + i, item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
						sqlCommand.Parameters.AddWithValue("GSInternComment" + i, item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
						sqlCommand.Parameters.AddWithValue("GSWithoutCopper" + i, item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos" + i, item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos" + i, item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Lagerbewegung" + i, item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
						sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig" + i, item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken" + i, item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
						sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_" + i, item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos" + i, item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
						sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos" + i, item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
						sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt" + i, item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
						sqlCommand.Parameters.AddWithValue("OriginalAnzahl" + i, item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager" + i, item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
						sqlCommand.Parameters.AddWithValue("Packstatus" + i, item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("PositionZUEDI" + i, item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
						sqlCommand.Parameters.AddWithValue("POSTEXT" + i, item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
						sqlCommand.Parameters.AddWithValue("Preis_ausweisen" + i, item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos" + i, item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("RA_Abgerufen" + i, item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RA_Offen" + i, item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
						sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl" + i, item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos" + i, item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
						sqlCommand.Parameters.AddWithValue("RP" + i, item.RP == null ? (object)DBNull.Value : item.RP);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("Seriennummern_drucken" + i, item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
						sqlCommand.Parameters.AddWithValue("Stuckliste_drucken" + i, item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
						sqlCommand.Parameters.AddWithValue("Summenberechnung" + i, item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("VDA_gedruckt" + i, item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
						sqlCommand.Parameters.AddWithValue("Versand_gedruckt" + i, item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
						sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddienstleister" + i, item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
						sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS" + i, item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
						sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager" + i, item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
						sqlCommand.Parameters.AddWithValue("Versandnummer" + i, item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
						sqlCommand.Parameters.AddWithValue("Versandstatus" + i, item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
						sqlCommand.Parameters.AddWithValue("VKEinzelpreis" + i, item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
						sqlCommand.Parameters.AddWithValue("VKGesamtpreis" + i, item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
						sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
						sqlCommand.Parameters.AddWithValue("zwischensumme" + i, item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Angebotene Artikel] SET [AB Pos zu BV Pos]=@AB_Pos_zu_BV_Pos, [AB Pos zu RA Pos]=@AB_Pos_zu_RA_Pos, [Abladestelle]=@Abladestelle, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Angebot-Nr]=@Angebot_Nr, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Auswahl]=@Auswahl, [Bemerkungsfeld1]=@Bemerkungsfeld1, [Bemerkungsfeld2]=@Bemerkungsfeld2, [Bestellnummer]=@Bestellnummer, [Bezeichnung1]=@Bezeichnung1, [Bezeichnung2]=@Bezeichnung2, [Bezeichnung2_Kunde]=@Bezeichnung2_Kunde, [Bezeichnung3]=@Bezeichnung3, [CSInterneBemerkung]=@CSInterneBemerkung, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [EDI_Historie_Nr]=@EDI_Historie_Nr, [EDI_PREIS_KUNDE]=@EDI_PREIS_KUNDE, [EDI_PREISEINHEIT]=@EDI_PREISEINHEIT, [EDI_Quantity_Ordered]=@EDI_Quantity_Ordered, [Einheit]=@Einheit, [EinzelCu-Gewicht]=@EinzelCu_Gewicht, [Einzelkupferzuschlag]=@Einzelkupferzuschlag, [Einzelpreis]=@Einzelpreis, [EKPreise_Fix]=@EKPreise_Fix, [EndeLagerBestand]=@EndeLagerBestand, [erledigt_pos]=@erledigt_pos, [Fertigungsnummer]=@Fertigungsnummer, [FM]=@FM, [FM_Einzelpreis]=@FM_Einzelpreis, [FM_Gesamtpreis]=@FM_Gesamtpreis, [Freies_Format_EDI]=@Freies_Format_EDI, [Geliefert]=@Geliefert, [Gepackt_von]=@Gepackt_von, [Gepackt_Zeitpunkt]=@Gepackt_Zeitpunkt, [GesamtCu-Gewicht]=@GesamtCu_Gewicht, [Gesamtkupferzuschlag]=@Gesamtkupferzuschlag, [Gesamtpreis]=@Gesamtpreis, [GSExternComment]=@GSExternComment, [GSInternComment]=@GSInternComment, [GSWithoutCopper]=@GSWithoutCopper, [In Bearbeitung]=@In_Bearbeitung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_datum]=@Index_Kunde_datum, [KB Pos zu BV Pos]=@KB_Pos_zu_BV_Pos, [KB Pos zu RA Pos]=@KB_Pos_zu_RA_Pos, [Kupferbasis]=@Kupferbasis, [Lagerbewegung]=@Lagerbewegung, [Lagerbewegung_rückgängig]=@Lagerbewegung_ruckgangig, [Lagerort_id]=@Lagerort_id, [Langtext]=@Langtext, [Langtext_drucken]=@Langtext_drucken, [Lieferanweisung (P_FTXDIN_TEXT)]=@Lieferanweisung__P_FTXDIN_TEXT_, [Liefertermin]=@Liefertermin, [Löschen]=@Loschen, [LS Pos zu AB Pos]=@LS_Pos_zu_AB_Pos, [LS Pos zu KB Pos]=@LS_Pos_zu_KB_Pos, [LS_von_Versand_gedruckt]=@LS_von_Versand_gedruckt, [OriginalAnzahl]=@OriginalAnzahl, [Packinfo_von_Lager]=@Packinfo_von_Lager, [Packstatus]=@Packstatus, [Position]=@Position, [PositionZUEDI]=@PositionZUEDI, [POSTEXT]=@POSTEXT, [Preis_ausweisen]=@Preis_ausweisen, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [RA Pos zu BV Pos]=@RA_Pos_zu_BV_Pos, [RA_Abgerufen]=@RA_Abgerufen, [RA_Offen]=@RA_Offen, [RA_OriginalAnzahl]=@RA_OriginalAnzahl, [Rabatt]=@Rabatt, [RE Pos zu GS Pos]=@RE_Pos_zu_GS_Pos, [RP]=@RP, [schriftart]=@schriftart, [Seriennummern_drucken]=@Seriennummern_drucken, [sortierung]=@sortierung, [Stückliste]=@Stuckliste, [Stückliste_drucken]=@Stuckliste_drucken, [Summenberechnung]=@Summenberechnung, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [USt]=@USt, [VDA_gedruckt]=@VDA_gedruckt, [Versand_gedruckt]=@Versand_gedruckt, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Versanddienstleister]=@Versanddienstleister, [Versandinfo_von_CS]=@Versandinfo_von_CS, [Versandinfo_von_Lager]=@Versandinfo_von_Lager, [Versandnummer]=@Versandnummer, [Versandstatus]=@Versandstatus, [VKEinzelpreis]=@VKEinzelpreis, [VK-Festpreis]=@VK_Festpreis, [VKGesamtpreis]=@VKGesamtpreis, [Wunschtermin]=@Wunschtermin, [Zeichnungsnummer]=@Zeichnungsnummer, [Zuschlag_VK]=@Zuschlag_VK, [zwischensumme]=@zwischensumme WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos", item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
				sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos", item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
				sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
				sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
				sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1", item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
				sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2", item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
				sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde", item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
				sqlCommand.Parameters.AddWithValue("Bezeichnung3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
				sqlCommand.Parameters.AddWithValue("CSInterneBemerkung", item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
				sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
				sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
				sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr", item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
				sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE", item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
				sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT", item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
				sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered", item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht", item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
				sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag", item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
				sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
				sqlCommand.Parameters.AddWithValue("EKPreise_Fix", item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("FM", item.FM == null ? (object)DBNull.Value : item.FM);
				sqlCommand.Parameters.AddWithValue("FM_Einzelpreis", item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
				sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis", item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("Freies_Format_EDI", item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
				sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
				sqlCommand.Parameters.AddWithValue("Gepackt_von", item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
				sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt", item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
				sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht", item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
				sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag", item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("GSExternComment", item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
				sqlCommand.Parameters.AddWithValue("GSInternComment", item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
				sqlCommand.Parameters.AddWithValue("GSWithoutCopper", item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Index_Kunde_datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos", item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
				sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos", item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
				sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
				sqlCommand.Parameters.AddWithValue("Lagerbewegung", item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
				sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig", item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken", item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
				sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_", item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos", item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
				sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos", item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
				sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt", item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
				sqlCommand.Parameters.AddWithValue("OriginalAnzahl", item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
				sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager", item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
				sqlCommand.Parameters.AddWithValue("Packstatus", item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("PositionZUEDI", item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
				sqlCommand.Parameters.AddWithValue("POSTEXT", item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
				sqlCommand.Parameters.AddWithValue("Preis_ausweisen", item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos", item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
				sqlCommand.Parameters.AddWithValue("RA_Abgerufen", item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
				sqlCommand.Parameters.AddWithValue("RA_Offen", item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
				sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl", item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos", item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
				sqlCommand.Parameters.AddWithValue("RP", item.RP == null ? (object)DBNull.Value : item.RP);
				sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
				sqlCommand.Parameters.AddWithValue("Seriennummern_drucken", item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
				sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
				sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
				sqlCommand.Parameters.AddWithValue("Stuckliste_drucken", item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
				sqlCommand.Parameters.AddWithValue("Summenberechnung", item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
				sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
				sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
				sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
				sqlCommand.Parameters.AddWithValue("VDA_gedruckt", item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
				sqlCommand.Parameters.AddWithValue("Versand_gedruckt", item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
				sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
				sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
				sqlCommand.Parameters.AddWithValue("Versanddienstleister", item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
				sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS", item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
				sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager", item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
				sqlCommand.Parameters.AddWithValue("Versandnummer", item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
				sqlCommand.Parameters.AddWithValue("Versandstatus", item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
				sqlCommand.Parameters.AddWithValue("VKEinzelpreis", item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
				sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
				sqlCommand.Parameters.AddWithValue("VKGesamtpreis", item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
				sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
				sqlCommand.Parameters.AddWithValue("zwischensumme", item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 104; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = update(items);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += update(items.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += update(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber));
				}

				return results;
			}

			return -1;
		}
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [Angebotene Artikel] SET "

							+ "[AB Pos zu BV Pos]=@AB_Pos_zu_BV_Pos" + i + ","
							+ "[AB Pos zu RA Pos]=@AB_Pos_zu_RA_Pos" + i + ","
							+ "[Abladestelle]=@Abladestelle" + i + ","
							+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
							+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
							+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Auswahl]=@Auswahl" + i + ","
							+ "[Bemerkungsfeld1]=@Bemerkungsfeld1" + i + ","
							+ "[Bemerkungsfeld2]=@Bemerkungsfeld2" + i + ","
							+ "[Bestellnummer]=@Bestellnummer" + i + ","
							+ "[Bezeichnung1]=@Bezeichnung1" + i + ","
							+ "[Bezeichnung2]=@Bezeichnung2" + i + ","
							+ "[Bezeichnung2_Kunde]=@Bezeichnung2_Kunde" + i + ","
							+ "[Bezeichnung3]=@Bezeichnung3" + i + ","
							+ "[CSInterneBemerkung]=@CSInterneBemerkung" + i + ","
							+ "[DEL]=@DEL" + i + ","
							+ "[DEL fixiert]=@DEL_fixiert" + i + ","
							+ "[EDI_Historie_Nr]=@EDI_Historie_Nr" + i + ","
							+ "[EDI_PREIS_KUNDE]=@EDI_PREIS_KUNDE" + i + ","
							+ "[EDI_PREISEINHEIT]=@EDI_PREISEINHEIT" + i + ","
							+ "[EDI_Quantity_Ordered]=@EDI_Quantity_Ordered" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[EinzelCu-Gewicht]=@EinzelCu_Gewicht" + i + ","
							+ "[Einzelkupferzuschlag]=@Einzelkupferzuschlag" + i + ","
							+ "[Einzelpreis]=@Einzelpreis" + i + ","
							+ "[EKPreise_Fix]=@EKPreise_Fix" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[erledigt_pos]=@erledigt_pos" + i + ","
							+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
							+ "[FM]=@FM" + i + ","
							+ "[FM_Einzelpreis]=@FM_Einzelpreis" + i + ","
							+ "[FM_Gesamtpreis]=@FM_Gesamtpreis" + i + ","
							+ "[Freies_Format_EDI]=@Freies_Format_EDI" + i + ","
							+ "[Geliefert]=@Geliefert" + i + ","
							+ "[Gepackt_von]=@Gepackt_von" + i + ","
							+ "[Gepackt_Zeitpunkt]=@Gepackt_Zeitpunkt" + i + ","
							+ "[GesamtCu-Gewicht]=@GesamtCu_Gewicht" + i + ","
							+ "[Gesamtkupferzuschlag]=@Gesamtkupferzuschlag" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[GSExternComment]=@GSExternComment" + i + ","
							+ "[GSInternComment]=@GSInternComment" + i + ","
							+ "[GSWithoutCopper]=@GSWithoutCopper" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[Index_Kunde_datum]=@Index_Kunde_datum" + i + ","
							+ "[KB Pos zu BV Pos]=@KB_Pos_zu_BV_Pos" + i + ","
							+ "[KB Pos zu RA Pos]=@KB_Pos_zu_RA_Pos" + i + ","
							+ "[Kupferbasis]=@Kupferbasis" + i + ","
							+ "[Lagerbewegung]=@Lagerbewegung" + i + ","
							+ "[Lagerbewegung_rückgängig]=@Lagerbewegung_ruckgangig" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Langtext]=@Langtext" + i + ","
							+ "[Langtext_drucken]=@Langtext_drucken" + i + ","
							+ "[Lieferanweisung (P_FTXDIN_TEXT)]=@Lieferanweisung__P_FTXDIN_TEXT_" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Löschen]=@Loschen" + i + ","
							+ "[LS Pos zu AB Pos]=@LS_Pos_zu_AB_Pos" + i + ","
							+ "[LS Pos zu KB Pos]=@LS_Pos_zu_KB_Pos" + i + ","
							+ "[LS_von_Versand_gedruckt]=@LS_von_Versand_gedruckt" + i + ","
							+ "[OriginalAnzahl]=@OriginalAnzahl" + i + ","
							+ "[Packinfo_von_Lager]=@Packinfo_von_Lager" + i + ","
							+ "[Packstatus]=@Packstatus" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[PositionZUEDI]=@PositionZUEDI" + i + ","
							+ "[POSTEXT]=@POSTEXT" + i + ","
							+ "[Preis_ausweisen]=@Preis_ausweisen" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + ","
							+ "[RA Pos zu BV Pos]=@RA_Pos_zu_BV_Pos" + i + ","
							+ "[RA_Abgerufen]=@RA_Abgerufen" + i + ","
							+ "[RA_Offen]=@RA_Offen" + i + ","
							+ "[RA_OriginalAnzahl]=@RA_OriginalAnzahl" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[RE Pos zu GS Pos]=@RE_Pos_zu_GS_Pos" + i + ","
							+ "[RP]=@RP" + i + ","
							+ "[schriftart]=@schriftart" + i + ","
							+ "[Seriennummern_drucken]=@Seriennummern_drucken" + i + ","
							+ "[sortierung]=@sortierung" + i + ","
							+ "[Stückliste]=@Stuckliste" + i + ","
							+ "[Stückliste_drucken]=@Stuckliste_drucken" + i + ","
							+ "[Summenberechnung]=@Summenberechnung" + i + ","
							+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
							+ "[Typ]=@Typ" + i + ","
							+ "[USt]=@USt" + i + ","
							+ "[VDA_gedruckt]=@VDA_gedruckt" + i + ","
							+ "[Versand_gedruckt]=@Versand_gedruckt" + i + ","
							+ "[Versandarten_Auswahl]=@Versandarten_Auswahl" + i + ","
							+ "[Versanddatum_Auswahl]=@Versanddatum_Auswahl" + i + ","
							+ "[Versanddienstleister]=@Versanddienstleister" + i + ","
							+ "[Versandinfo_von_CS]=@Versandinfo_von_CS" + i + ","
							+ "[Versandinfo_von_Lager]=@Versandinfo_von_Lager" + i + ","
							+ "[Versandnummer]=@Versandnummer" + i + ","
							+ "[Versandstatus]=@Versandstatus" + i + ","
							+ "[VKEinzelpreis]=@VKEinzelpreis" + i + ","
							+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
							+ "[VKGesamtpreis]=@VKGesamtpreis" + i + ","
							+ "[Wunschtermin]=@Wunschtermin" + i + ","
							+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
							+ "[Zuschlag_VK]=@Zuschlag_VK" + i + ","
							+ "[zwischensumme]=@zwischensumme" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos" + i, item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos" + i, item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1" + i, item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
						sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2" + i, item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde" + i, item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
						sqlCommand.Parameters.AddWithValue("Bezeichnung3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
						sqlCommand.Parameters.AddWithValue("CSInterneBemerkung" + i, item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
						sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr" + i, item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
						sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE" + i, item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
						sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT" + i, item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
						sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered" + i, item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht" + i, item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
						sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag" + i, item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EKPreise_Fix" + i, item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("FM" + i, item.FM == null ? (object)DBNull.Value : item.FM);
						sqlCommand.Parameters.AddWithValue("FM_Einzelpreis" + i, item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
						sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis" + i, item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Freies_Format_EDI" + i, item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
						sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
						sqlCommand.Parameters.AddWithValue("Gepackt_von" + i, item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
						sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt" + i, item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
						sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht" + i, item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
						sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag" + i, item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("GSExternComment" + i, item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
						sqlCommand.Parameters.AddWithValue("GSInternComment" + i, item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
						sqlCommand.Parameters.AddWithValue("GSWithoutCopper" + i, item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos" + i, item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos" + i, item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Lagerbewegung" + i, item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
						sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig" + i, item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken" + i, item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
						sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_" + i, item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos" + i, item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
						sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos" + i, item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
						sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt" + i, item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
						sqlCommand.Parameters.AddWithValue("OriginalAnzahl" + i, item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager" + i, item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
						sqlCommand.Parameters.AddWithValue("Packstatus" + i, item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("PositionZUEDI" + i, item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
						sqlCommand.Parameters.AddWithValue("POSTEXT" + i, item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
						sqlCommand.Parameters.AddWithValue("Preis_ausweisen" + i, item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos" + i, item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
						sqlCommand.Parameters.AddWithValue("RA_Abgerufen" + i, item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RA_Offen" + i, item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
						sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl" + i, item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos" + i, item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
						sqlCommand.Parameters.AddWithValue("RP" + i, item.RP == null ? (object)DBNull.Value : item.RP);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("Seriennummern_drucken" + i, item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
						sqlCommand.Parameters.AddWithValue("Stuckliste_drucken" + i, item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
						sqlCommand.Parameters.AddWithValue("Summenberechnung" + i, item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
						sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
						sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
						sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
						sqlCommand.Parameters.AddWithValue("VDA_gedruckt" + i, item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
						sqlCommand.Parameters.AddWithValue("Versand_gedruckt" + i, item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
						sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
						sqlCommand.Parameters.AddWithValue("Versanddienstleister" + i, item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
						sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS" + i, item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
						sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager" + i, item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
						sqlCommand.Parameters.AddWithValue("Versandnummer" + i, item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
						sqlCommand.Parameters.AddWithValue("Versandstatus" + i, item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
						sqlCommand.Parameters.AddWithValue("VKEinzelpreis" + i, item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
						sqlCommand.Parameters.AddWithValue("VKGesamtpreis" + i, item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
						sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
						sqlCommand.Parameters.AddWithValue("zwischensumme" + i, item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Angebotene Artikel] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM [Angebotene Artikel] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion Default Methods

		#region Custom Methods
		public static List<AngeboteneArtikelEntity> GetByAngeboteNr(int OrderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction, bool ignoreDeleted = true)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				string query = "";
				if(ignoreDeleted)
				{
					query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId AND [Anzahl]<>0";
				}
				else
				{
					query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetByAngeboteNr(int OrderId, bool ignoreDeleted = true)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					if(ignoreDeleted)
					{
						query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId AND [Anzahl]<>0";
					}
					else
					{
						query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId";
					}
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetByAngeboteNr(List<int> OrderIds, bool ignoreDeleted = true)
		{
			if(OrderIds == null || OrderIds.Count <= 0)
				return null;

			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					string ids = string.Join(",", OrderIds);
					if(ignoreDeleted)
					{
						query = $"SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN ({ids}) AND [Anzahl]<>0";
					}
					else
					{
						query = $"SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN ({ids})";
					}
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyAngebote(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<AngeboteneArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getbyAngebote(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getbyAngebote(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getbyAngebote(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<AngeboteneArtikelEntity>();
		}
		private static List<AngeboteneArtikelEntity> getbyAngebote(List<int> ids)
		{
			DataTable dataTable = new DataTable();
			if(ids != null && ids.Count > 0)
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand("", sqlConnection);

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN (" + queryIds + ")";

					var response = new List<AngeboteneArtikelEntity>();
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetByNr(int Nr, bool ignoreDeleted = true)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "";
					if(ignoreDeleted)
					{
						query = "SELECT * FROM [Angebotene Artikel] WHERE [Nr] = @Nr AND [Anzahl]<>0";
					}
					else
					{
						query = "SELECT * FROM [Angebotene Artikel] WHERE [Nr] = @Nr";
					}
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Nr", Nr);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetOriginalByAngeboteNr(int OrderId)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId and [PositionZUEDI] is null";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetOriginalByAngeboteNr(int OrderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				string query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId and [PositionZUEDI] is null";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static int GetNextPositionNumberForOrder(int orderId)
		{
			int nextPositionNumber;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select max(position) + 1 nextPosition from [angebotene Artikel] WHERE [Angebot-Nr] = @OrderId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("OrderId", orderId);

				nextPositionNumber = int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int nextPosition) ? nextPosition : 0;
			}
			return nextPositionNumber;
		}
		public static List<AngeboteneArtikelEntity> GetSplitPositionByAngeboteNr(int orderId)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId and [PositionZUEDI] is not null";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("OrderId", orderId);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetSplitPositionByAngeboteNr(int orderId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				string query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId and [PositionZUEDI] is not null";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("OrderId", orderId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetSplitPositionsALL(int id)
		{
			DataTable dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Angebotene Artikel] WHERE [PositionZUEDI]=@id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetSplitPositions(int id)
		{
			DataTable dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Angebotene Artikel] WHERE [PositionZUEDI]=@id AND Anzahl > 0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static List<AngeboteneArtikelEntity> GetSplitPositions(int id, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			DataTable dataTable = new DataTable();
			string query = "SELECT * FROM [Angebotene Artikel] WHERE [PositionZUEDI]=@id AND Anzahl > 0";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static int SoftDelete(int nr)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [Angebotene Artikel] SET [Anzahl]=@quantity, [Aktuelle Anzahl]=@quantity WHERE [Nr]=@Nr ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);
				sqlCommand.Parameters.AddWithValue("quantity", 0);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int GetMaxPositionNumberByOrderId(int angebotNr)
		{
			int response;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT max(position) nextPosition FROM [angebotene Artikel] WHERE [Angebot-Nr]=@angebotNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

				response = int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int nextPosition) ? nextPosition : 0;
			}

			return response;
		}
		public static int GetMaxPositionNumberByOrderId(int angebotNr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "SELECT max(position) nextPosition FROM [angebotene Artikel] WHERE [Angebot-Nr]=@angebotNr";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

			return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out int nextPosition) ? nextPosition : 0;

		}
		public static List<KeyValuePair<int, decimal>> GetKundenNrSumGesamtpreis(List<string> typs,
			List<int> kundenNrs,
			bool? gebucht,
			bool? erledigt,
			bool? erledigtPos,
			DateTime? angeboteDatumFrom,
			DateTime? angeboteDatumTo,
			DateTime? lieferterminFrom,
			DateTime? lieferterminTo)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					string selectAngeboteNrsQuery = "SELECT [Nr] FROM [Angebote] ";
					{
						bool isFirstCondition = true;

						if(typs != null && typs.Count > 0)
						{
							var formatedTyps = new List<string>();
							typs.ForEach(e => formatedTyps.Add($"'{e}'"));

							selectAngeboteNrsQuery += $" {(isFirstCondition ? " WHERE " : " AND ")} [Typ] IN ({string.Join(", ", formatedTyps)}) ";
							isFirstCondition = false;
						}

						if(kundenNrs != null && kundenNrs.Count > 0)
						{
							selectAngeboteNrsQuery += $" {(isFirstCondition ? " WHERE " : " AND ")} [Kunden-Nr] IN ({string.Join(", ", kundenNrs)}) ";
							isFirstCondition = false;
						}

						if(gebucht.HasValue)
						{
							selectAngeboteNrsQuery += $" {(isFirstCondition ? " WHERE " : " AND ")} [gebucht]=@gebucht ";
							sqlCommand.Parameters.AddWithValue("gebucht", gebucht.Value);
							isFirstCondition = false;
						}

						if(erledigt.HasValue)
						{
							selectAngeboteNrsQuery += $" {(isFirstCondition ? " WHERE " : " AND ")} [erledigt]=@erledigt ";
							sqlCommand.Parameters.AddWithValue("erledigt", erledigt.Value);
							isFirstCondition = false;
						}

						if(angeboteDatumFrom.HasValue && angeboteDatumTo.HasValue)
						{
							selectAngeboteNrsQuery += $" {(isFirstCondition ? " WHERE " : " AND ")} [Datum] >= @datumFrom AND [Datum] < @datumTo ";
							sqlCommand.Parameters.AddWithValue("datumFrom", angeboteDatumFrom.Value);
							sqlCommand.Parameters.AddWithValue("datumTo", angeboteDatumTo.Value);
							isFirstCondition = false;
						}
					}

					string mainQuery = " SELECT [Angebote].[Kunden-Nr], Sum([angebotene Artikel].Gesamtpreis) AS [TotalAmount] "
						+ " FROM [angebotene Artikel] "
						+ " INNER JOIN [Angebote] ON  [Angebote].Nr = [angebotene Artikel].[Angebot-Nr] "
						+ $" WHERE [angebotene Artikel].[Angebot-Nr] IN ({selectAngeboteNrsQuery}) ";

					if(lieferterminFrom.HasValue && lieferterminTo.HasValue)
					{
						mainQuery += $" AND [angebotene Artikel].[Liefertermin] >= @lieferterminFrom AND [angebotene Artikel].[Liefertermin] < @lieferterminTo ";
						sqlCommand.Parameters.AddWithValue("lieferterminFrom", lieferterminFrom.Value);
						sqlCommand.Parameters.AddWithValue("lieferterminTo", lieferterminTo.Value);
					}

					if(erledigtPos.HasValue)
					{
						mainQuery += $" AND [angebotene Artikel].erledigt_pos = @erledigtPos ";
						sqlCommand.Parameters.AddWithValue("erledigtPos", erledigtPos.Value);
					}

					mainQuery += " GROUP BY [Angebote].[Kunden-Nr] ";

					sqlCommand.CommandText = mainQuery;
					sqlCommand.Connection = sqlConnection;
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
			}

			if(dataTable.Rows.Count == 0)
			{
				return new List<KeyValuePair<int, decimal>>();
			}

			var response = new List<KeyValuePair<int, decimal>>();
			foreach(DataRow dataRow in dataTable.Rows)
			{
				if(dataRow["Kunden-Nr"] == System.DBNull.Value)
				{
					continue;
				}

				var kundenNr = Convert.ToInt32(dataRow["Kunden-Nr"]);
				var totalAmount = (dataRow["TotalAmount"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["TotalAmount"]);

				response.Add(new KeyValuePair<int, decimal>(kundenNr, totalAmount));
			}
			return response;
		}

		public static int UpdateFertigungsnummer(int nr, float fertigungsnummer)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [Angebotene Artikel] SET "
					+ " [Fertigungsnummer]=@fertigungsnummer "
					+ " WHERE [Nr]=@nr ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nr", nr);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", fertigungsnummer);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int UpdateFertigungsnummer(int nr, float fertigungsnummer, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = " UPDATE [Angebotene Artikel] SET "
					+ " [Fertigungsnummer]=@fertigungsnummer "
					+ " WHERE [Nr]=@nr ";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("nr", nr);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", fertigungsnummer);

			return sqlCommand.ExecuteNonQuery();
		}
		public static int UpdatePrimaryPosition(int orderId, int position, int article, int primaryPositionNr, int PLACEHOLDER)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = " UPDATE [Angebotene Artikel] SET "
					+ " [PositionZUEDI]=@primaryPositionNr "
					+ " WHERE [Angebot-Nr]=@orderId AND [Position]=@position AND [Artikel-Nr]=@article AND [PositionZUEDI]=@PLACEHOLDER";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("position", position);
				sqlCommand.Parameters.AddWithValue("article", article);
				sqlCommand.Parameters.AddWithValue("primaryPositionNr", primaryPositionNr);
				sqlCommand.Parameters.AddWithValue("PLACEHOLDER", PLACEHOLDER);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int UpdatePrimaryPosition(int orderId, int position, int article, int primaryPositionNr, int PLACEHOLDER, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = -1;

			string query = " UPDATE [Angebotene Artikel] SET "
				+ " [PositionZUEDI]=@primaryPositionNr "
				+ " WHERE [Angebot-Nr]=@orderId AND [Position]=@position AND [Artikel-Nr]=@article AND [PositionZUEDI]=@PLACEHOLDER";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("orderId", orderId);
			sqlCommand.Parameters.AddWithValue("position", position);
			sqlCommand.Parameters.AddWithValue("article", article);
			sqlCommand.Parameters.AddWithValue("primaryPositionNr", primaryPositionNr);
			sqlCommand.Parameters.AddWithValue("PLACEHOLDER", PLACEHOLDER);

			response = sqlCommand.ExecuteNonQuery();

			return response;
		}
		public static List<AngeboteneArtikelEntity> GetByAngeboteNrAndPosition(int id, int OrderId, int position)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM [Angebotene Artikel] WHERE Nr<>@id AND [Angebot-Nr] = @OrderId AND [Position]=@position";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("id", id);
					sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
					sqlCommand.Parameters.AddWithValue("position", position);
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static int DeleteByOrder(int nr)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [Angebotene Artikel] WHERE [Angebot-Nr]=@Nr ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int DeleteByOrder(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [Angebotene Artikel] WHERE [Angebot-Nr]=@Nr ";

			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			return sqlCommand.ExecuteNonQuery();
		}
		public static AngeboteneArtikelEntity GetByFertigungsnummer(int Fertigungsnummer)
		{
			DataTable dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [Angebotene Artikel] WHERE [Fertigungsnummer]=@Fertigungsnummer";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", Fertigungsnummer);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new AngeboteneArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static IEnumerable<AngeboteneArtikelEntity> GetByFertigungsnummers(IEnumerable<int> Fertigungsnummers)
		{
			if(Fertigungsnummers == null || Fertigungsnummers.Count() <= 0)
			{
				return null;
			}
			DataTable dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Angebotene Artikel] WHERE [Fertigungsnummer] IN ({string.Join(",", Fertigungsnummers)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			}
		}
		public static int DeleteByAngeboteNr(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [Angebotene Artikel] WHERE [Angebot-Nr]=@Nr ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", id);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}

		#region Blanket
		public static List<AngeboteneArtikelEntity> GetAbByRahmenPosition(int RAposId)
		{
			try
			{

				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM [Angebotene Artikel] p JOIN [Angebote] a on a.[Nr]=p.[Angebot-Nr] WHERE a.[Typ]='Auftragsbestätigung' AND [AB Pos zu RA Pos]=@Id";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("Id", RAposId);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyRahmenPositions(List<int> RAposIds)
		{
			try
			{
				if(RAposIds == null || RAposIds.Count == 0)
					return null;
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [Angebotene Artikel] WHERE [AB Pos zu RA Pos] IN ({string.Join(",", RAposIds)})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyABLSPositions(List<int> ABLSposIds)
		{
			try
			{
				if(ABLSposIds == null || ABLSposIds.Count == 0)
					return null;
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [Angebotene Artikel] WHERE [LS Pos zu AB Pos] IN ({string.Join(",", ABLSposIds)})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyRechnungPositions(List<int> REposIds)
		{
			try
			{
				if(REposIds == null || REposIds.Count == 0)
					return null;
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [Angebotene Artikel] WHERE [RE Pos zu GS Pos] IN ({string.Join(",", REposIds)})";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyAngeboteNrs(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<AngeboteneArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getbyAngeboteNrs(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getbyAngeboteNrs(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getbyAngeboteNrs(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<AngeboteneArtikelEntity>();
		}
		private static List<AngeboteneArtikelEntity> getbyAngeboteNrs(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand("", sqlConnection);

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			}
			return new List<AngeboteneArtikelEntity>();
		}
		public static List<AngeboteneArtikelEntity> GetbyAngeboteNrs(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<AngeboteneArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getbyAngeboteNrs(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getbyAngeboteNrs(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					result.AddRange(getbyAngeboteNrs(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return result;
			}
			return new List<AngeboteneArtikelEntity>();
		}
		private static List<AngeboteneArtikelEntity> getbyAngeboteNrs(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				DataTable dt = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN (" + queryIds + ")";

				new SqlDataAdapter(sqlCommand).Fill(dt);
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			}
			return new List<AngeboteneArtikelEntity>();
		}
		public static List<AngeboteneArtikelEntity> GetClosedByRA(int orderId)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId and [AB_Pos_zu_BV_Pos]>0 AND Anzahl <= 0";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("OrderId", orderId);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetABBeforPreisUpdate(int RahmenNr, int PositionNr, DateTime DateFin)
		{
			try
			{
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = @"SELECT * FROM [Angebotene Artikel] WHERE 
                                   [AB Pos zu RA Pos]=@PositionNr
                                   AND [Liefertermin]>=@DateFin
                                   AND [Geliefert]=0";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					//sqlCommand.Parameters.AddWithValue("RahmenNr", RahmenNr);
					sqlCommand.Parameters.AddWithValue("PositionNr", PositionNr);
					sqlCommand.Parameters.AddWithValue("DateFin", DateFin);

					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetABBeforPreisUpdate(int RahmenNr, int PositionNr, DateTime DateFin, SqlConnection connection, SqlTransaction transaction)
		{
			try
			{
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				string query = @"SELECT * FROM [Angebotene Artikel] WHERE 
                                   [AB Pos zu RA Pos]=@PositionNr
                                   AND [Liefertermin]>=@DateFin
                                   AND [Geliefert]=0";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					sqlCommand.Parameters.AddWithValue("PositionNr", PositionNr);
					sqlCommand.Parameters.AddWithValue("DateFin", DateFin);

					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetOpenByRahmen(List<int> orderIds)
		{
			if(orderIds == null || orderIds.Count <= 0)
			{
				return null;
			}

			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] IN ({(string.Join(",", orderIds))}) AND Anzahl>0";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> GetLSLinkedToAbs(List<int> Nrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select * from [angebotene Artikel] where [LS Pos zu AB Pos] IN ({string.Join(",", Nrs)}) ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<AngeboteneArtikelEntity> GetOpenAbWithRaByArticleId(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
			{
				return new List<AngeboteneArtikelEntity>();
			}
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = $"SELECT * FROM [Angebotene Artikel] WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)}) AND IsNULL([AB Pos zu RA Pos],0)>0 AND Anzahl>0 ";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		#endregion
		#endregion Default Methods

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebotene Artikel] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Angebotene Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Angebotene Artikel] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Angebotene Artikel] ([AB Pos zu BV Pos],[AB Pos zu RA Pos],[Abladestelle],[Aktuelle Anzahl],[AnfangLagerBestand],[Angebot-Nr],[Anzahl],[Artikel-Nr],[Auswahl],[Bemerkungsfeld1],[Bemerkungsfeld2],[Bestellnummer],[Bezeichnung1],[Bezeichnung2],[Bezeichnung2_Kunde],[Bezeichnung3],[CSInterneBemerkung],[DEL],[DEL fixiert],[EDI_Historie_Nr],[EDI_PREIS_KUNDE],[EDI_PREISEINHEIT],[EDI_Quantity_Ordered],[Einheit],[EinzelCu-Gewicht],[Einzelkupferzuschlag],[Einzelpreis],[EKPreise_Fix],[EndeLagerBestand],[erledigt_pos],[Fertigungsnummer],[FM],[FM_Einzelpreis],[FM_Gesamtpreis],[Freies_Format_EDI],[Geliefert],[Gepackt_von],[Gepackt_Zeitpunkt],[GesamtCu-Gewicht],[Gesamtkupferzuschlag],[Gesamtpreis],[GSExternComment],[GSInternComment],[GSWithoutCopper],[In Bearbeitung],[Index_Kunde],[Index_Kunde_datum],[KB Pos zu BV Pos],[KB Pos zu RA Pos],[Kupferbasis],[Lagerbewegung],[Lagerbewegung_rückgängig],[Lagerort_id],[Langtext],[Langtext_drucken],[Lieferanweisung (P_FTXDIN_TEXT)],[Liefertermin],[Löschen],[LS Pos zu AB Pos],[LS Pos zu KB Pos],[LS_von_Versand_gedruckt],[OriginalAnzahl],[Packinfo_von_Lager],[Packstatus],[Position],[PositionZUEDI],[POSTEXT],[Preis_ausweisen],[Preiseinheit],[Preisgruppe],[RA Pos zu BV Pos],[RA_Abgerufen],[RA_Offen],[RA_OriginalAnzahl],[Rabatt],[RE Pos zu GS Pos],[RP],[schriftart],[Seriennummern_drucken],[sortierung],[Stückliste],[Stückliste_drucken],[Summenberechnung],[termin_eingehalten],[Typ],[USt],[VDA_gedruckt],[Versand_gedruckt],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Versanddienstleister],[Versandinfo_von_CS],[Versandinfo_von_Lager],[Versandnummer],[Versandstatus],[VKEinzelpreis],[VK-Festpreis],[VKGesamtpreis],[Wunschtermin],[Zeichnungsnummer],[Zuschlag_VK],[zwischensumme]) OUTPUT INSERTED.[Nr] VALUES (@AB_Pos_zu_BV_Pos,@AB_Pos_zu_RA_Pos,@Abladestelle,@Aktuelle_Anzahl,@AnfangLagerBestand,@Angebot_Nr,@Anzahl,@Artikel_Nr,@Auswahl,@Bemerkungsfeld1,@Bemerkungsfeld2,@Bestellnummer,@Bezeichnung1,@Bezeichnung2,@Bezeichnung2_Kunde,@Bezeichnung3,@CSInterneBemerkung,@DEL,@DEL_fixiert,@EDI_Historie_Nr,@EDI_PREIS_KUNDE,@EDI_PREISEINHEIT,@EDI_Quantity_Ordered,@Einheit,@EinzelCu_Gewicht,@Einzelkupferzuschlag,@Einzelpreis,@EKPreise_Fix,@EndeLagerBestand,@erledigt_pos,@Fertigungsnummer,@FM,@FM_Einzelpreis,@FM_Gesamtpreis,@Freies_Format_EDI,@Geliefert,@Gepackt_von,@Gepackt_Zeitpunkt,@GesamtCu_Gewicht,@Gesamtkupferzuschlag,@Gesamtpreis,@GSExternComment,@GSInternComment,@GSWithoutCopper,@In_Bearbeitung,@Index_Kunde,@Index_Kunde_datum,@KB_Pos_zu_BV_Pos,@KB_Pos_zu_RA_Pos,@Kupferbasis,@Lagerbewegung,@Lagerbewegung_ruckgangig,@Lagerort_id,@Langtext,@Langtext_drucken,@Lieferanweisung__P_FTXDIN_TEXT_,@Liefertermin,@Loschen,@LS_Pos_zu_AB_Pos,@LS_Pos_zu_KB_Pos,@LS_von_Versand_gedruckt,@OriginalAnzahl,@Packinfo_von_Lager,@Packstatus,@Position,@PositionZUEDI,@POSTEXT,@Preis_ausweisen,@Preiseinheit,@Preisgruppe,@RA_Pos_zu_BV_Pos,@RA_Abgerufen,@RA_Offen,@RA_OriginalAnzahl,@Rabatt,@RE_Pos_zu_GS_Pos,@RP,@schriftart,@Seriennummern_drucken,@sortierung,@Stuckliste,@Stuckliste_drucken,@Summenberechnung,@termin_eingehalten,@Typ,@USt,@VDA_gedruckt,@Versand_gedruckt,@Versandarten_Auswahl,@Versanddatum_Auswahl,@Versanddienstleister,@Versandinfo_von_CS,@Versandinfo_von_Lager,@Versandnummer,@Versandstatus,@VKEinzelpreis,@VK_Festpreis,@VKGesamtpreis,@Wunschtermin,@Zeichnungsnummer,@Zuschlag_VK,@zwischensumme); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos", item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos", item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1", item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
			sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2", item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde", item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
			sqlCommand.Parameters.AddWithValue("Bezeichnung3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("CSInterneBemerkung", item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
			sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr", item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
			sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE", item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
			sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT", item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
			sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered", item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht", item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
			sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag", item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EKPreise_Fix", item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("FM", item.FM == null ? (object)DBNull.Value : item.FM);
			sqlCommand.Parameters.AddWithValue("FM_Einzelpreis", item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
			sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis", item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("Freies_Format_EDI", item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
			sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
			sqlCommand.Parameters.AddWithValue("Gepackt_von", item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
			sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt", item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
			sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht", item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
			sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag", item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("GSExternComment", item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
			sqlCommand.Parameters.AddWithValue("GSInternComment", item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
			sqlCommand.Parameters.AddWithValue("GSWithoutCopper", item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos", item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos", item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Lagerbewegung", item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
			sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig", item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken", item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
			sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_", item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos", item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
			sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos", item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
			sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt", item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
			sqlCommand.Parameters.AddWithValue("OriginalAnzahl", item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager", item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
			sqlCommand.Parameters.AddWithValue("Packstatus", item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("PositionZUEDI", item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
			sqlCommand.Parameters.AddWithValue("POSTEXT", item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
			sqlCommand.Parameters.AddWithValue("Preis_ausweisen", item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos", item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("RA_Abgerufen", item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
			sqlCommand.Parameters.AddWithValue("RA_Offen", item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
			sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl", item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos", item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
			sqlCommand.Parameters.AddWithValue("RP", item.RP == null ? (object)DBNull.Value : item.RP);
			sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
			sqlCommand.Parameters.AddWithValue("Seriennummern_drucken", item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
			sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
			sqlCommand.Parameters.AddWithValue("Stuckliste_drucken", item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
			sqlCommand.Parameters.AddWithValue("Summenberechnung", item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("VDA_gedruckt", item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
			sqlCommand.Parameters.AddWithValue("Versand_gedruckt", item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
			sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddienstleister", item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
			sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS", item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
			sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager", item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
			sqlCommand.Parameters.AddWithValue("Versandnummer", item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
			sqlCommand.Parameters.AddWithValue("Versandstatus", item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
			sqlCommand.Parameters.AddWithValue("VKEinzelpreis", item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("VKGesamtpreis", item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
			sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
			sqlCommand.Parameters.AddWithValue("zwischensumme", item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 104; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Angebotene Artikel] ([AB Pos zu BV Pos],[AB Pos zu RA Pos],[Abladestelle],[Aktuelle Anzahl],[AnfangLagerBestand],[Angebot-Nr],[Anzahl],[Artikel-Nr],[Auswahl],[Bemerkungsfeld1],[Bemerkungsfeld2],[Bestellnummer],[Bezeichnung1],[Bezeichnung2],[Bezeichnung2_Kunde],[Bezeichnung3],[CSInterneBemerkung],[DEL],[DEL fixiert],[EDI_Historie_Nr],[EDI_PREIS_KUNDE],[EDI_PREISEINHEIT],[EDI_Quantity_Ordered],[Einheit],[EinzelCu-Gewicht],[Einzelkupferzuschlag],[Einzelpreis],[EKPreise_Fix],[EndeLagerBestand],[erledigt_pos],[Fertigungsnummer],[FM],[FM_Einzelpreis],[FM_Gesamtpreis],[Freies_Format_EDI],[Geliefert],[Gepackt_von],[Gepackt_Zeitpunkt],[GesamtCu-Gewicht],[Gesamtkupferzuschlag],[Gesamtpreis],[GSExternComment],[GSInternComment],[GSWithoutCopper],[In Bearbeitung],[Index_Kunde],[Index_Kunde_datum],[KB Pos zu BV Pos],[KB Pos zu RA Pos],[Kupferbasis],[Lagerbewegung],[Lagerbewegung_rückgängig],[Lagerort_id],[Langtext],[Langtext_drucken],[Lieferanweisung (P_FTXDIN_TEXT)],[Liefertermin],[Löschen],[LS Pos zu AB Pos],[LS Pos zu KB Pos],[LS_von_Versand_gedruckt],[OriginalAnzahl],[Packinfo_von_Lager],[Packstatus],[Position],[PositionZUEDI],[POSTEXT],[Preis_ausweisen],[Preiseinheit],[Preisgruppe],[RA Pos zu BV Pos],[RA_Abgerufen],[RA_Offen],[RA_OriginalAnzahl],[Rabatt],[RE Pos zu GS Pos],[RP],[schriftart],[Seriennummern_drucken],[sortierung],[Stückliste],[Stückliste_drucken],[Summenberechnung],[termin_eingehalten],[Typ],[USt],[VDA_gedruckt],[Versand_gedruckt],[Versandarten_Auswahl],[Versanddatum_Auswahl],[Versanddienstleister],[Versandinfo_von_CS],[Versandinfo_von_Lager],[Versandnummer],[Versandstatus],[VKEinzelpreis],[VK-Festpreis],[VKGesamtpreis],[Wunschtermin],[Zeichnungsnummer],[Zuschlag_VK],[zwischensumme]) VALUES ( "

						+ "@AB_Pos_zu_BV_Pos" + i + ","
						+ "@AB_Pos_zu_RA_Pos" + i + ","
						+ "@Abladestelle" + i + ","
						+ "@Aktuelle_Anzahl" + i + ","
						+ "@AnfangLagerBestand" + i + ","
						+ "@Angebot_Nr" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Auswahl" + i + ","
						+ "@Bemerkungsfeld1" + i + ","
						+ "@Bemerkungsfeld2" + i + ","
						+ "@Bestellnummer" + i + ","
						+ "@Bezeichnung1" + i + ","
						+ "@Bezeichnung2" + i + ","
						+ "@Bezeichnung2_Kunde" + i + ","
						+ "@Bezeichnung3" + i + ","
						+ "@CSInterneBemerkung" + i + ","
						+ "@DEL" + i + ","
						+ "@DEL_fixiert" + i + ","
						+ "@EDI_Historie_Nr" + i + ","
						+ "@EDI_PREIS_KUNDE" + i + ","
						+ "@EDI_PREISEINHEIT" + i + ","
						+ "@EDI_Quantity_Ordered" + i + ","
						+ "@Einheit" + i + ","
						+ "@EinzelCu_Gewicht" + i + ","
						+ "@Einzelkupferzuschlag" + i + ","
						+ "@Einzelpreis" + i + ","
						+ "@EKPreise_Fix" + i + ","
						+ "@EndeLagerBestand" + i + ","
						+ "@erledigt_pos" + i + ","
						+ "@Fertigungsnummer" + i + ","
						+ "@FM" + i + ","
						+ "@FM_Einzelpreis" + i + ","
						+ "@FM_Gesamtpreis" + i + ","
						+ "@Freies_Format_EDI" + i + ","
						+ "@Geliefert" + i + ","
						+ "@Gepackt_von" + i + ","
						+ "@Gepackt_Zeitpunkt" + i + ","
						+ "@GesamtCu_Gewicht" + i + ","
						+ "@Gesamtkupferzuschlag" + i + ","
						+ "@Gesamtpreis" + i + ","
						+ "@GSExternComment" + i + ","
						+ "@GSInternComment" + i + ","
						+ "@GSWithoutCopper" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@Index_Kunde_datum" + i + ","
						+ "@KB_Pos_zu_BV_Pos" + i + ","
						+ "@KB_Pos_zu_RA_Pos" + i + ","
						+ "@Kupferbasis" + i + ","
						+ "@Lagerbewegung" + i + ","
						+ "@Lagerbewegung_ruckgangig" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Langtext" + i + ","
						+ "@Langtext_drucken" + i + ","
						+ "@Lieferanweisung__P_FTXDIN_TEXT_" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Loschen" + i + ","
						+ "@LS_Pos_zu_AB_Pos" + i + ","
						+ "@LS_Pos_zu_KB_Pos" + i + ","
						+ "@LS_von_Versand_gedruckt" + i + ","
						+ "@OriginalAnzahl" + i + ","
						+ "@Packinfo_von_Lager" + i + ","
						+ "@Packstatus" + i + ","
						+ "@Position" + i + ","
						+ "@PositionZUEDI" + i + ","
						+ "@POSTEXT" + i + ","
						+ "@Preis_ausweisen" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@Preisgruppe" + i + ","
						+ "@RA_Pos_zu_BV_Pos" + i + ","
						+ "@RA_Abgerufen" + i + ","
						+ "@RA_Offen" + i + ","
						+ "@RA_OriginalAnzahl" + i + ","
						+ "@Rabatt" + i + ","
						+ "@RE_Pos_zu_GS_Pos" + i + ","
						+ "@RP" + i + ","
						+ "@schriftart" + i + ","
						+ "@Seriennummern_drucken" + i + ","
						+ "@sortierung" + i + ","
						+ "@Stuckliste" + i + ","
						+ "@Stuckliste_drucken" + i + ","
						+ "@Summenberechnung" + i + ","
						+ "@termin_eingehalten" + i + ","
						+ "@Typ" + i + ","
						+ "@USt" + i + ","
						+ "@VDA_gedruckt" + i + ","
						+ "@Versand_gedruckt" + i + ","
						+ "@Versandarten_Auswahl" + i + ","
						+ "@Versanddatum_Auswahl" + i + ","
						+ "@Versanddienstleister" + i + ","
						+ "@Versandinfo_von_CS" + i + ","
						+ "@Versandinfo_von_Lager" + i + ","
						+ "@Versandnummer" + i + ","
						+ "@Versandstatus" + i + ","
						+ "@VKEinzelpreis" + i + ","
						+ "@VK_Festpreis" + i + ","
						+ "@VKGesamtpreis" + i + ","
						+ "@Wunschtermin" + i + ","
						+ "@Zeichnungsnummer" + i + ","
						+ "@Zuschlag_VK" + i + ","
						+ "@zwischensumme" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos" + i, item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos" + i, item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1" + i, item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2" + i, item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde" + i, item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
					sqlCommand.Parameters.AddWithValue("Bezeichnung3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("CSInterneBemerkung" + i, item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
					sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr" + i, item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
					sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE" + i, item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
					sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT" + i, item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
					sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered" + i, item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht" + i, item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
					sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag" + i, item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EKPreise_Fix" + i, item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("FM" + i, item.FM == null ? (object)DBNull.Value : item.FM);
					sqlCommand.Parameters.AddWithValue("FM_Einzelpreis" + i, item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
					sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis" + i, item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Freies_Format_EDI" + i, item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
					sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Gepackt_von" + i, item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
					sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt" + i, item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
					sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht" + i, item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
					sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag" + i, item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GSExternComment" + i, item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
					sqlCommand.Parameters.AddWithValue("GSInternComment" + i, item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
					sqlCommand.Parameters.AddWithValue("GSWithoutCopper" + i, item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos" + i, item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos" + i, item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung" + i, item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig" + i, item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken" + i, item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
					sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_" + i, item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos" + i, item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos" + i, item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
					sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt" + i, item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("OriginalAnzahl" + i, item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager" + i, item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Packstatus" + i, item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("PositionZUEDI" + i, item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
					sqlCommand.Parameters.AddWithValue("POSTEXT" + i, item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
					sqlCommand.Parameters.AddWithValue("Preis_ausweisen" + i, item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos" + i, item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("RA_Abgerufen" + i, item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RA_Offen" + i, item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
					sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl" + i, item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos" + i, item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
					sqlCommand.Parameters.AddWithValue("RP" + i, item.RP == null ? (object)DBNull.Value : item.RP);
					sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Seriennummern_drucken" + i, item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
					sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
					sqlCommand.Parameters.AddWithValue("Stuckliste_drucken" + i, item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
					sqlCommand.Parameters.AddWithValue("Summenberechnung" + i, item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("VDA_gedruckt" + i, item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versand_gedruckt" + i, item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddienstleister" + i, item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS" + i, item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager" + i, item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Versandnummer" + i, item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
					sqlCommand.Parameters.AddWithValue("Versandstatus" + i, item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
					sqlCommand.Parameters.AddWithValue("VKEinzelpreis" + i, item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("VKGesamtpreis" + i, item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
					sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					sqlCommand.Parameters.AddWithValue("zwischensumme" + i, item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Angebotene Artikel] SET [AB Pos zu BV Pos]=@AB_Pos_zu_BV_Pos, [AB Pos zu RA Pos]=@AB_Pos_zu_RA_Pos, [Abladestelle]=@Abladestelle, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Angebot-Nr]=@Angebot_Nr, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Auswahl]=@Auswahl, [Bemerkungsfeld1]=@Bemerkungsfeld1, [Bemerkungsfeld2]=@Bemerkungsfeld2, [Bestellnummer]=@Bestellnummer, [Bezeichnung1]=@Bezeichnung1, [Bezeichnung2]=@Bezeichnung2, [Bezeichnung2_Kunde]=@Bezeichnung2_Kunde, [Bezeichnung3]=@Bezeichnung3, [CSInterneBemerkung]=@CSInterneBemerkung, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [EDI_Historie_Nr]=@EDI_Historie_Nr, [EDI_PREIS_KUNDE]=@EDI_PREIS_KUNDE, [EDI_PREISEINHEIT]=@EDI_PREISEINHEIT, [EDI_Quantity_Ordered]=@EDI_Quantity_Ordered, [Einheit]=@Einheit, [EinzelCu-Gewicht]=@EinzelCu_Gewicht, [Einzelkupferzuschlag]=@Einzelkupferzuschlag, [Einzelpreis]=@Einzelpreis, [EKPreise_Fix]=@EKPreise_Fix, [EndeLagerBestand]=@EndeLagerBestand, [erledigt_pos]=@erledigt_pos, [Fertigungsnummer]=@Fertigungsnummer, [FM]=@FM, [FM_Einzelpreis]=@FM_Einzelpreis, [FM_Gesamtpreis]=@FM_Gesamtpreis, [Freies_Format_EDI]=@Freies_Format_EDI, [Geliefert]=@Geliefert, [Gepackt_von]=@Gepackt_von, [Gepackt_Zeitpunkt]=@Gepackt_Zeitpunkt, [GesamtCu-Gewicht]=@GesamtCu_Gewicht, [Gesamtkupferzuschlag]=@Gesamtkupferzuschlag, [Gesamtpreis]=@Gesamtpreis, [GSExternComment]=@GSExternComment, [GSInternComment]=@GSInternComment, [GSWithoutCopper]=@GSWithoutCopper, [In Bearbeitung]=@In_Bearbeitung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_datum]=@Index_Kunde_datum, [KB Pos zu BV Pos]=@KB_Pos_zu_BV_Pos, [KB Pos zu RA Pos]=@KB_Pos_zu_RA_Pos, [Kupferbasis]=@Kupferbasis, [Lagerbewegung]=@Lagerbewegung, [Lagerbewegung_rückgängig]=@Lagerbewegung_ruckgangig, [Lagerort_id]=@Lagerort_id, [Langtext]=@Langtext, [Langtext_drucken]=@Langtext_drucken, [Lieferanweisung (P_FTXDIN_TEXT)]=@Lieferanweisung__P_FTXDIN_TEXT_, [Liefertermin]=@Liefertermin, [Löschen]=@Loschen, [LS Pos zu AB Pos]=@LS_Pos_zu_AB_Pos, [LS Pos zu KB Pos]=@LS_Pos_zu_KB_Pos, [LS_von_Versand_gedruckt]=@LS_von_Versand_gedruckt, [OriginalAnzahl]=@OriginalAnzahl, [Packinfo_von_Lager]=@Packinfo_von_Lager, [Packstatus]=@Packstatus, [Position]=@Position, [PositionZUEDI]=@PositionZUEDI, [POSTEXT]=@POSTEXT, [Preis_ausweisen]=@Preis_ausweisen, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [RA Pos zu BV Pos]=@RA_Pos_zu_BV_Pos, [RA_Abgerufen]=@RA_Abgerufen, [RA_Offen]=@RA_Offen, [RA_OriginalAnzahl]=@RA_OriginalAnzahl, [Rabatt]=@Rabatt, [RE Pos zu GS Pos]=@RE_Pos_zu_GS_Pos, [RP]=@RP, [schriftart]=@schriftart, [Seriennummern_drucken]=@Seriennummern_drucken, [sortierung]=@sortierung, [Stückliste]=@Stuckliste, [Stückliste_drucken]=@Stuckliste_drucken, [Summenberechnung]=@Summenberechnung, [termin_eingehalten]=@termin_eingehalten, [Typ]=@Typ, [USt]=@USt, [VDA_gedruckt]=@VDA_gedruckt, [Versand_gedruckt]=@Versand_gedruckt, [Versandarten_Auswahl]=@Versandarten_Auswahl, [Versanddatum_Auswahl]=@Versanddatum_Auswahl, [Versanddienstleister]=@Versanddienstleister, [Versandinfo_von_CS]=@Versandinfo_von_CS, [Versandinfo_von_Lager]=@Versandinfo_von_Lager, [Versandnummer]=@Versandnummer, [Versandstatus]=@Versandstatus, [VKEinzelpreis]=@VKEinzelpreis, [VK-Festpreis]=@VK_Festpreis, [VKGesamtpreis]=@VKGesamtpreis, [Wunschtermin]=@Wunschtermin, [Zeichnungsnummer]=@Zeichnungsnummer, [Zuschlag_VK]=@Zuschlag_VK, [zwischensumme]=@zwischensumme WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos", item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos", item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Angebot_Nr", item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1", item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
			sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2", item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung1", item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2", item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde", item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
			sqlCommand.Parameters.AddWithValue("Bezeichnung3", item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
			sqlCommand.Parameters.AddWithValue("CSInterneBemerkung", item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
			sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr", item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
			sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE", item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
			sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT", item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
			sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered", item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht", item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
			sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag", item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EKPreise_Fix", item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("FM", item.FM == null ? (object)DBNull.Value : item.FM);
			sqlCommand.Parameters.AddWithValue("FM_Einzelpreis", item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
			sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis", item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("Freies_Format_EDI", item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
			sqlCommand.Parameters.AddWithValue("Geliefert", item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
			sqlCommand.Parameters.AddWithValue("Gepackt_von", item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
			sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt", item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
			sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht", item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
			sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag", item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("GSExternComment", item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
			sqlCommand.Parameters.AddWithValue("GSInternComment", item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
			sqlCommand.Parameters.AddWithValue("GSWithoutCopper", item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos", item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos", item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Lagerbewegung", item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
			sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig", item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken", item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
			sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_", item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos", item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
			sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos", item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
			sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt", item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
			sqlCommand.Parameters.AddWithValue("OriginalAnzahl", item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager", item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
			sqlCommand.Parameters.AddWithValue("Packstatus", item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("PositionZUEDI", item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
			sqlCommand.Parameters.AddWithValue("POSTEXT", item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
			sqlCommand.Parameters.AddWithValue("Preis_ausweisen", item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos", item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
			sqlCommand.Parameters.AddWithValue("RA_Abgerufen", item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
			sqlCommand.Parameters.AddWithValue("RA_Offen", item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
			sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl", item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos", item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
			sqlCommand.Parameters.AddWithValue("RP", item.RP == null ? (object)DBNull.Value : item.RP);
			sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
			sqlCommand.Parameters.AddWithValue("Seriennummern_drucken", item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
			sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
			sqlCommand.Parameters.AddWithValue("Stuckliste_drucken", item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
			sqlCommand.Parameters.AddWithValue("Summenberechnung", item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
			sqlCommand.Parameters.AddWithValue("termin_eingehalten", item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
			sqlCommand.Parameters.AddWithValue("Typ", item.Typ == null ? (object)DBNull.Value : item.Typ);
			sqlCommand.Parameters.AddWithValue("USt", item.USt == null ? (object)DBNull.Value : item.USt);
			sqlCommand.Parameters.AddWithValue("VDA_gedruckt", item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
			sqlCommand.Parameters.AddWithValue("Versand_gedruckt", item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
			sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl", item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl", item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
			sqlCommand.Parameters.AddWithValue("Versanddienstleister", item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
			sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS", item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
			sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager", item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
			sqlCommand.Parameters.AddWithValue("Versandnummer", item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
			sqlCommand.Parameters.AddWithValue("Versandstatus", item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
			sqlCommand.Parameters.AddWithValue("VKEinzelpreis", item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
			sqlCommand.Parameters.AddWithValue("VKGesamtpreis", item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
			sqlCommand.Parameters.AddWithValue("Wunschtermin", item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
			sqlCommand.Parameters.AddWithValue("zwischensumme", item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 104; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), connection, transaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				//int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Angebotene Artikel] SET "

					+ "[AB Pos zu BV Pos]=@AB_Pos_zu_BV_Pos" + i + ","
					+ "[AB Pos zu RA Pos]=@AB_Pos_zu_RA_Pos" + i + ","
					+ "[Abladestelle]=@Abladestelle" + i + ","
					+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
					+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
					+ "[Angebot-Nr]=@Angebot_Nr" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Auswahl]=@Auswahl" + i + ","
					+ "[Bemerkungsfeld1]=@Bemerkungsfeld1" + i + ","
					+ "[Bemerkungsfeld2]=@Bemerkungsfeld2" + i + ","
					+ "[Bestellnummer]=@Bestellnummer" + i + ","
					+ "[Bezeichnung1]=@Bezeichnung1" + i + ","
					+ "[Bezeichnung2]=@Bezeichnung2" + i + ","
					+ "[Bezeichnung2_Kunde]=@Bezeichnung2_Kunde" + i + ","
					+ "[Bezeichnung3]=@Bezeichnung3" + i + ","
					+ "[CSInterneBemerkung]=@CSInterneBemerkung" + i + ","
					+ "[DEL]=@DEL" + i + ","
					+ "[DEL fixiert]=@DEL_fixiert" + i + ","
					+ "[EDI_Historie_Nr]=@EDI_Historie_Nr" + i + ","
					+ "[EDI_PREIS_KUNDE]=@EDI_PREIS_KUNDE" + i + ","
					+ "[EDI_PREISEINHEIT]=@EDI_PREISEINHEIT" + i + ","
					+ "[EDI_Quantity_Ordered]=@EDI_Quantity_Ordered" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[EinzelCu-Gewicht]=@EinzelCu_Gewicht" + i + ","
					+ "[Einzelkupferzuschlag]=@Einzelkupferzuschlag" + i + ","
					+ "[Einzelpreis]=@Einzelpreis" + i + ","
					+ "[EKPreise_Fix]=@EKPreise_Fix" + i + ","
					+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
					+ "[erledigt_pos]=@erledigt_pos" + i + ","
					+ "[Fertigungsnummer]=@Fertigungsnummer" + i + ","
					+ "[FM]=@FM" + i + ","
					+ "[FM_Einzelpreis]=@FM_Einzelpreis" + i + ","
					+ "[FM_Gesamtpreis]=@FM_Gesamtpreis" + i + ","
					+ "[Freies_Format_EDI]=@Freies_Format_EDI" + i + ","
					+ "[Geliefert]=@Geliefert" + i + ","
					+ "[Gepackt_von]=@Gepackt_von" + i + ","
					+ "[Gepackt_Zeitpunkt]=@Gepackt_Zeitpunkt" + i + ","
					+ "[GesamtCu-Gewicht]=@GesamtCu_Gewicht" + i + ","
					+ "[Gesamtkupferzuschlag]=@Gesamtkupferzuschlag" + i + ","
					+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
					+ "[GSExternComment]=@GSExternComment" + i + ","
					+ "[GSInternComment]=@GSInternComment" + i + ","
					+ "[GSWithoutCopper]=@GSWithoutCopper" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[Index_Kunde_datum]=@Index_Kunde_datum" + i + ","
					+ "[KB Pos zu BV Pos]=@KB_Pos_zu_BV_Pos" + i + ","
					+ "[KB Pos zu RA Pos]=@KB_Pos_zu_RA_Pos" + i + ","
					+ "[Kupferbasis]=@Kupferbasis" + i + ","
					+ "[Lagerbewegung]=@Lagerbewegung" + i + ","
					+ "[Lagerbewegung_rückgängig]=@Lagerbewegung_ruckgangig" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Langtext]=@Langtext" + i + ","
					+ "[Langtext_drucken]=@Langtext_drucken" + i + ","
					+ "[Lieferanweisung (P_FTXDIN_TEXT)]=@Lieferanweisung__P_FTXDIN_TEXT_" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[LS Pos zu AB Pos]=@LS_Pos_zu_AB_Pos" + i + ","
					+ "[LS Pos zu KB Pos]=@LS_Pos_zu_KB_Pos" + i + ","
					+ "[LS_von_Versand_gedruckt]=@LS_von_Versand_gedruckt" + i + ","
					+ "[OriginalAnzahl]=@OriginalAnzahl" + i + ","
					+ "[Packinfo_von_Lager]=@Packinfo_von_Lager" + i + ","
					+ "[Packstatus]=@Packstatus" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[PositionZUEDI]=@PositionZUEDI" + i + ","
					+ "[POSTEXT]=@POSTEXT" + i + ","
					+ "[Preis_ausweisen]=@Preis_ausweisen" + i + ","
					+ "[Preiseinheit]=@Preiseinheit" + i + ","
					+ "[Preisgruppe]=@Preisgruppe" + i + ","
					+ "[RA Pos zu BV Pos]=@RA_Pos_zu_BV_Pos" + i + ","
					+ "[RA_Abgerufen]=@RA_Abgerufen" + i + ","
					+ "[RA_Offen]=@RA_Offen" + i + ","
					+ "[RA_OriginalAnzahl]=@RA_OriginalAnzahl" + i + ","
					+ "[Rabatt]=@Rabatt" + i + ","
					+ "[RE Pos zu GS Pos]=@RE_Pos_zu_GS_Pos" + i + ","
					+ "[RP]=@RP" + i + ","
					+ "[schriftart]=@schriftart" + i + ","
					+ "[Seriennummern_drucken]=@Seriennummern_drucken" + i + ","
					+ "[sortierung]=@sortierung" + i + ","
					+ "[Stückliste]=@Stuckliste" + i + ","
					+ "[Stückliste_drucken]=@Stuckliste_drucken" + i + ","
					+ "[Summenberechnung]=@Summenberechnung" + i + ","
					+ "[termin_eingehalten]=@termin_eingehalten" + i + ","
					+ "[Typ]=@Typ" + i + ","
					+ "[USt]=@USt" + i + ","
					+ "[VDA_gedruckt]=@VDA_gedruckt" + i + ","
					+ "[Versand_gedruckt]=@Versand_gedruckt" + i + ","
					+ "[Versandarten_Auswahl]=@Versandarten_Auswahl" + i + ","
					+ "[Versanddatum_Auswahl]=@Versanddatum_Auswahl" + i + ","
					+ "[Versanddienstleister]=@Versanddienstleister" + i + ","
					+ "[Versandinfo_von_CS]=@Versandinfo_von_CS" + i + ","
					+ "[Versandinfo_von_Lager]=@Versandinfo_von_Lager" + i + ","
					+ "[Versandnummer]=@Versandnummer" + i + ","
					+ "[Versandstatus]=@Versandstatus" + i + ","
					+ "[VKEinzelpreis]=@VKEinzelpreis" + i + ","
					+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
					+ "[VKGesamtpreis]=@VKGesamtpreis" + i + ","
					+ "[Wunschtermin]=@Wunschtermin" + i + ","
					+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
					+ "[Zuschlag_VK]=@Zuschlag_VK" + i + ","
					+ "[zwischensumme]=@zwischensumme" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_BV_Pos" + i, item.ABPoszuBVPos == null ? (object)DBNull.Value : item.ABPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("AB_Pos_zu_RA_Pos" + i, item.ABPoszuRAPos == null ? (object)DBNull.Value : item.ABPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.AktuelleAnzahl == null ? (object)DBNull.Value : item.AktuelleAnzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Angebot_Nr" + i, item.AngebotNr == null ? (object)DBNull.Value : item.AngebotNr);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld1" + i, item.Bemerkungsfeld1 == null ? (object)DBNull.Value : item.Bemerkungsfeld1);
					sqlCommand.Parameters.AddWithValue("Bemerkungsfeld2" + i, item.Bemerkungsfeld2 == null ? (object)DBNull.Value : item.Bemerkungsfeld2);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung1" + i, item.Bezeichnung1 == null ? (object)DBNull.Value : item.Bezeichnung1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2" + i, item.Bezeichnung2 == null ? (object)DBNull.Value : item.Bezeichnung2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung2_Kunde" + i, item.Bezeichnung2_Kunde == null ? (object)DBNull.Value : item.Bezeichnung2_Kunde);
					sqlCommand.Parameters.AddWithValue("Bezeichnung3" + i, item.Bezeichnung3 == null ? (object)DBNull.Value : item.Bezeichnung3);
					sqlCommand.Parameters.AddWithValue("CSInterneBemerkung" + i, item.CSInterneBemerkung == null ? (object)DBNull.Value : item.CSInterneBemerkung);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DELFixiert == null ? (object)DBNull.Value : item.DELFixiert);
					sqlCommand.Parameters.AddWithValue("EDI_Historie_Nr" + i, item.EDI_Historie_Nr == null ? (object)DBNull.Value : item.EDI_Historie_Nr);
					sqlCommand.Parameters.AddWithValue("EDI_PREIS_KUNDE" + i, item.EDI_PREIS_KUNDE == null ? (object)DBNull.Value : item.EDI_PREIS_KUNDE);
					sqlCommand.Parameters.AddWithValue("EDI_PREISEINHEIT" + i, item.EDI_PREISEINHEIT == null ? (object)DBNull.Value : item.EDI_PREISEINHEIT);
					sqlCommand.Parameters.AddWithValue("EDI_Quantity_Ordered" + i, item.EDI_Quantity_Ordered == null ? (object)DBNull.Value : item.EDI_Quantity_Ordered);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EinzelCu_Gewicht" + i, item.EinzelCuGewicht == null ? (object)DBNull.Value : item.EinzelCuGewicht);
					sqlCommand.Parameters.AddWithValue("Einzelkupferzuschlag" + i, item.Einzelkupferzuschlag == null ? (object)DBNull.Value : item.Einzelkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EKPreise_Fix" + i, item.EKPreise_Fix == null ? (object)DBNull.Value : item.EKPreise_Fix);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("FM" + i, item.FM == null ? (object)DBNull.Value : item.FM);
					sqlCommand.Parameters.AddWithValue("FM_Einzelpreis" + i, item.FM_Einzelpreis == null ? (object)DBNull.Value : item.FM_Einzelpreis);
					sqlCommand.Parameters.AddWithValue("FM_Gesamtpreis" + i, item.FM_Gesamtpreis == null ? (object)DBNull.Value : item.FM_Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Freies_Format_EDI" + i, item.Freies_Format_EDI == null ? (object)DBNull.Value : item.Freies_Format_EDI);
					sqlCommand.Parameters.AddWithValue("Geliefert" + i, item.Geliefert == null ? (object)DBNull.Value : item.Geliefert);
					sqlCommand.Parameters.AddWithValue("Gepackt_von" + i, item.Gepackt_von == null ? (object)DBNull.Value : item.Gepackt_von);
					sqlCommand.Parameters.AddWithValue("Gepackt_Zeitpunkt" + i, item.Gepackt_Zeitpunkt == null ? (object)DBNull.Value : item.Gepackt_Zeitpunkt);
					sqlCommand.Parameters.AddWithValue("GesamtCu_Gewicht" + i, item.GesamtCuGewicht == null ? (object)DBNull.Value : item.GesamtCuGewicht);
					sqlCommand.Parameters.AddWithValue("Gesamtkupferzuschlag" + i, item.Gesamtkupferzuschlag == null ? (object)DBNull.Value : item.Gesamtkupferzuschlag);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("GSExternComment" + i, item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
					sqlCommand.Parameters.AddWithValue("GSInternComment" + i, item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
					sqlCommand.Parameters.AddWithValue("GSWithoutCopper" + i, item.GSWithoutCopper == null ? (object)DBNull.Value : item.GSWithoutCopper);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.InBearbeitung == null ? (object)DBNull.Value : item.InBearbeitung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_BV_Pos" + i, item.KBPoszuBVPos == null ? (object)DBNull.Value : item.KBPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("KB_Pos_zu_RA_Pos" + i, item.KBPoszuRAPos == null ? (object)DBNull.Value : item.KBPoszuRAPos);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung" + i, item.Lagerbewegung == null ? (object)DBNull.Value : item.Lagerbewegung);
					sqlCommand.Parameters.AddWithValue("Lagerbewegung_ruckgangig" + i, item.Lagerbewegung_rückgängig == null ? (object)DBNull.Value : item.Lagerbewegung_rückgängig);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken" + i, item.Langtext_drucken == null ? (object)DBNull.Value : item.Langtext_drucken);
					sqlCommand.Parameters.AddWithValue("Lieferanweisung__P_FTXDIN_TEXT_" + i, item.LieferanweisungP_FTXDIN_TEXT == null ? (object)DBNull.Value : item.LieferanweisungP_FTXDIN_TEXT);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_AB_Pos" + i, item.LSPoszuABPos == null ? (object)DBNull.Value : item.LSPoszuABPos);
					sqlCommand.Parameters.AddWithValue("LS_Pos_zu_KB_Pos" + i, item.LSPoszuKBPos == null ? (object)DBNull.Value : item.LSPoszuKBPos);
					sqlCommand.Parameters.AddWithValue("LS_von_Versand_gedruckt" + i, item.LS_von_Versand_gedruckt == null ? (object)DBNull.Value : item.LS_von_Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("OriginalAnzahl" + i, item.OriginalAnzahl == null ? (object)DBNull.Value : item.OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Packinfo_von_Lager" + i, item.Packinfo_von_Lager == null ? (object)DBNull.Value : item.Packinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Packstatus" + i, item.Packstatus == null ? (object)DBNull.Value : item.Packstatus);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("PositionZUEDI" + i, item.PositionZUEDI == null ? (object)DBNull.Value : item.PositionZUEDI);
					sqlCommand.Parameters.AddWithValue("POSTEXT" + i, item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);
					sqlCommand.Parameters.AddWithValue("Preis_ausweisen" + i, item.Preis_ausweisen == null ? (object)DBNull.Value : item.Preis_ausweisen);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("RA_Pos_zu_BV_Pos" + i, item.RAPoszuBVPos == null ? (object)DBNull.Value : item.RAPoszuBVPos);
					sqlCommand.Parameters.AddWithValue("RA_Abgerufen" + i, item.RA_Abgerufen == null ? (object)DBNull.Value : item.RA_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RA_Offen" + i, item.RA_Offen == null ? (object)DBNull.Value : item.RA_Offen);
					sqlCommand.Parameters.AddWithValue("RA_OriginalAnzahl" + i, item.RA_OriginalAnzahl == null ? (object)DBNull.Value : item.RA_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("RE_Pos_zu_GS_Pos" + i, item.REPoszuGSPos == null ? (object)DBNull.Value : item.REPoszuGSPos);
					sqlCommand.Parameters.AddWithValue("RP" + i, item.RP == null ? (object)DBNull.Value : item.RP);
					sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("Seriennummern_drucken" + i, item.Seriennummern_drucken == null ? (object)DBNull.Value : item.Seriennummern_drucken);
					sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stückliste == null ? (object)DBNull.Value : item.Stückliste);
					sqlCommand.Parameters.AddWithValue("Stuckliste_drucken" + i, item.Stückliste_drucken == null ? (object)DBNull.Value : item.Stückliste_drucken);
					sqlCommand.Parameters.AddWithValue("Summenberechnung" + i, item.Summenberechnung == null ? (object)DBNull.Value : item.Summenberechnung);
					sqlCommand.Parameters.AddWithValue("termin_eingehalten" + i, item.termin_eingehalten == null ? (object)DBNull.Value : item.termin_eingehalten);
					sqlCommand.Parameters.AddWithValue("Typ" + i, item.Typ == null ? (object)DBNull.Value : item.Typ);
					sqlCommand.Parameters.AddWithValue("USt" + i, item.USt == null ? (object)DBNull.Value : item.USt);
					sqlCommand.Parameters.AddWithValue("VDA_gedruckt" + i, item.VDA_gedruckt == null ? (object)DBNull.Value : item.VDA_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versand_gedruckt" + i, item.Versand_gedruckt == null ? (object)DBNull.Value : item.Versand_gedruckt);
					sqlCommand.Parameters.AddWithValue("Versandarten_Auswahl" + i, item.Versandarten_Auswahl == null ? (object)DBNull.Value : item.Versandarten_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddatum_Auswahl" + i, item.Versanddatum_Auswahl == null ? (object)DBNull.Value : item.Versanddatum_Auswahl);
					sqlCommand.Parameters.AddWithValue("Versanddienstleister" + i, item.Versanddienstleister == null ? (object)DBNull.Value : item.Versanddienstleister);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_CS" + i, item.Versandinfo_von_CS == null ? (object)DBNull.Value : item.Versandinfo_von_CS);
					sqlCommand.Parameters.AddWithValue("Versandinfo_von_Lager" + i, item.Versandinfo_von_Lager == null ? (object)DBNull.Value : item.Versandinfo_von_Lager);
					sqlCommand.Parameters.AddWithValue("Versandnummer" + i, item.Versandnummer == null ? (object)DBNull.Value : item.Versandnummer);
					sqlCommand.Parameters.AddWithValue("Versandstatus" + i, item.Versandstatus == null ? (object)DBNull.Value : item.Versandstatus);
					sqlCommand.Parameters.AddWithValue("VKEinzelpreis" + i, item.VKEinzelpreis == null ? (object)DBNull.Value : item.VKEinzelpreis);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VKFestpreis == null ? (object)DBNull.Value : item.VKFestpreis);
					sqlCommand.Parameters.AddWithValue("VKGesamtpreis" + i, item.VKGesamtpreis == null ? (object)DBNull.Value : item.VKGesamtpreis);
					sqlCommand.Parameters.AddWithValue("Wunschtermin" + i, item.Wunschtermin == null ? (object)DBNull.Value : item.Wunschtermin);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					sqlCommand.Parameters.AddWithValue("zwischensumme" + i, item.zwischensumme == null ? (object)DBNull.Value : item.zwischensumme);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Angebotene Artikel] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;

				var sqlCommand = new SqlCommand("", connection, transaction);

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [Angebotene Artikel] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction

		public static List<AngeboteneArtikelEntity> GetByAngeboteNrWithTransaction(int OrderId, SqlConnection connection, SqlTransaction transaction, bool ignoreDeleted = true)
		{
			try
			{
				List<AngeboteneArtikelEntity> Lt = new List<AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dataTable = new DataTable();
				string query = "";
				if(ignoreDeleted)
				{
					query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId AND [Anzahl]<>0";
				}
				else
				{
					query = "SELECT * FROM [Angebotene Artikel] WHERE [Angebot-Nr] = @OrderId";
				}
				var sqlCommand = new SqlCommand(query, connection, transaction);
				sqlCommand.Parameters.AddWithValue("OrderId", OrderId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<AngeboteneArtikelEntity> GetbyArticleIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				var result = new List<AngeboteneArtikelEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getbyArticleIds(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<AngeboteneArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getbyArticleIds(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getbyArticleIds(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<AngeboteneArtikelEntity>();
		}
		private static List<AngeboteneArtikelEntity> getbyArticleIds(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				DataTable dt = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();

					var sqlCommand = new SqlCommand("", sqlConnection);

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [Angebotene Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dt);
				}
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<AngeboteneArtikelEntity>();
				}
			}
			return new List<AngeboteneArtikelEntity>();
		}
		public static List<AngeboteneArtikelEntity> GetByArticleInLager(int id, int? lagerId, bool ignoreClosed = false, bool withoutFA = true)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Angebotene Artikel] WHERE [Artikel-Nr]=@id AND ISNULL(erledigt_pos,0)=0 {(ignoreClosed ? " AND [Anzahl]>0" : "")} {(lagerId.HasValue == true ? $"AND lagerort_id={lagerId.Value}" : "")} {(withoutFA ? " AND ISNULL([Fertigungsnummer],0)=0" : "")} ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new AngeboteneArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<AngeboteneArtikelEntity>();
			}
		}
		public static int UpdateGSText(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Angebotene Artikel] SET [GSExternComment]=@GSExternComment, [GSInternComment]=@GSInternComment,[POSTEXT]=@POSTEXT WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("GSExternComment", item.GSExternComment == null ? (object)DBNull.Value : item.GSExternComment);
				sqlCommand.Parameters.AddWithValue("GSInternComment", item.GSInternComment == null ? (object)DBNull.Value : item.GSInternComment);
				sqlCommand.Parameters.AddWithValue("POSTEXT", item.POSTEXT == null ? (object)DBNull.Value : item.POSTEXT);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> GetForE_Rechnung(List<int> angebotNrs, SqlConnection connection, SqlTransaction transaction)
		{
			try
			{
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				SqlDataAdapter SelectAdapter = new SqlDataAdapter();
				DataTable dt = new DataTable();

				string query = $@"SELECT [Angebote].[Nr] AS ANr, [angebotene Artikel].*
                    FROM [Angebote] INNER JOIN [angebotene Artikel] ON [Angebote].[nr_lie] = [angebotene Artikel].[Angebot-Nr]
                    WHERE [Angebote].[Typ]='Rechnung' AND [Angebote].[Angebot-Nr] IN ({string.Join(",", angebotNrs)})";
				var sqlCommand = new SqlCommand(query, connection, transaction);
				new SqlDataAdapter(sqlCommand).Fill(dt);
				if(dt.Rows.Count > 0)
				{
					return dt.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity(x, true)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
				}
			} catch(Exception Ex)
			{
				throw;
			}
		}
		public static List<KeyValuePair<int, decimal>> GetTotalQuantityByAB(List<int> abIds)
		{
			if(abIds == null || abIds.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [Angebot-Nr], SUM([OriginalAnzahl]) AS Anzahl FROM [Angebotene Artikel]  WHERE [Angebot-Nr] IN ({string.Join(",", abIds)}) GROUP BY [Angebot-Nr]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, decimal>(
					int.TryParse(x[0].ToString(), out var n) ? n : -1,
					decimal.TryParse(x[1].ToString(), out var m) ? m : 0)).ToList();
			}
			else
			{
				return new List<KeyValuePair<int, decimal>>();
			}
		}
		public static int GetUpdatedLager(int nr)
		{
			try
			{
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					string query = "SELECT Lagerort_id FROM [Angebotene Artikel] WHERE [Nr] = @Id";

					using(var sqlCommand = new SqlCommand(query, sqlConnection))
					{
						sqlCommand.Parameters.AddWithValue("@Id", nr);

						var result = sqlCommand.ExecuteScalar();
						return result != null ? Convert.ToInt32(result) : -1; // Retourne -1 si aucune donnée trouvée
					}
				}
			} catch(Exception ex)
			{
				Console.WriteLine($"Erreur : {ex.Message}");
				return -1; // Retourne une valeur par défaut en cas d'erreur
			}
		}

		public static int UpdateQuantities(List<KeyValuePair<int, decimal>> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items == null || items.Count <= 0)
				return 0;

			string query = string.Join("; " , items.Select(x=> $"UPDATE [Angebotene Artikel] SET [Anzahl]={x.Value}, [Geliefert]=[OriginalAnzahl]-{x.Value} WHERE [Nr]={x.Key}"));
			var sqlCommand = new SqlCommand(query, connection, transaction);

			return sqlCommand.ExecuteNonQuery();
		}
	}
}
