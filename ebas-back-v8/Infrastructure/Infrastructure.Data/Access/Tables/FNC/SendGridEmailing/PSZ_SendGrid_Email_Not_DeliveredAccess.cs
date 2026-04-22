namespace Infrastructure.Data.Access.Tables.FNC.SendGridEmailing
{
	public class PSZ_SendGrid_Email_Not_DeliveredAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PSZ_SendGrid_Email_Not_Delivered] ([AddedOn],[EBASUI],[EmailContent],[EmailFrom],[EmailStatus],[EmailTo],[Exception],[Failed],[Iscc],[MessageId],[Subject],[UserId],[UserNotifiedByEmail],[ViewedByTheUser]) OUTPUT INSERTED.[Id] VALUES (@AddedOn,@EBASUI,@EmailContent,@EmailFrom,@EmailStatus,@EmailTo,@Exception,@Failed,@Iscc,@MessageId,@Subject,@UserId,@UserNotifiedByEmail,@ViewedByTheUser); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("EBASUI", item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
					sqlCommand.Parameters.AddWithValue("EmailContent", item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
					sqlCommand.Parameters.AddWithValue("EmailFrom", item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
					sqlCommand.Parameters.AddWithValue("EmailStatus", item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
					sqlCommand.Parameters.AddWithValue("EmailTo", item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
					sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("Failed", item.Failed == null ? (object)DBNull.Value : item.Failed);
					sqlCommand.Parameters.AddWithValue("Iscc", item.Iscc == null ? (object)DBNull.Value : item.Iscc);
					sqlCommand.Parameters.AddWithValue("MessageId", item.MessageId == null ? (object)DBNull.Value : item.MessageId);
					sqlCommand.Parameters.AddWithValue("Subject", item.Subject == null ? (object)DBNull.Value : item.Subject);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail", item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
					sqlCommand.Parameters.AddWithValue("ViewedByTheUser", item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items)
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
						query += " INSERT INTO [PSZ_SendGrid_Email_Not_Delivered] ([AddedOn],[EBASUI],[EmailContent],[EmailFrom],[EmailStatus],[EmailTo],[Exception],[Failed],[Iscc],[MessageId],[Subject],[UserId],[UserNotifiedByEmail],[ViewedByTheUser]) VALUES ( "

							+ "@AddedOn" + i + ","
							+ "@EBASUI" + i + ","
							+ "@EmailContent" + i + ","
							+ "@EmailFrom" + i + ","
							+ "@EmailStatus" + i + ","
							+ "@EmailTo" + i + ","
							+ "@Exception" + i + ","
							+ "@Failed" + i + ","
							+ "@Iscc" + i + ","
							+ "@MessageId" + i + ","
							+ "@Subject" + i + ","
							+ "@UserId" + i + ","
							+ "@UserNotifiedByEmail" + i + ","
							+ "@ViewedByTheUser" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
						sqlCommand.Parameters.AddWithValue("EBASUI" + i, item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
						sqlCommand.Parameters.AddWithValue("EmailContent" + i, item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
						sqlCommand.Parameters.AddWithValue("EmailFrom" + i, item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
						sqlCommand.Parameters.AddWithValue("EmailStatus" + i, item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
						sqlCommand.Parameters.AddWithValue("EmailTo" + i, item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
						sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
						sqlCommand.Parameters.AddWithValue("Failed" + i, item.Failed == null ? (object)DBNull.Value : item.Failed);
						sqlCommand.Parameters.AddWithValue("Iscc" + i, item.Iscc == null ? (object)DBNull.Value : item.Iscc);
						sqlCommand.Parameters.AddWithValue("MessageId" + i, item.MessageId == null ? (object)DBNull.Value : item.MessageId);
						sqlCommand.Parameters.AddWithValue("Subject" + i, item.Subject == null ? (object)DBNull.Value : item.Subject);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail" + i, item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
						sqlCommand.Parameters.AddWithValue("ViewedByTheUser" + i, item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET [AddedOn]=@AddedOn, [EBASUI]=@EBASUI, [EmailContent]=@EmailContent, [EmailFrom]=@EmailFrom, [EmailStatus]=@EmailStatus, [EmailTo]=@EmailTo, [Exception]=@Exception, [Failed]=@Failed, [Iscc]=@Iscc, [MessageId]=@MessageId, [Subject]=@Subject, [UserId]=@UserId, [UserNotifiedByEmail]=@UserNotifiedByEmail, [ViewedByTheUser]=@ViewedByTheUser WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
				sqlCommand.Parameters.AddWithValue("EBASUI", item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
				sqlCommand.Parameters.AddWithValue("EmailContent", item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
				sqlCommand.Parameters.AddWithValue("EmailFrom", item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
				sqlCommand.Parameters.AddWithValue("EmailStatus", item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
				sqlCommand.Parameters.AddWithValue("EmailTo", item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
				sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
				sqlCommand.Parameters.AddWithValue("Failed", item.Failed == null ? (object)DBNull.Value : item.Failed);
				sqlCommand.Parameters.AddWithValue("Iscc", item.Iscc == null ? (object)DBNull.Value : item.Iscc);
				sqlCommand.Parameters.AddWithValue("MessageId", item.MessageId == null ? (object)DBNull.Value : item.MessageId);
				sqlCommand.Parameters.AddWithValue("Subject", item.Subject == null ? (object)DBNull.Value : item.Subject);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail", item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
				sqlCommand.Parameters.AddWithValue("ViewedByTheUser", item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items)
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
						query += " UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET "

							+ "[AddedOn]=@AddedOn" + i + ","
							+ "[EBASUI]=@EBASUI" + i + ","
							+ "[EmailContent]=@EmailContent" + i + ","
							+ "[EmailFrom]=@EmailFrom" + i + ","
							+ "[EmailStatus]=@EmailStatus" + i + ","
							+ "[EmailTo]=@EmailTo" + i + ","
							+ "[Exception]=@Exception" + i + ","
							+ "[Failed]=@Failed" + i + ","
							+ "[Iscc]=@Iscc" + i + ","
							+ "[MessageId]=@MessageId" + i + ","
							+ "[Subject]=@Subject" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserNotifiedByEmail]=@UserNotifiedByEmail" + i + ","
							+ "[ViewedByTheUser]=@ViewedByTheUser" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
						sqlCommand.Parameters.AddWithValue("EBASUI" + i, item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
						sqlCommand.Parameters.AddWithValue("EmailContent" + i, item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
						sqlCommand.Parameters.AddWithValue("EmailFrom" + i, item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
						sqlCommand.Parameters.AddWithValue("EmailStatus" + i, item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
						sqlCommand.Parameters.AddWithValue("EmailTo" + i, item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
						sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
						sqlCommand.Parameters.AddWithValue("Failed" + i, item.Failed == null ? (object)DBNull.Value : item.Failed);
						sqlCommand.Parameters.AddWithValue("Iscc" + i, item.Iscc == null ? (object)DBNull.Value : item.Iscc);
						sqlCommand.Parameters.AddWithValue("MessageId" + i, item.MessageId == null ? (object)DBNull.Value : item.MessageId);
						sqlCommand.Parameters.AddWithValue("Subject" + i, item.Subject == null ? (object)DBNull.Value : item.Subject);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail" + i, item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
						sqlCommand.Parameters.AddWithValue("ViewedByTheUser" + i, item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);
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
				string query = "DELETE FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			//int response = int.MinValue;


			string query = "INSERT INTO [PSZ_SendGrid_Email_Not_Delivered] ([AddedOn],[EBASUI],[EmailContent],[EmailFrom],[EmailStatus],[EmailTo],[Exception],[Failed],[Iscc],[MessageId],[Subject],[UserId],[UserNotifiedByEmail],[ViewedByTheUser]) OUTPUT INSERTED.[Id] VALUES (@AddedOn,@EBASUI,@EmailContent,@EmailFrom,@EmailStatus,@EmailTo,@Exception,@Failed,@Iscc,@MessageId,@Subject,@UserId,@UserNotifiedByEmail,@ViewedByTheUser); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
			sqlCommand.Parameters.AddWithValue("EBASUI", item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
			sqlCommand.Parameters.AddWithValue("EmailContent", item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
			sqlCommand.Parameters.AddWithValue("EmailFrom", item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
			sqlCommand.Parameters.AddWithValue("EmailStatus", item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
			sqlCommand.Parameters.AddWithValue("EmailTo", item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
			sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
			sqlCommand.Parameters.AddWithValue("Failed", item.Failed == null ? (object)DBNull.Value : item.Failed);
			sqlCommand.Parameters.AddWithValue("Iscc", item.Iscc == null ? (object)DBNull.Value : item.Iscc);
			sqlCommand.Parameters.AddWithValue("MessageId", item.MessageId == null ? (object)DBNull.Value : item.MessageId);
			sqlCommand.Parameters.AddWithValue("Subject", item.Subject == null ? (object)DBNull.Value : item.Subject);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail", item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
			sqlCommand.Parameters.AddWithValue("ViewedByTheUser", item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [PSZ_SendGrid_Email_Not_Delivered] ([AddedOn],[EBASUI],[EmailContent],[EmailFrom],[EmailStatus],[EmailTo],[Exception],[Failed],[Iscc],[MessageId],[Subject],[UserId],[UserNotifiedByEmail],[ViewedByTheUser]) VALUES ( "

						+ "@AddedOn" + i + ","
						+ "@EBASUI" + i + ","
						+ "@EmailContent" + i + ","
						+ "@EmailFrom" + i + ","
						+ "@EmailStatus" + i + ","
						+ "@EmailTo" + i + ","
						+ "@Exception" + i + ","
						+ "@Failed" + i + ","
						+ "@Iscc" + i + ","
						+ "@MessageId" + i + ","
						+ "@Subject" + i + ","
						+ "@UserId" + i + ","
						+ "@UserNotifiedByEmail" + i + ","
						+ "@ViewedByTheUser" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("EBASUI" + i, item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
					sqlCommand.Parameters.AddWithValue("EmailContent" + i, item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
					sqlCommand.Parameters.AddWithValue("EmailFrom" + i, item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
					sqlCommand.Parameters.AddWithValue("EmailStatus" + i, item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
					sqlCommand.Parameters.AddWithValue("EmailTo" + i, item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
					sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("Failed" + i, item.Failed == null ? (object)DBNull.Value : item.Failed);
					sqlCommand.Parameters.AddWithValue("Iscc" + i, item.Iscc == null ? (object)DBNull.Value : item.Iscc);
					sqlCommand.Parameters.AddWithValue("MessageId" + i, item.MessageId == null ? (object)DBNull.Value : item.MessageId);
					sqlCommand.Parameters.AddWithValue("Subject" + i, item.Subject == null ? (object)DBNull.Value : item.Subject);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail" + i, item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
					sqlCommand.Parameters.AddWithValue("ViewedByTheUser" + i, item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET [AddedOn]=@AddedOn, [EBASUI]=@EBASUI, [EmailContent]=@EmailContent, [EmailFrom]=@EmailFrom, [EmailStatus]=@EmailStatus, [EmailTo]=@EmailTo, [Exception]=@Exception, [Failed]=@Failed, [Iscc]=@Iscc, [MessageId]=@MessageId, [Subject]=@Subject, [UserId]=@UserId, [UserNotifiedByEmail]=@UserNotifiedByEmail, [ViewedByTheUser]=@ViewedByTheUser WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("AddedOn", item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
			sqlCommand.Parameters.AddWithValue("EBASUI", item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
			sqlCommand.Parameters.AddWithValue("EmailContent", item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
			sqlCommand.Parameters.AddWithValue("EmailFrom", item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
			sqlCommand.Parameters.AddWithValue("EmailStatus", item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
			sqlCommand.Parameters.AddWithValue("EmailTo", item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
			sqlCommand.Parameters.AddWithValue("Exception", item.Exception == null ? (object)DBNull.Value : item.Exception);
			sqlCommand.Parameters.AddWithValue("Failed", item.Failed == null ? (object)DBNull.Value : item.Failed);
			sqlCommand.Parameters.AddWithValue("Iscc", item.Iscc == null ? (object)DBNull.Value : item.Iscc);
			sqlCommand.Parameters.AddWithValue("MessageId", item.MessageId == null ? (object)DBNull.Value : item.MessageId);
			sqlCommand.Parameters.AddWithValue("Subject", item.Subject == null ? (object)DBNull.Value : item.Subject);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail", item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
			sqlCommand.Parameters.AddWithValue("ViewedByTheUser", item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 15; // Nb params per query
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET "

					+ "[AddedOn]=@AddedOn" + i + ","
					+ "[EBASUI]=@EBASUI" + i + ","
					+ "[EmailContent]=@EmailContent" + i + ","
					+ "[EmailFrom]=@EmailFrom" + i + ","
					+ "[EmailStatus]=@EmailStatus" + i + ","
					+ "[EmailTo]=@EmailTo" + i + ","
					+ "[Exception]=@Exception" + i + ","
					+ "[Failed]=@Failed" + i + ","
					+ "[Iscc]=@Iscc" + i + ","
					+ "[MessageId]=@MessageId" + i + ","
					+ "[Subject]=@Subject" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[UserNotifiedByEmail]=@UserNotifiedByEmail" + i + ","
					+ "[ViewedByTheUser]=@ViewedByTheUser" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("AddedOn" + i, item.AddedOn == null ? (object)DBNull.Value : item.AddedOn);
					sqlCommand.Parameters.AddWithValue("EBASUI" + i, item.EBASUI == null ? (object)DBNull.Value : item.EBASUI);
					sqlCommand.Parameters.AddWithValue("EmailContent" + i, item.EmailContent == null ? (object)DBNull.Value : item.EmailContent);
					sqlCommand.Parameters.AddWithValue("EmailFrom" + i, item.EmailFrom == null ? (object)DBNull.Value : item.EmailFrom);
					sqlCommand.Parameters.AddWithValue("EmailStatus" + i, item.EmailStatus == null ? (object)DBNull.Value : item.EmailStatus);
					sqlCommand.Parameters.AddWithValue("EmailTo" + i, item.EmailTo == null ? (object)DBNull.Value : item.EmailTo);
					sqlCommand.Parameters.AddWithValue("Exception" + i, item.Exception == null ? (object)DBNull.Value : item.Exception);
					sqlCommand.Parameters.AddWithValue("Failed" + i, item.Failed == null ? (object)DBNull.Value : item.Failed);
					sqlCommand.Parameters.AddWithValue("Iscc" + i, item.Iscc == null ? (object)DBNull.Value : item.Iscc);
					sqlCommand.Parameters.AddWithValue("MessageId" + i, item.MessageId == null ? (object)DBNull.Value : item.MessageId);
					sqlCommand.Parameters.AddWithValue("Subject" + i, item.Subject == null ? (object)DBNull.Value : item.Subject);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserNotifiedByEmail" + i, item.UserNotifiedByEmail == null ? (object)DBNull.Value : item.UserNotifiedByEmail);
					sqlCommand.Parameters.AddWithValue("ViewedByTheUser" + i, item.ViewedByTheUser == null ? (object)DBNull.Value : item.ViewedByTheUser);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [PSZ_SendGrid_Email_Not_Delivered] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2> GetByMessagesIds(List<string> messagesIds)
		{


			string messegesID = "";
			if(messagesIds is null)
				return null;
			if(messagesIds.Count == 0)
				return null;

			for(int i = 0; i < messagesIds.Count(); i++)
			{
				if(i == messagesIds.Count() - 1)
				{
					messegesID += $"'{messagesIds.ElementAt(i)}'";
				}
				else
				{
					messegesID += $"'{messagesIds.ElementAt(i)}'" + ",";
				}

			}

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select Count(*) Over() TotalCount,* from PSZ_SendGrid_Email_Not_Delivered where  
								MessageId IN ({messegesID})";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2>();
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2> GetByMessagesIdsWithPagination(int UserId, string filter = null, Settings.PaginModel paging = null)
		{
			string emailfilter = (filter is not null && filter?.Length > 1) ? $" and EmailTo like '%{filter}%'" : "";

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"select Count(*) Over() TotalCount,* from PSZ_SendGrid_Email_Not_Delivered where  
								MessageId IN (select  MessageId  from [PSZ_SendGrid_Email_Not_Delivered] 
									group by  MessageId ,UserId,EmailStatus,ViewedByTheUser,EmailTo,Failed
									having UserId={UserId} AND (EmailStatus = -1 OR Failed = 1) )  {emailfilter}
									order by AddedOn desc ";
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2>();
			}
		}
		public static int UpdateMessageStatusToNotDelivered(string MessageId, string EmailTo, string EmailFrom)
		{
			int results = -1;


			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				//string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET [EmailStatus]=-1  WHERE [MessageId] IN ({messegesID})";
				string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] 
									SET [EmailStatus]=-1  
									WHERE [MessageId]  = '{MessageId}'
									AND EmailTo ='{EmailTo}' 
									AND EmailFrom = '{EmailFrom}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdEntity> GetUndeliveredEmailsNotViewedByUserMessagesId(int UserId, string filter = "")
		{
			var dataTable = new DataTable();
			string emailfilter = (filter is not null && filter?.Length > 1) ? $" and EmailTo like '%{filter}%'" : "";
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select 5 MessageId  from [PSZ_SendGrid_Email_Not_Delivered] 
									group by  MessageId ,UserId,EmailStatus,ViewedByTheUser,EmailTo,Failed
									having UserId={UserId} AND (EmailStatus = -1 OR Failed = 1) " + emailfilter + "  order by AddedOn desc ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.EmailToEntity> GetUndeliveredEmailsNotViewedByUserEmailTo(int UserId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select  Distinct EmailTo  from [PSZ_SendGrid_Email_Not_Delivered] 
									group by  MessageId ,UserId,EmailStatus,ViewedByTheUser,EmailTo,Failed
									having UserId={UserId} AND (EmailStatus = -1 OR Failed = 1)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.EmailToEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.EmailToEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity> GetMinimalUndeliveredEmailsNotViewedByUser(int UserId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select TOP 10  MessageId ,EmailFrom,Subject,AddedOn,EmailContent,UserId,EmailStatus,ViewedByTheUser,EmailTo,Iscc,Failed  from [PSZ_SendGrid_Email_Not_Delivered] 
									group by  MessageId ,UserId,EmailStatus,ViewedByTheUser,EmailTo,Subject,AddedOn,Iscc,EmailContent,EmailFrom,Failed
									having UserId={UserId} AND  (EmailStatus = -1 OR Failed = 1) And ViewedByTheUser = -1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2> GetUndeliveredEmailsNotViewedByUser(int UserId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select Count(*) Over() TotalCount , * from PSZ_SendGrid_Email_Not_Delivered where [EmailStatus]=-1 AND ViewedByTheUser = -1 AND UserId = {UserId}";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdCountEntity> GetUndeliveredEmailsNotViewedByUserCount(int UserId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select Top 1 count(*) over() TotalCount   
									from [PSZ_SendGrid_Email_Not_Delivered] 
									group by  MessageId ,UserId,EmailStatus,ViewedByTheUser,EmailTo,Subject,AddedOn,Iscc,EmailContent,EmailFrom,Failed
									having UserId={UserId} AND  (EmailStatus = -1 OR Failed = 1) And ViewedByTheUser = -1";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdCountEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.MessagesIdCountEntity>();
			}
		}
		public static int SetMessageStatus(string MessageId, string EmailTo, string EmailFrom)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				//string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET [EmailStatus]=-1  WHERE [MessageId] IN ({messegesID})";
				string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] 
									SET [ViewedByTheUser]= 0  
									WHERE [MessageId]  = '{MessageId}'
									AND EmailTo ='{EmailTo}' 
									AND EmailFrom = '{EmailFrom}'";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2> GetUndeliveredEmailsNotNotifiedUser(string MessageId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select UserId,EmailTo,Subject,AddedOn,EmailContent from [PSZ_SendGrid_Email_Not_Delivered] 
											where UserNotifiedByEmail = -1 
											AND MessageId = '{MessageId}' 
											AND EmailStatus = -1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2> GetInternalFailedEmailNotNotifiedUser(string MessageId)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $@"select UserId,EmailTo,Subject,AddedOn,EmailContent from [PSZ_SendGrid_Email_Not_Delivered] 
											where UserNotifiedByEmail = -1 
											AND MessageId = '{MessageId}' 
											AND Failed = 1 ";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}
			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.SendGridEmailing.PSZ_SendGrid_Email_Not_DeliveredMinimalEntity2>();
			}
		}
		public static int UpdateMessageNotificationStatus(string MessageId, List<string> EmailTo)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				var emailsTo = string.Join(",", EmailTo.Select(email => $"'{email}'"));
				sqlConnection.Open();
				//string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] SET [EmailStatus]=-1  WHERE [MessageId] IN ({messegesID})";
				string query = @$"UPDATE [PSZ_SendGrid_Email_Not_Delivered] 
									SET [UserNotifiedByEmail]= 0  
									WHERE [MessageId]  = '{MessageId}'
									AND EmailTo IN ({emailsTo}) ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		#endregion Custom Methods

	}



}
