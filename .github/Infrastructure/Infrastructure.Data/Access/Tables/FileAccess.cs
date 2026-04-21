using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables
{
	public class FileAccess
	{
		#region Default Methods
		public static Data.Entities.Tables.FileEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__File] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.FileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Data.Entities.Tables.FileEntity> Get()
		{
			var dataTable = new DataTable();

			using(SqlConnection sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__File]";
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Data.Entities.Tables.FileEntity>();
			}
		}
		public static List<Data.Entities.Tables.FileEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int MAX_Query_Number = Access.Settings.MAX_BATCH_SIZE;
				List<Data.Entities.Tables.FileEntity> results = new List<Entities.Tables.FileEntity>();
				if(ids.Count <= MAX_Query_Number)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / MAX_Query_Number;
					results = new List<Data.Entities.Tables.FileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * MAX_Query_Number, MAX_Query_Number)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * MAX_Query_Number, ids.Count - batchNumber * MAX_Query_Number)));
				}
				return results;
			}
			return new List<Data.Entities.Tables.FileEntity>();
		}
		private static List<Data.Entities.Tables.FileEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [__File] WHERE [Id] IN (" + queryIds + ")";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Data.Entities.Tables.FileEntity>();
				}
			}
			return new List<Data.Entities.Tables.FileEntity>();
		}

		public static int Insert(Data.Entities.Tables.FileEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__File] ([CreationTime],[CreationUserId],[ArchiveTime],[ArchiveUserId],[Extension],[Name],[Path],[Title],[Module]) VALUES (@CreationTime,@CreationUserId,@ArchiveTime,@ArchiveUserId,@Extension,@Name,@Path,@Title,@Module);";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Extension", item.Extension);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);
					sqlCommand.Parameters.AddWithValue("Path", item.Path);
					sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);
					sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Data.Entities.Tables.FileEntity> items)
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
		private static int insert(List<Data.Entities.Tables.FileEntity> items)
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
						query += " INSERT INTO [__File] ([ArchiveTime],[ArchiveUserId],[CreationTime],[CreationUserId],[Extension],[Module],[Name],[Path],[Title]) VALUES ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@Extension" + i + ","
							+ "@Module" + i + ","
							+ "@Name" + i + ","
							+ "@Path" + i + ","
							+ "@Title" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("Extension" + i, item.Extension);
						sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
						sqlCommand.Parameters.AddWithValue("Path" + i, item.Path);
						sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(Data.Entities.Tables.FileEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__File] SET [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Extension]=@Extension, [Name]=@Name, [Path]=@Path, [Title]=@Title,[Module]=@Module WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Extension", item.Extension);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);
				sqlCommand.Parameters.AddWithValue("Path", item.Path);
				sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);
				//
				sqlCommand.Parameters.AddWithValue("Module", item.Title == null ? (object)DBNull.Value : item.Module);
				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;
			using(SqlConnection sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__File] WHERE [Id]=@Id";
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
				int MAX_Params_Number = Access.Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= MAX_Params_Number)
				{
					results = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / MAX_Params_Number;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * MAX_Params_Number, MAX_Params_Number));
					}
					results += delete(ids.GetRange(batchNumber * MAX_Params_Number, ids.Count - batchNumber * MAX_Params_Number));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				using(SqlConnection sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [__File] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FileEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__File] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FileEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__File]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FileEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FileEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FileEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FileEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FileEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FileEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FileEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__File] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FileEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FileEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FileEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__File] ([ArchiveTime],[ArchiveUserId],[CreationTime],[CreationUserId],[Extension],[Module],[Name],[Path],[Title]) OUTPUT INSERTED.[Id] VALUES (@ArchiveTime,@ArchiveUserId,@CreationTime,@CreationUserId,@Extension,@Module,@Name,@Path,@Title); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Extension", item.Extension);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("Name", item.Name);
			sqlCommand.Parameters.AddWithValue("Path", item.Path);
			sqlCommand.Parameters.AddWithValue("Title", item.Title);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FileEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__File] ([ArchiveTime],[ArchiveUserId],[CreationTime],[CreationUserId],[Extension],[Module],[Name],[Path],[Title]) VALUES ( "

						+ "@ArchiveTime" + i + ","
						+ "@ArchiveUserId" + i + ","
						+ "@CreationTime" + i + ","
						+ "@CreationUserId" + i + ","
						+ "@Extension" + i + ","
						+ "@Module" + i + ","
						+ "@Name" + i + ","
						+ "@Path" + i + ","
						+ "@Title" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Extension" + i, item.Extension);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					sqlCommand.Parameters.AddWithValue("Path" + i, item.Path);
					sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FileEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__File] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [Extension]=@Extension, [Module]=@Module, [Name]=@Name, [Path]=@Path, [Title]=@Title WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
			sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("Extension", item.Extension);
			sqlCommand.Parameters.AddWithValue("Module", item.Module == null ? (object)DBNull.Value : item.Module);
			sqlCommand.Parameters.AddWithValue("Name", item.Name);
			sqlCommand.Parameters.AddWithValue("Path", item.Path);
			sqlCommand.Parameters.AddWithValue("Title", item.Title);

					results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FileEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__File] SET "

					+ "[ArchiveTime]=@ArchiveTime" + i + ","
					+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
					+ "[CreationTime]=@CreationTime" + i + ","
					+ "[CreationUserId]=@CreationUserId" + i + ","
					+ "[Extension]=@Extension" + i + ","
					+ "[Module]=@Module" + i + ","
					+ "[Name]=@Name" + i + ","
					+ "[Path]=@Path" + i + ","
					+ "[Title]=@Title" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("Extension" + i, item.Extension);
					sqlCommand.Parameters.AddWithValue("Module" + i, item.Module == null ? (object)DBNull.Value : item.Module);
					sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					sqlCommand.Parameters.AddWithValue("Path" + i, item.Path);
					sqlCommand.Parameters.AddWithValue("Title" + i, item.Title);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__File] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__File] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion

		#region Custom Methods
		public static Data.Entities.Tables.FileEntity GetByName(string name)
		{
			if(string.IsNullOrEmpty(name))
			{
				return null;
			}

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__File] WHERE [Name]=@name";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.FileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static Data.Entities.Tables.FileEntity GetByModuleType(int module)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__File] WHERE [Module]=@module";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("module", module);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Data.Entities.Tables.FileEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Helpers
		private static List<Data.Entities.Tables.FileEntity> toList(DataTable dataTable)
		{
			var list = new List<Data.Entities.Tables.FileEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				list.Add(new Data.Entities.Tables.FileEntity(dataRow));
			}
			return list;
		}
		#endregion
	}
}
