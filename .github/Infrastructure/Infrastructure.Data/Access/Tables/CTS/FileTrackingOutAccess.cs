using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Infrastructure.Data.Entities.Tables.CTS;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public static class FileTrackingOutAccess
	{
		#region Default Methods
		public static FileTrackingOutEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [FileTrackingOut] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new FileTrackingOutEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<FileTrackingOutEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [FileTrackingOut]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new FileTrackingOutEntity(x)).ToList();
			}
			else
			{
				return new List<FileTrackingOutEntity>();
			}
		}
		public static List<FileTrackingOutEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
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

					sqlCommand.CommandText = $"SELECT * FROM [FileTrackingOut] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new FileTrackingOutEntity(x)).ToList();
				}
				else
				{
					return new List<FileTrackingOutEntity>();
				}
			}
			return new List<FileTrackingOutEntity>();
		}
		public static List<FileTrackingOutEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.FileTrackingOutEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<FileTrackingOutEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<FileTrackingOutEntity>();
		}
		public static int Insert(FileTrackingOutEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [FileTrackingOut] ([FileName],[SentTime],[MsgType],[EbasGenerateTime],[CustomerNumber],[CustomerName],[DocumentNumber],[LastCheckStauts],[LastCheckTime]) OUTPUT INSERTED.[Id] VALUES (@FileName,@SentTime,@MsgType,@EbasGenerateTime,@CustomerNumber,@CustomerName,@DocumentNumber,@LastCheckStauts,@LastCheckTime); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);
					sqlCommand.Parameters.AddWithValue("MsgType", item.MsgType == null ? (object)DBNull.Value : item.MsgType);
					sqlCommand.Parameters.AddWithValue("EbasGenerateTime", item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("LastCheckStauts", item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
					sqlCommand.Parameters.AddWithValue("LastCheckTime", item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<FileTrackingOutEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [FileTrackingOut] ([FileName],[SentTime],[MsgType],[EbasGenerateTime],[CustomerNumber],[CustomerName],[DocumentNumber],[LastCheckStauts],[LastCheckTime]) VALUES ("
							+ "@FileName" + i +
							 ","
							+ "@SentTime" + i +
							 ","
							+ "@MsgType" + i +
							 ","
							+ "@EbasGenerateTime" + i +
							 ","
							+ "@CustomerNumber" + i +
							 ","
							+ "@CustomerName" + i +
							 ","
							+ "@DocumentNumber" + i +
							 ","
							+ "@LastCheckStauts" + i +
							 ","
							+ "@LastCheckTime" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
						sqlCommand.Parameters.AddWithValue("MsgType" + i, item.MsgType == null ? (object)DBNull.Value : item.MsgType);
						sqlCommand.Parameters.AddWithValue("EbasGenerateTime" + i, item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("LastCheckStauts" + i, item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
						sqlCommand.Parameters.AddWithValue("LastCheckTime" + i, item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<FileTrackingOutEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10;
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
		public static int Update(FileTrackingOutEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [FileTrackingOut] SET [FileName] = @FileName, [SentTime] = @SentTime, [MsgType] = @MsgType, [EbasGenerateTime] = @EbasGenerateTime, [CustomerNumber] = @CustomerNumber, [CustomerName] = @CustomerName, [DocumentNumber] = @DocumentNumber, [LastCheckStauts] = @LastCheckStauts, [LastCheckTime] = @LastCheckTime WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);
					sqlCommand.Parameters.AddWithValue("MsgType", item.MsgType == null ? (object)DBNull.Value : item.MsgType);
					sqlCommand.Parameters.AddWithValue("EbasGenerateTime", item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("LastCheckStauts", item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
					sqlCommand.Parameters.AddWithValue("LastCheckTime", item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<FileTrackingOutEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [FileTrackingOut] SET "
						  + "[FileName]=@FileName" + i +
						   ","
						  + "[SentTime]=@SentTime" + i +
						   ","
						  + "[MsgType]=@MsgType" + i +
						   ","
						  + "[EbasGenerateTime]=@EbasGenerateTime" + i +
						   ","
						  + "[CustomerNumber]=@CustomerNumber" + i +
						   ","
						  + "[CustomerName]=@CustomerName" + i +
						   ","
						  + "[DocumentNumber]=@DocumentNumber" + i +
						   ","
						  + "[LastCheckStauts]=@LastCheckStauts" + i +
						   ","
						  + "[LastCheckTime]=@LastCheckTime" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
						sqlCommand.Parameters.AddWithValue("MsgType" + i, item.MsgType == null ? (object)DBNull.Value : item.MsgType);
						sqlCommand.Parameters.AddWithValue("EbasGenerateTime" + i, item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("LastCheckStauts" + i, item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
						sqlCommand.Parameters.AddWithValue("LastCheckTime" + i, item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<FileTrackingOutEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10;
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
		public static int Delete(int id)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [FileTrackingOut] WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", id);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int delete(List<int> ids)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string queryIds = string.Join(",", Enumerable.Range(0, ids.Count).Select(i => "@Id" + i));
				string query = "DELETE FROM [FileTrackingOut] WHERE [Id] IN (" + queryIds + ")";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
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

				return results;
			}
			else
			{
				return -1;
			}
		}
		#region Transaction Methods
		public static FileTrackingOutEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [FileTrackingOut] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return new FileTrackingOutEntity(dataTable.Rows[0]);
			else
				return null;
		}
		public static List<FileTrackingOutEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [FileTrackingOut]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new FileTrackingOutEntity(x)).ToList();
			else
				return new List<FileTrackingOutEntity>();
		}
		public static List<FileTrackingOutEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = "SELECT * FROM [FileTrackingOut] WHERE [Id] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new FileTrackingOutEntity(x)).ToList();
				else
					return new List<FileTrackingOutEntity>();
			}
			return new List<FileTrackingOutEntity>();
		}
		public static List<FileTrackingOutEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<FileTrackingOutEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<FileTrackingOutEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<FileTrackingOutEntity>();
		}
		public static int InsertWithTransaction(FileTrackingOutEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [FileTrackingOut] ([FileName],[SentTime],[MsgType],[EbasGenerateTime],[CustomerNumber],[CustomerName],[DocumentNumber],[LastCheckStauts],[LastCheckTime]) OUTPUT INSERTED.[Id] VALUES (@FileName,@SentTime,@MsgType,@EbasGenerateTime,@CustomerNumber,@CustomerName,@DocumentNumber,@LastCheckStauts,@LastCheckTime); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
				sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);
				sqlCommand.Parameters.AddWithValue("MsgType", item.MsgType == null ? (object)DBNull.Value : item.MsgType);
				sqlCommand.Parameters.AddWithValue("EbasGenerateTime", item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
				sqlCommand.Parameters.AddWithValue("LastCheckStauts", item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
				sqlCommand.Parameters.AddWithValue("LastCheckTime", item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<FileTrackingOutEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [FileTrackingOut] ([FileName],[SentTime],[MsgType],[EbasGenerateTime],[CustomerNumber],[CustomerName],[DocumentNumber],[LastCheckStauts],[LastCheckTime]) VALUES ("
							+ "@FileName" + i +
							 ","
							+ "@SentTime" + i +
							 ","
							+ "@MsgType" + i +
							 ","
							+ "@EbasGenerateTime" + i +
							 ","
							+ "@CustomerNumber" + i +
							 ","
							+ "@CustomerName" + i +
							 ","
							+ "@DocumentNumber" + i +
							 ","
							+ "@LastCheckStauts" + i +
							 ","
							+ "@LastCheckTime" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
						sqlCommand.Parameters.AddWithValue("MsgType" + i, item.MsgType == null ? (object)DBNull.Value : item.MsgType);
						sqlCommand.Parameters.AddWithValue("EbasGenerateTime" + i, item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
						sqlCommand.Parameters.AddWithValue("LastCheckStauts" + i, item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
						sqlCommand.Parameters.AddWithValue("LastCheckTime" + i, item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<FileTrackingOutEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insertWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
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
		public static int UpdateWithTransaction(FileTrackingOutEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [FileTrackingOut] SET [FileName] = @FileName, [SentTime] = @SentTime, [MsgType] = @MsgType, [EbasGenerateTime] = @EbasGenerateTime, [CustomerNumber] = @CustomerNumber, [CustomerName] = @CustomerName, [DocumentNumber] = @DocumentNumber, [LastCheckStauts] = @LastCheckStauts, [LastCheckTime] = @LastCheckTime WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
			sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);
			sqlCommand.Parameters.AddWithValue("MsgType", item.MsgType == null ? (object)DBNull.Value : item.MsgType);
			sqlCommand.Parameters.AddWithValue("EbasGenerateTime", item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("DocumentNumber", item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
			sqlCommand.Parameters.AddWithValue("LastCheckStauts", item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
			sqlCommand.Parameters.AddWithValue("LastCheckTime", item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int updateWithTransaction(List<FileTrackingOutEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [FileTrackingOut] SET "
					  + "[FileName]=@FileName" + i +
					   ","
					  + "[SentTime]=@SentTime" + i +
					   ","
					  + "[MsgType]=@MsgType" + i +
					   ","
					  + "[EbasGenerateTime]=@EbasGenerateTime" + i +
					   ","
					  + "[CustomerNumber]=@CustomerNumber" + i +
					   ","
					  + "[CustomerName]=@CustomerName" + i +
					   ","
					  + "[DocumentNumber]=@DocumentNumber" + i +
					   ","
					  + "[LastCheckStauts]=@LastCheckStauts" + i +
					   ","
					  + "[LastCheckTime]=@LastCheckTime" + i +
					 " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
					sqlCommand.Parameters.AddWithValue("MsgType" + i, item.MsgType == null ? (object)DBNull.Value : item.MsgType);
					sqlCommand.Parameters.AddWithValue("EbasGenerateTime" + i, item.EbasGenerateTime == null ? (object)DBNull.Value : item.EbasGenerateTime);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("DocumentNumber" + i, item.DocumentNumber == null ? (object)DBNull.Value : item.DocumentNumber);
					sqlCommand.Parameters.AddWithValue("LastCheckStauts" + i, item.LastCheckStauts == null ? (object)DBNull.Value : item.LastCheckStauts);
					sqlCommand.Parameters.AddWithValue("LastCheckTime" + i, item.LastCheckTime == null ? (object)DBNull.Value : item.LastCheckTime);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<FileTrackingOutEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = updateWithTransaction(items, connection, transaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					results = 0;
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
		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [FileTrackingOut] WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Join(",", ids.Select((id, i) => "@Id" + i));
				sqlCommand.CommandText = $"DELETE FROM [FileTrackingOut] WHERE [Id] IN (" + queryIds + ")";
				for(int i = 0; i < ids.Count; i++)
				{
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				results = DbExecution.ExecuteNonQuery(sqlCommand);
				return results;
			}
			return -1;
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
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		#endregion Transaction Methods
		#endregion Default Methods

		#region Custom Methods
		public static void UpdateSentTime(string fileName, DateTime sentTime)
		{
			if(string.IsNullOrEmpty(fileName))
				throw new ArgumentException("fileName darf nicht null oder leer sein", nameof(fileName));
			using(var connection = new SqlConnection(Settings.ConnectionStringEdiPlatformCnxEBAS))
			{
				connection.Open();
				using(var sqlCommand = new SqlCommand(@"
			UPDATE [EdiPlatform].[dbo].[FileTrackingOut]
			SET SentTime = @SentTime
			WHERE FileName = @FileName", connection))
				{
					sqlCommand.Parameters.AddWithValue("@SentTime", sentTime);
					sqlCommand.Parameters.AddWithValue("@FileName", fileName);
					DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
		}

		#endregion Custom Methods
	}
}
