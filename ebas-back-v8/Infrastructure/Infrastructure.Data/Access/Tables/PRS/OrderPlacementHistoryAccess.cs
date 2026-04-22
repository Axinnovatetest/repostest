using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderPlacementHistoryAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_OrderPlacementHistory] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_OrderPlacementHistory]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__PRS_OrderPlacementHistory] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__PRS_OrderPlacementHistory] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[OrderId],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail]) OUTPUT INSERTED.[Id] VALUES (@AttachmentIds,@CCEmails,@EmailMessage,@EmailTitle,@OrderId,@SenderCC,@SenderUserEmail,@SenderUserId,@SenderUserName,@SendingTime,@ToEmail); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
					sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
					sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
					sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
					sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
					sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
					sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
					sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
					sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
					sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items)
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
						query += " INSERT INTO [__PRS_OrderPlacementHistory] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[OrderId],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail]) VALUES ( "

							+ "@AttachmentIds" + i + ","
							+ "@CCEmails" + i + ","
							+ "@EmailMessage" + i + ","
							+ "@EmailTitle" + i + ","
							+ "@OrderId" + i + ","
							+ "@SenderCC" + i + ","
							+ "@SenderUserEmail" + i + ","
							+ "@SenderUserId" + i + ","
							+ "@SenderUserName" + i + ","
							+ "@SendingTime" + i + ","
							+ "@ToEmail" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
						sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
						sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
						sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
						sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
						sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
						sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
						sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
						sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__PRS_OrderPlacementHistory] SET [AttachmentIds]=@AttachmentIds, [CCEmails]=@CCEmails, [EmailMessage]=@EmailMessage, [EmailTitle]=@EmailTitle, [OrderId]=@OrderId, [SenderCC]=@SenderCC, [SenderUserEmail]=@SenderUserEmail, [SenderUserId]=@SenderUserId, [SenderUserName]=@SenderUserName, [SendingTime]=@SendingTime, [ToEmail]=@ToEmail WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
				sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
				sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
				sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
				sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
				sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
				sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
				sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
				sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
				sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
				sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items)
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
						query += " UPDATE [__PRS_OrderPlacementHistory] SET "

							+ "[AttachmentIds]=@AttachmentIds" + i + ","
							+ "[CCEmails]=@CCEmails" + i + ","
							+ "[EmailMessage]=@EmailMessage" + i + ","
							+ "[EmailTitle]=@EmailTitle" + i + ","
							+ "[OrderId]=@OrderId" + i + ","
							+ "[SenderCC]=@SenderCC" + i + ","
							+ "[SenderUserEmail]=@SenderUserEmail" + i + ","
							+ "[SenderUserId]=@SenderUserId" + i + ","
							+ "[SenderUserName]=@SenderUserName" + i + ","
							+ "[SendingTime]=@SendingTime" + i + ","
							+ "[ToEmail]=@ToEmail" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
						sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
						sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
						sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
						sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
						sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
						sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
						sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
						sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
						sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
						sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
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
				string query = "DELETE FROM [__PRS_OrderPlacementHistory] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__PRS_OrderPlacementHistory] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_OrderPlacementHistory] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__PRS_OrderPlacementHistory]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__PRS_OrderPlacementHistory] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__PRS_OrderPlacementHistory] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[OrderId],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail]) OUTPUT INSERTED.[Id] VALUES (@AttachmentIds,@CCEmails,@EmailMessage,@EmailTitle,@OrderId,@SenderCC,@SenderUserEmail,@SenderUserId,@SenderUserName,@SendingTime,@ToEmail); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
			sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
			sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
			sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
			sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
			sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
			sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
			sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
			sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__PRS_OrderPlacementHistory] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[OrderId],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail]) VALUES ( "

						+ "@AttachmentIds" + i + ","
						+ "@CCEmails" + i + ","
						+ "@EmailMessage" + i + ","
						+ "@EmailTitle" + i + ","
						+ "@OrderId" + i + ","
						+ "@SenderCC" + i + ","
						+ "@SenderUserEmail" + i + ","
						+ "@SenderUserId" + i + ","
						+ "@SenderUserName" + i + ","
						+ "@SendingTime" + i + ","
						+ "@ToEmail" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
					sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
					sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
					sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
					sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
					sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
					sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
					sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
					sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
					sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__PRS_OrderPlacementHistory] SET [AttachmentIds]=@AttachmentIds, [CCEmails]=@CCEmails, [EmailMessage]=@EmailMessage, [EmailTitle]=@EmailTitle, [OrderId]=@OrderId, [SenderCC]=@SenderCC, [SenderUserEmail]=@SenderUserEmail, [SenderUserId]=@SenderUserId, [SenderUserName]=@SenderUserName, [SendingTime]=@SendingTime, [ToEmail]=@ToEmail WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
			sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
			sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
			sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
			sqlCommand.Parameters.AddWithValue("OrderId", item.OrderId);
			sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
			sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
			sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
			sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
			sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
			sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__PRS_OrderPlacementHistory] SET "

					+ "[AttachmentIds]=@AttachmentIds" + i + ","
					+ "[CCEmails]=@CCEmails" + i + ","
					+ "[EmailMessage]=@EmailMessage" + i + ","
					+ "[EmailTitle]=@EmailTitle" + i + ","
					+ "[OrderId]=@OrderId" + i + ","
					+ "[SenderCC]=@SenderCC" + i + ","
					+ "[SenderUserEmail]=@SenderUserEmail" + i + ","
					+ "[SenderUserId]=@SenderUserId" + i + ","
					+ "[SenderUserName]=@SenderUserName" + i + ","
					+ "[SendingTime]=@SendingTime" + i + ","
					+ "[ToEmail]=@ToEmail" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
					sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
					sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
					sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
					sqlCommand.Parameters.AddWithValue("OrderId" + i, item.OrderId);
					sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
					sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
					sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
					sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
					sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
					sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__PRS_OrderPlacementHistory] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__PRS_OrderPlacementHistory] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity> GetByOrderId(int orderId)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__PRS_OrderPlacementHistory] WHERE [OrderId] = @orderId ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.OrderPlacementHistoryEntity>();
			}
		}

		public static int GetCountByOrderId(int orderId)
		{

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT count (*) FROM [__PRS_OrderPlacementHistory] WHERE [OrderId] = @orderId ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("orderId", orderId);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var number) ? number : 0;
			}
		}
		public static IEnumerable<KeyValuePair<int, int>> GetCountByOrderIds(IEnumerable<int> orderIds)
		{
			if(orderIds is null || orderIds.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT [OrderId], count (*) c FROM [__PRS_OrderPlacementHistory] WHERE [OrderId] IN ({string.Join(",", orderIds)}) GROUP BY [OrderId]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(
					int.TryParse(x[0].ToString(), out var i) ? i : 0,
					int.TryParse(x[1].ToString(), out var c) ? c : 0));
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
