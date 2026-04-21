namespace Infrastructure.Data.Access.Tables.CRP
{
	public static class CRP_FA_EmailUsersAccess
	{
		#region Default Methods
		public static Entities.Tables.CRP.CRP_FA_EmailUsersEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CRP_FA_EmailUsers] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CRP_FA_EmailUsers]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
			}
		}
		public static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
		}
		private static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [CRP_FA_EmailUsers] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
				}
			}
			return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
		}

		public static int Insert(Entities.Tables.CRP.CRP_FA_EmailUsersEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [CRP_FA_EmailUsers] ([CreateTime],[CreateUserId],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@CreateTime,@CreateUserId,@UserId,@UserName); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insert(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items)
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
						query += " INSERT INTO [CRP_FA_EmailUsers] ([CreateTime],[CreateUserId],[UserId],[UserName]) VALUES ( "

							+ "@CreateTime" + i + ","
							+ "@CreateUserId" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Entities.Tables.CRP.CRP_FA_EmailUsersEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [CRP_FA_EmailUsers] SET [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
				sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int update(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items)
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
						query += " UPDATE [CRP_FA_EmailUsers] SET "

							+ "[CreateTime]=@CreateTime" + i + ","
							+ "[CreateUserId]=@CreateUserId" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
						sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
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
				string query = "DELETE FROM [CRP_FA_EmailUsers] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [CRP_FA_EmailUsers] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Entities.Tables.CRP.CRP_FA_EmailUsersEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [CRP_FA_EmailUsers] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [CRP_FA_EmailUsers]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
			}
		}
		public static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
		}
		private static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [CRP_FA_EmailUsers] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
				}
			}
			return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
		}

		public static int InsertWithTransaction(Entities.Tables.CRP.CRP_FA_EmailUsersEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [CRP_FA_EmailUsers] ([CreateTime],[CreateUserId],[UserId],[UserName]) OUTPUT INSERTED.[Id] VALUES (@CreateTime,@CreateUserId,@UserId,@UserName); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int insertWithTransaction(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [CRP_FA_EmailUsers] ([CreateTime],[CreateUserId],[UserId],[UserName]) VALUES ( "

						+ "@CreateTime" + i + ","
						+ "@CreateUserId" + i + ","
						+ "@UserId" + i + ","
						+ "@UserName" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Entities.Tables.CRP.CRP_FA_EmailUsersEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [CRP_FA_EmailUsers] SET [CreateTime]=@CreateTime, [CreateUserId]=@CreateUserId, [UserId]=@UserId, [UserName]=@UserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("CreateTime", item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
			sqlCommand.Parameters.AddWithValue("CreateUserId", item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
			sqlCommand.Parameters.AddWithValue("UserId", item.UserId == null ? (object)DBNull.Value : item.UserId);
			sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 5; // Nb params per query
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
		private static int updateWithTransaction(List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [CRP_FA_EmailUsers] SET "

					+ "[CreateTime]=@CreateTime" + i + ","
					+ "[CreateUserId]=@CreateUserId" + i + ","
					+ "[UserId]=@UserId" + i + ","
					+ "[UserName]=@UserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("CreateTime" + i, item.CreateTime == null ? (object)DBNull.Value : item.CreateTime);
					sqlCommand.Parameters.AddWithValue("CreateUserId" + i, item.CreateUserId == null ? (object)DBNull.Value : item.CreateUserId);
					sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId == null ? (object)DBNull.Value : item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [CRP_FA_EmailUsers] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [CRP_FA_EmailUsers] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods
		public static Entities.Tables.CRP.CRP_FA_EmailUsersEntity GetByUser(int userId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [CRP_FA_EmailUsers] WHERE [UserId]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", userId);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static Entities.Tables.CRP.CRP_FA_EmailUsersEntity GetByUserWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [CRP_FA_EmailUsers] WHERE [UserId]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
				{
					sqlConnection.Open();
					var sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;

					string queryIds = string.Empty;
					for(int i = 0; i < usersIds.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, usersIds[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = $"SELECT * FROM [CRP_FA_EmailUsers] WHERE [UserId] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.CRP.CRP_FA_EmailUsersEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
				}
			}
			return new List<Entities.Tables.CRP.CRP_FA_EmailUsersEntity>();
		}
		#endregion Custom Methods

	}
}
