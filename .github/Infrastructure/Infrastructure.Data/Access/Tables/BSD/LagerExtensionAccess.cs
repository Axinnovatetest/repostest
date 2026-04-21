using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class LagerExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [LagerExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [LagerExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [LagerExtension] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [LagerExtension] ([ArtikelNr],[Bestand],[Index_Kunde],[Lagerort_id],[LastEditTime],[LastEditUserId]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Bestand,@Index_Kunde,@Lagerort_id,@LastEditTime,@LastEditUserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items)
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
						query += " INSERT INTO [LagerExtension] ([ArtikelNr],[Bestand],[Index_Kunde],[Lagerort_id],[LastEditTime],[LastEditUserId]) VALUES ( "

							+ "@ArtikelNr" + i + ","
							+ "@Bestand" + i + ","
							+ "@Index_Kunde" + i + ","
							+ "@Lagerort_id" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [LagerExtension] SET [ArtikelNr]=@ArtikelNr, [Bestand]=@Bestand, [Index_Kunde]=@Index_Kunde, [Lagerort_id]=@Lagerort_id, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items)
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
						query += " UPDATE [LagerExtension] SET "

							+ "[ArtikelNr]=@ArtikelNr" + i + ","
							+ "[Bestand]=@Bestand" + i + ","
							+ "[Index_Kunde]=@Index_Kunde" + i + ","
							+ "[Lagerort_id]=@Lagerort_id" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
						sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
						sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde);
						sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
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
				string query = "DELETE FROM [LagerExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [LagerExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}


		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [LagerExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [LagerExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [LagerExtension] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [LagerExtension] ([ArtikelNr],[Bestand],[Index_Kunde],[Lagerort_id],[LastEditTime],[LastEditUserId]) OUTPUT INSERTED.[Id] VALUES (@ArtikelNr,@Bestand,@Index_Kunde,@Lagerort_id,@LastEditTime,@LastEditUserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [LagerExtension] ([ArtikelNr],[Bestand],[Index_Kunde],[Lagerort_id],[LastEditTime],[LastEditUserId]) VALUES ( "

						+ "@ArtikelNr" + i + ","
						+ "@Bestand" + i + ","
						+ "@Index_Kunde" + i + ","
						+ "@Lagerort_id" + i + ","
						+ "@LastEditTime" + i + ","
						+ "@LastEditUserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [LagerExtension] SET [ArtikelNr]=@ArtikelNr, [Bestand]=@Bestand, [Index_Kunde]=@Index_Kunde, [Lagerort_id]=@Lagerort_id, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
			sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
			sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
			sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
			sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
			sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [LagerExtension] SET "

					+ "[ArtikelNr]=@ArtikelNr" + i + ","
					+ "[Bestand]=@Bestand" + i + ","
					+ "[Index_Kunde]=@Index_Kunde" + i + ","
					+ "[Lagerort_id]=@Lagerort_id" + i + ","
					+ "[LastEditTime]=@LastEditTime" + i + ","
					+ "[LastEditUserId]=@LastEditUserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArtikelNr" + i, item.ArtikelNr);
					sqlCommand.Parameters.AddWithValue("Bestand" + i, item.Bestand == null ? (object)DBNull.Value : item.Bestand);
					sqlCommand.Parameters.AddWithValue("Index_Kunde" + i, item.Index_Kunde);
					sqlCommand.Parameters.AddWithValue("Lagerort_id" + i, item.Lagerort_id);
					sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [LagerExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

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

				string query = "DELETE FROM [LagerExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIds(List<int> articleIds)
		{
			if(articleIds == null || articleIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE [ArtikelNr] IN ({string.Join(",", articleIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleAndLagerId(int articleId, int lagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE [ArtikelNr]=@articleId AND [Lagerort_Id]=@lagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("lagerId", lagerId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity GetByArticleIdAndKundenIndexLager(int articleId, string kundenIndex, int lagerorId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [LagerExtension] WHERE [ArtikelNr]=@articleId AND [Index_Kunde]=@kundenIndex AND [Lagerort_id]=@lagerorId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("kundenIndex", kundenIndex);
				sqlCommand.Parameters.AddWithValue("lagerorId", lagerorId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIdAndKundenIndex(int articleId, string kundenIndex)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE [ArtikelNr]=@articleId AND [Index_Kunde]=@kundenIndex";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("kundenIndex", kundenIndex);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIdAndKundenIndex(int articleId, string kundenIndex, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			{
				string query = $"SELECT * FROM [LagerExtension] WHERE [ArtikelNr]=@articleId AND [Index_Kunde]=@kundenIndex";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);
				sqlCommand.Parameters.AddWithValue("kundenIndex", kundenIndex);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIdAndKundenIndex(List<KeyValuePair<int, string>> articleIdKundenIndex)
		{
			if(articleIdKundenIndex == null || articleIdKundenIndex.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE " +
					$"{string.Join(" OR ", (articleIdKundenIndex.Select(x => $"([ArtikelNr]={x.Key} AND [Index_Kunde]='{x.Value}')")))}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIdAndKundenIndexLager(List<Tuple<int, string, int>> articleIdKundenIndexLagerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE " +
					$"{string.Join(" OR ", (articleIdKundenIndexLagerId.Select(x => $"([ArtikelNr]={x.Item1} AND [Index_Kunde]={x.Item2} AND [Lagerort_id]={x.Item3})")))}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleIdAndLagerId(List<KeyValuePair<int, int>> articleIdLagerId)
		{
			if(articleIdLagerId == null || articleIdLagerId.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE " +
					$"{string.Join(" OR ", (articleIdLagerId.Select(x => $"([ArtikelNr]={x.Key} AND [Lagerort_id]='{x.Value}')")))}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity> GetByArticleAndLagerIds(int articleId, List<int> lagerIds)
		{
			if(lagerIds == null || lagerIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [LagerExtension] WHERE " +
					$"[ArtikelNr]=@articleId AND [Lagerort_id] IN ({string.Join(", ", lagerIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("articleId", articleId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [LagerExtension] ([ArtikelNr],[Bestand],[Index_Kunde],[Lagerort_id],[LastEditTime],[LastEditUserId])  VALUES (@ArtikelNr,@Bestand,@Index_Kunde,@Lagerort_id,@LastEditTime,@LastEditUserId); ";
			query += "SELECT SCOPE_IDENTITY();";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;

				return response;
			}
		}
		public static int Update(Infrastructure.Data.Entities.Tables.BSD.LagerExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [LagerExtension] SET [ArtikelNr]=@ArtikelNr, [Bestand]=@Bestand, [Index_Kunde]=@Index_Kunde, [Lagerort_id]=@Lagerort_id, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId WHERE [Id]=@Id";

			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArtikelNr", item.ArtikelNr);
				sqlCommand.Parameters.AddWithValue("Bestand", item.Bestand == null ? (object)DBNull.Value : item.Bestand);
				sqlCommand.Parameters.AddWithValue("Index_Kunde", item.Index_Kunde);
				sqlCommand.Parameters.AddWithValue("Lagerort_id", item.Lagerort_id);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		#endregion
	}
}
