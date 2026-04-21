using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class AdressenAccess2
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 Get(int nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Adressen] WHERE [Nr]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Adressen]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Adressen] WHERE [Nr] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Adressen] ([Abteilung],[Adresstyp],[Anrede],[Auswahl],[Bemerkung],[Bemerkungen],[Briefanrede],[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],[DUNS],[EDI-Aktiv],[eMail],[erfasst],[Fax],[Freitag (Anliefertag)],[Funktion],[Kundennummer],[Land],[Lieferantennummer],[Mittwoch (Anliefertag)],[Montag (Anliefertag)],[Name1],[Name2],[Name3],[Ort],[PendingValidation],[Personalnummer],[PLZ_Postfach],[PLZ_Straße],[Postfach],[Postfach bevorzugt],[Sortierbegriff],[sperren],[Straße],[stufe],[Telefon],[Titel],[von],[Vorname],[WWW]) OUTPUT INSERTED.[Nr] VALUES (@Abteilung,@Adresstyp,@Anrede,@Auswahl,@Bemerkung,@Bemerkungen,@Briefanrede,@Dienstag__Anliefertag_,@Donnerstag__Anliefertag_,@DUNS,@EDI_Aktiv,@eMail,@erfasst,@Fax,@Freitag__Anliefertag_,@Funktion,@Kundennummer,@Land,@Lieferantennummer,@Mittwoch__Anliefertag_,@Montag__Anliefertag_,@Name1,@Name2,@Name3,@Ort,@PendingValidation,@Personalnummer,@PLZ_Postfach,@PLZ_Strasse,@Postfach,@Postfach_bevorzugt,@Sortierbegriff,@sperren,@Strasse,@stufe,@Telefon,@Titel,@von,@Vorname,@WWW); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_", item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_", item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
					sqlCommand.Parameters.AddWithValue("EDI_Aktiv", item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
					sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
					sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_", item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
					sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
					sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_", item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_", item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PendingValidation", item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
					sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
					sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
					sqlCommand.Parameters.AddWithValue("PLZ_Strasse", item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
					sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
					sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
					sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
					sqlCommand.Parameters.AddWithValue("sperren", item.sperren == null ? (object)DBNull.Value : item.sperren);
					sqlCommand.Parameters.AddWithValue("Strasse", item.Strasse == null ? (object)DBNull.Value : item.Strasse);
					sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
					sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
					sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
					sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items)
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
						query += " INSERT INTO [Adressen] ([Abteilung],[Adresstyp],[Anrede],[Auswahl],[Bemerkung],[Bemerkungen],[Briefanrede],[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],[DUNS],[EDI-Aktiv],[eMail],[erfasst],[Fax],[Freitag (Anliefertag)],[Funktion],[Kundennummer],[Land],[Lieferantennummer],[Mittwoch (Anliefertag)],[Montag (Anliefertag)],[Name1],[Name2],[Name3],[Ort],[PendingValidation],[Personalnummer],[PLZ_Postfach],[PLZ_Straße],[Postfach],[Postfach bevorzugt],[Sortierbegriff],[sperren],[Straße],[stufe],[Telefon],[Titel],[von],[Vorname],[WWW]) VALUES ( "

							+ "@Abteilung" + i + ","
							+ "@Adresstyp" + i + ","
							+ "@Anrede" + i + ","
							+ "@Auswahl" + i + ","
							+ "@Bemerkung" + i + ","
							+ "@Bemerkungen" + i + ","
							+ "@Briefanrede" + i + ","
							+ "@Dienstag__Anliefertag_" + i + ","
							+ "@Donnerstag__Anliefertag_" + i + ","
							+ "@DUNS" + i + ","
							+ "@EDI_Aktiv" + i + ","
							+ "@eMail" + i + ","
							+ "@erfasst" + i + ","
							+ "@Fax" + i + ","
							+ "@Freitag__Anliefertag_" + i + ","
							+ "@Funktion" + i + ","
							+ "@Kundennummer" + i + ","
							+ "@Land" + i + ","
							+ "@Lieferantennummer" + i + ","
							+ "@Mittwoch__Anliefertag_" + i + ","
							+ "@Montag__Anliefertag_" + i + ","
							+ "@Name1" + i + ","
							+ "@Name2" + i + ","
							+ "@Name3" + i + ","
							+ "@Ort" + i + ","
							+ "@PendingValidation" + i + ","
							+ "@Personalnummer" + i + ","
							+ "@PLZ_Postfach" + i + ","
							+ "@PLZ_Strasse" + i + ","
							+ "@Postfach" + i + ","
							+ "@Postfach_bevorzugt" + i + ","
							+ "@Sortierbegriff" + i + ","
							+ "@sperren" + i + ","
							+ "@Strasse" + i + ","
							+ "@stufe" + i + ","
							+ "@Telefon" + i + ","
							+ "@Titel" + i + ","
							+ "@von" + i + ","
							+ "@Vorname" + i + ","
							+ "@WWW" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Adresstyp" + i, item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_" + i, item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_" + i, item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("EDI_Aktiv" + i, item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
						sqlCommand.Parameters.AddWithValue("eMail" + i, item.eMail == null ? (object)DBNull.Value : item.eMail);
						sqlCommand.Parameters.AddWithValue("erfasst" + i, item.erfasst == null ? (object)DBNull.Value : item.erfasst);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_" + i, item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Funktion" + i, item.Funktion == null ? (object)DBNull.Value : item.Funktion);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
						sqlCommand.Parameters.AddWithValue("Lieferantennummer" + i, item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
						sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_" + i, item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_" + i, item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
						sqlCommand.Parameters.AddWithValue("PendingValidation" + i, item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
						sqlCommand.Parameters.AddWithValue("Personalnummer" + i, item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
						sqlCommand.Parameters.AddWithValue("PLZ_Postfach" + i, item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
						sqlCommand.Parameters.AddWithValue("PLZ_Strasse" + i, item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
						sqlCommand.Parameters.AddWithValue("Postfach" + i, item.Postfach == null ? (object)DBNull.Value : item.Postfach);
						sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt" + i, item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
						sqlCommand.Parameters.AddWithValue("Sortierbegriff" + i, item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
						sqlCommand.Parameters.AddWithValue("sperren" + i, item.sperren == null ? (object)DBNull.Value : item.sperren);
						sqlCommand.Parameters.AddWithValue("Strasse" + i, item.Strasse == null ? (object)DBNull.Value : item.Strasse);
						sqlCommand.Parameters.AddWithValue("stufe" + i, item.stufe == null ? (object)DBNull.Value : item.stufe);
						sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
						sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
						sqlCommand.Parameters.AddWithValue("von" + i, item.von == null ? (object)DBNull.Value : item.von);
						sqlCommand.Parameters.AddWithValue("Vorname" + i, item.Vorname == null ? (object)DBNull.Value : item.Vorname);
						sqlCommand.Parameters.AddWithValue("WWW" + i, item.WWW == null ? (object)DBNull.Value : item.WWW);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Adressen] SET [Abteilung]=@Abteilung, [Adresstyp]=@Adresstyp, [Anrede]=@Anrede, [Auswahl]=@Auswahl, [Bemerkung]=@Bemerkung, [Bemerkungen]=@Bemerkungen, [Briefanrede]=@Briefanrede, [Dienstag (Anliefertag)]=@Dienstag__Anliefertag_, [Donnerstag (Anliefertag)]=@Donnerstag__Anliefertag_, [DUNS]=@DUNS, [EDI-Aktiv]=@EDI_Aktiv, [eMail]=@eMail, [erfasst]=@erfasst, [Fax]=@Fax, [Freitag (Anliefertag)]=@Freitag__Anliefertag_, [Funktion]=@Funktion, [Kundennummer]=@Kundennummer, [Land]=@Land, [Lieferantennummer]=@Lieferantennummer, [Mittwoch (Anliefertag)]=@Mittwoch__Anliefertag_, [Montag (Anliefertag)]=@Montag__Anliefertag_, [Name1]=@Name1, [Name2]=@Name2, [Name3]=@Name3, [Ort]=@Ort, [PendingValidation]=@PendingValidation, [Personalnummer]=@Personalnummer, [PLZ_Postfach]=@PLZ_Postfach, [PLZ_Straße]=@PLZ_Strasse, [Postfach]=@Postfach, [Postfach bevorzugt]=@Postfach_bevorzugt, [Sortierbegriff]=@Sortierbegriff, [sperren]=@sperren, [Straße]=@Strasse, [stufe]=@stufe, [Telefon]=@Telefon, [Titel]=@Titel, [von]=@von, [Vorname]=@Vorname, [WWW]=@WWW WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
				sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_", item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
				sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_", item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
				sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
				sqlCommand.Parameters.AddWithValue("EDI_Aktiv", item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
				sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
				sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_", item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
				sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
				sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
				sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_", item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
				sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_", item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
				sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("PendingValidation", item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
				sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
				sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
				sqlCommand.Parameters.AddWithValue("PLZ_Strasse", item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
				sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
				sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
				sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
				sqlCommand.Parameters.AddWithValue("sperren", item.sperren == null ? (object)DBNull.Value : item.sperren);
				sqlCommand.Parameters.AddWithValue("Strasse", item.Strasse == null ? (object)DBNull.Value : item.Strasse);
				sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
				sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
				sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
				sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
				sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items)
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
						query += " UPDATE [Adressen] SET "

							+ "[Abteilung]=@Abteilung" + i + ","
							+ "[Adresstyp]=@Adresstyp" + i + ","
							+ "[Anrede]=@Anrede" + i + ","
							+ "[Auswahl]=@Auswahl" + i + ","
							+ "[Bemerkung]=@Bemerkung" + i + ","
							+ "[Bemerkungen]=@Bemerkungen" + i + ","
							+ "[Briefanrede]=@Briefanrede" + i + ","
							+ "[Dienstag (Anliefertag)]=@Dienstag__Anliefertag_" + i + ","
							+ "[Donnerstag (Anliefertag)]=@Donnerstag__Anliefertag_" + i + ","
							+ "[DUNS]=@DUNS" + i + ","
							+ "[EDI-Aktiv]=@EDI_Aktiv" + i + ","
							+ "[eMail]=@eMail" + i + ","
							+ "[erfasst]=@erfasst" + i + ","
							+ "[Fax]=@Fax" + i + ","
							+ "[Freitag (Anliefertag)]=@Freitag__Anliefertag_" + i + ","
							+ "[Funktion]=@Funktion" + i + ","
							+ "[Kundennummer]=@Kundennummer" + i + ","
							+ "[Land]=@Land" + i + ","
							+ "[Lieferantennummer]=@Lieferantennummer" + i + ","
							+ "[Mittwoch (Anliefertag)]=@Mittwoch__Anliefertag_" + i + ","
							+ "[Montag (Anliefertag)]=@Montag__Anliefertag_" + i + ","
							+ "[Name1]=@Name1" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[Name3]=@Name3" + i + ","
							+ "[Ort]=@Ort" + i + ","
							+ "[PendingValidation]=@PendingValidation" + i + ","
							+ "[Personalnummer]=@Personalnummer" + i + ","
							+ "[PLZ_Postfach]=@PLZ_Postfach" + i + ","
							+ "[PLZ_Straße]=@PLZ_Strasse" + i + ","
							+ "[Postfach]=@Postfach" + i + ","
							+ "[Postfach bevorzugt]=@Postfach_bevorzugt" + i + ","
							+ "[Sortierbegriff]=@Sortierbegriff" + i + ","
							+ "[sperren]=@sperren" + i + ","
							+ "[Straße]=@Strasse" + i + ","
							+ "[stufe]=@stufe" + i + ","
							+ "[Telefon]=@Telefon" + i + ","
							+ "[Titel]=@Titel" + i + ","
							+ "[von]=@von" + i + ","
							+ "[Vorname]=@Vorname" + i + ","
							+ "[WWW]=@WWW" + i + " WHERE [Nr]=@Nr" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
						sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
						sqlCommand.Parameters.AddWithValue("Adresstyp" + i, item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
						sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
						sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
						sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
						sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
						sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
						sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_" + i, item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_" + i, item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
						sqlCommand.Parameters.AddWithValue("EDI_Aktiv" + i, item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
						sqlCommand.Parameters.AddWithValue("eMail" + i, item.eMail == null ? (object)DBNull.Value : item.eMail);
						sqlCommand.Parameters.AddWithValue("erfasst" + i, item.erfasst == null ? (object)DBNull.Value : item.erfasst);
						sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
						sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_" + i, item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Funktion" + i, item.Funktion == null ? (object)DBNull.Value : item.Funktion);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
						sqlCommand.Parameters.AddWithValue("Lieferantennummer" + i, item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
						sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_" + i, item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_" + i, item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
						sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
						sqlCommand.Parameters.AddWithValue("PendingValidation" + i, item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
						sqlCommand.Parameters.AddWithValue("Personalnummer" + i, item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
						sqlCommand.Parameters.AddWithValue("PLZ_Postfach" + i, item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
						sqlCommand.Parameters.AddWithValue("PLZ_Strasse" + i, item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
						sqlCommand.Parameters.AddWithValue("Postfach" + i, item.Postfach == null ? (object)DBNull.Value : item.Postfach);
						sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt" + i, item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
						sqlCommand.Parameters.AddWithValue("Sortierbegriff" + i, item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
						sqlCommand.Parameters.AddWithValue("sperren" + i, item.sperren == null ? (object)DBNull.Value : item.sperren);
						sqlCommand.Parameters.AddWithValue("Strasse" + i, item.Strasse == null ? (object)DBNull.Value : item.Strasse);
						sqlCommand.Parameters.AddWithValue("stufe" + i, item.stufe == null ? (object)DBNull.Value : item.stufe);
						sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
						sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
						sqlCommand.Parameters.AddWithValue("von" + i, item.von == null ? (object)DBNull.Value : item.von);
						sqlCommand.Parameters.AddWithValue("Vorname" + i, item.Vorname == null ? (object)DBNull.Value : item.Vorname);
						sqlCommand.Parameters.AddWithValue("WWW" + i, item.WWW == null ? (object)DBNull.Value : item.WWW);
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
				string query = "DELETE FROM [Adressen] WHERE [Nr]=@Nr";
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

					string query = "DELETE FROM [Adressen] WHERE [Nr] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 GetWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Adressen] WHERE [Nr]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", nr);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Adressen]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Adressen] WHERE [Nr] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [Adressen] ([Abteilung],[Adresstyp],[Anrede],[Auswahl],[Bemerkung],[Bemerkungen],[Briefanrede],[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],[DUNS],[EDI-Aktiv],[eMail],[erfasst],[Fax],[Freitag (Anliefertag)],[Funktion],[Kundennummer],[Land],[Lieferantennummer],[Mittwoch (Anliefertag)],[Montag (Anliefertag)],[Name1],[Name2],[Name3],[Ort],[PendingValidation],[Personalnummer],[PLZ_Postfach],[PLZ_Straße],[Postfach],[Postfach bevorzugt],[Sortierbegriff],[sperren],[Straße],[stufe],[Telefon],[Titel],[von],[Vorname],[WWW]) OUTPUT INSERTED.[Nr] VALUES (@Abteilung,@Adresstyp,@Anrede,@Auswahl,@Bemerkung,@Bemerkungen,@Briefanrede,@Dienstag__Anliefertag_,@Donnerstag__Anliefertag_,@DUNS,@EDI_Aktiv,@eMail,@erfasst,@Fax,@Freitag__Anliefertag_,@Funktion,@Kundennummer,@Land,@Lieferantennummer,@Mittwoch__Anliefertag_,@Montag__Anliefertag_,@Name1,@Name2,@Name3,@Ort,@PendingValidation,@Personalnummer,@PLZ_Postfach,@PLZ_Strasse,@Postfach,@Postfach_bevorzugt,@Sortierbegriff,@sperren,@Strasse,@stufe,@Telefon,@Titel,@von,@Vorname,@WWW); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_", item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_", item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
			sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
			sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_", item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_", item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_", item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("PendingValidation", item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
			sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Strasse", item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
			sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", item.sperren == null ? (object)DBNull.Value : item.sperren);
			sqlCommand.Parameters.AddWithValue("Strasse", item.Strasse == null ? (object)DBNull.Value : item.Strasse);
			sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
			sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
			sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Adressen] ([Abteilung],[Adresstyp],[Anrede],[Auswahl],[Bemerkung],[Bemerkungen],[Briefanrede],[Dienstag (Anliefertag)],[Donnerstag (Anliefertag)],[DUNS],[EDI-Aktiv],[eMail],[erfasst],[Fax],[Freitag (Anliefertag)],[Funktion],[Kundennummer],[Land],[Lieferantennummer],[Mittwoch (Anliefertag)],[Montag (Anliefertag)],[Name1],[Name2],[Name3],[Ort],[PendingValidation],[Personalnummer],[PLZ_Postfach],[PLZ_Straße],[Postfach],[Postfach bevorzugt],[Sortierbegriff],[sperren],[Straße],[stufe],[Telefon],[Titel],[von],[Vorname],[WWW]) VALUES ( "

						+ "@Abteilung" + i + ","
						+ "@Adresstyp" + i + ","
						+ "@Anrede" + i + ","
						+ "@Auswahl" + i + ","
						+ "@Bemerkung" + i + ","
						+ "@Bemerkungen" + i + ","
						+ "@Briefanrede" + i + ","
						+ "@Dienstag__Anliefertag_" + i + ","
						+ "@Donnerstag__Anliefertag_" + i + ","
						+ "@DUNS" + i + ","
						+ "@EDI_Aktiv" + i + ","
						+ "@eMail" + i + ","
						+ "@erfasst" + i + ","
						+ "@Fax" + i + ","
						+ "@Freitag__Anliefertag_" + i + ","
						+ "@Funktion" + i + ","
						+ "@Kundennummer" + i + ","
						+ "@Land" + i + ","
						+ "@Lieferantennummer" + i + ","
						+ "@Mittwoch__Anliefertag_" + i + ","
						+ "@Montag__Anliefertag_" + i + ","
						+ "@Name1" + i + ","
						+ "@Name2" + i + ","
						+ "@Name3" + i + ","
						+ "@Ort" + i + ","
						+ "@PendingValidation" + i + ","
						+ "@Personalnummer" + i + ","
						+ "@PLZ_Postfach" + i + ","
						+ "@PLZ_Strasse" + i + ","
						+ "@Postfach" + i + ","
						+ "@Postfach_bevorzugt" + i + ","
						+ "@Sortierbegriff" + i + ","
						+ "@sperren" + i + ","
						+ "@Strasse" + i + ","
						+ "@stufe" + i + ","
						+ "@Telefon" + i + ","
						+ "@Titel" + i + ","
						+ "@von" + i + ","
						+ "@Vorname" + i + ","
						+ "@WWW" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Adresstyp" + i, item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_" + i, item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_" + i, item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
					sqlCommand.Parameters.AddWithValue("EDI_Aktiv" + i, item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
					sqlCommand.Parameters.AddWithValue("eMail" + i, item.eMail == null ? (object)DBNull.Value : item.eMail);
					sqlCommand.Parameters.AddWithValue("erfasst" + i, item.erfasst == null ? (object)DBNull.Value : item.erfasst);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_" + i, item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Funktion" + i, item.Funktion == null ? (object)DBNull.Value : item.Funktion);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Lieferantennummer" + i, item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
					sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_" + i, item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_" + i, item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PendingValidation" + i, item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
					sqlCommand.Parameters.AddWithValue("Personalnummer" + i, item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
					sqlCommand.Parameters.AddWithValue("PLZ_Postfach" + i, item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
					sqlCommand.Parameters.AddWithValue("PLZ_Strasse" + i, item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
					sqlCommand.Parameters.AddWithValue("Postfach" + i, item.Postfach == null ? (object)DBNull.Value : item.Postfach);
					sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt" + i, item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
					sqlCommand.Parameters.AddWithValue("Sortierbegriff" + i, item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
					sqlCommand.Parameters.AddWithValue("sperren" + i, item.sperren == null ? (object)DBNull.Value : item.sperren);
					sqlCommand.Parameters.AddWithValue("Strasse" + i, item.Strasse == null ? (object)DBNull.Value : item.Strasse);
					sqlCommand.Parameters.AddWithValue("stufe" + i, item.stufe == null ? (object)DBNull.Value : item.stufe);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("von" + i, item.von == null ? (object)DBNull.Value : item.von);
					sqlCommand.Parameters.AddWithValue("Vorname" + i, item.Vorname == null ? (object)DBNull.Value : item.Vorname);
					sqlCommand.Parameters.AddWithValue("WWW" + i, item.WWW == null ? (object)DBNull.Value : item.WWW);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2 item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Adressen] SET [Abteilung]=@Abteilung, [Adresstyp]=@Adresstyp, [Anrede]=@Anrede, [Auswahl]=@Auswahl, [Bemerkung]=@Bemerkung, [Bemerkungen]=@Bemerkungen, [Briefanrede]=@Briefanrede, [Dienstag (Anliefertag)]=@Dienstag__Anliefertag_, [Donnerstag (Anliefertag)]=@Donnerstag__Anliefertag_, [DUNS]=@DUNS, [EDI-Aktiv]=@EDI_Aktiv, [eMail]=@eMail, [erfasst]=@erfasst, [Fax]=@Fax, [Freitag (Anliefertag)]=@Freitag__Anliefertag_, [Funktion]=@Funktion, [Kundennummer]=@Kundennummer, [Land]=@Land, [Lieferantennummer]=@Lieferantennummer, [Mittwoch (Anliefertag)]=@Mittwoch__Anliefertag_, [Montag (Anliefertag)]=@Montag__Anliefertag_, [Name1]=@Name1, [Name2]=@Name2, [Name3]=@Name3, [Ort]=@Ort, [PendingValidation]=@PendingValidation, [Personalnummer]=@Personalnummer, [PLZ_Postfach]=@PLZ_Postfach, [PLZ_Straße]=@PLZ_Strasse, [Postfach]=@Postfach, [Postfach bevorzugt]=@Postfach_bevorzugt, [Sortierbegriff]=@Sortierbegriff, [sperren]=@sperren, [Straße]=@Strasse, [stufe]=@stufe, [Telefon]=@Telefon, [Titel]=@Titel, [von]=@von, [Vorname]=@Vorname, [WWW]=@WWW WHERE [Nr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Nr", item.Nr);
			sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
			sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
			sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
			sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
			sqlCommand.Parameters.AddWithValue("Bemerkung", item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
			sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
			sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
			sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_", item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_", item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("DUNS", item.DUNS == null ? (object)DBNull.Value : item.DUNS);
			sqlCommand.Parameters.AddWithValue("EDI_Aktiv", item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
			sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
			sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
			sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
			sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_", item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
			sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
			sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_", item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_", item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
			sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
			sqlCommand.Parameters.AddWithValue("PendingValidation", item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
			sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
			sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
			sqlCommand.Parameters.AddWithValue("PLZ_Strasse", item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
			sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
			sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
			sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
			sqlCommand.Parameters.AddWithValue("sperren", item.sperren == null ? (object)DBNull.Value : item.sperren);
			sqlCommand.Parameters.AddWithValue("Strasse", item.Strasse == null ? (object)DBNull.Value : item.Strasse);
			sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
			sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
			sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
			sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
			sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
			sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity2> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Adressen] SET "

					+ "[Abteilung]=@Abteilung" + i + ","
					+ "[Adresstyp]=@Adresstyp" + i + ","
					+ "[Anrede]=@Anrede" + i + ","
					+ "[Auswahl]=@Auswahl" + i + ","
					+ "[Bemerkung]=@Bemerkung" + i + ","
					+ "[Bemerkungen]=@Bemerkungen" + i + ","
					+ "[Briefanrede]=@Briefanrede" + i + ","
					+ "[Dienstag (Anliefertag)]=@Dienstag__Anliefertag_" + i + ","
					+ "[Donnerstag (Anliefertag)]=@Donnerstag__Anliefertag_" + i + ","
					+ "[DUNS]=@DUNS" + i + ","
					+ "[EDI-Aktiv]=@EDI_Aktiv" + i + ","
					+ "[eMail]=@eMail" + i + ","
					+ "[erfasst]=@erfasst" + i + ","
					+ "[Fax]=@Fax" + i + ","
					+ "[Freitag (Anliefertag)]=@Freitag__Anliefertag_" + i + ","
					+ "[Funktion]=@Funktion" + i + ","
					+ "[Kundennummer]=@Kundennummer" + i + ","
					+ "[Land]=@Land" + i + ","
					+ "[Lieferantennummer]=@Lieferantennummer" + i + ","
					+ "[Mittwoch (Anliefertag)]=@Mittwoch__Anliefertag_" + i + ","
					+ "[Montag (Anliefertag)]=@Montag__Anliefertag_" + i + ","
					+ "[Name1]=@Name1" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[Name3]=@Name3" + i + ","
					+ "[Ort]=@Ort" + i + ","
					+ "[PendingValidation]=@PendingValidation" + i + ","
					+ "[Personalnummer]=@Personalnummer" + i + ","
					+ "[PLZ_Postfach]=@PLZ_Postfach" + i + ","
					+ "[PLZ_Straße]=@PLZ_Strasse" + i + ","
					+ "[Postfach]=@Postfach" + i + ","
					+ "[Postfach bevorzugt]=@Postfach_bevorzugt" + i + ","
					+ "[Sortierbegriff]=@Sortierbegriff" + i + ","
					+ "[sperren]=@sperren" + i + ","
					+ "[Straße]=@Strasse" + i + ","
					+ "[stufe]=@stufe" + i + ","
					+ "[Telefon]=@Telefon" + i + ","
					+ "[Titel]=@Titel" + i + ","
					+ "[von]=@von" + i + ","
					+ "[Vorname]=@Vorname" + i + ","
					+ "[WWW]=@WWW" + i + " WHERE [Nr]=@Nr" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr);
					sqlCommand.Parameters.AddWithValue("Abteilung" + i, item.Abteilung == null ? (object)DBNull.Value : item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Adresstyp" + i, item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
					sqlCommand.Parameters.AddWithValue("Anrede" + i, item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Auswahl" + i, item.Auswahl == null ? (object)DBNull.Value : item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkung" + i, item.Bemerkung == null ? (object)DBNull.Value : item.Bemerkung);
					sqlCommand.Parameters.AddWithValue("Bemerkungen" + i, item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Briefanrede" + i, item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("Dienstag__Anliefertag_" + i, item.Dienstag__Anliefertag_ == null ? (object)DBNull.Value : item.Dienstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Donnerstag__Anliefertag_" + i, item.Donnerstag__Anliefertag_ == null ? (object)DBNull.Value : item.Donnerstag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("DUNS" + i, item.DUNS == null ? (object)DBNull.Value : item.DUNS);
					sqlCommand.Parameters.AddWithValue("EDI_Aktiv" + i, item.EDI_Aktiv == null ? (object)DBNull.Value : item.EDI_Aktiv);
					sqlCommand.Parameters.AddWithValue("eMail" + i, item.eMail == null ? (object)DBNull.Value : item.eMail);
					sqlCommand.Parameters.AddWithValue("erfasst" + i, item.erfasst == null ? (object)DBNull.Value : item.erfasst);
					sqlCommand.Parameters.AddWithValue("Fax" + i, item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("Freitag__Anliefertag_" + i, item.Freitag__Anliefertag_ == null ? (object)DBNull.Value : item.Freitag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Funktion" + i, item.Funktion == null ? (object)DBNull.Value : item.Funktion);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Land" + i, item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Lieferantennummer" + i, item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
					sqlCommand.Parameters.AddWithValue("Mittwoch__Anliefertag_" + i, item.Mittwoch__Anliefertag_ == null ? (object)DBNull.Value : item.Mittwoch__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Montag__Anliefertag_" + i, item.Montag__Anliefertag_ == null ? (object)DBNull.Value : item.Montag__Anliefertag_);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3" + i, item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Ort" + i, item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("PendingValidation" + i, item.PendingValidation == null ? (object)DBNull.Value : item.PendingValidation);
					sqlCommand.Parameters.AddWithValue("Personalnummer" + i, item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
					sqlCommand.Parameters.AddWithValue("PLZ_Postfach" + i, item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
					sqlCommand.Parameters.AddWithValue("PLZ_Strasse" + i, item.PLZ_Strasse == null ? (object)DBNull.Value : item.PLZ_Strasse);
					sqlCommand.Parameters.AddWithValue("Postfach" + i, item.Postfach == null ? (object)DBNull.Value : item.Postfach);
					sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt" + i, item.Postfach_bevorzugt == null ? (object)DBNull.Value : item.Postfach_bevorzugt);
					sqlCommand.Parameters.AddWithValue("Sortierbegriff" + i, item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
					sqlCommand.Parameters.AddWithValue("sperren" + i, item.sperren == null ? (object)DBNull.Value : item.sperren);
					sqlCommand.Parameters.AddWithValue("Strasse" + i, item.Strasse == null ? (object)DBNull.Value : item.Strasse);
					sqlCommand.Parameters.AddWithValue("stufe" + i, item.stufe == null ? (object)DBNull.Value : item.stufe);
					sqlCommand.Parameters.AddWithValue("Telefon" + i, item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel" + i, item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("von" + i, item.von == null ? (object)DBNull.Value : item.von);
					sqlCommand.Parameters.AddWithValue("Vorname" + i, item.Vorname == null ? (object)DBNull.Value : item.Vorname);
					sqlCommand.Parameters.AddWithValue("WWW" + i, item.WWW == null ? (object)DBNull.Value : item.WWW);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int nr, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Adressen] WHERE [Nr]=@Nr";
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

				string query = "DELETE FROM [Adressen] WHERE [Nr] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		#endregion Custom Methods

	}


}
