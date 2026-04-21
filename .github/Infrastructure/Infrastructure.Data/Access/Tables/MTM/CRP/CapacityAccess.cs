using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityAccess
	{
		#region Default Methods
		public static Entities.Tables.MTM.CapacityEntity Get(int id,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "SELECT * FROM [__MTM_CRP_Capacity] WHERE [Id]=@Id "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return new Entities.Tables.MTM.CapacityEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Entities.Tables.MTM.CapacityEntity> Get(bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var query = "SELECT * FROM [__MTM_CRP_Capacity] "
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
				return new List<Entities.Tables.MTM.CapacityEntity>();
			}
		}
		public static List<Entities.Tables.MTM.CapacityEntity> Get(List<int> ids,
			bool? isArchived = false)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Access.Settings.MAX_BATCH_SIZE;
				if(ids.Count <= maxQueryNumber)
				{
					return get(ids, isArchived);
				}

				int batchNumber = ids.Count / maxQueryNumber;
				var results = new List<Entities.Tables.MTM.CapacityEntity>();
				for(int i = 0; i < batchNumber; i++)
				{
					results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber), isArchived));
				}
				results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber), isArchived));
				return results;
			}
			return new List<Entities.Tables.MTM.CapacityEntity>();
		}
		private static List<Entities.Tables.MTM.CapacityEntity> get(List<int> ids,
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

					sqlCommand.CommandText = "SELECT * FROM [__MTM_CRP_Capacity] WHERE [Id] IN (" + queryIds + ") "
						+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);
					DbExecution.Fill(sqlCommand, dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return toList(dataTable);
				}
				else
				{
					return new List<Entities.Tables.MTM.CapacityEntity>();
				}
			}
			return new List<Entities.Tables.MTM.CapacityEntity>();
		}

		public static int Insert(Entities.Tables.MTM.CapacityEntity item)
		{
			int response = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_Capacity] "
					+ " ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableSrDaily],[CountryId], "
					+ " [CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName], "
					+ " [Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName], "
					+ " [HolidaysNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName], "
					+ " [PlanCapacity],[PlanId],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees], "
					+ " [ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly], "
					+ " [Version],[WeekNumber],[WorkAreaId],[WorkAreaName],[Year], "
					+ " [WorkStationId],[WorkStationName],[WeekFirstDay],[WeekLastDay],[HrHardResourcesNumber],[AvailableHrDaily],[WorkingHoursPerShift]) "
					+ " VALUES "
					+ " (@ArchiveTime,@ArchiveUserId,@Attendance,@AvailableSrDaily,@CountryId,"
					+ " @CountryName,@CreationTime,@CreationUserId,@DepartementId,@DepartementName, "
					+ " @Factor1HrDaily,@Factor2HrDaily,@Factor3SrDaily,@FormToolInsert,@HallId,@HallName, "
					+ " @HolidaysNumber,@IsArchived,@LastUpdateTime,@LastUpdateUserId,@OperationId,@OperationName, "
					+ " @PlanCapacity,@PlanId,@ProductivityHrDaily,@ProductivitySrDaily,@RequiredEmployees, "
					+ " @ShiftsNumberWeekly,@SoftRessourcesNumberDaily,@SpecialHoursWeekly,@SpecialShiftsWeekly, "
					+ " @Version,@WeekNumber,@WorkAreaId,@WorkAreaName,@Year, "
					+ " @WorkStationId,@WorkStationName,@WeekFirstDay,@WeekLastDay,@HrHardResourcesNumber,@AvailableHrDaily,@WorkingHoursPerShift)";

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
					sqlCommand.Parameters.AddWithValue("HolidaysNumber", item.HolidaysNumber);
					sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
					sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
					sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
					sqlCommand.Parameters.AddWithValue("OperationId", item.OperationId);
					sqlCommand.Parameters.AddWithValue("OperationName", item.OperationName);
					sqlCommand.Parameters.AddWithValue("PlanCapacity", item.PlanCapacity);
					sqlCommand.Parameters.AddWithValue("PlanId", item.PlanId);
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

				using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [__MTM_CRP_Capacity] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
				{
					response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
				}

				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityEntity> items)
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
						query += " INSERT INTO [__MTM_CRP_Capacity] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HolidaysNumber],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[PlanId],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkingHoursPerShift],[WorkStationId],[WorkStationName],[Year]) VALUES ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@Attendance" + i + ","
							+ "@AvailableHrDaily" + i + ","
							+ "@AvailableSrDaily" + i + ","
							+ "@CountryId" + i + ","
							+ "@CountryName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@DepartementId" + i + ","
							+ "@DepartementName" + i + ","
							+ "@Factor1HrDaily" + i + ","
							+ "@Factor2HrDaily" + i + ","
							+ "@Factor3SrDaily" + i + ","
							+ "@FormToolInsert" + i + ","
							+ "@HallId" + i + ","
							+ "@HallName" + i + ","
							+ "@HolidaysNumber" + i + ","
							+ "@HrHardResourcesNumber" + i + ","
							+ "@IsArchived" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@OperationId" + i + ","
							+ "@OperationName" + i + ","
							+ "@PlanCapacity" + i + ","
							+ "@PlanId" + i + ","
							+ "@ProductivityHrDaily" + i + ","
							+ "@ProductivitySrDaily" + i + ","
							+ "@RequiredEmployees" + i + ","
							+ "@ShiftsNumberWeekly" + i + ","
							+ "@SoftRessourcesNumberDaily" + i + ","
							+ "@SpecialHoursWeekly" + i + ","
							+ "@SpecialShiftsWeekly" + i + ","
							+ "@Version" + i + ","
							+ "@WeekFirstDay" + i + ","
							+ "@WeekLastDay" + i + ","
							+ "@WeekNumber" + i + ","
							+ "@WorkAreaId" + i + ","
							+ "@WorkAreaName" + i + ","
							+ "@WorkingHoursPerShift" + i + ","
							+ "@WorkStationId" + i + ","
							+ "@WorkStationName" + i + ","
							+ "@Year" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Attendance" + i, item.Attendance);
						sqlCommand.Parameters.AddWithValue("AvailableHrDaily" + i, item.AvailableHrDaily);
						sqlCommand.Parameters.AddWithValue("AvailableSrDaily" + i, item.AvailableSrDaily);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DepartementId" + i, item.DepartementId);
						sqlCommand.Parameters.AddWithValue("DepartementName" + i, item.DepartementName);
						sqlCommand.Parameters.AddWithValue("Factor1HrDaily" + i, item.Factor1HrDaily);
						sqlCommand.Parameters.AddWithValue("Factor2HrDaily" + i, item.Factor2HrDaily);
						sqlCommand.Parameters.AddWithValue("Factor3SrDaily" + i, item.Factor3SrDaily);
						sqlCommand.Parameters.AddWithValue("FormToolInsert" + i, item.FormToolInsert == null ? (object)DBNull.Value : item.FormToolInsert);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("HolidaysNumber" + i, item.HolidaysNumber);
						sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber" + i, item.HrHardResourcesNumber);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OperationId" + i, item.OperationId);
						sqlCommand.Parameters.AddWithValue("OperationName" + i, item.OperationName);
						sqlCommand.Parameters.AddWithValue("PlanCapacity" + i, item.PlanCapacity);
						sqlCommand.Parameters.AddWithValue("PlanId" + i, item.PlanId);
						sqlCommand.Parameters.AddWithValue("ProductivityHrDaily" + i, item.ProductivityHrDaily);
						sqlCommand.Parameters.AddWithValue("ProductivitySrDaily" + i, item.ProductivitySrDaily);
						sqlCommand.Parameters.AddWithValue("RequiredEmployees" + i, item.RequiredEmployees);
						sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly" + i, item.ShiftsNumberWeekly);
						sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily" + i, item.SoftRessourcesNumberDaily);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
						sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift" + i, item.WorkingHoursPerShift == null ? (object)DBNull.Value : item.WorkingHoursPerShift);
						sqlCommand.Parameters.AddWithValue("WorkStationId" + i, item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
						sqlCommand.Parameters.AddWithValue("WorkStationName" + i, item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}


		//public static int Update(Entities.Tables.MTM.CapacityEntity item)
		//{
		//    int results = -1;

		//    using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
		//    {
		//        sqlConnection.Open();

		//        string query = "UPDATE [__MTM_CRP_Capacity] SET "
		//            + " [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Attendance]=@Attendance, "
		//            + " [AvailableSrDaily]=@AvailableSrDaily, [CountryId]=@CountryId, [CountryName]=@CountryName, "
		//            + " [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DepartementId]=@DepartementId, "
		//            + " [DepartementName]=@DepartementName, [Factor1HrDaily]=@Factor1HrDaily, "
		//            + " [Factor2HrDaily]=@Factor2HrDaily, [Factor3SrDaily]=@Factor3SrDaily, [FormToolInsert]=@FormToolInsert, "
		//            + " [HallId]=@HallId, [HallName]=@HallName, [HolidaysNumber]=@HolidaysNumber, [IsArchived]=@IsArchived, "
		//            + " [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OperationId]=@OperationId, "
		//            + " [OperationName]=@OperationName, [PlanCapacity]=@PlanCapacity, [PlanId]=@PlanId, "
		//            + " [ProductivityHrDaily]=@ProductivityHrDaily, [ProductivitySrDaily]=@ProductivitySrDaily, "
		//            + " [RequiredEmployees]=@RequiredEmployees, [ShiftsNumberWeekly]=@ShiftsNumberWeekly, "
		//            + " [SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily, [SpecialHoursWeekly]=@SpecialHoursWeekly, "
		//            + " [SpecialShiftsWeekly]=@SpecialShiftsWeekly, [Version]=@Version, [WeekNumber]=@WeekNumber, "
		//            + " [WorkAreaId]=@WorkAreaId, [WorkAreaName]=@WorkAreaName, [Year]=@Year, "
		//            + " [WorkStationId]=@WorkStationId, [WorkStationName]=@WorkStationName, "
		//            + " [WeekFirstDay]=@WeekFirstDay, [WeekLastDay]=@WeekLastDay, "
		//            + " [HrHardResourcesNumber]=@HrHardResourcesNumber, [AvailableHrDaily]=@AvailableHrDaily, [WorkingHoursPerShift]=@WorkingHoursPerShift "
		//            + " WHERE [Id]=@Id";

		//        var sqlCommand = new SqlCommand(query, sqlConnection);
		//        sqlCommand.Parameters.AddWithValue("Id", item.Id);
		//        sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
		//        sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
		//        sqlCommand.Parameters.AddWithValue("Attendance", item.Attendance);
		//        sqlCommand.Parameters.AddWithValue("AvailableSrDaily", item.AvailableSrDaily);
		//        sqlCommand.Parameters.AddWithValue("CountryId", item.CountryId);
		//        sqlCommand.Parameters.AddWithValue("CountryName", item.CountryName);
		//        sqlCommand.Parameters.AddWithValue("CreationTime", item.CreationTime);
		//        sqlCommand.Parameters.AddWithValue("CreationUserId", item.CreationUserId);
		//        sqlCommand.Parameters.AddWithValue("DepartementId", item.DepartementId);
		//        sqlCommand.Parameters.AddWithValue("DepartementName", item.DepartementName);
		//        sqlCommand.Parameters.AddWithValue("Factor1HrDaily", item.Factor1HrDaily);
		//        sqlCommand.Parameters.AddWithValue("Factor2HrDaily", item.Factor2HrDaily);
		//        sqlCommand.Parameters.AddWithValue("Factor3SrDaily", item.Factor3SrDaily);
		//        sqlCommand.Parameters.AddWithValue("FormToolInsert", item.FormToolInsert == null ? (object)DBNull.Value : item.FormToolInsert);
		//        sqlCommand.Parameters.AddWithValue("HallId", item.HallId);
		//        sqlCommand.Parameters.AddWithValue("HallName", item.HallName);
		//        sqlCommand.Parameters.AddWithValue("HolidaysNumber", item.HolidaysNumber);
		//        sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
		//        sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
		//        sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
		//        sqlCommand.Parameters.AddWithValue("OperationId", item.OperationId);
		//        sqlCommand.Parameters.AddWithValue("OperationName", item.OperationName);
		//        sqlCommand.Parameters.AddWithValue("PlanCapacity", item.PlanCapacity);
		//        sqlCommand.Parameters.AddWithValue("PlanId", item.PlanId);
		//        sqlCommand.Parameters.AddWithValue("ProductivityHrDaily", item.ProductivityHrDaily);
		//        sqlCommand.Parameters.AddWithValue("ProductivitySrDaily", item.ProductivitySrDaily);
		//        sqlCommand.Parameters.AddWithValue("RequiredEmployees", item.RequiredEmployees);
		//        sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly", item.ShiftsNumberWeekly);
		//        sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily", item.SoftRessourcesNumberDaily);
		//        sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", item.SpecialHoursWeekly);
		//        sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", item.SpecialShiftsWeekly);
		//        sqlCommand.Parameters.AddWithValue("Version", item.Version);
		//        sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
		//        sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
		//        sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
		//        sqlCommand.Parameters.AddWithValue("Year", item.Year);

		//        sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
		//        sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
		//        sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
		//        sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);

		//        sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", item.HrHardResourcesNumber);
		//        sqlCommand.Parameters.AddWithValue("AvailableHrDaily", item.AvailableHrDaily);
		//        sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", item.WorkingHoursPerShift);

		//        results = DbExecution.ExecuteNonQuery(sqlCommand);
		//    }

		//    return results;
		//}

		public static int Delete(int id)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [__MTM_CRP_Capacity] WHERE [Id]=@Id";

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

					string query = "DELETE FROM [__MTM_CRP_Capacity] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Get_Count_BY_CountryId_HallId_Year(
			int year,
			int countryId,
			int hallId)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [__MTM_CRP_Capacity] "
					+ $" WHERE [CountryId]=@countryId "
					+ $" AND [HallId]=@hallId"
					+ $" AND [Year] = @year";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("year", year);

				return int.TryParse(DbExecution.ExecuteScalar(sqlCommand).ToString(), out var _v) ? _v : 0;
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

				string query = "UPDATE [__MTM_CRP_Capacity] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ " WHERE [Id]=@id ";

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

				string query = "UPDATE [__MTM_CRP_Capacity] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ $" WHERE [Id] IN ({string.Join(",", ids)}) ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update_IsArchived_ArchiveTime_ArchiveUserId(int year, int countryId, int hallId,
			bool isArchived,
			DateTime? archiveTime,
			int? archiveUserId)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "UPDATE [__MTM_CRP_Capacity] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ " WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId ";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}
		public static int Update_IsArchived_ArchiveTime_ArchiveUserId(int year, int countryId, int hallId,
		   bool isArchived,
		   DateTime? archiveTime,
		   int? archiveUserId, int firstWeek = 1, int lastWeek = 53)
		{
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "UPDATE [__MTM_CRP_Capacity] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ " WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId AND @firstWeek<=[WeekNumber] AND [WeekNumber]<=@lastWeek";

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
				sqlCommand.Parameters.AddWithValue("firstWeek", firstWeek);
				sqlCommand.Parameters.AddWithValue("lastWeek", lastWeek);
				sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
				sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);

				return DbExecution.ExecuteNonQuery(sqlCommand);
			}
		}

		public static List<Entities.Tables.MTM.CapacityEntity> Get_BY_CountryId_HallId_BETWEEN_WeekFirstDay_WeekLastDay(
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

				string query = " SELECT * FROM [__MTM_CRP_Capacity] "
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
				return new List<Entities.Tables.MTM.CapacityEntity>();
			}
		}

		public static List<Entities.Tables.MTM.CapacityEntity> GetBYCountryHallYear(
			int countryId,
			int hallId,
			int year,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_Capacity] "
					+ $" WHERE [CountryId]=@countryId "
					+ $" AND [HallId]=@hallId "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("year", year);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityEntity>();
			}
		}

		public static List<Entities.Tables.MTM.CapacityEntity> Get(
			int countryId,
			int year,
			int weekNumberFrom,
			int weekNumberUntil,
			int? operationId,
			int? hallId,
			int? departementId,
			int? workAreaId,
			int? workStationId,
			bool? isArchived = false)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT * FROM [__MTM_CRP_Capacity] "
					+ $" WHERE [CountryId]={countryId} "
					+ $" AND [Year]={year} "
					+ $" AND [WeekNumber]>={weekNumberFrom} "
					+ $" AND [WeekNumber]<={weekNumberUntil} "
					+ (operationId.HasValue ? $" AND [OperationId]={operationId.Value}" : string.Empty)
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : string.Empty)
					+ (departementId.HasValue ? $" AND [DepartementId]={departementId.Value}" : string.Empty)
					+ (workAreaId.HasValue ? $" AND [WorkAreaId]={workAreaId.Value}" : string.Empty)
					+ (workStationId.HasValue ? $" AND [WorkStationId]={workStationId.Value}" : string.Empty)
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				new SqlDataAdapter(new SqlCommand(query, sqlConnection)).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityEntity>();
			}
		}


		public static int Delete(
			int year,
			int countryId,
			int weekNumberFrom,
			int weekNumberUntil,
			int? operationId,
			int? hallId,
			int? departementId,
			int? workAreaId,
			int? workStationId = null,
			bool? isArchived = false)
		{
			int results = -1;

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = "DELETE FROM [__MTM_CRP_Capacity] "
					+ $" WHERE [CountryId]={countryId} "
					+ $" AND [Year]={year} "
					+ $" AND [WeekNumber]>={weekNumberFrom} "
					+ $" AND [WeekNumber]<={weekNumberUntil} "
					+ (operationId.HasValue ? $" AND [OperationId]={operationId.Value}" : " AND [OperationId] IS NULL")
					+ (hallId.HasValue ? $" AND [HallId]={hallId.Value}" : " AND [HallId] IS NULL")
					+ (departementId.HasValue ? $" AND [DepartementId]={departementId.Value}" : string.Empty)
					+ (workAreaId.HasValue ? $" AND [WorkAreaId]={workAreaId.Value}" : " AND [WorkAreaId]")
					+ (workStationId.HasValue ? $" AND [WorkStationId]={workStationId.Value}" : " AND [WorkStationId] IS NULL")
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : " AND [IsArchived] IS NULL");

				var sqlCommand = new SqlCommand(query, sqlConnection);

				results = DbExecution.ExecuteNonQuery(sqlCommand);
			}

			return results;
		}

		public static bool HasSpecialShifts(
			int countryId,
			int hallId,
			int year,
			int weekNumber,
			bool? isArchived = false)
		{

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				string query = " SELECT COUNT(*) FROM [__MTM_CRP_Capacity] "
					+ $" WHERE [SpecialShiftsWeekly]>0 AND [SpecialHoursWeekly]>0 AND [CountryId]=@countryId AND [HallId]=@hallId AND [Year]=@year AND [WeekNumber]= @weekNumber "
					+ (isArchived.HasValue ? $" AND [IsArchived]={(isArchived.Value ? 1 : 0)}" : string.Empty);

				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("weekNumber", weekNumber);

				return bool.TryParse(sqlCommand.ToString(), out var v) ? v : false;
			}
		}

		public static List<Entities.Tables.MTM.CapacityEntity> GetSpecialShifts(int? year, int? countryId, int? hallId, List<int> weekNumbers = null)
		{
			var dataTable = new DataTable();

			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();

				var query = "SELECT * FROM [__MTM_CRP_Capacity] WHERE [IsArchived]=0 AND [SpecialShiftsWeekly]>0 AND [SpecialHoursWeekly]>0 " +
					$"{(countryId.HasValue ? $" AND [CountryId]={countryId.Value} " : "")}" +
					$"{(hallId.HasValue ? $" AND [HallId]={hallId.Value} " : "")}" +
					$"{(year.HasValue ? $"AND [Year]={year.Value}" : "")}" +
					$"{(weekNumbers == null || weekNumbers.Count <= 0 ? "" : $" AND [WeekNumber] IN ({string.Join(",", weekNumbers)})")}";

				var sqlCommand = new SqlCommand(query, sqlConnection);

				DbExecution.Fill(sqlCommand, dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return toList(dataTable);
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityEntity>();
			}
		}

		public static int Update_IsArchived_ArchiveTime_ArchiveUserId_AND_Insert(int id,
			bool isArchived,
			DateTime archiveTime,
			int archiveUserId,
			Entities.Tables.MTM.CapacityEntity item)
		{
			var response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				try
				{

					// - Archive
					var queryArchive = "UPDATE [__MTM_CRP_Capacity] SET "
						+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
						+ " WHERE [Id]=@id ";
					using(var sqlCommand = new SqlCommand(queryArchive, sqlConnection, sqlTransaction))
					{
						sqlCommand.Parameters.AddWithValue("id", id);
						sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
						sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime);
						sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId);
						DbExecution.ExecuteNonQuery(sqlCommand);
					}

					// - Insert
					#region Insert
					string query = "INSERT INTO [__MTM_CRP_Capacity] "
						+ " ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableSrDaily],[CountryId], "
						+ " [CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName], "
						+ " [Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName], "
						+ " [HolidaysNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName], "
						+ " [PlanCapacity],[PlanId],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees], "
						+ " [ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly], "
						+ " [Version],[WeekNumber],[WorkAreaId],[WorkAreaName],[Year], "
						+ " [WorkStationId],[WorkStationName],[WeekFirstDay],[WeekLastDay],[HrHardResourcesNumber],[AvailableHrDaily],[WorkingHoursPerShift]) "
						+ " VALUES "
						+ " (@ArchiveTime,@ArchiveUserId,@Attendance,@AvailableSrDaily,@CountryId,"
						+ " @CountryName,@CreationTime,@CreationUserId,@DepartementId,@DepartementName, "
						+ " @Factor1HrDaily,@Factor2HrDaily,@Factor3SrDaily,@FormToolInsert,@HallId,@HallName, "
						+ " @HolidaysNumber,@IsArchived,@LastUpdateTime,@LastUpdateUserId,@OperationId,@OperationName, "
						+ " @PlanCapacity,@PlanId,@ProductivityHrDaily,@ProductivitySrDaily,@RequiredEmployees, "
						+ " @ShiftsNumberWeekly,@SoftRessourcesNumberDaily,@SpecialHoursWeekly,@SpecialShiftsWeekly, "
						+ " @Version,@WeekNumber,@WorkAreaId,@WorkAreaName,@Year, "
						+ " @WorkStationId,@WorkStationName,@WeekFirstDay,@WeekLastDay,@HrHardResourcesNumber,@AvailableHrDaily,@WorkingHoursPerShift)";

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
						sqlCommand.Parameters.AddWithValue("HolidaysNumber", item.HolidaysNumber);
						sqlCommand.Parameters.AddWithValue("IsArchived", item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime", item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId", item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OperationId", item.OperationId);
						sqlCommand.Parameters.AddWithValue("OperationName", item.OperationName);
						sqlCommand.Parameters.AddWithValue("PlanCapacity", item.PlanCapacity);
						sqlCommand.Parameters.AddWithValue("PlanId", item.PlanId);
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

					using(var sqlCommand = new SqlCommand("SELECT [Id] FROM [__MTM_CRP_Capacity] WHERE [Id] = @@IDENTITY", sqlConnection, sqlTransaction))
					{
						response = int.Parse(DbExecution.ExecuteScalar(sqlCommand)?.ToString() ?? "-1");
					}

					#endregion Insert

					// -
					sqlTransaction.Commit();
				} catch(Exception ex)
				{
					// Attempt to roll back the transaction.
					try
					{
						sqlTransaction.Rollback();
					} catch(Exception ex2)
					{
						// This catch block will handle any errors that may have occurred
						// on the server that would cause the rollback to fail, such as
						// a closed connection.
						throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}" +
							$"Rollback Exception Type: {ex2.GetType()} // Message: {ex2.Message}"));
					}

					// -
					throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}"));
				}
				return response;
			}
		}
		public static int Archive_Insert(int year, int countryId, int hallId,
				bool isArchived, DateTime? archiveTime,
				int? archiveUserId, int firstWeek = 1, int lastWeek = 53,
				List<Entities.Tables.MTM.CapacityEntity> items = null)
		{
			var response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				try
				{

					// - Archive
					var queryArchive = "UPDATE [__MTM_CRP_Capacity] SET "
					+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
					+ " WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId AND @firstWeek<=[WeekNumber] AND [WeekNumber]<=@lastWeek";
					using(var sqlCommand = new SqlCommand(queryArchive, sqlConnection, sqlTransaction))
					{
						sqlCommand.Parameters.AddWithValue("year", year);
						sqlCommand.Parameters.AddWithValue("countryId", countryId);
						sqlCommand.Parameters.AddWithValue("hallId", hallId);
						sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
						sqlCommand.Parameters.AddWithValue("firstWeek", firstWeek);
						sqlCommand.Parameters.AddWithValue("lastWeek", lastWeek);
						sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
						sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);
						DbExecution.ExecuteNonQuery(sqlCommand);
					}

					// - Insert
					Insert(items, sqlConnection, sqlTransaction);

					// -
					sqlTransaction.Commit();
				} catch(Exception ex)
				{
					// Attempt to roll back the transaction.
					try
					{
						sqlTransaction.Rollback();
					} catch(Exception ex2)
					{
						// This catch block will handle any errors that may have occurred
						// on the server that would cause the rollback to fail, such as
						// a closed connection.
						throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}" +
							$"Rollback Exception Type: {ex2.GetType()} // Message: {ex2.Message}"));
					}

					// -
					throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}"));
				}
				return response;
			}
		}

		/// <summary>
		/// Archive, then Delete then Insert new Capacities.
		/// If year is negative, no Archive
		/// If deleteIds is NULL or Empty, no Delete
		/// If items is NULL or Empty, no Insert
		/// </summary>
		/// <param name="year"></param>
		/// <param name="countryId"></param>
		/// <param name="hallId"></param>
		/// <param name="isArchived"></param>
		/// <param name="archiveTime"></param>
		/// <param name="archiveUserId"></param>
		/// <param name="firstWeek"></param>
		/// <param name="lastWeek"></param>
		/// <param name="deleteIds"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public static int Archive_Delete_Insert(
				int year, int countryId, int hallId,
				bool isArchived, DateTime? archiveTime,
				int? archiveUserId, int firstWeek = 1, int lastWeek = 53,
				List<int> deleteIds = null,
				List<Entities.Tables.MTM.CapacityEntity> items = null)
		{
			var response = -1;
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				try
				{

					// - Archive
					if(year > 0)
					{
						var queryArchive = "UPDATE [__MTM_CRP_Capacity] SET "
						+ " [ArchiveTime]=@archiveTime, [ArchiveUserId]=@archiveUserId, [IsArchived]=@isArchived "
						+ " WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId AND @firstWeek<=[WeekNumber] AND [WeekNumber]<=@lastWeek";
						using(var sqlCommand = new SqlCommand(queryArchive, sqlConnection, sqlTransaction))
						{
							sqlCommand.Parameters.AddWithValue("year", year);
							sqlCommand.Parameters.AddWithValue("countryId", countryId);
							sqlCommand.Parameters.AddWithValue("hallId", hallId);
							sqlCommand.Parameters.AddWithValue("isArchived", isArchived);
							sqlCommand.Parameters.AddWithValue("firstWeek", firstWeek);
							sqlCommand.Parameters.AddWithValue("lastWeek", lastWeek);
							sqlCommand.Parameters.AddWithValue("archiveTime", archiveTime == null ? (object)DBNull.Value : archiveTime);
							sqlCommand.Parameters.AddWithValue("archiveUserId", archiveUserId == null ? (object)DBNull.Value : archiveUserId);
							DbExecution.ExecuteNonQuery(sqlCommand);
						}
					}

					// - Delete
					if(deleteIds != null && deleteIds.Count > 0)
					{
						Delete(deleteIds, sqlConnection, sqlTransaction);
					}

					// - Insert
					if(items != null && items.Count > 0)
					{
						Insert(items, sqlConnection, sqlTransaction);
					}

					// -
					sqlTransaction.Commit();
				} catch(Exception ex)
				{
					// Attempt to roll back the transaction.
					try
					{
						sqlTransaction.Rollback();
					} catch(Exception ex2)
					{
						// This catch block will handle any errors that may have occurred
						// on the server that would cause the rollback to fail, such as
						// a closed connection.
						throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}" +
							$"Rollback Exception Type: {ex2.GetType()} // Message: {ex2.Message}"));
					}

					// -
					throw (new Exception($"Commit Exception Type: {ex.GetType()} // Message: {ex.Message}"));
				}
				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 44; // Nb params per query
				int results = 0;
				if(items.Count <= maxParamsNumber)
				{
					results = insert(items, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = items.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += insert(items.GetRange(i * maxParamsNumber, maxParamsNumber), sqlConnection, sqlTransaction);
					}
					results += insert(items.GetRange(batchNumber * maxParamsNumber, items.Count - batchNumber * maxParamsNumber), sqlConnection, sqlTransaction);
				}
				return results;
			}

			return -1;
		}
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityEntity> items, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(items != null && items.Count > 0)
			{
				int results = -1;
				// using (var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
				{
					// sqlConnection.Open();
					string query = "";
					var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);

					int i = 0;
					foreach(var item in items)
					{
						i++;
						query += " INSERT INTO [__MTM_CRP_Capacity] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HolidaysNumber],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[PlanId],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkingHoursPerShift],[WorkStationId],[WorkStationName],[Year]) VALUES ( "

							+ "@ArchiveTime" + i + ","
							+ "@ArchiveUserId" + i + ","
							+ "@Attendance" + i + ","
							+ "@AvailableHrDaily" + i + ","
							+ "@AvailableSrDaily" + i + ","
							+ "@CountryId" + i + ","
							+ "@CountryName" + i + ","
							+ "@CreationTime" + i + ","
							+ "@CreationUserId" + i + ","
							+ "@DepartementId" + i + ","
							+ "@DepartementName" + i + ","
							+ "@Factor1HrDaily" + i + ","
							+ "@Factor2HrDaily" + i + ","
							+ "@Factor3SrDaily" + i + ","
							+ "@FormToolInsert" + i + ","
							+ "@HallId" + i + ","
							+ "@HallName" + i + ","
							+ "@HolidaysNumber" + i + ","
							+ "@HrHardResourcesNumber" + i + ","
							+ "@IsArchived" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@OperationId" + i + ","
							+ "@OperationName" + i + ","
							+ "@PlanCapacity" + i + ","
							+ "@PlanId" + i + ","
							+ "@ProductivityHrDaily" + i + ","
							+ "@ProductivitySrDaily" + i + ","
							+ "@RequiredEmployees" + i + ","
							+ "@ShiftsNumberWeekly" + i + ","
							+ "@SoftRessourcesNumberDaily" + i + ","
							+ "@SpecialHoursWeekly" + i + ","
							+ "@SpecialShiftsWeekly" + i + ","
							+ "@Version" + i + ","
							+ "@WeekFirstDay" + i + ","
							+ "@WeekLastDay" + i + ","
							+ "@WeekNumber" + i + ","
							+ "@WorkAreaId" + i + ","
							+ "@WorkAreaName" + i + ","
							+ "@WorkingHoursPerShift" + i + ","
							+ "@WorkStationId" + i + ","
							+ "@WorkStationName" + i + ","
							+ "@Year" + i
							+ "); ";


						sqlCommand.Parameters.AddWithValue("ArchiveTime" + i, item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
						sqlCommand.Parameters.AddWithValue("ArchiveUserId" + i, item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
						sqlCommand.Parameters.AddWithValue("Attendance" + i, item.Attendance);
						sqlCommand.Parameters.AddWithValue("AvailableHrDaily" + i, item.AvailableHrDaily);
						sqlCommand.Parameters.AddWithValue("AvailableSrDaily" + i, item.AvailableSrDaily);
						sqlCommand.Parameters.AddWithValue("CountryId" + i, item.CountryId);
						sqlCommand.Parameters.AddWithValue("CountryName" + i, item.CountryName);
						sqlCommand.Parameters.AddWithValue("CreationTime" + i, item.CreationTime);
						sqlCommand.Parameters.AddWithValue("CreationUserId" + i, item.CreationUserId);
						sqlCommand.Parameters.AddWithValue("DepartementId" + i, item.DepartementId);
						sqlCommand.Parameters.AddWithValue("DepartementName" + i, item.DepartementName);
						sqlCommand.Parameters.AddWithValue("Factor1HrDaily" + i, item.Factor1HrDaily);
						sqlCommand.Parameters.AddWithValue("Factor2HrDaily" + i, item.Factor2HrDaily);
						sqlCommand.Parameters.AddWithValue("Factor3SrDaily" + i, item.Factor3SrDaily);
						sqlCommand.Parameters.AddWithValue("FormToolInsert" + i, item.FormToolInsert == null ? (object)DBNull.Value : item.FormToolInsert);
						sqlCommand.Parameters.AddWithValue("HallId" + i, item.HallId);
						sqlCommand.Parameters.AddWithValue("HallName" + i, item.HallName);
						sqlCommand.Parameters.AddWithValue("HolidaysNumber" + i, item.HolidaysNumber);
						sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber" + i, item.HrHardResourcesNumber);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OperationId" + i, item.OperationId);
						sqlCommand.Parameters.AddWithValue("OperationName" + i, item.OperationName);
						sqlCommand.Parameters.AddWithValue("PlanCapacity" + i, item.PlanCapacity);
						sqlCommand.Parameters.AddWithValue("PlanId" + i, item.PlanId);
						sqlCommand.Parameters.AddWithValue("ProductivityHrDaily" + i, item.ProductivityHrDaily);
						sqlCommand.Parameters.AddWithValue("ProductivitySrDaily" + i, item.ProductivitySrDaily);
						sqlCommand.Parameters.AddWithValue("RequiredEmployees" + i, item.RequiredEmployees);
						sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly" + i, item.ShiftsNumberWeekly);
						sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily" + i, item.SoftRessourcesNumberDaily);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
						sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift" + i, item.WorkingHoursPerShift == null ? (object)DBNull.Value : item.WorkingHoursPerShift);
						sqlCommand.Parameters.AddWithValue("WorkStationId" + i, item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
						sqlCommand.Parameters.AddWithValue("WorkStationName" + i, item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = DbExecution.ExecuteNonQuery(sqlCommand);
				}

				return results;
			}

			return -1;
		}

		public static int Delete(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxParamsNumber = 2100 / Access.Settings.MAX_BATCH_SIZE;
				int results = 0;
				if(ids.Count <= maxParamsNumber)
				{
					results = delete(ids, sqlConnection, sqlTransaction);
				}
				else
				{
					int batchNumber = ids.Count / maxParamsNumber;
					for(int i = 0; i < batchNumber; i++)
					{
						results += delete(ids.GetRange(i * maxParamsNumber, maxParamsNumber), sqlConnection, sqlTransaction);
					}
					results += delete(ids.GetRange(batchNumber * maxParamsNumber, ids.Count - batchNumber * maxParamsNumber), sqlConnection, sqlTransaction);
				}
			}
			return -1;
		}
		private static int delete(List<int> ids, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
		{
			if(ids != null && ids.Count > 0)
			{
				int results = -1;
				// using (var sqlConnection = new SqlConnection(Access.Settings.ConnectionStringMTM))
				{
					// sqlConnection.Open();
					using(var sqlCommand = new SqlCommand("", sqlConnection, sqlTransaction))
					{
						string queryIds = string.Empty;
						for(int i = 0; i < ids.Count; i++)
						{
							queryIds += "@Id" + i + ",";
							sqlCommand.Parameters.AddWithValue("Id" + i, ids[i]);
						}
						queryIds = queryIds.TrimEnd(',');

						string query = "DELETE FROM [__MTM_CRP_Capacity] WHERE [Id] IN (" + queryIds + ")";
						sqlCommand.CommandText = query;

						results = DbExecution.ExecuteNonQuery(sqlCommand);
					}
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Helpers
		private static List<Entities.Tables.MTM.CapacityEntity> toList(DataTable dataTable)
		{
			var list = new List<Entities.Tables.MTM.CapacityEntity>(dataTable.Rows.Count);
			foreach(DataRow dataRow in dataTable.Rows)
			{ list.Add(new Entities.Tables.MTM.CapacityEntity(dataRow)); }
			return list;
		}
		#endregion
	}
}
