using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityPlanValidationUserAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationUser]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [Id] IN ({queryIds})";
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_CapacityPlan_ValidationUser] ([CountryId],[CountryName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName],[ValidationLevel])  VALUES (@CountryId,@CountryName,@CreationTime,@CreationUserId,@UserEmail,@UserId,@UserName,@ValidationLevel); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
					sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
					sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
					sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);

					var result = DbExecution.ExecuteScalar(sqlCommand);
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__MTM_CRP_CapacityPlan_ValidationUser] ([CountryId],[CountryName],[CreationTime],[CreationUserId],[UserEmail],[UserId],[UserName],[ValidationLevel]) VALUES ( "

							+ "@CountryId" + i + ","
							+ "@CountryName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@UserEmail" + i + ","
							+ "@UserId" + i + ","
							+ "@UserName" + i + ","
							+ "@ValidationLevel" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_CapacityPlan_ValidationUser] SET [CountryId]=@CountryId, [CountryName]=@CountryName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [UserEmail]=@UserEmail, [UserId]=@UserId, [UserName]=@UserName, [ValidationLevel]=@ValidationLevel WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName == null ? (object)DBNull.Value : item.CountryName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("UserEmail", item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
				sqlCommand.Parameters.AddWithValue("UserId", item.UserId);
				sqlCommand.Parameters.AddWithValue("UserName", item.UserName == null ? (object)DBNull.Value : item.UserName);
				sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " UPDATE [__MTM_CRP_CapacityPlan_ValidationUser] SET "

							+ "[CountryId]=@CountryId" + i + ","
							+ "[CountryName]=@CountryName" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[UserEmail]=@UserEmail" + i + ","
							+ "[UserId]=@UserId" + i + ","
							+ "[UserName]=@UserName" + i + ","
							+ "[ValidationLevel]=@ValidationLevel" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName == null ? (object)DBNull.Value : item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime == null ? (object)DBNull.Value : item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId == null ? (object)DBNull.Value : item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("UserEmail" + i, item.UserEmail == null ? (object)DBNull.Value : item.UserEmail);
						sqlCommand.Parameters.AddWithValue("UserId" + i, item.UserId);
						sqlCommand.Parameters.AddWithValue("UserName" + i, item.UserName == null ? (object)DBNull.Value : item.UserName);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [Id]=@Id";
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
				using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods

		public static int DeleteByCountryUserLevel(int countryId, int userId, int level)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [CountryId]=@countryId AND [UserId]=@userId AND [ValidationLevel]=@level";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("userId", userId);
				sqlCommand.Parameters.AddWithValue("level", level);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity> GetByCountryUserLevel(int countryId, int level)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationUser] WHERE [CountryId]=@countryId AND [ValidationLevel]=@level";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("level", level);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationUserEntity>();
			}
		}
		#endregion
	}
}
