using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.Logistics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Access.Joins.Logistics
{
	public class PlantBookingsTicketLogsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PlantBookingsTicketLogs] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PlantBookingsTicketLogs]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PlantBookingsTicketLogs] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PlantBookingsTicketLogs] ([artikelnummer],[CreationDate],[LagerId],[ticketscount],[Userfullname],[UserId],[Username],[verpackungnr]) OUTPUT INSERTED.[Id] VALUES (@artikelnummer,@CreationDate,@LagerId,@ticketscount,@Userfullname,@UserId,@Username,@verpackungnr); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("ticketscount", item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
					sqlCommand.Parameters.AddWithValue("Userfullname", item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
					sqlCommand.Parameters.AddWithValue("verpackungnr", item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items)
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
						query += " INSERT INTO [PlantBookingsTicketLogs] ([artikelnummer],[CreationDate],[LagerId],[ticketscount],[Userfullname],[UserId],[Username],[verpackungnr]) VALUES ( "

							+ "@artikelnummer" + i + ","
							+ "@CreationDate" + i + ","
							+ "@LagerId" + i + ","
							+ "@ticketscount" + i + ","
							+ "@Userfullname" + i + ","
							+ "@UserId" + i + ","
							+ "@Username" + i + ","
							+ "@verpackungnr" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
						sqlCommand.Parameters.AddWithValue("ticketscount" + i, item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
						sqlCommand.Parameters.AddWithValue("Userfullname" + i, item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
						sqlCommand.Parameters.AddWithValue("verpackungnr" + i, item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PlantBookingsTicketLogs] SET [artikelnummer]=@artikelnummer, [CreationDate]=@CreationDate, [LagerId]=@LagerId, [ticketscount]=@ticketscount, [Userfullname]=@Userfullname, [UserId]=@UserId, [Username]=@Username, [verpackungnr]=@verpackungnr WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
				sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
				sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
				sqlCommand.Parameters.AddWithValue("ticketscount", item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
				sqlCommand.Parameters.AddWithValue("Userfullname", item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
				sqlCommand.Parameters.AddWithValue("verpackungnr", item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items)
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
						query += " UPDATE [PlantBookingsTicketLogs] SET "

							+ "[artikelnummer]=@artikelnummer" + i + ","
							+ "[CreationDate]=@CreationDate" + i + ","
							+ "[LagerId]=@LagerId" + i + ","
							+ "[ticketscount]=@ticketscount" + i + ","
							+ "[Userfullname]=@Userfullname" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[Username]=@Username" + i + ","
							+ "[verpackungnr]=@verpackungnr" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
						sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
						sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
						sqlCommand.Parameters.AddWithValue("ticketscount" + i, item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
						sqlCommand.Parameters.AddWithValue("Userfullname" + i, item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
						sqlCommand.Parameters.AddWithValue("verpackungnr" + i, item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);
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
				string query = "DELETE FROM [PlantBookingsTicketLogs] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [PlantBookingsTicketLogs] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PlantBookingsTicketLogs] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PlantBookingsTicketLogs]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PlantBookingsTicketLogs] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [PlantBookingsTicketLogs] ([artikelnummer],[CreationDate],[LagerId],[ticketscount],[Userfullname],[UserId],[Username],[verpackungnr]) OUTPUT INSERTED.[Id] VALUES (@artikelnummer,@CreationDate,@LagerId,@ticketscount,@Userfullname,@UserId,@Username,@verpackungnr); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
			sqlCommand.Parameters.AddWithValue("ticketscount", item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
			sqlCommand.Parameters.AddWithValue("Userfullname", item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
			sqlCommand.Parameters.AddWithValue("verpackungnr", item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PlantBookingsTicketLogs] ([artikelnummer],[CreationDate],[LagerId],[ticketscount],[Userfullname],[UserId],[Username],[verpackungnr]) VALUES ( "

						+ "@artikelnummer" + i + ","
						+ "@CreationDate" + i + ","
						+ "@LagerId" + i + ","
						+ "@ticketscount" + i + ","
						+ "@Userfullname" + i + ","
						+ "@UserId" + i + ","
						+ "@Username" + i + ","
						+ "@verpackungnr" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("ticketscount" + i, item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
					sqlCommand.Parameters.AddWithValue("Userfullname" + i, item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					sqlCommand.Parameters.AddWithValue("verpackungnr" + i, item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PlantBookingsTicketLogs] SET [artikelnummer]=@artikelnummer, [CreationDate]=@CreationDate, [LagerId]=@LagerId, [ticketscount]=@ticketscount, [Userfullname]=@Userfullname, [UserId]=@UserId, [Username]=@Username, [verpackungnr]=@verpackungnr WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("artikelnummer", item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
			sqlCommand.Parameters.AddWithValue("CreationDate", item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
			sqlCommand.Parameters.AddWithValue("LagerId", item.LagerId == null ? (object)DBNull.Value : item.LagerId);
			sqlCommand.Parameters.AddWithValue("ticketscount", item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
			sqlCommand.Parameters.AddWithValue("Userfullname", item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("Username", item.Username == null ? (object)DBNull.Value : item.Username);
			sqlCommand.Parameters.AddWithValue("verpackungnr", item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PlantBookingsTicketLogs] SET "

					+ "[artikelnummer]=@artikelnummer" + i + ","
					+ "[CreationDate]=@CreationDate" + i + ","
					+ "[LagerId]=@LagerId" + i + ","
					+ "[ticketscount]=@ticketscount" + i + ","
					+ "[Userfullname]=@Userfullname" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[Username]=@Username" + i + ","
					+ "[verpackungnr]=@verpackungnr" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("artikelnummer" + i, item.artikelnummer == null ? (object)DBNull.Value : item.artikelnummer);
					sqlCommand.Parameters.AddWithValue("CreationDate" + i, item.CreationDate == null ? (object)DBNull.Value : item.CreationDate);
					sqlCommand.Parameters.AddWithValue("LagerId" + i, item.LagerId == null ? (object)DBNull.Value : item.LagerId);
					sqlCommand.Parameters.AddWithValue("ticketscount" + i, item.ticketscount == null ? (object)DBNull.Value : item.ticketscount);
					sqlCommand.Parameters.AddWithValue("Userfullname" + i, item.Userfullname == null ? (object)DBNull.Value : item.Userfullname);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("Username" + i, item.Username == null ? (object)DBNull.Value : item.Username);
					sqlCommand.Parameters.AddWithValue("verpackungnr" + i, item.verpackungnr == null ? (object)DBNull.Value : item.verpackungnr);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PlantBookingsTicketLogs] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [PlantBookingsTicketLogs] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static int CountLogsTicket(Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"SELECT Count(*) FROM  [PlantBookingsTicketLogs] ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 300;

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var val) ? val : 0;
			}
		}
		public static List<TicketCountEntity> CountResponseTicket(DateTime dateBegin, DateTime dateEnd)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{

				sqlConnection.Open();
				string query = @$"	SELECT SUM(ticketscount) ticketcount,LagerId FROM [PlantBookingsTicketLogs]
								    WHERE CONVERT(date, CreationDate) BETWEEN 
									CONVERT(date, '{dateBegin.ToString("yyyyMMdd")}') AND CONVERT(date,'{dateEnd.ToString("yyyyMMdd")}')
							     	GROUP by LagerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.TicketCountEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.Logistics.TicketCountEntity>();

				}
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity> GetLogsTicket(string filterSearch, Settings.SortingModel dataSorting, Settings.PaginModel dataPaging)
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
				string query = "SELECT * FROM [PlantBookingsTicketLogs]";

				if(filterSearch != null)
				{
					query += $" WHERE [artikelnummer] LIKE '%{filterSearch}%' OR [verpackungnr] LIKE '%{filterSearch}%' OR [Userfullname] LIKE '%{filterSearch}%'";
				}
				if(dataSorting != null && !string.IsNullOrWhiteSpace(dataSorting.SortFieldName))
				{
					query += $" ORDER BY {dataSorting.SortFieldName} {(dataSorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY [CreationDate]";
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsTicketLogsEntity>();

			}
		}
		#endregion Custom Methods

	}
}
