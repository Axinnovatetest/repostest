using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CRP.HistoryFG
{
	public class HistoryHeaderFGBestandAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Header]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [stats].[__CRP_HistoryFG_Header] ([CreateDate],[CreatedUserName],[CreateUserId],[ImportDate],[ImportType]) OUTPUT INSERTED.[Id] VALUES (@CreateDate,@CreatedUserName,@CreateUserId,@ImportDate,@ImportType); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
					sqlCommand.Parameters.AddWithValue("CreatedUserName", item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("ImportDate", item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
					sqlCommand.Parameters.AddWithValue("ImportType", item.ImportType == null ? (object)DBNull.Value : item.ImportType);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items)
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
						query += " INSERT INTO [stats].[__CRP_HistoryFG_Header] ([CreateDate],[CreatedUserName],[CreateUserId],[ImportDate],[ImportType]) VALUES ( "

							+ "@CreateDate" + i + ","
							+ "@CreatedUserName" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@ImportDate" + i + ","
							+ "@ImportType" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
						sqlCommand.Parameters.AddWithValue("CreatedUserName" + i, item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("ImportDate" + i, item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
						sqlCommand.Parameters.AddWithValue("ImportType" + i, item.ImportType == null ? (object)DBNull.Value : item.ImportType);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [stats].[__CRP_HistoryFG_Header] SET [CreateDate]=@CreateDate, [CreatedUserName]=@CreatedUserName, [CreateUserId]=@CreateUserId, [ImportDate]=@ImportDate, [ImportType]=@ImportType WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
				sqlCommand.Parameters.AddWithValue("CreatedUserName", item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("ImportDate", item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
				sqlCommand.Parameters.AddWithValue("ImportType", item.ImportType == null ? (object)DBNull.Value : item.ImportType);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items)
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
						query += " UPDATE [stats].[__CRP_HistoryFG_Header] SET "

							+ "[CreateDate]=@CreateDate" + i + ","
							+ "[CreatedUserName]=@CreatedUserName" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[ImportDate]=@ImportDate" + i + ","
							+ "[ImportType]=@ImportType" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
						sqlCommand.Parameters.AddWithValue("CreatedUserName" + i, item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("ImportDate" + i, item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
						sqlCommand.Parameters.AddWithValue("ImportType" + i, item.ImportType == null ? (object)DBNull.Value : item.ImportType);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
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
				string query = "DELETE FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

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

					string query = "DELETE FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__CRP_HistoryFG_Header]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [stats].[__CRP_HistoryFG_Header] ([CreateDate],[CreatedUserName],[CreateUserId],[ImportDate],[ImportType]) OUTPUT INSERTED.[Id] VALUES (@CreateDate,@CreatedUserName,@CreateUserId,@ImportDate,@ImportType); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
			sqlCommand.Parameters.AddWithValue("CreatedUserName", item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("ImportDate", item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
			sqlCommand.Parameters.AddWithValue("ImportType", item.ImportType == null ? (object)DBNull.Value : item.ImportType);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [stats].[__CRP_HistoryFG_Header] ([CreateDate],[CreatedUserName],[CreateUserId],[ImportDate],[ImportType]) VALUES ( "

						+ "@CreateDate" + i + ","
						+ "@CreatedUserName" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@ImportDate" + i + ","
						+ "@ImportType" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
					sqlCommand.Parameters.AddWithValue("CreatedUserName" + i, item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("ImportDate" + i, item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
					sqlCommand.Parameters.AddWithValue("ImportType" + i, item.ImportType == null ? (object)DBNull.Value : item.ImportType);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [stats].[__CRP_HistoryFG_Header] SET [CreateDate]=@CreateDate, [CreatedUserName]=@CreatedUserName, [CreateUserId]=@CreateUserId, [ImportDate]=@ImportDate, [ImportType]=@ImportType WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
			sqlCommand.Parameters.AddWithValue("CreatedUserName", item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("ImportDate", item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
			sqlCommand.Parameters.AddWithValue("ImportType", item.ImportType == null ? (object)DBNull.Value : item.ImportType);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [stats].[__CRP_HistoryFG_Header] SET "

					+ "[CreateDate]=@CreateDate" + i + ","
					+ "[CreatedUserName]=@CreatedUserName" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[ImportDate]=@ImportDate" + i + ","
					+ "[ImportType]=@ImportType" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
					sqlCommand.Parameters.AddWithValue("CreatedUserName" + i, item.CreatedUserName == null ? (object)DBNull.Value : item.CreatedUserName);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("ImportDate" + i, item.ImportDate == null ? (object)DBNull.Value : item.ImportDate);
					sqlCommand.Parameters.AddWithValue("ImportType" + i, item.ImportType == null ? (object)DBNull.Value : item.ImportType);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [stats].[__CRP_HistoryFG_Header] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> GetHistoryFGHeaderData(DateTime? from,DateTime? to, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			string paging = "";
			string sorting = "";

			if(dataPaging != null && (0 >= dataPaging.RequestRows || dataPaging.RequestRows > 100))
			{
				dataPaging.RequestRows = 100;
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select distinct h.* from [stats].[__CRP_HistoryFG_Header] h inner join [stats].[__CRP_HistoryFG_Details] d on h.Id=d.HeaderId";

				var isFirstClaus = true;
				if(from is not null)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} CONVERT(date,h.ImportDate)>='{from}'";
					isFirstClaus=false;
				}
				if(to is not null)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} CONVERT(date,h.ImportDate)<='{to}'";
					isFirstClaus = false;
				}
				
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY h.[CreateDate]";
				}
				if(paging is not null)
				{
					query += $" OFFSET {dataPaging.FirstRowNumber} ROWS FETCH NEXT {dataPaging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();

			}
		}
		public static int CountHistoryFGHeaderData(DateTime? from, DateTime? to)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM (
                               select distinct h.* from [stats].[__CRP_HistoryFG_Header] h inner join [stats].[__CRP_HistoryFG_Details] d on h.Id=d.HeaderId";
				var isFirstClaus = true;
				if(from is not null)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} CONVERT(date,h.ImportDate)>='{from}'";
					isFirstClaus = false;
				}
				if(to is not null)
				{
					query += $"{(isFirstClaus ? " WHERE" : " AND")} CONVERT(date,h.ImportDate)<='{to}'";
					isFirstClaus = false;
				}
				query += " ) A";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity GetDetailsAnalysis(int id, DateTime From, DateTime To)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [stats].[__CRP_HistoryFG_Header]
			WHERE CONVERT(date, CreateDate) BETWEEN CONVERT(date, '{From.ToString("yyyyMMdd")}') AND CONVERT(date,'{To.ToString("yyyyMMdd")}')
               and [CustomerNumber]=@Id
			";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity> GetHistoryFGHeaderAgentData()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "select distinct h.* from [stats].[__CRP_HistoryFG_Header] h inner join [stats].[__CRP_HistoryFG_Details] d on h.Id=d.HeaderId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);

			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.HistoryHeaderFGBestandEntity>();

			}
		}
		public static DateTime? GetHistorieFGAgentLastExcutionTime()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT MAX(ImportDate) FROM [stats].[__CRP_HistoryFG_Header] WHERE ImportType in (0,1,2)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return DateTime.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out DateTime date) ? date : null;
			}
		}
		public static DateTime? GetHistorieFGForcedAgentLastExcutionTime()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT MAX(ImportDate) FROM [stats].[__CRP_HistoryFG_Header] WHERE ImportType in (2)";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return DateTime.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out DateTime date) ? date : null;
			}
		}
		#region FG Bestand Historie
		public static int FGHistorieRefreshData(int CreateUserId, string CreatedUserName, int ImportType)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("usp_crp_historie_fg_bestand", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandTimeout = 400;
				sqlCommand.Parameters.AddWithValue("UserId", CreateUserId);
				sqlCommand.Parameters.AddWithValue("Username", CreatedUserName);
				sqlCommand.Parameters.AddWithValue("ImportType", ImportType);


				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion FG Bestand Historie


		#endregion Custom Methods
	}
}