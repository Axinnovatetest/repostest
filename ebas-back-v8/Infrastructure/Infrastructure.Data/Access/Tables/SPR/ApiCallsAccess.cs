using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data.Entities.Tables.SPR;

namespace Infrastructure.Data.Access.Tables.SPR
{
	public class ApiCallsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM perf.ApiCalls WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM perf.ApiCalls";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM perf.ApiCalls WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO perf.ApiCalls ([ApiArea],[ApiController],[ApiMethod],[FirstCallTime],[LastCallTime],[ProcessingTime],[TotalCall02HCount],[TotalCall04HCount],[TotalCall06HCount],[TotalCall08HCount],[TotalCall10HCount],[TotalCall12HCount],[TotalCall14HCount],[TotalCall16HCount],[TotalCall18HCount],[TotalCall20HCount],[TotalCall22HCount],[TotalCall24HCount],[TotalCallCount],[TotalCallDistinctUserCount],[UserId]) OUTPUT INSERTED.[Id] VALUES (@ApiArea,@ApiController,@ApiMethod,@FirstCallTime,@LastCallTime,@ProcessingTime,@TotalCall02HCount,@TotalCall04HCount,@TotalCall06HCount,@TotalCall08HCount,@TotalCall10HCount,@TotalCall12HCount,@TotalCall14HCount,@TotalCall16HCount,@TotalCall18HCount,@TotalCall20HCount,@TotalCall22HCount,@TotalCall24HCount,@TotalCallCount,@TotalCallDistinctUserCount,@UserId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ApiArea", item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
					sqlCommand.Parameters.AddWithValue("ApiController", item.ApiController == null ? (object)DBNull.Value : item.ApiController);
					sqlCommand.Parameters.AddWithValue("ApiMethod", item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
					sqlCommand.Parameters.AddWithValue("FirstCallTime", item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
					sqlCommand.Parameters.AddWithValue("LastCallTime", item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
					sqlCommand.Parameters.AddWithValue("ProcessingTime", item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
					sqlCommand.Parameters.AddWithValue("TotalCall02HCount", item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall04HCount", item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall06HCount", item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall08HCount", item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall10HCount", item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall12HCount", item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall14HCount", item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall16HCount", item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall18HCount", item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall20HCount", item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall22HCount", item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall24HCount", item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
					sqlCommand.Parameters.AddWithValue("TotalCallCount", item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
					sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount", item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items)
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
						query += " INSERT INTO perf.ApiCalls ([ApiArea],[ApiController],[ApiMethod],[FirstCallTime],[LastCallTime],[ProcessingTime],[TotalCall02HCount],[TotalCall04HCount],[TotalCall06HCount],[TotalCall08HCount],[TotalCall10HCount],[TotalCall12HCount],[TotalCall14HCount],[TotalCall16HCount],[TotalCall18HCount],[TotalCall20HCount],[TotalCall22HCount],[TotalCall24HCount],[TotalCallCount],[TotalCallDistinctUserCount],[UserId]) VALUES ( "

							+ "@ApiArea" + i + ","
							+ "@ApiController" + i + ","
							+ "@ApiMethod" + i + ","
							+ "@FirstCallTime" + i + ","
							+ "@LastCallTime" + i + ","
							+ "@ProcessingTime" + i + ","
							+ "@TotalCall02HCount" + i + ","
							+ "@TotalCall04HCount" + i + ","
							+ "@TotalCall06HCount" + i + ","
							+ "@TotalCall08HCount" + i + ","
							+ "@TotalCall10HCount" + i + ","
							+ "@TotalCall12HCount" + i + ","
							+ "@TotalCall14HCount" + i + ","
							+ "@TotalCall16HCount" + i + ","
							+ "@TotalCall18HCount" + i + ","
							+ "@TotalCall20HCount" + i + ","
							+ "@TotalCall22HCount" + i + ","
							+ "@TotalCall24HCount" + i + ","
							+ "@TotalCallCount" + i + ","
							+ "@TotalCallDistinctUserCount" + i + ","
							+ "@UserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ApiArea" + i, item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
						sqlCommand.Parameters.AddWithValue("ApiController" + i, item.ApiController == null ? (object)DBNull.Value : item.ApiController);
						sqlCommand.Parameters.AddWithValue("ApiMethod" + i, item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
						sqlCommand.Parameters.AddWithValue("FirstCallTime" + i, item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
						sqlCommand.Parameters.AddWithValue("LastCallTime" + i, item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
						sqlCommand.Parameters.AddWithValue("ProcessingTime" + i, item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
						sqlCommand.Parameters.AddWithValue("TotalCall02HCount" + i, item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall04HCount" + i, item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall06HCount" + i, item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall08HCount" + i, item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall10HCount" + i, item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall12HCount" + i, item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall14HCount" + i, item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall16HCount" + i, item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall18HCount" + i, item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall20HCount" + i, item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall22HCount" + i, item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall24HCount" + i, item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
						sqlCommand.Parameters.AddWithValue("TotalCallCount" + i, item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
						sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount" + i, item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE perf.ApiCalls SET [ApiArea]=@ApiArea, [ApiController]=@ApiController, [ApiMethod]=@ApiMethod, [FirstCallTime]=@FirstCallTime, [LastCallTime]=@LastCallTime, [ProcessingTime]=@ProcessingTime, [TotalCall02HCount]=@TotalCall02HCount, [TotalCall04HCount]=@TotalCall04HCount, [TotalCall06HCount]=@TotalCall06HCount, [TotalCall08HCount]=@TotalCall08HCount, [TotalCall10HCount]=@TotalCall10HCount, [TotalCall12HCount]=@TotalCall12HCount, [TotalCall14HCount]=@TotalCall14HCount, [TotalCall16HCount]=@TotalCall16HCount, [TotalCall18HCount]=@TotalCall18HCount, [TotalCall20HCount]=@TotalCall20HCount, [TotalCall22HCount]=@TotalCall22HCount, [TotalCall24HCount]=@TotalCall24HCount, [TotalCallCount]=@TotalCallCount, [TotalCallDistinctUserCount]=@TotalCallDistinctUserCount, [UserId]=@UserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ApiArea", item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
				sqlCommand.Parameters.AddWithValue("ApiController", item.ApiController == null ? (object)DBNull.Value : item.ApiController);
				sqlCommand.Parameters.AddWithValue("ApiMethod", item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
				sqlCommand.Parameters.AddWithValue("FirstCallTime", item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
				sqlCommand.Parameters.AddWithValue("LastCallTime", item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
				sqlCommand.Parameters.AddWithValue("ProcessingTime", item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
				sqlCommand.Parameters.AddWithValue("TotalCall02HCount", item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall04HCount", item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall06HCount", item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall08HCount", item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall10HCount", item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall12HCount", item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall14HCount", item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall16HCount", item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall18HCount", item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall20HCount", item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall22HCount", item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
				sqlCommand.Parameters.AddWithValue("TotalCall24HCount", item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
				sqlCommand.Parameters.AddWithValue("TotalCallCount", item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
				sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount", item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items)
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
						query += " UPDATE perf.ApiCalls SET "

							+ "[ApiArea]=@ApiArea" + i + ","
							+ "[ApiController]=@ApiController" + i + ","
							+ "[ApiMethod]=@ApiMethod" + i + ","
							+ "[FirstCallTime]=@FirstCallTime" + i + ","
							+ "[LastCallTime]=@LastCallTime" + i + ","
							+ "[ProcessingTime]=@ProcessingTime" + i + ","
							+ "[TotalCall02HCount]=@TotalCall02HCount" + i + ","
							+ "[TotalCall04HCount]=@TotalCall04HCount" + i + ","
							+ "[TotalCall06HCount]=@TotalCall06HCount" + i + ","
							+ "[TotalCall08HCount]=@TotalCall08HCount" + i + ","
							+ "[TotalCall10HCount]=@TotalCall10HCount" + i + ","
							+ "[TotalCall12HCount]=@TotalCall12HCount" + i + ","
							+ "[TotalCall14HCount]=@TotalCall14HCount" + i + ","
							+ "[TotalCall16HCount]=@TotalCall16HCount" + i + ","
							+ "[TotalCall18HCount]=@TotalCall18HCount" + i + ","
							+ "[TotalCall20HCount]=@TotalCall20HCount" + i + ","
							+ "[TotalCall22HCount]=@TotalCall22HCount" + i + ","
							+ "[TotalCall24HCount]=@TotalCall24HCount" + i + ","
							+ "[TotalCallCount]=@TotalCallCount" + i + ","
							+ "[TotalCallDistinctUserCount]=@TotalCallDistinctUserCount" + i + ","
							+ "[UserId]=@UserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ApiArea" + i, item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
						sqlCommand.Parameters.AddWithValue("ApiController" + i, item.ApiController == null ? (object)DBNull.Value : item.ApiController);
						sqlCommand.Parameters.AddWithValue("ApiMethod" + i, item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
						sqlCommand.Parameters.AddWithValue("FirstCallTime" + i, item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
						sqlCommand.Parameters.AddWithValue("LastCallTime" + i, item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
						sqlCommand.Parameters.AddWithValue("ProcessingTime" + i, item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
						sqlCommand.Parameters.AddWithValue("TotalCall02HCount" + i, item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall04HCount" + i, item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall06HCount" + i, item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall08HCount" + i, item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall10HCount" + i, item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall12HCount" + i, item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall14HCount" + i, item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall16HCount" + i, item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall18HCount" + i, item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall20HCount" + i, item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall22HCount" + i, item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
						sqlCommand.Parameters.AddWithValue("TotalCall24HCount" + i, item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
						sqlCommand.Parameters.AddWithValue("TotalCallCount" + i, item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
						sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount" + i, item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
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
				string query = "DELETE FROM perf.ApiCalls WHERE [Id]=@Id";
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

					string query = "DELETE FROM perf.ApiCalls WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM perf.ApiCalls WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM perf.ApiCalls";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM perf.ApiCalls WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO perf.ApiCalls ([ApiArea],[ApiController],[ApiMethod],[FirstCallTime],[LastCallTime],[ProcessingTime],[TotalCall02HCount],[TotalCall04HCount],[TotalCall06HCount],[TotalCall08HCount],[TotalCall10HCount],[TotalCall12HCount],[TotalCall14HCount],[TotalCall16HCount],[TotalCall18HCount],[TotalCall20HCount],[TotalCall22HCount],[TotalCall24HCount],[TotalCallCount],[TotalCallDistinctUserCount],[UserId]) OUTPUT INSERTED.[Id] VALUES (@ApiArea,@ApiController,@ApiMethod,@FirstCallTime,@LastCallTime,@ProcessingTime,@TotalCall02HCount,@TotalCall04HCount,@TotalCall06HCount,@TotalCall08HCount,@TotalCall10HCount,@TotalCall12HCount,@TotalCall14HCount,@TotalCall16HCount,@TotalCall18HCount,@TotalCall20HCount,@TotalCall22HCount,@TotalCall24HCount,@TotalCallCount,@TotalCallDistinctUserCount,@UserId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ApiArea", item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
			sqlCommand.Parameters.AddWithValue("ApiController", item.ApiController == null ? (object)DBNull.Value : item.ApiController);
			sqlCommand.Parameters.AddWithValue("ApiMethod", item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
			sqlCommand.Parameters.AddWithValue("FirstCallTime", item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
			sqlCommand.Parameters.AddWithValue("LastCallTime", item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
			sqlCommand.Parameters.AddWithValue("ProcessingTime", item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
			sqlCommand.Parameters.AddWithValue("TotalCall02HCount", item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall04HCount", item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall06HCount", item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall08HCount", item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall10HCount", item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall12HCount", item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall14HCount", item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall16HCount", item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall18HCount", item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall20HCount", item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall22HCount", item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall24HCount", item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
			sqlCommand.Parameters.AddWithValue("TotalCallCount", item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
			sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount", item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO perf.ApiCalls ([ApiArea],[ApiController],[ApiMethod],[FirstCallTime],[LastCallTime],[ProcessingTime],[TotalCall02HCount],[TotalCall04HCount],[TotalCall06HCount],[TotalCall08HCount],[TotalCall10HCount],[TotalCall12HCount],[TotalCall14HCount],[TotalCall16HCount],[TotalCall18HCount],[TotalCall20HCount],[TotalCall22HCount],[TotalCall24HCount],[TotalCallCount],[TotalCallDistinctUserCount],[UserId]) VALUES ( "

						+ "@ApiArea" + i + ","
						+ "@ApiController" + i + ","
						+ "@ApiMethod" + i + ","
						+ "@FirstCallTime" + i + ","
						+ "@LastCallTime" + i + ","
						+ "@ProcessingTime" + i + ","
						+ "@TotalCall02HCount" + i + ","
						+ "@TotalCall04HCount" + i + ","
						+ "@TotalCall06HCount" + i + ","
						+ "@TotalCall08HCount" + i + ","
						+ "@TotalCall10HCount" + i + ","
						+ "@TotalCall12HCount" + i + ","
						+ "@TotalCall14HCount" + i + ","
						+ "@TotalCall16HCount" + i + ","
						+ "@TotalCall18HCount" + i + ","
						+ "@TotalCall20HCount" + i + ","
						+ "@TotalCall22HCount" + i + ","
						+ "@TotalCall24HCount" + i + ","
						+ "@TotalCallCount" + i + ","
						+ "@TotalCallDistinctUserCount" + i + ","
						+ "@UserId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ApiArea" + i, item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
					sqlCommand.Parameters.AddWithValue("ApiController" + i, item.ApiController == null ? (object)DBNull.Value : item.ApiController);
					sqlCommand.Parameters.AddWithValue("ApiMethod" + i, item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
					sqlCommand.Parameters.AddWithValue("FirstCallTime" + i, item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
					sqlCommand.Parameters.AddWithValue("LastCallTime" + i, item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
					sqlCommand.Parameters.AddWithValue("ProcessingTime" + i, item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
					sqlCommand.Parameters.AddWithValue("TotalCall02HCount" + i, item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall04HCount" + i, item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall06HCount" + i, item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall08HCount" + i, item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall10HCount" + i, item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall12HCount" + i, item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall14HCount" + i, item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall16HCount" + i, item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall18HCount" + i, item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall20HCount" + i, item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall22HCount" + i, item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall24HCount" + i, item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
					sqlCommand.Parameters.AddWithValue("TotalCallCount" + i, item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
					sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount" + i, item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE perf.ApiCalls SET [ApiArea]=@ApiArea, [ApiController]=@ApiController, [ApiMethod]=@ApiMethod, [FirstCallTime]=@FirstCallTime, [LastCallTime]=@LastCallTime, [ProcessingTime]=@ProcessingTime, [TotalCall02HCount]=@TotalCall02HCount, [TotalCall04HCount]=@TotalCall04HCount, [TotalCall06HCount]=@TotalCall06HCount, [TotalCall08HCount]=@TotalCall08HCount, [TotalCall10HCount]=@TotalCall10HCount, [TotalCall12HCount]=@TotalCall12HCount, [TotalCall14HCount]=@TotalCall14HCount, [TotalCall16HCount]=@TotalCall16HCount, [TotalCall18HCount]=@TotalCall18HCount, [TotalCall20HCount]=@TotalCall20HCount, [TotalCall22HCount]=@TotalCall22HCount, [TotalCall24HCount]=@TotalCall24HCount, [TotalCallCount]=@TotalCallCount, [TotalCallDistinctUserCount]=@TotalCallDistinctUserCount, [UserId]=@UserId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ApiArea", item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
			sqlCommand.Parameters.AddWithValue("ApiController", item.ApiController == null ? (object)DBNull.Value : item.ApiController);
			sqlCommand.Parameters.AddWithValue("ApiMethod", item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
			sqlCommand.Parameters.AddWithValue("FirstCallTime", item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
			sqlCommand.Parameters.AddWithValue("LastCallTime", item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
			sqlCommand.Parameters.AddWithValue("ProcessingTime", item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
			sqlCommand.Parameters.AddWithValue("TotalCall02HCount", item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall04HCount", item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall06HCount", item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall08HCount", item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall10HCount", item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall12HCount", item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall14HCount", item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall16HCount", item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall18HCount", item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall20HCount", item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall22HCount", item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
			sqlCommand.Parameters.AddWithValue("TotalCall24HCount", item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
			sqlCommand.Parameters.AddWithValue("TotalCallCount", item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
			sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount", item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 22; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.SPR.ApiCallsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE perf.ApiCalls SET "

					+ "[ApiArea]=@ApiArea" + i + ","
					+ "[ApiController]=@ApiController" + i + ","
					+ "[ApiMethod]=@ApiMethod" + i + ","
					+ "[FirstCallTime]=@FirstCallTime" + i + ","
					+ "[LastCallTime]=@LastCallTime" + i + ","
					+ "[ProcessingTime]=@ProcessingTime" + i + ","
					+ "[TotalCall02HCount]=@TotalCall02HCount" + i + ","
					+ "[TotalCall04HCount]=@TotalCall04HCount" + i + ","
					+ "[TotalCall06HCount]=@TotalCall06HCount" + i + ","
					+ "[TotalCall08HCount]=@TotalCall08HCount" + i + ","
					+ "[TotalCall10HCount]=@TotalCall10HCount" + i + ","
					+ "[TotalCall12HCount]=@TotalCall12HCount" + i + ","
					+ "[TotalCall14HCount]=@TotalCall14HCount" + i + ","
					+ "[TotalCall16HCount]=@TotalCall16HCount" + i + ","
					+ "[TotalCall18HCount]=@TotalCall18HCount" + i + ","
					+ "[TotalCall20HCount]=@TotalCall20HCount" + i + ","
					+ "[TotalCall22HCount]=@TotalCall22HCount" + i + ","
					+ "[TotalCall24HCount]=@TotalCall24HCount" + i + ","
					+ "[TotalCallCount]=@TotalCallCount" + i + ","
					+ "[TotalCallDistinctUserCount]=@TotalCallDistinctUserCount" + i + ","
					+ "[UserId]=@UserId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ApiArea" + i, item.ApiArea == null ? (object)DBNull.Value : item.ApiArea);
					sqlCommand.Parameters.AddWithValue("ApiController" + i, item.ApiController == null ? (object)DBNull.Value : item.ApiController);
					sqlCommand.Parameters.AddWithValue("ApiMethod" + i, item.ApiMethod == null ? (object)DBNull.Value : item.ApiMethod);
					sqlCommand.Parameters.AddWithValue("FirstCallTime" + i, item.FirstCallTime == null ? (object)DBNull.Value : item.FirstCallTime);
					sqlCommand.Parameters.AddWithValue("LastCallTime" + i, item.LastCallTime == null ? (object)DBNull.Value : item.LastCallTime);
					sqlCommand.Parameters.AddWithValue("ProcessingTime" + i, item.ProcessingTime == null ? (object)DBNull.Value : item.ProcessingTime);
					sqlCommand.Parameters.AddWithValue("TotalCall02HCount" + i, item.TotalCall02HCount == null ? (object)DBNull.Value : item.TotalCall02HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall04HCount" + i, item.TotalCall04HCount == null ? (object)DBNull.Value : item.TotalCall04HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall06HCount" + i, item.TotalCall06HCount == null ? (object)DBNull.Value : item.TotalCall06HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall08HCount" + i, item.TotalCall08HCount == null ? (object)DBNull.Value : item.TotalCall08HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall10HCount" + i, item.TotalCall10HCount == null ? (object)DBNull.Value : item.TotalCall10HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall12HCount" + i, item.TotalCall12HCount == null ? (object)DBNull.Value : item.TotalCall12HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall14HCount" + i, item.TotalCall14HCount == null ? (object)DBNull.Value : item.TotalCall14HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall16HCount" + i, item.TotalCall16HCount == null ? (object)DBNull.Value : item.TotalCall16HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall18HCount" + i, item.TotalCall18HCount == null ? (object)DBNull.Value : item.TotalCall18HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall20HCount" + i, item.TotalCall20HCount == null ? (object)DBNull.Value : item.TotalCall20HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall22HCount" + i, item.TotalCall22HCount == null ? (object)DBNull.Value : item.TotalCall22HCount);
					sqlCommand.Parameters.AddWithValue("TotalCall24HCount" + i, item.TotalCall24HCount == null ? (object)DBNull.Value : item.TotalCall24HCount);
					sqlCommand.Parameters.AddWithValue("TotalCallCount" + i, item.TotalCallCount == null ? (object)DBNull.Value : item.TotalCallCount);
					sqlCommand.Parameters.AddWithValue("TotalCallDistinctUserCount" + i, item.TotalCallDistinctUserCount == null ? (object)DBNull.Value : item.TotalCallDistinctUserCount);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM perf.ApiCalls WHERE [Id]=@Id";
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

				string query = "DELETE FROM perf.ApiCalls WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<string> GetApiAreas(int NumberOfDays)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select Distinct ApiArea  from perf.ApiCalls where CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE) AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE) order by ApiArea ";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["ApiArea"] == DBNull.Value ? "" : Convert.ToString(x["ApiArea"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<string> GetApiControllers(int NumberOfDays,string ApiArea)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select Distinct ApiController  from perf.ApiCalls where ApiArea = @ApiArea and CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE) AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["ApiController"] == DBNull.Value ? "" : Convert.ToString(x["ApiController"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<string> GetApiMethods(int NumberOfDays, string ApiArea,string ApiController)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select Distinct ApiMethod  from perf.ApiCalls where ApiArea = @ApiArea and ApiController = @ApiController and CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE) AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE) order by ApiMethod";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);
				sqlCommand.Parameters.AddWithValue("ApiController", ApiController);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => x["ApiMethod"] == DBNull.Value ? "" : Convert.ToString(x["ApiMethod"])).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<ApiAreaCallsEntity> GetApiAreaCounts(int NumberOfDays)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ApiArea,sum(TotalCallCount) TotalCallCount ,SUM(TotalCallDistinctUserCount) TotalCallDistinctUserCount
								,CONVERT(VARCHAR(10), FirstCallTime, 103)  Date
								from perf.ApiCalls 
								where CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE)
								  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
								Group by ApiArea , CONVERT(VARCHAR(10), FirstCallTime, 103) 
								order by CONVERT(VARCHAR(10), FirstCallTime, 103)   desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiAreaCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiAreaCallsEntity>();
			}
		}
		public static List<ApiALLAreaCallsEntity> GetAllApiAreaCounts(int NumberOfDays)
		{
			
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ApiArea,sum(TotalCallCount) TotalCallCount ,COUNT(DISTINCT UserId) AS TotalCallDistinctUserCount
								from perf.ApiCalls 
								where CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE)
								  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
								Group by ApiArea 
								order by ApiArea   desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiALLAreaCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiALLAreaCallsEntity>();
			}
		}
		public static List<ApiAreaCallsEntity> GetSingleApiAreaCounts(int NumberOfDays,string ApiArea)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ApiArea,sum(TotalCallCount) TotalCallCount ,COUNT(DISTINCT UserId) AS TotalCallDistinctUserCount
								,CONVERT(VARCHAR(10), FirstCallTime, 103)  Date
								from perf.ApiCalls 
								where ApiArea=@ApiArea and CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE)
								  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
								Group by ApiArea , CONVERT(VARCHAR(10), FirstCallTime, 103) 
								order by CONVERT(VARCHAR(10), FirstCallTime, 103)   desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiAreaCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiAreaCallsEntity>();
			}
		}
		public static List<ApiSingleControllerCallsEntity> GetApiSingleControllersCounts(int NumberOfDays,string ApiArea,string ApiController)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"	select ApiController,sum(TotalCallCount) TotalCallCount ,COUNT(DISTINCT UserId) AS TotalCallDistinctUserCount
									,CONVERT(VARCHAR(10), FirstCallTime, 103)  Date
									from perf.ApiCalls 
									where ApiArea=@ApiArea and ApiController = @ApiController and  CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE)
									  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
									Group by ApiArea ,ApiController, CONVERT(VARCHAR(10), FirstCallTime, 103) 
									order by CONVERT(VARCHAR(10), FirstCallTime, 103)  desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);
				sqlCommand.Parameters.AddWithValue("ApiController", ApiController);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiSingleControllerCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiSingleControllerCallsEntity>();
			}
		}
		public static List<ApiAllControllerCallsEntity> GetApiAllControllersCounts(int NumberOfDays, string ApiArea)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ApiController,sum(TotalCallCount) TotalCallCount ,COUNT(DISTINCT UserId) AS TotalCallDistinctUserCount
									
									from perf.ApiCalls 
									where ApiArea=@ApiArea and  CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays, GETDATE()) AS DATE)
									  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
									Group by ApiArea ,ApiController
									order by ApiController  desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiAllControllerCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiAllControllerCallsEntity>();
			}
		}
		public static List<ApiMethodCallsEntity> GetApiMethodCounts(int NumberOfDays, string ApiArea,string ApiController,string ApiMethod)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select ApiMethod,sum(TotalCallCount) TotalCallCount ,COUNT(DISTINCT UserId) AS TotalCallDistinctUserCount
								,CONVERT(VARCHAR(10), FirstCallTime, 103)  Date
								from perf.ApiCalls 
								where ApiArea=@ApiArea and ApiController = @ApiController and ApiMethod = @ApiMethod and   CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day,@NumberOfDays, GETDATE()) AS DATE)
								  AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)
								Group by ApiArea ,ApiController,ApiMethod, CONVERT(VARCHAR(10), FirstCallTime, 103) 
								order by CONVERT(VARCHAR(10), FirstCallTime, 103)   desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);
				sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);
				sqlCommand.Parameters.AddWithValue("ApiController", ApiController);
				sqlCommand.Parameters.AddWithValue("ApiMethod", ApiMethod);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.ApiMethodCallsEntity(x)).ToList();
			}
			else
			{
				return new List<ApiMethodCallsEntity>();
			}
		}
		public static Entities.Tables.SPR.GetFirstAndLastCall GetFirstAndLastCalls(int NumberOfDays, string ApiArea, string ApiController,string ApiMethod)
		{
			string ApiAreaCondition = "";
			string ApiControllerCondition = "";
			string ApiMethodsCondition = "";
			if(ApiArea != null && ApiArea != "")
			{
				ApiAreaCondition = @"AND ApiArea =@ApiArea";
			}
			if(ApiController != null && ApiController != "")
			{
				ApiControllerCondition = @"and ApiController = @ApiController";
			}
			if(ApiMethod != null && ApiMethod != "")
			{
				ApiMethodsCondition = @"AND ApiMethod =@ApiMethod";
			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select top 1 MIN(cast(FirstCallTime as time)) FirstCallTime,MAX(cast(LastCallTime as time)) LastCallTime
									from perf.ApiCalls 
									where 
									CAST(FirstCallTime AS DATE) >= CAST(DATEADD(day, @NumberOfDays , GETDATE()) AS DATE)
									AND CAST(FirstCallTime AS DATE) <= CAST(GETDATE() AS DATE)   "
									+ ApiAreaCondition  
									+ " " + " " 
									+ ApiControllerCondition 
									+ " " + " " 
									+ ApiMethodsCondition ;

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("NumberOfDays", NumberOfDays);

				if(ApiAreaCondition != null && ApiAreaCondition != "")
				{
					sqlCommand.Parameters.AddWithValue("ApiArea", ApiArea);
				}
				if(ApiControllerCondition != null && ApiControllerCondition != "")
				{
					sqlCommand.Parameters.AddWithValue("ApiController", ApiController);
				}
				if(ApiMethodsCondition != null && ApiMethodsCondition != "")
				{
					sqlCommand.Parameters.AddWithValue("ApiMethod", ApiMethod);
				}

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.SPR.GetFirstAndLastCall(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}




		public static List<HeavlyUsedApisEntity> GetHeavlyUsedApis()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select SUM(TotalCallCount) AreasCalls,ApiArea from perf.ApiCalls group by ApiArea order by SUM(TotalCallCount) desc";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.HeavlyUsedApisEntity(x)).ToList();
			}
			else
			{
				return new List<HeavlyUsedApisEntity>();
			}
		}

		public static List<UsersMostUsingERPEntity> UsersMostUsingERP()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @"select top 5 UserId,Count(UserId) UserCount from perf.ApiCalls group by UserId order by Count(UserId) desc ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.SPR.UsersMostUsingERPEntity(x)).ToList();
			}
			else
			{
				return new List<UsersMostUsingERPEntity>();
			}
		}


		#endregion Custom Methods

	}
}
