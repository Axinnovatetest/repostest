using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityPlanAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.CapacityPlanEntity Get(int id,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan] WHERE [Id]=@Id "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.CapacityPlanEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get(bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var query = "SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ (isArchived.HasValue ? $" WHERE [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}
		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get(List<int> ids,
			bool? isArchived = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				var results = new List<Entities.Tables.MTM.CapacityPlanEntity>();
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids, isArchived);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Entities.Tables.MTM.CapacityPlanEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived));
				}
				return results;
			}
			return new List<Entities.Tables.MTM.CapacityPlanEntity>();
		}
		private static List<Entities.Tables.MTM.CapacityPlanEntity> get(List<int> ids,
			bool? isArchived)
		{
			if(ids != null && ids.Count > 0)
			{
				var dataTable = new DataTable();

				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
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

					sqlCommand.CommandText = "SELECT * FROM [__MTM_CRP_CapacityPlan] WHERE [Id] IN (" + queryIds + ") "
						+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.MTM.CapacityPlanEntity>();
				}
			}
			return new List<Entities.Tables.MTM.CapacityPlanEntity>();
		}

		public static int Insert(Entities.Tables.MTM.CapacityPlanEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_CapacityPlan] "
					+ " ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly],[Version],[WeekNumber],[WorkAreaId],[WorkAreaName],[Year],[WorkStationId],[WorkStationName],[WeekFirstDay],[WeekLastDay],[HrHardResourcesNumber],[AvailableHrDaily],[WorkingHoursPerShift]) "
					+ " VALUES "
					+ " (@ArchiveTime,@ArchiveUserId,@Attendance,@AvailableSrDaily,@CountryId,@CountryName,@CreationTime,@CreationUserId,@DepartementId,@DepartementName,@Factor1HrDaily,@Factor2HrDaily,@Factor3SrDaily,@FormToolInsert,@HallId,@HallName,@IsArchived,@LastUpdateTime,@LastUpdateUserId,@OperationId,@OperationName,@PlanCapacity,@ProductivityHrDaily,@ProductivitySrDaily,@RequiredEmployees,@ShiftsNumberWeekly,@SoftRessourcesNumberDaily,@SpecialHoursWeekly,@SpecialShiftsWeekly,@Version,@WeekNumber,@WorkAreaId,@WorkAreaName,@Year,@WorkStationId,@WorkStationName,@WeekFirstDay,@WeekLastDay,@HrHardResourcesNumber,@AvailableHrDaily,@WorkingHoursPerShift)";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{
					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Attendance", item.Attendance);
					sqlCommand.Parameters.AddWithValue("AvailableSrDaily", item.AvailableSrDaily);
					sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
					sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
					sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
					sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
					sqlCommand.Parameters.AddWithValue("DepartementId", item.DepartementId);
					sqlCommand.Parameters.AddWithValue("DepartementName", item.DepartementName);
					sqlCommand.Parameters.AddWithValue("Factor1HrDaily", item.Factor1HrDaily);
					sqlCommand.Parameters.AddWithValue("Factor2HrDaily", item.Factor2HrDaily);
					sqlCommand.Parameters.AddWithValue("Factor3SrDaily", item.Factor3SrDaily);
					sqlCommand.Parameters.AddWithValue("FormToolInsert", item.FormToolInsert == null ? (object)DBNull.Value : item.FormToolInsert);
					sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
					sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
					sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("OperationId", item.OperationId);
					sqlCommand.Parameters.AddWithValue("OperationName", item.OperationName);
					sqlCommand.Parameters.AddWithValue("PlanCapacity", item.PlanCapacity);
					sqlCommand.Parameters.AddWithValue("ProductivityHrDaily", item.ProductivityHrDaily);
					sqlCommand.Parameters.AddWithValue("ProductivitySrDaily", item.ProductivitySrDaily);
					sqlCommand.Parameters.AddWithValue("RequiredEmployees", item.RequiredEmployees);
					sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly", item.ShiftsNumberWeekly);
					sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily", item.SoftRessourcesNumberDaily);
					sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", item.SpecialHoursWeekly);
					sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", item.SpecialShiftsWeekly);
					sqlCommand.Parameters.AddWithValue("Version", item.Version);
					sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
					sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
					sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
					sqlCommand.Parameters.AddWithValue("Year", item.Year);

					sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
					sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
					sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
					sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);

					sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", item.HrHardResourcesNumber);
					sqlCommand.Parameters.AddWithValue("AvailableHrDaily", item.AvailableHrDaily);
					sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", item.WorkingHoursPerShift);

					DbExecution.ExecuteNonQuery(sqlCommand);
				}

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [__MTM_CRP_CapacityPlan] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}

		public static int Update(Entities.Tables.MTM.CapacityPlanEntity item)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Attendance]=@Attendance, "
					+ " [AvailableSrDaily]=@AvailableSrDaily, [CountryId]=@CountryId, [CountryName]=@CountryName, "
					+ " [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DepartementId]=@DepartementId, "
					+ " [DepartementName]=@DepartementName, [Factor1HrDaily]=@Factor1HrDaily, "
					+ " [Factor2HrDaily]=@Factor2HrDaily, [Factor3SrDaily]=@Factor3SrDaily, "
					+ " [FormToolInsert]=@FormToolInsert, [HallId]=@HallId, [HallName]=@HallName, [IsArchived]=@IsArchived, "
					+ " [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OperationId]=@OperationId, "
					+ " [OperationName]=@OperationName, [PlanCapacity]=@PlanCapacity, "
					+ " [ProductivityHrDaily]=@ProductivityHrDaily, [ProductivitySrDaily]=@ProductivitySrDaily, "
					+ " [RequiredEmployees]=@RequiredEmployees, [ShiftsNumberWeekly]=@ShiftsNumberWeekly, "
					+ " [SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily, [SpecialHoursWeekly]=@SpecialHoursWeekly, "
					+ " [SpecialShiftsWeekly]=@SpecialShiftsWeekly, [Version]=@Version, [WeekNumber]=@WeekNumber, "
					+ " [WorkAreaId]=@WorkAreaId, [WorkAreaName]=@WorkAreaName, [Year]=@Year, "
					+ " [WorkStationId]=@WorkStationId, [WorkStationName]=@WorkStationName, "
					+ " [WeekFirstDay]=@WeekFirstDay, [WeekLastDay]=@WeekLastDay, "
					+ " [HrHardResourcesNumber]=@HrHardResourcesNumber, [AvailableHrDaily]=@AvailableHrDaily, [WorkingHoursPerShift]=@WorkingHoursPerShift "
					+ " WHERE [Id]=@Id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Attendance", item.Attendance);
				sqlCommand.Parameters.AddWithValue("AvailableSrDaily", item.AvailableSrDaily);
				sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
				sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
				sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
				sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
				sqlCommand.Parameters.AddWithValue("DepartementId", item.DepartementId);
				sqlCommand.Parameters.AddWithValue("DepartementName", item.DepartementName);
				sqlCommand.Parameters.AddWithValue("Factor1HrDaily", item.Factor1HrDaily);
				sqlCommand.Parameters.AddWithValue("Factor2HrDaily", item.Factor2HrDaily);
				sqlCommand.Parameters.AddWithValue("Factor3SrDaily", item.Factor3SrDaily);
				sqlCommand.Parameters.AddWithValue("FormToolInsert", item.FormToolInsert == null ? (object)DBNull.Value : item.FormToolInsert);
				sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
				sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
				sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
				sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
				sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
				sqlCommand.Parameters.AddWithValue("OperationId", item.OperationId);
				sqlCommand.Parameters.AddWithValue("OperationName", item.OperationName);
				sqlCommand.Parameters.AddWithValue("PlanCapacity", item.PlanCapacity);
				sqlCommand.Parameters.AddWithValue("ProductivityHrDaily", item.ProductivityHrDaily);
				sqlCommand.Parameters.AddWithValue("ProductivitySrDaily", item.ProductivitySrDaily);
				sqlCommand.Parameters.AddWithValue("RequiredEmployees", item.RequiredEmployees);
				sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly", item.ShiftsNumberWeekly);
				sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily", item.SoftRessourcesNumberDaily);
				sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", item.SpecialHoursWeekly);
				sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", item.SpecialShiftsWeekly);
				sqlCommand.Parameters.AddWithValue("Version", item.Version);
				sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
				sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
				sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
				sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
				sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
				sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);

				sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", item.HrHardResourcesNumber);
				sqlCommand.Parameters.AddWithValue("AvailableHrDaily", item.AvailableHrDaily);
				sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", item.WorkingHoursPerShift);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static int Delete(int id)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [__MTM_CRP_CapacityPlan] WHERE [Id]=@Id";

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
				int maxParamsNumber = Access.Settings.MAX_BATCH_SIZE;
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
				using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
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

					string query = "DELETE FROM [__MTM_CRP_CapacityPlan] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.MTM.CapacityPlanEntity> GetBy_CountryId_HallId_Year(int countryId,
			int? hallId,
			int year,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = $" SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND [Year]={year} "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}

		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get_BY_Country_Hall_Week(
			int countryId,
			int? hallId,
			int weekNumber,
			int year,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND [WeekNumber] = @weekNumber "
					+ $" AND [Year] = @year "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("weekNumber", weekNumber);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}
		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get_BY_Country_Hall_Week_Operation_Department_WorkArea(
			int countryId,
			int? hallId,
			int year,
			int weekNumber,
			int operationId,
			int departmentId,
			int workAreaId,
			int? workStationId,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND [WeekNumber] = @weekNumber "
					+ $" AND [Year] = @year "
					+ $" AND [OperationId] = @operationId "
					+ $" AND [DepartementId] = @departmentId "
					+ $" AND [WorkAreaId] = @workAreaId "
					+ $"{(workStationId.HasValue ? $" AND [WorkStationId] = {workStationId.Value} " : "")} "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("weekNumber", weekNumber);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("operationId", operationId);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				sqlCommand.Parameters.AddWithValue("workAreaId", workAreaId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}
		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get_BY_Country_Hall_Week_Operation_Department_WorkArea_BETWEEN_Weeks(
		   int countryId,
		   int? hallId,
		   int year,
		   int operationId,
		   int departmentId,
		   int workAreaId,
		   int? workStationId,
		   int weekNumberFrom,
		   int weekNumberUntil,
		   bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND @weekNumberFrom <= [WeekNumber] AND [WeekNumber] <= @weekNumberUntil "
					+ $" AND [Year] = @year "
					+ $" AND [OperationId] = @operationId "
					+ $" AND [DepartementId] = @departmentId "
					+ $" AND [WorkAreaId] = @workAreaId "
					+ $"{(workStationId.HasValue ? $" AND [WorkStationId] = {workStationId.Value} " : "")} "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("weekNumberFrom", weekNumberFrom);
				sqlCommand.Parameters.AddWithValue("weekNumberUntil", weekNumberUntil);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("operationId", operationId);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				sqlCommand.Parameters.AddWithValue("workAreaId", workAreaId);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}
		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get_BY_CountryId_HallId_BETWEEN_WeekFirstDay_WeekLastDay(
			int countryId,
			int? hallId,
			DateTime weekFirstDay,
			DateTime weekLastDay,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND [WeekFirstDay] >= @weekFirstDay AND [WeekLastDay] <= @weekLastDay "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("weekFirstDay", weekFirstDay);
				sqlCommand.Parameters.AddWithValue("weekLastDay", weekLastDay);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}

		public static int Update_IsArchived_ArchiveTime_ArchiveUserId(int id,
			bool isArchived,
			DateTime? archiveTime,
			int? archiveUserId)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ " WHERE [Id]=@id";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("id", id);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update_IsArchived_ArchiveTime_ArchiveUserId(List<int> ids,
			bool isArchived,
			DateTime? archiveTime,
			int? archiveUserId)
		{
			if(ids == null || ids.Count <= 0)
				return 0;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ $" WHERE [Id] IN ({string.Join(",", ids)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static int SoftDelete(List<int> ids,
			bool isArchived,
			DateTime? archiveTime,
			int? archiveUserId)
		{
			if(ids == null || ids.Count <= 0)
				return 0;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [Attendance]=@Attendance, "
					+ " [AvailableSrDaily]=@AvailableSrDaily, "
					+ " [Factor1HrDaily]=@Factor1HrDaily, "
					+ " [Factor2HrDaily]=@Factor2HrDaily, [Factor3SrDaily]=@Factor3SrDaily, "
					+ " [PlanCapacity]=@PlanCapacity, "
					+ " [ProductivityHrDaily]=@ProductivityHrDaily, [ProductivitySrDaily]=@ProductivitySrDaily, "
					+ " [RequiredEmployees]=@RequiredEmployees, [ShiftsNumberWeekly]=@ShiftsNumberWeekly, "
					+ " [SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily, [SpecialHoursWeekly]=@SpecialHoursWeekly, "
					+ " [SpecialShiftsWeekly]=@SpecialShiftsWeekly, "
					+ " [HrHardResourcesNumber]=@HrHardResourcesNumber, [AvailableHrDaily]=@AvailableHrDaily, [WorkingHoursPerShift]=@WorkingHoursPerShift "
					+ $" WHERE [Id] IN ({string.Join(",", ids)})";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Attendance", 0);
				sqlCommand.Parameters.AddWithValue("AvailableSrDaily", 0);
				sqlCommand.Parameters.AddWithValue("Factor1HrDaily", 0);
				sqlCommand.Parameters.AddWithValue("Factor2HrDaily", 0);
				sqlCommand.Parameters.AddWithValue("Factor3SrDaily", 0);
				sqlCommand.Parameters.AddWithValue("PlanCapacity", 0);
				sqlCommand.Parameters.AddWithValue("ProductivityHrDaily", 0);
				sqlCommand.Parameters.AddWithValue("ProductivitySrDaily", 0);
				sqlCommand.Parameters.AddWithValue("RequiredEmployees", 0);
				sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly", 0);
				sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily", 0);
				sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", 0);
				sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", 0);

				sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", 0);
				sqlCommand.Parameters.AddWithValue("AvailableHrDaily", 0);
				sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", 0);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static int Archive_BY_Country_Hall_Week_Operation_Department_WorkArea(
			int archiveUserId,
			int countryId,
			int? hallId,
			int year,
			int weekNumber,
			int operationId,
			int departmentId,
			int workAreaId,
			int? workStationId,
			bool? isArchived = false)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ $" WHERE [CountryId]={countryId} "
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ $" AND [WeekNumber] = @weekNumber "
					+ $" AND [Year] = @year "
					+ $" AND [OperationId] = @operationId "
					+ $" AND [DepartementId] = @departmentId "
					+ $" AND [WorkAreaId] = @workAreaId "
					+ $"{(workStationId.HasValue ? $" AND [WorkStationId] = {workStationId.Value} " : "")} "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("weekNumber", weekNumber);
				sqlCommand.Parameters.AddWithValue("operationId", operationId);
				sqlCommand.Parameters.AddWithValue("departmentId", departmentId);
				sqlCommand.Parameters.AddWithValue("workAreaId", workAreaId);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("archiveTime", DateTime.Now);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}

		}

		public static List<Entities.Tables.MTM.CapacityPlanEntity> Get(int countryId,
			int year,
			int operationId,
			int hallId,
			int departementId,
			int? workAreaId,
			int? workStationId,
			int weekNumberFrom,
			int weekNumberTo,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan] "
					+ " WHERE [CountryId]=@countryId AND [Year]=@year "
					+ " AND [OperationId]=@operationId AND [HallId]=@hallId AND [DepartementId]=@departementId "
					+ $" AND {(workAreaId.HasValue ? $"[WorkAreaId]={workAreaId.Value}" : " [WorkAreaId] IS NULL ")}"
					+ $" AND {(workStationId.HasValue ? $"[WorkStationId]={workStationId.Value}" : " [WorkStationId] IS NULL ")}"
					+ " AND [WeekNumber] BETWEEN @weekNumberFrom AND @weekNumberTo "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("operationId", operationId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("departementId", departementId);
				sqlCommand.Parameters.AddWithValue("weekNumberFrom", weekNumberFrom);
				sqlCommand.Parameters.AddWithValue("weekNumberTo", weekNumberTo);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}

		public static Entities.Tables.MTM.CapacityPlanEntity GetCreationBy_CountryId_HallId_Year(int countryId,
		   int hallId,
		   int year)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = $" SELECT * FROM [__MTM_CRP_CapacityPlan] WHERE Id = "
					+ $" (SELECT MAX(Id) FROM [__MTM_CRP_CapacityPlan] WHERE [CountryId]={countryId} AND [HallId]={hallId} AND [Year]={year} AND [IsArchived]=0)";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.CapacityPlanEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}
		public static List<Entities.Tables.MTM.CapacityPlanEntity> GetCreationBy_CountryId_HallId_Year(int countryId,
		   List<int> hallId,
		   int year)
		{
			if(hallId == null || hallId.Count <= 0)
				return null;

			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = $" SELECT * FROM [__MTM_CRP_CapacityPlan] WHERE Id = "
					+ $" (SELECT MAX(Id) FROM [__MTM_CRP_CapacityPlan] WHERE [CountryId]={countryId} AND [HallId] IN ({string.Join(",", hallId)}) AND [Year]={year} AND [IsArchived]=0 GROUP BY [HallId])";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanEntity>();
			}
		}
		public static int UpdateVersion(int year, int countryId, int hallId, int userId)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " UPDATE [__MTM_CRP_CapacityPlan] SET "
					+ " [LastUpdateTime]=@updateTime, [LastUpdateUserId]=@updateUserId, [Version]="
					+ " (SELECT MAX([Version])+1 FROM  [__MTM_CRP_CapacityPlan] WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId) "
					+ " WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId AND [IsArchived]=0";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("updateTime", DateTime.Now);
				sqlCommand.Parameters.AddWithValue("updateUserId", userId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.MTM.CapacityPlanEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.MTM.CapacityPlanEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Entities.Tables.MTM.CapacityPlanEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
