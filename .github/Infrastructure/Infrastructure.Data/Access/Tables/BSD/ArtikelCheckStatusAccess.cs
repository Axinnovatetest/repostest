using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class ArtikelCheckStatusAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelCheckStatus] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelCheckStatus]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_ArtikelCheckStatus] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_ArtikelCheckStatus] ([CreateTime],[CreateUserId],[Description],[Status],[UpdateTime],[UpdateUserId])  VALUES (@CreateTime,@CreateUserId,@Description,@Status,@UpdateTime,@UpdateUserId);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> items)
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
						query += " INSERT INTO [__BSD_ArtikelCheckStatus] ([CreateTime],[CreateUserId],[Description],[Status],[UpdateTime],[UpdateUserId]) VALUES ( "

							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@Description" + i + ","
							+ "@Status" + i + ","
							+ "@UpdateTime" + i + ","
							+ "@UpdateUserId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArtikelCheckStatus] SET [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [Description]=@Description, [Status]=@Status, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
				sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCheckStatusEntity> items)
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
						query += " UPDATE [__BSD_ArtikelCheckStatus] SET "

							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[UpdateTime]=@UpdateTime" + i + ","
							+ "[UpdateUserId]=@UpdateUserId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
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
				string query = "DELETE FROM [__BSD_ArtikelCheckStatus] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_ArtikelCheckStatus] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods



		#endregion
	}
}
