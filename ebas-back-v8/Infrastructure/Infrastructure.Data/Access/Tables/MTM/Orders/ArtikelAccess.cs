using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class ArtikelAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.ArtikelEntity Get(int artikel_nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikel_nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.ArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}
		private static List<Entities.Tables.MTM.ArtikelEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.MTM.ArtikelEntity>();
				}
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}

		public static int Insert(Entities.Tables.MTM.ArtikelEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryName],[ProductionCountrySequence],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) OUTPUT INSERTED.[Artikel-Nr] VALUES (@Abladestelle,@aktiv,@aktualisiert,@Anfangsbestand,@ArticleNumber,@Artikel_aus_eigener_Produktion,@Artikel_fur_weitere_Bestellungen_sperren,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@artikelklassifizierung,@Artikelkurztext,@Artikelnummer,@Barverkauf,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@BezeichnungAL,@Blokiert_Status,@COF_Pflichtig,@CP_required,@Crossreferenz,@Cu_Gewicht,@CustomerIndex,@CustomerIndexSequence,@CustomerItemNumber,@CustomerItemNumberSequence,@CustomerNumber,@CustomerPrefix,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dienstelistung,@Dokumente,@EAN,@Einheit,@EMPB,@EMPB_Freigegeben,@Ersatzartikel,@ESD_Schutz,@ESD_Schutz_Text,@Exportgewicht,@fakturieren_Stuckliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Grosse,@Grund_fur_Sperre,@gultig_bis,@Halle,@Hubmastleitungen,@ID_Klassifizierung,@Index_Kunde,@Index_Kunde_Datum,@Info_WE,@Kanban,@Kategorie,@Klassifizierung,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Lieferzeit,@Losgroesse,@Materialkosten_Alt,@MHD,@Minerals_Confirmity,@Praeferenz_Aktuelles_jahr,@Praeferenz_Folgejahr,@Preiseinheit,@pro_Zeiteinheit,@ProductionCountryName,@ProductionCountrySequence,@ProductionSiteName,@ProductionSiteSequence,@Produktionszeit,@Provisionsartikel,@Prufstatus_TN_Ware,@Rabattierfahig,@Rahmen,@Rahmen2,@Rahmenauslauf,@Rahmenauslauf2,@Rahmenmenge,@Rahmenmenge2,@Rahmen_Nr,@Rahmen_Nr2,@REACH_SVHC_Confirmity,@ROHS_EEE_Confirmity,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stuckliste,@Stundensatz,@Sysmonummer,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@VDA_1,@VDA_2,@Verpackung,@Verpackungsart,@Verpackungsmenge,@VK_Festpreis,@Volumen,@Warengruppe,@Warentyp,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zeitraum_MHD,@Zolltarif_nr,@Zuschlag_VK); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
					sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
					sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse", item.Grosse == null ? (object)DBNull.Value : item.Grosse);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis", item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
					sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
					sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
					sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Entities.Tables.MTM.ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129; // Nb params per query
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
		private static int insert(List<Entities.Tables.MTM.ArtikelEntity> items)
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
						query += " INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryName],[ProductionCountrySequence],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) VALUES ( "

							+ "@Abladestelle" + i + ","
							+ "@aktiv" + i + ","
							+ "@aktualisiert" + i + ","
							+ "@Anfangsbestand" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@Artikel_aus_eigener_Produktion" + i + ","
							+ "@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
							+ "@Artikelfamilie_Kunde" + i + ","
							+ "@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "@artikelklassifizierung" + i + ","

							+ "@Artikelkurztext" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Barverkauf" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@Bezeichnung_3" + i + ","
							+ "@BezeichnungAL" + i + ","
							+ "@Blokiert_Status" + i + ","
							+ "@COF_Pflichtig" + i + ","
							+ "@CP_required" + i + ","
							+ "@Crossreferenz" + i + ","
							+ "@Cu_Gewicht" + i + ","
							+ "@CustomerIndex" + i + ","
							+ "@CustomerIndexSequence" + i + ","
							+ "@CustomerItemNumber" + i + ","
							+ "@CustomerItemNumberSequence" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@CustomerPrefix" + i + ","
							+ "@Datum_Anfangsbestand" + i + ","
							+ "@DEL" + i + ","
							+ "@DEL_fixiert" + i + ","
							+ "@Dienstelistung" + i + ","
							+ "@Dokumente" + i + ","
							+ "@EAN" + i + ","
							+ "@Einheit" + i + ","
							+ "@EMPB" + i + ","
							+ "@EMPB_Freigegeben" + i + ","
							+ "@Ersatzartikel" + i + ","
							+ "@ESD_Schutz" + i + ","
							+ "@ESD_Schutz_Text" + i + ","
							+ "@Exportgewicht" + i + ","
							+ "@fakturieren_Stuckliste" + i + ","
							+ "@Farbe" + i + ","
							+ "@fibu_rahmen" + i + ","
							+ "@Freigabestatus" + i + ","
							+ "@Freigabestatus_TN_intern" + i + ","
							+ "@Gebinde" + i + ","
							+ "@Gewicht" + i + ","
							+ "@Grosse" + i + ","
							+ "@Grund_fur_Sperre" + i + ","
							+ "@gultig_bis" + i + ","
							+ "@Halle" + i + ","
							+ "@Hubmastleitungen" + i + ","
							+ "@ID_Klassifizierung" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@Index_Kunde_Datum" + i + ","
							+ "@Info_WE" + i + ","
							+ "@Kanban" + i + ","
							+ "@Kategorie" + i + ","
							+ "@Klassifizierung" + i + ","
							+ "@Kriterium1" + i + ","
							+ "@Kriterium2" + i + ","
							+ "@Kriterium3" + i + ","
							+ "@Kriterium4" + i + ","
							+ "@Kupferbasis" + i + ","
							+ "@Kupferzahl" + i + ","
							+ "@Lagerartikel" + i + ","
							+ "@Lagerhaltungskosten" + i + ","
							+ "@Langtext" + i + ","
							+ "@Langtext_drucken_AB" + i + ","
							+ "@Langtext_drucken_BW" + i + ","
							+ "@Lieferzeit" + i + ","
							+ "@Losgroesse" + i + ","
							+ "@Materialkosten_Alt" + i + ","
							+ "@MHD" + i + ","
							+ "@Minerals_Confirmity" + i + ","
							+ "@Praeferenz_Aktuelles_jahr" + i + ","
							+ "@Praeferenz_Folgejahr" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@pro_Zeiteinheit" + i + ","
							+ "@ProductionCountryName" + i + ","
							+ "@ProductionCountrySequence" + i + ","
							+ "@ProductionSiteName" + i + ","
							+ "@ProductionSiteSequence" + i + ","
							+ "@Produktionszeit" + i + ","
							+ "@Provisionsartikel" + i + ","
							+ "@Prufstatus_TN_Ware" + i + ","
							+ "@Rabattierfahig" + i + ","
							+ "@Rahmen" + i + ","
							+ "@Rahmen2" + i + ","
							+ "@Rahmenauslauf" + i + ","
							+ "@Rahmenauslauf2" + i + ","
							+ "@Rahmenmenge" + i + ","
							+ "@Rahmenmenge2" + i + ","
							+ "@Rahmen_Nr" + i + ","
							+ "@Rahmen_Nr2" + i + ","
							+ "@REACH_SVHC_Confirmity" + i + ","
							+ "@ROHS_EEE_Confirmity" + i + ","
							+ "@Seriennummer" + i + ","
							+ "@Seriennummernverwaltung" + i + ","
							+ "@Sonderrabatt" + i + ","
							+ "@Standard_Lagerort_id" + i + ","
							+ "@Stuckliste" + i + ","
							+ "@Stundensatz" + i + ","
							+ "@Sysmonummer" + i + ","
							+ "@UL_Etikett" + i + ","
							+ "@UL_zertifiziert" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@Ursprungsland" + i + ","
							+ "@VDA_1" + i + ","
							+ "@VDA_2" + i + ","
							+ "@Verpackung" + i + ","
							+ "@Verpackungsart" + i + ","
							+ "@Verpackungsmenge" + i + ","
							+ "@VK_Festpreis" + i + ","
							+ "@Volumen" + i + ","
							+ "@Warengruppe" + i + ","
							+ "@Warentyp" + i + ","
							+ "@Webshop" + i + ","
							+ "@Werkzeug" + i + ","
							+ "@Wert_Anfangsbestand" + i + ","
							+ "@Zeichnungsnummer" + i + ","
							+ "@Zeitraum_MHD" + i + ","
							+ "@Zolltarif_nr" + i + ","
							+ "@Zuschlag_VK" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
						sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
						sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
						sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
						sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
						sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
						sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
						sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
						sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Grosse == null ? (object)DBNull.Value : item.Grosse);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
						sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
						sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
						sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
						sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
						sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
						sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
						sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Entities.Tables.MTM.ArtikelEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Artikel] SET [Abladestelle]=@Abladestelle, [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [ArticleNumber]=@ArticleNumber, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [artikelklassifizierung]=@artikelklassifizierung, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [BezeichnungAL]=@BezeichnungAL, [Blokiert_Status]=@Blokiert_Status, [COF_Pflichtig]=@COF_Pflichtig, [CP_required]=@CP_required, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [CustomerIndex]=@CustomerIndex, [CustomerIndexSequence]=@CustomerIndexSequence, [CustomerItemNumber]=@CustomerItemNumber, [CustomerItemNumberSequence]=@CustomerItemNumberSequence, [CustomerNumber]=@CustomerNumber, [CustomerPrefix]=@CustomerPrefix, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dienstelistung]=@Dienstelistung, [Dokumente]=@Dokumente, [EAN]=@EAN, [Einheit]=@Einheit, [EMPB]=@EMPB, [EMPB_Freigegeben]=@EMPB_Freigegeben, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [ESD_Schutz_Text]=@ESD_Schutz_Text, [Exportgewicht]=@Exportgewicht, [fakturieren Stückliste]=@fakturieren_Stuckliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Grosse, [Grund für Sperre]=@Grund_fur_Sperre, [gültig bis]=@gultig_bis, [Halle]=@Halle, [Hubmastleitungen]=@Hubmastleitungen, [ID_Klassifizierung]=@ID_Klassifizierung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Info_WE]=@Info_WE, [Kanban]=@Kanban, [Kategorie]=@Kategorie, [Klassifizierung]=@Klassifizierung, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [Materialkosten_Alt]=@Materialkosten_Alt, [MHD]=@MHD, [Minerals Confirmity]=@Minerals_Confirmity, [Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr, [Praeferenz_Folgejahr]=@Praeferenz_Folgejahr, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [ProductionCountryName]=@ProductionCountryName, [ProductionCountrySequence]=@ProductionCountrySequence, [ProductionSiteName]=@ProductionSiteName, [ProductionSiteSequence]=@ProductionSiteSequence, [Produktionszeit]=@Produktionszeit, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prufstatus_TN_Ware, [Rabattierfähig]=@Rabattierfahig, [Rahmen]=@Rahmen, [Rahmen2]=@Rahmen2, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenauslauf2]=@Rahmenauslauf2, [Rahmenmenge]=@Rahmenmenge, [Rahmenmenge2]=@Rahmenmenge2, [Rahmen-Nr]=@Rahmen_Nr, [Rahmen-Nr2]=@Rahmen_Nr2, [REACH SVHC Confirmity]=@REACH_SVHC_Confirmity, [ROHS EEE Confirmity]=@ROHS_EEE_Confirmity, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stuckliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [VDA_1]=@VDA_1, [VDA_2]=@VDA_2, [Verpackung]=@Verpackung, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Warentyp]=@Warentyp, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zeitraum_MHD]=@Zeitraum_MHD, [Zolltarif_nr]=@Zolltarif_nr, [Zuschlag_VK]=@Zuschlag_VK WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
				sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
				sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
				sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
				sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
				sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
				sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
				sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
				sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
				sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
				sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
				sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
				sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
				sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
				sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
				sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
				sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
				sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
				sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
				sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
				sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
				sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
				sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
				sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
				sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
				sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
				sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
				sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
				sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
				sqlCommand.Parameters.AddWithValue("Grosse", item.Grosse == null ? (object)DBNull.Value : item.Grosse);
				sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
				sqlCommand.Parameters.AddWithValue("gultig_bis", item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
				sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
				sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
				sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
				sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
				sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
				sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
				sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
				sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
				sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
				sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
				sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
				sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
				sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
				sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
				sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
				sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
				sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
				sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
				sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
				sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
				sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
				sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
				sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
				sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
				sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
				sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
				sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
				sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
				sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
				sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
				sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
				sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
				sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
				sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
				sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
				sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
				sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
				sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
				sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
				sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
				sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
				sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
				sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
				sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
				sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
				sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
				sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
				sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
				sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
				sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Entities.Tables.MTM.ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129; // Nb params per query
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
		private static int update(List<Entities.Tables.MTM.ArtikelEntity> items)
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
						query += " UPDATE [Artikel] SET "

							+ "[Abladestelle]=@Abladestelle" + i + ","
							+ "[aktiv]=@aktiv" + i + ","
							+ "[aktualisiert]=@aktualisiert" + i + ","
							+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion" + i + ","
							+ "[Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
							+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
							+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "[artikelklassifizierung]=@artikelklassifizierung" + i + ","
							+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Barverkauf]=@Barverkauf" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
							+ "[Bezeichnung 3]=@Bezeichnung_3" + i + ","
							+ "[BezeichnungAL]=@BezeichnungAL" + i + ","
							+ "[Blokiert_Status]=@Blokiert_Status" + i + ","
							+ "[COF_Pflichtig]=@COF_Pflichtig" + i + ","
							+ "[CP_required]=@CP_required" + i + ","
							+ "[Crossreferenz]=@Crossreferenz" + i + ","
							+ "[Cu-Gewicht]=@Cu_Gewicht" + i + ","
							+ "[CustomerIndex]=@CustomerIndex" + i + ","
							+ "[CustomerIndexSequence]=@CustomerIndexSequence" + i + ","
							+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
							+ "[CustomerItemNumberSequence]=@CustomerItemNumberSequence" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[CustomerPrefix]=@CustomerPrefix" + i + ","
							+ "[Datum Anfangsbestand]=@Datum_Anfangsbestand" + i + ","
							+ "[DEL]=@DEL" + i + ","
							+ "[DEL fixiert]=@DEL_fixiert" + i + ","
							+ "[Dienstelistung]=@Dienstelistung" + i + ","
							+ "[Dokumente]=@Dokumente" + i + ","
							+ "[EAN]=@EAN" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[EMPB]=@EMPB" + i + ","
							+ "[EMPB_Freigegeben]=@EMPB_Freigegeben" + i + ","
							+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
							+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
							+ "[ESD_Schutz_Text]=@ESD_Schutz_Text" + i + ","
							+ "[Exportgewicht]=@Exportgewicht" + i + ","
							+ "[fakturieren Stückliste]=@fakturieren_Stuckliste" + i + ","
							+ "[Farbe]=@Farbe" + i + ","
							+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
							+ "[Freigabestatus]=@Freigabestatus" + i + ","
							+ "[Freigabestatus TN intern]=@Freigabestatus_TN_intern" + i + ","
							+ "[Gebinde]=@Gebinde" + i + ","
							+ "[Gewicht]=@Gewicht" + i + ","
							+ "[Größe]=@Grosse" + i + ","
							+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
							+ "[gültig bis]=@gultig_bis" + i + ","
							+ "[Halle]=@Halle" + i + ","
							+ "[Hubmastleitungen]=@Hubmastleitungen" + i + ","
							+ "[ID_Klassifizierung]=@ID_Klassifizierung" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
							+ "[Info_WE]=@Info_WE" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Kategorie]=@Kategorie" + i + ","
							+ "[Klassifizierung]=@Klassifizierung" + i + ","
							+ "[Kriterium1]=@Kriterium1" + i + ","
							+ "[Kriterium2]=@Kriterium2" + i + ","
							+ "[Kriterium3]=@Kriterium3" + i + ","
							+ "[Kriterium4]=@Kriterium4" + i + ","
							+ "[Kupferbasis]=@Kupferbasis" + i + ","
							+ "[Kupferzahl]=@Kupferzahl" + i + ","
							+ "[Lagerartikel]=@Lagerartikel" + i + ","
							+ "[Lagerhaltungskosten]=@Lagerhaltungskosten" + i + ","
							+ "[Langtext]=@Langtext" + i + ","
							+ "[Langtext_drucken_AB]=@Langtext_drucken_AB" + i + ","
							+ "[Langtext_drucken_BW]=@Langtext_drucken_BW" + i + ","
							+ "[Lieferzeit]=@Lieferzeit" + i + ","
							+ "[Losgroesse]=@Losgroesse" + i + ","
							+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
							+ "[MHD]=@MHD" + i + ","
							+ "[Minerals Confirmity]=@Minerals_Confirmity" + i + ","
							+ "[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr" + i + ","
							+ "[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[pro Zeiteinheit]=@pro_Zeiteinheit" + i + ","
							+ "[ProductionCountryName]=@ProductionCountryName" + i + ","
							+ "[ProductionCountrySequence]=@ProductionCountrySequence" + i + ","
							+ "[ProductionSiteName]=@ProductionSiteName" + i + ","
							+ "[ProductionSiteSequence]=@ProductionSiteSequence" + i + ","
							+ "[Produktionszeit]=@Produktionszeit" + i + ","
							+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
							+ "[Prüfstatus TN Ware]=@Prufstatus_TN_Ware" + i + ","
							+ "[Rabattierfähig]=@Rabattierfahig" + i + ","
							+ "[Rahmen]=@Rahmen" + i + ","
							+ "[Rahmen2]=@Rahmen2" + i + ","
							+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
							+ "[Rahmenauslauf2]=@Rahmenauslauf2" + i + ","
							+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
							+ "[Rahmenmenge2]=@Rahmenmenge2" + i + ","
							+ "[Rahmen-Nr]=@Rahmen_Nr" + i + ","
							+ "[Rahmen-Nr2]=@Rahmen_Nr2" + i + ","
							+ "[REACH SVHC Confirmity]=@REACH_SVHC_Confirmity" + i + ","
							+ "[ROHS EEE Confirmity]=@ROHS_EEE_Confirmity" + i + ","
							+ "[Seriennummer]=@Seriennummer" + i + ","
							+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
							+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
							+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
							+ "[Stückliste]=@Stuckliste" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + ","
							+ "[Sysmonummer]=@Sysmonummer" + i + ","
							+ "[UL Etikett]=@UL_Etikett" + i + ","
							+ "[UL zertifiziert]=@UL_zertifiziert" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[Ursprungsland]=@Ursprungsland" + i + ","
							+ "[VDA_1]=@VDA_1" + i + ","
							+ "[VDA_2]=@VDA_2" + i + ","
							+ "[Verpackung]=@Verpackung" + i + ","
							+ "[Verpackungsart]=@Verpackungsart" + i + ","
							+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
							+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
							+ "[Volumen]=@Volumen" + i + ","
							+ "[Warengruppe]=@Warengruppe" + i + ","
							+ "[Warentyp]=@Warentyp" + i + ","
							+ "[Webshop]=@Webshop" + i + ","
							+ "[Werkzeug]=@Werkzeug" + i + ","
							+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
							+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
							+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + ","
							+ "[Zolltarif_nr]=@Zolltarif_nr" + i + ","
							+ "[Zuschlag_VK]=@Zuschlag_VK" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
						sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
						sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
						sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
						sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
						sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
						sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
						sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
						sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
						sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
						sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
						sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Grosse == null ? (object)DBNull.Value : item.Grosse);
						sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
						sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
						sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
						sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
						sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
						sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
						sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
						sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
						sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
						sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
						sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
						sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
						sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
						sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
						sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
						sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
						sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int artikel_nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", artikel_nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
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

					string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Entities.Tables.MTM.ArtikelEntity GetWithTransaction(int artikel_nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Artikel] WHERE [Artikel-Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", artikel_nr);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.ArtikelEntity>();
			}
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}
		private static List<Entities.Tables.MTM.ArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.MTM.ArtikelEntity>();
				}
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}

		public static int InsertWithTransaction(Entities.Tables.MTM.ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryName],[ProductionCountrySequence],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) OUTPUT INSERTED.[Artikel-Nr] VALUES (@Abladestelle,@aktiv,@aktualisiert,@Anfangsbestand,@ArticleNumber,@Artikel_aus_eigener_Produktion,@Artikel_fur_weitere_Bestellungen_sperren,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@artikelklassifizierung,@Artikelkurztext,@Artikelnummer,@Barverkauf,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@BezeichnungAL,@Blokiert_Status,@COF_Pflichtig,@CP_required,@Crossreferenz,@Cu_Gewicht,@CustomerIndex,@CustomerIndexSequence,@CustomerItemNumber,@CustomerItemNumberSequence,@CustomerNumber,@CustomerPrefix,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dienstelistung,@Dokumente,@EAN,@Einheit,@EMPB,@EMPB_Freigegeben,@Ersatzartikel,@ESD_Schutz,@ESD_Schutz_Text,@Exportgewicht,@fakturieren_Stuckliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Grosse,@Grund_fur_Sperre,@gultig_bis,@Halle,@Hubmastleitungen,@ID_Klassifizierung,@Index_Kunde,@Index_Kunde_Datum,@Info_WE,@Kanban,@Kategorie,@Klassifizierung,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Lieferzeit,@Losgroesse,@Materialkosten_Alt,@MHD,@Minerals_Confirmity,@Praeferenz_Aktuelles_jahr,@Praeferenz_Folgejahr,@Preiseinheit,@pro_Zeiteinheit,@ProductionCountryName,@ProductionCountrySequence,@ProductionSiteName,@ProductionSiteSequence,@Produktionszeit,@Provisionsartikel,@Prufstatus_TN_Ware,@Rabattierfahig,@Rahmen,@Rahmen2,@Rahmenauslauf,@Rahmenauslauf2,@Rahmenmenge,@Rahmenmenge2,@Rahmen_Nr,@Rahmen_Nr2,@REACH_SVHC_Confirmity,@ROHS_EEE_Confirmity,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stuckliste,@Stundensatz,@Sysmonummer,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@VDA_1,@VDA_2,@Verpackung,@Verpackungsart,@Verpackungsmenge,@VK_Festpreis,@Volumen,@Warengruppe,@Warentyp,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zeitraum_MHD,@Zolltarif_nr,@Zuschlag_VK); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
			sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
			sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
			sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
			sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
			sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
			sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
			sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
			sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
			sqlCommand.Parameters.AddWithValue("Grosse", item.Grosse == null ? (object)DBNull.Value : item.Grosse);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
			sqlCommand.Parameters.AddWithValue("gultig_bis", item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
			sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
			sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
			sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
			sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
			sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
			sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Entities.Tables.MTM.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129; // Nb params per query
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
		private static int insertWithTransaction(List<Entities.Tables.MTM.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Artikel] ([Abladestelle],[aktiv],[aktualisiert],[Anfangsbestand],[ArticleNumber],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[artikelklassifizierung],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[BezeichnungAL],[Blokiert_Status],[COF_Pflichtig],[CP_required],[Crossreferenz],[Cu-Gewicht],[CustomerIndex],[CustomerIndexSequence],[CustomerItemNumber],[CustomerItemNumberSequence],[CustomerNumber],[CustomerPrefix],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dienstelistung],[Dokumente],[EAN],[Einheit],[EMPB],[EMPB_Freigegeben],[Ersatzartikel],[ESD_Schutz],[ESD_Schutz_Text],[Exportgewicht],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Hubmastleitungen],[ID_Klassifizierung],[Index_Kunde],[Index_Kunde_Datum],[Info_WE],[Kanban],[Kategorie],[Klassifizierung],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Lieferzeit],[Losgroesse],[Materialkosten_Alt],[MHD],[Minerals Confirmity],[Praeferenz_Aktuelles_jahr],[Praeferenz_Folgejahr],[Preiseinheit],[pro Zeiteinheit],[ProductionCountryName],[ProductionCountrySequence],[ProductionSiteName],[ProductionSiteSequence],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmen2],[Rahmenauslauf],[Rahmenauslauf2],[Rahmenmenge],[Rahmenmenge2],[Rahmen-Nr],[Rahmen-Nr2],[REACH SVHC Confirmity],[ROHS EEE Confirmity],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[VDA_1],[VDA_2],[Verpackung],[Verpackungsart],[Verpackungsmenge],[VK-Festpreis],[Volumen],[Warengruppe],[Warentyp],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zeitraum_MHD],[Zolltarif_nr],[Zuschlag_VK]) VALUES ( "

						+ "@Abladestelle" + i + ","
						+ "@aktiv" + i + ","
						+ "@aktualisiert" + i + ","
						+ "@Anfangsbestand" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@Artikel_aus_eigener_Produktion" + i + ","
						+ "@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
						+ "@Artikelfamilie_Kunde" + i + ","
						+ "@Artikelfamilie_Kunde_Detail1" + i + ","
						+ "@Artikelfamilie_Kunde_Detail2" + i + ","
						+ "@artikelklassifizierung" + i + ","
						+ "@Artikelkurztext" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Barverkauf" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Bezeichnung_2" + i + ","
						+ "@Bezeichnung_3" + i + ","
						+ "@BezeichnungAL" + i + ","
						+ "@Blokiert_Status" + i + ","
						+ "@COF_Pflichtig" + i + ","
						+ "@CP_required" + i + ","
						+ "@Crossreferenz" + i + ","
						+ "@Cu_Gewicht" + i + ","
						+ "@CustomerIndex" + i + ","
						+ "@CustomerIndexSequence" + i + ","
						+ "@CustomerItemNumber" + i + ","
						+ "@CustomerItemNumberSequence" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@CustomerPrefix" + i + ","
						+ "@Datum_Anfangsbestand" + i + ","
						+ "@DEL" + i + ","
						+ "@DEL_fixiert" + i + ","
						+ "@Dienstelistung" + i + ","
						+ "@Dokumente" + i + ","
						+ "@EAN" + i + ","
						+ "@Einheit" + i + ","
						+ "@EMPB" + i + ","
						+ "@EMPB_Freigegeben" + i + ","
						+ "@Ersatzartikel" + i + ","
						+ "@ESD_Schutz" + i + ","
						+ "@ESD_Schutz_Text" + i + ","
						+ "@Exportgewicht" + i + ","
						+ "@fakturieren_Stuckliste" + i + ","
						+ "@Farbe" + i + ","
						+ "@fibu_rahmen" + i + ","
						+ "@Freigabestatus" + i + ","
						+ "@Freigabestatus_TN_intern" + i + ","
						+ "@Gebinde" + i + ","
						+ "@Gewicht" + i + ","
						+ "@Grosse" + i + ","
						+ "@Grund_fur_Sperre" + i + ","
						+ "@gultig_bis" + i + ","
						+ "@Halle" + i + ","
						+ "@Hubmastleitungen" + i + ","
						+ "@ID_Klassifizierung" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@Index_Kunde_Datum" + i + ","
						+ "@Info_WE" + i + ","
						+ "@Kanban" + i + ","
						+ "@Kategorie" + i + ","
						+ "@Klassifizierung" + i + ","
						+ "@Kriterium1" + i + ","
						+ "@Kriterium2" + i + ","
						+ "@Kriterium3" + i + ","
						+ "@Kriterium4" + i + ","
						+ "@Kupferbasis" + i + ","
						+ "@Kupferzahl" + i + ","
						+ "@Lagerartikel" + i + ","
						+ "@Lagerhaltungskosten" + i + ","
						+ "@Langtext" + i + ","
						+ "@Langtext_drucken_AB" + i + ","
						+ "@Langtext_drucken_BW" + i + ","
						+ "@Lieferzeit" + i + ","
						+ "@Losgroesse" + i + ","
						+ "@Materialkosten_Alt" + i + ","
						+ "@MHD" + i + ","
						+ "@Minerals_Confirmity" + i + ","
						+ "@Praeferenz_Aktuelles_jahr" + i + ","
						+ "@Praeferenz_Folgejahr" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@pro_Zeiteinheit" + i + ","
						+ "@ProductionCountryName" + i + ","
						+ "@ProductionCountrySequence" + i + ","
						+ "@ProductionSiteName" + i + ","
						+ "@ProductionSiteSequence" + i + ","
						+ "@Produktionszeit" + i + ","
						+ "@Provisionsartikel" + i + ","
						+ "@Prufstatus_TN_Ware" + i + ","
						+ "@Rabattierfahig" + i + ","
						+ "@Rahmen" + i + ","
						+ "@Rahmen2" + i + ","
						+ "@Rahmenauslauf" + i + ","
						+ "@Rahmenauslauf2" + i + ","
						+ "@Rahmenmenge" + i + ","
						+ "@Rahmenmenge2" + i + ","
						+ "@Rahmen_Nr" + i + ","
						+ "@Rahmen_Nr2" + i + ","
						+ "@REACH_SVHC_Confirmity" + i + ","
						+ "@ROHS_EEE_Confirmity" + i + ","
						+ "@Seriennummer" + i + ","
						+ "@Seriennummernverwaltung" + i + ","
						+ "@Sonderrabatt" + i + ","
						+ "@Standard_Lagerort_id" + i + ","
						+ "@Stuckliste" + i + ","
						+ "@Stundensatz" + i + ","
						+ "@Sysmonummer" + i + ","
						+ "@UL_Etikett" + i + ","
						+ "@UL_zertifiziert" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@Ursprungsland" + i + ","
						+ "@VDA_1" + i + ","
						+ "@VDA_2" + i + ","
						+ "@Verpackung" + i + ","
						+ "@Verpackungsart" + i + ","
						+ "@Verpackungsmenge" + i + ","
						+ "@VK_Festpreis" + i + ","
						+ "@Volumen" + i + ","
						+ "@Warengruppe" + i + ","
						+ "@Warentyp" + i + ","
						+ "@Webshop" + i + ","
						+ "@Werkzeug" + i + ","
						+ "@Wert_Anfangsbestand" + i + ","
						+ "@Zeichnungsnummer" + i + ","
						+ "@Zeitraum_MHD" + i + ","
						+ "@Zolltarif_nr" + i + ","
						+ "@Zuschlag_VK" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
					sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
					sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Grosse == null ? (object)DBNull.Value : item.Grosse);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
					sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
					sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Entities.Tables.MTM.ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Artikel] SET [Abladestelle]=@Abladestelle, [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [ArticleNumber]=@ArticleNumber, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [artikelklassifizierung]=@artikelklassifizierung, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [BezeichnungAL]=@BezeichnungAL, [Blokiert_Status]=@Blokiert_Status, [COF_Pflichtig]=@COF_Pflichtig, [CP_required]=@CP_required, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [CustomerIndex]=@CustomerIndex, [CustomerIndexSequence]=@CustomerIndexSequence, [CustomerItemNumber]=@CustomerItemNumber, [CustomerItemNumberSequence]=@CustomerItemNumberSequence, [CustomerNumber]=@CustomerNumber, [CustomerPrefix]=@CustomerPrefix, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dienstelistung]=@Dienstelistung, [Dokumente]=@Dokumente, [EAN]=@EAN, [Einheit]=@Einheit, [EMPB]=@EMPB, [EMPB_Freigegeben]=@EMPB_Freigegeben, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [ESD_Schutz_Text]=@ESD_Schutz_Text, [Exportgewicht]=@Exportgewicht, [fakturieren Stückliste]=@fakturieren_Stuckliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Grosse, [Grund für Sperre]=@Grund_fur_Sperre, [gültig bis]=@gultig_bis, [Halle]=@Halle, [Hubmastleitungen]=@Hubmastleitungen, [ID_Klassifizierung]=@ID_Klassifizierung, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Info_WE]=@Info_WE, [Kanban]=@Kanban, [Kategorie]=@Kategorie, [Klassifizierung]=@Klassifizierung, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Lieferzeit]=@Lieferzeit, [Losgroesse]=@Losgroesse, [Materialkosten_Alt]=@Materialkosten_Alt, [MHD]=@MHD, [Minerals Confirmity]=@Minerals_Confirmity, [Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr, [Praeferenz_Folgejahr]=@Praeferenz_Folgejahr, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [ProductionCountryName]=@ProductionCountryName, [ProductionCountrySequence]=@ProductionCountrySequence, [ProductionSiteName]=@ProductionSiteName, [ProductionSiteSequence]=@ProductionSiteSequence, [Produktionszeit]=@Produktionszeit, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prufstatus_TN_Ware, [Rabattierfähig]=@Rabattierfahig, [Rahmen]=@Rahmen, [Rahmen2]=@Rahmen2, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenauslauf2]=@Rahmenauslauf2, [Rahmenmenge]=@Rahmenmenge, [Rahmenmenge2]=@Rahmenmenge2, [Rahmen-Nr]=@Rahmen_Nr, [Rahmen-Nr2]=@Rahmen_Nr2, [REACH SVHC Confirmity]=@REACH_SVHC_Confirmity, [ROHS EEE Confirmity]=@ROHS_EEE_Confirmity, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stuckliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [VDA_1]=@VDA_1, [VDA_2]=@VDA_2, [Verpackung]=@Verpackung, [Verpackungsart]=@Verpackungsart, [Verpackungsmenge]=@Verpackungsmenge, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Warentyp]=@Warentyp, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zeitraum_MHD]=@Zeitraum_MHD, [Zolltarif_nr]=@Zolltarif_nr, [Zuschlag_VK]=@Zuschlag_VK WHERE [Artikel-Nr]=@Artikel_Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Abladestelle", item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
			sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv == null ? (object)DBNull.Value : item.aktiv);
			sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
			sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
			sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren", item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
			sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
			sqlCommand.Parameters.AddWithValue("artikelklassifizierung", item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
			sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
			sqlCommand.Parameters.AddWithValue("BezeichnungAL", item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
			sqlCommand.Parameters.AddWithValue("Blokiert_Status", item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
			sqlCommand.Parameters.AddWithValue("COF_Pflichtig", item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
			sqlCommand.Parameters.AddWithValue("CP_required", item.CP_required == null ? (object)DBNull.Value : item.CP_required);
			sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
			sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
			sqlCommand.Parameters.AddWithValue("CustomerIndex", item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
			sqlCommand.Parameters.AddWithValue("CustomerIndexSequence", item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumber", item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
			sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence", item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerPrefix", item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
			sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
			sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
			sqlCommand.Parameters.AddWithValue("Dienstelistung", item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
			sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
			sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("EMPB", item.EMPB == null ? (object)DBNull.Value : item.EMPB);
			sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben", item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
			sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
			sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text", item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
			sqlCommand.Parameters.AddWithValue("Exportgewicht", item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
			sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste", item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
			sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
			sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
			sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
			sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
			sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
			sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
			sqlCommand.Parameters.AddWithValue("Grosse", item.Grosse == null ? (object)DBNull.Value : item.Grosse);
			sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre", item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
			sqlCommand.Parameters.AddWithValue("gultig_bis", item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
			sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
			sqlCommand.Parameters.AddWithValue("Hubmastleitungen", item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
			sqlCommand.Parameters.AddWithValue("ID_Klassifizierung", item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
			sqlCommand.Parameters.AddWithValue("Info_WE", item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
			sqlCommand.Parameters.AddWithValue("Klassifizierung", item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
			sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
			sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
			sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
			sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
			sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
			sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
			sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
			sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
			sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
			sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
			sqlCommand.Parameters.AddWithValue("Lieferzeit", item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
			sqlCommand.Parameters.AddWithValue("Losgroesse", item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
			sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
			sqlCommand.Parameters.AddWithValue("MHD", item.MHD == null ? (object)DBNull.Value : item.MHD);
			sqlCommand.Parameters.AddWithValue("Minerals_Confirmity", item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr", item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
			sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr", item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
			sqlCommand.Parameters.AddWithValue("ProductionCountryName", item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
			sqlCommand.Parameters.AddWithValue("ProductionCountrySequence", item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
			sqlCommand.Parameters.AddWithValue("ProductionSiteName", item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
			sqlCommand.Parameters.AddWithValue("ProductionSiteSequence", item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
			sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
			sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
			sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware", item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
			sqlCommand.Parameters.AddWithValue("Rabattierfahig", item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
			sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
			sqlCommand.Parameters.AddWithValue("Rahmen2", item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
			sqlCommand.Parameters.AddWithValue("Rahmenauslauf2", item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
			sqlCommand.Parameters.AddWithValue("Rahmenmenge2", item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
			sqlCommand.Parameters.AddWithValue("Rahmen_Nr2", item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
			sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity", item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
			sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity", item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
			sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
			sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
			sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
			sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Stuckliste", item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
			sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
			sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
			sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
			sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
			sqlCommand.Parameters.AddWithValue("VDA_1", item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
			sqlCommand.Parameters.AddWithValue("VDA_2", item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
			sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
			sqlCommand.Parameters.AddWithValue("Verpackungsart", item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
			sqlCommand.Parameters.AddWithValue("Verpackungsmenge", item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
			sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
			sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
			sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Warentyp", item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
			sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop == null ? (object)DBNull.Value : item.Webshop);
			sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
			sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
			sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
			sqlCommand.Parameters.AddWithValue("Zeitraum_MHD", item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
			sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
			sqlCommand.Parameters.AddWithValue("Zuschlag_VK", item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.MTM.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 129; // Nb params per query
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
		private static int updateWithTransaction(List<Entities.Tables.MTM.ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Artikel] SET "

					+ "[Abladestelle]=@Abladestelle" + i + ","
					+ "[aktiv]=@aktiv" + i + ","
					+ "[aktualisiert]=@aktualisiert" + i + ","
					+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion" + i + ","
					+ "[Artikel für weitere Bestellungen sperren]=@Artikel_fur_weitere_Bestellungen_sperren" + i + ","
					+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
					+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
					+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
					+ "[artikelklassifizierung]=@artikelklassifizierung" + i + ","
					+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Barverkauf]=@Barverkauf" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
					+ "[Bezeichnung 3]=@Bezeichnung_3" + i + ","
					+ "[BezeichnungAL]=@BezeichnungAL" + i + ","
					+ "[Blokiert_Status]=@Blokiert_Status" + i + ","
					+ "[COF_Pflichtig]=@COF_Pflichtig" + i + ","
					+ "[CP_required]=@CP_required" + i + ","
					+ "[Crossreferenz]=@Crossreferenz" + i + ","
					+ "[Cu-Gewicht]=@Cu_Gewicht" + i + ","
					+ "[CustomerIndex]=@CustomerIndex" + i + ","
					+ "[CustomerIndexSequence]=@CustomerIndexSequence" + i + ","
					+ "[CustomerItemNumber]=@CustomerItemNumber" + i + ","
					+ "[CustomerItemNumberSequence]=@CustomerItemNumberSequence" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[CustomerPrefix]=@CustomerPrefix" + i + ","
					+ "[Datum Anfangsbestand]=@Datum_Anfangsbestand" + i + ","
					+ "[DEL]=@DEL" + i + ","
					+ "[DEL fixiert]=@DEL_fixiert" + i + ","
					+ "[Dienstelistung]=@Dienstelistung" + i + ","
					+ "[Dokumente]=@Dokumente" + i + ","
					+ "[EAN]=@EAN" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[EMPB]=@EMPB" + i + ","
					+ "[EMPB_Freigegeben]=@EMPB_Freigegeben" + i + ","
					+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
					+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
					+ "[ESD_Schutz_Text]=@ESD_Schutz_Text" + i + ","
					+ "[Exportgewicht]=@Exportgewicht" + i + ","
					+ "[fakturieren Stückliste]=@fakturieren_Stuckliste" + i + ","
					+ "[Farbe]=@Farbe" + i + ","
					+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
					+ "[Freigabestatus]=@Freigabestatus" + i + ","
					+ "[Freigabestatus TN intern]=@Freigabestatus_TN_intern" + i + ","
					+ "[Gebinde]=@Gebinde" + i + ","
					+ "[Gewicht]=@Gewicht" + i + ","
					+ "[Größe]=@Grosse" + i + ","
					+ "[Grund für Sperre]=@Grund_fur_Sperre" + i + ","
					+ "[gültig bis]=@gultig_bis" + i + ","
					+ "[Halle]=@Halle" + i + ","
					+ "[Hubmastleitungen]=@Hubmastleitungen" + i + ","
					+ "[ID_Klassifizierung]=@ID_Klassifizierung" + i + ","
					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
					+ "[Info_WE]=@Info_WE" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Kategorie]=@Kategorie" + i + ","
					+ "[Klassifizierung]=@Klassifizierung" + i + ","
					+ "[Kriterium1]=@Kriterium1" + i + ","
					+ "[Kriterium2]=@Kriterium2" + i + ","
					+ "[Kriterium3]=@Kriterium3" + i + ","
					+ "[Kriterium4]=@Kriterium4" + i + ","
					+ "[Kupferbasis]=@Kupferbasis" + i + ","
					+ "[Kupferzahl]=@Kupferzahl" + i + ","
					+ "[Lagerartikel]=@Lagerartikel" + i + ","
					+ "[Lagerhaltungskosten]=@Lagerhaltungskosten" + i + ","
					+ "[Langtext]=@Langtext" + i + ","
					+ "[Langtext_drucken_AB]=@Langtext_drucken_AB" + i + ","
					+ "[Langtext_drucken_BW]=@Langtext_drucken_BW" + i + ","
					+ "[Lieferzeit]=@Lieferzeit" + i + ","
					+ "[Losgroesse]=@Losgroesse" + i + ","
					+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
					+ "[MHD]=@MHD" + i + ","
					+ "[Minerals Confirmity]=@Minerals_Confirmity" + i + ","
					+ "[Praeferenz_Aktuelles_jahr]=@Praeferenz_Aktuelles_jahr" + i + ","
					+ "[Praeferenz_Folgejahr]=@Praeferenz_Folgejahr" + i + ","
					+ "[Preiseinheit]=@Preiseinheit" + i + ","
					+ "[pro Zeiteinheit]=@pro_Zeiteinheit" + i + ","
					+ "[ProductionCountryName]=@ProductionCountryName" + i + ","
					+ "[ProductionCountrySequence]=@ProductionCountrySequence" + i + ","
					+ "[ProductionSiteName]=@ProductionSiteName" + i + ","
					+ "[ProductionSiteSequence]=@ProductionSiteSequence" + i + ","
					+ "[Produktionszeit]=@Produktionszeit" + i + ","
					+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
					+ "[Prüfstatus TN Ware]=@Prufstatus_TN_Ware" + i + ","
					+ "[Rabattierfähig]=@Rabattierfahig" + i + ","
					+ "[Rahmen]=@Rahmen" + i + ","
					+ "[Rahmen2]=@Rahmen2" + i + ","
					+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
					+ "[Rahmenauslauf2]=@Rahmenauslauf2" + i + ","
					+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
					+ "[Rahmenmenge2]=@Rahmenmenge2" + i + ","
					+ "[Rahmen-Nr]=@Rahmen_Nr" + i + ","
					+ "[Rahmen-Nr2]=@Rahmen_Nr2" + i + ","
					+ "[REACH SVHC Confirmity]=@REACH_SVHC_Confirmity" + i + ","
					+ "[ROHS EEE Confirmity]=@ROHS_EEE_Confirmity" + i + ","
					+ "[Seriennummer]=@Seriennummer" + i + ","
					+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
					+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
					+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
					+ "[Stückliste]=@Stuckliste" + i + ","
					+ "[Stundensatz]=@Stundensatz" + i + ","
					+ "[Sysmonummer]=@Sysmonummer" + i + ","
					+ "[UL Etikett]=@UL_Etikett" + i + ","
					+ "[UL zertifiziert]=@UL_zertifiziert" + i + ","
					+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
					+ "[Ursprungsland]=@Ursprungsland" + i + ","
					+ "[VDA_1]=@VDA_1" + i + ","
					+ "[VDA_2]=@VDA_2" + i + ","
					+ "[Verpackung]=@Verpackung" + i + ","
					+ "[Verpackungsart]=@Verpackungsart" + i + ","
					+ "[Verpackungsmenge]=@Verpackungsmenge" + i + ","
					+ "[VK-Festpreis]=@VK_Festpreis" + i + ","
					+ "[Volumen]=@Volumen" + i + ","
					+ "[Warengruppe]=@Warengruppe" + i + ","
					+ "[Warentyp]=@Warentyp" + i + ","
					+ "[Webshop]=@Webshop" + i + ","
					+ "[Werkzeug]=@Werkzeug" + i + ","
					+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
					+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
					+ "[Zeitraum_MHD]=@Zeitraum_MHD" + i + ","
					+ "[Zolltarif_nr]=@Zolltarif_nr" + i + ","
					+ "[Zuschlag_VK]=@Zuschlag_VK" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Abladestelle" + i, item.Abladestelle == null ? (object)DBNull.Value : item.Abladestelle);
					sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv == null ? (object)DBNull.Value : item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand == null ? (object)DBNull.Value : item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion == null ? (object)DBNull.Value : item.Artikel_aus_eigener_Produktion);
					sqlCommand.Parameters.AddWithValue("Artikel_fur_weitere_Bestellungen_sperren" + i, item.Artikel_fur_weitere_Bestellungen_sperren == null ? (object)DBNull.Value : item.Artikel_fur_weitere_Bestellungen_sperren);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("artikelklassifizierung" + i, item.artikelklassifizierung == null ? (object)DBNull.Value : item.artikelklassifizierung);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf == null ? (object)DBNull.Value : item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
					sqlCommand.Parameters.AddWithValue("BezeichnungAL" + i, item.BezeichnungAL == null ? (object)DBNull.Value : item.BezeichnungAL);
					sqlCommand.Parameters.AddWithValue("Blokiert_Status" + i, item.Blokiert_Status == null ? (object)DBNull.Value : item.Blokiert_Status);
					sqlCommand.Parameters.AddWithValue("COF_Pflichtig" + i, item.COF_Pflichtig == null ? (object)DBNull.Value : item.COF_Pflichtig);
					sqlCommand.Parameters.AddWithValue("CP_required" + i, item.CP_required == null ? (object)DBNull.Value : item.CP_required);
					sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
					sqlCommand.Parameters.AddWithValue("CustomerIndex" + i, item.CustomerIndex == null ? (object)DBNull.Value : item.CustomerIndex);
					sqlCommand.Parameters.AddWithValue("CustomerIndexSequence" + i, item.CustomerIndexSequence == null ? (object)DBNull.Value : item.CustomerIndexSequence);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumber" + i, item.CustomerItemNumber == null ? (object)DBNull.Value : item.CustomerItemNumber);
					sqlCommand.Parameters.AddWithValue("CustomerItemNumberSequence" + i, item.CustomerItemNumberSequence == null ? (object)DBNull.Value : item.CustomerItemNumberSequence);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerPrefix" + i, item.CustomerPrefix == null ? (object)DBNull.Value : item.CustomerPrefix);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert == null ? (object)DBNull.Value : item.DEL_fixiert);
					sqlCommand.Parameters.AddWithValue("Dienstelistung" + i, item.Dienstelistung == null ? (object)DBNull.Value : item.Dienstelistung);
					sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("EMPB" + i, item.EMPB == null ? (object)DBNull.Value : item.EMPB);
					sqlCommand.Parameters.AddWithValue("EMPB_Freigegeben" + i, item.EMPB_Freigegeben == null ? (object)DBNull.Value : item.EMPB_Freigegeben);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz == null ? (object)DBNull.Value : item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz_Text" + i, item.ESD_Schutz_Text == null ? (object)DBNull.Value : item.ESD_Schutz_Text);
					sqlCommand.Parameters.AddWithValue("Exportgewicht" + i, item.Exportgewicht == null ? (object)DBNull.Value : item.Exportgewicht);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stuckliste" + i, item.fakturieren_Stuckliste == null ? (object)DBNull.Value : item.fakturieren_Stuckliste);
					sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
					sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Grosse" + i, item.Grosse == null ? (object)DBNull.Value : item.Grosse);
					sqlCommand.Parameters.AddWithValue("Grund_fur_Sperre" + i, item.Grund_fur_Sperre == null ? (object)DBNull.Value : item.Grund_fur_Sperre);
					sqlCommand.Parameters.AddWithValue("gultig_bis" + i, item.gultig_bis == null ? (object)DBNull.Value : item.gultig_bis);
					sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Hubmastleitungen" + i, item.Hubmastleitungen == null ? (object)DBNull.Value : item.Hubmastleitungen);
					sqlCommand.Parameters.AddWithValue("ID_Klassifizierung" + i, item.ID_Klassifizierung == null ? (object)DBNull.Value : item.ID_Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Info_WE" + i, item.Info_WE == null ? (object)DBNull.Value : item.Info_WE);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Klassifizierung" + i, item.Klassifizierung == null ? (object)DBNull.Value : item.Klassifizierung);
					sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel == null ? (object)DBNull.Value : item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB == null ? (object)DBNull.Value : item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW == null ? (object)DBNull.Value : item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Lieferzeit" + i, item.Lieferzeit == null ? (object)DBNull.Value : item.Lieferzeit);
					sqlCommand.Parameters.AddWithValue("Losgroesse" + i, item.Losgroesse == null ? (object)DBNull.Value : item.Losgroesse);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("MHD" + i, item.MHD == null ? (object)DBNull.Value : item.MHD);
					sqlCommand.Parameters.AddWithValue("Minerals_Confirmity" + i, item.Minerals_Confirmity == null ? (object)DBNull.Value : item.Minerals_Confirmity);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Aktuelles_jahr" + i, item.Praeferenz_Aktuelles_jahr == null ? (object)DBNull.Value : item.Praeferenz_Aktuelles_jahr);
					sqlCommand.Parameters.AddWithValue("Praeferenz_Folgejahr" + i, item.Praeferenz_Folgejahr == null ? (object)DBNull.Value : item.Praeferenz_Folgejahr);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
					sqlCommand.Parameters.AddWithValue("ProductionCountryName" + i, item.ProductionCountryName == null ? (object)DBNull.Value : item.ProductionCountryName);
					sqlCommand.Parameters.AddWithValue("ProductionCountrySequence" + i, item.ProductionCountrySequence == null ? (object)DBNull.Value : item.ProductionCountrySequence);
					sqlCommand.Parameters.AddWithValue("ProductionSiteName" + i, item.ProductionSiteName == null ? (object)DBNull.Value : item.ProductionSiteName);
					sqlCommand.Parameters.AddWithValue("ProductionSiteSequence" + i, item.ProductionSiteSequence == null ? (object)DBNull.Value : item.ProductionSiteSequence);
					sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel == null ? (object)DBNull.Value : item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prufstatus_TN_Ware" + i, item.Prufstatus_TN_Ware == null ? (object)DBNull.Value : item.Prufstatus_TN_Ware);
					sqlCommand.Parameters.AddWithValue("Rabattierfahig" + i, item.Rabattierfahig == null ? (object)DBNull.Value : item.Rabattierfahig);
					sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen == null ? (object)DBNull.Value : item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmen2" + i, item.Rahmen2 == null ? (object)DBNull.Value : item.Rahmen2);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf2" + i, item.Rahmenauslauf2 == null ? (object)DBNull.Value : item.Rahmenauslauf2);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge2" + i, item.Rahmenmenge2 == null ? (object)DBNull.Value : item.Rahmenmenge2);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr2" + i, item.Rahmen_Nr2 == null ? (object)DBNull.Value : item.Rahmen_Nr2);
					sqlCommand.Parameters.AddWithValue("REACH_SVHC_Confirmity" + i, item.REACH_SVHC_Confirmity == null ? (object)DBNull.Value : item.REACH_SVHC_Confirmity);
					sqlCommand.Parameters.AddWithValue("ROHS_EEE_Confirmity" + i, item.ROHS_EEE_Confirmity == null ? (object)DBNull.Value : item.ROHS_EEE_Confirmity);
					sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung == null ? (object)DBNull.Value : item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stuckliste" + i, item.Stuckliste == null ? (object)DBNull.Value : item.Stuckliste);
					sqlCommand.Parameters.AddWithValue("Stundensatz" + i, item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);
					sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett == null ? (object)DBNull.Value : item.UL_Etikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert == null ? (object)DBNull.Value : item.UL_zertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("VDA_1" + i, item.VDA_1 == null ? (object)DBNull.Value : item.VDA_1);
					sqlCommand.Parameters.AddWithValue("VDA_2" + i, item.VDA_2 == null ? (object)DBNull.Value : item.VDA_2);
					sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("Verpackungsart" + i, item.Verpackungsart == null ? (object)DBNull.Value : item.Verpackungsart);
					sqlCommand.Parameters.AddWithValue("Verpackungsmenge" + i, item.Verpackungsmenge == null ? (object)DBNull.Value : item.Verpackungsmenge);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis == null ? (object)DBNull.Value : item.VK_Festpreis);
					sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Warentyp" + i, item.Warentyp == null ? (object)DBNull.Value : item.Warentyp);
					sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop == null ? (object)DBNull.Value : item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand == null ? (object)DBNull.Value : item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zeitraum_MHD" + i, item.Zeitraum_MHD == null ? (object)DBNull.Value : item.Zeitraum_MHD);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Zuschlag_VK" + i, item.Zuschlag_VK == null ? (object)DBNull.Value : item.Zuschlag_VK);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int artikel_nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr]=@Artikel_Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", artikel_nr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


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

				string query = "DELETE FROM [Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Entities.Tables.MTM.ArtikelEntity> GetFiltered(List<int> ids, string filter)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.MTM.ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getFiltered(ids, filter);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getFiltered(ids.GetRange(i * maxQueryNumber, maxQueryNumber), filter));
					}
					results.AddRange(getFiltered(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), filter));
				}
				return results?.OrderBy(x => x.Artikelnummer).Take(10).ToList();
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}
		private static List<Entities.Tables.MTM.ArtikelEntity> getFiltered(List<int> ids, string filter)
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

					sqlCommand.CommandText = $"SELECT * FROM [Artikel] WHERE [Artikel-Nr] IN ({queryIds}) AND Artikelnummer Like '{filter.SqlEscape()}%' AND Aktiv=1";

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.MTM.ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.MTM.ArtikelEntity>();
				}
			}
			return new List<Entities.Tables.MTM.ArtikelEntity>();
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> GetLikeNummer(string nummer, int? maxItemsCount = null)
		{
			nummer = nummer?.Trim() ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT {(maxItemsCount.HasValue ? $"TOP {maxItemsCount.Value}" : "")} * FROM [Artikel] WHERE [Artikelnummer] LIKE '{nummer.SqlEscape()}%' ORDER by [Artikelnummer] ASC";
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.MTM.ArtikelEntity> GetLikeNummerForAutocomplete(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT TOP 10 * FROM [Artikel] WHERE [Artikelnummer] LIKE '{nummer.SqlEscape()}%' AND [Stückliste]=1 AND [aktiv]=1 AND [Freigabestatus]<>'O' ORDER by [Artikelnummer] ASC";
					sqlCommand.Connection = sqlConnection;
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.ArtikelEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.MTM.ArtikelEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.MTM.ArtikelEntity(dataRow)); }
			return list;
		}

		public static Entities.Tables.MTM.ArtikelEntity GetByArtikelnummer(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Artikel] WHERE [Artikelnummer]=@artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("artikelnummer", artikelnummer);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// it takes ArtikelNr and ArtikelNummer and returns both
		/// </summary>
		/// <param name="ArtikleNr"></param>
		/// <param name="ArtikleNummer"></param>
		/// <param name="choice"></param>
		/// <returns></returns>
		public static List<Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity> GetArtikelNrOrArtikelNummer(int ArtikleNr = 0, string ArtikleNummer = "", bool choice = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				//paginationFilter
				string query = $@"";
				if(choice && !string.IsNullOrEmpty(ArtikleNummer))
				{
					query = $@"select top 1 a.[Artikel-Nr] , '{ArtikleNummer}' as Artikelnummer  from Artikel a where a.Artikelnummer = '{ArtikleNummer}' ";
				}
				else
				{
					query = $@"select top 1 a.Artikelnummer , {ArtikleNr} as [Artikel-Nr]  from Artikel a where a.[Artikel-Nr] = {ArtikleNr} ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity>();
			}
		}
		#endregion Custom Methods

	}
}
