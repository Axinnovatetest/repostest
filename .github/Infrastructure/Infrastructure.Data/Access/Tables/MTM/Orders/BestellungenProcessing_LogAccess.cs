using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class BestellungenProcessing_LogAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [BestellungenProcessing_Log] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [BestellungenProcessing_Log]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [BestellungenProcessing_Log] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [BestellungenProcessing_Log] ([BestellungenNr],[DateTime],[LogObject],[LogText],[LogType],[Nr],[Origin],[ProjektNr],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@BestellungenNr,@DateTime,@LogObject,@LogText,@LogType,@Nr,@Origin,@ProjektNr,@UserId,@Username); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("BestellungenNr", item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
					sqlCommand.Parameters.AddWithValue("DateTime", item.DateTime == null ? (object)DBNull.Value : item.DateTime);
					sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogText", item.LogText == null ? (object)DBNull.Value : item.LogText);
					sqlCommand.Parameters.AddWithValue("LogType", item.LogType == null ? (object)DBNull.Value : item.LogType);
					sqlCommand.Parameters.AddWithValue("Nr", item.Nr == null ? (object)DBNull.Value : item.Nr);
					sqlCommand.Parameters.AddWithValue("Origin", item.Origin == null ? (object)DBNull.Value : item.Origin);
					sqlCommand.Parameters.AddWithValue("ProjektNr", item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items)
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
						query += " INSERT INTO [BestellungenProcessing_Log] ([BestellungenNr],[DateTime],[LogObject],[LogText],[LogType],[Nr],[Origin],[ProjektNr],[UserId],[Username]) VALUES ( "

							+ "@BestellungenNr" + i + ","
							+ "@DateTime" + i + ","
							+ "@LogObject" + i + ","
							+ "@LogText" + i + ","
							+ "@LogType" + i + ","
							+ "@Nr" + i + ","
							+ "@Origin" + i + ","
							+ "@ProjektNr" + i + ","
							+ "@UserId" + i + ","
							+ "@Username" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("BestellungenNr" + i, item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
						sqlCommand.Parameters.AddWithValue("DateTime" + i, item.DateTime == null ? (object)DBNull.Value : item.DateTime);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogText" + i, item.LogText == null ? (object)DBNull.Value : item.LogText);
						sqlCommand.Parameters.AddWithValue("LogType" + i, item.LogType == null ? (object)DBNull.Value : item.LogType);
						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr == null ? (object)DBNull.Value : item.Nr);
						sqlCommand.Parameters.AddWithValue("Origin" + i, item.Origin == null ? (object)DBNull.Value : item.Origin);
						sqlCommand.Parameters.AddWithValue("ProjektNr" + i, item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [BestellungenProcessing_Log] SET [BestellungenNr]=@BestellungenNr, [DateTime]=@DateTime, [LogObject]=@LogObject, [LogText]=@LogText, [LogType]=@LogType, [Nr]=@Nr, [Origin]=@Origin, [ProjektNr]=@ProjektNr, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("BestellungenNr", item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
				sqlCommand.Parameters.AddWithValue("DateTime", item.DateTime == null ? (object)DBNull.Value : item.DateTime);
				sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
				sqlCommand.Parameters.AddWithValue("LogText", item.LogText == null ? (object)DBNull.Value : item.LogText);
				sqlCommand.Parameters.AddWithValue("LogType", item.LogType == null ? (object)DBNull.Value : item.LogType);
				sqlCommand.Parameters.AddWithValue("Nr", item.Nr == null ? (object)DBNull.Value : item.Nr);
				sqlCommand.Parameters.AddWithValue("Origin", item.Origin == null ? (object)DBNull.Value : item.Origin);
				sqlCommand.Parameters.AddWithValue("ProjektNr", item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items)
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
						query += " UPDATE [BestellungenProcessing_Log] SET "

							+ "[BestellungenNr]=@BestellungenNr" + i + ","
							+ "[DateTime]=@DateTime" + i + ","
							+ "[LogObject]=@LogObject" + i + ","
							+ "[LogText]=@LogText" + i + ","
							+ "[LogType]=@LogType" + i + ","
							+ "[Nr]=@Nr" + i + ","
							+ "[Origin]=@Origin" + i + ","
							+ "[ProjektNr]=@ProjektNr" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("BestellungenNr" + i, item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
						sqlCommand.Parameters.AddWithValue("DateTime" + i, item.DateTime == null ? (object)DBNull.Value : item.DateTime);
						sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
						sqlCommand.Parameters.AddWithValue("LogText" + i, item.LogText == null ? (object)DBNull.Value : item.LogText);
						sqlCommand.Parameters.AddWithValue("LogType" + i, item.LogType == null ? (object)DBNull.Value : item.LogType);
						sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr == null ? (object)DBNull.Value : item.Nr);
						sqlCommand.Parameters.AddWithValue("Origin" + i, item.Origin == null ? (object)DBNull.Value : item.Origin);
						sqlCommand.Parameters.AddWithValue("ProjektNr" + i, item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
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
				string query = "DELETE FROM [BestellungenProcessing_Log] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [BestellungenProcessing_Log] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [BestellungenProcessing_Log] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [BestellungenProcessing_Log]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [BestellungenProcessing_Log] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [BestellungenProcessing_Log] ([BestellungenNr],[DateTime],[LogObject],[LogText],[LogType],[Nr],[Origin],[ProjektNr],[UserId],[Username]) OUTPUT INSERTED.[Id] VALUES (@BestellungenNr,@DateTime,@LogObject,@LogText,@LogType,@Nr,@Origin,@ProjektNr,@UserId,@Username); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("BestellungenNr", item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
			sqlCommand.Parameters.AddWithValue("DateTime", item.DateTime == null ? (object)DBNull.Value : item.DateTime);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogText", item.LogText == null ? (object)DBNull.Value : item.LogText);
			sqlCommand.Parameters.AddWithValue("LogType", item.LogType == null ? (object)DBNull.Value : item.LogType);
			sqlCommand.Parameters.AddWithValue("Nr", item.Nr == null ? (object)DBNull.Value : item.Nr);
			sqlCommand.Parameters.AddWithValue("Origin", item.Origin == null ? (object)DBNull.Value : item.Origin);
			sqlCommand.Parameters.AddWithValue("ProjektNr", item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [BestellungenProcessing_Log] ([BestellungenNr],[DateTime],[LogObject],[LogText],[LogType],[Nr],[Origin],[ProjektNr],[UserId],[Username]) VALUES ( "

						+ "@BestellungenNr" + i + ","
						+ "@DateTime" + i + ","
						+ "@LogObject" + i + ","
						+ "@LogText" + i + ","
						+ "@LogType" + i + ","
						+ "@Nr" + i + ","
						+ "@Origin" + i + ","
						+ "@ProjektNr" + i + ","
						+ "@UserId" + i + ","
						+ "@Username" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("BestellungenNr" + i, item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
					sqlCommand.Parameters.AddWithValue("DateTime" + i, item.DateTime == null ? (object)DBNull.Value : item.DateTime);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogText" + i, item.LogText == null ? (object)DBNull.Value : item.LogText);
					sqlCommand.Parameters.AddWithValue("LogType" + i, item.LogType == null ? (object)DBNull.Value : item.LogType);
					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr == null ? (object)DBNull.Value : item.Nr);
					sqlCommand.Parameters.AddWithValue("Origin" + i, item.Origin == null ? (object)DBNull.Value : item.Origin);
					sqlCommand.Parameters.AddWithValue("ProjektNr" + i, item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [BestellungenProcessing_Log] SET [BestellungenNr]=@BestellungenNr, [DateTime]=@DateTime, [LogObject]=@LogObject, [LogText]=@LogText, [LogType]=@LogType, [Nr]=@Nr, [Origin]=@Origin, [ProjektNr]=@ProjektNr, [UserId]=@UserId, [Username]=@Username WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("BestellungenNr", item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
			sqlCommand.Parameters.AddWithValue("DateTime", item.DateTime == null ? (object)DBNull.Value : item.DateTime);
			sqlCommand.Parameters.AddWithValue("LogObject", item.LogObject == null ? (object)DBNull.Value : item.LogObject);
			sqlCommand.Parameters.AddWithValue("LogText", item.LogText == null ? (object)DBNull.Value : item.LogText);
			sqlCommand.Parameters.AddWithValue("LogType", item.LogType == null ? (object)DBNull.Value : item.LogType);
			sqlCommand.Parameters.AddWithValue("Nr", item.Nr == null ? (object)DBNull.Value : item.Nr);
			sqlCommand.Parameters.AddWithValue("Origin", item.Origin == null ? (object)DBNull.Value : item.Origin);
			sqlCommand.Parameters.AddWithValue("ProjektNr", item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [BestellungenProcessing_Log] SET "

					+ "[BestellungenNr]=@BestellungenNr" + i + ","
					+ "[DateTime]=@DateTime" + i + ","
					+ "[LogObject]=@LogObject" + i + ","
					+ "[LogText]=@LogText" + i + ","
					+ "[LogType]=@LogType" + i + ","
					+ "[Nr]=@Nr" + i + ","
					+ "[Origin]=@Origin" + i + ","
					+ "[ProjektNr]=@ProjektNr" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[Username]=@Username" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("BestellungenNr" + i, item.BestellungenNr == null ? (object)DBNull.Value : item.BestellungenNr);
					sqlCommand.Parameters.AddWithValue("DateTime" + i, item.DateTime == null ? (object)DBNull.Value : item.DateTime);
					sqlCommand.Parameters.AddWithValue("LogObject" + i, item.LogObject == null ? (object)DBNull.Value : item.LogObject);
					sqlCommand.Parameters.AddWithValue("LogText" + i, item.LogText == null ? (object)DBNull.Value : item.LogText);
					sqlCommand.Parameters.AddWithValue("LogType" + i, item.LogType == null ? (object)DBNull.Value : item.LogType);
					sqlCommand.Parameters.AddWithValue("Nr" + i, item.Nr == null ? (object)DBNull.Value : item.Nr);
					sqlCommand.Parameters.AddWithValue("Origin" + i, item.Origin == null ? (object)DBNull.Value : item.Origin);
					sqlCommand.Parameters.AddWithValue("ProjektNr" + i, item.ProjektNr == null ? (object)DBNull.Value : item.ProjektNr);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [BestellungenProcessing_Log] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [BestellungenProcessing_Log] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> GetByOrderid(int id, string type)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BestellungenProcessing_Log] WHERE [Nr]=@id AND [LogObject]=@type ORDER BY [DateTime] DESC", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("type", type);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity> GetByOrderid(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			using(var sqlCommand = new SqlCommand("SELECT * FROM [BestellungenProcessing_Log] WHERE [Nr]=@id ORDER BY [DateTime] DESC", sqlConnection))
			{
				sqlConnection.Open();
				sqlCommand.Parameters.AddWithValue("id", id);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity>();
			}
		}
		#endregion Custom Methods

	}
}
