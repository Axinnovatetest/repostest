using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static Infrastructure.Data.Access.Settings;

namespace Infrastructure.Data.Access.Tables.BSD.BomChangeRequests
{
	public class __BSD_BomChangesRequestsAccess
	{
		#region Default Methods
		public static List<Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_BomChangesRequests]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
			}
		}
		public static Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__BSD_BomChangesRequests] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__BSD_BomChangesRequests] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__BSD_BomChangesRequests] ([AcceptanceDate],[Artikel_Nr],[Artikelnummer],[Comments],[Deleted_date],[Deleted_user_id],[Deleted_username],[Is_deleted],[Reason],[RejectionDate],[RejectionReason],[RequestDate],[Requester_email],[Requester_id],[Requester_name],[Status],[SubmissionDate],[Validator_id]) OUTPUT INSERTED.[Id] VALUES (@AcceptanceDate,@Artikel_Nr,@Artikelnummer,@Comments,@Deleted_date,@Deleted_user_id,@Deleted_username,@Is_deleted,@Reason,@RejectionDate,@RejectionReason,@RequestDate,@Requester_email,@Requester_id,@Requester_name,@Status,@SubmissionDate,@Validator_id); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AcceptanceDate", item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Comments", item.Comments == null ? (object)DBNull.Value : item.Comments);
					sqlCommand.Parameters.AddWithValue("Deleted_date", item.Deleted_date);
					sqlCommand.Parameters.AddWithValue("Deleted_user_id", item.Deleted_user_id);
					sqlCommand.Parameters.AddWithValue("Deleted_username", item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
					sqlCommand.Parameters.AddWithValue("Is_deleted", item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
					sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
					sqlCommand.Parameters.AddWithValue("RejectionDate", item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
					sqlCommand.Parameters.AddWithValue("RejectionReason", item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
					sqlCommand.Parameters.AddWithValue("RequestDate", item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
					sqlCommand.Parameters.AddWithValue("Requester_email", item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
					sqlCommand.Parameters.AddWithValue("Requester_id", item.Requester_id);
					sqlCommand.Parameters.AddWithValue("Requester_name", item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
					sqlCommand.Parameters.AddWithValue("Status", item.Status);
					sqlCommand.Parameters.AddWithValue("SubmissionDate", item.SubmissionDate);
					sqlCommand.Parameters.AddWithValue("Validator_id", item.Validator_id);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items)
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
						query += " INSERT INTO [__BSD_BomChangesRequests] ([AcceptanceDate],[Artikel_Nr],[Artikelnummer],[Comments],[Deleted_date],[Deleted_user_id],[Deleted_username],[Is_deleted],[Reason],[RejectionDate],[RejectionReason],[RequestDate],[Requester_email],[Requester_id],[Requester_name],[Status],[SubmissionDate],[Validator_id]) VALUES ( "

							+ "@AcceptanceDate" + i + ","
							+ "@Artikel_Nr" + i + ","
							+ "@Artikelnummer" + i + ","
							+ "@Comments" + i + ","
							+ "@Deleted_date" + i + ","
							+ "@Deleted_user_id" + i + ","
							+ "@Deleted_username" + i + ","
							+ "@Is_deleted" + i + ","
							+ "@Reason" + i + ","
							+ "@RejectionDate" + i + ","
							+ "@RejectionReason" + i + ","
							+ "@RequestDate" + i + ","
							+ "@Requester_email" + i + ","
							+ "@Requester_id" + i + ","
							+ "@Requester_name" + i + ","
							+ "@Status" + i + ","
							+ "@SubmissionDate" + i + ","
							+ "@Validator_id" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AcceptanceDate" + i, item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Comments" + i, item.Comments == null ? (object)DBNull.Value : item.Comments);
						sqlCommand.Parameters.AddWithValue("Deleted_date" + i, item.Deleted_date);
						sqlCommand.Parameters.AddWithValue("Deleted_user_id" + i, item.Deleted_user_id);
						sqlCommand.Parameters.AddWithValue("Deleted_username" + i, item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
						sqlCommand.Parameters.AddWithValue("Is_deleted" + i, item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
						sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
						sqlCommand.Parameters.AddWithValue("RejectionDate" + i, item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
						sqlCommand.Parameters.AddWithValue("RejectionReason" + i, item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
						sqlCommand.Parameters.AddWithValue("RequestDate" + i, item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
						sqlCommand.Parameters.AddWithValue("Requester_email" + i, item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
						sqlCommand.Parameters.AddWithValue("Requester_id" + i, item.Requester_id);
						sqlCommand.Parameters.AddWithValue("Requester_name" + i, item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
						sqlCommand.Parameters.AddWithValue("SubmissionDate" + i, item.SubmissionDate);
						sqlCommand.Parameters.AddWithValue("Validator_id" + i, item.Validator_id);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__BSD_BomChangesRequests] SET [AcceptanceDate]=@AcceptanceDate, [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Comments]=@Comments, [Deleted_date]=@Deleted_date, [Deleted_user_id]=@Deleted_user_id, [Deleted_username]=@Deleted_username, [Is_deleted]=@Is_deleted, [Reason]=@Reason, [RejectionDate]=@RejectionDate, [RejectionReason]=@RejectionReason, [RequestDate]=@RequestDate, [Requester_email]=@Requester_email, [Requester_id]=@Requester_id, [Requester_name]=@Requester_name, [Status]=@Status, [SubmissionDate]=@SubmissionDate, [Validator_id]=@Validator_id WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AcceptanceDate", item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
				sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
				sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
				sqlCommand.Parameters.AddWithValue("Comments", item.Comments == null ? (object)DBNull.Value : item.Comments);
				sqlCommand.Parameters.AddWithValue("Deleted_date", item.Deleted_date);
				sqlCommand.Parameters.AddWithValue("Deleted_user_id", item.Deleted_user_id);
				sqlCommand.Parameters.AddWithValue("Deleted_username", item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
				sqlCommand.Parameters.AddWithValue("Is_deleted", item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
				sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
				sqlCommand.Parameters.AddWithValue("RejectionDate", item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
				sqlCommand.Parameters.AddWithValue("RejectionReason", item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
				sqlCommand.Parameters.AddWithValue("RequestDate", item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
				sqlCommand.Parameters.AddWithValue("Requester_email", item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
				sqlCommand.Parameters.AddWithValue("Requester_id", item.Requester_id);
				sqlCommand.Parameters.AddWithValue("Requester_name", item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
				sqlCommand.Parameters.AddWithValue("Status", item.Status);
				sqlCommand.Parameters.AddWithValue("SubmissionDate", item.SubmissionDate);
				sqlCommand.Parameters.AddWithValue("Validator_id", item.Validator_id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items)
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
						query += " UPDATE [__BSD_BomChangesRequests] SET "

							+ "[AcceptanceDate]=@AcceptanceDate" + i + ","
							+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
							+ "[Artikelnummer]=@Artikelnummer" + i + ","
							+ "[Comments]=@Comments" + i + ","
							+ "[Deleted_date]=@Deleted_date" + i + ","
							+ "[Deleted_user_id]=@Deleted_user_id" + i + ","
							+ "[Deleted_username]=@Deleted_username" + i + ","
							+ "[Is_deleted]=@Is_deleted" + i + ","
							+ "[Reason]=@Reason" + i + ","
							+ "[RejectionDate]=@RejectionDate" + i + ","
							+ "[RejectionReason]=@RejectionReason" + i + ","
							+ "[RequestDate]=@RequestDate" + i + ","
							+ "[Requester_email]=@Requester_email" + i + ","
							+ "[Requester_id]=@Requester_id" + i + ","
							+ "[Requester_name]=@Requester_name" + i + ","
							+ "[Status]=@Status" + i + ","
							+ "[SubmissionDate]=@SubmissionDate" + i + ","
							+ "[Validator_id]=@Validator_id" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AcceptanceDate" + i, item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
						sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
						sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
						sqlCommand.Parameters.AddWithValue("Comments" + i, item.Comments == null ? (object)DBNull.Value : item.Comments);
						sqlCommand.Parameters.AddWithValue("Deleted_date" + i, item.Deleted_date);
						sqlCommand.Parameters.AddWithValue("Deleted_user_id" + i, item.Deleted_user_id);
						sqlCommand.Parameters.AddWithValue("Deleted_username" + i, item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
						sqlCommand.Parameters.AddWithValue("Is_deleted" + i, item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
						sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
						sqlCommand.Parameters.AddWithValue("RejectionDate" + i, item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
						sqlCommand.Parameters.AddWithValue("RejectionReason" + i, item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
						sqlCommand.Parameters.AddWithValue("RequestDate" + i, item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
						sqlCommand.Parameters.AddWithValue("Requester_email" + i, item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
						sqlCommand.Parameters.AddWithValue("Requester_id" + i, item.Requester_id);
						sqlCommand.Parameters.AddWithValue("Requester_name" + i, item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
						sqlCommand.Parameters.AddWithValue("SubmissionDate" + i, item.SubmissionDate);
						sqlCommand.Parameters.AddWithValue("Validator_id" + i, item.Validator_id);
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
				string query = "DELETE FROM [__BSD_BomChangesRequests] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__BSD_BomChangesRequests] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_BomChangesRequests] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__BSD_BomChangesRequests]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_BomChangesRequests] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__BSD_BomChangesRequests] ([AcceptanceDate],[Artikel_Nr],[Artikelnummer],[Comments],[Deleted_date],[Deleted_user_id],[Deleted_username],[Is_deleted],[Reason],[RejectionDate],[RejectionReason],[RequestDate],[Requester_email],[Requester_id],[Requester_name],[Status],[SubmissionDate],[Validator_id]) OUTPUT INSERTED.[Id] VALUES (@AcceptanceDate,@Artikel_Nr,@Artikelnummer,@Comments,@Deleted_date,@Deleted_user_id,@Deleted_username,@Is_deleted,@Reason,@RejectionDate,@RejectionReason,@RequestDate,@Requester_email,@Requester_id,@Requester_name,@Status,@SubmissionDate,@Validator_id); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AcceptanceDate", item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Comments", item.Comments == null ? (object)DBNull.Value : item.Comments);
			sqlCommand.Parameters.AddWithValue("Deleted_date", item.Deleted_date);
			sqlCommand.Parameters.AddWithValue("Deleted_user_id", item.Deleted_user_id);
			sqlCommand.Parameters.AddWithValue("Deleted_username", item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
			sqlCommand.Parameters.AddWithValue("Is_deleted", item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
			sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
			sqlCommand.Parameters.AddWithValue("RejectionDate", item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
			sqlCommand.Parameters.AddWithValue("RejectionReason", item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
			sqlCommand.Parameters.AddWithValue("RequestDate", item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
			sqlCommand.Parameters.AddWithValue("Requester_email", item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
			sqlCommand.Parameters.AddWithValue("Requester_id", item.Requester_id);
			sqlCommand.Parameters.AddWithValue("Requester_name", item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
			sqlCommand.Parameters.AddWithValue("Status", item.Status);
			sqlCommand.Parameters.AddWithValue("SubmissionDate", item.SubmissionDate);
			sqlCommand.Parameters.AddWithValue("Validator_id", item.Validator_id);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__BSD_BomChangesRequests] ([AcceptanceDate],[Artikel_Nr],[Artikelnummer],[Comments],[Deleted_date],[Deleted_user_id],[Deleted_username],[Is_deleted],[Reason],[RejectionDate],[RejectionReason],[RequestDate],[Requester_email],[Requester_id],[Requester_name],[Status],[SubmissionDate],[Validator_id]) VALUES ( "

						+ "@AcceptanceDate" + i + ","
						+ "@Artikel_Nr" + i + ","
						+ "@Artikelnummer" + i + ","
						+ "@Comments" + i + ","
						+ "@Deleted_date" + i + ","
						+ "@Deleted_user_id" + i + ","
						+ "@Deleted_username" + i + ","
						+ "@Is_deleted" + i + ","
						+ "@Reason" + i + ","
						+ "@RejectionDate" + i + ","
						+ "@RejectionReason" + i + ","
						+ "@RequestDate" + i + ","
						+ "@Requester_email" + i + ","
						+ "@Requester_id" + i + ","
						+ "@Requester_name" + i + ","
						+ "@Status" + i + ","
						+ "@SubmissionDate" + i + ","
						+ "@Validator_id" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AcceptanceDate" + i, item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Comments" + i, item.Comments == null ? (object)DBNull.Value : item.Comments);
					sqlCommand.Parameters.AddWithValue("Deleted_date" + i, item.Deleted_date);
					sqlCommand.Parameters.AddWithValue("Deleted_user_id" + i, item.Deleted_user_id);
					sqlCommand.Parameters.AddWithValue("Deleted_username" + i, item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
					sqlCommand.Parameters.AddWithValue("Is_deleted" + i, item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
					sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
					sqlCommand.Parameters.AddWithValue("RejectionDate" + i, item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
					sqlCommand.Parameters.AddWithValue("RejectionReason" + i, item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
					sqlCommand.Parameters.AddWithValue("RequestDate" + i, item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
					sqlCommand.Parameters.AddWithValue("Requester_email" + i, item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
					sqlCommand.Parameters.AddWithValue("Requester_id" + i, item.Requester_id);
					sqlCommand.Parameters.AddWithValue("Requester_name" + i, item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
					sqlCommand.Parameters.AddWithValue("SubmissionDate" + i, item.SubmissionDate);
					sqlCommand.Parameters.AddWithValue("Validator_id" + i, item.Validator_id);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__BSD_BomChangesRequests] SET [AcceptanceDate]=@AcceptanceDate, [Artikel_Nr]=@Artikel_Nr, [Artikelnummer]=@Artikelnummer, [Comments]=@Comments, [Deleted_date]=@Deleted_date, [Deleted_user_id]=@Deleted_user_id, [Deleted_username]=@Deleted_username, [Is_deleted]=@Is_deleted, [Reason]=@Reason, [RejectionDate]=@RejectionDate, [RejectionReason]=@RejectionReason, [RequestDate]=@RequestDate, [Requester_email]=@Requester_email, [Requester_id]=@Requester_id, [Requester_name]=@Requester_name, [Status]=@Status, [SubmissionDate]=@SubmissionDate, [Validator_id]=@Validator_id WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AcceptanceDate", item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
			sqlCommand.Parameters.AddWithValue("Artikel_Nr", item.Artikel_Nr);
			sqlCommand.Parameters.AddWithValue("Artikelnummer", item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
			sqlCommand.Parameters.AddWithValue("Comments", item.Comments == null ? (object)DBNull.Value : item.Comments);
			sqlCommand.Parameters.AddWithValue("Deleted_date", item.Deleted_date);
			sqlCommand.Parameters.AddWithValue("Deleted_user_id", item.Deleted_user_id);
			sqlCommand.Parameters.AddWithValue("Deleted_username", item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
			sqlCommand.Parameters.AddWithValue("Is_deleted", item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
			sqlCommand.Parameters.AddWithValue("Reason", item.Reason);
			sqlCommand.Parameters.AddWithValue("RejectionDate", item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
			sqlCommand.Parameters.AddWithValue("RejectionReason", item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
			sqlCommand.Parameters.AddWithValue("RequestDate", item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
			sqlCommand.Parameters.AddWithValue("Requester_email", item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
			sqlCommand.Parameters.AddWithValue("Requester_id", item.Requester_id);
			sqlCommand.Parameters.AddWithValue("Requester_name", item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
			sqlCommand.Parameters.AddWithValue("Status", item.Status);
			sqlCommand.Parameters.AddWithValue("SubmissionDate", item.SubmissionDate);
			sqlCommand.Parameters.AddWithValue("Validator_id", item.Validator_id);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 19; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__BSD_BomChangesRequests] SET "

					+ "[AcceptanceDate]=@AcceptanceDate" + i + ","
					+ "[Artikel_Nr]=@Artikel_Nr" + i + ","
					+ "[Artikelnummer]=@Artikelnummer" + i + ","
					+ "[Comments]=@Comments" + i + ","
					+ "[Deleted_date]=@Deleted_date" + i + ","
					+ "[Deleted_user_id]=@Deleted_user_id" + i + ","
					+ "[Deleted_username]=@Deleted_username" + i + ","
					+ "[Is_deleted]=@Is_deleted" + i + ","
					+ "[Reason]=@Reason" + i + ","
					+ "[RejectionDate]=@RejectionDate" + i + ","
					+ "[RejectionReason]=@RejectionReason" + i + ","
					+ "[RequestDate]=@RequestDate" + i + ","
					+ "[Requester_email]=@Requester_email" + i + ","
					+ "[Requester_id]=@Requester_id" + i + ","
					+ "[Requester_name]=@Requester_name" + i + ","
					+ "[Status]=@Status" + i + ","
					+ "[SubmissionDate]=@SubmissionDate" + i + ","
					+ "[Validator_id]=@Validator_id" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AcceptanceDate" + i, item.AcceptanceDate == null ? (object)DBNull.Value : item.AcceptanceDate);
					sqlCommand.Parameters.AddWithValue("Artikel_Nr" + i, item.Artikel_Nr);
					sqlCommand.Parameters.AddWithValue("Artikelnummer" + i, item.Artikelnummer == null ? (object)DBNull.Value : item.Artikelnummer);
					sqlCommand.Parameters.AddWithValue("Comments" + i, item.Comments == null ? (object)DBNull.Value : item.Comments);
					sqlCommand.Parameters.AddWithValue("Deleted_date" + i, item.Deleted_date);
					sqlCommand.Parameters.AddWithValue("Deleted_user_id" + i, item.Deleted_user_id);
					sqlCommand.Parameters.AddWithValue("Deleted_username" + i, item.Deleted_username == null ? (object)DBNull.Value : item.Deleted_username);
					sqlCommand.Parameters.AddWithValue("Is_deleted" + i, item.Is_deleted == null ? (object)DBNull.Value : item.Is_deleted);
					sqlCommand.Parameters.AddWithValue("Reason" + i, item.Reason);
					sqlCommand.Parameters.AddWithValue("RejectionDate" + i, item.RejectionDate == null ? (object)DBNull.Value : item.RejectionDate);
					sqlCommand.Parameters.AddWithValue("RejectionReason" + i, item.RejectionReason == null ? (object)DBNull.Value : item.RejectionReason);
					sqlCommand.Parameters.AddWithValue("RequestDate" + i, item.RequestDate == null ? (object)DBNull.Value : item.RequestDate);
					sqlCommand.Parameters.AddWithValue("Requester_email" + i, item.Requester_email == null ? (object)DBNull.Value : item.Requester_email);
					sqlCommand.Parameters.AddWithValue("Requester_id" + i, item.Requester_id);
					sqlCommand.Parameters.AddWithValue("Requester_name" + i, item.Requester_name == null ? (object)DBNull.Value : item.Requester_name);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status);
					sqlCommand.Parameters.AddWithValue("SubmissionDate" + i, item.SubmissionDate);
					sqlCommand.Parameters.AddWithValue("Validator_id" + i, item.Validator_id);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__BSD_BomChangesRequests] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__BSD_BomChangesRequests] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods



		public static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetAllByWithTransaction(List<int> articleNrs, SqlConnection connection, SqlTransaction transaction)
		{
			if(articleNrs != null && articleNrs.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> results = null;
				if(articleNrs.Count <= maxQueryNumber)
				{
					results = GetByArtikelNrWithTransaction(articleNrs, connection, transaction);
				}
				else
				{
					int batchNumber = articleNrs.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(GetByArtikelNrWithTransaction(articleNrs.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(GetByArtikelNrWithTransaction(articleNrs.GetRange(batchNumber * maxQueryNumber, articleNrs.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}


		private static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetByArtikelNrWithTransaction(List<int> articleNrs, SqlConnection connection, SqlTransaction transaction)
		{
			if(articleNrs != null && articleNrs.Count > 0)
			{
				var dataTable = new DataTable();

				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Empty;
				for(int i = 0; i < articleNrs.Count; i++)
				{
					queryIds += "@Id" + i + ",";
					sqlCommand.Parameters.AddWithValue("Id" + i, articleNrs[i]);
				}
				queryIds = queryIds.TrimEnd(',');

				sqlCommand.CommandText = $"SELECT * FROM [__BSD_BomChangesRequests] WHERE [Artikel_Nr] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
		}

		public static List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> Get(string ArticleNummer, SortingModel sorting)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				List<string> where = new List<string>();
				string query = "SELECT *  FROM [__BSD_BomChangesRequests] ";

				if(!string.IsNullOrEmpty(ArticleNummer))
				{
					where.Add($"[Artikelnummer] LIKE '{ArticleNummer}%'");
				}

				if(where.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", where)}";
				}
				#region >>>>> pagination <<<<<<<
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")}";
				}
				else
				{
					query += " ORDER BY [RequestDate] DESC ";
				}
				#endregion pagination
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
			}
		}


		public static List<Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetValidatedBCR(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT *  FROM [__BSD_BomChangesRequests] Where [Status]='Accepted'";

				if(articleId != 0)
				{
					query += $"AND [Artikel_Nr]={articleId}";
				}

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
			}
		}


		public static List<Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> GetOpenWArtikel(int articleId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT *  FROM [__BSD_BomChangesRequests] Where [Status]='Open' AND [Artikel_Nr]={articleId}";


				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>();
			}
		}

		#endregion Custom Methods

	}
}
