using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityPlanValidationLogAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationLog]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_CapacityPlan_ValidationLog] ([CountryId],[CountryName],[HallId],[HallName],[ValidationLevel],[ValidationReason],[ValidationStatus],[ValidationStatusName],[ValidationTime],[ValidationUserId],[Year])  VALUES (@CountryId,@CountryName,@HallId,@HallName,@ValidationLevel,@ValidationReason,@ValidationStatus,@ValidationStatusName,@ValidationTime,@ValidationUserId,@Year); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
					sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
					sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
					sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
					sqlCommand.Parameters.AddWithValue("ValidationReason", item.ValidationReason == null ? (object)DBNull.Value : item.ValidationReason);
					sqlCommand.Parameters.AddWithValue("ValidationStatus", item.ValidationStatus == null ? (object)DBNull.Value : item.ValidationStatus);
					sqlCommand.Parameters.AddWithValue("ValidationStatusName", item.ValidationStatusName == null ? (object)DBNull.Value : item.ValidationStatusName);
					sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
					sqlCommand.Parameters.AddWithValue("Year", item.Year);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> items)
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> items)
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
						query += " INSERT INTO [__MTM_CRP_CapacityPlan_ValidationLog] ([CountryId],[CountryName],[HallId],[HallName],[ValidationLevel],[ValidationReason],[ValidationStatus],[ValidationStatusName],[ValidationTime],[ValidationUserId],[Year]) VALUES ( "

							+ "@CountryId" + i + ","
							+ "@CountryName" + i + ","
							+ "@HallId" + i + ","
							+ "@HallName" + i + ","
							+ "@ValidationLevel" + i + ","
							+ "@ValidationReason" + i + ","
							+ "@ValidationStatus" + i + ","
							+ "@ValidationStatusName" + i + ","
							+ "@ValidationTime" + i + ","
							+ "@ValidationUserId" + i + ","
							+ "@Year" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationReason" + i, item.ValidationReason == null ? (object)DBNull.Value : item.ValidationReason);
						sqlCommand.Parameters.AddWithValue("ValidationStatus" + i, item.ValidationStatus == null ? (object)DBNull.Value : item.ValidationStatus);
						sqlCommand.Parameters.AddWithValue("ValidationStatusName" + i, item.ValidationStatusName == null ? (object)DBNull.Value : item.ValidationStatusName);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_CapacityPlan_ValidationLog] SET [CountryId]=@CountryId, [CountryName]=@CountryName, [HallId]=@HallId, [HallName]=@HallName, [ValidationLevel]=@ValidationLevel, [ValidationReason]=@ValidationReason, [ValidationStatus]=@ValidationStatus, [ValidationStatusName]=@ValidationStatusName, [ValidationTime]=@ValidationTime, [ValidationUserId]=@ValidationUserId, [Year]=@Year WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
				sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
				sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
				sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
				sqlCommand.Parameters.AddWithValue("ValidationReason", item.ValidationReason == null ? (object)DBNull.Value : item.ValidationReason);
				sqlCommand.Parameters.AddWithValue("ValidationStatus", item.ValidationStatus == null ? (object)DBNull.Value : item.ValidationStatus);
				sqlCommand.Parameters.AddWithValue("ValidationStatusName", item.ValidationStatusName == null ? (object)DBNull.Value : item.ValidationStatusName);
				sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
				sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> items)
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> items)
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
						query += " UPDATE [__MTM_CRP_CapacityPlan_ValidationLog] SET "

							+ "[CountryId]=@CountryId" + i + ","
							+ "[CountryName]=@CountryName" + i + ","
							+ "[HallId]=@HallId" + i + ","
							+ "[HallName]=@HallName" + i + ","
							+ "[ValidationLevel]=@ValidationLevel" + i + ","
							+ "[ValidationReason]=@ValidationReason" + i + ","
							+ "[ValidationStatus]=@ValidationStatus" + i + ","
							+ "[ValidationStatusName]=@ValidationStatusName" + i + ","
							+ "[ValidationTime]=@ValidationTime" + i + ","
							+ "[ValidationUserId]=@ValidationUserId" + i + ","
							+ "[Year]=@Year" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationReason" + i, item.ValidationReason == null ? (object)DBNull.Value : item.ValidationReason);
						sqlCommand.Parameters.AddWithValue("ValidationStatus" + i, item.ValidationStatus == null ? (object)DBNull.Value : item.ValidationStatus);
						sqlCommand.Parameters.AddWithValue("ValidationStatusName" + i, item.ValidationStatusName == null ? (object)DBNull.Value : item.ValidationStatusName);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
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
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity GetLastByYearCountryHall(int year, int countryId, int hallId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Id]=" +
					"(SELECT MAX(Id) FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId)";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity> GetByYearCountryHall(int year, int countryId, int hallId)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_ValidationLog] WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationLogEntity>();
			}
		}
		#endregion
	}
}
