using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Access.Tables.BSD.ROH.OfferRequests
{
	public class OfferRequestEmailAttachmentsAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferRequestEmailAttachments] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferRequestEmailAttachments]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [OfferRequestEmailAttachments] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [OfferRequestEmailAttachments] ([FileId],[FileName],[OfferId]) OUTPUT INSERTED.[Id] VALUES (@FileId,@FileName,@OfferId); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);
					sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items)
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
						query += " INSERT INTO [OfferRequestEmailAttachments] ([FileId],[FileName],[OfferId]) VALUES ( "

							+ "@FileId" + i + ","
							+ "@FileName" + i + ","
							+ "@OfferId" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [OfferRequestEmailAttachments] SET [FileId]=@FileId, [FileName]=@FileName, [OfferId]=@OfferId WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);
				sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
				sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items)
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
						query += " UPDATE [OfferRequestEmailAttachments] SET "

							+ "[FileId]=@FileId" + i + ","
							+ "[FileName]=@FileName" + i + ","
							+ "[OfferId]=@OfferId" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
						sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
						sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
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
				string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [OfferRequestEmailAttachments] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [OfferRequestEmailAttachments]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [OfferRequestEmailAttachments] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [OfferRequestEmailAttachments] ([FileId],[FileName],[OfferId]) OUTPUT INSERTED.[Id] VALUES (@FileId,@FileName,@OfferId); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
			sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [OfferRequestEmailAttachments] ([FileId],[FileName],[OfferId]) VALUES ( "

						+ "@FileId" + i + ","
						+ "@FileName" + i + ","
						+ "@OfferId" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [OfferRequestEmailAttachments] SET [FileId]=@FileId, [FileName]=@FileName, [OfferId]=@OfferId WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("FileId", item.FileId == null ? (object)DBNull.Value : item.FileId);
			sqlCommand.Parameters.AddWithValue("FileName", item.FileName == null ? (object)DBNull.Value : item.FileName);
			sqlCommand.Parameters.AddWithValue("OfferId", item.OfferId == null ? (object)DBNull.Value : item.OfferId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [OfferRequestEmailAttachments] SET "

					+ "[FileId]=@FileId" + i + ","
					+ "[FileName]=@FileName" + i + ","
					+ "[OfferId]=@OfferId" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("FileId" + i, item.FileId == null ? (object)DBNull.Value : item.FileId);
					sqlCommand.Parameters.AddWithValue("FileName" + i, item.FileName == null ? (object)DBNull.Value : item.FileName);
					sqlCommand.Parameters.AddWithValue("OfferId" + i, item.OfferId == null ? (object)DBNull.Value : item.OfferId);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static int DeleteMultipleRowsWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [FileId] IN ({0}) ";
			string inClause = string.Join(",", ids);
			query = string.Format(query, inClause);
			var sqlCommand = new SqlCommand(query, connection, transaction);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int DeleteMultipleWithTransaction(int FileId, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [OfferRequestEmailAttachments] WHERE [FileId]=@FileId";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("FileId", FileId);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			
			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity> GetAttachmentsPerOffers(List<int> ids)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [OfferRequestEmailAttachments] WHERE [OfferId] IN ({0})";
				string inClause = string.Join(",", ids);
				query = string.Format(query, inClause);
				var sqlCommand = new SqlCommand(query, sqlConnection);
				
				
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.OfferRequestEmailAttachmentsEntity>();
			}

		}


		#endregion Custom Methods

	}
}
