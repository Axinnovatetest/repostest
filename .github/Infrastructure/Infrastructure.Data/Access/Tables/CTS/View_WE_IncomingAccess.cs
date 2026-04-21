


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables
{
	public class View_WE_IncomingAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity Get(string artikelnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View-WE-Incoming] WHERE [Artikelnummer]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", artikelnummer);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [View-WE-Incoming]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> Get(List<string> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> get(List<string> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [View-WE-Incoming] WHERE [Artikelnummer] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [View-WE-Incoming] ([Artikelnummer],[Eingangslieferscheinnr],[Pos],[WE_ID],[WE_VOH_Datum],[WE_VOH_Menge]) OUTPUT INSERTED.[Artikelnummer] VALUES (@Artikelnummer,@Eingangslieferscheinnr,@Pos,@WE_ID,@WE_VOH_Datum,@WE_VOH_Menge); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Pos", item.Pos == null ? (object)DBNull.Value : item.Pos);
					sqlCommand.Parameters.AddWithValue("WE_ID", item.WE_ID);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Datum", item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Menge", item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items)
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
						query += " INSERT INTO [View-WE-Incoming] ([Artikelnummer],[Eingangslieferscheinnr],[Pos],[WE_ID],[WE_VOH_Datum],[WE_VOH_Menge]) VALUES ( "

							+ "@Artikelnummer" + i + ","
							+ "@Eingangslieferscheinnr" + i + ","
							+ "@Pos" + i + ","
							+ "@WE_ID" + i + ","
							+ "@WE_VOH_Datum" + i + ","
							+ "@WE_VOH_Menge" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Pos" + i, item.Pos == null ? (object)DBNull.Value : item.Pos);
						sqlCommand.Parameters.AddWithValue("WE_ID" + i, item.WE_ID);
						sqlCommand.Parameters.AddWithValue("WE_VOH_Datum" + i, item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
						sqlCommand.Parameters.AddWithValue("WE_VOH_Menge" + i, item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [View-WE-Incoming] SET [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Pos]=@Pos, [WE_ID]=@WE_ID, [WE_VOH_Datum]=@WE_VOH_Datum, [WE_VOH_Menge]=@WE_VOH_Menge WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
				sqlCommand.Parameters.AddWithValue("Pos", item.Pos == null ? (object)DBNull.Value : item.Pos);
				sqlCommand.Parameters.AddWithValue("WE_ID", item.WE_ID);
				sqlCommand.Parameters.AddWithValue("WE_VOH_Datum", item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
				sqlCommand.Parameters.AddWithValue("WE_VOH_Menge", item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items)
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
						query += " UPDATE [View-WE-Incoming] SET "

							+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
							+ "[Pos]=@Pos" + i + ","
							+ "[WE_ID]=@WE_ID" + i + ","
							+ "[WE_VOH_Datum]=@WE_VOH_Datum" + i + ","
							+ "[WE_VOH_Menge]=@WE_VOH_Menge" + i + " WHERE [Artikelnummer]=@Artikelnummer" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
						sqlCommand.Parameters.AddWithValue("Pos" + i, item.Pos == null ? (object)DBNull.Value : item.Pos);
						sqlCommand.Parameters.AddWithValue("WE_ID" + i, item.WE_ID);
						sqlCommand.Parameters.AddWithValue("WE_VOH_Datum" + i, item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
						sqlCommand.Parameters.AddWithValue("WE_VOH_Menge" + i, item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Delete(string artikelnummer)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [View-WE-Incoming] WHERE [Artikelnummer]=@Artikelnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", artikelnummer);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Delete(List<string> ids)
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
		private static int delete(List<string> ids)
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

					string query = "DELETE FROM [View-WE-Incoming] WHERE [Artikelnummer] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity GetWithTransaction(string artikelnummer, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [View-WE-Incoming] WHERE [Artikelnummer]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", artikelnummer);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [View-WE-Incoming]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> GetWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> getWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [View-WE-Incoming] WHERE [Artikelnummer] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO [View-WE-Incoming] ([Artikelnummer],[Eingangslieferscheinnr],[Pos],[WE_ID],[WE_VOH_Datum],[WE_VOH_Menge]) OUTPUT INSERTED.[Artikelnummer] VALUES (@Artikelnummer,@Eingangslieferscheinnr,@Pos,@WE_ID,@WE_VOH_Datum,@WE_VOH_Menge); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Pos", item.Pos == null ? (object)DBNull.Value : item.Pos);
			sqlCommand.Parameters.AddWithValue("WE_ID", item.WE_ID);
			sqlCommand.Parameters.AddWithValue("WE_VOH_Datum", item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
			sqlCommand.Parameters.AddWithValue("WE_VOH_Menge", item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [View-WE-Incoming] ([Artikelnummer],[Eingangslieferscheinnr],[Pos],[WE_ID],[WE_VOH_Datum],[WE_VOH_Menge]) VALUES ( "

						+ "@Artikelnummer" + i + ","
						+ "@Eingangslieferscheinnr" + i + ","
						+ "@Pos" + i + ","
						+ "@WE_ID" + i + ","
						+ "@WE_VOH_Datum" + i + ","
						+ "@WE_VOH_Menge" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Pos" + i, item.Pos == null ? (object)DBNull.Value : item.Pos);
					sqlCommand.Parameters.AddWithValue("WE_ID" + i, item.WE_ID);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Datum" + i, item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Menge" + i, item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [View-WE-Incoming] SET [Eingangslieferscheinnr]=@Eingangslieferscheinnr, [Pos]=@Pos, [WE_ID]=@WE_ID, [WE_VOH_Datum]=@WE_VOH_Datum, [WE_VOH_Menge]=@WE_VOH_Menge WHERE [Artikelnummer]=@Artikelnummer";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr", item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
			sqlCommand.Parameters.AddWithValue("Pos", item.Pos == null ? (object)DBNull.Value : item.Pos);
			sqlCommand.Parameters.AddWithValue("WE_ID", item.WE_ID);
			sqlCommand.Parameters.AddWithValue("WE_VOH_Datum", item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
			sqlCommand.Parameters.AddWithValue("WE_VOH_Menge", item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 6; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.View_WE_IncomingEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [View-WE-Incoming] SET "

					+ "[Eingangslieferscheinnr]=@Eingangslieferscheinnr" + i + ","
					+ "[Pos]=@Pos" + i + ","
					+ "[WE_ID]=@WE_ID" + i + ","
					+ "[WE_VOH_Datum]=@WE_VOH_Datum" + i + ","
					+ "[WE_VOH_Menge]=@WE_VOH_Menge" + i + " WHERE [Artikelnummer]=@Artikelnummer" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Eingangslieferscheinnr" + i, item.Eingangslieferscheinnr == null ? (object)DBNull.Value : item.Eingangslieferscheinnr);
					sqlCommand.Parameters.AddWithValue("Pos" + i, item.Pos == null ? (object)DBNull.Value : item.Pos);
					sqlCommand.Parameters.AddWithValue("WE_ID" + i, item.WE_ID);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Datum" + i, item.WE_VOH_Datum == null ? (object)DBNull.Value : item.WE_VOH_Datum);
					sqlCommand.Parameters.AddWithValue("WE_VOH_Menge" + i, item.WE_VOH_Menge == null ? (object)DBNull.Value : item.WE_VOH_Menge);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(string artikelnummer, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [View-WE-Incoming] WHERE [Artikelnummer]=@Artikelnummer";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", artikelnummer);

			results = sqlCommand.ExecuteNonQuery();


			return results;
		}
		public static int DeleteWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
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
		private static int deleteWithTransaction(List<string> ids, SqlConnection connection, SqlTransaction transaction)
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

				string query = "DELETE FROM [View-WE-Incoming] WHERE [Artikelnummer] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<int> GetAllIds(int filter)
		{
			var dataTable = new DataTable();
			string Queryfilter = string.Empty;
			if(filter > 0)
			{
				Queryfilter = $" where WE_ID like '{filter}%' ";
			}
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT TOP 10 WE_ID FROM [View-WE-Incoming]  " + Queryfilter;
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => Convert.ToInt32(x["WE_ID"])).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		#endregion Custom Methods

	}
}