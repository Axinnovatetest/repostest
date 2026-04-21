using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.FNC
{

	public class BudgetSupplementCompanyAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetSupplementCompany] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__FNC_BudgetSupplementCompany]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					sqlCommand.CommandText = $"SELECT * FROM [__FNC_BudgetSupplementCompany] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__FNC_BudgetSupplementCompany] ([AmountInitial],[ComapnyName],[CompanyId],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[Year])  VALUES (@AmountInitial,@ComapnyName,@CompanyId,@CreationTime,@CreationUserId,@LastEditTime,@LastEditUserId,@Year); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("AmountInitial", item.AmountInitial);
					sqlCommand.Parameters.AddWithValue("ComapnyName", item.ComapnyName == null ? (object)DBNull.Value : item.ComapnyName);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("Year", item.Year);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__FNC_BudgetSupplementCompany] ([AmountInitial],[ComapnyName],[CompanyId],[CreationTime],[CreationUserId],[LastEditTime],[LastEditUserId],[Year]) VALUES ( "

							+ "@AmountInitial" + i + ","
							+ "@ComapnyName" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@Year" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("AmountInitial" + i, item.AmountInitial);
						sqlCommand.Parameters.AddWithValue("ComapnyName" + i, item.ComapnyName == null ? (object)DBNull.Value : item.ComapnyName);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "UPDATE [__FNC_BudgetSupplementCompany] SET [AmountInitial]=@AmountInitial, [ComapnyName]=@ComapnyName, [CompanyId]=@CompanyId, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [Year]=@Year WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("AmountInitial", item.AmountInitial);
				sqlCommand.Parameters.AddWithValue("ComapnyName", item.ComapnyName == null ? (object)DBNull.Value : item.ComapnyName);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 9; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__FNC_BudgetSupplementCompany] SET "

							+ "[AmountInitial]=@AmountInitial" + i + ","
							+ "[ComapnyName]=@ComapnyName" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[Year]=@Year" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("AmountInitial" + i, item.AmountInitial);
						sqlCommand.Parameters.AddWithValue("ComapnyName" + i, item.ComapnyName == null ? (object)DBNull.Value : item.ComapnyName);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__FNC_BudgetSupplementCompany] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
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

					string query = "DELETE FROM [__FNC_BudgetSupplementCompany] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity> GetByCompaniesAndYear(List<int> companyIds, int year)
		{
			if(companyIds == null || companyIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__FNC_BudgetSupplementCompany] WHERE CompanyId IN ({string.Join(",", companyIds)}) AND Year=@year";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BudgetSupplementCompanyEntity>();
			}
		}
		public static int DeleteByCompaniesAndYear(List<int> companyIds, int year)
		{
			if(companyIds == null || companyIds.Count <= 0)
				return -1;

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString_FNC))
			{
				sqlConnection.Open();
				var sqlCommand = new SqlCommand();
				string query = $"DELETE FROM [__FNC_BudgetSupplementCompany] WHERE [Id] IN ({string.Join(",", companyIds)}) AND Year = @year";
				sqlCommand.CommandText = query;
				sqlCommand.Parameters.AddWithValue("year", year);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		#endregion
	}
}
