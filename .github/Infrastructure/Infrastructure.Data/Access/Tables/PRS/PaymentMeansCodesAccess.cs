using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Infrastructure.Data.Entities.Tables;
using Infrastructure.Data.Entities.Tables.PRS;

namespace Infrastructure.Data.Access.Tables.PRS
{
	public static class PaymentMeansCodesAccess
	{
		#region Default Methods
		public static Entities.Tables.PRS.PaymentMeansCodesEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PaymentMeansCodes] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("@Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.PRS.PaymentMeansCodesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PaymentMeansCodes]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.PaymentMeansCodesEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
			}
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [PaymentMeansCodes] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.PaymentMeansCodesEntity(x)).ToList();
				}
				else
				{
					return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
				}
			}
			return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.PRS.PaymentMeansCodesEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
		}
		public static int Insert(Entities.Tables.PRS.PaymentMeansCodesEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [PaymentMeansCodes] ([Code],[DescriptionEnglish]) OUTPUT INSERTED.[Id] VALUES (@Code,@DescriptionEnglish); SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Code", item.Code);
					sqlCommand.Parameters.AddWithValue("DescriptionEnglish", item.DescriptionEnglish);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int insert(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items)
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
						query += " INSERT INTO [PaymentMeansCodes] ([Code],[DescriptionEnglish]) VALUES ("
							+ "@Code" + i +
							 ","
							+ "@DescriptionEnglish" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
						sqlCommand.Parameters.AddWithValue("DescriptionEnglish" + i, item.DescriptionEnglish);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int Insert(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 3;
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
		public static int Update(Entities.Tables.PRS.PaymentMeansCodesEntity item)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "UPDATE [PaymentMeansCodes] SET [Code] = @Code, [DescriptionEnglish] = @DescriptionEnglish WHERE [Id] = @Id";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("Id", item.Id);
					sqlCommand.Parameters.AddWithValue("Code", item.Code);
					sqlCommand.Parameters.AddWithValue("DescriptionEnglish", item.DescriptionEnglish);

					int rowsAffected = DbExecution.ExecuteNonQuery(sqlCommand);
					sqlTransaction.Commit();

					return rowsAffected;
				}
			}
		}
		public static int update(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items)
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
						query += " UPDATE [PaymentMeansCodes] SET "
						  + "[Code]=@Code" + i +
						   ","
						  + "[DescriptionEnglish]=@DescriptionEnglish" + i +
						 " WHERE [Id]=@Id" + i
							+ "; ";
						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
						sqlCommand.Parameters.AddWithValue("DescriptionEnglish" + i, item.DescriptionEnglish);
						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 3;
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

				string query = "DELETE FROM [PaymentMeansCodes] WHERE [Id] = @Id";

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
				string query = "DELETE FROM [PaymentMeansCodes] WHERE [Id] IN (" + queryIds + ")";

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
		public static Entities.Tables.PRS.PaymentMeansCodesEntity GetWithTransaction(int id, SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PaymentMeansCodes] WHERE [Id]=@Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", id);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return new Entities.Tables.PRS.PaymentMeansCodesEntity(dataTable.Rows[0]);
			else
				return null;
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> GetWithTransaction(SqlConnection connection, SqlTransaction transaction)
		{
			var dataTable = new DataTable();

			string query = "SELECT * FROM [PaymentMeansCodes]";
			var sqlCommand = new SqlCommand(query, connection, transaction);

			DbExecution.Fill(sqlCommand, dataTable);

			if(dataTable.Rows.Count > 0)
				return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.PaymentMeansCodesEntity(x)).ToList();
			else
				return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> getWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
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

				sqlCommand.CommandText = "SELECT * FROM [PaymentMeansCodes] WHERE [Id] IN (" + queryIds + ")";
				DbExecution.Fill(sqlCommand, dataTable);

				if(dataTable.Rows.Count > 0)
					return dataTable.Rows.Cast<DataRow>().Select(x => new Entities.Tables.PRS.PaymentMeansCodesEntity(x)).ToList();
				else
					return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
			}
			return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
		}
		public static List<Entities.Tables.PRS.PaymentMeansCodesEntity> GetWithTransaction(List<int> ids, SqlConnection connection, SqlTransaction transaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Entities.Tables.PRS.PaymentMeansCodesEntity> results = null;

				if(ids.Count <= maxQueryNumber)
				{
					results = getWithTransaction(ids, connection, transaction);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();

					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(getWithTransaction(ids.GetRange(i * maxQueryNumber, maxQueryNumber), connection, transaction));
					}

					results.AddRange(getWithTransaction(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), connection, transaction));
				}

				return results;
			}
			return new List<Entities.Tables.PRS.PaymentMeansCodesEntity>();
		}
		public static int InsertWithTransaction(Entities.Tables.PRS.PaymentMeansCodesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int response = int.MinValue;
			string query = "INSERT INTO [PaymentMeansCodes] ([Code],[DescriptionEnglish]) OUTPUT INSERTED.[Id] VALUES (@Code,@DescriptionEnglish); SELECT SCOPE_IDENTITY();";
			using(var sqlCommand = new SqlCommand(query, connection, transaction))
			{
				sqlCommand.Parameters.AddWithValue("Code", item.Code);
				sqlCommand.Parameters.AddWithValue("DescriptionEnglish", item.DescriptionEnglish);

				var result = DbExecution.ExecuteScalar(sqlCommand);
				response = result == null || result == DBNull.Value ? int.MinValue : Convert.ToInt32(result);
			}

			return response;
		}
		public static int insertWithTransaction(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
						query += " INSERT INTO [PaymentMeansCodes] ([Code],[DescriptionEnglish]) VALUES ("
							+ "@Code" + i +
							 ","
							+ "@DescriptionEnglish" + i +
							 "); ";
						sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
						sqlCommand.Parameters.AddWithValue("DescriptionEnglish" + i, item.DescriptionEnglish);
					}
					sqlCommand.CommandText = query;
					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}
				return results;
			}
			return -1;
		}
		public static int InsertWithTransaction(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
		public static int UpdateWithTransaction(Entities.Tables.PRS.PaymentMeansCodesEntity item, SqlConnection connection, SqlTransaction transaction)
		{
			int results = -1;
			string query = "UPDATE [PaymentMeansCodes] SET [Code] = @Code, [DescriptionEnglish] = @DescriptionEnglish WHERE [Id] = @Id";
			var sqlCommand = new SqlCommand(query, connection, transaction);
			sqlCommand.Parameters.AddWithValue("Id", item.Id);
			sqlCommand.Parameters.AddWithValue("Code", item.Code);
			sqlCommand.Parameters.AddWithValue("DescriptionEnglish", item.DescriptionEnglish);
			results = DbExecution.ExecuteNonQuery(sqlCommand);
			return results;
		}
		public static int updateWithTransaction(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
					query += " UPDATE [PaymentMeansCodes] SET "
					  + "[Code]=@Code" + i +
					   ","
					  + "[DescriptionEnglish]=@DescriptionEnglish" + i +
					 " WHERE [Id]=@Id" + i
						+ "; ";
					sqlCommand.Parameters.AddWithValue("Code" + i, item.Code);
					sqlCommand.Parameters.AddWithValue("DescriptionEnglish" + i, item.DescriptionEnglish);
					sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
				}
				sqlCommand.CommandText = query;
				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
			return -1;
		}
		public static int UpdateWithTransaction(List<Entities.Tables.PRS.PaymentMeansCodesEntity> items, SqlConnection connection, SqlTransaction transaction)
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
			string query = "DELETE FROM [PaymentMeansCodes] WHERE [Id] = @Id";
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
				sqlCommand.CommandText = $"DELETE FROM [PaymentMeansCodes] WHERE [Id] IN (" + queryIds + ")";
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

		public static Entities.Tables.PRS.PaymentMeansCodesEntity GetPaymentMeansCodeValue(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [PaymentMeansCodes] WHERE [Id]=@Id";
				using (var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@Id", id);
					DbExecution.Fill(sqlCommand, dataTable);
				}
			}
			if(dataTable.Rows.Count > 0)
			{
				return new PaymentMeansCodesEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		#endregion Custom Methods
	}
}
