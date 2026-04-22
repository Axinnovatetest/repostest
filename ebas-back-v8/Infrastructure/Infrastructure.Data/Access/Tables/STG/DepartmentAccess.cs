using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.STG
{

	public class DepartmentAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.STG.DepartmentEntity Get(long id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE [Id]=@Id AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> Get(bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> Get(List<long> ids, bool isArchived = false, bool isDeleted = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived, isDeleted);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived, isDeleted));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived, isDeleted));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> get(List<long> ids, bool isArchived = false, bool isDeleted = false)
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

					sqlCommand.CommandText = $"SELECT * FROM [__STG_Department] WHERE [Id] IN ({queryIds}) AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
					sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
					sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
		}
		public static long Insert(Infrastructure.Data.Entities.Tables.STG.DepartmentEntity item)
		{
			long response = long.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__STG_Department] ([ArchiveTime],[ArchiveUserId],[CompanyId],[CompanyName],[CreationTime],[CreationUserId],[DeleteTime],[DeleteUserId],[Description],[HeadUserEmail],[HeadUserId],[HeadUserName],[IsArchived],[IsDeleted],[IsFNC],[IsSTG],[IsWPL],[LastEditTime],[LastEditUserId],[Name])  VALUES (@ArchiveTime,@ArchiveUserId,@CompanyId,@CompanyName,@CreationTime,@CreationUserId,@DeleteTime,@DeleteUserId,@Description,@HeadUserEmail,@HeadUserId,@HeadUserName,@IsArchived,@IsDeleted,@IsFNC,@IsSTG,@IsWPL,@LastEditTime,@LastEditUserId,@Name); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
					sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
					sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
					sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
					sqlCommand.Parameters.AddWithValue("HeadUserEmail", item.HeadUserEmail == null ? (object)DBNull.Value : item.HeadUserEmail);
					sqlCommand.Parameters.AddWithValue("HeadUserId", item.HeadUserId);
					sqlCommand.Parameters.AddWithValue("HeadUserName", item.HeadUserName);
					sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived == null ? (object)DBNull.Value : item.IsArchived);
					sqlCommand.Parameters.AddWithValue("IsDeleted", item.IsDeleted == null ? (object)DBNull.Value : item.IsDeleted);
					sqlCommand.Parameters.AddWithValue("IsFNC", item.IsFNC == null ? (object)DBNull.Value : item.IsFNC);
					sqlCommand.Parameters.AddWithValue("IsSTG", item.IsSTG == null ? (object)DBNull.Value : item.IsSTG);
					sqlCommand.Parameters.AddWithValue("IsWPL", item.IsWPL == null ? (object)DBNull.Value : item.IsWPL);
					sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
					sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
					sqlCommand.Parameters.AddWithValue("Name", item.Name);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? long.MinValue : long.TryParse(result.ToString(), out var insertedId) ? insertedId : long.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> items)
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
						query += " INSERT INTO [__STG_Department] ([ArchiveTime],[ArchiveUserId],[CompanyId],[CompanyName],[CreationTime],[CreationUserId],[DeleteTime],[DeleteUserId],[Description],[HeadUserEmail],[HeadUserId],[HeadUserName],[IsArchived],[IsDeleted],[IsFNC],[IsSTG],[IsWPL],[LastEditTime],[LastEditUserId],[Name]) VALUES ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@CompanyId" + i + ","
							+ "@CompanyName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@DeleteTime" + i + ","
							+ "@DeleteUserId" + i + ","
							+ "@Description" + i + ","
							+ "@HeadUserEmail" + i + ","
							+ "@HeadUserId" + i + ","
							+ "@HeadUserName" + i + ","
							+ "@IsArchived" + i + ","
							+ "@IsDeleted" + i + ","
							+ "@IsFNC" + i + ","
							+ "@IsSTG" + i + ","
							+ "@IsWPL" + i + ","
							+ "@LastEditTime" + i + ","
							+ "@LastEditUserId" + i + ","
							+ "@Name" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("HeadUserEmail" + i, item.HeadUserEmail == null ? (object)DBNull.Value : item.HeadUserEmail);
						sqlCommand.Parameters.AddWithValue("HeadUserId" + i, item.HeadUserId);
						sqlCommand.Parameters.AddWithValue("HeadUserName" + i, item.HeadUserName);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived == null ? (object)DBNull.Value : item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsDeleted" + i, item.IsDeleted == null ? (object)DBNull.Value : item.IsDeleted);
						sqlCommand.Parameters.AddWithValue("IsFNC" + i, item.IsFNC == null ? (object)DBNull.Value : item.IsFNC);
						sqlCommand.Parameters.AddWithValue("IsSTG" + i, item.IsSTG == null ? (object)DBNull.Value : item.IsSTG);
						sqlCommand.Parameters.AddWithValue("IsWPL" + i, item.IsWPL == null ? (object)DBNull.Value : item.IsWPL);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Update(Infrastructure.Data.Entities.Tables.STG.DepartmentEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "UPDATE [__STG_Department] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [CompanyId]=@CompanyId, [CompanyName]=@CompanyName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DeleteTime]=@DeleteTime, [DeleteUserId]=@DeleteUserId, [Description]=@Description, [HeadUserEmail]=@HeadUserEmail, [HeadUserId]=@HeadUserId, [HeadUserName]=@HeadUserName, [IsArchived]=@IsArchived, [IsDeleted]=@IsDeleted, [IsFNC]=@IsFNC, [IsSTG]=@IsSTG, [IsWPL]=@IsWPL, [LastEditTime]=@LastEditTime, [LastEditUserId]=@LastEditUserId, [Name]=@Name WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("CompanyId", item.CompanyId);
				sqlCommand.Parameters.AddWithValue("CompanyName", item.CompanyName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("DeleteTime", item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
				sqlCommand.Parameters.AddWithValue("DeleteUserId", item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
				sqlCommand.Parameters.AddWithValue("Description", item.Description == null ? (object)DBNull.Value : item.Description);
				sqlCommand.Parameters.AddWithValue("HeadUserEmail", item.HeadUserEmail == null ? (object)DBNull.Value : item.HeadUserEmail);
				sqlCommand.Parameters.AddWithValue("HeadUserId", item.HeadUserId);
				sqlCommand.Parameters.AddWithValue("HeadUserName", item.HeadUserName);
				sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived == null ? (object)DBNull.Value : item.IsArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", item.IsDeleted == null ? (object)DBNull.Value : item.IsDeleted);
				sqlCommand.Parameters.AddWithValue("IsFNC", item.IsFNC == null ? (object)DBNull.Value : item.IsFNC);
				sqlCommand.Parameters.AddWithValue("IsSTG", item.IsSTG == null ? (object)DBNull.Value : item.IsSTG);
				sqlCommand.Parameters.AddWithValue("IsWPL", item.IsWPL == null ? (object)DBNull.Value : item.IsWPL);
				sqlCommand.Parameters.AddWithValue("LastEditTime", item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
				sqlCommand.Parameters.AddWithValue("LastEditUserId", item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
				sqlCommand.Parameters.AddWithValue("Name", item.Name);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 21; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> items)
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
						query += " UPDATE [__STG_Department] SET "

							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[CompanyId]=@CompanyId" + i + ","
							+ "[CompanyName]=@CompanyName" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[DeleteTime]=@DeleteTime" + i + ","
							+ "[DeleteUserId]=@DeleteUserId" + i + ","
							+ "[Description]=@Description" + i + ","
							+ "[HeadUserEmail]=@HeadUserEmail" + i + ","
							+ "[HeadUserId]=@HeadUserId" + i + ","
							+ "[HeadUserName]=@HeadUserName" + i + ","
							+ "[IsArchived]=@IsArchived" + i + ","
							+ "[IsDeleted]=@IsDeleted" + i + ","
							+ "[IsFNC]=@IsFNC" + i + ","
							+ "[IsSTG]=@IsSTG" + i + ","
							+ "[IsWPL]=@IsWPL" + i + ","
							+ "[LastEditTime]=@LastEditTime" + i + ","
							+ "[LastEditUserId]=@LastEditUserId" + i + ","
							+ "[Name]=@Name" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("CompanyId" + i, item.CompanyId);
						sqlCommand.Parameters.AddWithValue("CompanyName" + i, item.CompanyName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DeleteTime" + i, item.DeleteTime == null ? (object)DBNull.Value : item.DeleteTime);
						sqlCommand.Parameters.AddWithValue("DeleteUserId" + i, item.DeleteUserId == null ? (object)DBNull.Value : item.DeleteUserId);
						sqlCommand.Parameters.AddWithValue("Description" + i, item.Description == null ? (object)DBNull.Value : item.Description);
						sqlCommand.Parameters.AddWithValue("HeadUserEmail" + i, item.HeadUserEmail == null ? (object)DBNull.Value : item.HeadUserEmail);
						sqlCommand.Parameters.AddWithValue("HeadUserId" + i, item.HeadUserId);
						sqlCommand.Parameters.AddWithValue("HeadUserName" + i, item.HeadUserName);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived == null ? (object)DBNull.Value : item.IsArchived);
						sqlCommand.Parameters.AddWithValue("IsDeleted" + i, item.IsDeleted == null ? (object)DBNull.Value : item.IsDeleted);
						sqlCommand.Parameters.AddWithValue("IsFNC" + i, item.IsFNC == null ? (object)DBNull.Value : item.IsFNC);
						sqlCommand.Parameters.AddWithValue("IsSTG" + i, item.IsSTG == null ? (object)DBNull.Value : item.IsSTG);
						sqlCommand.Parameters.AddWithValue("IsWPL" + i, item.IsWPL == null ? (object)DBNull.Value : item.IsWPL);
						sqlCommand.Parameters.AddWithValue("LastEditTime" + i, item.LastEditTime == null ? (object)DBNull.Value : item.LastEditTime);
						sqlCommand.Parameters.AddWithValue("LastEditUserId" + i, item.LastEditUserId == null ? (object)DBNull.Value : item.LastEditUserId);
						sqlCommand.Parameters.AddWithValue("Name" + i, item.Name);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}
		public static int Delete(long id)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__STG_Department] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Delete(List<long> ids)
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
		private static int delete(List<long> ids)
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

					string query = "DELETE FROM [__STG_Department] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion
		#region Custom Methods
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> GetByName(string name, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE [Name] LIKE @name AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name ?? "");
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> FilterByName(string name, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE [Name] LIKE %@name% AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("name", name ?? "");
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> GetByCompany(long companyId, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE [CompanyId]=@companyId AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("companyId", companyId);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> GetByCompanies(List<long> companyIds, bool isArchived = false, bool isDeleted = false)
		{
			if(companyIds == null || companyIds.Count <= 0)
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();

			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM [__STG_Department] WHERE [CompanyId] IN ({string.Join(",", companyIds)}) AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> GetByDirectorId(int id, bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE HeadUserId=@id AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity> GetAllowedLeasing(bool isArchived = false, bool isDeleted = false)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__STG_Department] WHERE ([Name]='IT' OR Ltrim(Rtrim(CompanyName))='psz electronic gmbh') AND ([IsArchived] IS NULL OR [IsArchived]=@IsArchived) AND ([IsDeleted] IS NULL OR [IsDeleted]=@IsDeleted)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("IsArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("IsDeleted", isDeleted);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.STG.DepartmentEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();
			}
		}
		#endregion
	}
}