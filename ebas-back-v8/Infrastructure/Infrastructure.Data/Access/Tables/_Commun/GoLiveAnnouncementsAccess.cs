using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables._Commun
{
	public class GoLiveAnnouncementsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [GoLiveAnnouncements] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [GoLiveAnnouncements]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [GoLiveAnnouncements] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [GoLiveAnnouncements] ([CreatedAt],[CreatedBy],[EndDate],[IsActive],[Message],[StartDate],[Title]) OUTPUT INSERTED.[Id] VALUES (@CreatedAt,@CreatedBy,@EndDate,@IsActive,@Message,@StartDate,@Title); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreatedAt", item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("CreatedBy", item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
					sqlCommand.Parameters.AddWithValue("EndDate", item.EndDate == null ? (object)DBNull.Value : item.EndDate);
					sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
					sqlCommand.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
					sqlCommand.Parameters.AddWithValue("StartDate", item.StartDate == null ? (object)DBNull.Value : item.StartDate);
					sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items)
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
						query += " INSERT INTO [GoLiveAnnouncements] ([CreatedAt],[CreatedBy],[EndDate],[IsActive],[Message],[StartDate],[Title]) VALUES ( "

							+ "@CreatedAt" + i + ","
							+ "@CreatedBy" + i + ","
							+ "@EndDate" + i + ","
							+ "@IsActive" + i + ","
							+ "@Message" + i + ","
							+ "@StartDate" + i + ","
							+ "@Title" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
						sqlCommand.Parameters.AddWithValue("CreatedBy" + i, item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
						sqlCommand.Parameters.AddWithValue("EndDate" + i, item.EndDate == null ? (object)DBNull.Value : item.EndDate);
						sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
						sqlCommand.Parameters.AddWithValue("Message" + i, item.Message == null ? (object)DBNull.Value : item.Message);
						sqlCommand.Parameters.AddWithValue("StartDate" + i, item.StartDate == null ? (object)DBNull.Value : item.StartDate);
						sqlCommand.Parameters.AddWithValue("Title" + i, item.Title == null ? (object)DBNull.Value : item.Title);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [GoLiveAnnouncements] SET [CreatedAt]=@CreatedAt, [CreatedBy]=@CreatedBy, [EndDate]=@EndDate, [IsActive]=@IsActive, [Message]=@Message, [StartDate]=@StartDate, [Title]=@Title WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreatedAt", item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
				sqlCommand.Parameters.AddWithValue("CreatedBy", item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
				sqlCommand.Parameters.AddWithValue("EndDate", item.EndDate == null ? (object)DBNull.Value : item.EndDate);
				sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
				sqlCommand.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
				sqlCommand.Parameters.AddWithValue("StartDate", item.StartDate == null ? (object)DBNull.Value : item.StartDate);
				sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items)
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
						query += " UPDATE [GoLiveAnnouncements] SET "

							+ "[CreatedAt]=@CreatedAt" + i + ","
							+ "[CreatedBy]=@CreatedBy" + i + ","
							+ "[EndDate]=@EndDate" + i + ","
							+ "[IsActive]=@IsActive" + i + ","
							+ "[Message]=@Message" + i + ","
							+ "[StartDate]=@StartDate" + i + ","
							+ "[Title]=@Title" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
						sqlCommand.Parameters.AddWithValue("CreatedBy" + i, item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
						sqlCommand.Parameters.AddWithValue("EndDate" + i, item.EndDate == null ? (object)DBNull.Value : item.EndDate);
						sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
						sqlCommand.Parameters.AddWithValue("Message" + i, item.Message == null ? (object)DBNull.Value : item.Message);
						sqlCommand.Parameters.AddWithValue("StartDate" + i, item.StartDate == null ? (object)DBNull.Value : item.StartDate);
						sqlCommand.Parameters.AddWithValue("Title" + i, item.Title == null ? (object)DBNull.Value : item.Title);
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
				string query = "DELETE FROM [GoLiveAnnouncements] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [GoLiveAnnouncements] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [GoLiveAnnouncements] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [GoLiveAnnouncements]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [GoLiveAnnouncements] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [GoLiveAnnouncements] ([CreatedAt],[CreatedBy],[EndDate],[IsActive],[Message],[StartDate],[Title]) OUTPUT INSERTED.[Id] VALUES (@CreatedAt,@CreatedBy,@EndDate,@IsActive,@Message,@StartDate,@Title); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CreatedAt", item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
			sqlCommand.Parameters.AddWithValue("CreatedBy", item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
			sqlCommand.Parameters.AddWithValue("EndDate", item.EndDate == null ? (object)DBNull.Value : item.EndDate);
			sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
			sqlCommand.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
			sqlCommand.Parameters.AddWithValue("StartDate", item.StartDate == null ? (object)DBNull.Value : item.StartDate);
			sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [GoLiveAnnouncements] ([CreatedAt],[CreatedBy],[EndDate],[IsActive],[Message],[StartDate],[Title]) VALUES ( "

						+ "@CreatedAt" + i + ","
						+ "@CreatedBy" + i + ","
						+ "@EndDate" + i + ","
						+ "@IsActive" + i + ","
						+ "@Message" + i + ","
						+ "@StartDate" + i + ","
						+ "@Title" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("CreatedBy" + i, item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
					sqlCommand.Parameters.AddWithValue("EndDate" + i, item.EndDate == null ? (object)DBNull.Value : item.EndDate);
					sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
					sqlCommand.Parameters.AddWithValue("Message" + i, item.Message == null ? (object)DBNull.Value : item.Message);
					sqlCommand.Parameters.AddWithValue("StartDate" + i, item.StartDate == null ? (object)DBNull.Value : item.StartDate);
					sqlCommand.Parameters.AddWithValue("Title" + i, item.Title == null ? (object)DBNull.Value : item.Title);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [GoLiveAnnouncements] SET [CreatedAt]=@CreatedAt, [CreatedBy]=@CreatedBy, [EndDate]=@EndDate, [IsActive]=@IsActive, [Message]=@Message, [StartDate]=@StartDate, [Title]=@Title WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CreatedAt", item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
			sqlCommand.Parameters.AddWithValue("CreatedBy", item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
			sqlCommand.Parameters.AddWithValue("EndDate", item.EndDate == null ? (object)DBNull.Value : item.EndDate);
			sqlCommand.Parameters.AddWithValue("IsActive", item.IsActive == null ? (object)DBNull.Value : item.IsActive);
			sqlCommand.Parameters.AddWithValue("Message", item.Message == null ? (object)DBNull.Value : item.Message);
			sqlCommand.Parameters.AddWithValue("StartDate", item.StartDate == null ? (object)DBNull.Value : item.StartDate);
			sqlCommand.Parameters.AddWithValue("Title", item.Title == null ? (object)DBNull.Value : item.Title);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 8; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [GoLiveAnnouncements] SET "

					+ "[CreatedAt]=@CreatedAt" + i + ","
					+ "[CreatedBy]=@CreatedBy" + i + ","
					+ "[EndDate]=@EndDate" + i + ","
					+ "[IsActive]=@IsActive" + i + ","
					+ "[Message]=@Message" + i + ","
					+ "[StartDate]=@StartDate" + i + ","
					+ "[Title]=@Title" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CreatedAt" + i, item.CreatedAt == null ? (object)DBNull.Value : item.CreatedAt);
					sqlCommand.Parameters.AddWithValue("CreatedBy" + i, item.CreatedBy == null ? (object)DBNull.Value : item.CreatedBy);
					sqlCommand.Parameters.AddWithValue("EndDate" + i, item.EndDate == null ? (object)DBNull.Value : item.EndDate);
					sqlCommand.Parameters.AddWithValue("IsActive" + i, item.IsActive == null ? (object)DBNull.Value : item.IsActive);
					sqlCommand.Parameters.AddWithValue("Message" + i, item.Message == null ? (object)DBNull.Value : item.Message);
					sqlCommand.Parameters.AddWithValue("StartDate" + i, item.StartDate == null ? (object)DBNull.Value : item.StartDate);
					sqlCommand.Parameters.AddWithValue("Title" + i, item.Title == null ? (object)DBNull.Value : item.Title);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [GoLiveAnnouncements] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [GoLiveAnnouncements] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity GetById(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [GoLiveAnnouncements] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		// 07/05 only 1 release is actve 
		public static void DeactivateAllExcept(int activeId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();

				var query = @"
			UPDATE [GoLiveAnnouncements]
			SET [IsActive] = 0
			WHERE [Id] <> @Id AND [IsActive] = 1";

				using(var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@Id", activeId);
					DbExecution.ExecuteNonQuery(sqlCommand);
				}
			}
		}


		#endregion Custom Methods

	}
}
