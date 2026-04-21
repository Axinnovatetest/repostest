using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Linq;

namespace Infrastructure.Data.Access.Tables
{
	public class ArticleMinStockUpdateHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> GetByPage(Data.Access.Settings.SortingModel sorting, Data.Access.Settings.PaginModel paging)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory]";

				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [Id] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
			}
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_ArticleMinStockUpdateHistory] ([ArticleId],[ArticleNumber],[LagerId],[NewMinStock],[OldMinStock],[UpdateDate],[UpdateUserId],[UpdateUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@LagerId,@NewMinStock,@OldMinStock,@UpdateDate,@UpdateUserId,@UpdateUserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("NewMinStock", item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
					sqlCommand.Parameters.AddWithValue("OldMinStock", item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
					sqlCommand.Parameters.AddWithValue("UpdateDate", item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
					sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateUserName", item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items)
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
						query += " INSERT INTO [__BSD_ArticleMinStockUpdateHistory] ([ArticleId],[ArticleNumber],[LagerId],[NewMinStock],[OldMinStock],[UpdateDate],[UpdateUserId],[UpdateUserName]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@LagerId" + i + ","
							+ "@NewMinStock" + i + ","
							+ "@OldMinStock" + i + ","
							+ "@UpdateDate" + i + ","
							+ "@UpdateUserId" + i + ","
							+ "@UpdateUserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
						sqlCommand.Parameters.AddWithValue("NewMinStock" + i, item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
						sqlCommand.Parameters.AddWithValue("OldMinStock" + i, item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
						sqlCommand.Parameters.AddWithValue("UpdateDate" + i, item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
						sqlCommand.Parameters.AddWithValue("UpdateUserName" + i, item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArticleMinStockUpdateHistory] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [LagerId]=@LagerId, [NewMinStock]=@NewMinStock, [OldMinStock]=@OldMinStock, [UpdateDate]=@UpdateDate, [UpdateUserId]=@UpdateUserId, [UpdateUserName]=@UpdateUserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
				sqlCommand.Parameters.AddWithValue("NewMinStock", item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
				sqlCommand.Parameters.AddWithValue("OldMinStock", item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
				sqlCommand.Parameters.AddWithValue("UpdateDate", item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
				sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
				sqlCommand.Parameters.AddWithValue("UpdateUserName", item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items)
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
						query += " UPDATE [__BSD_ArticleMinStockUpdateHistory] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[NewMinStock]=@NewMinStock" + i + ","
							+ "[OldMinStock]=@OldMinStock" + i + ","
							+ "[UpdateDate]=@UpdateDate" + i + ","
							+ "[UpdateUserId]=@UpdateUserId" + i + ","
							+ "[UpdateUserName]=@UpdateUserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
						sqlCommand.Parameters.AddWithValue("NewMinStock" + i, item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
						sqlCommand.Parameters.AddWithValue("OldMinStock" + i, item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
						sqlCommand.Parameters.AddWithValue("UpdateDate" + i, item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
						sqlCommand.Parameters.AddWithValue("UpdateUserName" + i, item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);
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
				string query = "DELETE FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_ArticleMinStockUpdateHistory] ([ArticleId],[ArticleNumber],[LagerId],[NewMinStock],[OldMinStock],[UpdateDate],[UpdateUserId],[UpdateUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@LagerId,@NewMinStock,@OldMinStock,@UpdateDate,@UpdateUserId,@UpdateUserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
			sqlCommand.Parameters.AddWithValue("NewMinStock", item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
			sqlCommand.Parameters.AddWithValue("OldMinStock", item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
			sqlCommand.Parameters.AddWithValue("UpdateDate", item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
			sqlCommand.Parameters.AddWithValue("UpdateUserName", item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_ArticleMinStockUpdateHistory] ([ArticleId],[ArticleNumber],[LagerId],[NewMinStock],[OldMinStock],[UpdateDate],[UpdateUserId],[UpdateUserName]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@LagerId" + i + ","
						+ "@NewMinStock" + i + ","
						+ "@OldMinStock" + i + ","
						+ "@UpdateDate" + i + ","
						+ "@UpdateUserId" + i + ","
						+ "@UpdateUserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("NewMinStock" + i, item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
					sqlCommand.Parameters.AddWithValue("OldMinStock" + i, item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
					sqlCommand.Parameters.AddWithValue("UpdateDate" + i, item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateUserName" + i, item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_ArticleMinStockUpdateHistory] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [LagerId]=@LagerId, [NewMinStock]=@NewMinStock, [OldMinStock]=@OldMinStock, [UpdateDate]=@UpdateDate, [UpdateUserId]=@UpdateUserId, [UpdateUserName]=@UpdateUserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
			sqlCommand.Parameters.AddWithValue("NewMinStock", item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
			sqlCommand.Parameters.AddWithValue("OldMinStock", item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
			sqlCommand.Parameters.AddWithValue("UpdateDate", item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
			sqlCommand.Parameters.AddWithValue("UpdateUserName", item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_ArticleMinStockUpdateHistory] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[NewMinStock]=@NewMinStock" + i + ","
					+ "[OldMinStock]=@OldMinStock" + i + ","
					+ "[UpdateDate]=@UpdateDate" + i + ","
					+ "[UpdateUserId]=@UpdateUserId" + i + ","
					+ "[UpdateUserName]=@UpdateUserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("NewMinStock" + i, item.NewMinStock == null ? (object)DBNull.Value : item.NewMinStock);
					sqlCommand.Parameters.AddWithValue("OldMinStock" + i, item.OldMinStock == null ? (object)DBNull.Value : item.OldMinStock);
					sqlCommand.Parameters.AddWithValue("UpdateDate" + i, item.UpdateDate == null ? (object)DBNull.Value : item.UpdateDate);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateUserName" + i, item.UpdateUserName == null ? (object)DBNull.Value : item.UpdateUserName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> GetByArtikelNrs(List<int> artikelNrs)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
				if(artikelNrs.Count <= maxQueryNumber)
				{
					result = getByArtikelNrs(artikelNrs);
				}
				else
				{
					int batchNumber = artikelNrs.Count / maxQueryNumber;
					result = new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByArtikelNrs(artikelNrs.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByArtikelNrs(artikelNrs.GetRange(batchNumber * maxQueryNumber, artikelNrs.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}
		private static List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> getByArtikelNrs(List<int> artikelNrs)
		{
			if(artikelNrs != null && artikelNrs.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < artikelNrs.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, artikelNrs[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory] WHERE [ArticleId] IN (" + queryIds + ") Order By [UpdateDate] DESC";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
				}
			}
			return new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
		}

		public static int Get_Count(string articleNumber, DateTime? from = null, DateTime? to = null)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				List<string> where = new List<string>();
				string query = "SELECT COUNT(*) FROM [__BSD_ArticleMinStockUpdateHistory]";

				// -
				if(!string.IsNullOrWhiteSpace(articleNumber))
					where.Add($"[ArticleNumber] LIKE '{articleNumber.Trim()}%'");
				if(from.HasValue)
					where.Add($"[UpdateDate]>='{from.Value.ToString("yyyyMMdd")}'");
				if(to.HasValue)
					where.Add($"[UpdateDate]<='{to.Value.ToString("yyyyMMdd")}'");

				if(where.Count > 0)
					query += $" WHERE {string.Join(" AND ", where)}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var v) ? v : 0;
			}
		}


		public static List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity> Get(Settings.SortingModel sorting, Settings.PaginModel paging, string articleNumber, DateTime? from = null, DateTime? to = null)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				List<string> where = new List<string>();
				string query = "SELECT * FROM [__BSD_ArticleMinStockUpdateHistory]";

				// -
				if(!string.IsNullOrWhiteSpace(articleNumber))
					where.Add($"[ArticleNumber] LIKE '{articleNumber.Trim()}%'");
				if(from.HasValue)
					where.Add($"[UpdateDate]>='{from.Value.ToString("yyyyMMdd")}'");
				if(to.HasValue)
					where.Add($"[UpdateDate]<='{to.Value.ToString("yyyyMMdd")}'");

				if(where.Count > 0)
					query += $" WHERE {string.Join(" AND ", where)}";

				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [UpdateDate] DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				#endregion pagination

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity>();
			}
		}

		#endregion Custom Methods

	}
}
