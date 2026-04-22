using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.TLS
{

	public class EmailAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.TLS.EmailEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Email] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.TLS.EmailEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_Email]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.TLS.EmailEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_Email] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.TLS.EmailEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.TLS.EmailEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_Email] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail])  VALUES (@AttachmentIds,@CCEmails,@EmailMessage,@EmailTitle,@SenderCC,@SenderUserEmail,@SenderUserId,@SenderUserName,@SendingTime,@ToEmail); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
					sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
					sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
					sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
					sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
					sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
					sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
					sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
					sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
					sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_Email] ([AttachmentIds],[CCEmails],[EmailMessage],[EmailTitle],[SenderCC],[SenderUserEmail],[SenderUserId],[SenderUserName],[SendingTime],[ToEmail]) VALUES ( "

							+ "@AttachmentIds" + i + ","
							+ "@CCEmails" + i + ","
							+ "@EmailMessage" + i + ","
							+ "@EmailTitle" + i + ","
							+ "@SenderCC" + i + ","
							+ "@SenderUserEmail" + i + ","
							+ "@SenderUserId" + i + ","
							+ "@SenderUserName" + i + ","
							+ "@SendingTime" + i + ","
							+ "@ToEmail" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
						sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
						sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
						sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
						sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
						sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
						sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
						sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
						sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
						sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.TLS.EmailEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_Email] SET [AttachmentIds]=@AttachmentIds, [CCEmails]=@CCEmails, [EmailMessage]=@EmailMessage, [EmailTitle]=@EmailTitle, [SenderCC]=@SenderCC, [SenderUserEmail]=@SenderUserEmail, [SenderUserId]=@SenderUserId, [SenderUserName]=@SenderUserName, [SendingTime]=@SendingTime, [ToEmail]=@ToEmail WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AttachmentIds", item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
				sqlCommand.Parameters.AddWithValue("CCEmails", item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
				sqlCommand.Parameters.AddWithValue("EmailMessage", item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
				sqlCommand.Parameters.AddWithValue("EmailTitle", item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
				sqlCommand.Parameters.AddWithValue("SenderCC", item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
				sqlCommand.Parameters.AddWithValue("SenderUserEmail", item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
				sqlCommand.Parameters.AddWithValue("SenderUserId", item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
				sqlCommand.Parameters.AddWithValue("SenderUserName", item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
				sqlCommand.Parameters.AddWithValue("SendingTime", item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
				sqlCommand.Parameters.AddWithValue("ToEmail", item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.TLS.EmailEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_Email] SET "

							+ "[AttachmentIds]=@AttachmentIds" + i + ","
							+ "[CCEmails]=@CCEmails" + i + ","
							+ "[EmailMessage]=@EmailMessage" + i + ","
							+ "[EmailTitle]=@EmailTitle" + i + ","
							+ "[SenderCC]=@SenderCC" + i + ","
							+ "[SenderUserEmail]=@SenderUserEmail" + i + ","
							+ "[SenderUserId]=@SenderUserId" + i + ","
							+ "[SenderUserName]=@SenderUserName" + i + ","
							+ "[SendingTime]=@SendingTime" + i + ","
							+ "[ToEmail]=@ToEmail" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AttachmentIds" + i, item.AttachmentIds == null ? (object)DBNull.Value : item.AttachmentIds);
						sqlCommand.Parameters.AddWithValue("CCEmails" + i, item.CCEmails == null ? (object)DBNull.Value : item.CCEmails);
						sqlCommand.Parameters.AddWithValue("EmailMessage" + i, item.EmailMessage == null ? (object)DBNull.Value : item.EmailMessage);
						sqlCommand.Parameters.AddWithValue("EmailTitle" + i, item.EmailTitle == null ? (object)DBNull.Value : item.EmailTitle);
						sqlCommand.Parameters.AddWithValue("SenderCC" + i, item.SenderCC == null ? (object)DBNull.Value : item.SenderCC);
						sqlCommand.Parameters.AddWithValue("SenderUserEmail" + i, item.SenderUserEmail == null ? (object)DBNull.Value : item.SenderUserEmail);
						sqlCommand.Parameters.AddWithValue("SenderUserId" + i, item.SenderUserId == null ? (object)DBNull.Value : item.SenderUserId);
						sqlCommand.Parameters.AddWithValue("SenderUserName" + i, item.SenderUserName == null ? (object)DBNull.Value : item.SenderUserName);
						sqlCommand.Parameters.AddWithValue("SendingTime" + i, item.SendingTime == null ? (object)DBNull.Value : item.SendingTime);
						sqlCommand.Parameters.AddWithValue("ToEmail" + i, item.ToEmail == null ? (object)DBNull.Value : item.ToEmail);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_Email] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_Email] WHERE [Id] IN (" + queryIds + ")";
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
