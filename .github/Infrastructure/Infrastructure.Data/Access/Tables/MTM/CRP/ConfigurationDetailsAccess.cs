using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{

	public class ConfigurationDetailsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_ConfigurationDetails] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_ConfigurationDetails]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_CRP_ConfigurationDetails] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_ConfigurationDetails] ([CreationTime],[CreationUserId],[DepartmentId],[DepartmentName],[DepartmentWeekNumber],[HeaderId],[IsLowerThanThreshold],[Validated],[ValidatedDate],[ValidatedUserId])  VALUES (@CreationTime,@CreationUserId,@DepartmentId,@DepartmentName,@DepartmentWeekNumber,@HeaderId,@IsLowerThanThreshold,@Validated,@ValidatedDate,@ValidatedUserId); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId);
					sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName);
					sqlCommand.Parameters.AddWithValue("DepartmentWeekNumber", item.DepartmentWeekNumber);
					sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
					sqlCommand.Parameters.AddWithValue("IsLowerThanThreshold", item.IsLowerThanThreshold);
					sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
					sqlCommand.Parameters.AddWithValue("ValidatedDate", item.ValidatedDate == null ? (object)DBNull.Value : item.ValidatedDate);
					sqlCommand.Parameters.AddWithValue("ValidatedUserId", item.ValidatedUserId == null ? (object)DBNull.Value : item.ValidatedUserId);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__MTM_CRP_ConfigurationDetails] ([CreationTime],[CreationUserId],[DepartmentId],[DepartmentName],[DepartmentWeekNumber],[HeaderId],[IsLowerThanThreshold],[Validated],[ValidatedDate],[ValidatedUserId]) VALUES ( "

							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@DepartmentId" + i + ","
							+ "@DepartmentName" + i + ","
							+ "@DepartmentWeekNumber" + i + ","
							+ "@HeaderId" + i + ","
							+ "@IsLowerThanThreshold" + i + ","
							+ "@Validated" + i + ","
							+ "@ValidatedDate" + i + ","
							+ "@ValidatedUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("DepartmentWeekNumber" + i, item.DepartmentWeekNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("IsLowerThanThreshold" + i, item.IsLowerThanThreshold);
						sqlCommand.Parameters.AddWithValue("Validated" + i, item.Validated == null ? (object)DBNull.Value : item.Validated);
						sqlCommand.Parameters.AddWithValue("ValidatedDate" + i, item.ValidatedDate == null ? (object)DBNull.Value : item.ValidatedDate);
						sqlCommand.Parameters.AddWithValue("ValidatedUserId" + i, item.ValidatedUserId == null ? (object)DBNull.Value : item.ValidatedUserId);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_ConfigurationDetails] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DepartmentId]=@DepartmentId, [DepartmentName]=@DepartmentName, [DepartmentWeekNumber]=@DepartmentWeekNumber, [HeaderId]=@HeaderId, [IsLowerThanThreshold]=@IsLowerThanThreshold, [Validated]=@Validated, [ValidatedDate]=@ValidatedDate, [ValidatedUserId]=@ValidatedUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("DepartmentId", item.DepartmentId);
				sqlCommand.Parameters.AddWithValue("DepartmentName", item.DepartmentName);
				sqlCommand.Parameters.AddWithValue("DepartmentWeekNumber", item.DepartmentWeekNumber);
				sqlCommand.Parameters.AddWithValue("HeaderId", item.HeaderId);
				sqlCommand.Parameters.AddWithValue("IsLowerThanThreshold", item.IsLowerThanThreshold);
				sqlCommand.Parameters.AddWithValue("Validated", item.Validated == null ? (object)DBNull.Value : item.Validated);
				sqlCommand.Parameters.AddWithValue("ValidatedDate", item.ValidatedDate == null ? (object)DBNull.Value : item.ValidatedDate);
				sqlCommand.Parameters.AddWithValue("ValidatedUserId", item.ValidatedUserId == null ? (object)DBNull.Value : item.ValidatedUserId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__MTM_CRP_ConfigurationDetails] SET "

							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[DepartmentId]=@DepartmentId" + i + ","
							+ "[DepartmentName]=@DepartmentName" + i + ","
							+ "[DepartmentWeekNumber]=@DepartmentWeekNumber" + i + ","
							+ "[HeaderId]=@HeaderId" + i + ","
							+ "[IsLowerThanThreshold]=@IsLowerThanThreshold" + i + ","
							+ "[Validated]=@Validated" + i + ","
							+ "[ValidatedDate]=@ValidatedDate" + i + ","
							+ "[ValidatedUserId]=@ValidatedUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DepartmentId" + i, item.DepartmentId);
						sqlCommand.Parameters.AddWithValue("DepartmentName" + i, item.DepartmentName);
						sqlCommand.Parameters.AddWithValue("DepartmentWeekNumber" + i, item.DepartmentWeekNumber);
						sqlCommand.Parameters.AddWithValue("HeaderId" + i, item.HeaderId);
						sqlCommand.Parameters.AddWithValue("IsLowerThanThreshold" + i, item.IsLowerThanThreshold);
						sqlCommand.Parameters.AddWithValue("Validated" + i, item.Validated == null ? (object)DBNull.Value : item.Validated);
						sqlCommand.Parameters.AddWithValue("ValidatedDate" + i, item.ValidatedDate == null ? (object)DBNull.Value : item.ValidatedDate);
						sqlCommand.Parameters.AddWithValue("ValidatedUserId" + i, item.ValidatedUserId == null ? (object)DBNull.Value : item.ValidatedUserId);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_ConfigurationDetails] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_CRP_ConfigurationDetails] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static int DeleteByHeader(int headerId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_ConfigurationDetails] WHERE [HeaderId]=@headerId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("headerId", headerId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity> GetByHeaders(List<int> headerIds)
		{
			if(headerIds == null || headerIds.Count <= 0)
				return null;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__MTM_CRP_ConfigurationDetails] WHERE HeaderId IN ({string.Join(",", headerIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.ConfigurationDetailsEntity>();
			}
		}

		public static int GetValidationPendingDepartments()
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT COUNT(*) FROM [__MTM_CRP_ConfigurationDetails] WHERE [Validated] IS NULL OR [Validated]=0";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				return int.TryParse(sqlCommand.ExecuteScalar()?.ToString(), out var v) ? v : 0;
			}
		}
		public static int UpdateDepartmentName(int id, string name)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_ConfigurationDetails] SET [DepartmentName]=@DepartmentName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("DepartmentName", name);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int AcceptPendingDepartments(List<int> headerIds, int userId, DateTime date)
		{
			if(headerIds == null || headerIds.Count <= 0)
				return 0;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"UPDATE [__MTM_CRP_ConfigurationDetails] SET [Validated]=1, [ValidatedUserId]=@userId, [ValidatedDate]=@date WHERE [HeaderId] IN ({string.Join(",", headerIds)})";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("date", date);

				return sqlCommand.ExecuteNonQuery();
			}
		}

		#endregion
	}
}
