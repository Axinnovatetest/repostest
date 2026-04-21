using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class CapacityPlanEntity
	{
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public decimal Attendance { get; set; }
		public decimal AvailableSrDaily { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int DepartementId { get; set; }
		public string DepartementName { get; set; }
		public decimal Factor1HrDaily { get; set; }
		public decimal Factor2HrDaily { get; set; }
		public decimal Factor3SrDaily { get; set; }
		public string FormToolInsert { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public int OperationId { get; set; }
		public string OperationName { get; set; }
		public decimal PlanCapacity { get; set; }
		public decimal ProductivityHrDaily { get; set; }
		public decimal ProductivitySrDaily { get; set; }
		public decimal RequiredEmployees { get; set; }
		public decimal SoftRessourcesNumberDaily { get; set; }
		public int Version { get; set; }
		public int WeekNumber { get; set; }
		public int WorkAreaId { get; set; }
		public string WorkAreaName { get; set; }
		public int Year { get; set; }

		public int? WorkStationId { get; set; }
		public string WorkStationName { get; set; }
		public DateTime WeekFirstDay { get; set; }
		public DateTime WeekLastDay { get; set; }

		public decimal WorkingHoursPerShift { get; set; }
		public decimal ShiftsNumberWeekly { get; set; } /* TYPE CHANGED */
		public decimal SpecialHoursWeekly { get; set; } /* TYPE CHANGED */
		public decimal SpecialShiftsWeekly { get; set; } /* TYPE CHANGED */

		public decimal HrHardResourcesNumber { get; set; } /* NEW */
		public decimal AvailableHrDaily { get; set; } /* NEW */

		public CapacityPlanEntity() { }
		public CapacityPlanEntity(DataRow dataRow)
		{
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			Attendance = Convert.ToDecimal(dataRow["Attendance"]);
			AvailableSrDaily = Convert.ToDecimal(dataRow["AvailableSrDaily"]);
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = Convert.ToString(dataRow["CountryName"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			DepartementId = Convert.ToInt32(dataRow["DepartementId"]);
			DepartementName = Convert.ToString(dataRow["DepartementName"]);
			Factor1HrDaily = Convert.ToDecimal(dataRow["Factor1HrDaily"]);
			Factor2HrDaily = Convert.ToDecimal(dataRow["Factor2HrDaily"]);
			Factor3SrDaily = Convert.ToDecimal(dataRow["Factor3SrDaily"]);
			FormToolInsert = (dataRow["FormToolInsert"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["FormToolInsert"]);
			HallId = Convert.ToInt32(dataRow["HallId"]);
			HallName = Convert.ToString(dataRow["HallName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["IsArchived"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			OperationId = Convert.ToInt32(dataRow["OperationId"]);
			OperationName = Convert.ToString(dataRow["OperationName"]);
			PlanCapacity = Convert.ToDecimal(dataRow["PlanCapacity"]);
			ProductivityHrDaily = Convert.ToDecimal(dataRow["ProductivityHrDaily"]);
			ProductivitySrDaily = Convert.ToDecimal(dataRow["ProductivitySrDaily"]);
			RequiredEmployees = Convert.ToDecimal(dataRow["RequiredEmployees"]);
			SoftRessourcesNumberDaily = Convert.ToDecimal(dataRow["SoftRessourcesNumberDaily"]);
			Version = Convert.ToInt32(dataRow["Version"]);
			WeekNumber = Convert.ToInt32(dataRow["WeekNumber"]);
			WorkAreaId = Convert.ToInt32(dataRow["WorkAreaId"]);
			WorkAreaName = Convert.ToString(dataRow["WorkAreaName"]);
			Year = Convert.ToInt32(dataRow["Year"]);

			WorkStationId = (dataRow["WorkStationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WorkStationId"]);
			WorkStationName = (dataRow["WorkStationName"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["WorkStationName"]);
			WeekFirstDay = Convert.ToDateTime(dataRow["WeekFirstDay"]);
			WeekLastDay = Convert.ToDateTime(dataRow["WeekLastDay"]);

			WorkingHoursPerShift = Convert.ToDecimal(dataRow["WorkingHoursPerShift"]);
			ShiftsNumberWeekly = Convert.ToDecimal(dataRow["ShiftsNumberWeekly"]);
			SpecialHoursWeekly = Convert.ToDecimal(dataRow["SpecialHoursWeekly"]);
			SpecialShiftsWeekly = Convert.ToDecimal(dataRow["SpecialShiftsWeekly"]);

			HrHardResourcesNumber = Convert.ToDecimal(dataRow["HrHardResourcesNumber"]);
			AvailableHrDaily = Convert.ToDecimal(dataRow["AvailableHrDaily"]);
		}
	}
}

