using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class __bsd_pm_CablesAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__bsd_pm_Cables] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__bsd_pm_Cables]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__bsd_pm_Cables] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__bsd_pm_Cables] ([ArticleCustomerNumber],[ArticleId],[ArticleNumber],[CreationUserId],[CreationUsername],[ProjectId],[ResponsibleUserId],[ResponsibleUsername],[Status],[StatusId]) OUTPUT INSERTED.[Id] VALUES (@ArticleCustomerNumber,@ArticleId,@ArticleNumber,@CreationUserId,@CreationUsername,@ProjectId,@ResponsibleUserId,@ResponsibleUsername,@Status,@StatusId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber", item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUsername", item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
					sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUserId", item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUsername", item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items)
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
						query += " INSERT INTO [__bsd_pm_Cables] ([ArticleCustomerNumber],[ArticleId],[ArticleNumber],[CreationUserId],[CreationUsername],[ProjectId],[ResponsibleUserId],[ResponsibleUsername],[Status],[StatusId]) VALUES ( "

							+ "@ArticleCustomerNumber" + i + ","
							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUsername" + i + ","
							+ "@ProjectId" + i + ","
							+ "@ResponsibleUserId" + i + ","
							+ "@ResponsibleUsername" + i + ","
							+ "@Status" + i + ","
							+ "@StatusId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber" + i, item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUsername" + i, item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ResponsibleUserId" + i, item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
						sqlCommand.Parameters.AddWithValue("ResponsibleUsername" + i, item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__bsd_pm_Cables] SET [ArticleCustomerNumber]=@ArticleCustomerNumber, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CreationUserId]=@CreationUserId, [CreationUsername]=@CreationUsername, [ProjectId]=@ProjectId, [ResponsibleUserId]=@ResponsibleUserId, [ResponsibleUsername]=@ResponsibleUsername, [Status]=@Status, [StatusId]=@StatusId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber", item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUsername", item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
				sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
				sqlCommand.Parameters.AddWithValue("ResponsibleUserId", item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
				sqlCommand.Parameters.AddWithValue("ResponsibleUsername", item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items)
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
						query += " UPDATE [__bsd_pm_Cables] SET "

							+ "[ArticleCustomerNumber]=@ArticleCustomerNumber" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUsername]=@CreationUsername" + i + ","
							+ "[ProjectId]=@ProjectId" + i + ","
							+ "[ResponsibleUserId]=@ResponsibleUserId" + i + ","
							+ "[ResponsibleUsername]=@ResponsibleUsername" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[StatusId]=@StatusId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber" + i, item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUsername" + i, item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
						sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
						sqlCommand.Parameters.AddWithValue("ResponsibleUserId" + i, item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
						sqlCommand.Parameters.AddWithValue("ResponsibleUsername" + i, item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
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
				string query = "DELETE FROM [__bsd_pm_Cables] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__bsd_pm_Cables] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__bsd_pm_Cables] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__bsd_pm_Cables]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__bsd_pm_Cables] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__bsd_pm_Cables] ([ArticleCustomerNumber],[ArticleId],[ArticleNumber],[CreationUserId],[CreationUsername],[ProjectId],[ResponsibleUserId],[ResponsibleUsername],[Status],[StatusId]) OUTPUT INSERTED.[Id] VALUES (@ArticleCustomerNumber,@ArticleId,@ArticleNumber,@CreationUserId,@CreationUsername,@ProjectId,@ResponsibleUserId,@ResponsibleUsername,@Status,@StatusId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber", item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUsername", item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
			sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
			sqlCommand.Parameters.AddWithValue("ResponsibleUserId", item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
			sqlCommand.Parameters.AddWithValue("ResponsibleUsername", item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__bsd_pm_Cables] ([ArticleCustomerNumber],[ArticleId],[ArticleNumber],[CreationUserId],[CreationUsername],[ProjectId],[ResponsibleUserId],[ResponsibleUsername],[Status],[StatusId]) VALUES ( "

						+ "@ArticleCustomerNumber" + i + ","
						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreationUsername" + i + ","
						+ "@ProjectId" + i + ","
						+ "@ResponsibleUserId" + i + ","
						+ "@ResponsibleUsername" + i + ","
						+ "@Status" + i + ","
						+ "@StatusId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber" + i, item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUsername" + i, item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
					sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUserId" + i, item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUsername" + i, item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__bsd_pm_Cables] SET [ArticleCustomerNumber]=@ArticleCustomerNumber, [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CreationUserId]=@CreationUserId, [CreationUsername]=@CreationUsername, [ProjectId]=@ProjectId, [ResponsibleUserId]=@ResponsibleUserId, [ResponsibleUsername]=@ResponsibleUsername, [Status]=@Status, [StatusId]=@StatusId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber", item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationUsername", item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
			sqlCommand.Parameters.AddWithValue("ProjectId", item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
			sqlCommand.Parameters.AddWithValue("ResponsibleUserId", item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
			sqlCommand.Parameters.AddWithValue("ResponsibleUsername", item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 11; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__bsd_pm_Cables] SET "

					+ "[ArticleCustomerNumber]=@ArticleCustomerNumber" + i + ","
					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[CreationUsername]=@CreationUsername" + i + ","
					+ "[ProjectId]=@ProjectId" + i + ","
					+ "[ResponsibleUserId]=@ResponsibleUserId" + i + ","
					+ "[ResponsibleUsername]=@ResponsibleUsername" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[StatusId]=@StatusId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleCustomerNumber" + i, item.ArticleCustomerNumber == null ? (object)DBNull.Value : item.ArticleCustomerNumber);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUsername" + i, item.CreationUsername == null ? (object)DBNull.Value : item.CreationUsername);
					sqlCommand.Parameters.AddWithValue("ProjectId" + i, item.ProjectId == null ? (object)DBNull.Value : item.ProjectId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUserId" + i, item.ResponsibleUserId == null ? (object)DBNull.Value : item.ResponsibleUserId);
					sqlCommand.Parameters.AddWithValue("ResponsibleUsername" + i, item.ResponsibleUsername == null ? (object)DBNull.Value : item.ResponsibleUsername);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__bsd_pm_Cables] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__bsd_pm_Cables] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity> GetByByProjectAndStatus(int projectId, int? statusId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__bsd_pm_Cables] WHERE [ProjectId]=@projectId {(statusId.HasValue? $"AND [StatusId]={statusId}" : "")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("projectId", projectId);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_CablesEntity>();
			}
		}

		#endregion Custom Methods

	}
}
