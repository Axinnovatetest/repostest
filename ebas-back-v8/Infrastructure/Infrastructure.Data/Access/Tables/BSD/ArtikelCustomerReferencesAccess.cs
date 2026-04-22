
using System.Transactions;

namespace Infrastructure.Data.Access.Tables.BSD;

public class ArtikelCustomerReferencesAccess
{
	#region Default Methods
	public static Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity Get(int id)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "SELECT * FROM [ArtikelCustomerReferences] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

		}

		if(dataTable.Rows.Count > 0)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(dataTable.Rows[0]);
		}
		else
		{
			return null;
		}
	}

	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> Get()
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "SELECT * FROM [ArtikelCustomerReferences]";
			var sqlCommand = new SqlCommand(query, sqlConnection);

			DbExecution.Fill(sqlCommand, dataTable);
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
		}
	}
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> Get(List<int> ids)
	{
		if(ids != null && ids.Count > 0)
		{
			int maxQueryNumber = Settings.MAX_BATCH_SIZE;
			List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> results = null;
			if(ids.Count <= maxQueryNumber)
			{
				results = get(ids);
			}
			else
			{
				int batchNumber = ids.Count / maxQueryNumber;
				results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
				for(int i = 0; i < batchNumber; i++)
				{
					results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
				}
				results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
			}
			return results;
		}
		return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
	}
	private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> get(List<int> ids)
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

				sqlCommand.CommandText = $"SELECT * FROM [ArtikelCustomerReferences] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
			}
		}
		return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
	}

	public static int Insert(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity item)
	{
		int response = int.MinValue;
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			var sqlTransaction = sqlConnection.BeginTransaction();

			string query = "INSERT INTO [ArtikelCustomerReferences] ([ArticleId],[CreateDate],[CreateUser],[CreateUserName],[CustomerId],[CustomerName],[CustomerNumber],[CustomerReference],[EditDate],[EditUser],[EditUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@CreateDate,@CreateUser,@CreateUserName,@CustomerId,@CustomerName,@CustomerNumber,@CustomerReference,@EditDate,@EditUser,@EditUserName); ";

			using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
			{

				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
				sqlCommand.Parameters.AddWithValue("CreateUser", item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
				sqlCommand.Parameters.AddWithValue("CreateUserName", item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
				sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerReference", item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
				sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
				sqlCommand.Parameters.AddWithValue("EditUser", item.EditUser == null ? (object)DBNull.Value : item.EditUser);
				sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}
			sqlTransaction.Commit();

			return response;
		}
	}
	public static int Insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items)
	{
		if(items != null && items.Count > 0)
		{
			int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
	private static int insert(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items)
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
					query += " INSERT INTO [ArtikelCustomerReferences] ([ArticleId],[CreateDate],[CreateUser],[CreateUserName],[CustomerId],[CustomerName],[CustomerNumber],[CustomerReference],[EditDate],[EditUser],[EditUserName]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@CreateDate" + i + ","
						+ "@CreateUser" + i + ","
						+ "@CreateUserName" + i + ","
						+ "@CustomerId" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@CustomerReference" + i + ","
						+ "@EditDate" + i + ","
						+ "@EditUser" + i + ","
						+ "@EditUserName" + i
						+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
					sqlCommand.Parameters.AddWithValue("CreateUser" + i, item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
					sqlCommand.Parameters.AddWithValue("CreateUserName" + i, item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerReference" + i, item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
					sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
					sqlCommand.Parameters.AddWithValue("EditUser" + i, item.EditUser == null ? (object)DBNull.Value : item.EditUser);
					sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
				}

				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		return -1;
	}

	public static int Update(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity item)
	{
		int results = -1;
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "UPDATE [ArtikelCustomerReferences] SET [ArticleId]=@ArticleId, [CreateDate]=@CreateDate, [CreateUser]=@CreateUser, [CreateUserName]=@CreateUserName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerReference]=@CustomerReference, [EditDate]=@EditDate, [EditUser]=@EditUser, [EditUserName]=@EditUserName WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
			sqlCommand.Parameters.AddWithValue("CreateUser", item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
			sqlCommand.Parameters.AddWithValue("CreateUserName", item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
			sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerReference", item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
			sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
			sqlCommand.Parameters.AddWithValue("EditUser", item.EditUser == null ? (object)DBNull.Value : item.EditUser);
			sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
		}

		return results;
	}
	public static int Update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items)
	{
		if(items != null && items.Count > 0)
		{
			int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
	private static int update(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items)
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
					query += " UPDATE [ArtikelCustomerReferences] SET "

						+ "[ArticleId]=@ArticleId" + i + ","
						+ "[CreateDate]=@CreateDate" + i + ","
						+ "[CreateUser]=@CreateUser" + i + ","
						+ "[CreateUserName]=@CreateUserName" + i + ","
						+ "[CustomerId]=@CustomerId" + i + ","
						+ "[CustomerName]=@CustomerName" + i + ","
						+ "[CustomerNumber]=@CustomerNumber" + i + ","
						+ "[CustomerReference]=@CustomerReference" + i + ","
						+ "[EditDate]=@EditDate" + i + ","
						+ "[EditUser]=@EditUser" + i + ","
						+ "[EditUserName]=@EditUserName" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
					sqlCommand.Parameters.AddWithValue("CreateUser" + i, item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
					sqlCommand.Parameters.AddWithValue("CreateUserName" + i, item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
					sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerReference" + i, item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
					sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
					sqlCommand.Parameters.AddWithValue("EditUser" + i, item.EditUser == null ? (object)DBNull.Value : item.EditUser);
					sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
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
			string query = "DELETE FROM [ArtikelCustomerReferences] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [ArtikelCustomerReferences] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		return -1;
	}

	#region Methods with transaction
	public static Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
	{
		var dataTable = new DataTable();

		string query = "SELECT * FROM [ArtikelCustomerReferences] WHERE [Id]=@Id";
		var sqlCommand = new SqlCommand(query, connection, transaction);
		sqlCommand.Parameters.AddWithValue("Id", id);
		DbExecution.Fill(sqlCommand, dataTable);

		if(dataTable.Rows.Count > 0)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(dataTable.Rows[0]);
		}
		else
		{
			return null;
		}
	}
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
	{
		var dataTable = new DataTable();

		string query = "SELECT * FROM [ArtikelCustomerReferences]";
		var sqlCommand = new SqlCommand(query, connection, transaction);

		DbExecution.Fill(sqlCommand, dataTable);

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
		}
	}
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
	{
		if(ids != null && ids.Count > 0)
		{
			int maxQueryNumber = Settings.MAX_BATCH_SIZE;
			List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> results = null;
			if(ids.Count <= maxQueryNumber)
			{
				results = getWithTransaction(ids, connection, transaction);
			}
			else
			{
				int batchNumber = ids.Count / maxQueryNumber;
				results = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
				for(int i = 0; i < batchNumber; i++)
				{
					results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
				}
				results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
			}
			return results;
		}
		return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
	}
	private static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

			sqlCommand.CommandText = $"SELECT * FROM [ArtikelCustomerReferences] WHERE [Id] IN ({queryIds})";
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
			}
		}
		return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
	}

	public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity item, SqlConnection connection, SqlTransaction transaction)
	{
		int response = int.MinValue;


		string query = "INSERT INTO [ArtikelCustomerReferences] ([ArticleId],[CreateDate],[CreateUser],[CreateUserName],[CustomerId],[CustomerName],[CustomerNumber],[CustomerReference],[EditDate],[EditUser],[EditUserName]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@CreateDate,@CreateUser,@CreateUserName,@CustomerId,@CustomerName,@CustomerNumber,@CustomerReference,@EditDate,@EditUser,@EditUserName); ";


		var sqlCommand = new SqlCommand(query, connection, transaction);
		sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
		sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
		sqlCommand.Parameters.AddWithValue("CreateUser", item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
		sqlCommand.Parameters.AddWithValue("CreateUserName", item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
		sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
		sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
		sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
		sqlCommand.Parameters.AddWithValue("CustomerReference", item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
		sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
		sqlCommand.Parameters.AddWithValue("EditUser", item.EditUser == null ? (object)DBNull.Value : item.EditUser);
		sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);

		var result = DbExecution.ExecuteScalar(sqlCommand);
		return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
	}
	public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items, SqlConnection connection, SqlTransaction transaction)
	{
		if(items != null && items.Count > 0)
		{
			int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
	private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items, SqlConnection connection, SqlTransaction transaction)
	{
		if(items != null && items.Count > 0)
		{
			string query = "";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			int i = 0;
			foreach(var item in items)
			{
				i++;
				query += " INSERT INTO [ArtikelCustomerReferences] ([ArticleId],[CreateDate],[CreateUser],[CreateUserName],[CustomerId],[CustomerName],[CustomerNumber],[CustomerReference],[EditDate],[EditUser],[EditUserName]) VALUES ( "

					+ "@ArticleId" + i + ","
					+ "@CreateDate" + i + ","
					+ "@CreateUser" + i + ","
					+ "@CreateUserName" + i + ","
					+ "@CustomerId" + i + ","
					+ "@CustomerName" + i + ","
					+ "@CustomerNumber" + i + ","
					+ "@CustomerReference" + i + ","
					+ "@EditDate" + i + ","
					+ "@EditUser" + i + ","
					+ "@EditUserName" + i
						+ "); ";


				sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
				sqlCommand.Parameters.AddWithValue("CreateUser" + i, item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
				sqlCommand.Parameters.AddWithValue("CreateUserName" + i, item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
				sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerReference" + i, item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
				sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
				sqlCommand.Parameters.AddWithValue("EditUser" + i, item.EditUser == null ? (object)DBNull.Value : item.EditUser);
				sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
			}

			sqlCommand.CommandText = query;

			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		return -1;
	}

	public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity item, SqlConnection connection, SqlTransaction transaction)
	{
		int results = -1;

		string query = "UPDATE [ArtikelCustomerReferences] SET [ArticleId]=@ArticleId, [CreateDate]=@CreateDate, [CreateUser]=@CreateUser, [CreateUserName]=@CreateUserName, [CustomerId]=@CustomerId, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerReference]=@CustomerReference, [EditDate]=@EditDate, [EditUser]=@EditUser, [EditUserName]=@EditUserName WHERE [Id]=@Id";
		var sqlCommand = new SqlCommand(query, connection, transaction);

		sqlCommand.Parameters.AddWithValue("Id", item.Id);
		sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
		sqlCommand.Parameters.AddWithValue("CreateDate", item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
		sqlCommand.Parameters.AddWithValue("CreateUser", item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
		sqlCommand.Parameters.AddWithValue("CreateUserName", item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
		sqlCommand.Parameters.AddWithValue("CustomerId", item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
		sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
		sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
		sqlCommand.Parameters.AddWithValue("CustomerReference", item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
		sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
		sqlCommand.Parameters.AddWithValue("EditUser", item.EditUser == null ? (object)DBNull.Value : item.EditUser);
		sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);

		results = DbExecution.ExecuteNonQuery(sqlCommand);
		return results;
	}
	public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items, SqlConnection connection, SqlTransaction transaction)
	{
		if(items != null && items.Count > 0)
		{
			int maxParamsNumber = Settings.MAX_BATCH_SIZE / 12; // Nb params per query
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
	private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
				query += " UPDATE [ArtikelCustomerReferences] SET "

				+ "[ArticleId]=@ArticleId" + i + ","
				+ "[CreateDate]=@CreateDate" + i + ","
				+ "[CreateUser]=@CreateUser" + i + ","
				+ "[CreateUserName]=@CreateUserName" + i + ","
				+ "[CustomerId]=@CustomerId" + i + ","
				+ "[CustomerName]=@CustomerName" + i + ","
				+ "[CustomerNumber]=@CustomerNumber" + i + ","
				+ "[CustomerReference]=@CustomerReference" + i + ","
				+ "[EditDate]=@EditDate" + i + ","
				+ "[EditUser]=@EditUser" + i + ","
				+ "[EditUserName]=@EditUserName" + i + " WHERE [Id]=@Id" + i
					+ "; ";

				sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("CreateDate" + i, item.CreateDate == null ? (object)DBNull.Value : item.CreateDate);
				sqlCommand.Parameters.AddWithValue("CreateUser" + i, item.CreateUser == null ? (object)DBNull.Value : item.CreateUser);
				sqlCommand.Parameters.AddWithValue("CreateUserName" + i, item.CreateUserName == null ? (object)DBNull.Value : item.CreateUserName);
				sqlCommand.Parameters.AddWithValue("CustomerId" + i, item.CustomerId == null ? (object)DBNull.Value : item.CustomerId);
				sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerReference" + i, item.CustomerReference == null ? (object)DBNull.Value : item.CustomerReference);
				sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
				sqlCommand.Parameters.AddWithValue("EditUser" + i, item.EditUser == null ? (object)DBNull.Value : item.EditUser);
				sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
			}

			sqlCommand.CommandText = query;
			return DbExecution.ExecuteNonQuery(sqlCommand);
		}

		return -1;
	}

	public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
	{
		int results = -1;

		string query = "DELETE FROM [ArtikelCustomerReferences] WHERE [Id]=@Id";
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

			string query = "DELETE FROM [ArtikelCustomerReferences] WHERE [Id] IN (" + queryIds + ")";
			sqlCommand.CommandText = query;

			results = DbExecution.ExecuteNonQuery(sqlCommand);


			return results;
		}
		return -1;
	}
	#endregion Methods with transaction
	#endregion Default Methods

	#region Custom Methods
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> GetByArtikelId(int id)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "select * from ArtikelCustomerReferences where ArticleId = @Id";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
		}
	}

	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> GetByCustomerNr(int customerNr, int Id)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "select * from ArtikelCustomerReferences where CustomerNumber = @CustomerNumber AND Id = @Id";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", customerNr);
			sqlCommand.Parameters.AddWithValue("Id", Id);

			DbExecution.Fill(sqlCommand, dataTable);
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
		}
	}
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity> GetByCustomerNrAndArtikelID(int customerNr, int Id)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "select * from ArtikelCustomerReferences where CustomerNumber = @CustomerNumber AND ArticleId = @Id";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", customerNr);
			sqlCommand.Parameters.AddWithValue("Id", Id);

			DbExecution.Fill(sqlCommand, dataTable);
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesEntity>();
		}
	}
	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesLikeEntity> GetLikeReference(string nummer)
	{
		nummer = nummer?.Trim() ?? "";
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
		{
			sqlConnection.Open();

			using(var sqlCommand = new SqlCommand())
			{
				sqlCommand.CommandText = $"select Distinct  top 10 CustomerReference,CustomerNumber,CustomerName FROM  ArtikelCustomerReferences WHERE CustomerReference LIKE '{nummer.SqlEscape()}%' ORDER by CustomerReference ASC";
				sqlCommand.Connection = sqlConnection;
				DbExecution.Fill(sqlCommand, dataTable);
			}
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesLikeEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesLikeEntity>();
		}
	}

	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity> GetLikeReferenceAndCustomerID(int CustomerID, string Reference)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
		{
			sqlConnection.Open();

			using(var sqlCommand = new SqlCommand())
			{
				sqlCommand.CommandText = $"select top 1  CustomerId,CustomerReference FROM  ArtikelCustomerReferences WHERE CustomerId = {CustomerID} AND CustomerReference = '{Reference.SqlEscape()}' ";
				sqlCommand.Connection = sqlConnection;
				DbExecution.Fill(sqlCommand, dataTable);
			}
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity>();
		}
	}

	public static List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity> GetLikeReferencesAndCustomerIDs(List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity> requestModels)
	{
		var dataTable = new DataTable();
		var queryConditions = new List<string>();

		foreach(var model in requestModels)
		{
			// Ensure both CustomerId and CustomerReference are valid before adding to the query
			if(model.CustomerId.HasValue && model.CustomerId != -1 && !string.IsNullOrEmpty(model.CustomerReference))
			{
				queryConditions.Add($" CustomerReference = '{model.CustomerReference.SqlEscape()}'");
			}
		}

		if(queryConditions.Count == 0)
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity>();
		}

		string query = $"SELECT CustomerId, CustomerReference FROM ArtikelCustomerReferences WHERE {string.Join(" OR ", queryConditions)}";

		using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
		{
			sqlConnection.Open();

			using(var sqlCommand = new SqlCommand(query, sqlConnection))
			{
				DbExecution.Fill(sqlCommand, dataTable);
			}
		}

		if(dataTable.Rows.Count > 0)
		{
			return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity(x)).ToList();
		}
		else
		{
			return new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity>();
		}
	}
	public static Infrastructure.Data.Entities.Tables.BSD.GetArtikelCustomerReferencesAndCustomerIDLikeEntity GetArticleNrByReferencesAndCustomerIDs(Infrastructure.Data.Entities.Tables.BSD.ArtikelCustomerReferencesAndCustomerIDLikeEntity requestModels)
	{
		var dataTable = new DataTable();
		using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
		{
			sqlConnection.Open();
			string query = "select * from ArtikelCustomerReferences where CustomerReference = @CustomerReference";
			var sqlCommand = new SqlCommand(query, sqlConnection);
			sqlCommand.Parameters.AddWithValue("CustomerReference", requestModels.CustomerReference);

			DbExecution.Fill(sqlCommand, dataTable);
		}

		if(dataTable.Rows.Count > 0)
		{
			return new Infrastructure.Data.Entities.Tables.BSD.GetArtikelCustomerReferencesAndCustomerIDLikeEntity(dataTable.Rows[0]);
		}
		else
		{
			return new Infrastructure.Data.Entities.Tables.BSD.GetArtikelCustomerReferencesAndCustomerIDLikeEntity();
		}

	}
	#endregion Custom Methods

}
