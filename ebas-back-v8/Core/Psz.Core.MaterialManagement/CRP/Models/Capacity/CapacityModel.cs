using Infrastructure.Data.Entities.Tables.MTM;

namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class CapacityModel
	{
		public int Id { get; set; }

		public int OperationId { get; set; }
		public string OperationName { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int DepartementId { get; set; }
		public string DepartementName { get; set; }
		public int WorkAreaId { get; set; }
		public string WorkAreaName { get; set; }
		public int? WorkStationId { get; set; }
		public string WorkStationName { get; set; }
		public string FormToolInsert { get; set; }

		public decimal HrHardResourcesNumber { get; set; }
		public decimal HrF1PersonPerMachine { get; set; }
		public decimal HrF2UtilisationRate { get; set; }
		public decimal HrProductivity { get; set; }

		public decimal SrSoftResourcesNumber { get; set; }
		public decimal SrF3AttendanceLevel { get; set; }
		public decimal SrProductivity { get; set; }

		public decimal AvailableHr { get; set; }
		public decimal AvailableSr { get; set; }

		public decimal ShiftsPerWeek { get; set; }
		public decimal SpecialShiftsPerWeek { get; set; }
		public decimal SpecialHours { get; set; }

		public decimal AttendancePerWeek { get; set; }
		public decimal PlannableCapacites { get; set; }
		public decimal RequiredEmployeesNumber { get; set; }
		public decimal WorkingHoursPerShift { get; set; }
		public int WeekNumber { get; set; }
		public int Year { get; set; }
		public int CountryId { get; set; }

		// -
		public bool CanEdit { get; set; } = false;
		public bool CanDelete { get; set; } = false;
		public CapacityModel(CapacityEntity capacityEntity)
		{
			this.Id = capacityEntity.Id;

			this.OperationId = capacityEntity.OperationId;
			this.OperationName = capacityEntity.OperationName;
			this.HallId = capacityEntity.HallId;
			this.HallName = capacityEntity.HallName;
			this.DepartementId = capacityEntity.DepartementId;
			this.DepartementName = capacityEntity.DepartementName;
			this.WorkAreaId = capacityEntity.WorkAreaId;
			this.WorkAreaName = capacityEntity.WorkAreaName;
			this.WorkStationId = capacityEntity.WorkStationId;
			this.WorkStationName = capacityEntity.WorkStationName;
			this.FormToolInsert = capacityEntity.FormToolInsert;

			this.HrHardResourcesNumber = capacityEntity.HrHardResourcesNumber;
			this.HrF1PersonPerMachine = capacityEntity.Factor1HrDaily;
			this.HrF2UtilisationRate = capacityEntity.Factor2HrDaily;
			this.HrProductivity = capacityEntity.ProductivityHrDaily;

			this.SrSoftResourcesNumber = capacityEntity.SoftRessourcesNumberDaily;
			this.SrF3AttendanceLevel = capacityEntity.Factor3SrDaily;
			this.SrProductivity = capacityEntity.ProductivitySrDaily;

			this.AvailableHr = capacityEntity.AvailableHrDaily;
			this.AvailableSr = capacityEntity.AvailableSrDaily;

			this.ShiftsPerWeek = capacityEntity.ShiftsNumberWeekly;
			this.SpecialShiftsPerWeek = capacityEntity.SpecialShiftsWeekly;
			this.SpecialHours = capacityEntity.SpecialHoursWeekly;

			this.AttendancePerWeek = capacityEntity.Attendance;
			this.PlannableCapacites = capacityEntity.PlanCapacity;
			this.RequiredEmployeesNumber = capacityEntity.RequiredEmployees;
			this.WorkingHoursPerShift = capacityEntity.WorkingHoursPerShift;
			this.WeekNumber = capacityEntity.WeekNumber;

			this.Year = capacityEntity.Year;
			this.CountryId = capacityEntity.CountryId;
		}
	}
}
