using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class InvoiceItemAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_InvoiceItem] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_InvoiceItem]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_InvoiceItem] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_InvoiceItem] ([AB-Nr_Lieferant],[AccountId],[AccountName],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[ArticleId],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestatigter_Termin],[Bestellnummer],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[ConfirmationDate],[CUPreis],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[InternalContact],[InvoiceId],[Kanban],[Lagerort_id],[Liefertermin],[LocationId],[LocationName],[Löschen],[MhdDatumArtikel],[OrderId],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[Umsatzsteuer],[UnitPrice],[UnitPriceDefaultCurrency],[VAT],[WE Pos zu Bestellposition])  VALUES (@AB_Nr_Lieferant,@AccountId,@AccountName,@Aktuelle_Anzahl,@AnfangLagerBestand,@Anzahl,@ArticleId,@Artikel_Nr,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Bestatigter_Termin,@Bestellnummer,@Bezeichnung_1,@Bezeichnung_2,@BP_zu_RBposition,@COC_bestätigung,@ConfirmationDate,@CUPreis,@CurrencyId,@CurrencyName,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@DeliveryDate,@Description,@Discount,@Einheit,@Einzelpreis,@EMPB_Bestätigung,@EndeLagerBestand,@Erhalten,@erledigt_pos,@Gesamtpreis,@In_Bearbeitung,@InfoRahmennummer,@InternalContact,@InvoiceId,@Kanban,@Lagerort_id,@Liefertermin,@LocationId,@LocationName,@Löschen,@MhdDatumArtikel,@OrderId,@Position,@Position_erledigt,@Preiseinheit,@Preisgruppe,@Produktionsort,@Quantity,@Rabatt,@Rabatt1,@RB_Abgerufen,@RB_Offen,@RB_OriginalAnzahl,@schriftart,@sortierung,@Start_Anzahl,@SupplierDeliveryDate,@SupplierOrderNumber,@TotalCost,@TotalCostDefaultCurrency,@Umsatzsteuer,@UnitPrice,@UnitPriceDefaultCurrency,@VAT,@WE_Pos_zu_Bestellposition); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
					sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestätigung", item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
					sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
					sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung", item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("InvoiceId", item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
					sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
					sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
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
					sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
					sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
					sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
					sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items)
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
						query += " INSERT INTO [__FNC_InvoiceItem] ([AB-Nr_Lieferant],[AccountId],[AccountName],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[ArticleId],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestatigter_Termin],[Bestellnummer],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[ConfirmationDate],[CUPreis],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[InternalContact],[InvoiceId],[Kanban],[Lagerort_id],[Liefertermin],[LocationId],[LocationName],[Löschen],[MhdDatumArtikel],[OrderId],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[Umsatzsteuer],[UnitPrice],[UnitPriceDefaultCurrency],[VAT],[WE Pos zu Bestellposition]) VALUES ( "

							+ "@AB_Nr_Lieferant" + i + ","
							+ "@AccountId" + i + ","
							+ "@AccountName" + i + ","
							+ "@Aktuelle_Anzahl" + i + ","
							+ "@AnfangLagerBestand" + i + ","
							+ "@Anzahl" + i + ","
							+ "@ArticleId" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Bemerkung_Pos" + i + ","
							+ "@Bemerkung_Pos_ID" + i + ","
							+ "@Bestatigter_Termin" + i + ","
							+ "@Bestellnummer" + i + ","
							+ "@Bezeichnung_1" + i + ","
							+ "@Bezeichnung_2" + i + ","
							+ "@BP_zu_RBposition" + i + ","
							+ "@COC_bestätigung" + i + ","
							+ "@ConfirmationDate" + i + ","
							+ "@CUPreis" + i + ","
							+ "@CurrencyId" + i + ","
							+ "@CurrencyName" + i + ","
							+ "@DefaultCurrencyDecimals" + i + ","
							+ "@DefaultCurrencyId" + i + ","
							+ "@DefaultCurrencyName" + i + ","
							+ "@DefaultCurrencyRate" + i + ","
							+ "@DeliveryDate" + i + ","
							+ "@Description" + i + ","
							+ "@Discount" + i + ","
							+ "@Einheit" + i + ","
							+ "@Einzelpreis" + i + ","
							+ "@EMPB_Bestätigung" + i + ","
							+ "@EndeLagerBestand" + i + ","
							+ "@Erhalten" + i + ","
							+ "@erledigt_pos" + i + ","
							+ "@Gesamtpreis" + i + ","
							+ "@In_Bearbeitung" + i + ","
							+ "@InfoRahmennummer" + i + ","
							+ "@InternalContact" + i + ","
							+ "@InvoiceId" + i + ","
							+ "@Kanban" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@Liefertermin" + i + ","
							+ "@LocationId" + i + ","
							+ "@LocationName" + i + ","
							+ "@Löschen" + i + ","
							+ "@MhdDatumArtikel" + i + ","
							+ "@OrderId" + i + ","
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
							+ "@SupplierDeliveryDate" + i + ","
							+ "@SupplierOrderNumber" + i + ","
							+ "@TotalCost" + i + ","
							+ "@TotalCostDefaultCurrency" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@UnitPrice" + i + ","
							+ "@UnitPriceDefaultCurrency" + i + ","
							+ "@VAT" + i + ","
							+ "@WE_Pos_zu_Bestellposition" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
						sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestätigung" + i, item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
						sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung" + i, item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("InvoiceId" + i, item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
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
						sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
						sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
						sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
						sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
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

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_InvoiceItem] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [AccountId]=@AccountId, [AccountName]=@AccountName, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [ArticleId]=@ArticleId, [Artikel-Nr]=@Artikel_Nr, [Bemerkung_Pos]=@Bemerkung_Pos, [Bemerkung_Pos_ID]=@Bemerkung_Pos_ID, [Bestatigter_Termin]=@Bestatigter_Termin, [Bestellnummer]=@Bestellnummer, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [BP zu RBposition]=@BP_zu_RBposition, [COC_bestätigung]=@COC_bestätigung, [ConfirmationDate]=@ConfirmationDate, [CUPreis]=@CUPreis, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [DeliveryDate]=@DeliveryDate, [Description]=@Description, [Discount]=@Discount, [Einheit]=@Einheit, [Einzelpreis]=@Einzelpreis, [EMPB_Bestätigung]=@EMPB_Bestätigung, [EndeLagerBestand]=@EndeLagerBestand, [Erhalten]=@Erhalten, [erledigt_pos]=@erledigt_pos, [Gesamtpreis]=@Gesamtpreis, [In Bearbeitung]=@In_Bearbeitung, [InfoRahmennummer]=@InfoRahmennummer, [InternalContact]=@InternalContact, [InvoiceId]=@InvoiceId, [Kanban]=@Kanban, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [LocationId]=@LocationId, [LocationName]=@LocationName, [Löschen]=@Löschen, [MhdDatumArtikel]=@MhdDatumArtikel, [OrderId]=@OrderId, [Position]=@Position, [Position erledigt]=@Position_erledigt, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [Produktionsort]=@Produktionsort, [Quantity]=@Quantity, [Rabatt]=@Rabatt, [Rabatt1]=@Rabatt1, [RB_Abgerufen]=@RB_Abgerufen, [RB_Offen]=@RB_Offen, [RB_OriginalAnzahl]=@RB_OriginalAnzahl, [schriftart]=@schriftart, [sortierung]=@sortierung, [Start Anzahl]=@Start_Anzahl, [SupplierDeliveryDate]=@SupplierDeliveryDate, [SupplierOrderNumber]=@SupplierOrderNumber, [TotalCost]=@TotalCost, [TotalCostDefaultCurrency]=@TotalCostDefaultCurrency, [Umsatzsteuer]=@Umsatzsteuer, [UnitPrice]=@UnitPrice, [UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency, [VAT]=@VAT, [WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
				sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
				sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
				sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
				sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
				sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
				sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
				sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
				sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
				sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
				sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
				sqlCommand.Parameters.AddWithValue("COC_bestätigung", item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
				sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
				sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
				sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
				sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
				sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
				sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
				sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
				sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
				sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung", item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
				sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
				sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
				sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
				sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
				sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
				sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
				sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
				sqlCommand.Parameters.AddWithValue("InvoiceId", item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
				sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
				sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
				sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
				sqlCommand.Parameters.AddWithValue("Löschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
				sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
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
				sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
				sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
				sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
				sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
				sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
				sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
				sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items)
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
						query += " UPDATE [__FNC_InvoiceItem] SET "

							+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
							+ "[AccountId]=@AccountId" + i + ","
							+ "[AccountName]=@AccountName" + i + ","
							+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
							+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
							+ "[Anzahl]=@Anzahl" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Bemerkung_Pos]=@Bemerkung_Pos" + i + ","
							+ "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i + ","
							+ "[Bestatigter_Termin]=@Bestatigter_Termin" + i + ","
							+ "[Bestellnummer]=@Bestellnummer" + i + ","
							+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
							+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
							+ "[BP zu RBposition]=@BP_zu_RBposition" + i + ","
							+ "[COC_bestätigung]=@COC_bestätigung" + i + ","
							+ "[ConfirmationDate]=@ConfirmationDate" + i + ","
							+ "[CUPreis]=@CUPreis" + i + ","
							+ "[CurrencyId]=@CurrencyId" + i + ","
							+ "[CurrencyName]=@CurrencyName" + i + ","
							+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
							+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
							+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
							+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
							+ "[DeliveryDate]=@DeliveryDate" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Discount]=@Discount" + i + ","
							+ "[Einheit]=@Einheit" + i + ","
							+ "[Einzelpreis]=@Einzelpreis" + i + ","
							+ "[EMPB_Bestätigung]=@EMPB_Bestätigung" + i + ","
							+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
							+ "[Erhalten]=@Erhalten" + i + ","
							+ "[erledigt_pos]=@erledigt_pos" + i + ","
							+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
							+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
							+ "[InfoRahmennummer]=@InfoRahmennummer" + i + ","
							+ "[InternalContact]=@InternalContact" + i + ","
							+ "[InvoiceId]=@InvoiceId" + i + ","
							+ "[Kanban]=@Kanban" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[Liefertermin]=@Liefertermin" + i + ","
							+ "[LocationId]=@LocationId" + i + ","
							+ "[LocationName]=@LocationName" + i + ","
							+ "[Löschen]=@Löschen" + i + ","
							+ "[MhdDatumArtikel]=@MhdDatumArtikel" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
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
							+ "[SupplierDeliveryDate]=@SupplierDeliveryDate" + i + ","
							+ "[SupplierOrderNumber]=@SupplierOrderNumber" + i + ","
							+ "[TotalCost]=@TotalCost" + i + ","
							+ "[TotalCostDefaultCurrency]=@TotalCostDefaultCurrency" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[UnitPrice]=@UnitPrice" + i + ","
							+ "[UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency" + i + ","
							+ "[VAT]=@VAT" + i + ","
							+ "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
						sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
						sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
						sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
						sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
						sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
						sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
						sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
						sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
						sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
						sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
						sqlCommand.Parameters.AddWithValue("COC_bestätigung" + i, item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
						sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
						sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
						sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
						sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
						sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
						sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
						sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
						sqlCommand.Parameters.AddWithValue("EMPB_Bestätigung" + i, item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
						sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
						sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
						sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
						sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
						sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
						sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
						sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
						sqlCommand.Parameters.AddWithValue("InvoiceId" + i, item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
						sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
						sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
						sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
						sqlCommand.Parameters.AddWithValue("Löschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
						sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
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
						sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
						sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
						sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
						sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
						sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
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

		public static int Delete(int nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_InvoiceItem] WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", nr);

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

					string query = "DELETE FROM [__FNC_InvoiceItem] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_InvoiceItem] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__FNC_InvoiceItem]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__FNC_InvoiceItem] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__FNC_InvoiceItem] ([AB-Nr_Lieferant],[AccountId],[AccountName],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[ArticleId],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestatigter_Termin],[Bestellnummer],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[ConfirmationDate],[CUPreis],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[InternalContact],[InvoiceId],[Kanban],[Lagerort_id],[Liefertermin],[LocationId],[LocationName],[Löschen],[MhdDatumArtikel],[OrderId],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[Umsatzsteuer],[UnitPrice],[UnitPriceDefaultCurrency],[VAT],[WE Pos zu Bestellposition]) OUTPUT INSERTED.[Id] VALUES (@AB_Nr_Lieferant,@AccountId,@AccountName,@Aktuelle_Anzahl,@AnfangLagerBestand,@Anzahl,@ArticleId,@Artikel_Nr,@Bemerkung_Pos,@Bemerkung_Pos_ID,@Bestatigter_Termin,@Bestellnummer,@Bezeichnung_1,@Bezeichnung_2,@BP_zu_RBposition,@COC_bestatigung,@ConfirmationDate,@CUPreis,@CurrencyId,@CurrencyName,@DefaultCurrencyDecimals,@DefaultCurrencyId,@DefaultCurrencyName,@DefaultCurrencyRate,@DeliveryDate,@Description,@Discount,@Einheit,@Einzelpreis,@EMPB_Bestatigung,@EndeLagerBestand,@Erhalten,@erledigt_pos,@Gesamtpreis,@In_Bearbeitung,@InfoRahmennummer,@InternalContact,@InvoiceId,@Kanban,@Lagerort_id,@Liefertermin,@LocationId,@LocationName,@Loschen,@MhdDatumArtikel,@OrderId,@Position,@Position_erledigt,@Preiseinheit,@Preisgruppe,@Produktionsort,@Quantity,@Rabatt,@Rabatt1,@RB_Abgerufen,@RB_Offen,@RB_OriginalAnzahl,@schriftart,@sortierung,@Start_Anzahl,@SupplierDeliveryDate,@SupplierOrderNumber,@TotalCost,@TotalCostDefaultCurrency,@Umsatzsteuer,@UnitPrice,@UnitPriceDefaultCurrency,@VAT,@WE_Pos_zu_Bestellposition); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
			sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
			sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
			sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
			sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
			sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestätigung== null ? (object)DBNull.Value : item.EMPB_Bestätigung);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
			sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
			sqlCommand.Parameters.AddWithValue("InvoiceId", item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
			sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
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
			sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
			sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
			sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
			sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
			sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
			sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__FNC_InvoiceItem] ([AB-Nr_Lieferant],[AccountId],[AccountName],[Aktuelle Anzahl],[AnfangLagerBestand],[Anzahl],[ArticleId],[Artikel-Nr],[Bemerkung_Pos],[Bemerkung_Pos_ID],[Bestatigter_Termin],[Bestellnummer],[Bezeichnung 1],[Bezeichnung 2],[BP zu RBposition],[COC_bestätigung],[ConfirmationDate],[CUPreis],[CurrencyId],[CurrencyName],[DefaultCurrencyDecimals],[DefaultCurrencyId],[DefaultCurrencyName],[DefaultCurrencyRate],[DeliveryDate],[Description],[Discount],[Einheit],[Einzelpreis],[EMPB_Bestätigung],[EndeLagerBestand],[Erhalten],[erledigt_pos],[Gesamtpreis],[In Bearbeitung],[InfoRahmennummer],[InternalContact],[InvoiceId],[Kanban],[Lagerort_id],[Liefertermin],[LocationId],[LocationName],[Löschen],[MhdDatumArtikel],[OrderId],[Position],[Position erledigt],[Preiseinheit],[Preisgruppe],[Produktionsort],[Quantity],[Rabatt],[Rabatt1],[RB_Abgerufen],[RB_Offen],[RB_OriginalAnzahl],[schriftart],[sortierung],[Start Anzahl],[SupplierDeliveryDate],[SupplierOrderNumber],[TotalCost],[TotalCostDefaultCurrency],[Umsatzsteuer],[UnitPrice],[UnitPriceDefaultCurrency],[VAT],[WE Pos zu Bestellposition]) VALUES ( "

						+ "@AB_Nr_Lieferant" + i + ","
						+ "@AccountId" + i + ","
						+ "@AccountName" + i + ","
						+ "@Aktuelle_Anzahl" + i + ","
						+ "@AnfangLagerBestand" + i + ","
						+ "@Anzahl" + i + ","
						+ "@ArticleId" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Bemerkung_Pos" + i + ","
						+ "@Bemerkung_Pos_ID" + i + ","
						+ "@Bestatigter_Termin" + i + ","
						+ "@Bestellnummer" + i + ","
						+ "@Bezeichnung_1" + i + ","
						+ "@Bezeichnung_2" + i + ","
						+ "@BP_zu_RBposition" + i + ","
						+ "@COC_bestatigung" + i + ","
						+ "@ConfirmationDate" + i + ","
						+ "@CUPreis" + i + ","
						+ "@CurrencyId" + i + ","
						+ "@CurrencyName" + i + ","
						+ "@DefaultCurrencyDecimals" + i + ","
						+ "@DefaultCurrencyId" + i + ","
						+ "@DefaultCurrencyName" + i + ","
						+ "@DefaultCurrencyRate" + i + ","
						+ "@DeliveryDate" + i + ","
						+ "@Description" + i + ","
						+ "@Discount" + i + ","
						+ "@Einheit" + i + ","
						+ "@Einzelpreis" + i + ","
						+ "@EMPB_Bestatigung" + i + ","
						+ "@EndeLagerBestand" + i + ","
						+ "@Erhalten" + i + ","
						+ "@erledigt_pos" + i + ","
						+ "@Gesamtpreis" + i + ","
						+ "@In_Bearbeitung" + i + ","
						+ "@InfoRahmennummer" + i + ","
						+ "@InternalContact" + i + ","
						+ "@InvoiceId" + i + ","
						+ "@Kanban" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@Liefertermin" + i + ","
						+ "@LocationId" + i + ","
						+ "@LocationName" + i + ","
						+ "@Loschen" + i + ","
						+ "@MhdDatumArtikel" + i + ","
						+ "@OrderId" + i + ","
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
						+ "@SupplierDeliveryDate" + i + ","
						+ "@SupplierOrderNumber" + i + ","
						+ "@TotalCost" + i + ","
						+ "@TotalCostDefaultCurrency" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@UnitPrice" + i + ","
						+ "@UnitPriceDefaultCurrency" + i + ","
						+ "@VAT" + i + ","
						+ "@WE_Pos_zu_Bestellposition" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
					sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
					sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
					sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("InvoiceId" + i, item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
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
					sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
					sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
					sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
					sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__FNC_InvoiceItem] SET [AB-Nr_Lieferant]=@AB_Nr_Lieferant, [AccountId]=@AccountId, [AccountName]=@AccountName, [Aktuelle Anzahl]=@Aktuelle_Anzahl, [AnfangLagerBestand]=@AnfangLagerBestand, [Anzahl]=@Anzahl, [ArticleId]=@ArticleId, [Artikel-Nr]=@Artikel_Nr, [Bemerkung_Pos]=@Bemerkung_Pos, [Bemerkung_Pos_ID]=@Bemerkung_Pos_ID, [Bestatigter_Termin]=@Bestatigter_Termin, [Bestellnummer]=@Bestellnummer, [Bezeichnung 1]=@Bezeichnung_1, [Bezeichnung 2]=@Bezeichnung_2, [BP zu RBposition]=@BP_zu_RBposition, [COC_bestätigung]=@COC_bestatigung, [ConfirmationDate]=@ConfirmationDate, [CUPreis]=@CUPreis, [CurrencyId]=@CurrencyId, [CurrencyName]=@CurrencyName, [DefaultCurrencyDecimals]=@DefaultCurrencyDecimals, [DefaultCurrencyId]=@DefaultCurrencyId, [DefaultCurrencyName]=@DefaultCurrencyName, [DefaultCurrencyRate]=@DefaultCurrencyRate, [DeliveryDate]=@DeliveryDate, [Description]=@Description, [Discount]=@Discount, [Einheit]=@Einheit, [Einzelpreis]=@Einzelpreis, [EMPB_Bestätigung]=@EMPB_Bestatigung, [EndeLagerBestand]=@EndeLagerBestand, [Erhalten]=@Erhalten, [erledigt_pos]=@erledigt_pos, [Gesamtpreis]=@Gesamtpreis, [In Bearbeitung]=@In_Bearbeitung, [InfoRahmennummer]=@InfoRahmennummer, [InternalContact]=@InternalContact, [InvoiceId]=@InvoiceId, [Kanban]=@Kanban, [Lagerort_id]=@Lagerort_id, [Liefertermin]=@Liefertermin, [LocationId]=@LocationId, [LocationName]=@LocationName, [Löschen]=@Loschen, [MhdDatumArtikel]=@MhdDatumArtikel, [OrderId]=@OrderId, [Position]=@Position, [Position erledigt]=@Position_erledigt, [Preiseinheit]=@Preiseinheit, [Preisgruppe]=@Preisgruppe, [Produktionsort]=@Produktionsort, [Quantity]=@Quantity, [Rabatt]=@Rabatt, [Rabatt1]=@Rabatt1, [RB_Abgerufen]=@RB_Abgerufen, [RB_Offen]=@RB_Offen, [RB_OriginalAnzahl]=@RB_OriginalAnzahl, [schriftart]=@schriftart, [sortierung]=@sortierung, [Start Anzahl]=@Start_Anzahl, [SupplierDeliveryDate]=@SupplierDeliveryDate, [SupplierOrderNumber]=@SupplierOrderNumber, [TotalCost]=@TotalCost, [TotalCostDefaultCurrency]=@TotalCostDefaultCurrency, [Umsatzsteuer]=@Umsatzsteuer, [UnitPrice]=@UnitPrice, [UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency, [VAT]=@VAT, [WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant", item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
			sqlCommand.Parameters.AddWithValue("AccountId", item.AccountId == null ? (object)DBNull.Value : item.AccountId);
			sqlCommand.Parameters.AddWithValue("AccountName", item.AccountName == null ? (object)DBNull.Value : item.AccountName);
			sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl", item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
			sqlCommand.Parameters.AddWithValue("AnfangLagerBestand", item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
			sqlCommand.Parameters.AddWithValue("Anzahl", item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos", item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
			sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID", item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
			sqlCommand.Parameters.AddWithValue("Bestatigter_Termin", item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
			sqlCommand.Parameters.AddWithValue("Bestellnummer", item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_1", item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
			sqlCommand.Parameters.AddWithValue("Bezeichnung_2", item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
			sqlCommand.Parameters.AddWithValue("BP_zu_RBposition", item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
			sqlCommand.Parameters.AddWithValue("COC_bestatigung", item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
			sqlCommand.Parameters.AddWithValue("ConfirmationDate", item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
			sqlCommand.Parameters.AddWithValue("CUPreis", item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
			sqlCommand.Parameters.AddWithValue("CurrencyId", item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
			sqlCommand.Parameters.AddWithValue("CurrencyName", item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals", item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyId", item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyName", item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
			sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate", item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
			sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
			sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
			sqlCommand.Parameters.AddWithValue("Discount", item.Discount == null ? (object)DBNull.Value : item.Discount);
			sqlCommand.Parameters.AddWithValue("Einheit", item.Einheit == null ? (object)DBNull.Value : item.Einheit);
			sqlCommand.Parameters.AddWithValue("Einzelpreis", item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
			sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung", item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
			sqlCommand.Parameters.AddWithValue("EndeLagerBestand", item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
			sqlCommand.Parameters.AddWithValue("Erhalten", item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
			sqlCommand.Parameters.AddWithValue("erledigt_pos", item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
			sqlCommand.Parameters.AddWithValue("Gesamtpreis", item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
			sqlCommand.Parameters.AddWithValue("In_Bearbeitung", item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
			sqlCommand.Parameters.AddWithValue("InfoRahmennummer", item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
			sqlCommand.Parameters.AddWithValue("InternalContact", item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
			sqlCommand.Parameters.AddWithValue("InvoiceId", item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
			sqlCommand.Parameters.AddWithValue("Kanban", item.Kanban == null ? (object)DBNull.Value : item.Kanban);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("Liefertermin", item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
			sqlCommand.Parameters.AddWithValue("LocationId", item.LocationId == null ? (object)DBNull.Value : item.LocationId);
			sqlCommand.Parameters.AddWithValue("LocationName", item.LocationName == null ? (object)DBNull.Value : item.LocationName);
			sqlCommand.Parameters.AddWithValue("Loschen", item.Löschen == null ? (object)DBNull.Value : item.Löschen);
			sqlCommand.Parameters.AddWithValue("MhdDatumArtikel", item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
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
			sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate", item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
			sqlCommand.Parameters.AddWithValue("SupplierOrderNumber", item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
			sqlCommand.Parameters.AddWithValue("TotalCost", item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
			sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency", item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("UnitPrice", item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
			sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency", item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
			sqlCommand.Parameters.AddWithValue("VAT", item.VAT == null ? (object)DBNull.Value : item.VAT);
			sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition", item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 71; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__FNC_InvoiceItem] SET "

					+ "[AB-Nr_Lieferant]=@AB_Nr_Lieferant" + i + ","
					+ "[AccountId]=@AccountId" + i + ","
					+ "[AccountName]=@AccountName" + i + ","
					+ "[Aktuelle Anzahl]=@Aktuelle_Anzahl" + i + ","
					+ "[AnfangLagerBestand]=@AnfangLagerBestand" + i + ","
					+ "[Anzahl]=@Anzahl" + i + ","
					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
					+ "[Bemerkung_Pos]=@Bemerkung_Pos" + i + ","
					+ "[Bemerkung_Pos_ID]=@Bemerkung_Pos_ID" + i + ","
					+ "[Bestatigter_Termin]=@Bestatigter_Termin" + i + ","
					+ "[Bestellnummer]=@Bestellnummer" + i + ","
					+ "[Bezeichnung 1]=@Bezeichnung_1" + i + ","
					+ "[Bezeichnung 2]=@Bezeichnung_2" + i + ","
					+ "[BP zu RBposition]=@BP_zu_RBposition" + i + ","
					+ "[COC_bestätigung]=@COC_bestatigung" + i + ","
					+ "[ConfirmationDate]=@ConfirmationDate" + i + ","
					+ "[CUPreis]=@CUPreis" + i + ","
					+ "[CurrencyId]=@CurrencyId" + i + ","
					+ "[CurrencyName]=@CurrencyName" + i + ","
					+ "[DefaultCurrencyDecimals]=@DefaultCurrencyDecimals" + i + ","
					+ "[DefaultCurrencyId]=@DefaultCurrencyId" + i + ","
					+ "[DefaultCurrencyName]=@DefaultCurrencyName" + i + ","
					+ "[DefaultCurrencyRate]=@DefaultCurrencyRate" + i + ","
					+ "[DeliveryDate]=@DeliveryDate" + i + ","
					+ "[Description]=@Description" + i + ","
					+ "[Discount]=@Discount" + i + ","
					+ "[Einheit]=@Einheit" + i + ","
					+ "[Einzelpreis]=@Einzelpreis" + i + ","
					+ "[EMPB_Bestätigung]=@EMPB_Bestatigung" + i + ","
					+ "[EndeLagerBestand]=@EndeLagerBestand" + i + ","
					+ "[Erhalten]=@Erhalten" + i + ","
					+ "[erledigt_pos]=@erledigt_pos" + i + ","
					+ "[Gesamtpreis]=@Gesamtpreis" + i + ","
					+ "[In Bearbeitung]=@In_Bearbeitung" + i + ","
					+ "[InfoRahmennummer]=@InfoRahmennummer" + i + ","
					+ "[InternalContact]=@InternalContact" + i + ","
					+ "[InvoiceId]=@InvoiceId" + i + ","
					+ "[Kanban]=@Kanban" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[Liefertermin]=@Liefertermin" + i + ","
					+ "[LocationId]=@LocationId" + i + ","
					+ "[LocationName]=@LocationName" + i + ","
					+ "[Löschen]=@Loschen" + i + ","
					+ "[MhdDatumArtikel]=@MhdDatumArtikel" + i + ","
					+ "[OrderId]=@OrderId" + i + ","
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
					+ "[SupplierDeliveryDate]=@SupplierDeliveryDate" + i + ","
					+ "[SupplierOrderNumber]=@SupplierOrderNumber" + i + ","
					+ "[TotalCost]=@TotalCost" + i + ","
					+ "[TotalCostDefaultCurrency]=@TotalCostDefaultCurrency" + i + ","
					+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
					+ "[UnitPrice]=@UnitPrice" + i + ","
					+ "[UnitPriceDefaultCurrency]=@UnitPriceDefaultCurrency" + i + ","
					+ "[VAT]=@VAT" + i + ","
					+ "[WE Pos zu Bestellposition]=@WE_Pos_zu_Bestellposition" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AB_Nr_Lieferant" + i, item.AB_Nr_Lieferant == null ? (object)DBNull.Value : item.AB_Nr_Lieferant);
					sqlCommand.Parameters.AddWithValue("AccountId" + i, item.AccountId == null ? (object)DBNull.Value : item.AccountId);
					sqlCommand.Parameters.AddWithValue("AccountName" + i, item.AccountName == null ? (object)DBNull.Value : item.AccountName);
					sqlCommand.Parameters.AddWithValue("Aktuelle_Anzahl" + i, item.Aktuelle_Anzahl == null ? (object)DBNull.Value : item.Aktuelle_Anzahl);
					sqlCommand.Parameters.AddWithValue("AnfangLagerBestand" + i, item.AnfangLagerBestand == null ? (object)DBNull.Value : item.AnfangLagerBestand);
					sqlCommand.Parameters.AddWithValue("Anzahl" + i, item.Anzahl == null ? (object)DBNull.Value : item.Anzahl);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr == null ? (object)DBNull.Value : item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos" + i, item.Bemerkung_Pos == null ? (object)DBNull.Value : item.Bemerkung_Pos);
					sqlCommand.Parameters.AddWithValue("Bemerkung_Pos_ID" + i, item.Bemerkung_Pos_ID == null ? (object)DBNull.Value : item.Bemerkung_Pos_ID);
					sqlCommand.Parameters.AddWithValue("Bestatigter_Termin" + i, item.Bestatigter_Termin == null ? (object)DBNull.Value : item.Bestatigter_Termin);
					sqlCommand.Parameters.AddWithValue("Bestellnummer" + i, item.Bestellnummer == null ? (object)DBNull.Value : item.Bestellnummer);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_1" + i, item.Bezeichnung_1 == null ? (object)DBNull.Value : item.Bezeichnung_1);
					sqlCommand.Parameters.AddWithValue("Bezeichnung_2" + i, item.Bezeichnung_2 == null ? (object)DBNull.Value : item.Bezeichnung_2);
					sqlCommand.Parameters.AddWithValue("BP_zu_RBposition" + i, item.BP_zu_RBposition == null ? (object)DBNull.Value : item.BP_zu_RBposition);
					sqlCommand.Parameters.AddWithValue("COC_bestatigung" + i, item.COC_bestätigung == null ? (object)DBNull.Value : item.COC_bestätigung);
					sqlCommand.Parameters.AddWithValue("ConfirmationDate" + i, item.ConfirmationDate == null ? (object)DBNull.Value : item.ConfirmationDate);
					sqlCommand.Parameters.AddWithValue("CUPreis" + i, item.CUPreis == null ? (object)DBNull.Value : item.CUPreis);
					sqlCommand.Parameters.AddWithValue("CurrencyId" + i, item.CurrencyId == null ? (object)DBNull.Value : item.CurrencyId);
					sqlCommand.Parameters.AddWithValue("CurrencyName" + i, item.CurrencyName == null ? (object)DBNull.Value : item.CurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyDecimals" + i, item.DefaultCurrencyDecimals == null ? (object)DBNull.Value : item.DefaultCurrencyDecimals);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyId" + i, item.DefaultCurrencyId == null ? (object)DBNull.Value : item.DefaultCurrencyId);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyName" + i, item.DefaultCurrencyName == null ? (object)DBNull.Value : item.DefaultCurrencyName);
					sqlCommand.Parameters.AddWithValue("DefaultCurrencyRate" + i, item.DefaultCurrencyRate == null ? (object)DBNull.Value : item.DefaultCurrencyRate);
					sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Discount" + i, item.Discount == null ? (object)DBNull.Value : item.Discount);
					sqlCommand.Parameters.AddWithValue("Einheit" + i, item.Einheit == null ? (object)DBNull.Value : item.Einheit);
					sqlCommand.Parameters.AddWithValue("Einzelpreis" + i, item.Einzelpreis == null ? (object)DBNull.Value : item.Einzelpreis);
					sqlCommand.Parameters.AddWithValue("EMPB_Bestatigung" + i, item.EMPB_Bestätigung == null ? (object)DBNull.Value : item.EMPB_Bestätigung);
					sqlCommand.Parameters.AddWithValue("EndeLagerBestand" + i, item.EndeLagerBestand == null ? (object)DBNull.Value : item.EndeLagerBestand);
					sqlCommand.Parameters.AddWithValue("Erhalten" + i, item.Erhalten == null ? (object)DBNull.Value : item.Erhalten);
					sqlCommand.Parameters.AddWithValue("erledigt_pos" + i, item.erledigt_pos == null ? (object)DBNull.Value : item.erledigt_pos);
					sqlCommand.Parameters.AddWithValue("Gesamtpreis" + i, item.Gesamtpreis == null ? (object)DBNull.Value : item.Gesamtpreis);
					sqlCommand.Parameters.AddWithValue("In_Bearbeitung" + i, item.In_Bearbeitung == null ? (object)DBNull.Value : item.In_Bearbeitung);
					sqlCommand.Parameters.AddWithValue("InfoRahmennummer" + i, item.InfoRahmennummer == null ? (object)DBNull.Value : item.InfoRahmennummer);
					sqlCommand.Parameters.AddWithValue("InternalContact" + i, item.InternalContact == null ? (object)DBNull.Value : item.InternalContact);
					sqlCommand.Parameters.AddWithValue("InvoiceId" + i, item.InvoiceId == null ? (object)DBNull.Value : item.InvoiceId);
					sqlCommand.Parameters.AddWithValue("Kanban" + i, item.Kanban == null ? (object)DBNull.Value : item.Kanban);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id == null ? (object)DBNull.Value : item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("Liefertermin" + i, item.Liefertermin == null ? (object)DBNull.Value : item.Liefertermin);
					sqlCommand.Parameters.AddWithValue("LocationId" + i, item.LocationId == null ? (object)DBNull.Value : item.LocationId);
					sqlCommand.Parameters.AddWithValue("LocationName" + i, item.LocationName == null ? (object)DBNull.Value : item.LocationName);
					sqlCommand.Parameters.AddWithValue("Loschen" + i, item.Löschen == null ? (object)DBNull.Value : item.Löschen);
					sqlCommand.Parameters.AddWithValue("MhdDatumArtikel" + i, item.MhdDatumArtikel == null ? (object)DBNull.Value : item.MhdDatumArtikel);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
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
					sqlCommand.Parameters.AddWithValue("SupplierDeliveryDate" + i, item.SupplierDeliveryDate == null ? (object)DBNull.Value : item.SupplierDeliveryDate);
					sqlCommand.Parameters.AddWithValue("SupplierOrderNumber" + i, item.SupplierOrderNumber == null ? (object)DBNull.Value : item.SupplierOrderNumber);
					sqlCommand.Parameters.AddWithValue("TotalCost" + i, item.TotalCost == null ? (object)DBNull.Value : item.TotalCost);
					sqlCommand.Parameters.AddWithValue("TotalCostDefaultCurrency" + i, item.TotalCostDefaultCurrency == null ? (object)DBNull.Value : item.TotalCostDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("UnitPrice" + i, item.UnitPrice == null ? (object)DBNull.Value : item.UnitPrice);
					sqlCommand.Parameters.AddWithValue("UnitPriceDefaultCurrency" + i, item.UnitPriceDefaultCurrency == null ? (object)DBNull.Value : item.UnitPriceDefaultCurrency);
					sqlCommand.Parameters.AddWithValue("VAT" + i, item.VAT == null ? (object)DBNull.Value : item.VAT);
					sqlCommand.Parameters.AddWithValue("WE_Pos_zu_Bestellposition" + i, item.WE_Pos_zu_Bestellposition == null ? (object)DBNull.Value : item.WE_Pos_zu_Bestellposition);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__FNC_InvoiceItem] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [__FNC_InvoiceItem] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> GetByInvoiceId(int invoiceId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_InvoiceItem] WHERE [InvoiceId]=@invoiceId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("invoiceId", invoiceId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity>();
			}
		}


		#endregion
	}
}
