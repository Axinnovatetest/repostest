using Infrastructure.Data.Entities.Tables.CTS;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class PSZ_Nummerschlüssel_KundeAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_Nummerschlüssel Kunde] ([Analyse],[AnalyseName],[CreationTime],[CreationUserId],[CreationUserName],[CS ID],[CS Kontakt],[Kunde],[Kundennummer],[LastEditUserId],[LastEditUserName],[Nummerschlüssel],[Projektbetreuer D],[Stufe],[Technik Kontakt],[Technik Kontakt TN]) OUTPUT INSERTED.[ID] VALUES (@Analyse,@AnalyseName,@CreationTime,@CreationUserId,@CreationUserName,@CS_ID,@CS_Kontakt,@Kunde,@Kundennummer,@LastEditUserId,@LastEditUserName,@Nummerschlussel,@Projektbetreuer_D,@Stufe,@Technik_Kontakt,@Technik_Kontakt_TN); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Analyse", item.Analyse == null ? (object)DBNull.Value : item.Analyse);
					sqlCommand.Parameters.AddWithValue("AnalyseName", item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CS_ID", item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("Nummerschlussel", item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
					sqlCommand.Parameters.AddWithValue("Projektbetreuer_D", item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
					sqlCommand.Parameters.AddWithValue("Stufe", item.Stufe == null ? (object)DBNull.Value : item.Stufe);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt", item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items)
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
						query += " INSERT INTO [PSZ_Nummerschlüssel Kunde] ([Analyse],[AnalyseName],[CreationTime],[CreationUserId],[CreationUserName],[CS ID],[CS Kontakt],[Kunde],[Kundennummer],[LastEditUserId],[LastEditUserName],[Nummerschlüssel],[Projektbetreuer D],[Stufe],[Technik Kontakt],[Technik Kontakt TN]) VALUES ( "

							+ "@Analyse" + i + ","
							+ "@AnalyseName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUserName" + i + ","
							+ "@CS_ID" + i + ","
							+ "@CS_Kontakt" + i + ","
							+ "@Kunde" + i + ","
							+ "@Kundennummer" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@LastEditUserName" + i + ","
							+ "@Nummerschlussel" + i + ","
							+ "@Projektbetreuer_D" + i + ","
							+ "@Stufe" + i + ","
							+ "@Technik_Kontakt" + i + ","
							+ "@Technik_Kontakt_TN" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Analyse" + i, item.Analyse == null ? (object)DBNull.Value : item.Analyse);
						sqlCommand.Parameters.AddWithValue("AnalyseName" + i, item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("CS_ID" + i, item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
						sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("Nummerschlussel" + i, item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
						sqlCommand.Parameters.AddWithValue("Projektbetreuer_D" + i, item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
						sqlCommand.Parameters.AddWithValue("Stufe" + i, item.Stufe == null ? (object)DBNull.Value : item.Stufe);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt" + i, item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_Nummerschlüssel Kunde] SET [Analyse]=@Analyse, [AnalyseName]=@AnalyseName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [CS ID]=@CS_ID, [CS Kontakt]=@CS_Kontakt, [Kunde]=@Kunde, [Kundennummer]=@Kundennummer, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [Nummerschlüssel]=@Nummerschlussel, [Projektbetreuer D]=@Projektbetreuer_D, [Stufe]=@Stufe, [Technik Kontakt]=@Technik_Kontakt, [Technik Kontakt TN]=@Technik_Kontakt_TN WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("ID", item.ID);
				sqlCommand.Parameters.AddWithValue("Analyse", item.Analyse == null ? (object)DBNull.Value : item.Analyse);
				sqlCommand.Parameters.AddWithValue("AnalyseName", item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("CS_ID", item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
				sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
				sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
				sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("Nummerschlussel", item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
				sqlCommand.Parameters.AddWithValue("Projektbetreuer_D", item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
				sqlCommand.Parameters.AddWithValue("Stufe", item.Stufe == null ? (object)DBNull.Value : item.Stufe);
				sqlCommand.Parameters.AddWithValue("Technik_Kontakt", item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
				sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items)
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
						query += " UPDATE [PSZ_Nummerschlüssel Kunde] SET "

							+ "[Analyse]=@Analyse" + i + ","
							+ "[AnalyseName]=@AnalyseName" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUserName]=@CreationUserName" + i + ","
							+ "[CS ID]=@CS_ID" + i + ","
							+ "[CS Kontakt]=@CS_Kontakt" + i + ","
							+ "[Kunde]=@Kunde" + i + ","
							+ "[Kundennummer]=@Kundennummer" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[LastEditUserName]=@LastEditUserName" + i + ","
							+ "[Nummerschlüssel]=@Nummerschlussel" + i + ","
							+ "[Projektbetreuer D]=@Projektbetreuer_D" + i + ","
							+ "[Stufe]=@Stufe" + i + ","
							+ "[Technik Kontakt]=@Technik_Kontakt" + i + ","
							+ "[Technik Kontakt TN]=@Technik_Kontakt_TN" + i + " WHERE [ID]=@ID" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
						sqlCommand.Parameters.AddWithValue("Analyse" + i, item.Analyse == null ? (object)DBNull.Value : item.Analyse);
						sqlCommand.Parameters.AddWithValue("AnalyseName" + i, item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("CS_ID" + i, item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
						sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
						sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
						sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("Nummerschlussel" + i, item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
						sqlCommand.Parameters.AddWithValue("Projektbetreuer_D" + i, item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
						sqlCommand.Parameters.AddWithValue("Stufe" + i, item.Stufe == null ? (object)DBNull.Value : item.Stufe);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt" + i, item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
						sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID]=@ID";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("ID", id);

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

					string query = "DELETE FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [PSZ_Nummerschlüssel Kunde] ([Analyse],[AnalyseName],[CreationTime],[CreationUserId],[CreationUserName],[CS ID],[CS Kontakt],[Kunde],[Kundennummer],[LastEditUserId],[LastEditUserName],[Nummerschlüssel],[Projektbetreuer D],[Stufe],[Technik Kontakt],[Technik Kontakt TN]) OUTPUT INSERTED.[ID] VALUES (@Analyse,@AnalyseName,@CreationTime,@CreationUserId,@CreationUserName,@CS_ID,@CS_Kontakt,@Kunde,@Kundennummer,@LastEditUserId,@LastEditUserName,@Nummerschlussel,@Projektbetreuer_D,@Stufe,@Technik_Kontakt,@Technik_Kontakt_TN); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Analyse", item.Analyse == null ? (object)DBNull.Value : item.Analyse);
			sqlCommand.Parameters.AddWithValue("AnalyseName", item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("CS_ID", item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
			sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
			sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
			sqlCommand.Parameters.AddWithValue("Nummerschlussel", item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
			sqlCommand.Parameters.AddWithValue("Projektbetreuer_D", item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
			sqlCommand.Parameters.AddWithValue("Stufe", item.Stufe == null ? (object)DBNull.Value : item.Stufe);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt", item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_Nummerschlüssel Kunde] ([Analyse],[AnalyseName],[CreationTime],[CreationUserId],[CreationUserName],[CS ID],[CS Kontakt],[Kunde],[Kundennummer],[LastEditUserId],[LastEditUserName],[Nummerschlüssel],[Projektbetreuer D],[Stufe],[Technik Kontakt],[Technik Kontakt TN]) VALUES ( "

						+ "@Analyse" + i + ","
						+ "@AnalyseName" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreationUserName" + i + ","
						+ "@CS_ID" + i + ","
						+ "@CS_Kontakt" + i + ","
						+ "@Kunde" + i + ","
						+ "@Kundennummer" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@LastEditUserName" + i + ","
						+ "@Nummerschlussel" + i + ","
						+ "@Projektbetreuer_D" + i + ","
						+ "@Stufe" + i + ","
						+ "@Technik_Kontakt" + i + ","
						+ "@Technik_Kontakt_TN" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Analyse" + i, item.Analyse == null ? (object)DBNull.Value : item.Analyse);
					sqlCommand.Parameters.AddWithValue("AnalyseName" + i, item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CS_ID" + i, item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("Nummerschlussel" + i, item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
					sqlCommand.Parameters.AddWithValue("Projektbetreuer_D" + i, item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
					sqlCommand.Parameters.AddWithValue("Stufe" + i, item.Stufe == null ? (object)DBNull.Value : item.Stufe);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt" + i, item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_Nummerschlüssel Kunde] SET [Analyse]=@Analyse, [AnalyseName]=@AnalyseName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [CS ID]=@CS_ID, [CS Kontakt]=@CS_Kontakt, [Kunde]=@Kunde, [Kundennummer]=@Kundennummer, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [Nummerschlüssel]=@Nummerschlussel, [Projektbetreuer D]=@Projektbetreuer_D, [Stufe]=@Stufe, [Technik Kontakt]=@Technik_Kontakt, [Technik Kontakt TN]=@Technik_Kontakt_TN WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("ID", item.ID);
			sqlCommand.Parameters.AddWithValue("Analyse", item.Analyse == null ? (object)DBNull.Value : item.Analyse);
			sqlCommand.Parameters.AddWithValue("AnalyseName", item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
			sqlCommand.Parameters.AddWithValue("CS_ID", item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
			sqlCommand.Parameters.AddWithValue("CS_Kontakt", item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
			sqlCommand.Parameters.AddWithValue("Kunde", item.Kunde == null ? (object)DBNull.Value : item.Kunde);
			sqlCommand.Parameters.AddWithValue("Kundennummer", item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
			sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
			sqlCommand.Parameters.AddWithValue("Nummerschlussel", item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
			sqlCommand.Parameters.AddWithValue("Projektbetreuer_D", item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
			sqlCommand.Parameters.AddWithValue("Stufe", item.Stufe == null ? (object)DBNull.Value : item.Stufe);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt", item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
			sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN", item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 18; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_Nummerschlüssel Kunde] SET "

					+ "[Analyse]=@Analyse" + i + ","
					+ "[AnalyseName]=@AnalyseName" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CreationUserName]=@CreationUserName" + i + ","
					+ "[CS ID]=@CS_ID" + i + ","
					+ "[CS Kontakt]=@CS_Kontakt" + i + ","
					+ "[Kunde]=@Kunde" + i + ","
					+ "[Kundennummer]=@Kundennummer" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + ","
					+ "[LastEditUserName]=@LastEditUserName" + i + ","
					+ "[Nummerschlüssel]=@Nummerschlussel" + i + ","
					+ "[Projektbetreuer D]=@Projektbetreuer_D" + i + ","
					+ "[Stufe]=@Stufe" + i + ","
					+ "[Technik Kontakt]=@Technik_Kontakt" + i + ","
					+ "[Technik Kontakt TN]=@Technik_Kontakt_TN" + i + " WHERE [ID]=@ID" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("ID" + i, item.ID);
					sqlCommand.Parameters.AddWithValue("Analyse" + i, item.Analyse == null ? (object)DBNull.Value : item.Analyse);
					sqlCommand.Parameters.AddWithValue("AnalyseName" + i, item.AnalyseName == null ? (object)DBNull.Value : item.AnalyseName);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("CS_ID" + i, item.CS_ID == null ? (object)DBNull.Value : item.CS_ID);
					sqlCommand.Parameters.AddWithValue("CS_Kontakt" + i, item.CS_Kontakt == null ? (object)DBNull.Value : item.CS_Kontakt);
					sqlCommand.Parameters.AddWithValue("Kunde" + i, item.Kunde == null ? (object)DBNull.Value : item.Kunde);
					sqlCommand.Parameters.AddWithValue("Kundennummer" + i, item.Kundennummer == null ? (object)DBNull.Value : item.Kundennummer);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("Nummerschlussel" + i, item.Nummerschlüssel == null ? (object)DBNull.Value : item.Nummerschlüssel);
					sqlCommand.Parameters.AddWithValue("Projektbetreuer_D" + i, item.Projektbetreuer_D == null ? (object)DBNull.Value : item.Projektbetreuer_D);
					sqlCommand.Parameters.AddWithValue("Stufe" + i, item.Stufe == null ? (object)DBNull.Value : item.Stufe);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt" + i, item.Technik_Kontakt == null ? (object)DBNull.Value : item.Technik_Kontakt);
					sqlCommand.Parameters.AddWithValue("Technik_Kontakt_TN" + i, item.Technik_Kontakt_TN == null ? (object)DBNull.Value : item.Technik_Kontakt_TN);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID]=@ID";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ID", id);

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

				string query = "DELETE FROM [PSZ_Nummerschlüssel Kunde] WHERE [ID] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetLikeKunde(string searchText)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] where [Kunde] like @searchText";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("searchText", "%" + searchText + "%");
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetByCustomerNumber(int nummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [Kundennummer]=@nummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nummer", nummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity GetByCustomerNumberUnique(int kundennummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [Kundennummer]=@kundennummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kundennummer", kundennummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetByCustomerNumber(List<int> nummers)
		{
			if(nummers is null || nummers.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [Kundennummer] IN ({string.Join(",", nummers)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetByKundeKreis(string kreis)
		{
			kreis = (kreis ?? "").Trim();
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] where LTRIM(RTRIM([Nummerschlüssel]))=@kreis";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("kreis", kreis);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetForBenchmark()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] where [Analyse]=1";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> Get(bool ignoreNullNumbers)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [PSZ_Nummerschlüssel Kunde]{(ignoreNullNumbers ? " WHERE [Kundennummer] IS NOT NULL" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity> GetByCustomerAndKreis(int nummer, string kreis)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [Kundennummer]=@nummer AND LTRIM(RTRIM([Nummerschlüssel]))=@kreis";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("nummer", nummer);
				sqlCommand.Parameters.AddWithValue("kreis", kreis?.Trim());

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity>();
			}
		}
		public static PSZ_Nummerschlüssel_KundeEntity GetArtikelByFertigungNummer(string articleNummer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [PSZ_Nummerschlüssel Kunde] WHERE [Nummerschlüssel]=@articleNr";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("articleNr", articleNummer ?? "");

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
