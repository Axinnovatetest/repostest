namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class SpecialShiftResponseModel
	{
		public int Id { get; set; }
		public string OperationName { get; set; }
		public string DepartmentName { get; set; }
		public string HallName { get; set; }
		public string CountryName { get; set; }
		public string WorkAreaName { get; set; }
		public string WorkStationName { get; set; }
		public DateTime Saturday { get; set; }
		public DateTime Sunday { get; set; }

		public SpecialShiftResponseModel(Infrastructure.Data.Entities.Tables.MTM.CapacityEntity capacityEntity)
		{
			if(capacityEntity == null)
				return;

			Id = capacityEntity.Id;
			OperationName = capacityEntity.OperationName;
			DepartmentName = capacityEntity.DepartementName;
			HallName = capacityEntity.HallName;
			CountryName = capacityEntity.CountryName;
			WorkAreaName = capacityEntity.WorkAreaName;
			WorkStationName = capacityEntity.WorkStationName;
			Saturday = capacityEntity.WeekLastDay.AddDays(-1);
			Sunday = capacityEntity.WeekLastDay;
		}
	}
}
