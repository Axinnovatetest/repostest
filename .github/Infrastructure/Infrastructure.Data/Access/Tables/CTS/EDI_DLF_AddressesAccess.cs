using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Data.Access.Tables.CTS
{
	public class EDI_DLF_AddressesAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_Addresses] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_Addresses]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_Addresses] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__EDI_DLF_Addresses] ([Country],[CustomerNumber],[FactoryNumber],[HouseNumber],[Location],[Name1],[Name2],[PostalCode],[StorageLocation],[StorageLocationDescription],[Street]) OUTPUT INSERTED.[Id] VALUES (@Country,@CustomerNumber,@FactoryNumber,@HouseNumber,@Location,@Name1,@Name2,@PostalCode,@StorageLocation,@StorageLocationDescription,@Street); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("FactoryNumber", item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
					sqlCommand.Parameters.AddWithValue("HouseNumber", item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
					sqlCommand.Parameters.AddWithValue("Location", item.Location == null ? (object)DBNull.Value : item.Location);
					sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("StorageLocationDescription", item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
					sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items)
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
						query += " INSERT INTO [__EDI_DLF_Addresses] ([Country],[CustomerNumber],[FactoryNumber],[HouseNumber],[Location],[Name1],[Name2],[PostalCode],[StorageLocation],[StorageLocationDescription],[Street]) VALUES ( "

							+ "@Country" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@FactoryNumber" + i + ","
							+ "@HouseNumber" + i + ","
							+ "@Location" + i + ","
							+ "@Name1" + i + ","
							+ "@Name2" + i + ","
							+ "@PostalCode" + i + ","
							+ "@StorageLocation" + i + ","
							+ "@StorageLocationDescription" + i + ","
							+ "@Street" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("FactoryNumber" + i, item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
						sqlCommand.Parameters.AddWithValue("HouseNumber" + i, item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
						sqlCommand.Parameters.AddWithValue("Location" + i, item.Location == null ? (object)DBNull.Value : item.Location);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("StorageLocationDescription" + i, item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__EDI_DLF_Addresses] SET [Country]=@Country, [CustomerNumber]=@CustomerNumber, [FactoryNumber]=@FactoryNumber, [HouseNumber]=@HouseNumber, [Location]=@Location, [Name1]=@Name1, [Name2]=@Name2, [PostalCode]=@PostalCode, [StorageLocation]=@StorageLocation, [StorageLocationDescription]=@StorageLocationDescription, [Street]=@Street WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("FactoryNumber", item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
				sqlCommand.Parameters.AddWithValue("HouseNumber", item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
				sqlCommand.Parameters.AddWithValue("Location", item.Location == null ? (object)DBNull.Value : item.Location);
				sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
				sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
				sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
				sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
				sqlCommand.Parameters.AddWithValue("StorageLocationDescription", item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
				sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items)
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
						query += " UPDATE [__EDI_DLF_Addresses] SET "

							+ "[Country]=@Country" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[FactoryNumber]=@FactoryNumber" + i + ","
							+ "[HouseNumber]=@HouseNumber" + i + ","
							+ "[Location]=@Location" + i + ","
							+ "[Name1]=@Name1" + i + ","
							+ "[Name2]=@Name2" + i + ","
							+ "[PostalCode]=@PostalCode" + i + ","
							+ "[StorageLocation]=@StorageLocation" + i + ","
							+ "[StorageLocationDescription]=@StorageLocationDescription" + i + ","
							+ "[Street]=@Street" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("FactoryNumber" + i, item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
						sqlCommand.Parameters.AddWithValue("HouseNumber" + i, item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
						sqlCommand.Parameters.AddWithValue("Location" + i, item.Location == null ? (object)DBNull.Value : item.Location);
						sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
						sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
						sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
						sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
						sqlCommand.Parameters.AddWithValue("StorageLocationDescription" + i, item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
						sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
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
				string query = "DELETE FROM [__EDI_DLF_Addresses] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__EDI_DLF_Addresses] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_Addresses] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__EDI_DLF_Addresses]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__EDI_DLF_Addresses] WHERE [Id] IN ({queryIds})";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity>();
		}

		public static int InsertWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__EDI_DLF_Addresses] ([Country],[CustomerNumber],[FactoryNumber],[HouseNumber],[Location],[Name1],[Name2],[PostalCode],[StorageLocation],[StorageLocationDescription],[Street]) OUTPUT INSERTED.[Id] VALUES (@Country,@CustomerNumber,@FactoryNumber,@HouseNumber,@Location,@Name1,@Name2,@PostalCode,@StorageLocation,@StorageLocationDescription,@Street); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("FactoryNumber", item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
			sqlCommand.Parameters.AddWithValue("HouseNumber", item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
			sqlCommand.Parameters.AddWithValue("Location", item.Location == null ? (object)DBNull.Value : item.Location);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
			sqlCommand.Parameters.AddWithValue("StorageLocationDescription", item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
			sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

			var result = DbExecution.ExecuteScalar(sqlCommand);
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int insertWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__EDI_DLF_Addresses] ([Country],[CustomerNumber],[FactoryNumber],[HouseNumber],[Location],[Name1],[Name2],[PostalCode],[StorageLocation],[StorageLocationDescription],[Street]) VALUES ( "

						+ "@Country" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@FactoryNumber" + i + ","
						+ "@HouseNumber" + i + ","
						+ "@Location" + i + ","
						+ "@Name1" + i + ","
						+ "@Name2" + i + ","
						+ "@PostalCode" + i + ","
						+ "@StorageLocation" + i + ","
						+ "@StorageLocationDescription" + i + ","
						+ "@Street" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("FactoryNumber" + i, item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
					sqlCommand.Parameters.AddWithValue("HouseNumber" + i, item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
					sqlCommand.Parameters.AddWithValue("Location" + i, item.Location == null ? (object)DBNull.Value : item.Location);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("StorageLocationDescription" + i, item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
					sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
				}

				sqlCommand.CommandText = query;

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int UpdateWithTransaction(Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__EDI_DLF_Addresses] SET [Country]=@Country, [CustomerNumber]=@CustomerNumber, [FactoryNumber]=@FactoryNumber, [HouseNumber]=@HouseNumber, [Location]=@Location, [Name1]=@Name1, [Name2]=@Name2, [PostalCode]=@PostalCode, [StorageLocation]=@StorageLocation, [StorageLocationDescription]=@StorageLocationDescription, [Street]=@Street WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Country", item.Country == null ? (object)DBNull.Value : item.Country);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("FactoryNumber", item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
			sqlCommand.Parameters.AddWithValue("HouseNumber", item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
			sqlCommand.Parameters.AddWithValue("Location", item.Location == null ? (object)DBNull.Value : item.Location);
			sqlCommand.Parameters.AddWithValue("Name1", item.Name1 == null ? (object)DBNull.Value : item.Name1);
			sqlCommand.Parameters.AddWithValue("Name2", item.Name2 == null ? (object)DBNull.Value : item.Name2);
			sqlCommand.Parameters.AddWithValue("PostalCode", item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
			sqlCommand.Parameters.AddWithValue("StorageLocation", item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
			sqlCommand.Parameters.AddWithValue("StorageLocationDescription", item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
			sqlCommand.Parameters.AddWithValue("Street", item.Street == null ? (object)DBNull.Value : item.Street);

			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int UpdateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		private static int updateWithTransaction(List<Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__EDI_DLF_Addresses] SET "

					+ "[Country]=@Country" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[FactoryNumber]=@FactoryNumber" + i + ","
					+ "[HouseNumber]=@HouseNumber" + i + ","
					+ "[Location]=@Location" + i + ","
					+ "[Name1]=@Name1" + i + ","
					+ "[Name2]=@Name2" + i + ","
					+ "[PostalCode]=@PostalCode" + i + ","
					+ "[StorageLocation]=@StorageLocation" + i + ","
					+ "[StorageLocationDescription]=@StorageLocationDescription" + i + ","
					+ "[Street]=@Street" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("Country" + i, item.Country == null ? (object)DBNull.Value : item.Country);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("FactoryNumber" + i, item.FactoryNumber == null ? (object)DBNull.Value : item.FactoryNumber);
					sqlCommand.Parameters.AddWithValue("HouseNumber" + i, item.HouseNumber == null ? (object)DBNull.Value : item.HouseNumber);
					sqlCommand.Parameters.AddWithValue("Location" + i, item.Location == null ? (object)DBNull.Value : item.Location);
					sqlCommand.Parameters.AddWithValue("Name1" + i, item.Name1 == null ? (object)DBNull.Value : item.Name1);
					sqlCommand.Parameters.AddWithValue("Name2" + i, item.Name2 == null ? (object)DBNull.Value : item.Name2);
					sqlCommand.Parameters.AddWithValue("PostalCode" + i, item.PostalCode == null ? (object)DBNull.Value : item.PostalCode);
					sqlCommand.Parameters.AddWithValue("StorageLocation" + i, item.StorageLocation == null ? (object)DBNull.Value : item.StorageLocation);
					sqlCommand.Parameters.AddWithValue("StorageLocationDescription" + i, item.StorageLocationDescription == null ? (object)DBNull.Value : item.StorageLocationDescription);
					sqlCommand.Parameters.AddWithValue("Street" + i, item.Street == null ? (object)DBNull.Value : item.Street);
				}

				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__EDI_DLF_Addresses] WHERE [Id]=@Id";
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

				string query = "DELETE FROM [__EDI_DLF_Addresses] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = DbExecution.ExecuteNonQuery(sqlCommand);


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods



		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity GetByCustomerNumber(int CustomerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__EDI_DLF_Addresses] WHERE [CustomerNumber]=@CustomerNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", CustomerNumber);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.CTS.EDI_DLF_AddressesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		
		#endregion Custom Methods

	}
}
