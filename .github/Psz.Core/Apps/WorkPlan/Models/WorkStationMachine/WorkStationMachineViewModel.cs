using System;

namespace Psz.Core.Apps.WorkPlan.Models.WorkStationMachine
{
	public class WorkStationMachineViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public string TypeName { get; set; }
		public Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes Type { get; set; }
		public DateTime CreationTime { get; set; }
		public DateTime? LastEditTime { get; set; }
		public string CreationUsername { get; set; }
		public string LastEditUsername { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int WorkAreaId { get; set; }
		public bool IsArchived { get; set; }
		public bool CanSafeDelete { get; set; }
		public string WorkAreaName { get; set; }

		public WorkStationMachineViewModel() { }
		public WorkStationMachineViewModel(Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity workStationMachineDb)
		{
			this.Id = workStationMachineDb.Id;
			this.Name = workStationMachineDb.Name;
			this.HallId = workStationMachineDb.HallId;
			this.HallName = Helpers.User.getHallNameById(workStationMachineDb.HallId);
			this.CreationUsername = Helpers.User.GetUserNameById(workStationMachineDb.CreationUserId);
			this.CreationTime = workStationMachineDb.CreationTime;
			this.LastEditTime = workStationMachineDb.LastEditTime.HasValue ? workStationMachineDb.LastEditTime.Value : (DateTime?)null;
			this.LastEditUsername = workStationMachineDb.LastEditUserId.HasValue ? Helpers.User.GetUserNameById(workStationMachineDb.LastEditUserId.Value) : "";
			this.Type = (Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes)workStationMachineDb.Type;
			this.TypeName = (Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes)workStationMachineDb.Type == Core.Apps.WorkPlan.Enums.WorkStationMachineEnums.WorkStationMachineTypes.Machine ? "Machine" : "WorkStation";
			this.CountryId = workStationMachineDb.CountryId;
			this.WorkAreaId = workStationMachineDb.WorkAreaId;
			this.CountryName = Helpers.User.getCountryNameById(workStationMachineDb.CountryId);
			this.CanSafeDelete = Psz.Core.Apps.WorkPlan.Helpers.DeleteCheck.CanSafeDeleteWorkStationMachine(this.Id);
			this.WorkAreaName = Helpers.User.GetWorkAreaNameById(workStationMachineDb.WorkAreaId);
		}
	}
}
