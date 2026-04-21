namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlan
{
	public class CalculateItemModel
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

		public decimal ShiftsPerWeek { get; set; }
		public decimal SpecialShiftsPerWeek { get; set; }
		public decimal SpecialHours { get; set; }
		public decimal WorkingHoursPerShift { get; set; }

		public CalculateItemModel() { }
		public CalculateItemModel(Infrastructure.Data.Entities.Tables.MTM.CapacityPlanEntity capacityPlanEntity,
			int holidaysInWorkDays,
			int holidaysInWeekends)
		{
			#region > Fix inputs
			if(holidaysInWorkDays < 0)
			{
				holidaysInWorkDays = 0;
			}
			else if(holidaysInWorkDays > 5)
			{
				holidaysInWorkDays = 5;
			}

			if(holidaysInWeekends < 0)
			{
				holidaysInWeekends = 0;
			}
			else if(holidaysInWeekends > 2)
			{
				holidaysInWeekends = 2;
			}

			var normalShifts = capacityPlanEntity.ShiftsNumberWeekly - holidaysInWorkDays > 0
				? capacityPlanEntity.ShiftsNumberWeekly - holidaysInWorkDays
				: 0;
			var specialShifts = capacityPlanEntity.SpecialShiftsWeekly - holidaysInWeekends > 0
				? capacityPlanEntity.SpecialShiftsWeekly - holidaysInWeekends
				: 0;
			#endregion

			this.OperationId = capacityPlanEntity.OperationId;
			this.OperationName = capacityPlanEntity.OperationName;
			this.HallId = capacityPlanEntity.HallId;
			this.HallName = capacityPlanEntity.HallName;
			this.DepartementId = capacityPlanEntity.DepartementId;
			this.DepartementName = capacityPlanEntity.DepartementName;
			this.WorkAreaId = capacityPlanEntity.WorkAreaId;
			this.WorkAreaName = capacityPlanEntity.WorkAreaName;
			this.WorkStationId = capacityPlanEntity.WorkStationId;
			this.WorkStationName = capacityPlanEntity.WorkStationName;
			this.FormToolInsert = capacityPlanEntity.FormToolInsert;

			this.HrHardResourcesNumber = capacityPlanEntity.HrHardResourcesNumber;
			this.HrF1PersonPerMachine = capacityPlanEntity.Factor1HrDaily;
			this.HrF2UtilisationRate = capacityPlanEntity.Factor2HrDaily;
			this.HrProductivity = capacityPlanEntity.ProductivityHrDaily;

			this.SrSoftResourcesNumber = capacityPlanEntity.SoftRessourcesNumberDaily;
			this.SrF3AttendanceLevel = capacityPlanEntity.Factor3SrDaily;
			this.SrProductivity = capacityPlanEntity.ProductivitySrDaily;

			this.ShiftsPerWeek = normalShifts;
			this.SpecialShiftsPerWeek = specialShifts;
			this.SpecialHours = capacityPlanEntity.SpecialHoursWeekly;
			this.WorkingHoursPerShift = capacityPlanEntity.WorkingHoursPerShift;
		}
		public CalculateItemModel(Infrastructure.Data.Entities.Tables.MTM.CapacityEntity capacityEntity,
			int holidaysInWorkDays,
			int holidaysInWeekends)
		{
			#region > Fix inputs
			if(holidaysInWorkDays < 0)
			{
				holidaysInWorkDays = 0;
			}
			else if(holidaysInWorkDays > 5)
			{
				holidaysInWorkDays = 5;
			}

			if(holidaysInWeekends < 0)
			{
				holidaysInWeekends = 0;
			}
			else if(holidaysInWeekends > 2)
			{
				holidaysInWeekends = 2;
			}

			var normalShifts = capacityEntity.ShiftsNumberWeekly * (1 - (decimal)holidaysInWorkDays / 5); // - Khelil 2021-08-23
			var specialShifts = capacityEntity.SpecialShiftsWeekly - holidaysInWeekends > 0
				? capacityEntity.SpecialShiftsWeekly - holidaysInWeekends
				: 0; // - Khelil 2021-08-23, awaiting simulation
			#endregion

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

			this.ShiftsPerWeek = normalShifts; //  capacityEntity.ShiftsNumberWeekly;
			this.SpecialShiftsPerWeek = specialShifts; //  capacityEntity.SpecialShiftsWeekly;
			this.SpecialHours = capacityEntity.SpecialHoursWeekly;
			this.WorkingHoursPerShift = capacityEntity.WorkingHoursPerShift;
		}
		public CalculateItemModel(Infrastructure.Data.Entities.Tables.MTM.CapacityEntity capacityEntity)
		{
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

			this.ShiftsPerWeek = capacityEntity.ShiftsNumberWeekly;
			this.SpecialShiftsPerWeek = capacityEntity.SpecialShiftsWeekly;
			this.SpecialHours = capacityEntity.SpecialHoursWeekly;
			this.WorkingHoursPerShift = capacityEntity.WorkingHoursPerShift;
		}

		public List<string> Validate()
		{
			var errors = new List<string>();

			if(!this.get_HrF1PersonPerMachine_Values().Contains(this.HrF1PersonPerMachine))
			{
				errors.Add("Hr F1 Person Per Machine: is not valid value");
			}
			if(!this.get_HrF2UtilisationRate_Values().Contains(this.HrF2UtilisationRate))
			{
				errors.Add("Hr F2 Utilisation Rate: is not valid value");
			}
			if(!this.get_HrProductivity_Values().Contains(this.HrProductivity))
			{
				errors.Add("Hr Productivity: is not valid value");
			}

			if(!this.get_SrF3AttendanceLevel_Values().Contains(this.SrF3AttendanceLevel))
			{
				errors.Add("Sr F3 Attendance Level: is not valid value");
			}
			if(!this.get_SrProductivity_Values().Contains(this.SrProductivity))
			{
				errors.Add("Sr Productivity: is not valid value");
			}

			if(!this.get_ShiftsPerWeek_Values().Contains(this.ShiftsPerWeek))
			{
				errors.Add("ShiftsPerWeek: is not valid value");
			}
			if(!this.get_SpecialShiftsPerWeek_Values().Contains(this.SpecialShiftsPerWeek))
			{
				errors.Add("Special Shifts Per Week: is not valid value");
			}
			if(!this.get_SpecialHours_Values().Contains(this.SpecialHours))
			{
				errors.Add("Special Hours: is not valid value");
			}
			if(this.WorkingHoursPerShift <= 0)
			{
				errors.Add("Working Hours Per Shift: is not valid value");
			}


			if((this.HrF1PersonPerMachine <= 0 || this.HrF2UtilisationRate <= 0 || this.HrHardResourcesNumber <= 0)
				&& (this.SrSoftResourcesNumber <= 0 || this.SrProductivity <= 0 || this.SrF3AttendanceLevel <= 0))
			{
				errors.Add("Either Hr or Sr values must be positive");
			}
			return errors;
		}

		private List<decimal> get_HrF1PersonPerMachine_Values()
		{
			var response = new List<decimal>();

			var value = 0m;
			while(value <= 3.00m)
			{
				response.Add(value);
				value += 0.25m;
			}

			return response;
		}
		private List<decimal> get_HrF2UtilisationRate_Values()
		{
			var response = new List<decimal>();

			var value = 0m;
			while(value <= 1.00m)
			{
				response.Add(value);
				value += 0.05m;
			}

			return response;
		}
		private List<decimal> get_HrProductivity_Values()
		{
			var response = new List<decimal>();

			response.Add(0m);
			var value = 0.20m;
			while(value <= 1.00m)
			{
				response.Add(value);
				value += 0.01m;
			}

			return response;
		}
		private List<decimal> get_SrF3AttendanceLevel_Values()
		{
			var response = new List<decimal>();

			response.Add(0m);
			var value = 0.50m;
			while(value <= 1.00m)
			{
				response.Add(value);
				value += 0.05m;
			}

			return response;
		}
		private List<decimal> get_SrProductivity_Values()
		{
			var response = new List<decimal>();

			response.Add(0m);
			var value = 0.20m;
			while(value <= 1.00m)
			{
				response.Add(value);
				value += 0.01m;
			}

			return response;
		}
		private List<decimal> get_ShiftsPerWeek_Values()
		{
			var response = new List<decimal>();

			var value = 0;
			while(value <= 15)
			{
				response.Add(value);
				value++;
			}

			return response;
		}
		private List<decimal> get_SpecialShiftsPerWeek_Values()
		{
			var response = new List<decimal>();

			var value = 0;
			while(value <= 6)
			{
				response.Add(value);
				value++;
			}

			return response;
		}
		private List<decimal> get_SpecialHours_Values()
		{
			var response = new List<decimal>();

			var value = 0m;
			while(value <= 28)
			{
				response.Add(value);
				value += 0.5m;
			}

			return response;
		}
	}
}
