using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CPL
{
	public class Capital_requests_positionsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Capital_requests_positions] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Capital_requests_positions]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [Capital_requests_positions] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [Capital_requests_positions] ([CapitalBOM],[CapitalClose],[CapitalCP],[CapitalDate],[CapitalDOC],[CapitalFB],[CapitalReply],[CapitalStatus],[EngeneeringValidation],[EngeneeringValidationDate],[HeaderId],[IncidentCategory],[IncidentCategoryId],[IncidentDate],[IncidentDescription],[PositionId]) OUTPUT INSERTED.[Id] VALUES (@CapitalBOM,@CapitalClose,@CapitalCP,@CapitalDate,@CapitalDOC,@CapitalFB,@CapitalReply,@CapitalStatus,@EngeneeringValidation,@EngeneeringValidationDate,@HeaderId,@IncidentCategory,@IncidentCategoryId,@IncidentDate,@IncidentDescription,@PositionId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CapitalBOM", item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
					sqlCommand.Parameters.AddWithValue("CapitalClose", item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
					sqlCommand.Parameters.AddWithValue("CapitalCP", item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
					sqlCommand.Parameters.AddWithValue("CapitalDate", item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
					sqlCommand.Parameters.AddWithValue("CapitalDOC", item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
					sqlCommand.Parameters.AddWithValue("CapitalFB", item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
					sqlCommand.Parameters.AddWithValue("CapitalReply", item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
					sqlCommand.Parameters.AddWithValue("CapitalStatus", item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidation", item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate", item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
					sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("IncidentCategory", item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
					sqlCommand.Parameters.AddWithValue("IncidentCategoryId", item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
					sqlCommand.Parameters.AddWithValue("IncidentDate", item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
					sqlCommand.Parameters.AddWithValue("IncidentDescription", item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
					sqlCommand.Parameters.AddWithValue("PositionId", item.PositionId == null ? (object)DBNull.Value : item.PositionId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items)
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
						query += " INSERT INTO [Capital_requests_positions] ([CapitalBOM],[CapitalClose],[CapitalCP],[CapitalDate],[CapitalDOC],[CapitalFB],[CapitalReply],[CapitalStatus],[EngeneeringValidation],[EngeneeringValidationDate],[HeaderId],[IncidentCategory],[IncidentCategoryId],[IncidentDate],[IncidentDescription],[PositionId]) VALUES ( "

							+ "@CapitalBOM" + i + ","
							+ "@CapitalClose" + i + ","
							+ "@CapitalCP" + i + ","
							+ "@CapitalDate" + i + ","
							+ "@CapitalDOC" + i + ","
							+ "@CapitalFB" + i + ","
							+ "@CapitalReply" + i + ","
							+ "@CapitalStatus" + i + ","
							+ "@EngeneeringValidation" + i + ","
							+ "@EngeneeringValidationDate" + i + ","
							+ "@HeaderId" + i + ","
							+ "@IncidentCategory" + i + ","
							+ "@IncidentCategoryId" + i + ","
							+ "@IncidentDate" + i + ","
							+ "@IncidentDescription" + i + ","
							+ "@PositionId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CapitalBOM" + i, item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
						sqlCommand.Parameters.AddWithValue("CapitalClose" + i, item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
						sqlCommand.Parameters.AddWithValue("CapitalCP" + i, item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
						sqlCommand.Parameters.AddWithValue("CapitalDate" + i, item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
						sqlCommand.Parameters.AddWithValue("CapitalDOC" + i, item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
						sqlCommand.Parameters.AddWithValue("CapitalFB" + i, item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
						sqlCommand.Parameters.AddWithValue("CapitalReply" + i, item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
						sqlCommand.Parameters.AddWithValue("CapitalStatus" + i, item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
						sqlCommand.Parameters.AddWithValue("EngeneeringValidation" + i, item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
						sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate" + i, item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("IncidentCategory" + i, item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
						sqlCommand.Parameters.AddWithValue("IncidentCategoryId" + i, item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
						sqlCommand.Parameters.AddWithValue("IncidentDate" + i, item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
						sqlCommand.Parameters.AddWithValue("IncidentDescription" + i, item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
						sqlCommand.Parameters.AddWithValue("PositionId" + i, item.PositionId == null ? (object)DBNull.Value : item.PositionId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [Capital_requests_positions] SET [CapitalBOM]=@CapitalBOM, [CapitalClose]=@CapitalClose, [CapitalCP]=@CapitalCP, [CapitalDate]=@CapitalDate, [CapitalDOC]=@CapitalDOC, [CapitalFB]=@CapitalFB, [CapitalReply]=@CapitalReply, [CapitalStatus]=@CapitalStatus, [EngeneeringValidation]=@EngeneeringValidation, [EngeneeringValidationDate]=@EngeneeringValidationDate, [HeaderId]=@HeaderId, [IncidentCategory]=@IncidentCategory, [IncidentCategoryId]=@IncidentCategoryId, [IncidentDate]=@IncidentDate, [IncidentDescription]=@IncidentDescription, [PositionId]=@PositionId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CapitalBOM", item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
				sqlCommand.Parameters.AddWithValue("CapitalClose", item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
				sqlCommand.Parameters.AddWithValue("CapitalCP", item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
				sqlCommand.Parameters.AddWithValue("CapitalDate", item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
				sqlCommand.Parameters.AddWithValue("CapitalDOC", item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
				sqlCommand.Parameters.AddWithValue("CapitalFB", item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
				sqlCommand.Parameters.AddWithValue("CapitalReply", item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
				sqlCommand.Parameters.AddWithValue("CapitalStatus", item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
				sqlCommand.Parameters.AddWithValue("EngeneeringValidation", item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
				sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate", item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
				sqlCommand.Parameters.AddWithValue("IncidentCategory", item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
				sqlCommand.Parameters.AddWithValue("IncidentCategoryId", item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
				sqlCommand.Parameters.AddWithValue("IncidentDate", item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
				sqlCommand.Parameters.AddWithValue("IncidentDescription", item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
				sqlCommand.Parameters.AddWithValue("PositionId", item.PositionId == null ? (object)DBNull.Value : item.PositionId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items)
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
						query += " UPDATE [Capital_requests_positions] SET "

							+ "[CapitalBOM]=@CapitalBOM" + i + ","
							+ "[CapitalClose]=@CapitalClose" + i + ","
							+ "[CapitalCP]=@CapitalCP" + i + ","
							+ "[CapitalDate]=@CapitalDate" + i + ","
							+ "[CapitalDOC]=@CapitalDOC" + i + ","
							+ "[CapitalFB]=@CapitalFB" + i + ","
							+ "[CapitalReply]=@CapitalReply" + i + ","
							+ "[CapitalStatus]=@CapitalStatus" + i + ","
							+ "[EngeneeringValidation]=@EngeneeringValidation" + i + ","
							+ "[EngeneeringValidationDate]=@EngeneeringValidationDate" + i + ","
							+ "[HeaderId]=@HeaderId" + i + ","
							+ "[IncidentCategory]=@IncidentCategory" + i + ","
							+ "[IncidentCategoryId]=@IncidentCategoryId" + i + ","
							+ "[IncidentDate]=@IncidentDate" + i + ","
							+ "[IncidentDescription]=@IncidentDescription" + i + ","
							+ "[PositionId]=@PositionId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CapitalBOM" + i, item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
						sqlCommand.Parameters.AddWithValue("CapitalClose" + i, item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
						sqlCommand.Parameters.AddWithValue("CapitalCP" + i, item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
						sqlCommand.Parameters.AddWithValue("CapitalDate" + i, item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
						sqlCommand.Parameters.AddWithValue("CapitalDOC" + i, item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
						sqlCommand.Parameters.AddWithValue("CapitalFB" + i, item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
						sqlCommand.Parameters.AddWithValue("CapitalReply" + i, item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
						sqlCommand.Parameters.AddWithValue("CapitalStatus" + i, item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
						sqlCommand.Parameters.AddWithValue("EngeneeringValidation" + i, item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
						sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate" + i, item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
						sqlCommand.Parameters.AddWithValue("IncidentCategory" + i, item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
						sqlCommand.Parameters.AddWithValue("IncidentCategoryId" + i, item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
						sqlCommand.Parameters.AddWithValue("IncidentDate" + i, item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
						sqlCommand.Parameters.AddWithValue("IncidentDescription" + i, item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
						sqlCommand.Parameters.AddWithValue("PositionId" + i, item.PositionId == null ? (object)DBNull.Value : item.PositionId);
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
				string query = "DELETE FROM [Capital_requests_positions] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [Capital_requests_positions] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Capital_requests_positions] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [Capital_requests_positions]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [Capital_requests_positions] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [Capital_requests_positions] ([CapitalBOM],[CapitalClose],[CapitalCP],[CapitalDate],[CapitalDOC],[CapitalFB],[CapitalReply],[CapitalStatus],[EngeneeringValidation],[EngeneeringValidationDate],[HeaderId],[IncidentCategory],[IncidentCategoryId],[IncidentDate],[IncidentDescription],[PositionId]) OUTPUT INSERTED.[Id] VALUES (@CapitalBOM,@CapitalClose,@CapitalCP,@CapitalDate,@CapitalDOC,@CapitalFB,@CapitalReply,@CapitalStatus,@EngeneeringValidation,@EngeneeringValidationDate,@HeaderId,@IncidentCategory,@IncidentCategoryId,@IncidentDate,@IncidentDescription,@PositionId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CapitalBOM", item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
			sqlCommand.Parameters.AddWithValue("CapitalClose", item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
			sqlCommand.Parameters.AddWithValue("CapitalCP", item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
			sqlCommand.Parameters.AddWithValue("CapitalDate", item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
			sqlCommand.Parameters.AddWithValue("CapitalDOC", item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
			sqlCommand.Parameters.AddWithValue("CapitalFB", item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
			sqlCommand.Parameters.AddWithValue("CapitalReply", item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
			sqlCommand.Parameters.AddWithValue("CapitalStatus", item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
			sqlCommand.Parameters.AddWithValue("EngeneeringValidation", item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
			sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate", item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("IncidentCategory", item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
			sqlCommand.Parameters.AddWithValue("IncidentCategoryId", item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
			sqlCommand.Parameters.AddWithValue("IncidentDate", item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
			sqlCommand.Parameters.AddWithValue("IncidentDescription", item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
			sqlCommand.Parameters.AddWithValue("PositionId", item.PositionId == null ? (object)DBNull.Value : item.PositionId);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [Capital_requests_positions] ([CapitalBOM],[CapitalClose],[CapitalCP],[CapitalDate],[CapitalDOC],[CapitalFB],[CapitalReply],[CapitalStatus],[EngeneeringValidation],[EngeneeringValidationDate],[HeaderId],[IncidentCategory],[IncidentCategoryId],[IncidentDate],[IncidentDescription],[PositionId]) VALUES ( "

						+ "@CapitalBOM" + i + ","
						+ "@CapitalClose" + i + ","
						+ "@CapitalCP" + i + ","
						+ "@CapitalDate" + i + ","
						+ "@CapitalDOC" + i + ","
						+ "@CapitalFB" + i + ","
						+ "@CapitalReply" + i + ","
						+ "@CapitalStatus" + i + ","
						+ "@EngeneeringValidation" + i + ","
						+ "@EngeneeringValidationDate" + i + ","
						+ "@HeaderId" + i + ","
						+ "@IncidentCategory" + i + ","
						+ "@IncidentCategoryId" + i + ","
						+ "@IncidentDate" + i + ","
						+ "@IncidentDescription" + i + ","
						+ "@PositionId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CapitalBOM" + i, item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
					sqlCommand.Parameters.AddWithValue("CapitalClose" + i, item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
					sqlCommand.Parameters.AddWithValue("CapitalCP" + i, item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
					sqlCommand.Parameters.AddWithValue("CapitalDate" + i, item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
					sqlCommand.Parameters.AddWithValue("CapitalDOC" + i, item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
					sqlCommand.Parameters.AddWithValue("CapitalFB" + i, item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
					sqlCommand.Parameters.AddWithValue("CapitalReply" + i, item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
					sqlCommand.Parameters.AddWithValue("CapitalStatus" + i, item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidation" + i, item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate" + i, item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("IncidentCategory" + i, item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
					sqlCommand.Parameters.AddWithValue("IncidentCategoryId" + i, item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
					sqlCommand.Parameters.AddWithValue("IncidentDate" + i, item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
					sqlCommand.Parameters.AddWithValue("IncidentDescription" + i, item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
					sqlCommand.Parameters.AddWithValue("PositionId" + i, item.PositionId == null ? (object)DBNull.Value : item.PositionId);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [Capital_requests_positions] SET [CapitalBOM]=@CapitalBOM, [CapitalClose]=@CapitalClose, [CapitalCP]=@CapitalCP, [CapitalDate]=@CapitalDate, [CapitalDOC]=@CapitalDOC, [CapitalFB]=@CapitalFB, [CapitalReply]=@CapitalReply, [CapitalStatus]=@CapitalStatus, [EngeneeringValidation]=@EngeneeringValidation, [EngeneeringValidationDate]=@EngeneeringValidationDate, [HeaderId]=@HeaderId, [IncidentCategory]=@IncidentCategory, [IncidentCategoryId]=@IncidentCategoryId, [IncidentDate]=@IncidentDate, [IncidentDescription]=@IncidentDescription, [PositionId]=@PositionId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CapitalBOM", item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
			sqlCommand.Parameters.AddWithValue("CapitalClose", item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
			sqlCommand.Parameters.AddWithValue("CapitalCP", item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
			sqlCommand.Parameters.AddWithValue("CapitalDate", item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
			sqlCommand.Parameters.AddWithValue("CapitalDOC", item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
			sqlCommand.Parameters.AddWithValue("CapitalFB", item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
			sqlCommand.Parameters.AddWithValue("CapitalReply", item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
			sqlCommand.Parameters.AddWithValue("CapitalStatus", item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
			sqlCommand.Parameters.AddWithValue("EngeneeringValidation", item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
			sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate", item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
			sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
			sqlCommand.Parameters.AddWithValue("IncidentCategory", item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
			sqlCommand.Parameters.AddWithValue("IncidentCategoryId", item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
			sqlCommand.Parameters.AddWithValue("IncidentDate", item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
			sqlCommand.Parameters.AddWithValue("IncidentDescription", item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
			sqlCommand.Parameters.AddWithValue("PositionId", item.PositionId == null ? (object)DBNull.Value : item.PositionId);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 17; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [Capital_requests_positions] SET "

					+ "[CapitalBOM]=@CapitalBOM" + i + ","
					+ "[CapitalClose]=@CapitalClose" + i + ","
					+ "[CapitalCP]=@CapitalCP" + i + ","
					+ "[CapitalDate]=@CapitalDate" + i + ","
					+ "[CapitalDOC]=@CapitalDOC" + i + ","
					+ "[CapitalFB]=@CapitalFB" + i + ","
					+ "[CapitalReply]=@CapitalReply" + i + ","
					+ "[CapitalStatus]=@CapitalStatus" + i + ","
					+ "[EngeneeringValidation]=@EngeneeringValidation" + i + ","
					+ "[EngeneeringValidationDate]=@EngeneeringValidationDate" + i + ","
					+ "[HeaderId]=@HeaderId" + i + ","
					+ "[IncidentCategory]=@IncidentCategory" + i + ","
					+ "[IncidentCategoryId]=@IncidentCategoryId" + i + ","
					+ "[IncidentDate]=@IncidentDate" + i + ","
					+ "[IncidentDescription]=@IncidentDescription" + i + ","
					+ "[PositionId]=@PositionId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CapitalBOM" + i, item.CapitalBOM == null ? (object)DBNull.Value : item.CapitalBOM);
					sqlCommand.Parameters.AddWithValue("CapitalClose" + i, item.CapitalClose == null ? (object)DBNull.Value : item.CapitalClose);
					sqlCommand.Parameters.AddWithValue("CapitalCP" + i, item.CapitalCP == null ? (object)DBNull.Value : item.CapitalCP);
					sqlCommand.Parameters.AddWithValue("CapitalDate" + i, item.CapitalDate == null ? (object)DBNull.Value : item.CapitalDate);
					sqlCommand.Parameters.AddWithValue("CapitalDOC" + i, item.CapitalDOC == null ? (object)DBNull.Value : item.CapitalDOC);
					sqlCommand.Parameters.AddWithValue("CapitalFB" + i, item.CapitalFB == null ? (object)DBNull.Value : item.CapitalFB);
					sqlCommand.Parameters.AddWithValue("CapitalReply" + i, item.CapitalReply == null ? (object)DBNull.Value : item.CapitalReply);
					sqlCommand.Parameters.AddWithValue("CapitalStatus" + i, item.CapitalStatus == null ? (object)DBNull.Value : item.CapitalStatus);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidation" + i, item.EngeneeringValidation == null ? (object)DBNull.Value : item.EngeneeringValidation);
					sqlCommand.Parameters.AddWithValue("EngeneeringValidationDate" + i, item.EngeneeringValidationDate == null ? (object)DBNull.Value : item.EngeneeringValidationDate);
					sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId == null ? (object)DBNull.Value : item.HeaderId);
					sqlCommand.Parameters.AddWithValue("IncidentCategory" + i, item.IncidentCategory == null ? (object)DBNull.Value : item.IncidentCategory);
					sqlCommand.Parameters.AddWithValue("IncidentCategoryId" + i, item.IncidentCategoryId == null ? (object)DBNull.Value : item.IncidentCategoryId);
					sqlCommand.Parameters.AddWithValue("IncidentDate" + i, item.IncidentDate == null ? (object)DBNull.Value : item.IncidentDate);
					sqlCommand.Parameters.AddWithValue("IncidentDescription" + i, item.IncidentDescription == null ? (object)DBNull.Value : item.IncidentDescription);
					sqlCommand.Parameters.AddWithValue("PositionId" + i, item.PositionId == null ? (object)DBNull.Value : item.PositionId);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [Capital_requests_positions] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [Capital_requests_positions] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> GetByHeaderId(int headerId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [Capital_requests_positions] WHERE [HeaderId]=@headerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("headerId", headerId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity> GetByHeaderId(int headerId, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();
			string query = "SELECT * FROM [Capital_requests_positions] WHERE [HeaderId]=@headerId";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("headerId", headerId);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CPL.Capital_requests_positionsEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods

	}
}
