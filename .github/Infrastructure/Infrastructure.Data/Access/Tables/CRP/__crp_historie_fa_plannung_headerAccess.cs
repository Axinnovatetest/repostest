using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.CRP
{
	public class __crp_historie_fa_plannung_headerAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_header]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [stats].[__crp_historie_fa_plannung_header] ([DateHistorie],[DateImport],[ImportTyeName],[ImportTypeId],[importUserId],[ImportUsername]) OUTPUT INSERTED.[Id] VALUES (@DateHistorie,@DateImport,@ImportTyeName,@ImportTypeId,@importUserId,@ImportUsername); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("DateHistorie", item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
					sqlCommand.Parameters.AddWithValue("DateImport", item.DateImport == null ? (object)DBNull.Value : item.DateImport);
					sqlCommand.Parameters.AddWithValue("ImportTyeName", item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
					sqlCommand.Parameters.AddWithValue("ImportTypeId", item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
					sqlCommand.Parameters.AddWithValue("importUserId", item.importUserId == null ? (object)DBNull.Value : item.importUserId);
					sqlCommand.Parameters.AddWithValue("ImportUsername", item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items)
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
						query += " INSERT INTO [stats].[__crp_historie_fa_plannung_header] ([DateHistorie],[DateImport],[ImportTyeName],[ImportTypeId],[importUserId],[ImportUsername]) VALUES ( "

							+ "@DateHistorie" + i + ","
							+ "@DateImport" + i + ","
							+ "@ImportTyeName" + i + ","
							+ "@ImportTypeId" + i + ","
							+ "@importUserId" + i + ","
							+ "@ImportUsername" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("DateHistorie" + i, item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
						sqlCommand.Parameters.AddWithValue("DateImport" + i, item.DateImport == null ? (object)DBNull.Value : item.DateImport);
						sqlCommand.Parameters.AddWithValue("ImportTyeName" + i, item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
						sqlCommand.Parameters.AddWithValue("ImportTypeId" + i, item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
						sqlCommand.Parameters.AddWithValue("importUserId" + i, item.importUserId == null ? (object)DBNull.Value : item.importUserId);
						sqlCommand.Parameters.AddWithValue("ImportUsername" + i, item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [stats].[__crp_historie_fa_plannung_header] SET [DateHistorie]=@DateHistorie, [DateImport]=@DateImport, [ImportTyeName]=@ImportTyeName, [ImportTypeId]=@ImportTypeId, [importUserId]=@importUserId, [ImportUsername]=@ImportUsername WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("DateHistorie", item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
				sqlCommand.Parameters.AddWithValue("DateImport", item.DateImport == null ? (object)DBNull.Value : item.DateImport);
				sqlCommand.Parameters.AddWithValue("ImportTyeName", item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
				sqlCommand.Parameters.AddWithValue("ImportTypeId", item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
				sqlCommand.Parameters.AddWithValue("importUserId", item.importUserId == null ? (object)DBNull.Value : item.importUserId);
				sqlCommand.Parameters.AddWithValue("ImportUsername", item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items)
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
						query += " UPDATE [stats].[__crp_historie_fa_plannung_header] SET "

							+ "[DateHistorie]=@DateHistorie" + i + ","
							+ "[DateImport]=@DateImport" + i + ","
							+ "[ImportTyeName]=@ImportTyeName" + i + ","
							+ "[ImportTypeId]=@ImportTypeId" + i + ","
							+ "[importUserId]=@importUserId" + i + ","
							+ "[ImportUsername]=@ImportUsername" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("DateHistorie" + i, item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
						sqlCommand.Parameters.AddWithValue("DateImport" + i, item.DateImport == null ? (object)DBNull.Value : item.DateImport);
						sqlCommand.Parameters.AddWithValue("ImportTyeName" + i, item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
						sqlCommand.Parameters.AddWithValue("ImportTypeId" + i, item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
						sqlCommand.Parameters.AddWithValue("importUserId" + i, item.importUserId == null ? (object)DBNull.Value : item.importUserId);
						sqlCommand.Parameters.AddWithValue("ImportUsername" + i, item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);
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
				string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [stats].[__crp_historie_fa_plannung_header]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [stats].[__crp_historie_fa_plannung_header] ([DateHistorie],[DateImport],[ImportTyeName],[ImportTypeId],[importUserId],[ImportUsername]) OUTPUT INSERTED.[Id] VALUES (@DateHistorie,@DateImport,@ImportTyeName,@ImportTypeId,@importUserId,@ImportUsername); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("DateHistorie", item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
			sqlCommand.Parameters.AddWithValue("DateImport", item.DateImport == null ? (object)DBNull.Value : item.DateImport);
			sqlCommand.Parameters.AddWithValue("ImportTyeName", item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
			sqlCommand.Parameters.AddWithValue("ImportTypeId", item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
			sqlCommand.Parameters.AddWithValue("importUserId", item.importUserId == null ? (object)DBNull.Value : item.importUserId);
			sqlCommand.Parameters.AddWithValue("ImportUsername", item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [stats].[__crp_historie_fa_plannung_header] ([DateHistorie],[DateImport],[ImportTyeName],[ImportTypeId],[importUserId],[ImportUsername]) VALUES ( "

						+ "@DateHistorie" + i + ","
						+ "@DateImport" + i + ","
						+ "@ImportTyeName" + i + ","
						+ "@ImportTypeId" + i + ","
						+ "@importUserId" + i + ","
						+ "@ImportUsername" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("DateHistorie" + i, item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
					sqlCommand.Parameters.AddWithValue("DateImport" + i, item.DateImport == null ? (object)DBNull.Value : item.DateImport);
					sqlCommand.Parameters.AddWithValue("ImportTyeName" + i, item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
					sqlCommand.Parameters.AddWithValue("ImportTypeId" + i, item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
					sqlCommand.Parameters.AddWithValue("importUserId" + i, item.importUserId == null ? (object)DBNull.Value : item.importUserId);
					sqlCommand.Parameters.AddWithValue("ImportUsername" + i, item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [stats].[__crp_historie_fa_plannung_header] SET [DateHistorie]=@DateHistorie, [DateImport]=@DateImport, [ImportTyeName]=@ImportTyeName, [ImportTypeId]=@ImportTypeId, [importUserId]=@importUserId, [ImportUsername]=@ImportUsername WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("DateHistorie", item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
			sqlCommand.Parameters.AddWithValue("DateImport", item.DateImport == null ? (object)DBNull.Value : item.DateImport);
			sqlCommand.Parameters.AddWithValue("ImportTyeName", item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
			sqlCommand.Parameters.AddWithValue("ImportTypeId", item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
			sqlCommand.Parameters.AddWithValue("importUserId", item.importUserId == null ? (object)DBNull.Value : item.importUserId);
			sqlCommand.Parameters.AddWithValue("ImportUsername", item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 7; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [stats].[__crp_historie_fa_plannung_header] SET "

					+ "[DateHistorie]=@DateHistorie" + i + ","
					+ "[DateImport]=@DateImport" + i + ","
					+ "[ImportTyeName]=@ImportTyeName" + i + ","
					+ "[ImportTypeId]=@ImportTypeId" + i + ","
					+ "[importUserId]=@importUserId" + i + ","
					+ "[ImportUsername]=@ImportUsername" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("DateHistorie" + i, item.DateHistorie == null ? (object)DBNull.Value : item.DateHistorie);
					sqlCommand.Parameters.AddWithValue("DateImport" + i, item.DateImport == null ? (object)DBNull.Value : item.DateImport);
					sqlCommand.Parameters.AddWithValue("ImportTyeName" + i, item.ImportTyeName == null ? (object)DBNull.Value : item.ImportTyeName);
					sqlCommand.Parameters.AddWithValue("ImportTypeId" + i, item.ImportTypeId == null ? (object)DBNull.Value : item.ImportTypeId);
					sqlCommand.Parameters.AddWithValue("importUserId" + i, item.importUserId == null ? (object)DBNull.Value : item.importUserId);
					sqlCommand.Parameters.AddWithValue("ImportUsername" + i, item.ImportUsername == null ? (object)DBNull.Value : item.ImportUsername);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [stats].[__crp_historie_fa_plannung_header] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity> GetHistorieHeaders(DateTime? from, DateTime? to,
			Settings.PaginModel paging, Settings.SortingModel sorting)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [stats].[__crp_historie_fa_plannung_header]";

				var isFirstClause = true;
				if(from is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} DateHistorie>= '{from}'";
					isFirstClause = false;
				}
				if(to is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} DateHistorie<= '{to}'";
					isFirstClause = false;
				}

				query += $" ORDER BY {(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName)
					? $"{sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}"
					: "DateHistorie")} {(paging is null ? ""
					: $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ")}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CRP.__crp_historie_fa_plannung_headerEntity>();
			}
		}
		public static int GetHistorieHeadersCount(DateTime? from, DateTime? to)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $@"SELECT COUNT(*) FROM [stats].[__crp_historie_fa_plannung_header]";

				var isFirstClause = true;
				if(from is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} DateHistorie>= '{from}'";
					isFirstClause = false;
				}
				if(to is not null)
				{
					query += $"{(isFirstClause ? " WHERE" : " AND")} DateHistorie<= '{to}'";
					isFirstClause = false;
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out int count) ? count : 0;
			}
		}
		public static DateTime? GetHistorieFaPlannungAgentLastExcutionTime(bool onlyForced = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = $"SELECT MAX(DateImport) FROM [stats].[__crp_historie_fa_plannung_header] WHERE ImportTypeId {(onlyForced ? " = 2" : "in (1,2)")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.CommandTimeout = 90; // sec
				return DateTime.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out DateTime date) ? date : null;
			}
		}
		#endregion Custom Methods
	}
}