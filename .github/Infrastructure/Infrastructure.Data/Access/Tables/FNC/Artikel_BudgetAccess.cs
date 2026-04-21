using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class Artikel_BudgetEnum
	{
		public enum ArtikelSearchColumns
		{
			[Description("Nr")]
			ArticleNr,
			[Description("Nummer")]
			ArticleNummer,
			[Description("Hall")]
			Hall,
			[Description("PriceGroup")]
			PriceGroup,
			[Description("Status")]
			Status,
			[Description("UnitPrice")]
			UnitPrice
		}
	}
	public class Artikel_BudgetAccess
	{

		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity Get(int artikel_nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Artikel] WHERE [Artikel-Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikel_nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = "SELECT * FROM [__FNC_Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Artikel] ([aktiv],[aktualisiert],[Anfangsbestand],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[Crossreferenz],[Cu-Gewicht],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dokumente],[EAN],[Einheit],[Ersatzartikel],[ESD_Schutz],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Index_Kunde],[Index_Kunde_Datum],[Kategorie],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Materialkosten_Alt],[Preiseinheit],[pro Zeiteinheit],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmenauslauf],[Rahmenmenge],[Rahmen-Nr],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[Verpackung],[VK-Festpreis],[Volumen],[Warengruppe],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zolltarif_nr])  VALUES (@aktiv,@aktualisiert,@Anfangsbestand,@Artikel_aus_eigener_Produktion,@Artikel_für_weitere_Bestellungen_sperren,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@Artikelkurztext,@Artikelnummer,@Barverkauf,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@Crossreferenz,@Cu_Gewicht,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dokumente,@EAN,@Einheit,@Ersatzartikel,@ESD_Schutz,@fakturieren_Stückliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Größe,@Grund_für_Sperre,@gültig_bis,@Halle,@Index_Kunde,@Index_Kunde_Datum,@Kategorie,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Materialkosten_Alt,@Preiseinheit,@pro_Zeiteinheit,@Produktionszeit,@Provisionsartikel,@Prüfstatus_TN_Ware,@Rabattierfähig,@Rahmen,@Rahmenauslauf,@Rahmenmenge,@Rahmen_Nr,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stückliste,@Stundensatz,@Sysmonummer,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@Verpackung,@VK_Festpreis,@Volumen,@Warengruppe,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zolltarif_nr)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv);
					sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion);
					sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren", item.Artikel_für_weitere_Bestellungen_sperren);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
					sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert);
					sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste", item.fakturieren_Stückliste);
					sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
					sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
					sqlCommand.Parameters.AddWithValue("Größe", item.Größe == null ? (object)DBNull.Value : item.Größe);
					sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
					sqlCommand.Parameters.AddWithValue("gültig_bis", item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
					sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
					sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
					sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
					sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
					sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel);
					sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware", item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : item.Prüfstatus_TN_Ware);
					sqlCommand.Parameters.AddWithValue("Rabattierfähig", item.Rabattierfähig);
					sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
					sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Stückliste", item.Stückliste);
					sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis);
					sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop);
					sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);

					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT [Artikel-Nr] FROM [__FNC_Artikel] WHERE [Artikel-Nr] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_Artikel] ([aktiv],[aktualisiert],[Anfangsbestand],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[Crossreferenz],[Cu-Gewicht],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dokumente],[EAN],[Einheit],[Ersatzartikel],[ESD_Schutz],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Index_Kunde],[Index_Kunde_Datum],[Kategorie],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Materialkosten_Alt],[Preiseinheit],[pro Zeiteinheit],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmenauslauf],[Rahmenmenge],[Rahmen-Nr],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[Verpackung],[VK-Festpreis],[Volumen],[Warengruppe],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zolltarif_nr]) VALUES ( "

							+ "@aktiv" + i + ","
							+ "@aktualisiert" + i + ","
							+ "@Anfangsbestand" + i + ","
							+ "@Artikel_aus_eigener_Produktion" + i + ","
							+ "@Artikel_für_weitere_Bestellungen_sperren" + i + ","
							+ "@Artikelfamilie_Kunde" + i + ","
							+ "@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "@Artikelkurztext" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Barverkauf" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@Bezeichnung_3" + i + ","
							+ "@Crossreferenz" + i + ","
							+ "@Cu_Gewicht" + i + ","
							+ "@Datum_Anfangsbestand" + i + ","
							+ "@DEL" + i + ","
							+ "@DEL_fixiert" + i + ","
							+ "@Dokumente" + i + ","
							+ "@EAN" + i + ","
							+ "@Einheit" + i + ","
							+ "@Ersatzartikel" + i + ","
							+ "@ESD_Schutz" + i + ","
							+ "@fakturieren_Stückliste" + i + ","
							+ "@Farbe" + i + ","
							+ "@fibu_rahmen" + i + ","
							+ "@Freigabestatus" + i + ","
							+ "@Freigabestatus_TN_intern" + i + ","
							+ "@Gebinde" + i + ","
							+ "@Gewicht" + i + ","
							+ "@Größe" + i + ","
							+ "@Grund_für_Sperre" + i + ","
							+ "@gültig_bis" + i + ","
							+ "@Halle" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@Index_Kunde_Datum" + i + ","
							+ "@Kategorie" + i + ","
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
							+ "@Materialkosten_Alt" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@pro_Zeiteinheit" + i + ","
							+ "@Produktionszeit" + i + ","
							+ "@Provisionsartikel" + i + ","
							+ "@Prüfstatus_TN_Ware" + i + ","
							+ "@Rabattierfähig" + i + ","
							+ "@Rahmen" + i + ","
							+ "@Rahmenauslauf" + i + ","
							+ "@Rahmenmenge" + i + ","
							+ "@Rahmen_Nr" + i + ","
							+ "@Seriennummer" + i + ","
							+ "@Seriennummernverwaltung" + i + ","
							+ "@Sonderrabatt" + i + ","
							+ "@Standard_Lagerort_id" + i + ","
							+ "@Stückliste" + i + ","
							+ "@Stundensatz" + i + ","
							+ "@Sysmonummer" + i + ","
							+ "@UL_Etikett" + i + ","
							+ "@UL_zertifiziert" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@Ursprungsland" + i + ","
							+ "@Verpackung" + i + ","
							+ "@VK_Festpreis" + i + ","
							+ "@Volumen" + i + ","
							+ "@Warengruppe" + i + ","
							+ "@Webshop" + i + ","
							+ "@Werkzeug" + i + ","
							+ "@Wert_Anfangsbestand" + i + ","
							+ "@Zeichnungsnummer" + i + ","
							+ "@Zolltarif_nr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion);
						sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren" + i, item.Artikel_für_weitere_Bestellungen_sperren);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste" + i, item.fakturieren_Stückliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Größe" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
						sqlCommand.Parameters.AddWithValue("Grund_für_Sperre" + i, item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
						sqlCommand.Parameters.AddWithValue("gültig_bis" + i, item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware" + i, item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : item.Prüfstatus_TN_Ware);
						sqlCommand.Parameters.AddWithValue("Rabattierfähig" + i, item.Rabattierfähig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stückliste" + i, item.Stückliste);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Artikel] SET [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_für_weitere_Bestellungen_sperren, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dokumente]=@Dokumente, [EAN]=@EAN, [Einheit]=@Einheit, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [fakturieren Stückliste]=@fakturieren_Stückliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Größe, [Grund für Sperre]=@Grund_für_Sperre, [gültig bis]=@gültig_bis, [Halle]=@Halle, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Kategorie]=@Kategorie, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Materialkosten_Alt]=@Materialkosten_Alt, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [Produktionszeit]=@Produktionszeit, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prüfstatus_TN_Ware, [Rabattierfähig]=@Rabattierfähig, [Rahmen]=@Rahmen, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenmenge]=@Rahmenmenge, [Rahmen-Nr]=@Rahmen_Nr, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stückliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [Verpackung]=@Verpackung, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zolltarif_nr]=@Zolltarif_nr WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("aktiv", item.aktiv);
				sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
				sqlCommand.Parameters.AddWithValue("Anfangsbestand", item.Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", item.Artikel_aus_eigener_Produktion);
				sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren", item.Artikel_für_weitere_Bestellungen_sperren);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
				sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Barverkauf", item.Barverkauf);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
				sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
				sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
				sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : item.DEL);
				sqlCommand.Parameters.AddWithValue("DEL_fixiert", item.DEL_fixiert);
				sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
				sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz);
				sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste", item.fakturieren_Stückliste);
				sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
				sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
				sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
				sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
				sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
				sqlCommand.Parameters.AddWithValue("Größe", item.Größe == null ? (object)DBNull.Value : item.Größe);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
				sqlCommand.Parameters.AddWithValue("gültig_bis", item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
				sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
				sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
				sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
				sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
				sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
				sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
				sqlCommand.Parameters.AddWithValue("Lagerartikel", item.Lagerartikel);
				sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
				sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", item.Langtext_drucken_AB);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", item.Langtext_drucken_BW);
				sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
				sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
				sqlCommand.Parameters.AddWithValue("Provisionsartikel", item.Provisionsartikel);
				sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware", item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : item.Prüfstatus_TN_Ware);
				sqlCommand.Parameters.AddWithValue("Rabattierfähig", item.Rabattierfähig);
				sqlCommand.Parameters.AddWithValue("Rahmen", item.Rahmen);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
				sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
				sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", item.Seriennummernverwaltung);
				sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
				sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Stückliste", item.Stückliste);
				sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
				sqlCommand.Parameters.AddWithValue("UL_Etikett", item.UL_Etikett);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert", item.UL_zertifiziert);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
				sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
				sqlCommand.Parameters.AddWithValue("VK_Festpreis", item.VK_Festpreis);
				sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
				sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Webshop", item.Webshop);
				sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
				sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", item.Wert_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 81; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_Artikel] SET "

							+ "[aktiv]=@aktiv" + i + ","
							+ "[aktualisiert]=@aktualisiert" + i + ","
							+ "[Anfangsbestand]=@Anfangsbestand" + i + ","
							+ "[Artikel aus eigener Produktion]=@Artikel aus eigener Produktion" + i + ","
							+ "[Artikel für weitere Bestellungen sperren]=@Artikel für weitere Bestellungen sperren" + i + ","
							+ "[Artikelfamilie_Kunde]=@Artikelfamilie_Kunde" + i + ","
							+ "[Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1" + i + ","
							+ "[Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2" + i + ","
							+ "[Artikelkurztext]=@Artikelkurztext" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Barverkauf]=@Barverkauf" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung 1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung 2" + i + ","
							+ "[Bezeichnung 3]=@Bezeichnung 3" + i + ","
							+ "[Crossreferenz]=@Crossreferenz" + i + ","
							+ "[Cu-Gewicht]=@Cu-Gewicht" + i + ","
							+ "[Datum Anfangsbestand]=@Datum Anfangsbestand" + i + ","
							+ "[DEL]=@DEL" + i + ","
							+ "[DEL fixiert]=@DEL fixiert" + i + ","
							+ "[Dokumente]=@Dokumente" + i + ","
							+ "[EAN]=@EAN" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[Ersatzartikel]=@Ersatzartikel" + i + ","
							+ "[ESD_Schutz]=@ESD_Schutz" + i + ","
							+ "[fakturieren Stückliste]=@fakturieren Stückliste" + i + ","
							+ "[Farbe]=@Farbe" + i + ","
							+ "[fibu_rahmen]=@fibu_rahmen" + i + ","
							+ "[Freigabestatus]=@Freigabestatus" + i + ","
							+ "[Freigabestatus TN intern]=@Freigabestatus TN intern" + i + ","
							+ "[Gebinde]=@Gebinde" + i + ","
							+ "[Gewicht]=@Gewicht" + i + ","
							+ "[Größe]=@Größe" + i + ","
							+ "[Grund für Sperre]=@Grund für Sperre" + i + ","
							+ "[gültig bis]=@gültig bis" + i + ","
							+ "[Halle]=@Halle" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[Index_Kunde_Datum]=@Index_Kunde_Datum" + i + ","
							+ "[Kategorie]=@Kategorie" + i + ","
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
							+ "[Materialkosten_Alt]=@Materialkosten_Alt" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[pro Zeiteinheit]=@pro Zeiteinheit" + i + ","
							+ "[Produktionszeit]=@Produktionszeit" + i + ","
							+ "[Provisionsartikel]=@Provisionsartikel" + i + ","
							+ "[Prüfstatus TN Ware]=@Prüfstatus TN Ware" + i + ","
							+ "[Rabattierfähig]=@Rabattierfähig" + i + ","
							+ "[Rahmen]=@Rahmen" + i + ","
							+ "[Rahmenauslauf]=@Rahmenauslauf" + i + ","
							+ "[Rahmenmenge]=@Rahmenmenge" + i + ","
							+ "[Rahmen-Nr]=@Rahmen-Nr" + i + ","
							+ "[Seriennummer]=@Seriennummer" + i + ","
							+ "[Seriennummernverwaltung]=@Seriennummernverwaltung" + i + ","
							+ "[Sonderrabatt]=@Sonderrabatt" + i + ","
							+ "[Standard_Lagerort_id]=@Standard_Lagerort_id" + i + ","
							+ "[Stückliste]=@Stückliste" + i + ","
							+ "[Stundensatz]=@Stundensatz" + i + ","
							+ "[Sysmonummer]=@Sysmonummer" + i + ","
							+ "[UL Etikett]=@UL Etikett" + i + ","
							+ "[UL zertifiziert]=@UL zertifiziert" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[Ursprungsland]=@Ursprungsland" + i + ","
							+ "[Verpackung]=@Verpackung" + i + ","
							+ "[VK-Festpreis]=@VK-Festpreis" + i + ","
							+ "[Volumen]=@Volumen" + i + ","
							+ "[Warengruppe]=@Warengruppe" + i + ","
							+ "[Webshop]=@Webshop" + i + ","
							+ "[Werkzeug]=@Werkzeug" + i + ","
							+ "[Wert_Anfangsbestand]=@Wert_Anfangsbestand" + i + ","
							+ "[Zeichnungsnummer]=@Zeichnungsnummer" + i + ","
							+ "[Zolltarif_nr]=@Zolltarif_nr" + i + " WHERE [Artikel-Nr]=@Artikel_Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("aktiv" + i, item.aktiv);
						sqlCommand.Parameters.AddWithValue("aktualisiert" + i, item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
						sqlCommand.Parameters.AddWithValue("Anfangsbestand" + i, item.Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion" + i, item.Artikel_aus_eigener_Produktion);
						sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren" + i, item.Artikel_für_weitere_Bestellungen_sperren);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde" + i, item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1" + i, item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
						sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2" + i, item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
						sqlCommand.Parameters.AddWithValue("Artikelkurztext" + i, item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Barverkauf" + i, item.Barverkauf);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_3" + i, item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
						sqlCommand.Parameters.AddWithValue("Crossreferenz" + i, item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
						sqlCommand.Parameters.AddWithValue("Cu_Gewicht" + i, item.Cu_Gewicht == null ? (object)DBNull.Value : item.Cu_Gewicht);
						sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand" + i, item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("DEL" + i, item.DEL == null ? (object)DBNull.Value : item.DEL);
						sqlCommand.Parameters.AddWithValue("DEL_fixiert" + i, item.DEL_fixiert);
						sqlCommand.Parameters.AddWithValue("Dokumente" + i, item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
						sqlCommand.Parameters.AddWithValue("EAN" + i, item.EAN == null ? (object)DBNull.Value : item.EAN);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Ersatzartikel" + i, item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
						sqlCommand.Parameters.AddWithValue("ESD_Schutz" + i, item.ESD_Schutz);
						sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste" + i, item.fakturieren_Stückliste);
						sqlCommand.Parameters.AddWithValue("Farbe" + i, item.Farbe == null ? (object)DBNull.Value : item.Farbe);
						sqlCommand.Parameters.AddWithValue("fibu_rahmen" + i, item.fibu_rahmen == null ? (object)DBNull.Value : item.fibu_rahmen);
						sqlCommand.Parameters.AddWithValue("Freigabestatus" + i, item.Freigabestatus == null ? (object)DBNull.Value : item.Freigabestatus);
						sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern" + i, item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : item.Freigabestatus_TN_intern);
						sqlCommand.Parameters.AddWithValue("Gebinde" + i, item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
						sqlCommand.Parameters.AddWithValue("Gewicht" + i, item.Gewicht == null ? (object)DBNull.Value : item.Gewicht);
						sqlCommand.Parameters.AddWithValue("Größe" + i, item.Größe == null ? (object)DBNull.Value : item.Größe);
						sqlCommand.Parameters.AddWithValue("Grund_für_Sperre" + i, item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
						sqlCommand.Parameters.AddWithValue("gültig_bis" + i, item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
						sqlCommand.Parameters.AddWithValue("Halle" + i, item.Halle == null ? (object)DBNull.Value : item.Halle);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum" + i, item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
						sqlCommand.Parameters.AddWithValue("Kategorie" + i, item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
						sqlCommand.Parameters.AddWithValue("Kriterium1" + i, item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
						sqlCommand.Parameters.AddWithValue("Kriterium2" + i, item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
						sqlCommand.Parameters.AddWithValue("Kriterium3" + i, item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
						sqlCommand.Parameters.AddWithValue("Kriterium4" + i, item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
						sqlCommand.Parameters.AddWithValue("Kupferbasis" + i, item.Kupferbasis == null ? (object)DBNull.Value : item.Kupferbasis);
						sqlCommand.Parameters.AddWithValue("Kupferzahl" + i, item.Kupferzahl == null ? (object)DBNull.Value : item.Kupferzahl);
						sqlCommand.Parameters.AddWithValue("Lagerartikel" + i, item.Lagerartikel);
						sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten" + i, item.Lagerhaltungskosten == null ? (object)DBNull.Value : item.Lagerhaltungskosten);
						sqlCommand.Parameters.AddWithValue("Langtext" + i, item.Langtext == null ? (object)DBNull.Value : item.Langtext);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB" + i, item.Langtext_drucken_AB);
						sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW" + i, item.Langtext_drucken_BW);
						sqlCommand.Parameters.AddWithValue("Materialkosten_Alt" + i, item.Materialkosten_Alt == null ? (object)DBNull.Value : item.Materialkosten_Alt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit" + i, item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
						sqlCommand.Parameters.AddWithValue("Produktionszeit" + i, item.Produktionszeit == null ? (object)DBNull.Value : item.Produktionszeit);
						sqlCommand.Parameters.AddWithValue("Provisionsartikel" + i, item.Provisionsartikel);
						sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware" + i, item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : item.Prüfstatus_TN_Ware);
						sqlCommand.Parameters.AddWithValue("Rabattierfähig" + i, item.Rabattierfähig);
						sqlCommand.Parameters.AddWithValue("Rahmen" + i, item.Rahmen);
						sqlCommand.Parameters.AddWithValue("Rahmenauslauf" + i, item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
						sqlCommand.Parameters.AddWithValue("Rahmenmenge" + i, item.Rahmenmenge == null ? (object)DBNull.Value : item.Rahmenmenge);
						sqlCommand.Parameters.AddWithValue("Rahmen_Nr" + i, item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
						sqlCommand.Parameters.AddWithValue("Seriennummer" + i, item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
						sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung" + i, item.Seriennummernverwaltung);
						sqlCommand.Parameters.AddWithValue("Sonderrabatt" + i, item.Sonderrabatt == null ? (object)DBNull.Value : item.Sonderrabatt);
						sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id" + i, item.Standard_Lagerort_id == null ? (object)DBNull.Value : item.Standard_Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Stückliste" + i, item.Stückliste);
						sqlCommand.Parameters.AddWithValue("Sysmonummer" + i, item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
						sqlCommand.Parameters.AddWithValue("UL_Etikett" + i, item.UL_Etikett);
						sqlCommand.Parameters.AddWithValue("UL_zertifiziert" + i, item.UL_zertifiziert);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Ursprungsland" + i, item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
						sqlCommand.Parameters.AddWithValue("Verpackung" + i, item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
						sqlCommand.Parameters.AddWithValue("VK_Festpreis" + i, item.VK_Festpreis);
						sqlCommand.Parameters.AddWithValue("Volumen" + i, item.Volumen == null ? (object)DBNull.Value : item.Volumen);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Webshop" + i, item.Webshop);
						sqlCommand.Parameters.AddWithValue("Werkzeug" + i, item.Werkzeug == null ? (object)DBNull.Value : item.Werkzeug);
						sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand" + i, item.Wert_Anfangsbestand);
						sqlCommand.Parameters.AddWithValue("Zeichnungsnummer" + i, item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
						sqlCommand.Parameters.AddWithValue("Zolltarif_nr" + i, item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int artikel_nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Artikel] WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", artikel_nr);

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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_Artikel] WHERE [Artikel-Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> GetByOrderId(List<int> id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_Artikel] where [Artikel-Nr] in ({string.Join(",", id)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity> GetArtikelOrder()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT CONCAT(Artikelnummer,'--',[Bezeichnung 1],'--',[Bezeichnung 2]) AS bindName, [Artikel-Nr] as Artikel_Nr, [Artikelnummer] as Artikelnummer, [Bezeichnung 1] as Article_Name1, [Bezeichnung 2]as Article_Name2 FROM [__FNC_Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListArtikelOrder(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> GetAllDataArtikel(string searchTerm = null, string sortColumn = "nr", bool sortDesc = false, bool? supplierRef = false, int page = 0, int pageSize = 100)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = $"select * from [__FNC_Artikel] ";
				if(!string.IsNullOrWhiteSpace(searchTerm))
				{
					searchTerm = searchTerm.Trim();
					query += $" WHERE Artikelnummer LIKE '%{searchTerm}%' OR [Bezeichnung 1]  LIKE '%{searchTerm}%' OR [Bezeichnung 2]  LIKE '%{searchTerm}%' OR [Bezeichnung 3]  LIKE '%{searchTerm}%' OR [Artikel-Nr]  LIKE '%{searchTerm}%'";

					if(supplierRef.HasValue && supplierRef.Value)
						query += $" OR [Artikel-Nr] IN (SELECT DISTINCT [Artikel-Nr] FROM [Bestellnummern] WHERE [Bestell-Nr] LIKE '%{searchTerm}%')";
				}
				query += $" ORDER BY [{getSearchColumn(getSearchColumn(sortColumn))}] {(sortDesc == true ? "DESC" : "ASC")} OFFSET {page * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 120;

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListAllData(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>();
			}
		}
		public static int GetAllDataArtikel_Count(string searchTerm = null, bool? supplierRef = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = $"select COUNT(*) from [__FNC_Artikel] ";
				if(!string.IsNullOrWhiteSpace(searchTerm))
				{
					searchTerm = searchTerm.Trim();
					query += $" WHERE Artikelnummer LIKE '%{searchTerm}%' OR [Bezeichnung 1]  LIKE '%{searchTerm}%' OR [Bezeichnung 2]  LIKE '%{searchTerm}%' OR [Bezeichnung 3]  LIKE '%{searchTerm}%' OR [Artikel-Nr]  LIKE '%{searchTerm}%' ";

					if(supplierRef.HasValue && supplierRef.Value)
						query += $" OR [Artikel-Nr] IN (SELECT DISTINCT [Artikel-Nr] FROM [Bestellnummern] WHERE [Bestell-Nr] LIKE '%{searchTerm}%')";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);
				var result = sqlCommand.ExecuteScalar();

				return int.TryParse(result.ToString(), out var _result) ? _result : 0;
			}
		}

		public static int InsertArtikel(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Artikel] ([aktiv],[aktualisiert],[Anfangsbestand],[Artikel aus eigener Produktion],[Artikel für weitere Bestellungen sperren],[Artikelfamilie_Kunde],[Artikelfamilie_Kunde_Detail1],[Artikelfamilie_Kunde_Detail2],[Artikelkurztext],[Artikelnummer],[Barverkauf],[Bezeichnung 1],[Bezeichnung 2],[Bezeichnung 3],[Crossreferenz],[Cu-Gewicht],[Datum Anfangsbestand],[DEL],[DEL fixiert],[Dokumente],[EAN],[Einheit],[Ersatzartikel],[ESD_Schutz],[fakturieren Stückliste],[Farbe],[fibu_rahmen],[Freigabestatus],[Freigabestatus TN intern],[Gebinde],[Gewicht],[Größe],[Grund für Sperre],[gültig bis],[Halle],[Index_Kunde],[Index_Kunde_Datum],[Kategorie],[Kriterium1],[Kriterium2],[Kriterium3],[Kriterium4],[Kupferbasis],[Kupferzahl],[Lagerartikel],[Lagerhaltungskosten],[Langtext],[Langtext_drucken_AB],[Langtext_drucken_BW],[Materialkosten_Alt],[Preiseinheit],[pro Zeiteinheit],[Produktionszeit],[Provisionsartikel],[Prüfstatus TN Ware],[Rabattierfähig],[Rahmen],[Rahmenauslauf],[Rahmenmenge],[Rahmen-Nr],[Seriennummer],[Seriennummernverwaltung],[Sonderrabatt],[Standard_Lagerort_id],[Stückliste],[Stundensatz],[Sysmonummer],[UL Etikett],[UL zertifiziert],[Umsatzsteuer],[Ursprungsland],[Verpackung],[VK-Festpreis],[Volumen],[Warengruppe],[Webshop],[Werkzeug],[Wert_Anfangsbestand],[Zeichnungsnummer],[Zolltarif_nr])  "
					+ "VALUES (@aktiv,@aktualisiert,@Anfangsbestand,@Artikel_aus_eigener_Produktion,@Artikel_für_weitere_Bestellungen_sperren,@Artikelfamilie_Kunde,@Artikelfamilie_Kunde_Detail1,@Artikelfamilie_Kunde_Detail2,@Artikelkurztext,@Artikelnummer,@Barverkauf,@Bezeichnung_1,@Bezeichnung_2,@Bezeichnung_3,@Crossreferenz,@Cu_Gewicht,@Datum_Anfangsbestand,@DEL,@DEL_fixiert,@Dokumente,@EAN,@Einheit,@Ersatzartikel,@ESD_Schutz,@fakturieren_Stückliste,@Farbe,@fibu_rahmen,@Freigabestatus,@Freigabestatus_TN_intern,@Gebinde,@Gewicht,@Größe,@Grund_für_Sperre,@gültig_bis,@Halle,@Index_Kunde,@Index_Kunde_Datum,@Kategorie,@Kriterium1,@Kriterium2,@Kriterium3,@Kriterium4,@Kupferbasis,@Kupferzahl,@Lagerartikel,@Lagerhaltungskosten,@Langtext,@Langtext_drucken_AB,@Langtext_drucken_BW,@Materialkosten_Alt,@Preiseinheit,@pro_Zeiteinheit,@Produktionszeit,@Provisionsartikel,@Prüfstatus_TN_Ware,@Rabattierfähig,@Rahmen,@Rahmenauslauf,@Rahmenmenge,@Rahmen_Nr,@Seriennummer,@Seriennummernverwaltung,@Sonderrabatt,@Standard_Lagerort_id,@Stückliste,@Stundensatz,@Sysmonummer,@UL_Etikett,@UL_zertifiziert,@Umsatzsteuer,@Ursprungsland,@Verpackung,@VK_Festpreis,@Volumen,@Warengruppe,@Webshop,@Werkzeug,@Wert_Anfangsbestand,@Zeichnungsnummer,@Zolltarif_nr)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("aktiv", 1);
					sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
					sqlCommand.Parameters.AddWithValue("Anfangsbestand", 0);
					sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", 0);
					sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren", 0);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
					sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
					sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Barverkauf", 0);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
					sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
					sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
					sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("DEL_fixiert", 0);
					sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
					sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : "St");
					sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
					sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz);
					sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste", 0);
					sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
					sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : "N");
					sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : "N");
					sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
					sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Größe", item.Größe == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
					sqlCommand.Parameters.AddWithValue("gültig_bis", item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
					sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
					sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
					sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
					sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
					sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
					sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
					sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Lagerartikel", 0);
					sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", 0);
					sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", 0);
					sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
					sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Provisionsartikel", 0);
					sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware", item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : "N");
					sqlCommand.Parameters.AddWithValue("Rabattierfähig", 0);
					sqlCommand.Parameters.AddWithValue("Rahmen", 0);
					sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
					sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
					sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
					sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", 0);
					sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : 0);
					sqlCommand.Parameters.AddWithValue("Stückliste", 0);
					sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
					sqlCommand.Parameters.AddWithValue("UL_Etikett", 0);
					sqlCommand.Parameters.AddWithValue("UL_zertifiziert", 0);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
					sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
					sqlCommand.Parameters.AddWithValue("VK_Festpreis", 1);
					sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
					sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Webshop", 0);
					sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : "na");
					sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", 0);
					sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
					sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
					sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);


					sqlCommand.ExecuteNonQuery();
				}

				using(var sqlCommand = new SqlCommand("SELECT MAX([Artikel-Nr]) FROM [__FNC_Artikel] ", sqlConnection, sqlTransaction))
				{
					response = int.Parse(sqlCommand.ExecuteScalar()?.ToString() ?? "-1");
				}


				sqlTransaction.Commit();

				return response;
			}
		}

		public static int DeleteArtikel(int article_number)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Artikel] WHERE [Artikel-Nr]=@Article_number";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Article_number", article_number);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static int UpdateArtikel(Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Artikel] SET [aktiv]=@aktiv, [aktualisiert]=@aktualisiert, [Anfangsbestand]=@Anfangsbestand, [Artikel aus eigener Produktion]=@Artikel_aus_eigener_Produktion, [Artikel für weitere Bestellungen sperren]=@Artikel_für_weitere_Bestellungen_sperren, [Artikelfamilie_Kunde]=@Artikelfamilie_Kunde, [Artikelfamilie_Kunde_Detail1]=@Artikelfamilie_Kunde_Detail1, [Artikelfamilie_Kunde_Detail2]=@Artikelfamilie_Kunde_Detail2, [Artikelkurztext]=@Artikelkurztext, [Artikelnummer]=@Artikelnummer, [Barverkauf]=@Barverkauf, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [Bezeichnung 3]=@Bezeichnung_3, [Crossreferenz]=@Crossreferenz, [Cu-Gewicht]=@Cu_Gewicht, [Datum Anfangsbestand]=@Datum_Anfangsbestand, [DEL]=@DEL, [DEL fixiert]=@DEL_fixiert, [Dokumente]=@Dokumente, [EAN]=@EAN, [Einheit]=@Einheit, [Ersatzartikel]=@Ersatzartikel, [ESD_Schutz]=@ESD_Schutz, [fakturieren Stückliste]=@fakturieren_Stückliste, [Farbe]=@Farbe, [fibu_rahmen]=@fibu_rahmen, [Freigabestatus]=@Freigabestatus, [Freigabestatus TN intern]=@Freigabestatus_TN_intern, [Gebinde]=@Gebinde, [Gewicht]=@Gewicht, [Größe]=@Größe, [Grund für Sperre]=@Grund_für_Sperre, [gültig bis]=@gültig_bis, [Halle]=@Halle, [Index_Kunde]=@Index_Kunde, [Index_Kunde_Datum]=@Index_Kunde_Datum, [Kategorie]=@Kategorie, [Kriterium1]=@Kriterium1, [Kriterium2]=@Kriterium2, [Kriterium3]=@Kriterium3, [Kriterium4]=@Kriterium4, [Kupferbasis]=@Kupferbasis, [Kupferzahl]=@Kupferzahl, [Lagerartikel]=@Lagerartikel, [Lagerhaltungskosten]=@Lagerhaltungskosten, [Langtext]=@Langtext, [Langtext_drucken_AB]=@Langtext_drucken_AB, [Langtext_drucken_BW]=@Langtext_drucken_BW, [Materialkosten_Alt]=@Materialkosten_Alt, [Preiseinheit]=@Preiseinheit, [pro Zeiteinheit]=@pro_Zeiteinheit, [Produktionszeit]=@Produktionszeit, [Provisionsartikel]=@Provisionsartikel, [Prüfstatus TN Ware]=@Prüfstatus_TN_Ware, [Rabattierfähig]=@Rabattierfähig, [Rahmen]=@Rahmen, [Rahmenauslauf]=@Rahmenauslauf, [Rahmenmenge]=@Rahmenmenge, [Rahmen-Nr]=@Rahmen_Nr, [Seriennummer]=@Seriennummer, [Seriennummernverwaltung]=@Seriennummernverwaltung, [Sonderrabatt]=@Sonderrabatt, [Standard_Lagerort_id]=@Standard_Lagerort_id, [Stückliste]=@Stückliste, [Stundensatz]=@Stundensatz, [Sysmonummer]=@Sysmonummer, [UL Etikett]=@UL_Etikett, [UL zertifiziert]=@UL_zertifiziert, [Umsatzsteuer]=@Umsatzsteuer, [Ursprungsland]=@Ursprungsland, [Verpackung]=@Verpackung, [VK-Festpreis]=@VK_Festpreis, [Volumen]=@Volumen, [Warengruppe]=@Warengruppe, [Webshop]=@Webshop, [Werkzeug]=@Werkzeug, [Wert_Anfangsbestand]=@Wert_Anfangsbestand, [Zeichnungsnummer]=@Zeichnungsnummer, [Zolltarif_nr]=@Zolltarif_nr WHERE [Artikel-Nr]=@Artikel_Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("aktiv", 1);
				sqlCommand.Parameters.AddWithValue("aktualisiert", item.aktualisiert == null ? (object)DBNull.Value : item.aktualisiert);
				sqlCommand.Parameters.AddWithValue("Anfangsbestand", 0);
				sqlCommand.Parameters.AddWithValue("Artikel_aus_eigener_Produktion", 0);
				sqlCommand.Parameters.AddWithValue("Artikel_für_weitere_Bestellungen_sperren", 0);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde", item.Artikelfamilie_Kunde == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail1", item.Artikelfamilie_Kunde_Detail1 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail1);
				sqlCommand.Parameters.AddWithValue("Artikelfamilie_Kunde_Detail2", item.Artikelfamilie_Kunde_Detail2 == null ? (object)DBNull.Value : item.Artikelfamilie_Kunde_Detail2);
				sqlCommand.Parameters.AddWithValue("Artikelkurztext", item.Artikelkurztext == null ? (object)DBNull.Value : item.Artikelkurztext);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Barverkauf", 0);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_3", item.Bezeichnung_3 == null ? (object)DBNull.Value : item.Bezeichnung_3);
				sqlCommand.Parameters.AddWithValue("Crossreferenz", item.Crossreferenz == null ? (object)DBNull.Value : item.Crossreferenz);
				sqlCommand.Parameters.AddWithValue("Cu_Gewicht", item.Cu_Gewicht == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Datum_Anfangsbestand", item.Datum_Anfangsbestand == null ? (object)DBNull.Value : item.Datum_Anfangsbestand);
				sqlCommand.Parameters.AddWithValue("DEL", item.DEL == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("DEL_fixiert", 0);
				sqlCommand.Parameters.AddWithValue("Dokumente", item.Dokumente == null ? (object)DBNull.Value : item.Dokumente);
				sqlCommand.Parameters.AddWithValue("EAN", item.EAN == null ? (object)DBNull.Value : item.EAN);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : "St");
				sqlCommand.Parameters.AddWithValue("Ersatzartikel", item.Ersatzartikel == null ? (object)DBNull.Value : item.Ersatzartikel);
				sqlCommand.Parameters.AddWithValue("ESD_Schutz", item.ESD_Schutz);
				sqlCommand.Parameters.AddWithValue("fakturieren_Stückliste", 0);
				sqlCommand.Parameters.AddWithValue("Farbe", item.Farbe == null ? (object)DBNull.Value : item.Farbe);
				sqlCommand.Parameters.AddWithValue("fibu_rahmen", item.fibu_rahmen == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Freigabestatus", item.Freigabestatus == null ? (object)DBNull.Value : "N");
				sqlCommand.Parameters.AddWithValue("Freigabestatus_TN_intern", item.Freigabestatus_TN_intern == null ? (object)DBNull.Value : "N");
				sqlCommand.Parameters.AddWithValue("Gebinde", item.Gebinde == null ? (object)DBNull.Value : item.Gebinde);
				sqlCommand.Parameters.AddWithValue("Gewicht", item.Gewicht == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Größe", item.Größe == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Grund_für_Sperre", item.Grund_für_Sperre == null ? (object)DBNull.Value : item.Grund_für_Sperre);
				sqlCommand.Parameters.AddWithValue("gültig_bis", item.gültig_bis == null ? (object)DBNull.Value : item.gültig_bis);
				sqlCommand.Parameters.AddWithValue("Halle", item.Halle == null ? (object)DBNull.Value : item.Halle);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde == null ? (object)DBNull.Value : item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Index_Kunde_Datum", item.Index_Kunde_Datum == null ? (object)DBNull.Value : item.Index_Kunde_Datum);
				sqlCommand.Parameters.AddWithValue("Kategorie", item.Kategorie == null ? (object)DBNull.Value : item.Kategorie);
				sqlCommand.Parameters.AddWithValue("Kriterium1", item.Kriterium1 == null ? (object)DBNull.Value : item.Kriterium1);
				sqlCommand.Parameters.AddWithValue("Kriterium2", item.Kriterium2 == null ? (object)DBNull.Value : item.Kriterium2);
				sqlCommand.Parameters.AddWithValue("Kriterium3", item.Kriterium3 == null ? (object)DBNull.Value : item.Kriterium3);
				sqlCommand.Parameters.AddWithValue("Kriterium4", item.Kriterium4 == null ? (object)DBNull.Value : item.Kriterium4);
				sqlCommand.Parameters.AddWithValue("Kupferbasis", item.Kupferbasis == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Kupferzahl", item.Kupferzahl == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Lagerartikel", 0);
				sqlCommand.Parameters.AddWithValue("Lagerhaltungskosten", item.Lagerhaltungskosten == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Langtext", item.Langtext == null ? (object)DBNull.Value : item.Langtext);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_AB", 0);
				sqlCommand.Parameters.AddWithValue("Langtext_drucken_BW", 0);
				sqlCommand.Parameters.AddWithValue("Materialkosten_Alt", item.Materialkosten_Alt == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("pro_Zeiteinheit", item.pro_Zeiteinheit == null ? (object)DBNull.Value : item.pro_Zeiteinheit);
				sqlCommand.Parameters.AddWithValue("Produktionszeit", item.Produktionszeit == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Provisionsartikel", 0);
				sqlCommand.Parameters.AddWithValue("Prüfstatus_TN_Ware", item.Prüfstatus_TN_Ware == null ? (object)DBNull.Value : "N");
				sqlCommand.Parameters.AddWithValue("Rabattierfähig", 0);
				sqlCommand.Parameters.AddWithValue("Rahmen", 0);
				sqlCommand.Parameters.AddWithValue("Rahmenauslauf", item.Rahmenauslauf == null ? (object)DBNull.Value : item.Rahmenauslauf);
				sqlCommand.Parameters.AddWithValue("Rahmenmenge", item.Rahmenmenge == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Rahmen_Nr", item.Rahmen_Nr == null ? (object)DBNull.Value : item.Rahmen_Nr);
				sqlCommand.Parameters.AddWithValue("Seriennummer", item.Seriennummer == null ? (object)DBNull.Value : item.Seriennummer);
				sqlCommand.Parameters.AddWithValue("Seriennummernverwaltung", 0);
				sqlCommand.Parameters.AddWithValue("Sonderrabatt", item.Sonderrabatt == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Standard_Lagerort_id", item.Standard_Lagerort_id == null ? (object)DBNull.Value : 0);
				sqlCommand.Parameters.AddWithValue("Stückliste", 0);
				sqlCommand.Parameters.AddWithValue("Sysmonummer", item.Sysmonummer == null ? (object)DBNull.Value : item.Sysmonummer);
				sqlCommand.Parameters.AddWithValue("UL_Etikett", 0);
				sqlCommand.Parameters.AddWithValue("UL_zertifiziert", 0);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Ursprungsland", item.Ursprungsland == null ? (object)DBNull.Value : item.Ursprungsland);
				sqlCommand.Parameters.AddWithValue("Verpackung", item.Verpackung == null ? (object)DBNull.Value : item.Verpackung);
				sqlCommand.Parameters.AddWithValue("VK_Festpreis", 1);
				sqlCommand.Parameters.AddWithValue("Volumen", item.Volumen == null ? (object)DBNull.Value : item.Volumen);
				sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Webshop", 0);
				sqlCommand.Parameters.AddWithValue("Werkzeug", item.Werkzeug == null ? (object)DBNull.Value : "na");
				sqlCommand.Parameters.AddWithValue("Wert_Anfangsbestand", 0);
				sqlCommand.Parameters.AddWithValue("Zeichnungsnummer", item.Zeichnungsnummer == null ? (object)DBNull.Value : item.Zeichnungsnummer);
				sqlCommand.Parameters.AddWithValue("Zolltarif_nr", item.Zolltarif_nr == null ? (object)DBNull.Value : item.Zolltarif_nr);
				sqlCommand.Parameters.AddWithValue("Stundensatz", item.Stundensatz == null ? (object)DBNull.Value : item.Stundensatz);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer> SearchByName(string name, int max, int supplierId)
		{
			if(max > 5000)
			{
				max = 5000;
			}

			if(max <= 0)
			{
				max = 100;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT TOP {max} Art.[Artikel-Nr],Art.[Artikelnummer],Art.[Sysmonummer],Art.[Seriennummer],Art.[Bezeichnung 1],Art.[Bezeichnung 2],Art.[Bezeichnung 3],"
								+ " Art.[Kategorie],Art.[Lagerartikel],Art.[Rabattierfähig],Art.[Stückliste],Art.[fakturieren Stückliste],Art.[Seriennummernverwaltung],"
								+ " Art.[Anfangsbestand],Art.[Datum Anfangsbestand],Art.[Wert_Anfangsbestand],Art.[Einheit],Art.[Gebinde],Art.[aktualisiert],Art.[Gewicht],"
								+ " Art.[Größe],Art.[Volumen],Art.[Farbe],Art.[Lagerhaltungskosten],Art.[pro Zeiteinheit],Art.[Ersatzartikel],Art.[Artikel aus eigener Produktion],"
								+ " Art.[Sonderrabatt],Art.[gültig bis],Art.[Provisionsartikel],Art.[Artikel für weitere Bestellungen sperren],Art.[Grund für Sperre],"
								+ " Art.[Kriterium1],Art.[Kriterium2],Art.[Kriterium3],Art.[Kriterium4],Art.[Langtext],Art.[Langtext_drucken_AB],Art.[Langtext_drucken_BW],"
								+ " Art.[aktiv],Art.[EAN],Art.[Zolltarif_nr],Art.[Ursprungsland],Art.[Zeichnungsnummer],Art.[fibu_rahmen],Art.[Barverkauf],Art.[Webshop],"
								+ " Art.[Freigabestatus],Art.[Produktionszeit],Art.[Kupferzahl],Art.[Kupferbasis],Art.[DEL],Art.[DEL fixiert],Art.[Rahmen],Art.[Rahmen-Nr],"
								+ " Art.[Rahmenmenge],Art.[Rahmenauslauf],Art.[Prüfstatus TN Ware],Art.[Dokumente],Art.[Verpackung],Art.[UL zertifiziert],"
								+ " Art.[Freigabestatus TN intern],Art.[Index_Kunde],Art.[Index_Kunde_Datum],Art.[Artikelfamilie_Kunde],Art.[Artikelfamilie_Kunde_Detail1],"
								+ " Art.[Artikelfamilie_Kunde_Detail2],Art.[Cu-Gewicht],Art.[VK-Festpreis],Art.[Standard_Lagerort_id],Art.[UL Etikett],Art.[Artikelkurztext],"
								+ " Art.[Materialkosten_Alt],Art.[Werkzeug],Art.[Crossreferenz],Art.[Halle],Art.[ESD_Schutz],"
								+ " Best.* FROM [__FNC_Artikel] AS Art "
								+ "JOIN Bestellnummern AS Best ON Art.[Artikel-Nr]=Best.[Artikel-Nr] "
								+ $"WHERE Best.[Lieferanten-Nr]=@supplierId AND ([Artikelnummer] LIKE '%{name ?? ""}%' OR [Bezeichnung 1] LIKE '%{name ?? ""}%') AND Art.[aktiv]=1 ORDER BY [Artikelnummer]";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("supplierId", supplierId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toListSearchData(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity GetFirstDiverse()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Artikel] WHERE [Bezeichnung 1] LIKE 'diverse%' ORDER BY [Artikel-Nr] ASC";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity> toListArtikelOrder(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Artikel_Order_BudgetEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> toListAllData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity(dataRow)); }
			return list;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer> toListSearchData(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.ArtikelBestellnummer(dataRow)); }
			return list;
		}
		private static string getSearchColumn(Artikel_BudgetEnum.ArtikelSearchColumns searchColumn)
		{
			switch(searchColumn)
			{
				case Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNr:
					return "Artikel-Nr";
				case Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNummer:
					return "Artikelnummer";
				case Artikel_BudgetEnum.ArtikelSearchColumns.Hall:
					return "Halle";
				case Artikel_BudgetEnum.ArtikelSearchColumns.PriceGroup:
					return "Warengruppe";
				case Artikel_BudgetEnum.ArtikelSearchColumns.Status:
					return "Freigabestatus";
				case Artikel_BudgetEnum.ArtikelSearchColumns.UnitPrice:
					return "Preiseinheit";
				default:
					return "Artikel-Nr";
			}
		}
		private static Artikel_BudgetEnum.ArtikelSearchColumns getSearchColumn(string searchColumn)
		{
			searchColumn = searchColumn.Trim().ToLower();
			if(string.IsNullOrWhiteSpace(searchColumn))
				return Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNummer;

			// supposed to be the same as the enum Desption annotation
			switch(searchColumn)
			{
				case "nr":
					return Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNr;
				case "nummer":
					return Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNummer;
				case "hall":
					return Artikel_BudgetEnum.ArtikelSearchColumns.Hall;
				case "pricegroup":
					return Artikel_BudgetEnum.ArtikelSearchColumns.PriceGroup;
				case "status":
					return Artikel_BudgetEnum.ArtikelSearchColumns.Status;
				case "unitprice":
					return Artikel_BudgetEnum.ArtikelSearchColumns.UnitPrice;
				default:
					return Artikel_BudgetEnum.ArtikelSearchColumns.ArticleNummer;
			}
		}
		#endregion
	}
}
