using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.BSD
{

	public class ArtikelLogisticsExtensionAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelLogisticsExtension]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> get(List<int> ids)
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

					sqlCommand.CommandText = "SELECT * FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_ArtikelLogisticsExtension] ([Abladeort],[Anlieferadresse],[Anlieferadresse_Abladeort],[ArticleId],[CreateTime],[CreateUserId],[UpdateTime],[UpdateUserId],[VDALabel]) OUTPUT INSERTED.[Id] VALUES (@Abladeort,@Anlieferadresse,@Anlieferadresse_Abladeort,@ArticleId,@CreateTime,@CreateUserId,@UpdateTime,@UpdateUserId,@VDALabel); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Abladeort", item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse", item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort", item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("VDALabel", item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items)
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
						query += " INSERT INTO [__BSD_ArtikelLogisticsExtension] ([Abladeort],[Anlieferadresse],[Anlieferadresse_Abladeort],[ArticleId],[CreateTime],[CreateUserId],[UpdateTime],[UpdateUserId],[VDALabel]) VALUES ( "

							+ "@Abladeort" + i + ","
							+ "@Anlieferadresse" + i + ","
							+ "@Anlieferadresse_Abladeort" + i + ","
							+ "@ArticleId" + i + ","
							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@UpdateTime" + i + ","
							+ "@UpdateUserId" + i + ","
							+ "@VDALabel" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Abladeort" + i, item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
						sqlCommand.Parameters.AddWithValue("Anlieferadresse" + i, item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
						sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort" + i, item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
						sqlCommand.Parameters.AddWithValue("VDALabel" + i, item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_ArtikelLogisticsExtension] SET [Abladeort]=@Abladeort, [Anlieferadresse]=@Anlieferadresse, [Anlieferadresse_Abladeort]=@Anlieferadresse_Abladeort, [ArticleId]=@ArticleId, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId, [VDALabel]=@VDALabel WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Abladeort", item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
				sqlCommand.Parameters.AddWithValue("Anlieferadresse", item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
				sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort", item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
				sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
				sqlCommand.Parameters.AddWithValue("VDALabel", item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items)
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
						query += " UPDATE [__BSD_ArtikelLogisticsExtension] SET "

							+ "[Abladeort]=@Abladeort" + i + ","
							+ "[Anlieferadresse]=@Anlieferadresse" + i + ","
							+ "[Anlieferadresse_Abladeort]=@Anlieferadresse_Abladeort" + i + ","
							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[UpdateTime]=@UpdateTime" + i + ","
							+ "[UpdateUserId]=@UpdateUserId" + i + ","
							+ "[VDALabel]=@VDALabel" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Abladeort" + i, item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
						sqlCommand.Parameters.AddWithValue("Anlieferadresse" + i, item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
						sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort" + i, item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
						sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
						sqlCommand.Parameters.AddWithValue("VDALabel" + i, item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);
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
				string query = "DELETE FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_ArtikelLogisticsExtension]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [__BSD_ArtikelLogisticsExtension] ([Abladeort],[Anlieferadresse],[Anlieferadresse_Abladeort],[ArticleId],[CreateTime],[CreateUserId],[UpdateTime],[UpdateUserId],[VDALabel]) OUTPUT INSERTED.[Id] VALUES (@Abladeort,@Anlieferadresse,@Anlieferadresse_Abladeort,@ArticleId,@CreateTime,@CreateUserId,@UpdateTime,@UpdateUserId,@VDALabel); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Abladeort", item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
			sqlCommand.Parameters.AddWithValue("Anlieferadresse", item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
			sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort", item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
			sqlCommand.Parameters.AddWithValue("VDALabel", item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_ArtikelLogisticsExtension] ([Abladeort],[Anlieferadresse],[Anlieferadresse_Abladeort],[ArticleId],[CreateTime],[CreateUserId],[UpdateTime],[UpdateUserId],[VDALabel]) VALUES ( "

						+ "@Abladeort" + i + ","
						+ "@Anlieferadresse" + i + ","
						+ "@Anlieferadresse_Abladeort" + i + ","
						+ "@ArticleId" + i + ","
						+ "@CreateTime" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@UpdateTime" + i + ","
						+ "@UpdateUserId" + i + ","
						+ "@VDALabel" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Abladeort" + i, item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse" + i, item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort" + i, item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("VDALabel" + i, item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_ArtikelLogisticsExtension] SET [Abladeort]=@Abladeort, [Anlieferadresse]=@Anlieferadresse, [Anlieferadresse_Abladeort]=@Anlieferadresse_Abladeort, [ArticleId]=@ArticleId, [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [UpdateTime]=@UpdateTime, [UpdateUserId]=@UpdateUserId, [VDALabel]=@VDALabel WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Abladeort", item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
			sqlCommand.Parameters.AddWithValue("Anlieferadresse", item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
			sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort", item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("UpdateTime", item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
			sqlCommand.Parameters.AddWithValue("UpdateUserId", item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
			sqlCommand.Parameters.AddWithValue("VDALabel", item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 10; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_ArtikelLogisticsExtension] SET "

					+ "[Abladeort]=@Abladeort" + i + ","
					+ "[Anlieferadresse]=@Anlieferadresse" + i + ","
					+ "[Anlieferadresse_Abladeort]=@Anlieferadresse_Abladeort" + i + ","
					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[CreateTime]=@CreateTime" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[UpdateTime]=@UpdateTime" + i + ","
					+ "[UpdateUserId]=@UpdateUserId" + i + ","
					+ "[VDALabel]=@VDALabel" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Abladeort" + i, item.Abladeort == null ? (object)DBNull.Value : item.Abladeort);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse" + i, item.Anlieferadresse == null ? (object)DBNull.Value : item.Anlieferadresse);
					sqlCommand.Parameters.AddWithValue("Anlieferadresse_Abladeort" + i, item.Anlieferadresse_Abladeort == null ? (object)DBNull.Value : item.Anlieferadresse_Abladeort);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UpdateTime" + i, item.UpdateTime == null ? (object)DBNull.Value : item.UpdateTime);
					sqlCommand.Parameters.AddWithValue("UpdateUserId" + i, item.UpdateUserId == null ? (object)DBNull.Value : item.UpdateUserId);
					sqlCommand.Parameters.AddWithValue("VDALabel" + i, item.VDALabel == null ? (object)DBNull.Value : item.VDALabel);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_ArtikelLogisticsExtension] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity GetByArticleId(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_ArtikelLogisticsExtension] WHERE [ArticleId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static int DeleteByArticleWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_ArtikelLogisticsExtension] WHERE [ArticleId]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		#endregion
	}
}
