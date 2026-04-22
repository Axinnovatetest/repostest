using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class BestellnummernAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Bestellnummern] WHERE [Nr] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Bestellnummern] ([Angebot],[Angebot_Datum],[Artikelbezeichnung],[Artikelbezeichnung2],[Artikel-Nr],[Basispreis],[Bemerkungen],[Bestell-Nr],[Einkaufspreis],[Einkaufspreis gültig bis],[Einkaufspreis1],[Einkaufspreis1 gültig bis],[Einkaufspreis2],[Einkaufspreis2 gültig bis],[EK_EUR],[EK_total],[Fracht],[letzte_Aktualisierung],[Lieferanten-Nr],[Logistik],[Mindestbestellmenge],[Preiseinheit],[Prüftiefe_WE],[Rabatt],[Standardlieferant],[Umsatzsteuer],[Verpackungseinheit],[Warengruppe],[Wiederbeschaffungszeitraum],[Zoll],[Zusatz])  VALUES (@Angebot,@Angebot_Datum,@Artikelbezeichnung,@Artikelbezeichnung2,@Artikel_Nr,@Basispreis,@Bemerkungen,@Bestell_Nr,@Einkaufspreis,@Einkaufspreis_gültig_bis,@Einkaufspreis1,@Einkaufspreis1_gültig_bis,@Einkaufspreis2,@Einkaufspreis2_gültig_bis,@EK_EUR,@EK_total,@Fracht,@letzte_Aktualisierung,@Lieferanten_Nr,@Logistik,@Mindestbestellmenge,@Preiseinheit,@Prüftiefe_WE,@Rabatt,@Standardlieferant,@Umsatzsteuer,@Verpackungseinheit,@Warengruppe,@Wiederbeschaffungszeitraum,@Zoll,@Zusatz); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Angebot", item.Angebot == null ? (object)DBNull.Value : item.Angebot);
					sqlCommand.Parameters.AddWithValue("Angebot_Datum", item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2", item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Basispreis", item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Bestell_Nr", item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis", item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1", item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis", item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2", item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis", item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
					sqlCommand.Parameters.AddWithValue("EK_EUR", item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
					sqlCommand.Parameters.AddWithValue("EK_total", item.EK_total == null ? (object)DBNull.Value : item.EK_total);
					sqlCommand.Parameters.AddWithValue("Fracht", item.Fracht == null ? (object)DBNull.Value : item.Fracht);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Logistik", item.Logistik == null ? (object)DBNull.Value : item.Logistik);
					sqlCommand.Parameters.AddWithValue("Mindestbestellmenge", item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
					sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Prüftiefe_WE", item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
					sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Standardlieferant", item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Verpackungseinheit", item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
					sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum", item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
					sqlCommand.Parameters.AddWithValue("Zoll", item.Zoll == null ? (object)DBNull.Value : item.Zoll);
					sqlCommand.Parameters.AddWithValue("Zusatz", item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 33; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items)
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
						query += " INSERT INTO [Bestellnummern] ([Angebot],[Angebot_Datum],[Artikelbezeichnung],[Artikelbezeichnung2],[Artikel-Nr],[Basispreis],[Bemerkungen],[Bestell-Nr],[Einkaufspreis],[Einkaufspreis gültig bis],[Einkaufspreis1],[Einkaufspreis1 gültig bis],[Einkaufspreis2],[Einkaufspreis2 gültig bis],[EK_EUR],[EK_total],[Fracht],[letzte_Aktualisierung],[Lieferanten-Nr],[Logistik],[Mindestbestellmenge],[Preiseinheit],[Prüftiefe_WE],[Rabatt],[Standardlieferant],[Umsatzsteuer],[Verpackungseinheit],[Warengruppe],[Wiederbeschaffungszeitraum],[Zoll],[Zusatz]) VALUES ( "

							+ "@Angebot" + i + ","
							+ "@Angebot_Datum" + i + ","
							+ "@Artikelbezeichnung" + i + ","
							+ "@Artikelbezeichnung2" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Basispreis" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Bestell_Nr" + i + ","
							+ "@Einkaufspreis" + i + ","
							+ "@Einkaufspreis_gültig_bis" + i + ","
							+ "@Einkaufspreis1" + i + ","
							+ "@Einkaufspreis1_gültig_bis" + i + ","
							+ "@Einkaufspreis2" + i + ","
							+ "@Einkaufspreis2_gültig_bis" + i + ","
							+ "@EK_EUR" + i + ","
							+ "@EK_total" + i + ","
							+ "@Fracht" + i + ","
							+ "@letzte_Aktualisierung" + i + ","
							+ "@Lieferanten_Nr" + i + ","
							+ "@Logistik" + i + ","
							+ "@Mindestbestellmenge" + i + ","
							+ "@Preiseinheit" + i + ","
							+ "@Prüftiefe_WE" + i + ","
							+ "@Rabatt" + i + ","
							+ "@Standardlieferant" + i + ","
							+ "@Umsatzsteuer" + i + ","
							+ "@Verpackungseinheit" + i + ","
							+ "@Warengruppe" + i + ","
							+ "@Wiederbeschaffungszeitraum" + i + ","
							+ "@Zoll" + i + ","
							+ "@Zusatz" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Angebot" + i, item.Angebot == null ? (object)DBNull.Value : item.Angebot);
						sqlCommand.Parameters.AddWithValue("Angebot_Datum" + i, item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2" + i, item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Basispreis" + i, item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Bestell_Nr" + i, item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis" + i, item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis1" + i, item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis" + i, item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis2" + i, item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis" + i, item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
						sqlCommand.Parameters.AddWithValue("EK_EUR" + i, item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
						sqlCommand.Parameters.AddWithValue("EK_total" + i, item.EK_total == null ? (object)DBNull.Value : item.EK_total);
						sqlCommand.Parameters.AddWithValue("Fracht" + i, item.Fracht == null ? (object)DBNull.Value : item.Fracht);
						sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Logistik" + i, item.Logistik == null ? (object)DBNull.Value : item.Logistik);
						sqlCommand.Parameters.AddWithValue("Mindestbestellmenge" + i, item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Prüftiefe_WE" + i, item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Standardlieferant" + i, item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Verpackungseinheit" + i, item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum" + i, item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
						sqlCommand.Parameters.AddWithValue("Zoll" + i, item.Zoll == null ? (object)DBNull.Value : item.Zoll);
						sqlCommand.Parameters.AddWithValue("Zusatz" + i, item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Bestellnummern] SET [Angebot]=@Angebot, [Angebot_Datum]=@Angebot_Datum, [Artikelbezeichnung]=@Artikelbezeichnung, [Artikelbezeichnung2]=@Artikelbezeichnung2, [Artikel-Nr]=@Artikel_Nr, [Basispreis]=@Basispreis, [Bemerkungen]=@Bemerkungen, [Bestell-Nr]=@Bestell_Nr, [Einkaufspreis]=@Einkaufspreis, [Einkaufspreis gültig bis]=@Einkaufspreis_gültig_bis, [Einkaufspreis1]=@Einkaufspreis1, [Einkaufspreis1 gültig bis]=@Einkaufspreis1_gültig_bis, [Einkaufspreis2]=@Einkaufspreis2, [Einkaufspreis2 gültig bis]=@Einkaufspreis2_gültig_bis, [EK_EUR]=@EK_EUR, [EK_total]=@EK_total, [Fracht]=@Fracht, [letzte_Aktualisierung]=@letzte_Aktualisierung, [Lieferanten-Nr]=@Lieferanten_Nr, [Logistik]=@Logistik, [Mindestbestellmenge]=@Mindestbestellmenge, [Preiseinheit]=@Preiseinheit, [Prüftiefe_WE]=@Prüftiefe_WE, [Rabatt]=@Rabatt, [Standardlieferant]=@Standardlieferant, [Umsatzsteuer]=@Umsatzsteuer, [Verpackungseinheit]=@Verpackungseinheit, [Warengruppe]=@Warengruppe, [Wiederbeschaffungszeitraum]=@Wiederbeschaffungszeitraum, [Zoll]=@Zoll, [Zusatz]=@Zusatz WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Angebot", item.Angebot == null ? (object)DBNull.Value : item.Angebot);
				sqlCommand.Parameters.AddWithValue("Angebot_Datum", item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2", item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Basispreis", item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Bestell_Nr", item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis", item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis1", item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis", item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis2", item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis", item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
				sqlCommand.Parameters.AddWithValue("EK_EUR", item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
				sqlCommand.Parameters.AddWithValue("EK_total", item.EK_total == null ? (object)DBNull.Value : item.EK_total);
				sqlCommand.Parameters.AddWithValue("Fracht", item.Fracht == null ? (object)DBNull.Value : item.Fracht);
				sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
				sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
				sqlCommand.Parameters.AddWithValue("Logistik", item.Logistik == null ? (object)DBNull.Value : item.Logistik);
				sqlCommand.Parameters.AddWithValue("Mindestbestellmenge", item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Prüftiefe_WE", item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Standardlieferant", item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Verpackungseinheit", item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
				sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum", item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
				sqlCommand.Parameters.AddWithValue("Zoll", item.Zoll == null ? (object)DBNull.Value : item.Zoll);
				sqlCommand.Parameters.AddWithValue("Zusatz", item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		//Set standard Supplier
		public static int UpdateStandardSuplier(bool? Standardlieferant, int? ArtikeNr, int Nr)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Bestellnummern] SET [Standardlieferant]=@Standardlieferant WHERE [Artikel-Nr]=@ArtikeNr and [Nr]<>@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", Nr);
				sqlCommand.Parameters.AddWithValue("ArtikeNr", ArtikeNr == null ? (object)DBNull.Value : ArtikeNr);
				sqlCommand.Parameters.AddWithValue("Standardlieferant", Standardlieferant == null ? (object)DBNull.Value : Standardlieferant);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 33; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items)
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
						query += " UPDATE [Bestellnummern] SET "

							+ "[Angebot]=@Angebot" + i + ","
							+ "[Angebot_Datum]=@Angebot_Datum" + i + ","
							+ "[Artikelbezeichnung]=@Artikelbezeichnung" + i + ","
							+ "[Artikelbezeichnung2]=@Artikelbezeichnung2" + i + ","
							+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
							+ "[Basispreis]=@Basispreis" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Bestell-Nr]=@Bestell_Nr" + i + ","
							+ "[Einkaufspreis]=@Einkaufspreis" + i + ","
							+ "[Einkaufspreis gültig bis]=@Einkaufspreis_gültig_bis" + i + ","
							+ "[Einkaufspreis1]=@Einkaufspreis1" + i + ","
							+ "[Einkaufspreis1 gültig bis]=@Einkaufspreis1_gültig_bis" + i + ","
							+ "[Einkaufspreis2]=@Einkaufspreis2" + i + ","
							+ "[Einkaufspreis2 gültig bis]=@Einkaufspreis2_gültig_bis" + i + ","
							+ "[EK_EUR]=@EK_EUR" + i + ","
							+ "[EK_total]=@EK_total" + i + ","
							+ "[Fracht]=@Fracht" + i + ","
							+ "[letzte_Aktualisierung]=@letzte_Aktualisierung" + i + ","
							+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
							+ "[Logistik]=@Logistik" + i + ","
							+ "[Mindestbestellmenge]=@Mindestbestellmenge" + i + ","
							+ "[Preiseinheit]=@Preiseinheit" + i + ","
							+ "[Prüftiefe_WE]=@Prüftiefe_WE" + i + ","
							+ "[Rabatt]=@Rabatt" + i + ","
							+ "[Standardlieferant]=@Standardlieferant" + i + ","
							+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
							+ "[Verpackungseinheit]=@Verpackungseinheit" + i + ","
							+ "[Warengruppe]=@Warengruppe" + i + ","
							+ "[Wiederbeschaffungszeitraum]=@Wiederbeschaffungszeitraum" + i + ","
							+ "[Zoll]=@Zoll" + i + ","
							+ "[Zusatz]=@Zusatz" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Angebot" + i, item.Angebot == null ? (object)DBNull.Value : item.Angebot);
						sqlCommand.Parameters.AddWithValue("Angebot_Datum" + i, item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
						sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2" + i, item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Basispreis" + i, item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Bestell_Nr" + i, item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis" + i, item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis1" + i, item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis" + i, item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis2" + i, item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
						sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis" + i, item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
						sqlCommand.Parameters.AddWithValue("EK_EUR" + i, item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
						sqlCommand.Parameters.AddWithValue("EK_total" + i, item.EK_total == null ? (object)DBNull.Value : item.EK_total);
						sqlCommand.Parameters.AddWithValue("Fracht" + i, item.Fracht == null ? (object)DBNull.Value : item.Fracht);
						sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
						sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
						sqlCommand.Parameters.AddWithValue("Logistik" + i, item.Logistik == null ? (object)DBNull.Value : item.Logistik);
						sqlCommand.Parameters.AddWithValue("Mindestbestellmenge" + i, item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
						sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
						sqlCommand.Parameters.AddWithValue("Prüftiefe_WE" + i, item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
						sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
						sqlCommand.Parameters.AddWithValue("Standardlieferant" + i, item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
						sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
						sqlCommand.Parameters.AddWithValue("Verpackungseinheit" + i, item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
						sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
						sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum" + i, item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
						sqlCommand.Parameters.AddWithValue("Zoll" + i, item.Zoll == null ? (object)DBNull.Value : item.Zoll);
						sqlCommand.Parameters.AddWithValue("Zusatz" + i, item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [Bestellnummern] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [Bestellnummern] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Trans
		public static Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity GetWithTransaction(int nr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Bestellnummern] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetWithTransaction(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Bestellnummern]";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetWithTransaction(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), sqlConnection, sqlTransaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), sqlConnection, sqlTransaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> getWithTransaction(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [Bestellnummern] WHERE [Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			int response = int.MinValue;

			string query = "INSERT INTO [Bestellnummern] ([Angebot],[Angebot_Datum],[Artikelbezeichnung],[Artikelbezeichnung2],[Artikel-Nr],[Basispreis],[Bemerkungen],[Bestell-Nr],[Einkaufspreis],[Einkaufspreis gültig bis],[Einkaufspreis1],[Einkaufspreis1 gültig bis],[Einkaufspreis2],[Einkaufspreis2 gültig bis],[EK_EUR],[EK_total],[Fracht],[letzte_Aktualisierung],[Lieferanten-Nr],[Logistik],[Mindestbestellmenge],[Preiseinheit],[Prüftiefe_WE],[Rabatt],[Standardlieferant],[Umsatzsteuer],[Verpackungseinheit],[Warengruppe],[Wiederbeschaffungszeitraum],[Zoll],[Zusatz])  VALUES (@Angebot,@Angebot_Datum,@Artikelbezeichnung,@Artikelbezeichnung2,@Artikel_Nr,@Basispreis,@Bemerkungen,@Bestell_Nr,@Einkaufspreis,@Einkaufspreis_gültig_bis,@Einkaufspreis1,@Einkaufspreis1_gültig_bis,@Einkaufspreis2,@Einkaufspreis2_gültig_bis,@EK_EUR,@EK_total,@Fracht,@letzte_Aktualisierung,@Lieferanten_Nr,@Logistik,@Mindestbestellmenge,@Preiseinheit,@Prüftiefe_WE,@Rabatt,@Standardlieferant,@Umsatzsteuer,@Verpackungseinheit,@Warengruppe,@Wiederbeschaffungszeitraum,@Zoll,@Zusatz); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{
				sqlCommand.Parameters.AddWithValue("Angebot", item.Angebot == null ? (object)DBNull.Value : item.Angebot);
				sqlCommand.Parameters.AddWithValue("Angebot_Datum", item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
				sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2", item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Basispreis", item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Bestell_Nr", item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis", item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis1", item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis", item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis2", item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
				sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis", item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
				sqlCommand.Parameters.AddWithValue("EK_EUR", item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
				sqlCommand.Parameters.AddWithValue("EK_total", item.EK_total == null ? (object)DBNull.Value : item.EK_total);
				sqlCommand.Parameters.AddWithValue("Fracht", item.Fracht == null ? (object)DBNull.Value : item.Fracht);
				sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
				sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
				sqlCommand.Parameters.AddWithValue("Logistik", item.Logistik == null ? (object)DBNull.Value : item.Logistik);
				sqlCommand.Parameters.AddWithValue("Mindestbestellmenge", item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
				sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
				sqlCommand.Parameters.AddWithValue("Prüftiefe_WE", item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
				sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
				sqlCommand.Parameters.AddWithValue("Standardlieferant", item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
				sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
				sqlCommand.Parameters.AddWithValue("Verpackungseinheit", item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
				sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
				sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum", item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
				sqlCommand.Parameters.AddWithValue("Zoll", item.Zoll == null ? (object)DBNull.Value : item.Zoll);
				sqlCommand.Parameters.AddWithValue("Zusatz", item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 33; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insertWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), sqlConnection, sqlTransaction);
					}
					results += insertWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), sqlConnection, sqlTransaction);
				}
				return results;
			}

			return -1;
		}
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Bestellnummern] ([Angebot],[Angebot_Datum],[Artikelbezeichnung],[Artikelbezeichnung2],[Artikel-Nr],[Basispreis],[Bemerkungen],[Bestell-Nr],[Einkaufspreis],[Einkaufspreis gültig bis],[Einkaufspreis1],[Einkaufspreis1 gültig bis],[Einkaufspreis2],[Einkaufspreis2 gültig bis],[EK_EUR],[EK_total],[Fracht],[letzte_Aktualisierung],[Lieferanten-Nr],[Logistik],[Mindestbestellmenge],[Preiseinheit],[Prüftiefe_WE],[Rabatt],[Standardlieferant],[Umsatzsteuer],[Verpackungseinheit],[Warengruppe],[Wiederbeschaffungszeitraum],[Zoll],[Zusatz]) VALUES ( "

						+ "@Angebot" + i + ","
						+ "@Angebot_Datum" + i + ","
						+ "@Artikelbezeichnung" + i + ","
						+ "@Artikelbezeichnung2" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Basispreis" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@Bestell_Nr" + i + ","
						+ "@Einkaufspreis" + i + ","
						+ "@Einkaufspreis_gültig_bis" + i + ","
						+ "@Einkaufspreis1" + i + ","
						+ "@Einkaufspreis1_gültig_bis" + i + ","
						+ "@Einkaufspreis2" + i + ","
						+ "@Einkaufspreis2_gültig_bis" + i + ","
						+ "@EK_EUR" + i + ","
						+ "@EK_total" + i + ","
						+ "@Fracht" + i + ","
						+ "@letzte_Aktualisierung" + i + ","
						+ "@Lieferanten_Nr" + i + ","
						+ "@Logistik" + i + ","
						+ "@Mindestbestellmenge" + i + ","
						+ "@Preiseinheit" + i + ","
						+ "@Prüftiefe_WE" + i + ","
						+ "@Rabatt" + i + ","
						+ "@Standardlieferant" + i + ","
						+ "@Umsatzsteuer" + i + ","
						+ "@Verpackungseinheit" + i + ","
						+ "@Warengruppe" + i + ","
						+ "@Wiederbeschaffungszeitraum" + i + ","
						+ "@Zoll" + i + ","
						+ "@Zusatz" + i
						+ "); ";


					sqlCommand.Parameters.AddWithValue("Angebot" + i, item.Angebot == null ? (object)DBNull.Value : item.Angebot);
					sqlCommand.Parameters.AddWithValue("Angebot_Datum" + i, item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2" + i, item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Basispreis" + i, item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Bestell_Nr" + i, item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis" + i, item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1" + i, item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis" + i, item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2" + i, item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis" + i, item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
					sqlCommand.Parameters.AddWithValue("EK_EUR" + i, item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
					sqlCommand.Parameters.AddWithValue("EK_total" + i, item.EK_total == null ? (object)DBNull.Value : item.EK_total);
					sqlCommand.Parameters.AddWithValue("Fracht" + i, item.Fracht == null ? (object)DBNull.Value : item.Fracht);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Logistik" + i, item.Logistik == null ? (object)DBNull.Value : item.Logistik);
					sqlCommand.Parameters.AddWithValue("Mindestbestellmenge" + i, item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Prüftiefe_WE" + i, item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Standardlieferant" + i, item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Verpackungseinheit" + i, item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum" + i, item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
					sqlCommand.Parameters.AddWithValue("Zoll" + i, item.Zoll == null ? (object)DBNull.Value : item.Zoll);
					sqlCommand.Parameters.AddWithValue("Zusatz" + i, item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity item, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "UPDATE [Bestellnummern] SET [Angebot]=@Angebot, [Angebot_Datum]=@Angebot_Datum, [Artikelbezeichnung]=@Artikelbezeichnung, [Artikelbezeichnung2]=@Artikelbezeichnung2, [Artikel-Nr]=@Artikel_Nr, [Basispreis]=@Basispreis, [Bemerkungen]=@Bemerkungen, [Bestell-Nr]=@Bestell_Nr, [Einkaufspreis]=@Einkaufspreis, [Einkaufspreis gültig bis]=@Einkaufspreis_gültig_bis, [Einkaufspreis1]=@Einkaufspreis1, [Einkaufspreis1 gültig bis]=@Einkaufspreis1_gültig_bis, [Einkaufspreis2]=@Einkaufspreis2, [Einkaufspreis2 gültig bis]=@Einkaufspreis2_gültig_bis, [EK_EUR]=@EK_EUR, [EK_total]=@EK_total, [Fracht]=@Fracht, [letzte_Aktualisierung]=@letzte_Aktualisierung, [Lieferanten-Nr]=@Lieferanten_Nr, [Logistik]=@Logistik, [Mindestbestellmenge]=@Mindestbestellmenge, [Preiseinheit]=@Preiseinheit, [Prüftiefe_WE]=@Prüftiefe_WE, [Rabatt]=@Rabatt, [Standardlieferant]=@Standardlieferant, [Umsatzsteuer]=@Umsatzsteuer, [Verpackungseinheit]=@Verpackungseinheit, [Warengruppe]=@Warengruppe, [Wiederbeschaffungszeitraum]=@Wiederbeschaffungszeitraum, [Zoll]=@Zoll, [Zusatz]=@Zusatz WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Angebot", item.Angebot == null ? (object)DBNull.Value : item.Angebot);
			sqlCommand.Parameters.AddWithValue("Angebot_Datum", item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
			sqlCommand.Parameters.AddWithValue("Artikelbezeichnung", item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
			sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2", item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Basispreis", item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Bestell_Nr", item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis", item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis", item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis1", item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis", item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis2", item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
			sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis", item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
			sqlCommand.Parameters.AddWithValue("EK_EUR", item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
			sqlCommand.Parameters.AddWithValue("EK_total", item.EK_total == null ? (object)DBNull.Value : item.EK_total);
			sqlCommand.Parameters.AddWithValue("Fracht", item.Fracht == null ? (object)DBNull.Value : item.Fracht);
			sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung", item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
			sqlCommand.Parameters.AddWithValue("Lieferanten_Nr", item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
			sqlCommand.Parameters.AddWithValue("Logistik", item.Logistik == null ? (object)DBNull.Value : item.Logistik);
			sqlCommand.Parameters.AddWithValue("Mindestbestellmenge", item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
			sqlCommand.Parameters.AddWithValue("Preiseinheit", item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
			sqlCommand.Parameters.AddWithValue("Prüftiefe_WE", item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
			sqlCommand.Parameters.AddWithValue("Rabatt", item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
			sqlCommand.Parameters.AddWithValue("Standardlieferant", item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
			sqlCommand.Parameters.AddWithValue("Umsatzsteuer", item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
			sqlCommand.Parameters.AddWithValue("Verpackungseinheit", item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
			sqlCommand.Parameters.AddWithValue("Warengruppe", item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
			sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum", item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
			sqlCommand.Parameters.AddWithValue("Zoll", item.Zoll == null ? (object)DBNull.Value : item.Zoll);
			sqlCommand.Parameters.AddWithValue("Zusatz", item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 33; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += updateWithTransaction(items.GetRange(i * maxParamsNumber, maxParamsNumber), sqlConnection, sqlTransaction);
					}
					results += updateWithTransaction(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), sqlConnection, sqlTransaction);
				}

				return results;
			}

			return -1;
		}
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " UPDATE [Bestellnummern] SET "

						+ "[Angebot]=@Angebot" + i + ","
						+ "[Angebot_Datum]=@Angebot_Datum" + i + ","
						+ "[Artikelbezeichnung]=@Artikelbezeichnung" + i + ","
						+ "[Artikelbezeichnung2]=@Artikelbezeichnung2" + i + ","
						+ "[Artikel-Nr]=@Artikel_Nr" + i + ","
						+ "[Basispreis]=@Basispreis" + i + ","
						+ "[Bemerkungen]=@Bemerkungen" + i + ","
						+ "[Bestell-Nr]=@Bestell_Nr" + i + ","
						+ "[Einkaufspreis]=@Einkaufspreis" + i + ","
						+ "[Einkaufspreis gültig bis]=@Einkaufspreis_gültig_bis" + i + ","
						+ "[Einkaufspreis1]=@Einkaufspreis1" + i + ","
						+ "[Einkaufspreis1 gültig bis]=@Einkaufspreis1_gültig_bis" + i + ","
						+ "[Einkaufspreis2]=@Einkaufspreis2" + i + ","
						+ "[Einkaufspreis2 gültig bis]=@Einkaufspreis2_gültig_bis" + i + ","
						+ "[EK_EUR]=@EK_EUR" + i + ","
						+ "[EK_total]=@EK_total" + i + ","
						+ "[Fracht]=@Fracht" + i + ","
						+ "[letzte_Aktualisierung]=@letzte_Aktualisierung" + i + ","
						+ "[Lieferanten-Nr]=@Lieferanten_Nr" + i + ","
						+ "[Logistik]=@Logistik" + i + ","
						+ "[Mindestbestellmenge]=@Mindestbestellmenge" + i + ","
						+ "[Preiseinheit]=@Preiseinheit" + i + ","
						+ "[Prüftiefe_WE]=@Prüftiefe_WE" + i + ","
						+ "[Rabatt]=@Rabatt" + i + ","
						+ "[Standardlieferant]=@Standardlieferant" + i + ","
						+ "[Umsatzsteuer]=@Umsatzsteuer" + i + ","
						+ "[Verpackungseinheit]=@Verpackungseinheit" + i + ","
						+ "[Warengruppe]=@Warengruppe" + i + ","
						+ "[Wiederbeschaffungszeitraum]=@Wiederbeschaffungszeitraum" + i + ","
						+ "[Zoll]=@Zoll" + i + ","
						+ "[Zusatz]=@Zusatz" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Angebot" + i, item.Angebot == null ? (object)DBNull.Value : item.Angebot);
					sqlCommand.Parameters.AddWithValue("Angebot_Datum" + i, item.Angebot_Datum == null ? (object)DBNull.Value : item.Angebot_Datum);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung" + i, item.Artikelbezeichnung == null ? (object)DBNull.Value : item.Artikelbezeichnung);
					sqlCommand.Parameters.AddWithValue("Artikelbezeichnung2" + i, item.Artikelbezeichnung2 == null ? (object)DBNull.Value : item.Artikelbezeichnung2);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.ArtikelNr == null ? (object)DBNull.Value : item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Basispreis" + i, item.Basispreis == null ? (object)DBNull.Value : item.Basispreis);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Bestell_Nr" + i, item.Bestell_Nr == null ? (object)DBNull.Value : item.Bestell_Nr);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis" + i, item.Einkaufspreis == null ? (object)DBNull.Value : item.Einkaufspreis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis_gültig_bis" + i, item.Einkaufspreis_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1" + i, item.Einkaufspreis1 == null ? (object)DBNull.Value : item.Einkaufspreis1);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis1_gültig_bis" + i, item.Einkaufspreis1_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis1_gultig_bis);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2" + i, item.Einkaufspreis2 == null ? (object)DBNull.Value : item.Einkaufspreis2);
					sqlCommand.Parameters.AddWithValue("Einkaufspreis2_gültig_bis" + i, item.Einkaufspreis2_gultig_bis == null ? (object)DBNull.Value : item.Einkaufspreis2_gultig_bis);
					sqlCommand.Parameters.AddWithValue("EK_EUR" + i, item.EK_EUR == null ? (object)DBNull.Value : item.EK_EUR);
					sqlCommand.Parameters.AddWithValue("EK_total" + i, item.EK_total == null ? (object)DBNull.Value : item.EK_total);
					sqlCommand.Parameters.AddWithValue("Fracht" + i, item.Fracht == null ? (object)DBNull.Value : item.Fracht);
					sqlCommand.Parameters.AddWithValue("letzte_Aktualisierung" + i, item.letzte_Aktualisierung == null ? (object)DBNull.Value : item.letzte_Aktualisierung);
					sqlCommand.Parameters.AddWithValue("Lieferanten_Nr" + i, item.Lieferanten_Nr == null ? (object)DBNull.Value : item.Lieferanten_Nr);
					sqlCommand.Parameters.AddWithValue("Logistik" + i, item.Logistik == null ? (object)DBNull.Value : item.Logistik);
					sqlCommand.Parameters.AddWithValue("Mindestbestellmenge" + i, item.Mindestbestellmenge == null ? (object)DBNull.Value : item.Mindestbestellmenge);
					sqlCommand.Parameters.AddWithValue("Preiseinheit" + i, item.Preiseinheit == null ? (object)DBNull.Value : item.Preiseinheit);
					sqlCommand.Parameters.AddWithValue("Prüftiefe_WE" + i, item.Pruftiefe_WE == null ? (object)DBNull.Value : item.Pruftiefe_WE);
					sqlCommand.Parameters.AddWithValue("Rabatt" + i, item.Rabatt == null ? (object)DBNull.Value : item.Rabatt);
					sqlCommand.Parameters.AddWithValue("Standardlieferant" + i, item.Standardlieferant == null ? (object)DBNull.Value : item.Standardlieferant);
					sqlCommand.Parameters.AddWithValue("Umsatzsteuer" + i, item.Umsatzsteuer == null ? (object)DBNull.Value : item.Umsatzsteuer);
					sqlCommand.Parameters.AddWithValue("Verpackungseinheit" + i, item.Verpackungseinheit == null ? (object)DBNull.Value : item.Verpackungseinheit);
					sqlCommand.Parameters.AddWithValue("Warengruppe" + i, item.Warengruppe == null ? (object)DBNull.Value : item.Warengruppe);
					sqlCommand.Parameters.AddWithValue("Wiederbeschaffungszeitraum" + i, item.Wiederbeschaffungszeitraum == null ? (object)DBNull.Value : item.Wiederbeschaffungszeitraum);
					sqlCommand.Parameters.AddWithValue("Zoll" + i, item.Zoll == null ? (object)DBNull.Value : item.Zoll);
					sqlCommand.Parameters.AddWithValue("Zusatz" + i, item.Zusatz == null ? (object)DBNull.Value : item.Zusatz);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			string query = "DELETE FROM [Bestellnummern] WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("Nr", nr);

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}
		public static int DeleteWithTransaction(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = deleteWithTransaction(ids, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), sqlConnection, sqlTransaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), sqlConnection, sqlTransaction);
				}
			}
			return -1;
		}
		private static int deleteWithTransaction(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.Transaction = sqlTransaction;

				string queryIds = string.Empty;
				for(int i = 0; i < ids.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				string query = "DELETE FROM [Bestellnummern] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		#endregion Trans - 
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByArticle(int articleId, bool? isStandardSupplier = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId";
				if(isStandardSupplier.HasValue)
				{
					query += $" AND [Standardlieferant]={(isStandardSupplier.Value ? 1 : 0)}";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByArticle(int articleId, bool? isStandardSupplier, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId";
			if(isStandardSupplier.HasValue)
			{
				query += $" AND [Standardlieferant]={(isStandardSupplier.Value ? 1 : 0)}";
			}
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByArticleAndSupplier(int articleId, int supplierId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId and [Lieferanten-Nr]=@supplierId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("supplierId", supplierId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByArticleAndSupplier(int articleId, int supplierId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId and [Lieferanten-Nr]=@supplierId";
			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("articleId", articleId);
			sqlCommand.Parameters.AddWithValue("supplierId", supplierId);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByStandardSupplier(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId and [Standardlieferant]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByStandardSupplier(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr] IN ({string.Join(",", articleIds)}) and [Standardlieferant]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity> GetByArticleAndSupplierNr(int articleId, int SuppllierNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Bestellnummern] WHERE [Artikel-Nr]=@articleId and [Lieferanten-Nr]=@SuppllierNr";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("SuppllierNr", SuppllierNr);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
			}
		}
		#endregion
	}
}
