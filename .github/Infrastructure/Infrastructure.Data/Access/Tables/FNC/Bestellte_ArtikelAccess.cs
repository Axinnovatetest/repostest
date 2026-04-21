using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Bestellte_ArtikelAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Bestellte Artikel] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Bestellte Artikel] ([AB-Nr_Lieferant],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[CUPreis],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[Kanban],[Lagerort_id],[Liefertermin],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[Umsatzsteuer],[WE Pos zu Bestellposition])  VALUES (@AB_Nr_Lieferant,@Aktuelle_Anzahl,@AnfangLagerBestand,@Anzahl,@Artikel_Nr,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Bestätigter_Termin,@Bestellnummer,@Bestellung_Nr,@Bezeichnung_1,@Bezeichnung_2,@BP_zu_RBposition,@COC_bestätigung,@CUPreis,@Einheit,@Einzelpreis,@EMPB_Bestätigung,@EndeLagerBestand,@Erhalten,@erledigt_pos,@Gesamtpreis,@In_Bearbeitung,@InfoRahmennummer,@Kanban,@Lagerort_id,@Liefertermin,@Löschen,@MhdDatumArtikel,@Position,@Position_erledigt,@Preiseinheit,@Preisgruppe,@Produktionsort,@Rabatt,@Rabatt1,@RB_Abgerufen,@RB_Offen,@RB_OriginalAnzahl,@schriftart,@sortierung,@Start_Anzahl,@Umsatzsteuer,@WE_Pos_zu_Bestellposition); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestätigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestätigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 46; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items)
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
						query += " INSERT INTO [Bestellte Artikel] ([AB-Nr_Lieferant],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[CUPreis],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[Kanban],[Lagerort_id],[Liefertermin],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[Umsatzsteuer],[WE Pos zu Bestellposition]) VALUES ( "

							+ "@AB_Nr_Lieferant" + i + ","
							+ "@Aktuelle_Anzahl" + i + ","
							+ "@AnfangLagerBestand" + i + ","
							+ "@Anzahl" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bemerkung_Pos" + i + ","
							+ "@Bemerkung_Pos_ID" + i + ","
							+ "@Bestätigter_Termin" + i + ","
							+ "@Bestellnummer" + i + ","
							+ "@Bestellung_Nr" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@BP_zu_RBposition" + i + ","
							+ "@COC_bestätigung" + i + ","
							+ "@CUPreis" + i + ","
							+ "@Einheit" + i + ","
							+ "@Einzelpreis" + i + ","
							+ "@EMPB_Bestätigung" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@Erhalten" + i + ","
							+ "@erledigt_pos" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@InfoRahmennummer" + i + ","
							+ "@Kanban" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Löschen" + i + ","
							+ "@MhdDatumArtikel" + i + ","
							+ "@Position" + i + ","
							+ "@Position_erledigt" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@Preisgruppe" + i + ","
							+ "@Produktionsort" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Rabatt1" + i + ","
							+ "@RB_Abgerufen" + i + ","
							+ "@RB_Offen" + i + ","
							+ "@RB_OriginalAnzahl" + i + ","
							+ "@schriftart" + i + ","
							+ "@sortierung" + i + ","
							+ "@Start_Anzahl" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@WE_Pos_zu_Bestellposition" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestätigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestätigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Bestellte Artikel] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Bemerkung_Pos]=@Bemerkung_Pos, [Bemerkung_Pos_ID]=@Bemerkung_Pos_ID, [Bestätigter_Termin]=@Bestätigter_Termin, [Bestellnummer]=@Bestellnummer, [Bestellung-Nr]=@Bestellung_Nr, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [BP zu RBposition]=@BP_zu_RBposition, [COC_bestätigung]=@COC_bestätigung, [CUPreis]=@CUPreis, [Einheit]=@Einheit, [Einzelpreis]=@Einzelpreis, [EMPB_Bestätigung]=@EMPB_Bestätigung, [EndeLagerBestand]=@EndeLagerBestand, [Erhalten]=@Erhalten, [erledigt_pos]=@erledigt_pos, [Gesamtpreis]=@Gesamtpreis, [In Bearbeitung]=@In_Bearbeitung, [InfoRahmennummer]=@InfoRahmennummer, [Kanban]=@Kanban, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [Löschen]=@Löschen, [MhdDatumArtikel]=@MhdDatumArtikel, [Position]=@Position, [Position erledigt]=@Position_erledigt, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [Produktionsort]=@Produktionsort, [Rabatt]=@Rabatt, [Rabatt1]=@Rabatt1, [RB_Abgerufen]=@RB_Abgerufen, [RB_Offen]=@RB_Offen, [RB_OriginalAnzahl]=@RB_OriginalAnzahl, [schriftart]=@schriftart, [sortierung]=@sortierung, [Start Anzahl]=@Start_Anzahl, [Umsatzsteuer]=@Umsatzsteuer, [WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
				sqlCommand.Parameters.AddWithValue("Bestätigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
				sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
				sqlCommand.Parameters.AddWithValue("COC_bestätigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
				sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
				sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
				sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
				sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
				sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
				sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
				sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
				sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
				sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 46; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items)
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
						query += " UPDATE [Bestellte Artikel] SET "

							+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
							+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
							+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Bemerkung_Pos]=@Bemerkung_Pos" + i + ","
							+ "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i + ","
							+ "[Bestätigter_Termin]=@Bestätigter_Termin" + i + ","
							+ "[Bestellnummer]=@Bestellnummer" + i + ","
							+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
							+ "[BP zu RBposition]=@BP_zu_RBposition" + i + ","
							+ "[COC_bestätigung]=@COC_bestätigung" + i + ","
							+ "[CUPreis]=@CUPreis" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[Einzelpreis]=@Einzelpreis" + i + ","
							+ "[EMPB_Bestätigung]=@EMPB_Bestätigung" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[Erhalten]=@Erhalten" + i + ","
							+ "[erledigt_pos]=@erledigt_pos" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[InfoRahmennummer]=@InfoRahmennummer" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[MhdDatumArtikel]=@MhdDatumArtikel" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[Position erledigt]=@Position_erledigt" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + ","
							+ "[Produktionsort]=@Produktionsort" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Rabatt1]=@Rabatt1" + i + ","
							+ "[RB_Abgerufen]=@RB_Abgerufen" + i + ","
							+ "[RB_Offen]=@RB_Offen" + i + ","
							+ "[RB_OriginalAnzahl]=@RB_OriginalAnzahl" + i + ","
							+ "[schriftart]=@schriftart" + i + ","
							+ "[sortierung]=@sortierung" + i + ","
							+ "[Start Anzahl]=@Start_Anzahl" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestätigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestätigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Bestellte Artikel] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [Bestellte Artikel] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellte Artikel] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Bestellte Artikel]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Bestellte Artikel] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Bestellte Artikel] ([AB-Nr_Lieferant],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[CUPreis],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[Kanban],[Lagerort_id],[Liefertermin],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[Umsatzsteuer],[WE Pos zu Bestellposition]) OUTPUT INSERTED.[Nr] VALUES (@AB_Nr_Lieferant,@Aktuelle_Anzahl,@AnfangLagerBestand,@Anzahl,@Artikel_Nr,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Bestatigter_Termin,@Bestellnummer,@Bestellung_Nr,@Bezeichnung_1,@Bezeichnung_2,@BP_zu_RBposition,@COC_bestatigung,@CUPreis,@Einheit,@Einzelpreis,@EMPB_Bestatigung,@EndeLagerBestand,@Erhalten,@erledigt_pos,@Gesamtpreis,@In_Bearbeitung,@InfoRahmennummer,@Kanban,@Lagerort_id,@Liefertermin,@Loschen,@MhdDatumArtikel,@Position,@Position_erledigt,@Preiseinheit,@Preisgruppe,@Produktionsort,@Rabatt,@Rabatt1,@RB_Abgerufen,@RB_Offen,@RB_OriginalAnzahl,@schriftart,@sortierung,@Start_Anzahl,@Umsatzsteuer,@WE_Pos_zu_Bestellposition); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
			sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
			sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
			sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
			sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
			sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
			sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
			sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 46; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Bestellte Artikel] ([AB-Nr_Lieferant],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[CUPreis],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[Kanban],[Lagerort_id],[Liefertermin],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[Umsatzsteuer],[WE Pos zu Bestellposition]) VALUES ( "

						+ "@AB_Nr_Lieferant" + i + ","
						+ "@Aktuelle_Anzahl" + i + ","
						+ "@AnfangLagerBestand" + i + ","
						+ "@Anzahl" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Bemerkung_Pos" + i + ","
						+ "@Bemerkung_Pos_ID" + i + ","
						+ "@Bestatigter_Termin" + i + ","
						+ "@Bestellnummer" + i + ","
						+ "@Bestellung_Nr" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Bezeichnung_2" + i + ","
						+ "@BP_zu_RBposition" + i + ","
						+ "@COC_bestatigung" + i + ","
						+ "@CUPreis" + i + ","
						+ "@Einheit" + i + ","
						+ "@Einzelpreis" + i + ","
						+ "@EMPB_Bestatigung" + i + ","
						+ "@EndeLagerBestand" + i + ","
						+ "@Erhalten" + i + ","
						+ "@erledigt_pos" + i + ","
						+ "@Gesamtpreis" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@InfoRahmennummer" + i + ","
						+ "@Kanban" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@Loschen" + i + ","
						+ "@MhdDatumArtikel" + i + ","
						+ "@Position" + i + ","
						+ "@Position_erledigt" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@Preisgruppe" + i + ","
						+ "@Produktionsort" + i + ","
						+ "@Rabatt" + i + ","
						+ "@Rabatt1" + i + ","
						+ "@RB_Abgerufen" + i + ","
						+ "@RB_Offen" + i + ","
						+ "@RB_OriginalAnzahl" + i + ","
						+ "@schriftart" + i + ","
						+ "@sortierung" + i + ","
						+ "@Start_Anzahl" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@WE_Pos_zu_Bestellposition" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Bestellte Artikel] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Bemerkung_Pos]=@Bemerkung_Pos, [Bemerkung_Pos_ID]=@Bemerkung_Pos_ID, [Bestätigter_Termin]=@Bestatigter_Termin, [Bestellnummer]=@Bestellnummer, [Bestellung-Nr]=@Bestellung_Nr, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [BP zu RBposition]=@BP_zu_RBposition, [COC_bestätigung]=@COC_bestatigung, [CUPreis]=@CUPreis, [Einheit]=@Einheit, [Einzelpreis]=@Einzelpreis, [EMPB_Bestätigung]=@EMPB_Bestatigung, [EndeLagerBestand]=@EndeLagerBestand, [Erhalten]=@Erhalten, [erledigt_pos]=@erledigt_pos, [Gesamtpreis]=@Gesamtpreis, [In Bearbeitung]=@In_Bearbeitung, [InfoRahmennummer]=@InfoRahmennummer, [Kanban]=@Kanban, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [Löschen]=@Loschen, [MhdDatumArtikel]=@MhdDatumArtikel, [Position]=@Position, [Position erledigt]=@Position_erledigt, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [Produktionsort]=@Produktionsort, [Rabatt]=@Rabatt, [Rabatt1]=@Rabatt1, [RB_Abgerufen]=@RB_Abgerufen, [RB_Offen]=@RB_Offen, [RB_OriginalAnzahl]=@RB_OriginalAnzahl, [schriftart]=@schriftart, [sortierung]=@sortierung, [Start Anzahl]=@Start_Anzahl, [Umsatzsteuer]=@Umsatzsteuer, [WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
			sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
			sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Loschen == null ? (object)DBNull.Value : item.Loschen);
			sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
			sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
			sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
			sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
			sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
			sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
			sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
			sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
			sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
			sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 46; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Bestellte Artikel] SET "

					+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
					+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
					+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Bemerkung_Pos]=@Bemerkung_Pos" + i + ","
					+ "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i + ","
					+ "[Bestätigter_Termin]=@Bestatigter_Termin" + i + ","
					+ "[Bestellnummer]=@Bestellnummer" + i + ","
					+ "[Bestellung-Nr]=@Bestellung_Nr" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
					+ "[BP zu RBposition]=@BP_zu_RBposition" + i + ","
					+ "[COC_bestätigung]=@COC_bestatigung" + i + ","
					+ "[CUPreis]=@CUPreis" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[Einzelpreis]=@Einzelpreis" + i + ","
					+ "[EMPB_Bestätigung]=@EMPB_Bestatigung" + i + ","
					+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
					+ "[Erhalten]=@Erhalten" + i + ","
					+ "[erledigt_pos]=@erledigt_pos" + i + ","
					+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[InfoRahmennummer]=@InfoRahmennummer" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[MhdDatumArtikel]=@MhdDatumArtikel" + i + ","
					+ "[Position]=@Position" + i + ","
					+ "[Position erledigt]=@Position_erledigt" + i + ","
					+ "[Preiseinheit]=@Preiseinheit" + i + ","
					+ "[Preisgruppe]=@Preisgruppe" + i + ","
					+ "[Produktionsort]=@Produktionsort" + i + ","
					+ "[Rabatt]=@Rabatt" + i + ","
					+ "[Rabatt1]=@Rabatt1" + i + ","
					+ "[RB_Abgerufen]=@RB_Abgerufen" + i + ","
					+ "[RB_Offen]=@RB_Offen" + i + ","
					+ "[RB_OriginalAnzahl]=@RB_OriginalAnzahl" + i + ","
					+ "[schriftart]=@schriftart" + i + ","
					+ "[sortierung]=@sortierung" + i + ","
					+ "[Start Anzahl]=@Start_Anzahl" + i + ","
					+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
					+ "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Loschen == null ? (object)DBNull.Value : item.Loschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Bestellte Artikel] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [Bestellte Artikel] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> GetByOrderId(int orderId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@orderId /* AND [Bestätigter_Termin] IS NULL*/";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> GetByOrderId(int orderId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.CommandText = "SELECT * FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@orderId /* AND [Bestätigter_Termin] IS NULL*/";
				
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> GetByOrderIds(List<int> orderIds)
		{
			if(orderIds == null || orderIds.Count <= 0)
			{
				return null;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellte Artikel] WHERE [Bestellung-Nr] IN ({string.Join(",", orderIds)}) /* AND [Bestätigter_Termin] IS NULL*/";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			}
		}

		public static Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity GetByOrderIdAndArticleId(int orderId, int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@orderId /*AND [Bestätigter_Termin] IS NULL*/ AND [Artikel-Nr]=@articleId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int DeleteByOrderId(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Bestellte Artikel] WHERE [Bestellung-Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		#endregion
	}
}
