namespace Infrastructure.Data.Access.Tables.PRS
{
	public class CustomerUserAccess
	{
		#region Default Methods
		public static Entities.Tables.PRS.CustomerUserEntity Get(int id)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = "SELECT * FROM [EDI_CustomerUser] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.CustomerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.CustomerUserEntity> Get()
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [EDI_CustomerUser]";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.CustomerUserEntity>();
			}
		}
		public static List<Entities.Tables.PRS.CustomerUserEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.CustomerUserEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.CustomerUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}
		private static List<Entities.Tables.PRS.CustomerUserEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [EDI_CustomerUser] WHERE [Id] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.CustomerUserEntity>();
				}
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}

		public static int Insert(Entities.Tables.PRS.CustomerUserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "INSERT INTO [EDI_CustomerUser] "
					+ " ([CustomerId],[IsPrimary],[UserId],[ValidIntoTime],[ValidFromTime]) "
					+ " VALUES "
					+ " (@CustomerId,@IsPrimary,@UserId,@ValidIntoTime,@ValidFromTime); ";
				query += "SELECT SCOPE_IDENTITY();";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("CustomerId", element.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("IsPrimary", element.IsPrimary);
				sqlCommand.Parameters.AddWithValue("UserId", element.UserId);
				sqlCommand.Parameters.AddWithValue("ValidIntoTime", element.ValidIntoTime);
				sqlCommand.Parameters.AddWithValue("ValidFromTime", element.ValidFromTime);

				var result = sqlCommand.ExecuteScalar();
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}

		public static int Update(Entities.Tables.PRS.CustomerUserEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE [EDI_CustomerUser] SET "
					+ " [CustomerId]=@CustomerId,[IsPrimary]=@IsPrimary,[UserId]=@UserId, "
					+ " [ValidIntoTime]=@ValidIntoTime,[ValidFromTime]=@ValidFromTime "
					+ " WHERE [Id]=@Id ";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("CustomerId", element.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("IsPrimary", element.IsPrimary);
				sqlCommand.Parameters.AddWithValue("UserId", element.UserId);
				sqlCommand.Parameters.AddWithValue("ValidIntoTime", element.ValidIntoTime);
				sqlCommand.Parameters.AddWithValue("ValidFromTime", element.ValidFromTime);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}

		public static int Delete(int id)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [EDI_CustomerUser] WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
				int result = 0;
				if(ids.Count <= maxParamsNumber)
				{
					result = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						result += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					result += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int response = -1;

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					string query = "DELETE FROM [EDI_CustomerUser] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					response = sqlCommand.ExecuteNonQuery();
				}

				return response;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.PRS.CustomerUserEntity> GetByCustomerNumber(int customerNumber)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = "SELECT * FROM [EDI_CustomerUser] WHERE [CustomerId]=@customerId";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("customerId", customerNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.CustomerUserEntity>();
			}
		}

		public static List<Entities.Tables.PRS.CustomerUserEntity> GetByCustomersNumbers(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.CustomerUserEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					result = getByCustomersNumbers(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.CustomerUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByCustomersNumbers(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByCustomersNumbers(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}
		private static List<Entities.Tables.PRS.CustomerUserEntity> getByCustomersNumbers(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = "SELECT * FROM [EDI_CustomerUser] WHERE [CustomerId] IN (" + queryIds + ")";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.CustomerUserEntity>();
				}
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}

		public static Entities.Tables.PRS.CustomerUserEntity GetBy_IsPrimary_CustomerNumber(bool isPrimary, int customerNumber)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = "SELECT * FROM [EDI_CustomerUser] WHERE [CustomerId]=@customerId AND [IsPrimary]=@isPrimary";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("isPrimary", isPrimary);
				sqlCommand.Parameters.AddWithValue("customerId", customerNumber);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.CustomerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.CustomerUserEntity> GetBy_IsPrimary_CustomerNumber(bool isPrimary, List<int> customerIds)
		{
			if(customerIds == null || customerIds.Count <= 0)
			{
				return null;
			}
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = $"SELECT * FROM [EDI_CustomerUser] WHERE [CustomerId] IN ({string.Join(",", customerIds)}) AND [IsPrimary]=@isPrimary";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("isPrimary", isPrimary);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.CustomerUserEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}

		public static Entities.Tables.PRS.CustomerUserEntity GetBy_CustomerNumber_UserId(int customerNumber, int userId)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = $"SELECT * FROM [EDI_CustomerUser] WHERE [CustomerId]=@customerId AND [UserId]=@userId AND ([IsPrimary]=1 OR (ISNULL([IsPrimary],0)=0 AND [ValidIntoTime]>='{DateTime.Now.ToString("yyyyMMdd")}' AND [ValidFromTime]<='{DateTime.Now.ToString("yyyyMMdd")}') )";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("customerId", customerNumber);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.CustomerUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.PRS.CustomerUserEntity> GetByUserId(int userId)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = $"SELECT * FROM [EDI_CustomerUser] WHERE [UserId]=@userId AND ([IsPrimary]=1 OR (ISNULL([IsPrimary],0)=0 AND [ValidIntoTime]>='{DateTime.Now.ToString("yyyyMMdd")}' AND [ValidFromTime]<='{DateTime.Now.ToString("yyyyMMdd")}') )";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.CustomerUserEntity>();
			}
		}
		public static List<Entities.Tables.PRS.CustomerUserEntity> GetByUserId(int userId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			var dataTable = new DataTable();

			string query = $"SELECT * FROM [EDI_CustomerUser] WHERE [UserId]=@userId AND ([IsPrimary]=1 OR (ISNULL([IsPrimary],0)=0 AND [ValidIntoTime]>='{DateTime.Now.ToString("yyyyMMdd")}' AND [ValidFromTime]<='{DateTime.Now.ToString("yyyyMMdd")}') )";

			var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
			sqlCommand.Parameters.AddWithValue("userId", userId);

			new SqlDataAdapter(sqlCommand).Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.CustomerUserEntity>();
			}
		}

		public static List<Entities.Tables.PRS.CustomerUserEntity> GetByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var result = new List<Entities.Tables.PRS.CustomerUserEntity>();
				if(usersIds.Count <= maxQueryNumber)
				{
					result = getByUsersIds(usersIds);
				}
				else
				{
					int batchNumber = usersIds.Count / maxQueryNumber;
					result = new List<Entities.Tables.PRS.CustomerUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						result.AddRange(getByUsersIds(usersIds.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					result.AddRange(getByUsersIds(usersIds.GetRange(batchNumber * maxQueryNumber, usersIds.Count - batchNumber * maxQueryNumber)));
				}
				return result;
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}
		private static List<Entities.Tables.PRS.CustomerUserEntity> getByUsersIds(List<int> usersIds)
		{
			if(usersIds != null && usersIds.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
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

					sqlCommand.CommandText = $"SELECT * FROM [EDI_CustomerUser] WHERE [UserId] IN ({queryIds}) AND ([IsPrimary]=1 OR (ISNULL([IsPrimary],0)=0 AND [ValidIntoTime]>='{DateTime.Now.ToString("yyyyMMdd")}' AND [ValidFromTime]<='{DateTime.Now.ToString("yyyyMMdd")}') )";

					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.PRS.CustomerUserEntity>();
				}
			}
			return new List<Entities.Tables.PRS.CustomerUserEntity>();
		}

		public static List<Entities.Tables.PRS.CustomerUserEntity> GetBy_IsPrimary_UserId(bool isPrimary, int userId)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = "SELECT * FROM [EDI_CustomerUser] WHERE [IsPrimary]=@isPrimary AND [UserId]=@userId";

				var sqlCommand = new SqlCommand(query, sqlconnection);
				sqlCommand.Parameters.AddWithValue("isPrimary", isPrimary);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.PRS.CustomerUserEntity>();
			}
		}

		public static int DeleteBy_IsPrimary_CustomerNumber(bool isPrimary, int customerNumber)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [EDI_CustomerUser] WHERE [CustomerId]=@customerId AND [IsPrimary]=@isPrimary";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("isPrimary", isPrimary);
				sqlCommand.Parameters.AddWithValue("customerId", customerNumber);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}

		public static int DeleteBy_CustomerNumber_UserId(int customerNumber, int userId)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [EDI_CustomerUser] WHERE [CustomerId]=@customerId AND [UserId]=@userId";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("customerId", customerNumber);
				sqlCommand.Parameters.AddWithValue("userId", userId);

				response = sqlCommand.ExecuteNonQuery();
			}

			return response;
		}


		public static List<Entities.Joins.CTS.AppointmentCustomerUserEntity> GetCustomersUsersList(string customername, string employeeName)
		{

			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();

				string query = $"SELECT k.Nr as [Id], " +
					$"(CASE WHEN a.Kundennummer is null THEN k.nummer ELSE a.Kundennummer END)  AS [CustomerNumber], a.[Name1] as [CustomerName], " +
					$" a.[Name2] as CustomerName2," +
					$" a.[Name3] as CustomerName3,    a.Straﬂe as CustomerAddress,    a.Anrede as CustomerType,    " +
					$" a.Briefanrede as CustomerContact,    " +
					$" u.Name as EmployeeName,u.Id as EmployeeId," +
					$" ap.Id as AccessProfileId,ap.Name as AccessProfileName" +
					$" FROM [EDI_CustomerUser] cu inner join [adressen] a on cu.CustomerId=a.Nr" +
					$" inner join [Kunden] k on cu.CustomerId = k.nummer" +
					$" inner join [User] u on u.Id = cu.UserId" +
					$" inner join [_AccessProfile] ap on ap.Id = u.AccessProfileId WHERE a.Kundennummer IS NOT NULL AND cu.IsPrimary=1";

				var clauses = new List<string>();

				if(!String.IsNullOrWhiteSpace(customername))
				{
					clauses.Add($" a.[Name1] LIKE '{customername}%' ");
				}
				if(!String.IsNullOrWhiteSpace(employeeName))
				{
					clauses.Add($" u.[Name]='{employeeName}' ");
				}

				if(clauses.Count > 0)
				{
					query += $" AND {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlconnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.CTS.AppointmentCustomerUserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.CTS.AppointmentCustomerUserEntity>();
			}
		}


		public static List<Entities.Joins.CTS.AppointmentCustomerUserEntity> GetCustomerWithoutEmployees(string customername)
		{
			var dataTable = new DataTable();

			using(var sqlconnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlconnection.Open();
				var clauses = new List<string>();

				string query = $"  SELECT k.Nr as [Id], "
					+ "(CASE WHEN a.Kundennummer is null THEN k.nummer ELSE a.Kundennummer END)  AS [CustomerNumber], "
								 + "a.[Name1] as [CustomerName], "
								  + " a.[Name2] as CustomerName2, "
								  + " a.[Name3] as CustomerName3, "
								 + "  a.Straﬂe as CustomerAddress, "
								 + "  a.Anrede as CustomerType, "
								 + "  a.Briefanrede as CustomerContact,"
								 + "  '' as EmployeeName, "
								  + " 0 as EmployeeId, "
								 + "  0 as AccessProfileId,"
								  + " '' as AccessProfileName"
									+ " FROM [adressen] a"
									+ " LEFT JOIN [kunden] k ON a.[Nr] = k.[nummer]";

				clauses.Add("a.Kundennummer IS NOT NULL");
				clauses.Add("a.Nr not in (select CustomerId from [EDI_CustomerUser] WHERE IsPrimary=1)");
				clauses.Add("a.Name1 IS NOT NULL");
				clauses.Add("k.Nr IS NOT NULL");
				if(!String.IsNullOrWhiteSpace(customername))
				{
					clauses.Add($" a.[Name1] LIKE '{customername}%' ");
				}


				if(clauses.Count > 0)
				{
					query += $" WHERE {string.Join(" AND ", clauses)}";
				}

				var sqlCommand = new SqlCommand(query, sqlconnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Joins.CTS.AppointmentCustomerUserEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Joins.CTS.AppointmentCustomerUserEntity>();
			}
		}

		#endregion

		#region Helpers
		private static List<Entities.Tables.PRS.CustomerUserEntity> toList(DataTable dataTable)
		{
			var result = new List<Entities.Tables.PRS.CustomerUserEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{
				result.Add(new Entities.Tables.PRS.CustomerUserEntity(dataRow));
			}
			return result;
		}
		#endregion
	}
}
