using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class __E_rechnung_CreatedAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__E_rechnung_Created] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__E_rechnung_Created]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__E_rechnung_Created] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__E_rechnung_Created] ([CreationTime],[CustomerName],[CustomerNr],[CustomerRechnungType],[LsAngebotNr],[LSNr],[RechnungForfallNr],[RechnungNr],[RechnungProjectNr],[SentTime]) OUTPUT INSERTED.[Id] VALUES (@CreationTime,@CustomerName,@CustomerNr,@CustomerRechnungType,@LsAngebotNr,@LSNr,@RechnungForfallNr,@RechnungNr,@RechnungProjectNr,@SentTime); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("CustomerRechnungType", item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
					sqlCommand.Parameters.AddWithValue("LsAngebotNr", item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
					sqlCommand.Parameters.AddWithValue("LSNr", item.LSNr == null ? (object)DBNull.Value : item.LSNr);
					sqlCommand.Parameters.AddWithValue("RechnungForfallNr", item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
					sqlCommand.Parameters.AddWithValue("RechnungNr", item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
					sqlCommand.Parameters.AddWithValue("RechnungProjectNr", item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
					sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items)
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
						query += " INSERT INTO [__E_rechnung_Created] ([CreationTime],[CustomerName],[CustomerNr],[CustomerRechnungType],[LsAngebotNr],[LSNr],[RechnungForfallNr],[RechnungNr],[RechnungProjectNr],[SentTime]) VALUES ( "

							+ "@CreationTime" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerNr" + i + ","
							+ "@CustomerRechnungType" + i + ","
							+ "@LsAngebotNr" + i + ","
							+ "@LSNr" + i + ","
							+ "@RechnungForfallNr" + i + ","
							+ "@RechnungNr" + i + ","
							+ "@RechnungProjectNr" + i + ","
							+ "@SentTime" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
						sqlCommand.Parameters.AddWithValue("CustomerRechnungType" + i, item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
						sqlCommand.Parameters.AddWithValue("LsAngebotNr" + i, item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
						sqlCommand.Parameters.AddWithValue("LSNr" + i, item.LSNr == null ? (object)DBNull.Value : item.LSNr);
						sqlCommand.Parameters.AddWithValue("RechnungForfallNr" + i, item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
						sqlCommand.Parameters.AddWithValue("RechnungNr" + i, item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
						sqlCommand.Parameters.AddWithValue("RechnungProjectNr" + i, item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
						sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__E_rechnung_Created] SET [CreationTime]=@CreationTime, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [CustomerRechnungType]=@CustomerRechnungType, [LsAngebotNr]=@LsAngebotNr, [LSNr]=@LSNr, [RechnungForfallNr]=@RechnungForfallNr, [RechnungNr]=@RechnungNr, [RechnungProjectNr]=@RechnungProjectNr, [SentTime]=@SentTime WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
				sqlCommand.Parameters.AddWithValue("CustomerRechnungType", item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
				sqlCommand.Parameters.AddWithValue("LsAngebotNr", item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
				sqlCommand.Parameters.AddWithValue("LSNr", item.LSNr == null ? (object)DBNull.Value : item.LSNr);
				sqlCommand.Parameters.AddWithValue("RechnungForfallNr", item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
				sqlCommand.Parameters.AddWithValue("RechnungNr", item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
				sqlCommand.Parameters.AddWithValue("RechnungProjectNr", item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
				sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items)
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
						query += " UPDATE [__E_rechnung_Created] SET "

							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerNr]=@CustomerNr" + i + ","
							+ "[CustomerRechnungType]=@CustomerRechnungType" + i + ","
							+ "[LsAngebotNr]=@LsAngebotNr" + i + ","
							+ "[LSNr]=@LSNr" + i + ","
							+ "[RechnungForfallNr]=@RechnungForfallNr" + i + ","
							+ "[RechnungNr]=@RechnungNr" + i + ","
							+ "[RechnungProjectNr]=@RechnungProjectNr" + i + ","
							+ "[SentTime]=@SentTime" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
						sqlCommand.Parameters.AddWithValue("CustomerRechnungType" + i, item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
						sqlCommand.Parameters.AddWithValue("LsAngebotNr" + i, item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
						sqlCommand.Parameters.AddWithValue("LSNr" + i, item.LSNr == null ? (object)DBNull.Value : item.LSNr);
						sqlCommand.Parameters.AddWithValue("RechnungForfallNr" + i, item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
						sqlCommand.Parameters.AddWithValue("RechnungNr" + i, item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
						sqlCommand.Parameters.AddWithValue("RechnungProjectNr" + i, item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
						sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
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
				string query = "DELETE FROM [__E_rechnung_Created] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__E_rechnung_Created] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__E_rechnung_Created] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__E_rechnung_Created]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__E_rechnung_Created] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__E_rechnung_Created] ([CreationTime],[CustomerName],[CustomerNr],[CustomerRechnungType],[LsAngebotNr],[LSNr],[RechnungForfallNr],[RechnungNr],[RechnungProjectNr],[SentTime]) OUTPUT INSERTED.[Id] VALUES (@CreationTime,@CustomerName,@CustomerNr,@CustomerRechnungType,@LsAngebotNr,@LSNr,@RechnungForfallNr,@RechnungNr,@RechnungProjectNr,@SentTime); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
			sqlCommand.Parameters.AddWithValue("CustomerRechnungType", item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
			sqlCommand.Parameters.AddWithValue("LsAngebotNr", item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
			sqlCommand.Parameters.AddWithValue("LSNr", item.LSNr == null ? (object)DBNull.Value : item.LSNr);
			sqlCommand.Parameters.AddWithValue("RechnungForfallNr", item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
			sqlCommand.Parameters.AddWithValue("RechnungNr", item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
			sqlCommand.Parameters.AddWithValue("RechnungProjectNr", item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
			sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__E_rechnung_Created] ([CreationTime],[CustomerName],[CustomerNr],[CustomerRechnungType],[LsAngebotNr],[LSNr],[RechnungForfallNr],[RechnungNr],[RechnungProjectNr],[SentTime]) VALUES ( "

						+ "@CreationTime" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNr" + i + ","
						+ "@CustomerRechnungType" + i + ","
						+ "@LsAngebotNr" + i + ","
						+ "@LSNr" + i + ","
						+ "@RechnungForfallNr" + i + ","
						+ "@RechnungNr" + i + ","
						+ "@RechnungProjectNr" + i + ","
						+ "@SentTime" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("CustomerRechnungType" + i, item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
					sqlCommand.Parameters.AddWithValue("LsAngebotNr" + i, item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
					sqlCommand.Parameters.AddWithValue("LSNr" + i, item.LSNr == null ? (object)DBNull.Value : item.LSNr);
					sqlCommand.Parameters.AddWithValue("RechnungForfallNr" + i, item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
					sqlCommand.Parameters.AddWithValue("RechnungNr" + i, item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
					sqlCommand.Parameters.AddWithValue("RechnungProjectNr" + i, item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
					sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__E_rechnung_Created] SET [CreationTime]=@CreationTime, [CustomerName]=@CustomerName, [CustomerNr]=@CustomerNr, [CustomerRechnungType]=@CustomerRechnungType, [LsAngebotNr]=@LsAngebotNr, [LSNr]=@LSNr, [RechnungForfallNr]=@RechnungForfallNr, [RechnungNr]=@RechnungNr, [RechnungProjectNr]=@RechnungProjectNr, [SentTime]=@SentTime WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNr", item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
			sqlCommand.Parameters.AddWithValue("CustomerRechnungType", item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
			sqlCommand.Parameters.AddWithValue("LsAngebotNr", item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
			sqlCommand.Parameters.AddWithValue("LSNr", item.LSNr == null ? (object)DBNull.Value : item.LSNr);
			sqlCommand.Parameters.AddWithValue("RechnungForfallNr", item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
			sqlCommand.Parameters.AddWithValue("RechnungNr", item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
			sqlCommand.Parameters.AddWithValue("RechnungProjectNr", item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
			sqlCommand.Parameters.AddWithValue("SentTime", item.SentTime == null ? (object)DBNull.Value : item.SentTime);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__E_rechnung_Created] SET "

					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[CustomerNr]=@CustomerNr" + i + ","
					+ "[CustomerRechnungType]=@CustomerRechnungType" + i + ","
					+ "[LsAngebotNr]=@LsAngebotNr" + i + ","
					+ "[LSNr]=@LSNr" + i + ","
					+ "[RechnungForfallNr]=@RechnungForfallNr" + i + ","
					+ "[RechnungNr]=@RechnungNr" + i + ","
					+ "[RechnungProjectNr]=@RechnungProjectNr" + i + ","
					+ "[SentTime]=@SentTime" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNr" + i, item.CustomerNr == null ? (object)DBNull.Value : item.CustomerNr);
					sqlCommand.Parameters.AddWithValue("CustomerRechnungType" + i, item.CustomerRechnungType == null ? (object)DBNull.Value : item.CustomerRechnungType);
					sqlCommand.Parameters.AddWithValue("LsAngebotNr" + i, item.LsAngebotNr == null ? (object)DBNull.Value : item.LsAngebotNr);
					sqlCommand.Parameters.AddWithValue("LSNr" + i, item.LSNr == null ? (object)DBNull.Value : item.LSNr);
					sqlCommand.Parameters.AddWithValue("RechnungForfallNr" + i, item.RechnungForfallNr == null ? (object)DBNull.Value : item.RechnungForfallNr);
					sqlCommand.Parameters.AddWithValue("RechnungNr" + i, item.RechnungNr == null ? (object)DBNull.Value : item.RechnungNr);
					sqlCommand.Parameters.AddWithValue("RechnungProjectNr" + i, item.RechnungProjectNr == null ? (object)DBNull.Value : item.RechnungProjectNr);
					sqlCommand.Parameters.AddWithValue("SentTime" + i, item.SentTime == null ? (object)DBNull.Value : item.SentTime);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__E_rechnung_Created] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__E_rechnung_Created] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetE_RechnungNotSentOrtNotValidated()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"SELECT E.* FROM [__E_rechnung_Created] E INNER JOIN Angebote A
                               ON E.RechnungNr=A.Nr
                               WHERE E.SentTime IS NULL
                               OR (A.gebucht IS NULL OR A.gebucht=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetE_RechnungByNr(List<int> nrs)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"SELECT E.* FROM [__E_rechnung_Created] E WHERE RechnungNr IN ({string.Join(",", nrs)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity GetByRechnungNr(int Nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__E_rechnung_Created] WHERE [RechnungNr]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", Nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity GetByRechnungNr(int Nr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__E_rechnung_Created] WHERE [RechnungNr]=@Nr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Nr", Nr);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetByRechnungAngobotNr(int angebotNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__E_rechnung_Created] WHERE [RechnungForfallNr]=@angebotNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetByRechnungAngobotNr(int angebotNr, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [__E_rechnung_Created] WHERE [RechnungForfallNr]=@angebotNr";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity> GetNotSent(IEnumerable<int> rgIds)
		{
			if(rgIds == null || rgIds.Count() <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__E_rechnung_Created] WHERE ISNULL([SentTime],0)=0 AND [RechnungNr] IN ({string.Join(",", rgIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity>();
			}
		}
		#endregion Custom Methods

	}
}
