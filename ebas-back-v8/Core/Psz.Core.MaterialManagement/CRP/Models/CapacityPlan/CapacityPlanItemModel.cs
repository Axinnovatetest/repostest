using Infrastructure.Data.Entities.Tables.MTM;

namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class CapacityPlanItemModel
	{
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

		public CapacityPlanItemModel() { }
		public CapacityPlanItemModel(CapacityPlanEntity capacityPlanEntity)
		{
			OperationId = capacityPlanEntity.OperationId;
			OperationName = capacityPlanEntity.OperationName;
			HallId = capacityPlanEntity.HallId;
			HallName = capacityPlanEntity.HallName;
			DepartementId = capacityPlanEntity.DepartementId;
			DepartementName = capacityPlanEntity.DepartementName;
			WorkAreaId = capacityPlanEntity.WorkAreaId;
			WorkAreaName = capacityPlanEntity.WorkAreaName;
			WorkStationId = capacityPlanEntity.WorkStationId;
			WorkStationName = capacityPlanEntity.WorkStationName;
			FormToolInsert = capacityPlanEntity.FormToolInsert;

			HrHardResourcesNumber = capacityPlanEntity.HrHardResourcesNumber;
			HrF1PersonPerMachine = capacityPlanEntity.Factor1HrDaily;
			HrF2UtilisationRate = capacityPlanEntity.Factor2HrDaily;
			HrProductivity = capacityPlanEntity.ProductivityHrDaily;

			SrSoftResourcesNumber = capacityPlanEntity.SoftRessourcesNumberDaily;
			SrF3AttendanceLevel = capacityPlanEntity.Factor3SrDaily;
			SrProductivity = capacityPlanEntity.ProductivitySrDaily;

			AvailableHr = capacityPlanEntity.AvailableHrDaily;
			AvailableSr = capacityPlanEntity.AvailableSrDaily;

			ShiftsPerWeek = capacityPlanEntity.ShiftsNumberWeekly;
			SpecialShiftsPerWeek = capacityPlanEntity.SpecialShiftsWeekly;
			SpecialHours = capacityPlanEntity.SpecialHoursWeekly;

			AttendancePerWeek = capacityPlanEntity.Attendance;
			PlannableCapacites = capacityPlanEntity.PlanCapacity;
			RequiredEmployeesNumber = capacityPlanEntity.RequiredEmployees;
		}
	}
}
