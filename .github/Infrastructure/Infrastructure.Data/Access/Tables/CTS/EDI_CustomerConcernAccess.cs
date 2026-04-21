using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class EDI_CustomerConcernAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern]", sqlConnection))
			{
				sqlConnection.Open();
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();

					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_CustomerConcern] WHERE [Id] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_CustomerConcern] ([ConcernName],[ConcernNumber],[CreationTime],[CreationUserId],[CreationUserName],[IncludeDescription],[LastEditUserId],[LastEditUserName],[TrimLeadingZeros]) OUTPUT INSERTED.[Id] VALUES (@ConcernName,@ConcernNumber,@CreationTime,@CreationUserId,@CreationUserName,@IncludeDescription,@LastEditUserId,@LastEditUserName,@TrimLeadingZeros); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ConcernName", item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
					sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
					sqlCommand.Parameters.AddWithValue("IncludeDescription", item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
					sqlCommand.Parameters.AddWithValue("TrimLeadingZeros", item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__EDI_CustomerConcern] ([ConcernName],[ConcernNumber],[CreationTime],[CreationUserId],[CreationUserName],[IncludeDescription],[LastEditUserId],[LastEditUserName],[TrimLeadingZeros]) VALUES ("

							+ "@ConcernName" + i + ","
							+ "@ConcernNumber" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@CreationUserName" + i + ","
							+ "@IncludeDescription" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@LastEditUserName" + i + ","
							+ "@TrimLeadingZeros" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ConcernName" + i, item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("IncludeDescription" + i, item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("TrimLeadingZeros" + i, item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
					}

					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_CustomerConcern] SET [ConcernName]=@ConcernName, [ConcernNumber]=@ConcernNumber, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [IncludeDescription]=@IncludeDescription, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [TrimLeadingZeros]=@TrimLeadingZeros WHERE [Id]=@Id";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ConcernName", item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("IncludeDescription", item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("TrimLeadingZeros", item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					string query = "";

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__EDI_CustomerConcern] SET "

							+ "[ConcernName]=@ConcernName" + i + ","
							+ "[ConcernNumber]=@ConcernNumber" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[CreationUserName]=@CreationUserName" + i + ","
							+ "[IncludeDescription]=@IncludeDescription" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[LastEditUserName]=@LastEditUserName" + i + ","
							+ "[TrimLeadingZeros]=@TrimLeadingZeros" + i + $" WHERE [Id]=@Id{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Id{i}", item.Id);

						sqlCommand.Parameters.AddWithValue("ConcernName" + i, item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("IncludeDescription" + i, item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("TrimLeadingZeros" + i, item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [__EDI_CustomerConcern] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				return DbExecution.ExecuteNonQuery(sqlCommand);
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
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				using(var sqlCommand = new SqlCommand("", sqlConnection))
				{
					sqlConnection.Open();
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [__EDI_CustomerConcern] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern] WHERE [Id] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern]", connection, transaction))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_CustomerConcern] WHERE [Id] IN ({string.Join(",", queryIds)})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO [__EDI_CustomerConcern] ([ConcernName],[ConcernNumber],[CreationTime],[CreationUserId],[CreationUserName],[IncludeDescription],[LastEditUserId],[LastEditUserName],[TrimLeadingZeros]) OUTPUT INSERTED.[Id] VALUES (@ConcernName,@ConcernNumber,@CreationTime,@CreationUserId,@CreationUserName,@IncludeDescription,@LastEditUserId,@LastEditUserName,@TrimLeadingZeros); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ConcernName", item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("IncludeDescription", item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("TrimLeadingZeros", item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "INSERT INTO [__EDI_CustomerConcern] ([ConcernName],[ConcernNumber],[CreationTime],[CreationUserId],[CreationUserName],[IncludeDescription],[LastEditUserId],[LastEditUserName],[TrimLeadingZeros]) VALUES ( "

						+ "@ConcernName" + i + ","
						+ "@ConcernNumber" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@CreationUserName" + i + ","
						+ "@IncludeDescription" + i + ","
						+ "@LastEditUserId" + i + ","
						+ "@LastEditUserName" + i + ","
						+ "@TrimLeadingZeros" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ConcernName" + i, item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("IncludeDescription" + i, item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("TrimLeadingZeros" + i, item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
					}

					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__EDI_CustomerConcern] SET [ConcernName]=@ConcernName, [ConcernNumber]=@ConcernNumber, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [CreationUserName]=@CreationUserName, [IncludeDescription]=@IncludeDescription, [LastEditUserId]=@LastEditUserId, [LastEditUserName]=@LastEditUserName, [TrimLeadingZeros]=@TrimLeadingZeros WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ConcernName", item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("IncludeDescription", item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("TrimLeadingZeros", item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += "UPDATE [__EDI_CustomerConcern] SET "

						+ "[ConcernName]=@ConcernName" + i + ","
						+ "[ConcernNumber]=@ConcernNumber" + i + ","
						+ "[CreationTime]=@CreationTime" + i + ","
						+ "[CreationUserId]=@CreationUserId" + i + ","
						+ "[CreationUserName]=@CreationUserName" + i + ","
						+ "[IncludeDescription]=@IncludeDescription" + i + ","
						+ "[LastEditUserId]=@LastEditUserId" + i + ","
						+ "[LastEditUserName]=@LastEditUserName" + i + ","
						+ "[TrimLeadingZeros]=@TrimLeadingZeros" + i + " WHERE [Id]=@Id" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);

						sqlCommand.Parameters.AddWithValue("ConcernName" + i, item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationUserName" + i, item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
						sqlCommand.Parameters.AddWithValue("IncludeDescription" + i, item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("LastEditUserName" + i, item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
						sqlCommand.Parameters.AddWithValue("TrimLeadingZeros" + i, item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
					}

					sqlCommand.CommandText = query;
					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [__EDI_CustomerConcern] WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
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
				using(var sqlCommand = new SqlCommand("", connection, transaction))
				{
					var queryIds = new List<string>();
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds.Add($"@Id{i}");
						sqlCommand.Parameters.AddWithValue($"Id{i}", ids[i]);
					}

					string query = $"DELETE FROM [__EDI_CustomerConcern] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
			return -1;
		}
		#endregion Methods with transaction


		#endregion Default Methods

		#region Custom Methods
		public static int InsertAutoNumber(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = @"DECLARE @maxNb int = 0;
							SELECT @maxNb=ISNULL(MAX([ConcernNumber]),0)+1 FROM [__EDI_CustomerConcern];
							INSERT INTO [__EDI_CustomerConcern] ([ConcernName],[ConcernNumber],[CreationTime],[CreationUserId],[CreationUserName],[IncludeDescription],[LastEditUserId],[LastEditUserName],[TrimLeadingZeros]) 
							OUTPUT INSERTED.[Id] 
							VALUES (@ConcernName,@maxNb,@CreationTime,@CreationUserId,@CreationUserName,@IncludeDescription,@LastEditUserId,@LastEditUserName,@TrimLeadingZeros); 
";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ConcernName", item.ConcernName == null ? (object)DBNull.Value : item.ConcernName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationUserName", item.CreationUserName == null ? (object)DBNull.Value : item.CreationUserName);
				sqlCommand.Parameters.AddWithValue("IncludeDescription", item.IncludeDescription == null ? (object)DBNull.Value : item.IncludeDescription);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("LastEditUserName", item.LastEditUserName == null ? (object)DBNull.Value : item.LastEditUserName);
				sqlCommand.Parameters.AddWithValue("TrimLeadingZeros", item.TrimLeadingZeros == null ? (object)DBNull.Value : item.TrimLeadingZeros);
				var result = DbExecution.ExecuteScalar(sqlCommand);
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> GetSameName(string name)
		{
			name = name ?? "";
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern] WHERE TRIM([ConcernName])=TRIM(@name)", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("name", name);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity> GetByConcernNumber(int concernNumber)
		{

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcern] WHERE [ConcernNumber]=@concernNumber", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("concernNumber", concernNumber);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity>();
			}
		}

		public static int ToggeIncludeDesignation(int id, int userId, string userName, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__EDI_CustomerConcern] SET [IncludeDescription]=1-ISNULL([IncludeDescription],0), [LastEditUserId]=@userId, [LastEditUserName]=@userName WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", userName);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int ToggeTrimLeadingZeros(int id, int userId, string userName, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__EDI_CustomerConcern] SET [TrimLeadingZeros]=1-ISNULL([TrimLeadingZeros],0), [LastEditUserId]=@userId, [LastEditUserName]=@userName WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("userName", userName);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion Custom Methods

	}
}
