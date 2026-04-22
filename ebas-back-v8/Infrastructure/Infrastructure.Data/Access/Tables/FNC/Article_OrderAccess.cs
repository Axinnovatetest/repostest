using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class Article_OrderAccessXXX
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity Get(int id_ao)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Article_Order] WHERE [Id_AO]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id_ao);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Article_Order]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Article_Order] WHERE [Id_AO] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Article_Order] ([AB-Nr_Lieferant],[Account_Id],[Account_Name],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestatigung],[Confirmation_Date],[CUPreis],[Currency_Article],[Delivery_Date],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestatigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[Id_Article],[Id_Currency_Article],[Id_Order],[In Bearbeitung],[InfoRahmennummer],[Internal_Contact],[Kanban],[Lagerort_id],[Liefertermin],[Location_Id],[Location_Name],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[TotalCost_Article],[Umsatzsteuer],[Unit_Price],[VAT],[WE Pos zu Bestellposition])  VALUES (@AB_Nr_Lieferant,@Account_Id,@Account_Name,@Aktuelle_Anzahl,@AnfangLagerBestand,@Anzahl,@Artikel_Nr,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Bestätigter_Termin,@Bestellnummer,@Bestellung_Nr,@Bezeichnung_1,@Bezeichnung_2,@BP_zu_RBposition,@COC_bestatigung,@Confirmation_Date,@CUPreis,@Currency_Article,@Delivery_Date,@Description,@Discount,@Einheit,@Einzelpreis,@EMPB_Bestatigung,@EndeLagerBestand,@Erhalten,@erledigt_pos,@Gesamtpreis,@Id_Article,@Id_Currency_Article,@Id_Order,@In_Bearbeitung,@InfoRahmennummer,@Internal_Contact,@Kanban,@Lagerort_id,@Liefertermin,@Location_Id,@Location_Name,@Löschen,@MhdDatumArtikel,@Position,@Position_erledigt,@Preiseinheit,@Preisgruppe,@Produktionsort,@Quantity,@Rabatt,@Rabatt1,@RB_Abgerufen,@RB_Offen,@RB_OriginalAnzahl,@schriftart,@sortierung,@Start_Anzahl,@TotalCost_Article,@Umsatzsteuer,@Unit_Price,@VAT,@WE_Pos_zu_Bestellposition); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("Account_Id", item.Account_Id == null ? (object)DBNull.Value : item.Account_Id);
					sqlCommand.Parameters.AddWithValue("Account_Name", item.Account_Name == null ? (object)DBNull.Value : item.Account_Name);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestätigter_Termin", item.Bestätigter_Termin == null ? (object)DBNull.Value : item.Bestätigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
					sqlCommand.Parameters.AddWithValue("Confirmation_Date", item.Confirmation_Date == null ? (object)DBNull.Value : item.Confirmation_Date);
					sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("Currency_Article", item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
					sqlCommand.Parameters.AddWithValue("Delivery_Date", item.Delivery_Date == null ? (object)DBNull.Value : item.Delivery_Date);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
					sqlCommand.Parameters.AddWithValue("Id_Currency_Article", item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
					sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("Internal_Contact", item.Internal_Contact == null ? (object)DBNull.Value : item.Internal_Contact);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("Location_Id", item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
					sqlCommand.Parameters.AddWithValue("Location_Name", item.Location_Name == null ? (object)DBNull.Value : item.Location_Name);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
					sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
					sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
					sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
					sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
					sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
					sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
					sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
					sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
					sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
					sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
					sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 63; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> items)
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
						query += " INSERT INTO [Article_Order] ([AB-Nr_Lieferant],[Account_Id],[Account_Name],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestätigter_Termin],[Bestellnummer],[Bestellung-Nr],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestatigung],[Confirmation_Date],[CUPreis],[Currency_Article],[Delivery_Date],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestatigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[Id_Article],[Id_Currency_Article],[Id_Order],[In Bearbeitung],[InfoRahmennummer],[Internal_Contact],[Kanban],[Lagerort_id],[Liefertermin],[Location_Id],[Location_Name],[Löschen],[MhdDatumArtikel],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[TotalCost_Article],[Umsatzsteuer],[Unit_Price],[VAT],[WE Pos zu Bestellposition]) VALUES ( "

							+ "@AB_Nr_Lieferant" + i + ","
							+ "@Account_Id" + i + ","
							+ "@Account_Name" + i + ","
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
							+ "@COC_bestatigung" + i + ","
							+ "@Confirmation_Date" + i + ","
							+ "@CUPreis" + i + ","
							+ "@Currency_Article" + i + ","
							+ "@Delivery_Date" + i + ","
							+ "@Description" + i + ","
							+ "@Discount" + i + ","
							+ "@Einheit" + i + ","
							+ "@Einzelpreis" + i + ","
							+ "@EMPB_Bestatigung" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@Erhalten" + i + ","
							+ "@erledigt_pos" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@Id_Article" + i + ","
							+ "@Id_Currency_Article" + i + ","
							+ "@Id_Order" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@InfoRahmennummer" + i + ","
							+ "@Internal_Contact" + i + ","
							+ "@Kanban" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@Location_Id" + i + ","
							+ "@Location_Name" + i + ","
							+ "@Löschen" + i + ","
							+ "@MhdDatumArtikel" + i + ","
							+ "@Position" + i + ","
							+ "@Position_erledigt" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@Preisgruppe" + i + ","
							+ "@Produktionsort" + i + ","
							+ "@Quantity" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Rabatt1" + i + ","
							+ "@RB_Abgerufen" + i + ","
							+ "@RB_Offen" + i + ","
							+ "@RB_OriginalAnzahl" + i + ","
							+ "@schriftart" + i + ","
							+ "@sortierung" + i + ","
							+ "@Start_Anzahl" + i + ","
							+ "@TotalCost_Article" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@Unit_Price" + i + ","
							+ "@VAT" + i + ","
							+ "@WE_Pos_zu_Bestellposition" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Account_Id" + i, item.Account_Id == null ? (object)DBNull.Value : item.Account_Id);
						sqlCommand.Parameters.AddWithValue("Account_Name" + i, item.Account_Name == null ? (object)DBNull.Value : item.Account_Name);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestätigter_Termin" + i, item.Bestätigter_Termin == null ? (object)DBNull.Value : item.Bestätigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("Confirmation_Date" + i, item.Confirmation_Date == null ? (object)DBNull.Value : item.Confirmation_Date);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("Currency_Article" + i, item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
						sqlCommand.Parameters.AddWithValue("Delivery_Date" + i, item.Delivery_Date == null ? (object)DBNull.Value : item.Delivery_Date);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Id_Article" + i, item.Id_Article);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Article" + i, item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("Internal_Contact" + i, item.Internal_Contact == null ? (object)DBNull.Value : item.Internal_Contact);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Location_Id" + i, item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
						sqlCommand.Parameters.AddWithValue("Location_Name" + i, item.Location_Name == null ? (object)DBNull.Value : item.Location_Name);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity == null ? (object)DBNull.Value : item.Quantity);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("TotalCost_Article" + i, item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Unit_Price" + i, item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
						sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [Article_Order] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [Account_Id]=@Account_Id, [Account_Name]=@Account_Name, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [Artikel-Nr]=@Artikel_Nr, [Bemerkung_Pos]=@Bemerkung_Pos, [Bemerkung_Pos_ID]=@Bemerkung_Pos_ID, [Bestätigter_Termin]=@Bestätigter_Termin, [Bestellnummer]=@Bestellnummer, [Bestellung-Nr]=@Bestellung_Nr, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [BP zu RBposition]=@BP_zu_RBposition, [COC_bestatigung]=@COC_bestatigung, [Confirmation_Date]=@Confirmation_Date, [CUPreis]=@CUPreis, [Currency_Article]=@Currency_Article, [Delivery_Date]=@Delivery_Date, [Description]=@Description, [Discount]=@Discount, [Einheit]=@Einheit, [Einzelpreis]=@Einzelpreis, [EMPB_Bestatigung]=@EMPB_Bestatigung, [EndeLagerBestand]=@EndeLagerBestand, [Erhalten]=@Erhalten, [erledigt_pos]=@erledigt_pos, [Gesamtpreis]=@Gesamtpreis, [Id_Article]=@Id_Article, [Id_Currency_Article]=@Id_Currency_Article, [Id_Order]=@Id_Order, [In Bearbeitung]=@In_Bearbeitung, [InfoRahmennummer]=@InfoRahmennummer, [Internal_Contact]=@Internal_Contact, [Kanban]=@Kanban, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [Location_Id]=@Location_Id, [Location_Name]=@Location_Name, [Löschen]=@Löschen, [MhdDatumArtikel]=@MhdDatumArtikel, [Position]=@Position, [Position erledigt]=@Position_erledigt, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [Produktionsort]=@Produktionsort, [Quantity]=@Quantity, [Rabatt]=@Rabatt, [Rabatt1]=@Rabatt1, [RB_Abgerufen]=@RB_Abgerufen, [RB_Offen]=@RB_Offen, [RB_OriginalAnzahl]=@RB_OriginalAnzahl, [schriftart]=@schriftart, [sortierung]=@sortierung, [Start Anzahl]=@Start_Anzahl, [TotalCost_Article]=@TotalCost_Article, [Umsatzsteuer]=@Umsatzsteuer, [Unit_Price]=@Unit_Price, [VAT]=@VAT, [WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition WHERE [Id_AO]=@Id_AO";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id_AO", item.Id_AO);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("Account_Id", item.Account_Id == null ? (object)DBNull.Value : item.Account_Id);
				sqlCommand.Parameters.AddWithValue("Account_Name", item.Account_Name == null ? (object)DBNull.Value : item.Account_Name);
				sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
				sqlCommand.Parameters.AddWithValue("Bestätigter_Termin", item.Bestätigter_Termin == null ? (object)DBNull.Value : item.Bestätigter_Termin);
				sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
				sqlCommand.Parameters.AddWithValue("Bestellung_Nr", item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
				sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
				sqlCommand.Parameters.AddWithValue("Confirmation_Date", item.Confirmation_Date == null ? (object)DBNull.Value : item.Confirmation_Date);
				sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
				sqlCommand.Parameters.AddWithValue("Currency_Article", item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
				sqlCommand.Parameters.AddWithValue("Delivery_Date", item.Delivery_Date == null ? (object)DBNull.Value : item.Delivery_Date);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
				sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Article", item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
				sqlCommand.Parameters.AddWithValue("Internal_Contact", item.Internal_Contact == null ? (object)DBNull.Value : item.Internal_Contact);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("Location_Id", item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
				sqlCommand.Parameters.AddWithValue("Location_Name", item.Location_Name == null ? (object)DBNull.Value : item.Location_Name);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
				sqlCommand.Parameters.AddWithValue("Position", item.Position == null ? (object)DBNull.Value : item.Position);
				sqlCommand.Parameters.AddWithValue("Position_erledigt", item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Preisgruppe", item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
				sqlCommand.Parameters.AddWithValue("Produktionsort", item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
				sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Rabatt1", item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
				sqlCommand.Parameters.AddWithValue("RB_Abgerufen", item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
				sqlCommand.Parameters.AddWithValue("RB_Offen", item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
				sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl", item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
				sqlCommand.Parameters.AddWithValue("schriftart", item.schriftart == null ? (object)DBNull.Value : item.schriftart);
				sqlCommand.Parameters.AddWithValue("sortierung", item.sortierung == null ? (object)DBNull.Value : item.sortierung);
				sqlCommand.Parameters.AddWithValue("Start_Anzahl", item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
				sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
				sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
				sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 63; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> items)
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
						query += " UPDATE [Article_Order] SET "

							+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
							+ "[Account_Id]=@Account_Id" + i + ","
							+ "[Account_Name]=@Account_Name" + i + ","
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
							+ "[COC_bestatigung]=@COC_bestatigung" + i + ","
							+ "[Confirmation_Date]=@Confirmation_Date" + i + ","
							+ "[CUPreis]=@CUPreis" + i + ","
							+ "[Currency_Article]=@Currency_Article" + i + ","
							+ "[Delivery_Date]=@Delivery_Date" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Discount]=@Discount" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[Einzelpreis]=@Einzelpreis" + i + ","
							+ "[EMPB_Bestatigung]=@EMPB_Bestatigung" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[Erhalten]=@Erhalten" + i + ","
							+ "[erledigt_pos]=@erledigt_pos" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[Id_Article]=@Id_Article" + i + ","
							+ "[Id_Currency_Article]=@Id_Currency_Article" + i + ","
							+ "[Id_Order]=@Id_Order" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[InfoRahmennummer]=@InfoRahmennummer" + i + ","
							+ "[Internal_Contact]=@Internal_Contact" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[Location_Id]=@Location_Id" + i + ","
							+ "[Location_Name]=@Location_Name" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[MhdDatumArtikel]=@MhdDatumArtikel" + i + ","
							+ "[Position]=@Position" + i + ","
							+ "[Position erledigt]=@Position_erledigt" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[Preisgruppe]=@Preisgruppe" + i + ","
							+ "[Produktionsort]=@Produktionsort" + i + ","
							+ "[Quantity]=@Quantity" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Rabatt1]=@Rabatt1" + i + ","
							+ "[RB_Abgerufen]=@RB_Abgerufen" + i + ","
							+ "[RB_Offen]=@RB_Offen" + i + ","
							+ "[RB_OriginalAnzahl]=@RB_OriginalAnzahl" + i + ","
							+ "[schriftart]=@schriftart" + i + ","
							+ "[sortierung]=@sortierung" + i + ","
							+ "[Start Anzahl]=@Start_Anzahl" + i + ","
							+ "[TotalCost_Article]=@TotalCost_Article" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[Unit_Price]=@Unit_Price" + i + ","
							+ "[VAT]=@VAT" + i + ","
							+ "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i + " WHERE [Id_AO]=@Id_AO" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id_AO" + i, item.Id_AO);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("Account_Id" + i, item.Account_Id == null ? (object)DBNull.Value : item.Account_Id);
						sqlCommand.Parameters.AddWithValue("Account_Name" + i, item.Account_Name == null ? (object)DBNull.Value : item.Account_Name);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestätigter_Termin" + i, item.Bestätigter_Termin == null ? (object)DBNull.Value : item.Bestätigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bestellung_Nr" + i, item.Bestellung_Nr == null ? (object)DBNull.Value : item.Bestellung_Nr);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestatigung == null ? (object)DBNull.Value : item.COC_bestatigung);
						sqlCommand.Parameters.AddWithValue("Confirmation_Date" + i, item.Confirmation_Date == null ? (object)DBNull.Value : item.Confirmation_Date);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("Currency_Article" + i, item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
						sqlCommand.Parameters.AddWithValue("Delivery_Date" + i, item.Delivery_Date == null ? (object)DBNull.Value : item.Delivery_Date);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestatigung == null ? (object)DBNull.Value : item.EMPB_Bestatigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("Id_Article" + i, item.Id_Article);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Article" + i, item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("Internal_Contact" + i, item.Internal_Contact == null ? (object)DBNull.Value : item.Internal_Contact);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("Location_Id" + i, item.Location_Id == null ? (object)DBNull.Value : item.Location_Id);
						sqlCommand.Parameters.AddWithValue("Location_Name" + i, item.Location_Name == null ? (object)DBNull.Value : item.Location_Name);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("Position" + i, item.Position == null ? (object)DBNull.Value : item.Position);
						sqlCommand.Parameters.AddWithValue("Position_erledigt" + i, item.Position_erledigt == null ? (object)DBNull.Value : item.Position_erledigt);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Preisgruppe" + i, item.Preisgruppe == null ? (object)DBNull.Value : item.Preisgruppe);
						sqlCommand.Parameters.AddWithValue("Produktionsort" + i, item.Produktionsort == null ? (object)DBNull.Value : item.Produktionsort);
						sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity == null ? (object)DBNull.Value : item.Quantity);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Rabatt1" + i, item.Rabatt1 == null ? (object)DBNull.Value : item.Rabatt1);
						sqlCommand.Parameters.AddWithValue("RB_Abgerufen" + i, item.RB_Abgerufen == null ? (object)DBNull.Value : item.RB_Abgerufen);
						sqlCommand.Parameters.AddWithValue("RB_Offen" + i, item.RB_Offen == null ? (object)DBNull.Value : item.RB_Offen);
						sqlCommand.Parameters.AddWithValue("RB_OriginalAnzahl" + i, item.RB_OriginalAnzahl == null ? (object)DBNull.Value : item.RB_OriginalAnzahl);
						sqlCommand.Parameters.AddWithValue("schriftart" + i, item.schriftart == null ? (object)DBNull.Value : item.schriftart);
						sqlCommand.Parameters.AddWithValue("sortierung" + i, item.sortierung == null ? (object)DBNull.Value : item.sortierung);
						sqlCommand.Parameters.AddWithValue("Start_Anzahl" + i, item.Start_Anzahl == null ? (object)DBNull.Value : item.Start_Anzahl);
						sqlCommand.Parameters.AddWithValue("TotalCost_Article" + i, item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Unit_Price" + i, item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
						sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
						sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id_ao)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Article_Order] WHERE [Id_AO]=@Id_AO";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_AO", id_ao);

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

					string query = "DELETE FROM [Article_Order] WHERE [Id_AO] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> GetByOrderId(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = "SELECT * FROM [Article_Order] where Id_Order=@Id_Order";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Order", id);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }



		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> GetByOrderIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = $"SELECT * FROM [Article_Order] where Id_Order IN ({string.Join(", ", ids)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);

			}
			else
			{ return null; }



		}



		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity> GetArtikelOrder_Article(int id, int id_Article)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				string query = @"SELECT ord_art.Id_AO, ord_art.Id_Order, ord_art.Id_Article, ord_art.Unit_Price, ord_art.Quantity,null as Article_Name_Order_Diverse, 0 as Unit_Price_Diverse,
                ord_art.TotalCost_Article, ord_art.Id_Currency_Article, ord_art.Currency_Article, 
                ord_art.Action_Article,ord_art.Id_User, ord_art.Article_date, ord_art.Id_Project, ord_art.Id_Dept, 
                ord_art.Id_Land, ord_art.Dept_name, ord_art.Land_name, 
                CONCAT(Artikelnummer,'--',[Bezeichnung 1],'--',[Bezeichnung 2]) AS Article_Name 
                FROM[Article_Order] as ord_art 
                inner join [Budget_Order] as ord on ord.Id_Order=ord_art.Id_Order
				inner join[Budget].[dbo].[Artikel_Budget] as art on art.[Artikel-Nr]=ord_art.[Id_Article]
                where ord_art.Id_Order=@Id_Ord and ord_art.Id_Article=@id_Article";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Ord", id);
				sqlCommand.Parameters.AddWithValue("id_Article", id_Article);

				DbExecution.Fill(sqlCommand, dataTable);


			}

			if(dataTable.Rows.Count > 0)
			{
				return toList_ConcatName(dataTable);

			}
			else
			{ return null; }



		}
		public static int InsertArtikelOrder(Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity item)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Article_Order] ([Currency_Article],[Id_Article],[Id_Currency_Article],[Id_Order],[Quantity],[TotalCost_Article],[Unit_Price])  VALUES (@Currency_Article,@Id_Article,@Id_Currency_Article,@Id_Order,@Quantity,@TotalCost_Article,@Unit_Price)";
				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Currency_Article", item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);

					sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
					sqlCommand.Parameters.AddWithValue("Id_Currency_Article", item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);

					sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);

					sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);

					sqlCommand.Parameters.AddWithValue("TotalCost_Article", (decimal?)item.Unit_Price * item.Quantity);
					sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id_AO] FROM [Article_Order] WHERE [Id_AO] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> GetLastVersionOrder(int Id_Order)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();


				/*string query = "select max(ord_ver.Id_VO) as Max_VO,ord_ver.Nr_version_Order as Nr_version_Order_param,ord_ver.Id_Level as Id_Level_param, ord_ver.Id_Status as Id_Status_param, ord_ver.Id_Dept as Id_Dept_param, ord_ver.Id_Land as Id_Land_param, ord_ver.Dept_name as Dept_name_param, ord_ver.Land_name as Land_name_param, " +
                "ord_ver.Id_Currency_Order as Id_Currency_Order_param, ord_ver.Id_Supplier_VersionOrder as Id_Supplier_VersionOrder_param,ord_ver.TotalCost_Order as TotalCost_Order_param, ord_ver.Step_Order as Step_Order_param, ord_ver.Id_Project as Id_Project_param " +
                "from Budget_Order_Version as ord_ver " +
                "inner join Article_Order as artikel on artikel.Id_Order = ord_ver.Id_Order " +
                "Group by ord_ver.Id_VO, ord_ver.Nr_version_Order, ord_ver.Id_Level, ord_ver.Id_Status,ord_ver.Id_Dept, ord_ver.Id_Land, ord_ver.Dept_name, ord_ver.Land_name, " +
                "ord_ver.Id_Currency_Order, ord_ver.Id_Supplier_VersionOrder, ord_ver.TotalCost_Order, ord_ver.Step_Order, ord_ver.Id_Project " +
                "Having ord_ver.Id_VO = max(ord_ver.Id_VO)";*/
				string query = "select max(ord_ver.Id_VO) as Max_VO,ord_ver.Nr_version_Order as Nr_version_Order_param,ord_ver.Id_Level as Id_Level_param, ord_ver.Id_Status as Id_Status_param, ord_ver.Id_Dept as Id_Dept_param, ord_ver.Id_Land as Id_Land_param, ord_ver.Dept_name as Dept_name_param, ord_ver.Land_name as Land_name_param, " +
				"ord_ver.Id_Currency_Order as Id_Currency_Order_param, ord_ver.Id_Supplier_VersionOrder as Id_Supplier_VersionOrder_param,ord_ver.TotalCost_Order as TotalCost_Order_param, ord_ver.Step_Order as Step_Order_param, ord_ver.Id_Project as Id_Project_param " +
				"from Budget_Order_Version as ord_ver WHERE ord_ver.Id_Order =@Id_Order " +
				"Group by ord_ver.Id_VO, ord_ver.Nr_version_Order, ord_ver.Id_Level, ord_ver.Id_Status,ord_ver.Id_Dept, ord_ver.Id_Land, ord_ver.Dept_name, ord_ver.Land_name, " +
				"ord_ver.Id_Currency_Order, ord_ver.Id_Supplier_VersionOrder, ord_ver.TotalCost_Order, ord_ver.Step_Order, ord_ver.Id_Project";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_Order", Id_Order);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return toListParamsArtikelOrder(dataTable);

			}
			else
			{ return null; }

		}
		public static int InsertArtikelVersionOrder(Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity item, int Max_Ver_Ord)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> params_Version_Order = GetLastVersionOrder(Max_Ver_Ord);

				int? Max_VO = params_Version_Order[0].Max_VO;
				int? Nr_version_Order_param = params_Version_Order[0].Nr_version_Order_param;
				int? Id_Level_param = params_Version_Order[0].Id_Level_param;
				int? Id_Status_param = params_Version_Order[0].Id_Status_param;
				int? Id_Dept_param = params_Version_Order[0].Id_Dept_param;
				int? Id_Land_param = params_Version_Order[0].Id_Land_param;
				string Dept_name_param = params_Version_Order[0].Dept_name_param;
				string Land_name_param = params_Version_Order[0].Land_name_param;
				int? Id_Currency_Order_param = params_Version_Order[0].Id_Currency_Order_param;
				int? Id_Supplier_VersionOrder_param = params_Version_Order[0].Id_Supplier_VersionOrder_param;
				double? TotalCost_Order_param = params_Version_Order[0].TotalCost_Order_param;
				string Step_Order_param = params_Version_Order[0].Step_Order_param;
				int? Id_Project_param = params_Version_Order[0].Id_Project_param;

				string query = "INSERT INTO [Article_Order] ([Currency_Article],[Id_Article],[Id_Currency_Article],[Id_Order],[Quantity],[TotalCost_Article],[Unit_Price])  VALUES (@Currency_Article,@Id_Article,@Id_Currency_Article,@Id_Order,@Quantity,@TotalCost_Article,@Unit_Price) " +
							   "UPDATE[Budget_Order_Version] SET[TotalCost_Order]=@TotalCost_Order WHERE[Id_VO]=@Max_VO";
				//using (var sqlCommand = new SqlCommand(query, sqlConnection)) { }

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Currency_Article", item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
				sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Article", item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
				sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
				//sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
				sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.Unit_Price * item.Quantity);
				sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
				sqlCommand.Parameters.AddWithValue("Max_VO", Max_VO);
				sqlCommand.Parameters.AddWithValue("TotalCost_Order", TotalCost_Order_param + item.Unit_Price * item.Quantity);

				Debug.WriteLine("article", item.Unit_Price.Value);
				DbExecution.ExecuteNonQuery(sqlCommand);



				using(var sqlCommand1 = new SqlCommand("SELECT [Id_AO] FROM [Article_Order] WHERE [Id_AO] = @@IDENTITY", sqlConnection))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand1)?.ToString() ?? "-1");
				}



				return response;
			}
		}

		public static int InsertListArtikelVersionOrder(List<Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity> items, int Max_Ver_Ord)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> params_Version_Order = GetLastVersionOrder(Max_Ver_Ord);

					int? Max_VO = params_Version_Order[0].Max_VO;
					int? Nr_version_Order_param = params_Version_Order[0].Nr_version_Order_param;
					int? Id_Level_param = params_Version_Order[0].Id_Level_param;
					int? Id_Status_param = params_Version_Order[0].Id_Status_param;
					int? Id_Dept_param = params_Version_Order[0].Id_Dept_param;
					int? Id_Land_param = params_Version_Order[0].Id_Land_param;
					string Dept_name_param = params_Version_Order[0].Dept_name_param;
					string Land_name_param = params_Version_Order[0].Land_name_param;
					int? Id_Currency_Order_param = params_Version_Order[0].Id_Currency_Order_param;
					int? Id_Supplier_VersionOrder_param = params_Version_Order[0].Id_Supplier_VersionOrder_param;
					double? TotalCost_Order_param = params_Version_Order[0].TotalCost_Order_param;
					string Step_Order_param = params_Version_Order[0].Step_Order_param;
					int? Id_Project_param = params_Version_Order[0].Id_Project_param;

					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [Article_Order] ([Currency_Article],[Id_Article],[Id_Currency_Article],[Id_Order],[Quantity],[TotalCost_Article],[Unit_Price]) VALUES ( "



							+ "@Currency_Article" + i + ","

							+ "@Id_Article" + i + ","
							+ "@Id_Currency_Article" + i + ","

							+ "@Id_Order" + i + ","
							+ "@Quantity" + i + ","
							+ "@TotalCost_Article" + i + ","
							+ "@Unit_Price" + i
							+ "); " + "UPDATE[Budget_Order_Version] SET[TotalCost_Order] = @TotalCost_Order WHERE[Id_VO] = @Max_VO";



						sqlCommand.Parameters.AddWithValue("Currency_Article" + i, item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);
						sqlCommand.Parameters.AddWithValue("Id_Article" + i, item.Id_Article);
						sqlCommand.Parameters.AddWithValue("Id_Currency_Article" + i, item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);

						sqlCommand.Parameters.AddWithValue("Id_Order" + i, item.Id_Order);

						sqlCommand.Parameters.AddWithValue("Quantity" + i, item.Quantity == null ? (object)DBNull.Value : item.Quantity);
						sqlCommand.Parameters.AddWithValue("TotalCost_Article" + i, item.TotalCost_Article == null ? (object)DBNull.Value : item.Unit_Price * item.Quantity);
						sqlCommand.Parameters.AddWithValue("Unit_Price" + i, item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
						sqlCommand.Parameters.AddWithValue("Max_VO", Max_VO);
						sqlCommand.Parameters.AddWithValue("TotalCost_Order", TotalCost_Order_param + item.Unit_Price * item.Quantity);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int UpdateArtikelVersionOrder(Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity item, int Max_Ver_Ord)
		{
			int response = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> params_Version_Order = GetLastVersionOrder(Max_Ver_Ord);

				int? Max_VO = params_Version_Order[0].Max_VO;
				int? Nr_version_Order_param = params_Version_Order[0].Nr_version_Order_param;
				int? Id_Level_param = params_Version_Order[0].Id_Level_param;
				int? Id_Status_param = params_Version_Order[0].Id_Status_param;
				int? Id_Dept_param = params_Version_Order[0].Id_Dept_param;
				int? Id_Land_param = params_Version_Order[0].Id_Land_param;
				string Dept_name_param = params_Version_Order[0].Dept_name_param;
				string Land_name_param = params_Version_Order[0].Land_name_param;
				int? Id_Currency_Order_param = params_Version_Order[0].Id_Currency_Order_param;
				int? Id_Supplier_VersionOrder_param = params_Version_Order[0].Id_Supplier_VersionOrder_param;
				double? TotalCost_Order_param = params_Version_Order[0].TotalCost_Order_param;
				string Step_Order_param = params_Version_Order[0].Step_Order_param;
				int? Id_Project_param = params_Version_Order[0].Id_Project_param;


				Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity old_Version_Artikel = GetArtikelOrderByIdArtikel(item.Id_AO);


				string query = "UPDATE [Article_Order] SET [Article_date]=@Article_date, [Currency_Article]=@Currency_Article,  [Id_Article]=@Id_Article, [Id_Currency_Article]=@Id_Currency_Article, [Id_Dept]=@Id_Dept, [Id_Land]=@Id_Land, [Id_Order]=@Id_Order, [Id_Project]=@Id_Project, [Id_User]=@Id_User, [Land_name]=@Land_name, [Quantity]=@Quantity, [TotalCost_Article]=@TotalCost_Article, [Unit_Price]=@Unit_Price WHERE [Id_AO]=@Id_AO " +
							   "UPDATE[Budget_Order_Version] SET [TotalCost_Order]=@TotalCost_Order WHERE [Id_VO]=@Max_VO";
				//using (var sqlCommand = new SqlCommand(query, sqlConnection)) { }

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_AO", item.Id_AO);
				sqlCommand.Parameters.AddWithValue("Article_date", item.Article_date == null ? (object)DBNull.Value : item.Article_date);
				sqlCommand.Parameters.AddWithValue("Currency_Article", item.Currency_Article == null ? (object)DBNull.Value : item.Currency_Article);

				sqlCommand.Parameters.AddWithValue("Id_Article", item.Id_Article);
				sqlCommand.Parameters.AddWithValue("Id_Currency_Article", item.Id_Currency_Article == null ? (object)DBNull.Value : item.Id_Currency_Article);
				sqlCommand.Parameters.AddWithValue("Id_Dept", item.Id_Dept == null ? (object)DBNull.Value : item.Id_Dept);
				sqlCommand.Parameters.AddWithValue("Id_Land", item.Id_Land == null ? (object)DBNull.Value : item.Id_Land);
				sqlCommand.Parameters.AddWithValue("Id_Order", item.Id_Order);
				sqlCommand.Parameters.AddWithValue("Id_Project", item.Id_Project == null ? (object)DBNull.Value : item.Id_Project);
				sqlCommand.Parameters.AddWithValue("Id_User", item.Id_User);
				sqlCommand.Parameters.AddWithValue("Land_name", item.Land_name == null ? (object)DBNull.Value : item.Land_name);
				sqlCommand.Parameters.AddWithValue("Quantity", item.Quantity == null ? (object)DBNull.Value : item.Quantity);
				//sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.TotalCost_Article == null ? (object)DBNull.Value : item.TotalCost_Article);
				sqlCommand.Parameters.AddWithValue("TotalCost_Article", item.Unit_Price * item.Quantity);
				sqlCommand.Parameters.AddWithValue("Unit_Price", item.Unit_Price == null ? (object)DBNull.Value : item.Unit_Price);
				sqlCommand.Parameters.AddWithValue("Max_VO", Max_VO);
				sqlCommand.Parameters.AddWithValue("TotalCost_Order", TotalCost_Order_param - old_Version_Artikel.TotalCost_Article + item.Unit_Price * item.Quantity);

				Debug.WriteLine("article", item.Unit_Price.Value);
				DbExecution.ExecuteNonQuery(sqlCommand);



				using(var sqlCommand1 = new SqlCommand("SELECT [Id_AO] FROM [Article_Order] WHERE [Id_AO] = @@IDENTITY", sqlConnection))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand1)?.ToString() ?? "-1");
				}





				return response;
			}
		}

		public static int DeleteArtikel_Order(int id_ao, int Max_Ver_Ord, Infrastructure.Data.Entities.Tables.FNC.Inserted_Article_OrderEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity old_Version_Artikel = GetArtikelOrderByIdArtikel(id_ao);

				List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> params_Version_Order = GetLastVersionOrder(Max_Ver_Ord);

				int? Max_VO = params_Version_Order[0].Max_VO;
				double? TotalCost_Order_param = params_Version_Order[0].TotalCost_Order_param;


				string query = "DELETE FROM [Article_Order] WHERE [Id_AO]=@Id_AO " +
					"UPDATE[Budget_Order_Version] SET [TotalCost_Order]=@TotalCost_Order WHERE [Id_VO]=@Max_VO";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_AO", id_ao);
				sqlCommand.Parameters.AddWithValue("Max_VO", Max_VO);
				sqlCommand.Parameters.AddWithValue("TotalCost_Order", TotalCost_Order_param - old_Version_Artikel.TotalCost_Article);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity GetArtikelOrderByIdArtikel(int Id)
		{
			var dataTable = new DataTable();


			var response = new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				string query = "select * from [Article_Order] where Id_AO=@Id_AO";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id_AO", Id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int DeleteByOrderId(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Article_Order] WHERE [Id_Order]=@id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		#endregion

		#region Helpers

		private static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity> toList(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Article_OrderEntity(dataRow)); }
			return list;
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity> toList_ConcatName(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Article_Order_ConcatNameEntity(dataRow)); }
			return list;
		}

		private static List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity> toListParamsArtikelOrder(DataTable dataTable)
		{
			var list = new List<Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Infrastructure.Data.Entities.Tables.FNC.Article_OrderParamsEntity(dataRow)); }
			return list;
		}

		#endregion
	}
}
