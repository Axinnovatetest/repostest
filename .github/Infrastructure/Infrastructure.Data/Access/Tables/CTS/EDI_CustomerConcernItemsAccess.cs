using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class EDI_CustomerConcernItemsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems]", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_CustomerConcernItems] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();

		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_CustomerConcernItems] ([ConcernId],[ConcernNumber],[CustomerDUNS],[CustomerNumber]) OUTPUT INSERTED.[Id] VALUES (@ConcernId,@ConcernNumber,@CustomerDUNS,@CustomerNumber); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ConcernId", item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
					sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
					sqlCommand.Parameters.AddWithValue("CustomerDUNS", item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; /* Nb params per query */
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items)
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
						query += " INSERT INTO [__EDI_CustomerConcernItems] ([ConcernId],[ConcernNumber],[CustomerDUNS],[CustomerNumber]) VALUES ("

							+ "@ConcernId" + i + ","
							+ "@ConcernNumber" + i + ","
							+ "@CustomerDUNS" + i + ","
							+ "@CustomerNumber" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ConcernId" + i, item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CustomerDUNS" + i, item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					}

					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("", sqlConnection))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_CustomerConcernItems] SET [ConcernId]=@ConcernId, [ConcernNumber]=@ConcernNumber, [CustomerDUNS]=@CustomerDUNS, [CustomerNumber]=@CustomerNumber WHERE [Id]=@Id";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ConcernId", item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CustomerDUNS", item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; /* Nb params per query */
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items)
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
						query += "UPDATE [__EDI_CustomerConcernItems] SET "

							+ "[ConcernId]=@ConcernId" + i + ","
							+ "[ConcernNumber]=@ConcernNumber" + i + ","
							+ "[CustomerDUNS]=@CustomerDUNS" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + $" WHERE [Id]=@Id{i}"
							+ "; ";

						sqlCommand.Parameters.AddWithValue($"Id{i}", item.Id);

						sqlCommand.Parameters.AddWithValue("ConcernId" + i, item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CustomerDUNS" + i, item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("DELETE FROM [__EDI_CustomerConcernItems] WHERE [Id]=@Id", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("Id", id);

				return sqlCommand.ExecuteNonQuery();
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

					string query = $"DELETE FROM [__EDI_CustomerConcernItems] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [Id] = @Id", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems]", connection, transaction))
			{
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_CustomerConcernItems] WHERE [Id] IN ({string.Join(",", queryIds)})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "INSERT INTO __EDI_CustomerConcernItems ([ConcernId],[ConcernNumber],[CustomerDUNS],[CustomerNumber]) OUTPUT INSERTED.[Id] VALUES (@ConcernId,@ConcernNumber,@CustomerDUNS,@CustomerNumber); ";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ConcernId", item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CustomerDUNS", item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				var result = sqlCommand.ExecuteScalar();
				return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; /* Nb params per query */
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += "INSERT INTO [__EDI_CustomerConcernItems] ([ConcernId],[ConcernNumber],[CustomerDUNS],[CustomerNumber]) VALUES ( "

						+ "@ConcernId" + i + ","
						+ "@ConcernNumber" + i + ","
						+ "@CustomerDUNS" + i + ","
						+ "@CustomerNumber" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ConcernId" + i, item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CustomerDUNS" + i, item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					}

					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "UPDATE [__EDI_CustomerConcernItems] SET [ConcernId]=@ConcernId, [ConcernNumber]=@ConcernNumber, [CustomerDUNS]=@CustomerDUNS, [CustomerNumber]=@CustomerNumber WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ConcernId", item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
				sqlCommand.Parameters.AddWithValue("ConcernNumber", item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
				sqlCommand.Parameters.AddWithValue("CustomerDUNS", item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);

				return sqlCommand.ExecuteNonQuery();
			}
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; /* Nb params per query */
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += "UPDATE [__EDI_CustomerConcernItems] SET "

						+ "[ConcernId]=@ConcernId" + i + ","
						+ "[ConcernNumber]=@ConcernNumber" + i + ","
						+ "[CustomerDUNS]=@CustomerDUNS" + i + ","
						+ "[CustomerNumber]=@CustomerNumber" + i + " WHERE [Id]=@Id" + i
							+ ";";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);

						sqlCommand.Parameters.AddWithValue("ConcernId" + i, item.ConcernId == null ? (object)DBNull.Value : item.ConcernId);
						sqlCommand.Parameters.AddWithValue("ConcernNumber" + i, item.ConcernNumber == null ? (object)DBNull.Value : item.ConcernNumber);
						sqlCommand.Parameters.AddWithValue("CustomerDUNS" + i, item.CustomerDUNS == null ? (object)DBNull.Value : item.CustomerDUNS);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					}

					sqlCommand.CommandText = query;
					return sqlCommand.ExecuteNonQuery();
				}
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			string query = "DELETE FROM [__EDI_CustomerConcernItems] WHERE [Id]=@Id";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Id", id);
				return sqlCommand.ExecuteNonQuery();
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

					string query = $"DELETE FROM __EDI_CustomerConcernItems] WHERE [Id] IN ({string.Join(",", queryIds)})";
					sqlCommand.CommandText = query;

					return sqlCommand.ExecuteNonQuery();
				}
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		public static List<int> GetCustomerNumbersInSameConcern(int customerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT DISTINCT a.CustomerNumber FROM [__EDI_CustomerConcernItems] a Join [__EDI_CustomerConcernItems] b on b.ConcernNumber=a.ConcernNumber WHERE b.[CustomerNumber]=@customerNumber", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0]?.ToString(), out var y) ? y : -1).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity GetByCustomerNumber(int customerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [CustomerNumber]=@customerNumber", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity GetByCustomerNumber(int customerNumber, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [CustomerNumber]=@customerNumber", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> GetListByConcernId(int concernId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [ConcernId]=@concernId", connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("concernId", concernId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> GetByConcernId(int concernId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [__EDI_CustomerConcernItems] WHERE [ConcernId]=@concernId", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("concernId", concernId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity> GetByCustomerNumbers(IEnumerable<int> customerNumbers)
		{
			if(customerNumbers == null || customerNumbers.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT * FROM [__EDI_CustomerConcernItems] WHERE [CustomerNumber] IN ({string.Join(",", customerNumbers)})", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity>();
			}
		}
		public static IEnumerable<KeyValuePair<int, int>> GetCustomersCount()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand($"SELECT COUNT(Id) AS nb, ConcernId FROM [__EDI_CustomerConcernItems] GROUP BY ConcernId", sqlConnection))
			{
				sqlConnection.Open();
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>(int.TryParse(x["ConcernId"].ToString(), out var _x) ? _x : 0,
					int.TryParse(x["nb"].ToString(), out var _y) ? _y : 0));
			}
			else
			{
				return null;
			}
		}
		#region Custom Methods

		#endregion Custom Methods

	}
}
