using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests
{
	public class __BSD_Offer_Request_LogsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_Offer_Request_Logs] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_Offer_Request_Logs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_Offer_Request_Logs] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_Offer_Request_Logs] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId],[ManufacturerNumber],[SupplierContactName]) OUTPUT INSERTED.[Id] VALUES (@LastUpdateTime,@LastUpdateUserFullName,@LastUpdateUserId,@LastUpdateUsername,@LogDescription,@LogObject,@LogObjectId,@ManufacturerNumber,@SupplierContactName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("SupplierContactName", item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items)
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
						query += " INSERT INTO [__BSD_Offer_Request_Logs] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId],[ManufacturerNumber],[SupplierContactName]) VALUES ( "

							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserFullName" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@LastUpdateUsername" + i + ","
							+ "@LogDescription" + i + ","
							+ "@LogObject" + i + ","
							+ "@LogObjectId" + i + ","
							+ "@ManufacturerNumber" + i + ","
							+ "@SupplierContactName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
						sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
						sqlCommand.Parameters.AddWithValue("SupplierContactName" + i, item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_Offer_Request_Logs] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserFullName]=@LastUpdateUserFullName, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId, [ManufacturerNumber]=@ManufacturerNumber, [SupplierContactName]=@SupplierContactName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
				sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
				sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
				sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
				sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
				sqlCommand.Parameters.AddWithValue("SupplierContactName", item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items)
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
						query += " UPDATE [__BSD_Offer_Request_Logs] SET "

							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserFullName]=@LastUpdateUserFullName" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[LastUpdateUsername]=@LastUpdateUsername" + i + ","
							+ "[LogDescription]=@LogDescription" + i + ","
							+ "[LogObject]=@LogObject" + i + ","
							+ "[LogObjectId]=@LogObjectId" + i + ","
							+ "[ManufacturerNumber]=@ManufacturerNumber" + i + ","
							+ "[SupplierContactName]=@SupplierContactName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
						sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
						sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
						sqlCommand.Parameters.AddWithValue("SupplierContactName" + i, item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);
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
				string query = "DELETE FROM [__BSD_Offer_Request_Logs] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_Offer_Request_Logs] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_Offer_Request_Logs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_Offer_Request_Logs]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_Offer_Request_Logs] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_Offer_Request_Logs] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId],[ManufacturerNumber],[SupplierContactName]) OUTPUT INSERTED.[Id] VALUES (@LastUpdateTime,@LastUpdateUserFullName,@LastUpdateUserId,@LastUpdateUsername,@LogDescription,@LogObject,@LogObjectId,@ManufacturerNumber,@SupplierContactName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
			sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
			sqlCommand.Parameters.AddWithValue("SupplierContactName", item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_Offer_Request_Logs] ([LastUpdateTime],[LastUpdateUserFullName],[LastUpdateUserId],[LastUpdateUsername],[LogDescription],[LogObject],[LogObjectId],[ManufacturerNumber],[SupplierContactName]) VALUES ( "

						+ "@LastUpdateTime" + i + ","
						+ "@LastUpdateUserFullName" + i + ","
						+ "@LastUpdateUserId" + i + ","
						+ "@LastUpdateUsername" + i + ","
						+ "@LogDescription" + i + ","
						+ "@LogObject" + i + ","
						+ "@LogObjectId" + i + ","
						+ "@ManufacturerNumber" + i + ","
						+ "@SupplierContactName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("SupplierContactName" + i, item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_Offer_Request_Logs] SET [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserFullName]=@LastUpdateUserFullName, [LastUpdateUserId]=@LastUpdateUserId, [LastUpdateUsername]=@LastUpdateUsername, [LogDescription]=@LogDescription, [LogObject]=@LogObject, [LogObjectId]=@LogObjectId, [ManufacturerNumber]=@ManufacturerNumber, [SupplierContactName]=@SupplierContactName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName", item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
			sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
			sqlCommand.Parameters.AddWithValue("LastUpdateUsername", item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
			sqlCommand.Parameters.AddWithValue("LogDescription", item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogObjectId", item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
			sqlCommand.Parameters.AddWithValue("ManufacturerNumber", item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
			sqlCommand.Parameters.AddWithValue("SupplierContactName", item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_Offer_Request_Logs] SET "

					+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
					+ "[LastUpdateUserFullName]=@LastUpdateUserFullName" + i + ","
					+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
					+ "[LastUpdateUsername]=@LastUpdateUsername" + i + ","
					+ "[LogDescription]=@LogDescription" + i + ","
					+ "[LogObject]=@LogObject" + i + ","
					+ "[LogObjectId]=@LogObjectId" + i + ","
					+ "[ManufacturerNumber]=@ManufacturerNumber" + i + ","
					+ "[SupplierContactName]=@SupplierContactName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserFullName" + i, item.LastUpdateUserFullName == null ? (object)DBNull.Value : item.LastUpdateUserFullName);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("LastUpdateUsername" + i, item.LastUpdateUsername == null ? (object)DBNull.Value : item.LastUpdateUsername);
					sqlCommand.Parameters.AddWithValue("LogDescription" + i, item.LogDescription == null ? (object)DBNull.Value : item.LogDescription);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogObjectId" + i, item.LogObjectId == null ? (object)DBNull.Value : item.LogObjectId);
					sqlCommand.Parameters.AddWithValue("ManufacturerNumber" + i, item.ManufacturerNumber == null ? (object)DBNull.Value : item.ManufacturerNumber);
					sqlCommand.Parameters.AddWithValue("SupplierContactName" + i, item.SupplierContactName == null ? (object)DBNull.Value : item.SupplierContactName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_Offer_Request_Logs] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_Offer_Request_Logs] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsWithTotalCountEntity> GetAll(Settings.PaginModel paging,string search)
		{

			string pagingSorting = "";
			string searchText = "";
			
			if(paging != null && (0 >= paging.RequestRows || paging.RequestRows > 100))
				paging.RequestRows = 100;

			if(paging != null)
			{
				pagingSorting += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT Count(*) over() TotalCount, * FROM [__BSD_Offer_Request_Logs]   ";
				

				if(search != "")
				{
					searchText = @"
						  where
						LastUpdateUserFullName like @search 
						or ManufacturerNumber like @search
						or LogDescription like @search
						or SupplierContactName like @search
						or LogObject like @search order by LastUpdateTime desc
					" + pagingSorting;
					query += searchText;
					var sqlCommand = new SqlCommand(query, sqlConnection);
					sqlCommand.Parameters.AddWithValue("@search", $"%{search}%");
					DbExecution.Fill(sqlCommand, dataTable);
				}
				else
				{
					query += " order by LastUpdateTime desc ";
					query += pagingSorting;
					var sqlCommand = new SqlCommand(query, sqlConnection);
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsWithTotalCountEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsWithTotalCountEntity>();
			}
		}
		#endregion Custom Methods

	}
}
