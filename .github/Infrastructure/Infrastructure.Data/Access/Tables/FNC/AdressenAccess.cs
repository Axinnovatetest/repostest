using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.FNC
{
	public class AdressenAccess
	{
		public enum AddressType
		{
			Supplier = 1,
			Customer = 3
		}
		#region Default Methods
		public static Entities.Tables.FNC.AdressenEntity Get(int id)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE Nr=@Id";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", id);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.FNC.AdressenEntity> Get()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen";

			var sqlCommand = new SqlCommand(query, sqlConnection);


			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}
		public static List<Entities.Tables.FNC.AdressenEntity> Get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			int max = Settings.MAX_BATCH_SIZE;

			if(ids.Count <= max)
			{
				return get(ids);
			}

			int batchNumber = ids.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(get(ids.GetRange(i * max, max)));
			}
			result.AddRange(get(ids.GetRange(batchNumber * max, ids.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.FNC.AdressenEntity> get(List<int> ids)
		{
			if(ids == null || ids.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
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

			sqlCommand.CommandText = $"SELECT * FROM __FNC_Adressen WHERE Nr IN ({queryIds})";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}

		public static int Insert(Entities.Tables.FNC.AdressenEntity item)
		{
			var response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Adressen] ([Abteilung],[Adresstyp],[Anrede],[Auswahl],[Bemerkungen],[Briefanrede],[eMail],[erfasst],[Fax],[Funktion],[Kundennummer],[Land],[Lieferantennummer],[Name1],[Name2],[Name3],[Ort],[Personalnummer],[PLZ_Postfach],[PLZ_Straße],[Postfach],[Postfach bevorzugt],[Sortierbegriff],[sperren],[Straße],[stufe],[Telefon],[Titel],[von],[Vorname],[WWW])  VALUES (@Abteilung,@Adresstyp,@Anrede,@Auswahl,@Bemerkungen,@Briefanrede,@eMail,@erfasst,@Fax,@Funktion,@Kundennummer,@Land,@Lieferantennummer,@Name1,@Name2,@Name3,@Ort,@Personalnummer,@PLZ_Postfach,@PLZ_Straße,@Postfach,@Postfach_bevorzugt,@Sortierbegriff,@sperren,@Straße,@stufe,@Telefon,@Titel,@von,@Vorname,@WWW); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung);
					sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
					sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
					sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl);
					sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
					sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
					sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
					sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
					sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
					sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
					sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
					sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
					sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
					sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
					sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
					sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
					sqlCommand.Parameters.AddWithValue("PLZ_Straße", item.PLZ_StraBe == null ? (object)DBNull.Value : item.PLZ_StraBe);
					sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
					sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt);
					sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
					sqlCommand.Parameters.AddWithValue("sperren", item.sperren);
					sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
					sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
					sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
					sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
					sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
					sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
					sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Entities.Tables.FNC.AdressenEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Adressen] SET [Abteilung]=@Abteilung, [Adresstyp]=@Adresstyp, [Anrede]=@Anrede, [Auswahl]=@Auswahl, [Bemerkungen]=@Bemerkungen, [Briefanrede]=@Briefanrede, [eMail]=@eMail, [erfasst]=@erfasst, [Fax]=@Fax, [Funktion]=@Funktion, [Kundennummer]=@Kundennummer, [Land]=@Land, [Lieferantennummer]=@Lieferantennummer, [Name1]=@Name1, [Name2]=@Name2, [Name3]=@Name3, [Ort]=@Ort, [Personalnummer]=@Personalnummer, [PLZ_Postfach]=@PLZ_Postfach, [PLZ_Straße]=@PLZ_Straße, [Postfach]=@Postfach, [Postfach bevorzugt]=@Postfach_bevorzugt, [Sortierbegriff]=@Sortierbegriff, [sperren]=@sperren, [Straße]=@Straße, [stufe]=@stufe, [Telefon]=@Telefon, [Titel]=@Titel, [von]=@von, [Vorname]=@Vorname, [WWW]=@WWW WHERE [Nr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Abteilung", item.Abteilung);
				sqlCommand.Parameters.AddWithValue("Adresstyp", item.Adresstyp == null ? (object)DBNull.Value : item.Adresstyp);
				sqlCommand.Parameters.AddWithValue("Anrede", item.Anrede == null ? (object)DBNull.Value : item.Anrede);
				sqlCommand.Parameters.AddWithValue("Auswahl", item.Auswahl);
				sqlCommand.Parameters.AddWithValue("Bemerkungen", item.Bemerkungen == null ? (object)DBNull.Value : item.Bemerkungen);
				sqlCommand.Parameters.AddWithValue("Briefanrede", item.Briefanrede == null ? (object)DBNull.Value : item.Briefanrede);
				sqlCommand.Parameters.AddWithValue("eMail", item.eMail == null ? (object)DBNull.Value : item.eMail);
				sqlCommand.Parameters.AddWithValue("erfasst", item.erfasst == null ? (object)DBNull.Value : item.erfasst);
				sqlCommand.Parameters.AddWithValue("Fax", item.Fax == null ? (object)DBNull.Value : item.Fax);
				sqlCommand.Parameters.AddWithValue("Funktion", item.Funktion == null ? (object)DBNull.Value : item.Funktion);
				sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
				sqlCommand.Parameters.AddWithValue("Land", item.Land == null ? (object)DBNull.Value : item.Land);
				sqlCommand.Parameters.AddWithValue("Lieferantennummer", item.Lieferantennummer == null ? (object)DBNull.Value : item.Lieferantennummer);
				sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("Name3", item.Name3 == null ? (object)DBNull.Value : item.Name3);
				sqlCommand.Parameters.AddWithValue("Ort", item.Ort == null ? (object)DBNull.Value : item.Ort);
				sqlCommand.Parameters.AddWithValue("Personalnummer", item.Personalnummer == null ? (object)DBNull.Value : item.Personalnummer);
				sqlCommand.Parameters.AddWithValue("PLZ_Postfach", item.PLZ_Postfach == null ? (object)DBNull.Value : item.PLZ_Postfach);
				sqlCommand.Parameters.AddWithValue("PLZ_Straße", item.PLZ_StraBe == null ? (object)DBNull.Value : item.PLZ_StraBe);
				sqlCommand.Parameters.AddWithValue("Postfach", item.Postfach == null ? (object)DBNull.Value : item.Postfach);
				sqlCommand.Parameters.AddWithValue("Postfach_bevorzugt", item.Postfach_bevorzugt);
				sqlCommand.Parameters.AddWithValue("Sortierbegriff", item.Sortierbegriff == null ? (object)DBNull.Value : item.Sortierbegriff);
				sqlCommand.Parameters.AddWithValue("sperren", item.sperren);
				sqlCommand.Parameters.AddWithValue("Straße", item.StraBe == null ? (object)DBNull.Value : item.StraBe);
				sqlCommand.Parameters.AddWithValue("stufe", item.stufe == null ? (object)DBNull.Value : item.stufe);
				sqlCommand.Parameters.AddWithValue("Telefon", item.Telefon == null ? (object)DBNull.Value : item.Telefon);
				sqlCommand.Parameters.AddWithValue("Titel", item.Titel == null ? (object)DBNull.Value : item.Titel);
				sqlCommand.Parameters.AddWithValue("von", item.von == null ? (object)DBNull.Value : item.von);
				sqlCommand.Parameters.AddWithValue("Vorname", item.Vorname == null ? (object)DBNull.Value : item.Vorname);
				sqlCommand.Parameters.AddWithValue("WWW", item.WWW == null ? (object)DBNull.Value : item.WWW);
				sqlCommand.Parameters.AddWithValue("Nr", item.Nr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int id)
		{
			var sqlConection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConection.Open();

			string query = "DELETE FROM __FNC_Adressen WHERE Nr=@Nr";

			var sqlCommand = new SqlCommand(query, sqlConection);
			sqlCommand.Parameters.AddWithValue("Nr", id);

			int response = DbExecution.ExecuteNonQuery(sqlCommand);

			sqlConection.Close();

			return response;
		}
		#endregion

		#region Custom Methods
		public static Entities.Tables.FNC.AdressenEntity GetByDunsNumber(int dunsNumer)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE DUNS=@DunsNumer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("DunsNumer", dunsNumer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Entities.Tables.FNC.AdressenEntity GetByKundenNummer(int kundenNummer)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE Kundennummer=@KundenNummer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("KundenNummer", kundenNummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.FNC.AdressenEntity> GetKundenAddresses()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE Adresstyp=1";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.FNC.AdressenEntity> GetLiferentenAddresses()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen -- WHERE Adresstyp=3";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.FNC.AdressenEntity> GetByKundenNummers(List<int> kundenNummers)
		{
			if(kundenNummers == null || kundenNummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			int max = Settings.MAX_BATCH_SIZE;

			if(kundenNummers.Count <= max)
			{
				return getByKundenNummers(kundenNummers);
			}

			int batchNumber = kundenNummers.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByKundenNummers(kundenNummers.GetRange(i * max, max)));
			}
			result.AddRange(getByKundenNummers(kundenNummers.GetRange(batchNumber * max, kundenNummers.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.FNC.AdressenEntity> getByKundenNummers(List<int> kundenNummers)
		{
			if(kundenNummers == null || kundenNummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < kundenNummers.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, kundenNummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $"SELECT * FROM __FNC_Adressen WHERE [KundenNummer] IN ({queryIds})";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}

		public static List<Entities.Tables.FNC.AdressenEntity> GetLikeNames(string searchText)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = " SELECT * FROM [__FNC_Adressen] "
				+ " WHERE [Name1] LIKE @searchText "
				+ " OR [Name2] LIKE @searchText "
				+ " OR [Name3] LIKE @searchText ";

			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}

		public static Entities.Tables.FNC.AdressenEntity GetByLieferantennummer(int lieferantennummer)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE Lieferantennummer=@lieferantennummer";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("lieferantennummer", lieferantennummer);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.FNC.AdressenEntity> GetByLieferantennummers(List<int> lieferantennummers)
		{
			if(lieferantennummers == null || lieferantennummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			int max = Settings.MAX_BATCH_SIZE;

			if(lieferantennummers.Count <= max)
			{
				return getByKundenNummers(lieferantennummers);
			}

			int batchNumber = lieferantennummers.Count / max;
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			for(int i = 0; i < batchNumber; i++)
			{
				result.AddRange(getByKundenNummers(lieferantennummers.GetRange(i * max, max)));
			}
			result.AddRange(getByKundenNummers(lieferantennummers.GetRange(batchNumber * max, lieferantennummers.Count - batchNumber * max)));
			return result;
		}
		private static List<Entities.Tables.FNC.AdressenEntity> getByLieferantennummers(List<int> lieferantennummers)
		{
			if(lieferantennummers == null || lieferantennummers.Count == 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}

			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;

			string queryIds = string.Empty;
			for(int i = 0; i < lieferantennummers.Count; i++)
			{
				queryIds += "@Id" + i + ",";
				sqlCommand.Parameters.AddWithValue("Id" + i, lieferantennummers[i]);
			}
			queryIds = queryIds.TrimEnd(',');

			sqlCommand.CommandText = $"SELECT * FROM __FNC_Adressen WHERE [Lieferantennummer] IN ({queryIds})";

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dt = new DataTable();
			selectAdapter.Fill(dt);
			if(dt.Rows.Count > 0)
			{
				return toList(dt);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}

		public static List<Entities.Tables.FNC.AdressenEntity> GetWhereLieferantennummerIsNotNull()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE [Lieferantennummer] IS NOT NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new List<Entities.Tables.FNC.AdressenEntity>();
			}

			return toList(dataTable);
		}
		public static List<Entities.Tables.FNC.AdressenEntity> GetWhereKundennummerIsNotNull()
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE [Kundennummer] IS NOT NULL";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count == 0)
			{
				return new List<Entities.Tables.FNC.AdressenEntity>();
			}

			return toList(dataTable);
		}

		public static Entities.Tables.FNC.AdressenEntity GetByName1(string name1, int addressType)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = "SELECT * FROM __FNC_Adressen WHERE [Name1]=@name1 AND adressTyp=@addressType";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("name1", name1);
			sqlCommand.Parameters.AddWithValue("addressType", addressType);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}


		public static List<Entities.Tables.FNC.AdressenEntity> GetLikeSupplierNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [__FNC_Adressen] AS Ad JOIN [__FNC_Lieferanten] AS lf ON ad.Nr = lf.nummer WHERE lf.nr is not null AND Ad.Nr IS NOT NULL AND [Lieferantennummer] LIKE '{nummer}%' ORDER by [Lieferantennummer] ASC";
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
		public static List<Entities.Tables.FNC.AdressenEntity> GetLikeSupplierNames(string searchText)
		{
			var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC);
			sqlConnection.Open();

			string query = $" SELECT * FROM [__FNC_Adressen] "
				+ $" WHERE ( [Name1] LIKE '%{searchText}%' "
				+ $" OR [Name2] LIKE '%{searchText}%' "
				+ $" OR [Name3] LIKE '%{searchText}%' ) ";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>();
			}
		}

		#region >>>>>>> Customers >>>>>>>

		public static List<Entities.Tables.FNC.AdressenEntity> GetCustomerDeliveryAddresses()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * "
						+ "FROM [__FNC_Adressen] WHERE [Adresstyp] = 1 "
						+ "AND [Nr] IS NOT NULL "
						+ "AND RTRIM(LTRIM(RTRIM(LTRIM(Name1)) +' ' + RTRIM(LTRIM(Name2)) + ' ' + RTRIM(LTRIM(Straße)) + ' ' + RTRIM(LTRIM(Ort)) + ' ' + RTRIM(LTRIM(Nr)) )) IS NOT NULL "
						+ "ORDER by[Sortierbegriff] ASC";

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
		public static List<Entities.Tables.FNC.AdressenEntity> GetLikeCustomerNumber(string nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();

				using(var sqlCommand = new SqlCommand())
				{
					sqlCommand.CommandText = $"SELECT * FROM [__FNC_Adressen] WHERE adresstyp = 1 AND Nr IS NOT NULL AND [kundennummer] LIKE '{nummer}%' ORDER by [kundennummer] ASC";
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
		#endregion >>>>>>> Customers >>>>>>>
		#endregion

		#region Helpers
		private static List<Entities.Tables.FNC.AdressenEntity> toList(DataTable dataTable)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.FNC.AdressenEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Infrastructure.Data.Entities.Tables.FNC.AdressenEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
