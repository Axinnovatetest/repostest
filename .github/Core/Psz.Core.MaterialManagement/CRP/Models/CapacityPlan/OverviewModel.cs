namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class OverviewModel
	{
		public int CreateuserId { get; set; }
		public string CreateUserName { get; set; }
		public DateTime? CreateTime { get; set; }
		public bool CanEdit { get; set; }
		public bool CanValidate { get; set; }
		public bool CanUnValidate { get; set; }
		public bool CanReject { get; set; }
		public int Level { get; set; }
		public List<WeeklyPlan> Data { get; set; } = new List<WeeklyPlan>();

		public enum ItemTypes: int
		{
			NotChanged = 0,
			Changed = 1,
			New = 2,
			Deleted = 3,
		}

		public class WeeklyPlan
		{
			public int FirstWeekNumber { get; set; }
			public int LastWeekNumber { get; set; }
			public List<Item> Items { get; set; } = new List<Item>();
		}

		public class Item
		{
			public int Id { get; set; }
			public ItemTypes Type { get; set; }

			public int OperationId { get; set; }
			public string OperationName { get; set; }
			public int HallId { get; set; }
			public string HallName { get; set; }
			public int DepartementId { get; set; }
			public string DepartementName { get; set; }
			public int? WorkAreaId { get; set; }
			public string WorkAreaName { get; set; }
			public int? WorkStationId { get; set; }
			public string WorkStationName { get; set; }
			public string FormToolInsert { get; set; }
			//
			public decimal HrHardResourcesNumber { get; set; }
			public decimal HrF1PersonPerMachine { get; set; }
			public decimal HrF2UtilisationRate { get; set; }
			public decimal HrProductivity { get; set; }
			//
			public decimal SrSoftResourcesNumber { get; set; }
			public decimal SrF3AttendanceLevel { get; set; }
			public decimal SrProductivity { get; set; }
			//
			public decimal AvailableHr { get; set; }
			public decimal AvailableSr { get; set; }
			//
			public decimal ShiftsPerWeek { get; set; }
			public decimal SpecialShiftsPerWeek { get; set; }
			public decimal SpecialHours { get; set; }
			public decimal WorkingHoursPerShift { get; set; }
			//
			public decimal AttendancePerWeek { get; set; }
			public decimal PlannableCapacites { get; set; }
			public decimal RequiredEmployeesNumber { get; set; }

			public bool OperationId_Changed { get; set; }
			public bool OperationName_Changed { get; set; }
			public bool HallId_Changed { get; set; }
			public bool HallName_Changed { get; set; }
			public bool DepartementId_Changed { get; set; }
			public bool DepartementName_Changed { get; set; }
			public bool WorkAreaId_Changed { get; set; }
			public bool WorkAreaName_Changed { get; set; }
			public bool WorkStationId_Changed { get; set; }
			public bool WorkStationName_Changed { get; set; }
			public bool FormToolInsert_Changed { get; set; }
			//
			public bool HrHardResourcesNumber_Changed { get; set; }
			public bool HrF1PersonPerMachine_Changed { get; set; }
			public bool HrF2UtilisationRate_Changed { get; set; }
			public bool HrProductivity_Changed { get; set; }
			//
			public bool SrSoftResourcesNumber_Changed { get; set; }
			public bool SrF3AttendanceLevel_Changed { get; set; }
			public bool SrProductivity_Changed { get; set; }
			//
			public bool AvailableHr_Changed { get; set; }
			public bool AvailableSr_Changed { get; set; }
			//
			public bool ShiftsPerWeek_Changed { get; set; }
			public bool SpecialShiftsPerWeek_Changed { get; set; }
			public bool SpecialHours_Changed { get; set; }
			public bool WorkingHoursPerShift_Changed { get; set; }

			//
			public bool AttendancePerWeek_Changed { get; set; }
			public bool PlannableCapacites_Changed { get; set; }
			public bool RequiredEmployeesNumber_Changed { get; set; }
		}
	}
}
