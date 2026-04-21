using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class FA_Werk_Wunsh_Update_detailsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [FA_Werk_Wunsh_Update_details] ([FA],[Id_update],[updated],[Werk]) OUTPUT INSERTED.[Id] VALUES (@FA,@Id_update,@updated,@Werk); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("FA", item.FA == null ? (object)DBNull.Value : item.FA);
					sqlCommand.Parameters.AddWithValue("Id_update", item.Id_update == null ? (object)DBNull.Value : item.Id_update);
					sqlCommand.Parameters.AddWithValue("updated", item.updated == null ? (object)DBNull.Value : item.updated);
					sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items)
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
						query += " INSERT INTO [FA_Werk_Wunsh_Update_details] ([FA],[Id_update],[updated],[Werk]) VALUES ( "

							+ "@FA" + i + ","
							+ "@Id_update" + i + ","
							+ "@updated" + i + ","
							+ "@Werk" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("FA" + i, item.FA == null ? (object)DBNull.Value : item.FA);
						sqlCommand.Parameters.AddWithValue("Id_update" + i, item.Id_update == null ? (object)DBNull.Value : item.Id_update);
						sqlCommand.Parameters.AddWithValue("updated" + i, item.updated == null ? (object)DBNull.Value : item.updated);
						sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [FA_Werk_Wunsh_Update_details] SET [FA]=@FA, [Id_update]=@Id_update, [updated]=@updated, [Werk]=@Werk WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("FA", item.FA == null ? (object)DBNull.Value : item.FA);
				sqlCommand.Parameters.AddWithValue("Id_update", item.Id_update == null ? (object)DBNull.Value : item.Id_update);
				sqlCommand.Parameters.AddWithValue("updated", item.updated == null ? (object)DBNull.Value : item.updated);
				sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items)
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
						query += " UPDATE [FA_Werk_Wunsh_Update_details] SET "

							+ "[FA]=@FA" + i + ","
							+ "[Id_update]=@Id_update" + i + ","
							+ "[updated]=@updated" + i + ","
							+ "[Werk]=@Werk" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("FA" + i, item.FA == null ? (object)DBNull.Value : item.FA);
						sqlCommand.Parameters.AddWithValue("Id_update" + i, item.Id_update == null ? (object)DBNull.Value : item.Id_update);
						sqlCommand.Parameters.AddWithValue("updated" + i, item.updated == null ? (object)DBNull.Value : item.updated);
						sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
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
				string query = "DELETE FROM [FA_Werk_Wunsh_Update_details] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [FA_Werk_Wunsh_Update_details] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [FA_Werk_Wunsh_Update_details] ([FA],[Id_update],[updated],[Werk]) OUTPUT INSERTED.[Id] VALUES (@FA,@Id_update,@updated,@Werk); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FA", item.FA == null ? (object)DBNull.Value : item.FA);
			sqlCommand.Parameters.AddWithValue("Id_update", item.Id_update == null ? (object)DBNull.Value : item.Id_update);
			sqlCommand.Parameters.AddWithValue("updated", item.updated == null ? (object)DBNull.Value : item.updated);
			sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [FA_Werk_Wunsh_Update_details] ([FA],[Id_update],[updated],[Werk]) VALUES ( "

						+ "@FA" + i + ","
						+ "@Id_update" + i + ","
						+ "@updated" + i + ","
						+ "@Werk" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("FA" + i, item.FA == null ? (object)DBNull.Value : item.FA);
					sqlCommand.Parameters.AddWithValue("Id_update" + i, item.Id_update == null ? (object)DBNull.Value : item.Id_update);
					sqlCommand.Parameters.AddWithValue("updated" + i, item.updated == null ? (object)DBNull.Value : item.updated);
					sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [FA_Werk_Wunsh_Update_details] SET [FA]=@FA, [Id_update]=@Id_update, [updated]=@updated, [Werk]=@Werk WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FA", item.FA == null ? (object)DBNull.Value : item.FA);
			sqlCommand.Parameters.AddWithValue("Id_update", item.Id_update == null ? (object)DBNull.Value : item.Id_update);
			sqlCommand.Parameters.AddWithValue("updated", item.updated == null ? (object)DBNull.Value : item.updated);
			sqlCommand.Parameters.AddWithValue("Werk", item.Werk == null ? (object)DBNull.Value : item.Werk);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [FA_Werk_Wunsh_Update_details] SET "

					+ "[FA]=@FA" + i + ","
					+ "[Id_update]=@Id_update" + i + ","
					+ "[updated]=@updated" + i + ","
					+ "[Werk]=@Werk" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("FA" + i, item.FA == null ? (object)DBNull.Value : item.FA);
					sqlCommand.Parameters.AddWithValue("Id_update" + i, item.Id_update == null ? (object)DBNull.Value : item.Id_update);
					sqlCommand.Parameters.AddWithValue("updated" + i, item.updated == null ? (object)DBNull.Value : item.updated);
					sqlCommand.Parameters.AddWithValue("Werk" + i, item.Werk == null ? (object)DBNull.Value : item.Werk);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [FA_Werk_Wunsh_Update_details] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [FA_Werk_Wunsh_Update_details] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> GetByUpdateId(int updateId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id_update]=@updateId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("updateId", updateId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity> GetByUpdateId(int updateId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();
			//using (var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				//sqlConnection.Open();
				string query = "SELECT * FROM [FA_Werk_Wunsh_Update_details] WHERE [Id_update]=@updateId";
				var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
				sqlCommand.Parameters.AddWithValue("updateId", updateId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity>();
			}
		}

		#endregion
	}
}
