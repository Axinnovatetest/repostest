using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Data.Access.Tables.MTM
{
	public class CapacityRequiredAccess
	{
		#region Default Methods
		public static Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity Get(int id)
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityRequired] WHERE [Id]=@Id";
				var sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.Parameters.AddWithValue("Id", id);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);

			}

			if(dataTable.Rows.Count > 0)
			{
				return new Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity(dataTable.Rows[0]);
			}
			else
			{
				return null;
			}
		}

		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> Get()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT * FROM [__MTM_CRP_CapacityRequired]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> Get(List<int> ids)
		{
			if(ids != null && ids.Count > 0)
			{
				int maxQueryNumber = Settings.MAX_BATCH_SIZE;
				List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> results = null;
				if(ids.Count <= maxQueryNumber)
				{
					results = get(ids);
				}
				else
				{
					int batchNumber = ids.Count / maxQueryNumber;
					results = new List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity>();
					for(int i = 0; i < batchNumber; i++)
					{
						results.AddRange(get(ids.GetRange(i * maxQueryNumber, maxQueryNumber)));
					}
					results.AddRange(get(ids.GetRange(batchNumber * maxQueryNumber, ids.Count - batchNumber * maxQueryNumber)));
				}
				return results;
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity>();
		}
		private static List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> get(List<int> ids)
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

					sqlCommand.CommandText = $"SELECT * FROM [__MTM_CRP_CapacityRequired] WHERE [Id] IN ({queryIds})";
					new SqlDataAdapter(sqlCommand).Fill(dataTable);
				}

				if(dataTable.Rows.Count > 0)
				{
					return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity(x)).ToList();
				}
				else
				{
					return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity>();
				}
			}
			return new List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity>();
		}

		public static int Insert(Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity item)
		{
			int response = int.MinValue;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				var sqlTransaction = sqlConnection.BeginTransaction();

				string query = "INSERT INTO [__MTM_CRP_CapacityRequired] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SourceId],[SpecialHoursWeekly],[SpecialShiftsWeekly],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkStationId],[WorkStationName],[Year])  VALUES (@ArchiveTime,@ArchiveUserId,@Attendance,@AvailableHrDaily,@AvailableSrDaily,@CountryId,@CountryName,@CreationTime,@CreationUserId,@DepartementId,@DepartementName,@Factor1HrDaily,@Factor2HrDaily,@Factor3SrDaily,@FormToolInsert,@HallId,@HallName,@HrHardResourcesNumber,@IsArchived,@LastUpdateTime,@LastUpdateUserId,@OperationId,@OperationName,@PlanCapacity,@ProductivityHrDaily,@ProductivitySrDaily,@RequiredEmployees,@ShiftsNumberWeekly,@SoftRessourcesNumberDaily,@SourceId,@SpecialHoursWeekly,@SpecialShiftsWeekly,@Version,@WeekFirstDay,@WeekLastDay,@WeekNumber,@WorkAreaId,@WorkAreaName,@WorkStationId,@WorkStationName,@Year); ";
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
					sqlCommand.Parameters.AddWithValue("SourceId", item.SourceId);
					sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", item.SpecialHoursWeekly);
					sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", item.SpecialShiftsWeekly);
					sqlCommand.Parameters.AddWithValue("Version", item.Version);
					sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
					sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);
					sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
					sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
					sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
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
		public static int Insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int insert(List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> items)
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
						query += " INSERT INTO [__MTM_CRP_CapacityRequired] ([ArchiveTime],[ArchiveUserId],[Attendance],[AvailableHrDaily],[AvailableSrDaily],[CountryId],[CountryName],[CreationTime],[CreationUserId],[DepartementId],[DepartementName],[Factor1HrDaily],[Factor2HrDaily],[Factor3SrDaily],[FormToolInsert],[HallId],[HallName],[HrHardResourcesNumber],[IsArchived],[LastUpdateTime],[LastUpdateUserId],[OperationId],[OperationName],[PlanCapacity],[ProductivityHrDaily],[ProductivitySrDaily],[RequiredEmployees],[ShiftsNumberWeekly],[SoftRessourcesNumberDaily],[SourceId],[SpecialHoursWeekly],[SpecialShiftsWeekly],[Version],[WeekFirstDay],[WeekLastDay],[WeekNumber],[WorkAreaId],[WorkAreaName],[WorkStationId],[WorkStationName],[Year]) VALUES ( "

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
							+ "@SourceId" + i + ","
							+ "@SpecialHoursWeekly" + i + ","
							+ "@SpecialShiftsWeekly" + i + ","
							+ "@Version" + i + ","
							+ "@WeekFirstDay" + i + ","
							+ "@WeekLastDay" + i + ","
							+ "@WeekNumber" + i + ","
							+ "@WorkAreaId" + i + ","
							+ "@WorkAreaName" + i + ","
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
						sqlCommand.Parameters.AddWithValue("SourceId" + i, item.SourceId);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
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

		public static int Update(Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity item)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "UPDATE [__MTM_CRP_CapacityRequired] SET [ArchiveTime]=@ArchiveTime, [ArchiveUserId]=@ArchiveUserId, [Attendance]=@Attendance, [AvailableHrDaily]=@AvailableHrDaily, [AvailableSrDaily]=@AvailableSrDaily, [CountryId]=@CountryId, [CountryName]=@CountryName, [CreationTime]=@CreationTime, [CreationUserId]=@CreationUserId, [DepartementId]=@DepartementId, [DepartementName]=@DepartementName, [Factor1HrDaily]=@Factor1HrDaily, [Factor2HrDaily]=@Factor2HrDaily, [Factor3SrDaily]=@Factor3SrDaily, [FormToolInsert]=@FormToolInsert, [HallId]=@HallId, [HallName]=@HallName, [HrHardResourcesNumber]=@HrHardResourcesNumber, [IsArchived]=@IsArchived, [LastUpdateTime]=@LastUpdateTime, [LastUpdateUserId]=@LastUpdateUserId, [OperationId]=@OperationId, [OperationName]=@OperationName, [PlanCapacity]=@PlanCapacity, [ProductivityHrDaily]=@ProductivityHrDaily, [ProductivitySrDaily]=@ProductivitySrDaily, [RequiredEmployees]=@RequiredEmployees, [ShiftsNumberWeekly]=@ShiftsNumberWeekly, [SoftRessourcesNumberDaily]=@SoftRessourcesNumberDaily, [SourceId]=@SourceId, [SpecialHoursWeekly]=@SpecialHoursWeekly, [SpecialShiftsWeekly]=@SpecialShiftsWeekly, [Version]=@Version, [WeekFirstDay]=@WeekFirstDay, [WeekLastDay]=@WeekLastDay, [WeekNumber]=@WeekNumber, [WorkAreaId]=@WorkAreaId, [WorkAreaName]=@WorkAreaName, [WorkStationId]=@WorkStationId, [WorkStationName]=@WorkStationName, [Year]=@Year WHERE [Id]=@Id";
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
				sqlCommand.Parameters.AddWithValue("SourceId", item.SourceId);
				sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly", item.SpecialHoursWeekly);
				sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly", item.SpecialShiftsWeekly);
				sqlCommand.Parameters.AddWithValue("Version", item.Version);
				sqlCommand.Parameters.AddWithValue("WeekFirstDay", item.WeekFirstDay);
				sqlCommand.Parameters.AddWithValue("WeekLastDay", item.WeekLastDay);
				sqlCommand.Parameters.AddWithValue("WeekNumber", item.WeekNumber);
				sqlCommand.Parameters.AddWithValue("WorkAreaId", item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
				sqlCommand.Parameters.AddWithValue("WorkAreaName", item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
				sqlCommand.Parameters.AddWithValue("WorkStationId", item.WorkStationId == null ? (object)DBNull.Value : item.WorkStationId);
				sqlCommand.Parameters.AddWithValue("WorkStationName", item.WorkStationName == null ? (object)DBNull.Value : item.WorkStationName);
				sqlCommand.Parameters.AddWithValue("Year", item.Year);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int Update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> items)
		{
			if(items != null && items.Count > 0)
			{
				int maxParamsNumber = Settings.MAX_BATCH_SIZE / 42; // Nb params per query
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
		private static int update(List<Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity> items)
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
						query += " UPDATE [__MTM_CRP_CapacityRequired] SET "

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
							+ "[SourceId]=@SourceId" + i + ","
							+ "[SpecialHoursWeekly]=@SpecialHoursWeekly" + i + ","
							+ "[SpecialShiftsWeekly]=@SpecialShiftsWeekly" + i + ","
							+ "[Version]=@Version" + i + ","
							+ "[WeekFirstDay]=@WeekFirstDay" + i + ","
							+ "[WeekLastDay]=@WeekLastDay" + i + ","
							+ "[WeekNumber]=@WeekNumber" + i + ","
							+ "[WorkAreaId]=@WorkAreaId" + i + ","
							+ "[WorkAreaName]=@WorkAreaName" + i + ","
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
						sqlCommand.Parameters.AddWithValue("SourceId" + i, item.SourceId);
						sqlCommand.Parameters.AddWithValue("SpecialHoursWeekly" + i, item.SpecialHoursWeekly);
						sqlCommand.Parameters.AddWithValue("SpecialShiftsWeekly" + i, item.SpecialShiftsWeekly);
						sqlCommand.Parameters.AddWithValue("Version" + i, item.Version);
						sqlCommand.Parameters.AddWithValue("WeekFirstDay" + i, item.WeekFirstDay);
						sqlCommand.Parameters.AddWithValue("WeekLastDay" + i, item.WeekLastDay);
						sqlCommand.Parameters.AddWithValue("WeekNumber" + i, item.WeekNumber);
						sqlCommand.Parameters.AddWithValue("WorkAreaId" + i, item.WorkAreaId == null ? (object)DBNull.Value : item.WorkAreaId);
						sqlCommand.Parameters.AddWithValue("WorkAreaName" + i, item.WorkAreaName == null ? (object)DBNull.Value : item.WorkAreaName);
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
				string query = "DELETE FROM [__MTM_CRP_CapacityRequired] WHERE [Id]=@Id";
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

					string query = "DELETE FROM [__MTM_CRP_CapacityRequired] WHERE [Id] IN (" + queryIds + ")";
					sqlCommand.CommandText = query;

					results = sqlCommand.ExecuteNonQuery();
				}

				return results;
			}
			return -1;
		}
		#endregion

		#region Custom Methods
		public static List<Entities.Tables.MTM.CapacityRequiredEntity> Get(
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

				string query = " SELECT * FROM [__MTM_CRP_CapacityRequired] "
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
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.MTM.CapacityRequiredEntity(x)).ToList();
			}
			else
			{
				return new List<Entities.Tables.MTM.CapacityRequiredEntity>();
			}
		}
		public static int ArchiveByFa(int userId, int faId, int version)
		{
			int results = -1;
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"UPDATE [__MTM_CRP_CapacityRequired] SET [ArchiveTime]=GetDate(), [ArchiveUserId]=@ArchiveUserId, IsArchived=1 WHERE [SourceId] = @faId AND [Version]=@version";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("faId", faId);
				sqlCommand.Parameters.AddWithValue("version", version);
				sqlCommand.Parameters.AddWithValue("ArchiveUserId", userId);

				results = sqlCommand.ExecuteNonQuery();
			}

			return results;
		}
		public static int GetVersionByFa(int faId)
		{
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = $"SELECT ISNULL(MAX([Version]),0) FROM [__MTM_CRP_CapacityRequired] WHERE [SourceId] = @faId AND ([IsArchived] IS NULL OR  [IsArchived]=0)";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				sqlCommand.Parameters.AddWithValue("faId", faId);

				return int.TryParse(sqlCommand.ExecuteScalar().ToString(), out var v) ? v : 0;
			}
		}
		public static List<int> GetFaIds()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Settings.ConnectionStringMTM))
			{
				sqlConnection.Open();
				string query = "SELECT DISTINCT SourceId FROM [__MTM_CRP_CapacityRequired]";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => int.TryParse(x[0].ToString(), out var id) ? id : -1).ToList();
			}
			else
			{
				return new List<int>();
			}
		}
		public static List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> GetNew()
		{
			var dataTable = new DataTable();
			using(var sqlConnection = new SqlConnection(Access.Settings.ConnectionString))
			{
				sqlConnection.Open();
				string query = $"SELECT * FROM Fertigung WHERE ID IN (select Distinct f.ID from dbo.fertigung f " +
					$"left join dbo.Artikel A ON A.[Artikel-Nr]=f.Artikel_Nr left join dbo.Article AA ON AA.[Name]=A.Artikelnummer left join dbo.WorkSchedule W ON W.Article_Id = AA.Id left join dbo.WorkScheduleDetails D ON D.WorkScheduleId=W.Id " +
					$"left join ErpMTM.dbo.__MTM_CRP_CapacityRequired c on f.id=c.SourceId " +
					$"where TRIM(Kennzeichen) = 'offen'  AND W.Is_Active = 1 AND D.Id IS NOT NULL AND c.id IS NULL) " +
					$"/*SELECT * FROM [Fertigung] WHERE TRIM(Kennzeichen) = 'offen' */";
				var sqlCommand = new SqlCommand(query, sqlConnection);

				new SqlDataAdapter(sqlCommand).Fill(dataTable);
			}

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity(x)).ToList();
			}
			else
			{
				return new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
			}
		}
		public static List<KeyValuePair<int, int>> GetIdsByIds(List<int> ids)
		{
			if(ids == null || ids.Count <= 0)
				return null;

			var sqlConnection = new SqlConnection(Infrastructure.Data.Access.Settings.ConnectionString);
			sqlConnection.Open();

			string query = $"SELECT ar.[Artikel-Nr], a.Id FROM DBO.Article a JOIN DBO.Artikel ar  ON a.Name=ar.Artikelnummer WHERE ar.[Artikel-Nr] IN ({string.Join(",", ids)})";

			var sqlCommand = new SqlCommand(query, sqlConnection);

			sqlCommand.Parameters.AddWithValue("IsArchived", false);

			var selectAdapter = new SqlDataAdapter(sqlCommand);

			sqlConnection.Close();

			var dataTable = new DataTable();

			selectAdapter.Fill(dataTable);

			if(dataTable.Rows.Count > 0)
			{
				return dataTable.Rows.Cast<DataRow>().Select(x => new KeyValuePair<int, int>((int)x[0], (int)x[1])).ToList();
			}
			else
			{
				return null;
			}
		}
		#endregion
	}
}
