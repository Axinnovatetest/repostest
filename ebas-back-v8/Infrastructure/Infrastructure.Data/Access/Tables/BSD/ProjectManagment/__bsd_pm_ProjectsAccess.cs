using Infrastructure.Data.Entities.Tables.BSD;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public static class __bsd_pm_ProjectsAccess
	{
		#region Default Methods
		public static __bsd_pm_ProjectsEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__bsd_pm_Projects] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new __bsd_pm_ProjectsEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<__bsd_pm_ProjectsEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__bsd_pm_Projects]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new __bsd_pm_ProjectsEntity(x)).ToList();
			}
			else
			{
				return new List<__bsd_pm_ProjectsEntity>();
			}
		}
		public static List<__bsd_pm_ProjectsEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__bsd_pm_Projects] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new __bsd_pm_ProjectsEntity(x)).ToList();
				}
				else
				{
					return new List<__bsd_pm_ProjectsEntity>();
				}
			}
			return new List<__bsd_pm_ProjectsEntity>();
		}
		public static List<__bsd_pm_ProjectsEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<__bsd_pm_ProjectsEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<__bsd_pm_ProjectsEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<__bsd_pm_ProjectsEntity>();
		}
		public static int Insert(__bsd_pm_ProjectsEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__bsd_pm_Projects] ([ProjectName],[CustomerNumber],[CustomerName],[Status],[StatusId],[PMManagerUserId],[PMManagerUsername],[PMManagerFactoryUserId],[PMManagerFactoryUsername],[CSManagerUserId],[CSManagerUsername],[OfferNumber],[QuantityKS],[Factory],[Type],[TypeId],[CreationUserId],[CreationTime],[DeliveryDate],[CustomerRefrence]) OUTPUT INSERTED.[Id] VALUES (@ProjectName,@CustomerNumber,@CustomerName,@Status,@StatusId,@PMManagerUserId,@PMManagerUsername,@PMManagerFactoryUserId,@PMManagerFactoryUsername,@CSManagerUserId,@CSManagerUsername,@OfferNumber,@QuantityKS,@Factory,@Type,@TypeId,@CreationUserId,@CreationTime,@DeliveryDate,@CustomerRefrence); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("PMManagerUserId", item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerUsername", item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId", item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername", item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
					sqlCommand.Parameters.AddWithValue("CSManagerUserId", item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
					sqlCommand.Parameters.AddWithValue("CSManagerUsername", item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
					sqlCommand.Parameters.AddWithValue("OfferNumber", item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
					sqlCommand.Parameters.AddWithValue("QuantityKS", item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
					sqlCommand.Parameters.AddWithValue("Factory", item.Factory == null ? (object)DBNull.Value : item.Factory);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
					sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("CustomerRefrence", item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<__bsd_pm_ProjectsEntity> items)
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
						query += " INSERT INTO [__bsd_pm_Projects] ([ProjectName],[CustomerNumber],[CustomerName],[Status],[StatusId],[PMManagerUserId],[PMManagerUsername],[PMManagerFactoryUserId],[PMManagerFactoryUsername],[CSManagerUserId],[CSManagerUsername],[OfferNumber],[QuantityKS],[Factory],[Type],[TypeId],[CreationUserId],[CreationTime],[DeliveryDate],[CustomerRefrence]) VALUES ("
							+ "@ProjectName" + i +
							 ","
							+ "@CustomerNumber" + i +
							 ","
							+ "@CustomerName" + i +
							 ","
							+ "@Status" + i +
							 ","
							+ "@StatusId" + i +
							 ","
							+ "@PMManagerUserId" + i +
							 ","
							+ "@PMManagerUsername" + i +
							 ","
							+ "@PMManagerFactoryUserId" + i +
							 ","
							+ "@PMManagerFactoryUsername" + i +
							 ","
							+ "@CSManagerUserId" + i +
							 ","
							+ "@CSManagerUsername" + i +
							 ","
							+ "@OfferNumber" + i +
							 ","
							+ "@QuantityKS" + i +
							 ","
							+ "@Factory" + i +
							 ","
							+ "@Type" + i +
							 ","
							+ "@TypeId" + i +
							 ","
							+ "@CreationUserId" + i +
							 ","
							+ "@CreationTime" + i +
							 ","
							+ "@DeliveryDate" + i +
							 ","
							+ "@CustomerRefrence" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("PMManagerUserId" + i, item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerUsername" + i, item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId" + i, item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername" + i, item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
						sqlCommand.Parameters.AddWithValue("CSManagerUserId" + i, item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
						sqlCommand.Parameters.AddWithValue("CSManagerUsername" + i, item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
						sqlCommand.Parameters.AddWithValue("OfferNumber" + i, item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
						sqlCommand.Parameters.AddWithValue("QuantityKS" + i, item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
						sqlCommand.Parameters.AddWithValue("Factory" + i, item.Factory == null ? (object)DBNull.Value : item.Factory);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("CustomerRefrence" + i, item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<__bsd_pm_ProjectsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21;
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
		public static int Update(__bsd_pm_ProjectsEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [__bsd_pm_Projects] SET [ProjectName] = @ProjectName, [CustomerNumber] = @CustomerNumber, [CustomerName] = @CustomerName, [Status] = @Status, [StatusId] = @StatusId, [PMManagerUserId] = @PMManagerUserId, [PMManagerUsername] = @PMManagerUsername, [PMManagerFactoryUserId] = @PMManagerFactoryUserId, [PMManagerFactoryUsername] = @PMManagerFactoryUsername, [CSManagerUserId] = @CSManagerUserId, [CSManagerUsername] = @CSManagerUsername, [OfferNumber] = @OfferNumber, [QuantityKS] = @QuantityKS, [Factory] = @Factory, [Type] = @Type, [TypeId] = @TypeId, [CreationUserId] = @CreationUserId, [CreationTime] = @CreationTime, [DeliveryDate] = @DeliveryDate, [CustomerRefrence] = @CustomerRefrence WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("PMManagerUserId", item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerUsername", item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId", item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername", item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
					sqlCommand.Parameters.AddWithValue("CSManagerUserId", item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
					sqlCommand.Parameters.AddWithValue("CSManagerUsername", item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
					sqlCommand.Parameters.AddWithValue("OfferNumber", item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
					sqlCommand.Parameters.AddWithValue("QuantityKS", item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
					sqlCommand.Parameters.AddWithValue("Factory", item.Factory == null ? (object)DBNull.Value : item.Factory);
					sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
					sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("CustomerRefrence", item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<__bsd_pm_ProjectsEntity> items)
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
						query += " UPDATE [__bsd_pm_Projects] SET "
						  + "[ProjectName]=@ProjectName" + i +
						   ","
						  + "[CustomerNumber]=@CustomerNumber" + i +
						   ","
						  + "[CustomerName]=@CustomerName" + i +
						   ","
						  + "[Status]=@Status" + i +
						   ","
						  + "[StatusId]=@StatusId" + i +
						   ","
						  + "[PMManagerUserId]=@PMManagerUserId" + i +
						   ","
						  + "[PMManagerUsername]=@PMManagerUsername" + i +
						   ","
						  + "[PMManagerFactoryUserId]=@PMManagerFactoryUserId" + i +
						   ","
						  + "[PMManagerFactoryUsername]=@PMManagerFactoryUsername" + i +
						   ","
						  + "[CSManagerUserId]=@CSManagerUserId" + i +
						   ","
						  + "[CSManagerUsername]=@CSManagerUsername" + i +
						   ","
						  + "[OfferNumber]=@OfferNumber" + i +
						   ","
						  + "[QuantityKS]=@QuantityKS" + i +
						   ","
						  + "[Factory]=@Factory" + i +
						   ","
						  + "[Type]=@Type" + i +
						   ","
						  + "[TypeId]=@TypeId" + i +
						   ","
						  + "[CreationUserId]=@CreationUserId" + i +
						   ","
						  + "[CreationTime]=@CreationTime" + i +
						   ","
						  + "[DeliveryDate]=@DeliveryDate" + i +
						   ","
						  + "[CustomerRefrence]=@CustomerRefrence" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("PMManagerUserId" + i, item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerUsername" + i, item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId" + i, item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername" + i, item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
						sqlCommand.Parameters.AddWithValue("CSManagerUserId" + i, item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
						sqlCommand.Parameters.AddWithValue("CSManagerUsername" + i, item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
						sqlCommand.Parameters.AddWithValue("OfferNumber" + i, item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
						sqlCommand.Parameters.AddWithValue("QuantityKS" + i, item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
						sqlCommand.Parameters.AddWithValue("Factory" + i, item.Factory == null ? (object)DBNull.Value : item.Factory);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("CustomerRefrence" + i, item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<__bsd_pm_ProjectsEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21;
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

				string query = "DELETE FROM [__bsd_pm_Projects] WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", id);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
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
				string query = "DELETE FROM [__bsd_pm_Projects] WHERE [Id] IN (" + queryIds + ")";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					for(int i = 0; i < ids.Count; i++)
					{
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
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
		public static __bsd_pm_ProjectsEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__bsd_pm_Projects] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return new __bsd_pm_ProjectsEntity(dataTable.Rows[0]);
			else
				return null;
		}
		public static List<__bsd_pm_ProjectsEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__bsd_pm_Projects]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new __bsd_pm_ProjectsEntity(x)).ToList();
			else
				return new List<__bsd_pm_ProjectsEntity>();
		}
		public static List<__bsd_pm_ProjectsEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = "SELECT * FROM [__bsd_pm_Projects] WHERE [Id] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new __bsd_pm_ProjectsEntity(x)).ToList();
				else
					return new List<__bsd_pm_ProjectsEntity>();
			}
			return new List<__bsd_pm_ProjectsEntity>();
		}
		public static List<__bsd_pm_ProjectsEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<__bsd_pm_ProjectsEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<__bsd_pm_ProjectsEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<__bsd_pm_ProjectsEntity>();
		}
		public static int InsertWithTransaction(__bsd_pm_ProjectsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [__bsd_pm_Projects] ([ProjectName],[CustomerNumber],[CustomerName],[Status],[StatusId],[PMManagerUserId],[PMManagerUsername],[PMManagerFactoryUserId],[PMManagerFactoryUsername],[CSManagerUserId],[CSManagerUsername],[OfferNumber],[QuantityKS],[Factory],[Type],[TypeId],[CreationUserId],[CreationTime],[DeliveryDate],[CustomerRefrence]) OUTPUT INSERTED.[Id] VALUES (@ProjectName,@CustomerNumber,@CustomerName,@Status,@StatusId,@PMManagerUserId,@PMManagerUsername,@PMManagerFactoryUserId,@PMManagerFactoryUsername,@CSManagerUserId,@CSManagerUsername,@OfferNumber,@QuantityKS,@Factory,@Type,@TypeId,@CreationUserId,@CreationTime,@DeliveryDate,@CustomerRefrence); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
				sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
				sqlCommand.Parameters.AddWithValue("PMManagerUserId", item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
				sqlCommand.Parameters.AddWithValue("PMManagerUsername", item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
				sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId", item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
				sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername", item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
				sqlCommand.Parameters.AddWithValue("CSManagerUserId", item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
				sqlCommand.Parameters.AddWithValue("CSManagerUsername", item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
				sqlCommand.Parameters.AddWithValue("OfferNumber", item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
				sqlCommand.Parameters.AddWithValue("QuantityKS", item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
				sqlCommand.Parameters.AddWithValue("Factory", item.Factory == null ? (object)DBNull.Value : item.Factory);
				sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
				sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
				sqlCommand.Parameters.AddWithValue("CustomerRefrence", item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<__bsd_pm_ProjectsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += " INSERT INTO [__bsd_pm_Projects] ([ProjectName],[CustomerNumber],[CustomerName],[Status],[StatusId],[PMManagerUserId],[PMManagerUsername],[PMManagerFactoryUserId],[PMManagerFactoryUsername],[CSManagerUserId],[CSManagerUsername],[OfferNumber],[QuantityKS],[Factory],[Type],[TypeId],[CreationUserId],[CreationTime],[DeliveryDate],[CustomerRefrence]) VALUES ("
							+ "@ProjectName" + i +
							 ","
							+ "@CustomerNumber" + i +
							 ","
							+ "@CustomerName" + i +
							 ","
							+ "@Status" + i +
							 ","
							+ "@StatusId" + i +
							 ","
							+ "@PMManagerUserId" + i +
							 ","
							+ "@PMManagerUsername" + i +
							 ","
							+ "@PMManagerFactoryUserId" + i +
							 ","
							+ "@PMManagerFactoryUsername" + i +
							 ","
							+ "@CSManagerUserId" + i +
							 ","
							+ "@CSManagerUsername" + i +
							 ","
							+ "@OfferNumber" + i +
							 ","
							+ "@QuantityKS" + i +
							 ","
							+ "@Factory" + i +
							 ","
							+ "@Type" + i +
							 ","
							+ "@TypeId" + i +
							 ","
							+ "@CreationUserId" + i +
							 ","
							+ "@CreationTime" + i +
							 ","
							+ "@DeliveryDate" + i +
							 ","
							+ "@CustomerRefrence" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
						sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
						sqlCommand.Parameters.AddWithValue("PMManagerUserId" + i, item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerUsername" + i, item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId" + i, item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
						sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername" + i, item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
						sqlCommand.Parameters.AddWithValue("CSManagerUserId" + i, item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
						sqlCommand.Parameters.AddWithValue("CSManagerUsername" + i, item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
						sqlCommand.Parameters.AddWithValue("OfferNumber" + i, item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
						sqlCommand.Parameters.AddWithValue("QuantityKS" + i, item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
						sqlCommand.Parameters.AddWithValue("Factory" + i, item.Factory == null ? (object)DBNull.Value : item.Factory);
						sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
						sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
						sqlCommand.Parameters.AddWithValue("CustomerRefrence" + i, item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<__bsd_pm_ProjectsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		public static int UpdateWithTransaction(__bsd_pm_ProjectsEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [__bsd_pm_Projects] SET [ProjectName] = @ProjectName, [CustomerNumber] = @CustomerNumber, [CustomerName] = @CustomerName, [Status] = @Status, [StatusId] = @StatusId, [PMManagerUserId] = @PMManagerUserId, [PMManagerUsername] = @PMManagerUsername, [PMManagerFactoryUserId] = @PMManagerFactoryUserId, [PMManagerFactoryUsername] = @PMManagerFactoryUsername, [CSManagerUserId] = @CSManagerUserId, [CSManagerUsername] = @CSManagerUsername, [OfferNumber] = @OfferNumber, [QuantityKS] = @QuantityKS, [Factory] = @Factory, [Type] = @Type, [TypeId] = @TypeId, [CreationUserId] = @CreationUserId, [CreationTime] = @CreationTime, [DeliveryDate] = @DeliveryDate, [CustomerRefrence] = @CustomerRefrence WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ProjectName", item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("Status", item.Status == null ? (object)DBNull.Value : item.Status);
			sqlCommand.Parameters.AddWithValue("StatusId", item.StatusId == null ? (object)DBNull.Value : item.StatusId);
			sqlCommand.Parameters.AddWithValue("PMManagerUserId", item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
			sqlCommand.Parameters.AddWithValue("PMManagerUsername", item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
			sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId", item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
			sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername", item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
			sqlCommand.Parameters.AddWithValue("CSManagerUserId", item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
			sqlCommand.Parameters.AddWithValue("CSManagerUsername", item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
			sqlCommand.Parameters.AddWithValue("OfferNumber", item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
			sqlCommand.Parameters.AddWithValue("QuantityKS", item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
			sqlCommand.Parameters.AddWithValue("Factory", item.Factory == null ? (object)DBNull.Value : item.Factory);
			sqlCommand.Parameters.AddWithValue("Type", item.Type == null ? (object)DBNull.Value : item.Type);
			sqlCommand.Parameters.AddWithValue("TypeId", item.TypeId == null ? (object)DBNull.Value : item.TypeId);
			sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
			sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
			sqlCommand.Parameters.AddWithValue("DeliveryDate", item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
			sqlCommand.Parameters.AddWithValue("CustomerRefrence", item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int updateWithTransaction(List<__bsd_pm_ProjectsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__bsd_pm_Projects] SET "
					  + "[ProjectName]=@ProjectName" + i +
					   ","
					  + "[CustomerNumber]=@CustomerNumber" + i +
					   ","
					  + "[CustomerName]=@CustomerName" + i +
					   ","
					  + "[Status]=@Status" + i +
					   ","
					  + "[StatusId]=@StatusId" + i +
					   ","
					  + "[PMManagerUserId]=@PMManagerUserId" + i +
					   ","
					  + "[PMManagerUsername]=@PMManagerUsername" + i +
					   ","
					  + "[PMManagerFactoryUserId]=@PMManagerFactoryUserId" + i +
					   ","
					  + "[PMManagerFactoryUsername]=@PMManagerFactoryUsername" + i +
					   ","
					  + "[CSManagerUserId]=@CSManagerUserId" + i +
					   ","
					  + "[CSManagerUsername]=@CSManagerUsername" + i +
					   ","
					  + "[OfferNumber]=@OfferNumber" + i +
					   ","
					  + "[QuantityKS]=@QuantityKS" + i +
					   ","
					  + "[Factory]=@Factory" + i +
					   ","
					  + "[Type]=@Type" + i +
					   ","
					  + "[TypeId]=@TypeId" + i +
					   ","
					  + "[CreationUserId]=@CreationUserId" + i +
					   ","
					  + "[CreationTime]=@CreationTime" + i +
					   ","
					  + "[DeliveryDate]=@DeliveryDate" + i +
					   ","
					  + "[CustomerRefrence]=@CustomerRefrence" + i +
					 " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("ProjectName" + i, item.ProjectName == null ? (object)DBNull.Value : item.ProjectName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("Status" + i, item.Status == null ? (object)DBNull.Value : item.Status);
					sqlCommand.Parameters.AddWithValue("StatusId" + i, item.StatusId == null ? (object)DBNull.Value : item.StatusId);
					sqlCommand.Parameters.AddWithValue("PMManagerUserId" + i, item.PMManagerUserId == null ? (object)DBNull.Value : item.PMManagerUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerUsername" + i, item.PMManagerUsername == null ? (object)DBNull.Value : item.PMManagerUsername);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUserId" + i, item.PMManagerFactoryUserId == null ? (object)DBNull.Value : item.PMManagerFactoryUserId);
					sqlCommand.Parameters.AddWithValue("PMManagerFactoryUsername" + i, item.PMManagerFactoryUsername == null ? (object)DBNull.Value : item.PMManagerFactoryUsername);
					sqlCommand.Parameters.AddWithValue("CSManagerUserId" + i, item.CSManagerUserId == null ? (object)DBNull.Value : item.CSManagerUserId);
					sqlCommand.Parameters.AddWithValue("CSManagerUsername" + i, item.CSManagerUsername == null ? (object)DBNull.Value : item.CSManagerUsername);
					sqlCommand.Parameters.AddWithValue("OfferNumber" + i, item.OfferNumber == null ? (object)DBNull.Value : item.OfferNumber);
					sqlCommand.Parameters.AddWithValue("QuantityKS" + i, item.QuantityKS == null ? (object)DBNull.Value : item.QuantityKS);
					sqlCommand.Parameters.AddWithValue("Factory" + i, item.Factory == null ? (object)DBNull.Value : item.Factory);
					sqlCommand.Parameters.AddWithValue("Type" + i, item.Type == null ? (object)DBNull.Value : item.Type);
					sqlCommand.Parameters.AddWithValue("TypeId" + i, item.TypeId == null ? (object)DBNull.Value : item.TypeId);
					sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("DeliveryDate" + i, item.DeliveryDate == null ? (object)DBNull.Value : item.DeliveryDate);
					sqlCommand.Parameters.AddWithValue("CustomerRefrence" + i, item.CustomerRefrence == null ? (object)DBNull.Value : item.CustomerRefrence);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<__bsd_pm_ProjectsEntity> items, SqlConnection connection, SqlTransaction transaction)
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
			string query = "DELETE FROM [__bsd_pm_Projects] WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int deleteWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				var sqlCommand = new SqlCommand("", connection, transaction);
				string queryIds = string.Join(",", ids.Select((id, i) => "@Id" + i));
				sqlCommand.CommandText = $"DELETE FROM [__bsd_pm_Projects] WHERE [Id] IN (" + queryIds + ")";
				for(int i = 0; i < ids.Count; i++)
				{
					sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
				}
				results = DbExecution.ExecuteNonQuery(sqlCommand);
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
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity> GetByStatus(string status)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__bsd_pm_Projects] where [Status]=@status";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("status", status);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity> GetByTime(bool late, bool closed)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var clause = closed
					? "where [Status]='Closed'"
					: late ? "where DeliveryDate < GETDATE()" : "where DeliveryDate >= GETDATE()";
				string query = $"SELECT * FROM [__bsd_pm_Projects] {clause}";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity> GetByCustomer(int customerNumber)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__bsd_pm_Projects] WHERE [CustomerNumber]=@customerNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerNumber", customerNumber);
				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity>();
			}
		}
		#endregion Custom Methods
	}
}
