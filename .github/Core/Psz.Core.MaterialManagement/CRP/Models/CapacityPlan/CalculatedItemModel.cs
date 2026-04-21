namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class CalculatedItemModel
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
		public decimal HrF1PersonPerMachine { get; set; } // [0,25 : 3,00] +0,25		
		public decimal HrF2UtilisationRate { get; set; } // [0,50 : 1,00] +0,05		
		public decimal HrProductivity { get; set; } // [0,20 : 1,00] +0,01		

		public decimal SrSoftResourcesNumber { get; set; }
		public decimal SrF3AttendanceLevel { get; set; } // Dropdown-list [0,50 : 1,00] +0,05		
		public decimal SrProductivity { get; set; } // Dropdown-list [0,20 : 1,00] +0,01		

		public decimal AvailableHr { get; set; } // HrHardResourcesNumber * HrF2UtilisationRate * HrProductivity
		public decimal AvailableSr { get; set; } // SrSoftResourcesNumber * SrF3AttendanceLevel * SrProductivity

		public decimal ShiftsPerWeek { get; set; } // Dropdown-list [0:15] +1		
		public decimal SpecialShiftsPerWeek { get; set; } //Dropdown-list[0:5] +1	
		public decimal SpecialHours { get; set; } // Dropdown-list [0:28] +1	
		public decimal WorkingHoursPerShift { get; set; } // Dropdown-list [0:12] +0.25	

		public decimal AttendancePerWeek { get; set; } // (ShiftsPerWeek * 8) + (SpecialShiftsPerWeek * 8) + SpecialHours
		public decimal PlannableCapacites { get; set; } // (AvailableHr * AttendancePerWeek) + (AvailableSr * AttendancePerWeek)
		public decimal RequiredEmployeesNumber { get; set; }
		// >> =SI(ShiftsPerWeek<1;0;SI(ShiftsPerWeek<=5;(HrHardResourcesNumber*HrF1PersonPerMachine)+SrSoftResourcesNumber;SI(ShiftsPerWeek<=10;((HrHardResourcesNumber*HrF1PersonPerMachine)+SrSoftResourcesNumber)*2;SI(ShiftsPerWeek<=15;((HrHardResourcesNumber*HrF1PersonPerMachine)+SrSoftResourcesNumber)*3))))
		// >> if (ShiftsPerWeek<1)
		//{
		//    response = 0;
		//}
		//else if (ShiftsPerWeek<=5)
		//{
		//    response = (HrHardResourcesNumber* HrF1PersonPerMachine)+SrSoftResourcesNumber;
		//}
		//else if (ShiftsPerWeek<=10)
		//{
		//    response = (HrHardResourcesNumber* HrF1PersonPerMachine)+SrSoftResourcesNumber)*2;
		//}
		//else if (ShiftsPerWeek <= 15)
		//{
		//    response = ((HrHardResourcesNumber * HrF1PersonPerMachine) + SrSoftResourcesNumber) * 3)
		//}

		public Infrastructure.Data.Entities.Tables.MTM.CapacityEntity ToCapacity(Infrastructure.Data.Entities.Tables.MTM.CapacityEntity capacityEntity)
		{
			return new Infrastructure.Data.Entities.Tables.MTM.CapacityEntity
			{
				Id = -1,

				Year = capacityEntity.Year,
				WeekNumber = capacityEntity.WeekNumber,
				WeekFirstDay = capacityEntity.WeekFirstDay,
				WeekLastDay = capacityEntity.WeekLastDay,

				OperationId = this.OperationId,
				OperationName = capacityEntity.OperationName,
				CountryId = capacityEntity.CountryId,
				CountryName = capacityEntity.CountryName,
				HallId = this.HallId,
				HallName = capacityEntity.HallName,
				DepartementId = this.DepartementId,
				DepartementName = capacityEntity.DepartementName,
				WorkAreaId = this.WorkAreaId,
				WorkAreaName = capacityEntity.WorkAreaName,
				WorkStationId = this.WorkStationId,
				WorkStationName = capacityEntity.WorkStationName,
				FormToolInsert = this.FormToolInsert,

				HrHardResourcesNumber = this.HrHardResourcesNumber,
				Factor1HrDaily = this.HrF1PersonPerMachine,
				Factor2HrDaily = this.HrF2UtilisationRate,
				ProductivityHrDaily = this.HrProductivity,

				SoftRessourcesNumberDaily = this.SrSoftResourcesNumber,
				Factor3SrDaily = this.SrF3AttendanceLevel,
				ProductivitySrDaily = this.SrProductivity,

				AvailableHrDaily = this.AvailableHr,
				AvailableSrDaily = this.AvailableSr,

				ShiftsNumberWeekly = this.ShiftsPerWeek,
				SpecialShiftsWeekly = this.SpecialShiftsPerWeek,
				SpecialHoursWeekly = this.SpecialHours,

				Attendance = this.AttendancePerWeek,
				PlanCapacity = this.PlannableCapacites,
				RequiredEmployees = this.RequiredEmployeesNumber,
				WorkingHoursPerShift = capacityEntity.WorkingHoursPerShift,

				CreationTime = capacityEntity.CreationTime,
				CreationUserId = capacityEntity.CreationUserId,
				Version = (capacityEntity?.Version ?? 0),
				LastUpdateTime = capacityEntity.LastUpdateTime,
				LastUpdateUserId = capacityEntity.LastUpdateUserId,
				IsArchived = capacityEntity.IsArchived,
				ArchiveTime = capacityEntity.ArchiveTime,
				ArchiveUserId = capacityEntity.ArchiveUserId,
			};
		}
	}
}
