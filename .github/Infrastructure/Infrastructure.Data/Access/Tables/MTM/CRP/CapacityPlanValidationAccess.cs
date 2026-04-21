using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityPlanValidationAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_Validation] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityPlan_Validation]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_CRP_CapacityPlan_Validation] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_CapacityPlan_Validation] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly],[ValidationLevel],[ValidationTime],[ValidationUserId],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkingHoursPerShift],[WorkStationId],[WorkStationName],[Year])  VALUES (@ArchiveTime,@ArchiveUserId,@Attendance,@AvailableHrDaily,@AvailableSrDaily,@CountryId,@CountryName,@CreationTime,@CreationUserId,@DepartementId,@DepartementName,@Factor1HrDaily,@Factor2HrDaily,@Factor3SrDaily,@FormToolInsert,@HallId,@HallName,@HrHardResourcesNumber,@IsArchived,@LastUpdateTime,@LastUpdateUserId,@OperationId,@OperationName,@PlanCapacity,@ProductivityHrDaily,@ProductivitySrDaily,@RequiredEmployees,@ShiftsNumberWeekly,@SoftRessourcesNumberDaily,@SpecialHoursWeekly,@SpecialShiftsWeekly,@ValidationLevel,@ValidationTime,@ValidationUserId,@Version,@WeekFirstDay,@WeekLastDay,@WeekNumber,@WorkAreaId,@WorkAreaName,@WorkingHoursPerShift,@WorkStationId,@WorkStationName,@Year); ";
				query += "SELECT SCOPE_IDENTITY();";

				using(var sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
				{

					sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
					sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
					sqlCommand.Parameters.AddWithValue("Attendance", item.Attendance);
					sqlCommand.Parameters.AddWithValue("AvailableHrDaily", item.AvailableHrDaily);
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
					sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", item.HrHardResourcesNumber);
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
					sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
					sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
					sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
					sqlCommand.Parameters.AddWithValue("Version", item.Version);
					sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
					sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);
					sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
					sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
					sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
					sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", item.WorkingHoursPerShift);
					sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
					sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
					sqlCommand.Parameters.AddWithValue("Year", item.Year);

					var result = sqlCommand.ExecuteScalar();
					response = result == null ? int.MinValue : int.TryParse(result.ToString(), out var insertedId) ? insertedId : int.MinValue;
				}
				sqlTransaction.Commit();

				return response;
			}
		}
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> items)
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
						query += " INSERT INTO [__MTM_CRP_CapacityPlan_Validation] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SpecialHoursWeekly],[SpecialShiftsWeekly],[ValidationLevel],[ValidationTime],[ValidationUserId],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkingHoursPerShift],[WorkStationId],[WorkStationName],[Year]) VALUES ( "

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
							+ "@HrHardResourcesNumber" + i + ","
							+ "@IsArchived" + i + ","
							+ "@LastUpdateTime" + i + ","
							+ "@LastUpdateUserId" + i + ","
							+ "@OperationId" + i + ","
							+ "@OperationName" + i + ","
							+ "@PlanCapacity" + i + ","
							+ "@ProductivityHrDaily" + i + ","
							+ "@ProductivitySrDaily" + i + ","
							+ "@RequiredEmployees" + i + ","
							+ "@ShiftsNumberWeekly" + i + ","
							+ "@SoftRessourcesNumberDaily" + i + ","
							+ "@SpecialHoursWeekly" + i + ","
							+ "@SpecialShiftsWeekly" + i + ","
							+ "@ValidationLevel" + i + ","
							+ "@ValidationTime" + i + ","
							+ "@ValidationUserId" + i + ","
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
						sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber" + i, item.HrHardResourcesNumber);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OperationId" + i, item.OperationId);
						sqlCommand.Parameters.AddWithValue("OperationName" + i, item.OperationName);
						sqlCommand.Parameters.AddWithValue("PlanCapacity" + i, item.PlanCapacity);
						sqlCommand.Parameters.AddWithValue("ProductivityHrDaily" + i, item.ProductivityHrDaily);
						sqlCommand.Parameters.AddWithValue("ProductivitySrDaily" + i, item.ProductivitySrDaily);
						sqlCommand.Parameters.AddWithValue("RequiredEmployees" + i, item.RequiredEmployees);
						sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly" + i, item.ShiftsNumberWeekly);
						sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily" + i, item.SoftRessourcesNumberDaily);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
						sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift" + i, item.WorkingHoursPerShift);
						sqlCommand.Parameters.AddWithValue("WorkStationId" + i, item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
						sqlCommand.Parameters.AddWithValue("WorkStationName" + i, item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
						sqlCommand.Parameters.AddWithValue("Year" + i, item.Year);
					}

					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}

			return -1;
		}

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_CapacityPlan_Validation] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Attendance]=@Attendance, [AvailableHrDaily]=@AvailableHrDaily, [AvailableSrDaily]=@AvailableSrDaily, [CountryId]=@CountryId, [CountryName]=@CountryName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DepartementId]=@DepartementId, [DepartementName]=@DepartementName, [Factor1HrDaily]=@Factor1HrDaily, [Factor2HrDaily]=@Factor2HrDaily, [Factor3SrDaily]=@Factor3SrDaily, [FormToolInsert]=@FormToolInsert, [HallId]=@HallId, [HallName]=@HallName, [HrHardResourcesNumber]=@HrHardResourcesNumber, [IsArchived]=@IsArchived, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OperationId]=@OperationId, [OperationName]=@OperationName, [PlanCapacity]=@PlanCapacity, [ProductivityHrDaily]=@ProductivityHrDaily, [ProductivitySrDaily]=@ProductivitySrDaily, [RequiredEmployees]=@RequiredEmployees, [ShiftsNumberWeekly]=@ShiftsNumberWeekly, [SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily, [SpecialHoursWeekly]=@SpecialHoursWeekly, [SpecialShiftsWeekly]=@SpecialShiftsWeekly, [ValidationLevel]=@ValidationLevel, [ValidationTime]=@ValidationTime, [ValidationUserId]=@ValidationUserId, [Version]=@Version, [WeekFirstDay]=@WeekFirstDay, [WeekLastDay]=@WeekLastDay, [WeekNumber]=@WeekNumber, [WorkAreaId]=@WorkAreaId, [WorkAreaName]=@WorkAreaName, [WorkingHoursPerShift]=@WorkingHoursPerShift, [WorkStationId]=@WorkStationId, [WorkStationName]=@WorkStationName, [Year]=@Year WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("Id", item.Id);
				sqlCommand.Parameters.AddWithValue("ArchiveTime", item.ArchiveTime == null ? (object)DBNull.Value : item.ArchiveTime);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", item.ArchiveUserId == null ? (object)DBNull.Value : item.ArchiveUserId);
				sqlCommand.Parameters.AddWithValue("Attendance", item.Attendance);
				sqlCommand.Parameters.AddWithValue("AvailableHrDaily", item.AvailableHrDaily);
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
				sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber", item.HrHardResourcesNumber);
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
				sqlCommand.Parameters.AddWithValue("ValidationLevel", item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
				sqlCommand.Parameters.AddWithValue("ValidationTime", item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
				sqlCommand.Parameters.AddWithValue("ValidationUserId", item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
				sqlCommand.Parameters.AddWithValue("Version", item.Version);
				sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
				sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);
				sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
				sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
				sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
				sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift", item.WorkingHoursPerShift);
				sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
				sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 45; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityPlanValidationEntity> items)
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
						query += " UPDATE [__MTM_CRP_CapacityPlan_Validation] SET "

							+ "[ArchiveTime]=@ArchiveTime" + i + ","
							+ "[ArchiveUserId]=@ArchiveUserId" + i + ","
							+ "[Attendance]=@Attendance" + i + ","
							+ "[AvailableHrDaily]=@AvailableHrDaily" + i + ","
							+ "[AvailableSrDaily]=@AvailableSrDaily" + i + ","
							+ "[CountryId]=@CountryId" + i + ","
							+ "[CountryName]=@CountryName" + i + ","
							+ "[CreationTime]=@CreationTime" + i + ","
							+ "[CreationUserId]=@CreationUserId" + i + ","
							+ "[DepartementId]=@DepartementId" + i + ","
							+ "[DepartementName]=@DepartementName" + i + ","
							+ "[Factor1HrDaily]=@Factor1HrDaily" + i + ","
							+ "[Factor2HrDaily]=@Factor2HrDaily" + i + ","
							+ "[Factor3SrDaily]=@Factor3SrDaily" + i + ","
							+ "[FormToolInsert]=@FormToolInsert" + i + ","
							+ "[HallId]=@HallId" + i + ","
							+ "[HallName]=@HallName" + i + ","
							+ "[HrHardResourcesNumber]=@HrHardResourcesNumber" + i + ","
							+ "[IsArchived]=@IsArchived" + i + ","
							+ "[LastUpdateTime]=@LastUpdateTime" + i + ","
							+ "[LastUpdateUserId]=@LastUpdateUserId" + i + ","
							+ "[OperationId]=@OperationId" + i + ","
							+ "[OperationName]=@OperationName" + i + ","
							+ "[PlanCapacity]=@PlanCapacity" + i + ","
							+ "[ProductivityHrDaily]=@ProductivityHrDaily" + i + ","
							+ "[ProductivitySrDaily]=@ProductivitySrDaily" + i + ","
							+ "[RequiredEmployees]=@RequiredEmployees" + i + ","
							+ "[ShiftsNumberWeekly]=@ShiftsNumberWeekly" + i + ","
							+ "[SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily" + i + ","
							+ "[SpecialHoursWeekly]=@SpecialHoursWeekly" + i + ","
							+ "[SpecialShiftsWeekly]=@SpecialShiftsWeekly" + i + ","
							+ "[ValidationLevel]=@ValidationLevel" + i + ","
							+ "[ValidationTime]=@ValidationTime" + i + ","
							+ "[ValidationUserId]=@ValidationUserId" + i + ","
							+ "[Version]=@Version" + i + ","
							+ "[WeekFirstDay]=@WeekFirstDay" + i + ","
							+ "[WeekLastDay]=@WeekLastDay" + i + ","
							+ "[WeekNumber]=@WeekNumber" + i + ","
							+ "[WorkAreaId]=@WorkAreaId" + i + ","
							+ "[WorkAreaName]=@WorkAreaName" + i + ","
							+ "[WorkingHoursPerShift]=@WorkingHoursPerShift" + i + ","
							+ "[WorkStationId]=@WorkStationId" + i + ","
							+ "[WorkStationName]=@WorkStationName" + i + ","
							+ "[Year]=@Year" + i + " WHERE [Id]=@Id" + i
							+ "; ";

						sqlCommand.Parameters.AddWithValue("Id" + i, item.Id);
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
						sqlCommand.Parameters.AddWithValue("HrHardResourcesNumber" + i, item.HrHardResourcesNumber);
						sqlCommand.Parameters.AddWithValue("IsArchived" + i, item.IsArchived);
						sqlCommand.Parameters.AddWithValue("LastUpdateTime" + i, item.LastUpdateTime == null ? (object)DBNull.Value : item.LastUpdateTime);
						sqlCommand.Parameters.AddWithValue("LastUpdateUserId" + i, item.LastUpdateUserId == null ? (object)DBNull.Value : item.LastUpdateUserId);
						sqlCommand.Parameters.AddWithValue("OperationId" + i, item.OperationId);
						sqlCommand.Parameters.AddWithValue("OperationName" + i, item.OperationName);
						sqlCommand.Parameters.AddWithValue("PlanCapacity" + i, item.PlanCapacity);
						sqlCommand.Parameters.AddWithValue("ProductivityHrDaily" + i, item.ProductivityHrDaily);
						sqlCommand.Parameters.AddWithValue("ProductivitySrDaily" + i, item.ProductivitySrDaily);
						sqlCommand.Parameters.AddWithValue("RequiredEmployees" + i, item.RequiredEmployees);
						sqlCommand.Parameters.AddWithValue("ShiftsNumberWeekly" + i, item.ShiftsNumberWeekly);
						sqlCommand.Parameters.AddWithValue("SoftRessourcesNumberDaily" + i, item.SoftRessourcesNumberDaily);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("ValidationLevel" + i, item.ValidationLevel == null ? (object)DBNull.Value : item.ValidationLevel);
						sqlCommand.Parameters.AddWithValue("ValidationTime" + i, item.ValidationTime == null ? (object)DBNull.Value : item.ValidationTime);
						sqlCommand.Parameters.AddWithValue("ValidationUserId" + i, item.ValidationUserId == null ? (object)DBNull.Value : item.ValidationUserId);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
						sqlCommand.Parameters.AddWithValue("WorkingHoursPerShift" + i, item.WorkingHoursPerShift);
						sqlCommand.Parameters.AddWithValue("WorkStationId" + i, item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
						sqlCommand.Parameters.AddWithValue("WorkStationName" + i, item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
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
				string query = "DELETE FROM [__MTM_CRP_CapacityPlan_Validation] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__MTM_CRP_CapacityPlan_Validation] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static int Delete(int year, int countryId, int hallId)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "DELETE FROM [__MTM_CRP_CapacityPlan_Validation] WHERE [Year]=@year AND [CountryId]=@countryId AND [HallId]=@hallId";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("year", year);
				sqlCommand.Parameters.AddWithValue("countryId", countryId);
				sqlCommand.Parameters.AddWithValue("hallId", hallId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}


		#endregion
	}
}
