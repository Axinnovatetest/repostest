namespace Infrastructure.Data.Access.Tables.CTS
{
	using Entities.Tables.CTS;
	public class InsideSalesWerksterminUpdatesAccess
	{

		#region Default Methods
		public static InsideSalesWerksterminUpdatesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesWerksterminUpdatesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<InsideSalesWerksterminUpdatesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesWerksterminUpdates]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesWerksterminUpdatesEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesWerksterminUpdatesEntity>();
			}
		}
		public static List<InsideSalesWerksterminUpdatesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesWerksterminUpdatesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesWerksterminUpdatesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<InsideSalesWerksterminUpdatesEntity>();
		}
		private static List<InsideSalesWerksterminUpdatesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesWerksterminUpdatesEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesWerksterminUpdatesEntity>();
				}
			}
			return new List<InsideSalesWerksterminUpdatesEntity>();
		}

		public static int Insert(InsideSalesWerksterminUpdatesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__CTS_InsideSalesWerksterminUpdates] ([ArticleId],[ArticleNumber],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[EditDate],[EditUserId],[EditUserName],[FertigungId],[FertigungNumber],[InsConfirmation],[InsConfirmationDate],[InsConfirmationUserId],[InsConfirmationUserName],[NewWorkDate],[OldWorkDate],[ReasonCapacity],[ReasonCapacityComments],[ReasonClarification],[ReasonClarificationComments],[ReasonDefective],[ReasonDefectiveComments],[ReasonMaterial],[ReasonMaterialComments],[ReasonQuality],[ReasonQualityComments],[ReasonStatusP],[ReasonStatusPComments]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@EditDate,@EditUserId,@EditUserName,@FertigungId,@FertigungNumber,@InsConfirmation,@InsConfirmationDate,@InsConfirmationUserId,@InsConfirmationUserName,@NewWorkDate,@OldWorkDate,@ReasonCapacity,@ReasonCapacityComments,@ReasonClarification,@ReasonClarificationComments,@ReasonDefective,@ReasonDefectiveComments,@ReasonMaterial,@ReasonMaterialComments,@ReasonQuality,@ReasonQualityComments,@ReasonStatusP,@ReasonStatusPComments); ";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
					sqlCommand.Parameters.AddWithValue("EditUserId", item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
					sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
					sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
					sqlCommand.Parameters.AddWithValue("FertigungNumber", item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
					sqlCommand.Parameters.AddWithValue("InsConfirmation", item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
					sqlCommand.Parameters.AddWithValue("InsConfirmationDate", item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
					sqlCommand.Parameters.AddWithValue("NewWorkDate", item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
					sqlCommand.Parameters.AddWithValue("OldWorkDate", item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
					sqlCommand.Parameters.AddWithValue("ReasonCapacity", item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
					sqlCommand.Parameters.AddWithValue("ReasonCapacityComments", item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
					sqlCommand.Parameters.AddWithValue("ReasonClarification", item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
					sqlCommand.Parameters.AddWithValue("ReasonClarificationComments", item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
					sqlCommand.Parameters.AddWithValue("ReasonDefective", item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
					sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments", item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
					sqlCommand.Parameters.AddWithValue("ReasonMaterial", item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
					sqlCommand.Parameters.AddWithValue("ReasonMaterialComments", item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
					sqlCommand.Parameters.AddWithValue("ReasonQuality", item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
					sqlCommand.Parameters.AddWithValue("ReasonQualityComments", item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
					sqlCommand.Parameters.AddWithValue("ReasonStatusP", item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
					sqlCommand.Parameters.AddWithValue("ReasonStatusPComments", item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<InsideSalesWerksterminUpdatesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int insert(List<InsideSalesWerksterminUpdatesEntity> items)
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
						query += " INSERT INTO [__CTS_InsideSalesWerksterminUpdates] ([ArticleId],[ArticleNumber],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[EditDate],[EditUserId],[EditUserName],[FertigungId],[FertigungNumber],[InsConfirmation],[InsConfirmationDate],[InsConfirmationUserId],[InsConfirmationUserName],[NewWorkDate],[OldWorkDate],[ReasonCapacity],[ReasonCapacityComments],[ReasonClarification],[ReasonClarificationComments],[ReasonDefective],[ReasonDefectiveComments],[ReasonMaterial],[ReasonMaterialComments],[ReasonQuality],[ReasonQualityComments],[ReasonStatusP],[ReasonStatusPComments]) VALUES ( "

							+ "@ArticleId" + i + ","
							+ "@ArticleNumber" + i + ","
							+ "@CustomerName" + i + ","
							+ "@CustomerNumber" + i + ","
							+ "@CustomerOrderNumber" + i + ","
							+ "@EditDate" + i + ","
							+ "@EditUserId" + i + ","
							+ "@EditUserName" + i + ","
							+ "@FertigungId" + i + ","
							+ "@FertigungNumber" + i + ","
							+ "@InsConfirmation" + i + ","
							+ "@InsConfirmationDate" + i + ","
							+ "@InsConfirmationUserId" + i + ","
							+ "@InsConfirmationUserName" + i + ","
							+ "@NewWorkDate" + i + ","
							+ "@OldWorkDate" + i + ","
							+ "@ReasonCapacity" + i + ","
							+ "@ReasonCapacityComments" + i + ","
							+ "@ReasonClarification" + i + ","
							+ "@ReasonClarificationComments" + i + ","
							+ "@ReasonDefective" + i + ","
							+ "@ReasonDefectiveComments" + i + ","
							+ "@ReasonMaterial" + i + ","
							+ "@ReasonMaterialComments" + i + ","
							+ "@ReasonQuality" + i + ","
							+ "@ReasonQualityComments" + i + ","
							+ "@ReasonStatusP" + i + ","
							+ "@ReasonStatusPComments" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
						sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
						sqlCommand.Parameters.AddWithValue("EditUserId" + i, item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
						sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
						sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
						sqlCommand.Parameters.AddWithValue("FertigungNumber" + i, item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
						sqlCommand.Parameters.AddWithValue("InsConfirmation" + i, item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
						sqlCommand.Parameters.AddWithValue("InsConfirmationDate" + i, item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
						sqlCommand.Parameters.AddWithValue("InsConfirmationUserId" + i, item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
						sqlCommand.Parameters.AddWithValue("InsConfirmationUserName" + i, item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
						sqlCommand.Parameters.AddWithValue("NewWorkDate" + i, item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
						sqlCommand.Parameters.AddWithValue("OldWorkDate" + i, item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
						sqlCommand.Parameters.AddWithValue("ReasonCapacity" + i, item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
						sqlCommand.Parameters.AddWithValue("ReasonCapacityComments" + i, item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
						sqlCommand.Parameters.AddWithValue("ReasonClarification" + i, item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
						sqlCommand.Parameters.AddWithValue("ReasonClarificationComments" + i, item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
						sqlCommand.Parameters.AddWithValue("ReasonDefective" + i, item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
						sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments" + i, item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
						sqlCommand.Parameters.AddWithValue("ReasonMaterial" + i, item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
						sqlCommand.Parameters.AddWithValue("ReasonMaterialComments" + i, item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
						sqlCommand.Parameters.AddWithValue("ReasonQuality" + i, item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
						sqlCommand.Parameters.AddWithValue("ReasonQualityComments" + i, item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
						sqlCommand.Parameters.AddWithValue("ReasonStatusP" + i, item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
						sqlCommand.Parameters.AddWithValue("ReasonStatusPComments" + i, item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(InsideSalesWerksterminUpdatesEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesWerksterminUpdates] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [EditDate]=@EditDate, [EditUserId]=@EditUserId, [EditUserName]=@EditUserName, [FertigungId]=@FertigungId, [FertigungNumber]=@FertigungNumber, [InsConfirmation]=@InsConfirmation, [InsConfirmationDate]=@InsConfirmationDate, [InsConfirmationUserId]=@InsConfirmationUserId, [InsConfirmationUserName]=@InsConfirmationUserName, [NewWorkDate]=@NewWorkDate, [OldWorkDate]=@OldWorkDate, [ReasonCapacity]=@ReasonCapacity, [ReasonCapacityComments]=@ReasonCapacityComments, [ReasonClarification]=@ReasonClarification, [ReasonClarificationComments]=@ReasonClarificationComments, [ReasonDefective]=@ReasonDefective, [ReasonDefectiveComments]=@ReasonDefectiveComments, [ReasonMaterial]=@ReasonMaterial, [ReasonMaterialComments]=@ReasonMaterialComments, [ReasonQuality]=@ReasonQuality, [ReasonQualityComments]=@ReasonQualityComments, [ReasonStatusP]=@ReasonStatusP, [ReasonStatusPComments]=@ReasonStatusPComments WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
				sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
				sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
				sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
				sqlCommand.Parameters.AddWithValue("EditUserId", item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
				sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
				sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
				sqlCommand.Parameters.AddWithValue("FertigungNumber", item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
				sqlCommand.Parameters.AddWithValue("InsConfirmation", item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
				sqlCommand.Parameters.AddWithValue("InsConfirmationDate", item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
				sqlCommand.Parameters.AddWithValue("NewWorkDate", item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
				sqlCommand.Parameters.AddWithValue("OldWorkDate", item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
				sqlCommand.Parameters.AddWithValue("ReasonCapacity", item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
				sqlCommand.Parameters.AddWithValue("ReasonCapacityComments", item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
				sqlCommand.Parameters.AddWithValue("ReasonClarification", item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
				sqlCommand.Parameters.AddWithValue("ReasonClarificationComments", item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
				sqlCommand.Parameters.AddWithValue("ReasonDefective", item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
				sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments", item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
				sqlCommand.Parameters.AddWithValue("ReasonMaterial", item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
				sqlCommand.Parameters.AddWithValue("ReasonMaterialComments", item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
				sqlCommand.Parameters.AddWithValue("ReasonQuality", item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
				sqlCommand.Parameters.AddWithValue("ReasonQualityComments", item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
				sqlCommand.Parameters.AddWithValue("ReasonStatusP", item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
				sqlCommand.Parameters.AddWithValue("ReasonStatusPComments", item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<InsideSalesWerksterminUpdatesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int update(List<InsideSalesWerksterminUpdatesEntity> items)
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
						query += " UPDATE [__CTS_InsideSalesWerksterminUpdates] SET "

							+ "[ArticleId]=@ArticleId" + i + ","
							+ "[ArticleNumber]=@ArticleNumber" + i + ","
							+ "[CustomerName]=@CustomerName" + i + ","
							+ "[CustomerNumber]=@CustomerNumber" + i + ","
							+ "[CustomerOrderNumber]=@CustomerOrderNumber" + i + ","
							+ "[EditDate]=@EditDate" + i + ","
							+ "[EditUserId]=@EditUserId" + i + ","
							+ "[EditUserName]=@EditUserName" + i + ","
							+ "[FertigungId]=@FertigungId" + i + ","
							+ "[FertigungNumber]=@FertigungNumber" + i + ","
							+ "[InsConfirmation]=@InsConfirmation" + i + ","
							+ "[InsConfirmationDate]=@InsConfirmationDate" + i + ","
							+ "[InsConfirmationUserId]=@InsConfirmationUserId" + i + ","
							+ "[InsConfirmationUserName]=@InsConfirmationUserName" + i + ","
							+ "[NewWorkDate]=@NewWorkDate" + i + ","
							+ "[OldWorkDate]=@OldWorkDate" + i + ","
							+ "[ReasonCapacity]=@ReasonCapacity" + i + ","
							+ "[ReasonCapacityComments]=@ReasonCapacityComments" + i + ","
							+ "[ReasonClarification]=@ReasonClarification" + i + ","
							+ "[ReasonClarificationComments]=@ReasonClarificationComments" + i + ","
							+ "[ReasonDefective]=@ReasonDefective" + i + ","
							+ "[ReasonDefectiveComments]=@ReasonDefectiveComments" + i + ","
							+ "[ReasonMaterial]=@ReasonMaterial" + i + ","
							+ "[ReasonMaterialComments]=@ReasonMaterialComments" + i + ","
							+ "[ReasonQuality]=@ReasonQuality" + i + ","
							+ "[ReasonQualityComments]=@ReasonQualityComments" + i + ","
							+ "[ReasonStatusP]=@ReasonStatusP" + i + ","
							+ "[ReasonStatusPComments]=@ReasonStatusPComments" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
						sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
						sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
						sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
						sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
						sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
						sqlCommand.Parameters.AddWithValue("EditUserId" + i, item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
						sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
						sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
						sqlCommand.Parameters.AddWithValue("FertigungNumber" + i, item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
						sqlCommand.Parameters.AddWithValue("InsConfirmation" + i, item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
						sqlCommand.Parameters.AddWithValue("InsConfirmationDate" + i, item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
						sqlCommand.Parameters.AddWithValue("InsConfirmationUserId" + i, item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
						sqlCommand.Parameters.AddWithValue("InsConfirmationUserName" + i, item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
						sqlCommand.Parameters.AddWithValue("NewWorkDate" + i, item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
						sqlCommand.Parameters.AddWithValue("OldWorkDate" + i, item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
						sqlCommand.Parameters.AddWithValue("ReasonCapacity" + i, item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
						sqlCommand.Parameters.AddWithValue("ReasonCapacityComments" + i, item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
						sqlCommand.Parameters.AddWithValue("ReasonClarification" + i, item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
						sqlCommand.Parameters.AddWithValue("ReasonClarificationComments" + i, item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
						sqlCommand.Parameters.AddWithValue("ReasonDefective" + i, item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
						sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments" + i, item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
						sqlCommand.Parameters.AddWithValue("ReasonMaterial" + i, item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
						sqlCommand.Parameters.AddWithValue("ReasonMaterialComments" + i, item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
						sqlCommand.Parameters.AddWithValue("ReasonQuality" + i, item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
						sqlCommand.Parameters.AddWithValue("ReasonQualityComments" + i, item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
						sqlCommand.Parameters.AddWithValue("ReasonStatusP" + i, item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
						sqlCommand.Parameters.AddWithValue("ReasonStatusPComments" + i, item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
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
				string query = "DELETE FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = sqlCommand.ExecuteNonQuery();
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

					string query = "DELETE FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}

		#region Methods with transaction
		public static InsideSalesWerksterminUpdatesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);
			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesWerksterminUpdatesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<InsideSalesWerksterminUpdatesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [__CTS_InsideSalesWerksterminUpdates]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesWerksterminUpdatesEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesWerksterminUpdatesEntity>();
			}
		}
		public static List<InsideSalesWerksterminUpdatesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<InsideSalesWerksterminUpdatesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<InsideSalesWerksterminUpdatesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}
					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}
				return results;
			}
			return new List<InsideSalesWerksterminUpdatesEntity>();
		}
		private static List<InsideSalesWerksterminUpdatesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = $"SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id] IN ({queryIds})";
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesWerksterminUpdatesEntity(x)).ToList();
				}
				else
				{
					return new List<InsideSalesWerksterminUpdatesEntity>();
				}
			}
			return new List<InsideSalesWerksterminUpdatesEntity>();
		}

		public static int InsertWithTransaction(InsideSalesWerksterminUpdatesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;


			string query = "INSERT INTO [__CTS_InsideSalesWerksterminUpdates] ([ArticleId],[ArticleNumber],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[EditDate],[EditUserId],[EditUserName],[FertigungId],[FertigungNumber],[InsConfirmation],[InsConfirmationDate],[InsConfirmationUserId],[InsConfirmationUserName],[NewWorkDate],[OldWorkDate],[ReasonCapacity],[ReasonCapacityComments],[ReasonClarification],[ReasonClarificationComments],[ReasonDefective],[ReasonDefectiveComments],[ReasonMaterial],[ReasonMaterialComments],[ReasonQuality],[ReasonQualityComments],[ReasonStatusP],[ReasonStatusPComments]) OUTPUT INSERTED.[Id] VALUES (@ArticleId,@ArticleNumber,@CustomerName,@CustomerNumber,@CustomerOrderNumber,@EditDate,@EditUserId,@EditUserName,@FertigungId,@FertigungNumber,@InsConfirmation,@InsConfirmationDate,@InsConfirmationUserId,@InsConfirmationUserName,@NewWorkDate,@OldWorkDate,@ReasonCapacity,@ReasonCapacityComments,@ReasonClarification,@ReasonClarificationComments,@ReasonDefective,@ReasonDefectiveComments,@ReasonMaterial,@ReasonMaterialComments,@ReasonQuality,@ReasonQualityComments,@ReasonStatusP,@ReasonStatusPComments); ";


			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
			sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
			sqlCommand.Parameters.AddWithValue("EditUserId", item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
			sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
			sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
			sqlCommand.Parameters.AddWithValue("FertigungNumber", item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
			sqlCommand.Parameters.AddWithValue("InsConfirmation", item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
			sqlCommand.Parameters.AddWithValue("InsConfirmationDate", item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
			sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
			sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
			sqlCommand.Parameters.AddWithValue("NewWorkDate", item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
			sqlCommand.Parameters.AddWithValue("OldWorkDate", item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
			sqlCommand.Parameters.AddWithValue("ReasonCapacity", item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
			sqlCommand.Parameters.AddWithValue("ReasonCapacityComments", item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
			sqlCommand.Parameters.AddWithValue("ReasonClarification", item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
			sqlCommand.Parameters.AddWithValue("ReasonClarificationComments", item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
			sqlCommand.Parameters.AddWithValue("ReasonDefective", item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
			sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments", item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
			sqlCommand.Parameters.AddWithValue("ReasonMaterial", item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
			sqlCommand.Parameters.AddWithValue("ReasonMaterialComments", item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
			sqlCommand.Parameters.AddWithValue("ReasonQuality", item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
			sqlCommand.Parameters.AddWithValue("ReasonQualityComments", item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
			sqlCommand.Parameters.AddWithValue("ReasonStatusP", item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
			sqlCommand.Parameters.AddWithValue("ReasonStatusPComments", item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);

			var result = sqlCommand.ExecuteScalar();
			return result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
		}
		public static int InsertWithTransaction(List<InsideSalesWerksterminUpdatesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int insertWithTransaction(List<InsideSalesWerksterminUpdatesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				string query = "";
				var sqlCommand = new SqlCommand(query, connection, transaction);

				int i = 0;
				foreach(var item in items)
				{
					i++;
					query += " INSERT INTO [__CTS_InsideSalesWerksterminUpdates] ([ArticleId],[ArticleNumber],[CustomerName],[CustomerNumber],[CustomerOrderNumber],[EditDate],[EditUserId],[EditUserName],[FertigungId],[FertigungNumber],[InsConfirmation],[InsConfirmationDate],[InsConfirmationUserId],[InsConfirmationUserName],[NewWorkDate],[OldWorkDate],[ReasonCapacity],[ReasonCapacityComments],[ReasonClarification],[ReasonClarificationComments],[ReasonDefective],[ReasonDefectiveComments],[ReasonMaterial],[ReasonMaterialComments],[ReasonQuality],[ReasonQualityComments],[ReasonStatusP],[ReasonStatusPComments]) VALUES ( "

						+ "@ArticleId" + i + ","
						+ "@ArticleNumber" + i + ","
						+ "@CustomerName" + i + ","
						+ "@CustomerNumber" + i + ","
						+ "@CustomerOrderNumber" + i + ","
						+ "@EditDate" + i + ","
						+ "@EditUserId" + i + ","
						+ "@EditUserName" + i + ","
						+ "@FertigungId" + i + ","
						+ "@FertigungNumber" + i + ","
						+ "@InsConfirmation" + i + ","
						+ "@InsConfirmationDate" + i + ","
						+ "@InsConfirmationUserId" + i + ","
						+ "@InsConfirmationUserName" + i + ","
						+ "@NewWorkDate" + i + ","
						+ "@OldWorkDate" + i + ","
						+ "@ReasonCapacity" + i + ","
						+ "@ReasonCapacityComments" + i + ","
						+ "@ReasonClarification" + i + ","
						+ "@ReasonClarificationComments" + i + ","
						+ "@ReasonDefective" + i + ","
						+ "@ReasonDefectiveComments" + i + ","
						+ "@ReasonMaterial" + i + ","
						+ "@ReasonMaterialComments" + i + ","
						+ "@ReasonQuality" + i + ","
						+ "@ReasonQualityComments" + i + ","
						+ "@ReasonStatusP" + i + ","
						+ "@ReasonStatusPComments" + i
							+ "); ";


					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
					sqlCommand.Parameters.AddWithValue("EditUserId" + i, item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
					sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
					sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
					sqlCommand.Parameters.AddWithValue("FertigungNumber" + i, item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
					sqlCommand.Parameters.AddWithValue("InsConfirmation" + i, item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
					sqlCommand.Parameters.AddWithValue("InsConfirmationDate" + i, item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserId" + i, item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserName" + i, item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
					sqlCommand.Parameters.AddWithValue("NewWorkDate" + i, item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
					sqlCommand.Parameters.AddWithValue("OldWorkDate" + i, item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
					sqlCommand.Parameters.AddWithValue("ReasonCapacity" + i, item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
					sqlCommand.Parameters.AddWithValue("ReasonCapacityComments" + i, item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
					sqlCommand.Parameters.AddWithValue("ReasonClarification" + i, item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
					sqlCommand.Parameters.AddWithValue("ReasonClarificationComments" + i, item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
					sqlCommand.Parameters.AddWithValue("ReasonDefective" + i, item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
					sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments" + i, item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
					sqlCommand.Parameters.AddWithValue("ReasonMaterial" + i, item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
					sqlCommand.Parameters.AddWithValue("ReasonMaterialComments" + i, item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
					sqlCommand.Parameters.AddWithValue("ReasonQuality" + i, item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
					sqlCommand.Parameters.AddWithValue("ReasonQualityComments" + i, item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
					sqlCommand.Parameters.AddWithValue("ReasonStatusP" + i, item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
					sqlCommand.Parameters.AddWithValue("ReasonStatusPComments" + i, item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);
				}

				sqlCommand.CommandText = query;

				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int UpdateWithTransaction(InsideSalesWerksterminUpdatesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "UPDATE [__CTS_InsideSalesWerksterminUpdates] SET [ArticleId]=@ArticleId, [ArticleNumber]=@ArticleNumber, [CustomerName]=@CustomerName, [CustomerNumber]=@CustomerNumber, [CustomerOrderNumber]=@CustomerOrderNumber, [EditDate]=@EditDate, [EditUserId]=@EditUserId, [EditUserName]=@EditUserName, [FertigungId]=@FertigungId, [FertigungNumber]=@FertigungNumber, [InsConfirmation]=@InsConfirmation, [InsConfirmationDate]=@InsConfirmationDate, [InsConfirmationUserId]=@InsConfirmationUserId, [InsConfirmationUserName]=@InsConfirmationUserName, [NewWorkDate]=@NewWorkDate, [OldWorkDate]=@OldWorkDate, [ReasonCapacity]=@ReasonCapacity, [ReasonCapacityComments]=@ReasonCapacityComments, [ReasonClarification]=@ReasonClarification, [ReasonClarificationComments]=@ReasonClarificationComments, [ReasonDefective]=@ReasonDefective, [ReasonDefectiveComments]=@ReasonDefectiveComments, [ReasonMaterial]=@ReasonMaterial, [ReasonMaterialComments]=@ReasonMaterialComments, [ReasonQuality]=@ReasonQuality, [ReasonQualityComments]=@ReasonQualityComments, [ReasonStatusP]=@ReasonStatusP, [ReasonStatusPComments]=@ReasonStatusPComments WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
			sqlCommand.Parameters.AddWithValue("ArticleNumber", item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
			sqlCommand.Parameters.AddWithValue("CustomerName", item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
			sqlCommand.Parameters.AddWithValue("CustomerNumber", item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
			sqlCommand.Parameters.AddWithValue("CustomerOrderNumber", item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
			sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? (object)DBNull.Value : item.EditDate);
			sqlCommand.Parameters.AddWithValue("EditUserId", item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
			sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
			sqlCommand.Parameters.AddWithValue("FertigungId", item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
			sqlCommand.Parameters.AddWithValue("FertigungNumber", item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
			sqlCommand.Parameters.AddWithValue("InsConfirmation", item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
			sqlCommand.Parameters.AddWithValue("InsConfirmationDate", item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
			sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
			sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
			sqlCommand.Parameters.AddWithValue("NewWorkDate", item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
			sqlCommand.Parameters.AddWithValue("OldWorkDate", item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
			sqlCommand.Parameters.AddWithValue("ReasonCapacity", item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
			sqlCommand.Parameters.AddWithValue("ReasonCapacityComments", item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
			sqlCommand.Parameters.AddWithValue("ReasonClarification", item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
			sqlCommand.Parameters.AddWithValue("ReasonClarificationComments", item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
			sqlCommand.Parameters.AddWithValue("ReasonDefective", item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
			sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments", item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
			sqlCommand.Parameters.AddWithValue("ReasonMaterial", item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
			sqlCommand.Parameters.AddWithValue("ReasonMaterialComments", item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
			sqlCommand.Parameters.AddWithValue("ReasonQuality", item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
			sqlCommand.Parameters.AddWithValue("ReasonQualityComments", item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
			sqlCommand.Parameters.AddWithValue("ReasonStatusP", item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
			sqlCommand.Parameters.AddWithValue("ReasonStatusPComments", item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);

			results = sqlCommand.ExecuteNonQuery();
			return results;
		}
		public static int UpdateWithTransaction(List<InsideSalesWerksterminUpdatesEntity> items, SqlConnection connection, SqlTransaction transaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 29; // Nb params per query
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
		private static int updateWithTransaction(List<InsideSalesWerksterminUpdatesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [__CTS_InsideSalesWerksterminUpdates] SET "

					+ "[ArticleId]=@ArticleId" + i + ","
					+ "[ArticleNumber]=@ArticleNumber" + i + ","
					+ "[CustomerName]=@CustomerName" + i + ","
					+ "[CustomerNumber]=@CustomerNumber" + i + ","
					+ "[CustomerOrderNumber]=@CustomerOrderNumber" + i + ","
					+ "[EditDate]=@EditDate" + i + ","
					+ "[EditUserId]=@EditUserId" + i + ","
					+ "[EditUserName]=@EditUserName" + i + ","
					+ "[FertigungId]=@FertigungId" + i + ","
					+ "[FertigungNumber]=@FertigungNumber" + i + ","
					+ "[InsConfirmation]=@InsConfirmation" + i + ","
					+ "[InsConfirmationDate]=@InsConfirmationDate" + i + ","
					+ "[InsConfirmationUserId]=@InsConfirmationUserId" + i + ","
					+ "[InsConfirmationUserName]=@InsConfirmationUserName" + i + ","
					+ "[NewWorkDate]=@NewWorkDate" + i + ","
					+ "[OldWorkDate]=@OldWorkDate" + i + ","
					+ "[ReasonCapacity]=@ReasonCapacity" + i + ","
					+ "[ReasonCapacityComments]=@ReasonCapacityComments" + i + ","
					+ "[ReasonClarification]=@ReasonClarification" + i + ","
					+ "[ReasonClarificationComments]=@ReasonClarificationComments" + i + ","
					+ "[ReasonDefective]=@ReasonDefective" + i + ","
					+ "[ReasonDefectiveComments]=@ReasonDefectiveComments" + i + ","
					+ "[ReasonMaterial]=@ReasonMaterial" + i + ","
					+ "[ReasonMaterialComments]=@ReasonMaterialComments" + i + ","
					+ "[ReasonQuality]=@ReasonQuality" + i + ","
					+ "[ReasonQualityComments]=@ReasonQualityComments" + i + ","
					+ "[ReasonStatusP]=@ReasonStatusP" + i + ","
					+ "[ReasonStatusPComments]=@ReasonStatusPComments" + i + " WHERE [Id]=@Id" + i
						+ "; ";

					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					sqlCommand.Parameters.AddWithValue("ArticleId" + i, item.ArticleId == null ? (object)DBNull.Value : item.ArticleId);
					sqlCommand.Parameters.AddWithValue("ArticleNumber" + i, item.ArticleNumber == null ? (object)DBNull.Value : item.ArticleNumber);
					sqlCommand.Parameters.AddWithValue("CustomerName" + i, item.CustomerName == null ? (object)DBNull.Value : item.CustomerName);
					sqlCommand.Parameters.AddWithValue("CustomerNumber" + i, item.CustomerNumber == null ? (object)DBNull.Value : item.CustomerNumber);
					sqlCommand.Parameters.AddWithValue("CustomerOrderNumber" + i, item.CustomerOrderNumber == null ? (object)DBNull.Value : item.CustomerOrderNumber);
					sqlCommand.Parameters.AddWithValue("EditDate" + i, item.EditDate == null ? (object)DBNull.Value : item.EditDate);
					sqlCommand.Parameters.AddWithValue("EditUserId" + i, item.EditUserId == null ? (object)DBNull.Value : item.EditUserId);
					sqlCommand.Parameters.AddWithValue("EditUserName" + i, item.EditUserName == null ? (object)DBNull.Value : item.EditUserName);
					sqlCommand.Parameters.AddWithValue("FertigungId" + i, item.FertigungId == null ? (object)DBNull.Value : item.FertigungId);
					sqlCommand.Parameters.AddWithValue("FertigungNumber" + i, item.FertigungNumber == null ? (object)DBNull.Value : item.FertigungNumber);
					sqlCommand.Parameters.AddWithValue("InsConfirmation" + i, item.InsConfirmation == null ? (object)DBNull.Value : item.InsConfirmation);
					sqlCommand.Parameters.AddWithValue("InsConfirmationDate" + i, item.InsConfirmationDate == null ? (object)DBNull.Value : item.InsConfirmationDate);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserId" + i, item.InsConfirmationUserId == null ? (object)DBNull.Value : item.InsConfirmationUserId);
					sqlCommand.Parameters.AddWithValue("InsConfirmationUserName" + i, item.InsConfirmationUserName == null ? (object)DBNull.Value : item.InsConfirmationUserName);
					sqlCommand.Parameters.AddWithValue("NewWorkDate" + i, item.NewWorkDate == null ? (object)DBNull.Value : item.NewWorkDate);
					sqlCommand.Parameters.AddWithValue("OldWorkDate" + i, item.OldWorkDate == null ? (object)DBNull.Value : item.OldWorkDate);
					sqlCommand.Parameters.AddWithValue("ReasonCapacity" + i, item.ReasonCapacity == null ? (object)DBNull.Value : item.ReasonCapacity);
					sqlCommand.Parameters.AddWithValue("ReasonCapacityComments" + i, item.ReasonCapacityComments == null ? (object)DBNull.Value : item.ReasonCapacityComments);
					sqlCommand.Parameters.AddWithValue("ReasonClarification" + i, item.ReasonClarification == null ? (object)DBNull.Value : item.ReasonClarification);
					sqlCommand.Parameters.AddWithValue("ReasonClarificationComments" + i, item.ReasonClarificationComments == null ? (object)DBNull.Value : item.ReasonClarificationComments);
					sqlCommand.Parameters.AddWithValue("ReasonDefective" + i, item.ReasonDefective == null ? (object)DBNull.Value : item.ReasonDefective);
					sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments" + i, item.ReasonDefectiveComments == null ? (object)DBNull.Value : item.ReasonDefectiveComments);
					sqlCommand.Parameters.AddWithValue("ReasonMaterial" + i, item.ReasonMaterial == null ? (object)DBNull.Value : item.ReasonMaterial);
					sqlCommand.Parameters.AddWithValue("ReasonMaterialComments" + i, item.ReasonMaterialComments == null ? (object)DBNull.Value : item.ReasonMaterialComments);
					sqlCommand.Parameters.AddWithValue("ReasonQuality" + i, item.ReasonQuality == null ? (object)DBNull.Value : item.ReasonQuality);
					sqlCommand.Parameters.AddWithValue("ReasonQualityComments" + i, item.ReasonQualityComments == null ? (object)DBNull.Value : item.ReasonQualityComments);
					sqlCommand.Parameters.AddWithValue("ReasonStatusP" + i, item.ReasonStatusP == null ? (object)DBNull.Value : item.ReasonStatusP);
					sqlCommand.Parameters.AddWithValue("ReasonStatusPComments" + i, item.ReasonStatusPComments == null ? (object)DBNull.Value : item.ReasonStatusPComments);
				}

				sqlCommand.CommandText = query;
				return sqlCommand.ExecuteNonQuery();
			}

			return -1;
		}

		public static int DeleteWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;

			string query = "DELETE FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			results = sqlCommand.ExecuteNonQuery();


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

				string query = "DELETE FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [Id] IN (" + queryIds + ")";
				sqlCommand.CommandText = query;

				results = sqlCommand.ExecuteNonQuery();


				return results;
			}
			return -1;
		}
		#endregion Methods with transaction
		#endregion Default Methods

		#region Custom Methods

		public static List<InsideSalesWerksterminUpdatesEntity> GetWerksterminHistoryByInsConfirmation(bool insConfirmation)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = @$"SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] W WHERE W.InsConfirmation=@insconfirmation";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("insconfirmation", insConfirmation);
				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new InsideSalesWerksterminUpdatesEntity(x)).ToList();
			}
			else
			{
				return new List<InsideSalesWerksterminUpdatesEntity>();
			}
		}


		public static int updateByFertigungNummer(InsideSalesWerksterminUpdatesEntity item)
		{
			int result = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesWerksterminUpdates] SET [EditDate]=@EditDate, [EditUserId]=@EditUserId, [EditUserName]=@EditUserName,[InsConfirmation]=@InsConfirmation, " +
					"[InsConfirmationDate]=@InsConfirmationDate, [InsConfirmationUserId]=@InsConfirmationUserId, [InsConfirmationUserName]=@InsConfirmationUserName, [NewWorkDate]=@NewWorkDate, " +
					"[OldWorkDate]=@OldWorkDate, [ReasonCapacity]=@ReasonCapacity, [ReasonCapacityComments]=@ReasonCapacityComments, " +
					"[ReasonDefective]=@ReasonDefective, [ReasonDefectiveComments]=@ReasonDefectiveComments, [ReasonMaterial]=@ReasonMaterial, " +
					"[ReasonMaterialComments]=@ReasonMaterialComments, [ArticleId]=@ArticleId, [ReasonQuality]=@ReasonQuality,[ReasonQualityComments]=@ReasonQualityComments,[ReasonStatusP]=@ReasonStatusP,[ReasonStatusPComments]=@ReasonStatusPComments" +
					" WHERE  [FertigungNumber]=@FertigungNumber";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("FertigungNumber", item.FertigungNumber == null ? DBNull.Value : item.FertigungNumber);
				sqlCommand.Parameters.AddWithValue("ArticleId", item.ArticleId == null ? DBNull.Value : item.ArticleId);
				sqlCommand.Parameters.AddWithValue("EditDate", item.EditDate == null ? DBNull.Value : item.EditDate);
				sqlCommand.Parameters.AddWithValue("EditUserId", item.EditUserId == null ? DBNull.Value : item.EditUserId);
				sqlCommand.Parameters.AddWithValue("EditUserName", item.EditUserName == null ? DBNull.Value : item.EditUserName);
				sqlCommand.Parameters.AddWithValue("InsConfirmation", item.InsConfirmation);
				sqlCommand.Parameters.AddWithValue("InsConfirmationDate", item.InsConfirmationDate == null ? DBNull.Value : item.InsConfirmationDate);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", item.InsConfirmationUserId == null ? DBNull.Value : item.InsConfirmationUserId);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", item.InsConfirmationUserName == null ? DBNull.Value : item.InsConfirmationUserName);
				sqlCommand.Parameters.AddWithValue("NewWorkDate", item.NewWorkDate == null ? DBNull.Value : item.NewWorkDate);
				sqlCommand.Parameters.AddWithValue("OldWorkDate", item.OldWorkDate == null ? DBNull.Value : item.OldWorkDate);
				sqlCommand.Parameters.AddWithValue("ReasonCapacity", item.ReasonCapacity == null ? DBNull.Value : item.ReasonCapacity);
				sqlCommand.Parameters.AddWithValue("ReasonCapacityComments", item.ReasonCapacityComments == null ? DBNull.Value : item.ReasonCapacityComments);
				sqlCommand.Parameters.AddWithValue("ReasonDefective", item.ReasonDefective == null ? DBNull.Value : item.ReasonDefective);
				sqlCommand.Parameters.AddWithValue("ReasonDefectiveComments", item.ReasonDefectiveComments == null ? DBNull.Value : item.ReasonDefectiveComments);
				sqlCommand.Parameters.AddWithValue("ReasonMaterial", item.ReasonMaterial == null ? DBNull.Value : item.ReasonMaterial);
				sqlCommand.Parameters.AddWithValue("ReasonMaterialComments", item.ReasonMaterialComments == null ? DBNull.Value : item.ReasonMaterialComments);
				sqlCommand.Parameters.AddWithValue("ReasonQuality", item.ReasonQuality == null ? DBNull.Value : item.ReasonQuality);
				sqlCommand.Parameters.AddWithValue("ReasonQualityComments", item.ReasonQualityComments == null ? DBNull.Value : item.ReasonQualityComments);
				sqlCommand.Parameters.AddWithValue("ReasonStatusP", item.ReasonStatusP == null ? DBNull.Value : item.ReasonStatusP);
				sqlCommand.Parameters.AddWithValue("ReasonStatusPComments", item.ReasonStatusPComments == null ? DBNull.Value : item.ReasonStatusPComments);

				result = sqlCommand.ExecuteNonQuery();
			}

			return result;
		}
		public static InsideSalesWerksterminUpdatesEntity GetByFertigungNumber(int Nr)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__CTS_InsideSalesWerksterminUpdates] WHERE [FertigungNumber]=@Nr";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Nr", Nr);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new InsideSalesWerksterminUpdatesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static int UpdateInsByFertigungNummer(int WerkterminId, bool insConfirmation, int userId, string username, DateTime insConfirmationDate)
		{
			int result = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__CTS_InsideSalesWerksterminUpdates] SET [InsConfirmation]=@InsConfirmation,[InsConfirmationDate]=@InsConfirmationDate, [InsConfirmationUserId]=@InsConfirmationUserId, [InsConfirmationUserName]=@InsConfirmationUserName WHERE [Id]=@werkterminId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("werkterminId", WerkterminId);
				sqlCommand.Parameters.AddWithValue("InsConfirmation", insConfirmation);
				sqlCommand.Parameters.AddWithValue("InsConfirmationDate", insConfirmationDate);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserId", userId);
				sqlCommand.Parameters.AddWithValue("InsConfirmationUserName", username);

				result = sqlCommand.ExecuteNonQuery();
			}

			return result;
		}
		#endregion Custom Methods

	}
}
