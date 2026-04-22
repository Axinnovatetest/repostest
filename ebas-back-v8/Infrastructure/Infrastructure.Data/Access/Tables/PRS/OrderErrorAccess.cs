using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public class OrderErrorAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity Get(int id)
		{
			var Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError WHERE [Id]=@Id";
				SqlCommand sqlCommand = new SqlCommand(query, cnn);
				sqlCommand.Parameters.AddWithValue("Id", id);

				using(var reader = DbExecution.ExecuteReader(sqlCommand))
				{
					Lt = toList(reader);
				}
			}
			return Lt.Count > 0 ? Lt[0] : null;
		}

		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Get()
		{
			var Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError";
				SqlCommand sqlCommand = new SqlCommand(query, cnn);

				using(var reader = DbExecution.ExecuteReader(sqlCommand))
				{
					Lt = toList(reader);
				}
			}

			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				var response = new List<Entities.Tables.PRS.OrderErrorEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					response = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					response = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						response.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					response.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return response;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();
				using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					cnn.Open();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = cnn;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					sqlCommand.CommandText = "SELECT * FROM EDI_OrderError WHERE [Id] IN (" + queryIds + ")";

					using(var reader = DbExecution.ExecuteReader(sqlCommand))
					{
						Lt = toList(reader);
					}
				}
				return Lt;
			}
			return new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity element)
		{
			int response = -1;
			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "INSERT INTO EDI_OrderError "
					+ " ([Error],[FileName],ClientName,Validated,ClientId,ValidationUserId,ValidationTime, CustomerNumber, CreationTime) "
					+ " VALUES "
					+ " (@Error,@FileName,@ClientName,@Validated,@ClientId,@ValidationUserId,@ValidationTime, @CustomerNumber, @CreationTime);";
				query += "SELECT SCOPE_IDENTITY();";

				SqlCommand sqlCommand = new SqlCommand(query, cnn);
				sqlCommand.Parameters.AddWithValue("Error", element.Error);
				sqlCommand.Parameters.AddWithValue("FileName", element.FileName);
				sqlCommand.Parameters.AddWithValue("ClientName", element.CustomerName);
				sqlCommand.Parameters.AddWithValue("Validated", element.Validated);
				sqlCommand.Parameters.AddWithValue("ClientId", element.CustomerId);

				sqlCommand.Parameters.AddWithValue("ValidationUserId", element.ValidationUserId == null ? (object)DBNull.Value : element.ValidationUserId);
				sqlCommand.Parameters.AddWithValue("ValidationTime", element.ValidationTime == null ? (object)DBNull.Value : element.ValidationTime);

				sqlCommand.Parameters.AddWithValue("CustomerNumber", element.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
			}

			return response;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity element)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "UPDATE EDI_OrderError SET "
					+ " ClientId=@ClientId,Validated=@Validated,ClientName=@ClientName,[Error]=@Error, "
					+ " [FileName]=@FileName,ValidationUserId=@ValidationUserId,ValidationTime=@ValidationTime, "
					+ " [CustomerNumber]=@CustomerNumber, [CreationTime]=@CreationTime "
					+ " WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", element.Id);
				sqlCommand.Parameters.AddWithValue("ClientId", element.CustomerId);
				sqlCommand.Parameters.AddWithValue("Validated", element.Validated);
				sqlCommand.Parameters.AddWithValue("Error", element.Error);
				sqlCommand.Parameters.AddWithValue("ClientName", element.CustomerName);
				sqlCommand.Parameters.AddWithValue("FileName", element.FileName);

				sqlCommand.Parameters.AddWithValue("ValidationUserId", element.ValidationUserId == null ? (object)DBNull.Value : element.ValidationUserId);
				sqlCommand.Parameters.AddWithValue("ValidationTime", element.ValidationTime == null ? (object)DBNull.Value : element.ValidationTime);

				sqlCommand.Parameters.AddWithValue("CustomerNumber", element.CustomerNumber);
				sqlCommand.Parameters.AddWithValue("CreationTime", element.CreationTime);

				response = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return response;
		}

		public static int Delete(int id)
		{
			int r = -1;
			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "DELETE FROM EDI_OrderError WHERE [Id]=@Id";
				SqlCommand sqlCommand = new SqlCommand(query, cnn);
				sqlCommand.Parameters.AddWithValue("Id", id);

				r = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return r;
		}
		public static int Delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = Infrastructure.Data.Access.Settings.MAX_BATCH_SIZE;
				int response = 0;
				if(ids.Count <= maxParamsNumber)
				{
					response = delete(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						response += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber));
					}
					response += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber));
				}
			}
			return -1;
		}
		private static int delete(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int r = -1;
				using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
				{
					cnn.Open();
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = cnn;

					string queryIds = string.Empty;
					for(int i = 0; i < ids.Count; i++)
					{
						queryIds += "@Id" + i + ",";
						sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
					}
					queryIds = queryIds.TrimEnd(',');

					string query = "DELETE FROM EDI_OrderError WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					r = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return r;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByCustomer(int customerId, string searchText,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			var Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError WHERE ClientId=@customerId";
				if(!string.IsNullOrWhiteSpace(searchText))
				{
					query += $" AND ([CustomerNumber] LIKE '%{searchText.SqlEscape()}%' OR [ClientName] LIKE '%{searchText.SqlEscape()}%' OR [Error] LIKE '%{searchText.SqlEscape()}%' OR [FileName] LIKE '%{searchText.SqlEscape()}%' OR [CreationTime] LIKE '%{searchText.SqlEscape()}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Id DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				SqlCommand sqlCommand = new SqlCommand(query, cnn);
				sqlCommand.Parameters.AddWithValue("customerId", customerId);

				using(var reader = DbExecution.ExecuteReader(sqlCommand))
				{
					Lt = toList(reader);
				}
			}

			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByCustomer(int customerId, bool isValidated)
		{
			List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError WHERE Validated=@Validated AND ClientId=@customerId";
				SqlCommand cmd = new SqlCommand(query, cnn);
				cmd.Parameters.AddWithValue("Validated", isValidated);
				cmd.Parameters.AddWithValue("customerId", customerId);

				using(var reader = DbExecution.ExecuteReader(cmd))
				{
					Lt = toList(reader);
				}
			}
			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByCustomer(List<int> customerIds, bool isValidated)
		{
			List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = $"SELECT * FROM EDI_OrderError WHERE Validated=@Validated{(customerIds is not null && customerIds.Count > 0 ? $" AND [ClientId] IN ({string.Join(",", customerIds)})" : "")}";
				SqlCommand cmd = new SqlCommand(query, cnn);
				cmd.Parameters.AddWithValue("Validated", isValidated);

				using(var reader = DbExecution.ExecuteReader(cmd))
				{
					Lt = toList(reader);
				}
			}
			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByIsValidated(int customerId, bool isValidated, string searchText,
			Data.Access.Settings.SortingModel sorting,
			Data.Access.Settings.PaginModel paging)
		{
			List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError WHERE Validated=@Validated AND ClientId=@customerId";
				if(!string.IsNullOrWhiteSpace(searchText))
				{
					query += $" AND ([CustomerNumber] LIKE '%{searchText.SqlEscape()}%' OR [ClientName] LIKE '%{searchText.SqlEscape()}%' OR [Error] LIKE '%{searchText.SqlEscape()}%' OR [FileName] LIKE '%{searchText.SqlEscape()}%' OR [CreationTime] LIKE '%{searchText.SqlEscape()}%')";
				}
				if(sorting != null && !string.IsNullOrWhiteSpace(sorting.SortFieldName))
				{
					query += $" ORDER BY {sorting.SortFieldName} {(sorting.SortDesc ? "DESC" : "ASC")} ";
				}
				else
				{
					query += " ORDER BY Id DESC ";
				}
				if(paging != null)
				{
					query += $" OFFSET {paging.FirstRowNumber} ROWS FETCH NEXT {paging.RequestRows} ROWS ONLY ";
				}
				SqlCommand cmd = new SqlCommand(query, cnn);
				cmd.Parameters.AddWithValue("Validated", isValidated);
				cmd.Parameters.AddWithValue("customerId", customerId);

				using(var reader = DbExecution.ExecuteReader(cmd))
				{
					Lt = toList(reader);
				}
			}
			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByIsValidated(int customerId, bool isValidated)
		{
			List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = "SELECT * FROM EDI_OrderError WHERE Validated=@Validated AND ClientId=@customerId";
				SqlCommand cmd = new SqlCommand(query, cnn);
				cmd.Parameters.AddWithValue("Validated", isValidated);
				cmd.Parameters.AddWithValue("customerId", customerId);

				using(var reader = DbExecution.ExecuteReader(cmd))
				{
					Lt = toList(reader);
				}
			}
			return Lt;
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> GetByIsValidated(List<int> customerIds, bool isValidated)
		{
			List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> Lt = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

			using(var cnn = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString))
			{
				cnn.Open();
				string query = $"SELECT * FROM EDI_OrderError WHERE Validated=@Validated{(customerIds is not null && customerIds.Count>0?$" AND [ClientId] IN ({string.Join(",", customerIds)})":"")}";
				SqlCommand cmd = new SqlCommand(query, cnn);
				cmd.Parameters.AddWithValue("Validated", isValidated);

				using(var reader = DbExecution.ExecuteReader(cmd))
				{
					Lt = toList(reader);
				}
			}
			return Lt;
		}
		public static int GetCountByIsValidated(bool isValidated)
		{
			var dataTable = new System.Data.DataTable();

			using(var sqlConnection = new SqlConnection(Data.Access.Settings.ConnectionString))
			{
				sqlConnection.Open();

				string query = "SELECT COUNT(*) FROM EDI_OrderError WHERE Validated=@Validated";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Validated", isValidated);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count == 0)
			{
				return 0;
			}

			return (int)dataTable.Rows[0][0];
		}
		#endregion

		#region Helpers
		private static List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> toList(SqlDataReader dataReader)
		{
			var result = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();
			while(dataReader.Read())
			{
				result.Add(new Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity(dataReader));
			}
			return result;
		}
		#endregion
	}
}
