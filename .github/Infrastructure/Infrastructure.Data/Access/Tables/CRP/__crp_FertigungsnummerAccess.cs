

using Infrastructure.Data.Entities.Tables.CRP;

namespace Infrastructure.Data.Access.Tables.CRP
{
	public static class __crp_FertigungsnummerAccess
	{
		#region Default Methods
		public static __crp_FertigungsnummerEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__crp_Fertigungsnummer] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new __crp_FertigungsnummerEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<__crp_FertigungsnummerEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__crp_Fertigungsnummer]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
			}
			else
			{
				return new List<__crp_FertigungsnummerEntity>();
			}
		}
		public static List<__crp_FertigungsnummerEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__crp_Fertigungsnummer] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
				}
				else
				{
					return new List<__crp_FertigungsnummerEntity>();
				}
			}
			return new List<__crp_FertigungsnummerEntity>();
		}
		public static List<__crp_FertigungsnummerEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<__crp_FertigungsnummerEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<__crp_FertigungsnummerEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<__crp_FertigungsnummerEntity>();
		}
		public static int Insert(__crp_FertigungsnummerEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__crp_Fertigungsnummer] ([Fertigungsnummer],[angebotNr],[User]) OUTPUT INSERTED.[Id] VALUES (@Fertigungsnummer,@angebotNr,@User); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("angebotNr", item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
					sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);

					var result = sqlCommand.ExecuteScalar();
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<__crp_FertigungsnummerEntity> items)
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
						query += " INSERT INTO [__crp_Fertigungsnummer] ([Fertigungsnummer],[angebotNr],[User]) VALUES ("
							+ "@Fertigungsnummer" + i +
							 ","
							+ "@angebotNr" + i +
							 ","
							+ "@User" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("angebotNr" + i, item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
						sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
					}
					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<__crp_FertigungsnummerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4;
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
		public static int Update(__crp_FertigungsnummerEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [__crp_Fertigungsnummer] SET [Fertigungsnummer] = @Fertigungsnummer, [angebotNr] = @angebotNr, [User] = @User WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("angebotNr", item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
					sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<__crp_FertigungsnummerEntity> items)
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
						query += " UPDATE [__crp_Fertigungsnummer] SET "
						  + "[Fertigungsnummer]=@Fertigungsnummer" + i +
						   ","
						  + "[angebotNr]=@angebotNr" + i +
						   ","
						  + "[User]=@User" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("angebotNr" + i, item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
						sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<__crp_FertigungsnummerEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 4;
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
		public static int Delete(int id)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [__crp_Fertigungsnummer] WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", id);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int delete(List<int> ids)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string queryIds = string.Join(",", Enumerable.Range(0, ids.Count).Select(i => "@Id" + i));
				string query = "DELETE FROM [__crp_Fertigungsnummer] WHERE [Id] IN (" + queryIds + ")";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
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

				return results;
			}
			else
			{
				return -1;
			}
		}
		#region Transaction Methods
		public static __crp_FertigungsnummerEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__crp_Fertigungsnummer] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
				return new __crp_FertigungsnummerEntity(dataTable.Rows[0]);
			else
				return null;
		}
		public static List<__crp_FertigungsnummerEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__crp_Fertigungsnummer]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
			else
				return new List<__crp_FertigungsnummerEntity>();
		}
		public static List<__crp_FertigungsnummerEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = "SELECT * FROM [__crp_Fertigungsnummer] WHERE [Id] IN (" + queryIds + ")";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
				else
					return new List<__crp_FertigungsnummerEntity>();
			}
			return new List<__crp_FertigungsnummerEntity>();
		}
		public static List<__crp_FertigungsnummerEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<__crp_FertigungsnummerEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<__crp_FertigungsnummerEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<__crp_FertigungsnummerEntity>();
		}
		public static int InsertWithTransaction(__crp_FertigungsnummerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [__crp_Fertigungsnummer] ([Fertigungsnummer],[angebotNr],[User]) OUTPUT INSERTED.[Id] VALUES (@Fertigungsnummer,@angebotNr,@User); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
				sqlCommand.Parameters.AddWithValue("angebotNr", item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
				sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);

				var result = sqlCommand.ExecuteScalar();
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<__crp_FertigungsnummerEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				string query = "";
				using(var sqlCommand = new SqlCommand(query, connection, transaction))
				{
					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__crp_Fertigungsnummer] ([Fertigungsnummer],[angebotNr],[User]) VALUES ("
							+ "@Fertigungsnummer" + i +
							 ","
							+ "@angebotNr" + i +
							 ","
							+ "@User" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
						sqlCommand.Parameters.AddWithValue("angebotNr" + i, item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
						sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
					}
					sqlCommand.CommandText = query;
					results = sqlCommand.ExecuteNonQuery();
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<__crp_FertigungsnummerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					results = 0;
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
		public static int UpdateWithTransaction(__crp_FertigungsnummerEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__crp_Fertigungsnummer] SET [Fertigungsnummer] = @Fertigungsnummer, [angebotNr] = @angebotNr, [User] = @User WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Fertigungsnummer", item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
			sqlCommand.Parameters.AddWithValue("angebotNr", item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
			sqlCommand.Parameters.AddWithValue("User", item.User == null ? (object)DBNull.Value : item.User);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int updateWithTransaction(List<__crp_FertigungsnummerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__crp_Fertigungsnummer] SET "
					  + "[Fertigungsnummer]=@Fertigungsnummer" + i +
					   ","
					  + "[angebotNr]=@angebotNr" + i +
					   ","
					  + "[User]=@User" + i +
					 " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("Fertigungsnummer" + i, item.Fertigungsnummer == null ? (object)DBNull.Value : item.Fertigungsnummer);
					sqlCommand.Parameters.AddWithValue("angebotNr" + i, item.angebotNr == null ? (object)DBNull.Value : item.angebotNr);
					sqlCommand.Parameters.AddWithValue("User" + i, item.User == null ? (object)DBNull.Value : item.User);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				}
				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<__crp_FertigungsnummerEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					results = 0;
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
		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "DELETE FROM [__crp_Fertigungsnummer] WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Join(",", ids.Select((id, i) => "@Id" + i));
				sqlCommand.CommandText = $"DELETE FROM [__crp_Fertigungsnummer] WHERE [Id] IN (" + queryIds + ")";
				for(int i = 0; i < ids.Count; i++)
				{
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				results = sqlCommand.ExecuteNonQuery();
				return results;
			}
			return -1;
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
					results = 0;
					for(int i = 0; i < batchNumber; i++)
					{
						results += deleteWithTransaction(ids.GetRange(i * maxParamsNumber, maxParamsNumber), connection, transaction);
					}
					results += deleteWithTransaction(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), connection, transaction);
				}
				return results;
			}
			return -1;
		}
		#endregion Transaction Methods
		#endregion Default Methods

		#region Custom Methods
		public static List<__crp_FertigungsnummerEntity> GetByFertigungsnummer(int fertigungsnummer)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__crp_Fertigungsnummer] WHERE [Fertigungsnummer]=@fertigungsnummer";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@fertigungsnummer", fertigungsnummer);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
			}
			else
			{
				return new List<__crp_FertigungsnummerEntity>();
			}
		}
		public static List<__crp_FertigungsnummerEntity> GetByAngebotNr(int angebotNr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__crp_Fertigungsnummer] WHERE [angebotNr]=@angebotNr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@angebotNr", angebotNr);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new __crp_FertigungsnummerEntity(x)).ToList();
			}
			else
			{
				return new List<__crp_FertigungsnummerEntity>();
			}
		}
		public static int DeleteByFertigungsnummer(int fertigungsnummer)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [__crp_Fertigungsnummer] WHERE [Fertigungsnummer] = @fertigungsnummer";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("fertigungsnummer", fertigungsnummer);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int DeleteByAngebotNr(int angebotNr)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "DELETE FROM [__crp_Fertigungsnummer] WHERE [angebotNr] = @angebotNr";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("angebotNr", angebotNr);

					int rowsAffected = sqlCommand.ExecuteNonQuery();
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}

		#endregion Custom Methods
	}
}